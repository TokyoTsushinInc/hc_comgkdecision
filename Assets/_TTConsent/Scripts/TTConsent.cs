using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using TT.OptOut;
using TT.Core;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace TT.Consent
{
    [Serializable]
    class ResponseData
    {
        public string country_code;
        public string state_name;
        public string city_name;
        public string postal_code;
        public string message;

        public bool NeedConsent()
        {
            return message.Trim().Length > 0;
        }
    }

    public class TTConsent : MonoBehaviour
    {
        [Header("Change these properties")]
        [SerializeField] private kDeveloper DeveloperType;
        [SerializeField] private string LaunchSceneName;
        
        private TTConsentDialog _mConsentDialog;
        private TTConsentErrorDialog _mErrorDialog;
        
        enum kProgressStatus
        {
            None,
            CheckingNeedConsent, // 圏内で同意が必要か確認中
            DisplayedDialog, // 同意ダイアログ表示済み
            End
        }
        kProgressStatus _mStatus = kProgressStatus.None;

        private void Awake()
        {
            _mConsentDialog = this.GetComponentInChildren<TTConsentDialog>();
            _mConsentDialog.OnConsent += (sender, args) =>
            {
                TTConsentPlayerPref.DidShowConsent();
                TTOptOut.SetNeedOptOut(!args.IsConsent);
                NextScene();
            };
            _mErrorDialog = this.GetComponentInChildren<TTConsentErrorDialog>();
            _mErrorDialog.OnRetry += (sender, args) =>
            {
                _mErrorDialog.Hide();
                Request();
            };
        }

        private void Start()
        {
            // すでに同意画面を表示している場合はチェック不要
            if (TTConsentPlayerPref.AlreadyShowConsent())
            {
                NextScene();
                return;
            }
            Request();
        }

        void NextScene()
        {
            _mStatus = kProgressStatus.End;
            SceneManager.LoadScene(LaunchSceneName);
        }

        void Request()
        {
            _mStatus = kProgressStatus.CheckingNeedConsent;

            string url = "";
            if (TTDebug.IsDebug())
            {
                url = TTConsentDefine.GetApiUrl(DeveloperType, true);
            }
            else
            {
#if UNITY_EDITOR
                url = TTConsentDefine.GetApiUrl(DeveloperType, true);
#else
                url = TTConsentDefine.GetApiUrl(DeveloperType);
#endif        
            }

            StartCoroutine(_Request(
                url,
                text =>
                {
                    try
                    {
                        var response = JsonUtility.FromJson<ResponseData>(text);
                        if (response.NeedConsent())
                        {
                            // iOS14.5以上の場合はATTが表示されているので、ダイアログは出さない
                            if (TTConsentNativePlugin.IsIOSVersionMoreThan_14_5())
                            {
                                TTConsentPlayerPref.DidShowConsent();
                                TTOptOut.SetNeedOptOut(false); // オプトイン状態にしておく（設定画面からオプトアウト可能）
                                NextScene();
                            }
                            else
                            {
                                ShowDialog(response);
                            }
                        }
                        else
                        {
                            Debug.Log("### TTConsent 同意不要");
                            NextScene();
                        }
                    }
                    catch
                    {
                        NextScene();
                    }
                },
                NextScene)
            );
        }

        private IEnumerator _Request(string url, Action<string> cb, Action cbFailed)
        {
            var request = UnityWebRequest.Get(url);
            request.timeout = 5;
            yield return request.SendWebRequest();
      
            // Error
            if (request.error != null)
            {
                // Response Error
                cbFailed();
                yield break;
            }

            cb(request.downloadHandler.text);
        }

        void ShowDialog(ResponseData responseData)
        {
            _mStatus = kProgressStatus.DisplayedDialog;
            _mConsentDialog.Setup(DeveloperType, responseData.message);
            _mConsentDialog.Show();
        }

        public static bool AlreadyShowConsent()
        {
            return TTConsentPlayerPref.AlreadyShowConsent();
        }

        [RuntimeInitializeOnLoadMethod()]
        static void DoOptInOrOut()
        {
            Debug.Log("### TTConsent RuntimeInitializeOnLoadMethod()");
            // まだ同意を得てない場合は何もしない
            if (!TTConsentPlayerPref.AlreadyShowConsent())
            {
                Debug.Log("### TTConsent 同意前なので処理なし");
                if (TTOptOut.NeedOptOut())
                {
                    Debug.Log("ただし、設定画面からオプトアウトしてる可能性もあるので、オプトアウト状態だったらオプトアウトだけさせる");
                    TTOptOut.DoOptOut();
                }
                return;
            }
            Debug.Log("### TTConsent 起動時 DoOptInOrOut");
            TTOptOut.DoOptInOrOut();
        }
    }

    class TTConsentPlayerPref
    {
        public static bool AlreadyShowConsent()
        {
            return PlayerPrefs.GetInt(TTConsentPlayerPrefKeys.IsConsent, 0) == 1;
        }

        public static void DidShowConsent()
        {
            PlayerPrefs.SetInt(TTConsentPlayerPrefKeys.IsConsent, 1);
            PlayerPrefs.Save();
        }
    }

    class TTConsentPlayerPrefKeys
    {
        public static string IsConsent = "ttisconsent";
    }
    
    static class TTConsentNativePlugin
    {
        /// <summary>
        /// iOSのバージョンが14.5以上かどうかを返す
        /// </summary>
        /// <returns>true: 14.5以上(含まれる), false: 14.5未満(含まれない)</returns>
        public static bool IsIOSVersionMoreThan_14_5()
        {
            if (Application.isEditor)
            {
                return true;
            }
#if UNITY_IPHONE
            return _IsVersionMoreThan_14_5_ttconsent();
#else
            return false;
#endif
        }
        
#if UNITY_IPHONE
        #region DllImport
        private const string DLL_NAME = "__Internal";
        [DllImport(DLL_NAME)] private static extern bool _IsVersionMoreThan_14_5_ttconsent();
        #endregion
#endif
    }
}
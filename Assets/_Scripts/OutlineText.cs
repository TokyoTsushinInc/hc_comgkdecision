using UnityEngine;

using System.Collections;

using System.Collections.Generic;

 

[ExecuteInEditMode]

[RequireComponent(typeof(TextMesh))]

public class OutlineText : MonoBehaviour {

 

	public Color outlineColor = Color.black;

	[Range(0.01f, 1.0f)] public float size = 0.1f;

 

	protected TextMesh[] textMeshes;

	protected TextMesh baseTextMesh;

 

	protected static Vector3[] fontPositions =

	{
		/*
		new Vector3(-1, 0, 0),

		new Vector3( 1, 0, 0),

		new Vector3( 0, 1, 0),

		new Vector3( 0,-1, 0),

		new Vector3(-0.75f, -0.75f, 0),

		new Vector3( 0.75f, -0.75f, 0),

		new Vector3( 0.75f,  0.75f, 0),

		new Vector3(-0.75f,  0.75f, 0),
		*/
		
		//z座標を少しだけ手前に持ってくる
        new Vector3(-1, 0, 0.1f),
        new Vector3( 1, 0, 0.1f),
        new Vector3( 0, 1, 0.1f),
        new Vector3( 0,-1, 0.1f),
        
        // 斜め方向を無効にしたい場合は以下をコメントアウトしようズラ！
        new Vector3(-0.75f, -0.75f, 0.1f),
        new Vector3( 0.75f, -0.75f, 0.1f),
        new Vector3( 0.75f,  0.75f, 0.1f),
        new Vector3(-0.75f,  0.75f, 0.1f),


	};

 

	void Start () 

	{

		baseTextMesh = GetComponent<TextMesh>();
		
		
		// 強制的に変更されるのはイヤなのでコメントアウトした
		//baseTextMesh.GetComponent<Renderer>().sortingOrder = 0;

 

		textMeshes = transform.GetComponentsInChildren<TextMesh>();

 

		List<TextMesh> textMeshLit = new List<TextMesh>();

		for(int i=0; i< transform.childCount && i < fontPositions.Length; i++)

		{

			var textMesh = transform.GetChild(i).GetComponent<TextMesh>();

			if( textMesh != null){ textMeshLit.Add(textMesh); }

		}

		textMeshes = textMeshLit.ToArray();

 

 

		if( textMeshes.Length == 0 )

		{

			textMeshes = GenerateTextMeshes(baseTextMesh);

		}

	}

	

	void LateUpdate () 

	{

		for( int i=0; i< fontPositions.Length; i++)

		{

			var textMesh = textMeshes[i];
			
			// 強制的に変更されるのはイヤなのでコメントアウトした
			//textMesh.GetComponent<MeshRenderer>().sortingOrder = 0;

			UpdateFont(baseTextMesh, textMesh);

			textMesh.color = outlineColor;

			textMesh.transform.localPosition = fontPositions[i] * size;

		}

	}

 

	[ContextMenu("Destroy")]

	void Ondestroy()

	{

		for(int i=0; i< textMeshes.Length; i++)

		{

				DestroyImmediate( textMeshes[i].gameObject );

		}

	}

 

	protected static TextMesh[] GenerateTextMeshes(TextMesh baseTextMesh)

	{

 

		List<TextMesh> textMeshList = new List<TextMesh>();

 

		for(int i=0; i< fontPositions.Length; i++)

		{

			var newFont = new GameObject(baseTextMesh.name + " copy font");

			newFont.hideFlags = HideFlags.HideInHierarchy;

			newFont.transform.parent = baseTextMesh.transform;

			newFont.transform.localScale = new Vector3(1,1,1);

 

			var meshRenderer = newFont.AddComponent<MeshRenderer>();

			meshRenderer.castShadows = false;

			meshRenderer.receiveShadows = false;

 

			var textMesh = newFont.AddComponent<TextMesh>();

			textMesh.GetComponent<Renderer>().material = baseTextMesh.GetComponent<Renderer>().sharedMaterial;

			textMesh.font = baseTextMesh.font;

 

			textMeshList.Add(textMesh);

		}

 

		return textMeshList.ToArray();

	}

 

	protected static void UpdateFont(TextMesh baseTextMesh, TextMesh newTextMesh)

	{

		newTextMesh.font = baseTextMesh.font;

		newTextMesh.text = baseTextMesh.text;

		newTextMesh.text = baseTextMesh.text;

		newTextMesh.alignment = baseTextMesh.alignment;

		newTextMesh.anchor = baseTextMesh.anchor;

		newTextMesh.characterSize = baseTextMesh.characterSize;

		newTextMesh.font = baseTextMesh.font;

		newTextMesh.fontSize = baseTextMesh.fontSize;

		newTextMesh.fontStyle = baseTextMesh.fontStyle;

		newTextMesh.richText = baseTextMesh.richText;

		newTextMesh.tabSize = baseTextMesh.tabSize;

		newTextMesh.lineSpacing = baseTextMesh.lineSpacing;

		newTextMesh.offsetZ = baseTextMesh.offsetZ;

	}

}

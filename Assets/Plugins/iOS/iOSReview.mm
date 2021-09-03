//////////////////////////////
// iOSのデフォルトのレビュー依頼をコールする
// 下記のサイトをコピペして実装したぉ
// http://kan-kikuchi.hatenablog.com/entry/RequestReview
//////////////////////////////

#import <StoreKit/StoreKit.h>

extern "C" {
    void _requestReview();
}

void _requestReview(){
	
	[SKStoreReviewController requestReview];
	
}

//////////////////////////////
// iOS�̃f�t�H���g�̃��r���[�˗����R�[������
// ���L�̃T�C�g���R�s�y���Ď���������
// http://kan-kikuchi.hatenablog.com/entry/RequestReview
//////////////////////////////

#import <StoreKit/StoreKit.h>

extern "C" {
    void _requestReview();
}

void _requestReview(){
	
	[SKStoreReviewController requestReview];
	
}

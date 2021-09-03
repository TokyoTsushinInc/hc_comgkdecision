#pragma mark - Unity Bridge
extern "C" {
    bool _IsVersionMoreThan_14_5_ttconsent() {
        if (@available(iOS 14.5, *))
        {
            return true;
        }
        return false;
    }
}


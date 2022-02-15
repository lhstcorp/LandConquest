using CefSharp;
using System;

namespace LandConquest.Logic
{
    public class LifeSpanHandler: ILifeSpanHandler
    {
        public event Action<string> PopupRequest;

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            throw new NotImplementedException();
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            throw new NotImplementedException();
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            throw new NotImplementedException();
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            browser.MainFrame.LoadUrl(targetUrl);
            newBrowser = null;
            return true;

        }
    }
}

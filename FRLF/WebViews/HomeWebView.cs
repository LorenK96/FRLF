using System;
using Android.App;
using Android.Webkit;
using FRLF.Models;
using System.Linq;
using System.Collections.Specialized;
using System.Net;

namespace FRLF
{
    public class HomeWebView : HybridWebViewClient
    {
        private Activity Activity { get; set; }
        public HomeView requestedView;

        public HomeWebView(Activity activity)
        {
            Activity = activity;
        }

        public override void RunServerFunc(WebView webView, string method, NameValueCollection parameters)
        {
            switch (method)
            {
                case "Refresh":
                    Activity.OnBackPressed();
                    break;

                case "CodeEntry":
                    ValidateCode(parameters, webView);
                    break;

                case "ValidateGrid":
                    ValidateGrid(parameters, webView);
                    break;

                case "ValidateInternet":
                    ValidateInternet(webView);
                    break;
            }
        }

        #region Actions
        public void ValidateInternet(WebView webView) {
            if (!InternetAvailable())
                RunJsMethod(webView, "noInternet");
        }

        public void ValidateCode(NameValueCollection parameters, WebView webView)
        {
            var code = parameters["CodeEntry"].ToUpper();
            switch (code)
            {
                case "MGCL2":
                    OpenPPTRemote(webView, 1);
                    requestedView = HomeView.GRID1;
                    break;

                case "BECL2":
                    OpenPPTRemote(webView, 1);
                    requestedView = HomeView.GRID2;
                    break;

                case "MGF2":
                    OpenPPTRemote(webView, 1);
                    requestedView = HomeView.GRID3;
                    break;

                default:
                    RunJsMethod(webView, "invalidCodeEntry");
                    return;
            }
        }

        public void ValidateGrid(NameValueCollection parameters, WebView webView)
        {
            var currentGrid = parameters["grid"];
            string[] selectedOrder = new string[0];
            string[] correctOrder = new string[0];
            switch (currentGrid)
            {
                case "1":
                    selectedOrder = new string[] { parameters["0"], parameters["1"], parameters["2"], parameters["3"], parameters["4"] };
                    correctOrder = new string[] { "btnSun", "btnCloud", "btnLightning", "btn1", "btn0" };
                    break;

                case "2":
                    selectedOrder = new string[] { parameters["0"], parameters["1"], parameters["2"], parameters["3"] };
                    correctOrder = new string[] { "btnId", "btnString", "btnRadioactive", "btn2" };
                    break;

                case "3":
                    selectedOrder = new string[] { parameters["0"], parameters["1"], parameters["2"], parameters["3"], parameters["4"] };
                    correctOrder = new string[] { "btnCash", "btnWallet", "btnPastDue", "btn1", "btn0" };
                    break;
            }

            bool isCorrect = correctOrder.SequenceEqual(selectedOrder);

            RunJsMethod(webView, "parseGridResult", new string[] { isCorrect.ToString().ToLower() });
            if (isCorrect)
                OpenPPTRemote(webView, 2);
        }
        #endregion

        public bool InternetAvailable()
        {
            try
            {
                using (var stream = new WebClient().OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void OpenPPTRemote(WebView view, int pptId)
        {
            // First Powerpoint
            string ppt = "...";
            if (pptId == 2)
            {
                // Second Powerpoint
                ppt = "...";
                requestedView = HomeView.LOGIN;
            }

            string weblink = "http://docs.google.com/gview?embedded=true&url=" + ppt;

            view.LoadUrl(weblink);
        }
    }
}
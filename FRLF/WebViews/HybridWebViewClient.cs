using Android.Webkit;
using System.Collections.Specialized;

namespace FRLF
{
    public abstract class HybridWebViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView webView, string url)
        {
            // If the URL is not our own custom scheme, just let the webView load the URL as usual
            var scheme = "hybrid:";

            if (!url.StartsWith(scheme))
                return false;

            // This handler will treat everything between the protocol and "?"
            // as the method name.  The querystring has all of the parameters.
            var resources = url.Substring(scheme.Length).Split('?');
            var method = resources[0];
            var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]);

            RunServerFunc(webView, method, parameters);

            return true;
        }

        public void RunJsMethod(WebView webView, string method, string[] parameters = null)
        {
            if (parameters == null)
            {
                webView.LoadUrl("javascript:" + method + "();");
                return;
            }

            method += "(";
            for (int i = 0; i < parameters.Length; i++)
            {
                method += "'{" + i + "}'";
                if (parameters.Length - 1 != i)
                    method += ",";
            }
            method += ");";

            var js = string.Format(method, parameters);

            webView.LoadUrl("javascript:" + js);
        }

        public abstract void RunServerFunc(WebView webView, string method, NameValueCollection parameters);
    }
}
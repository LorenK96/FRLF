using Android.App;
using Android.Webkit;
using Android.OS;
using FRLF.Views;
using FRLF.Models;
using Android.Content.PM;
using Android.Views;

namespace FRLF
{
    [Activity(Label = "FRLF", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        private HomeWebView homeView;
        private WebView webView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Main);

            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;

            homeView = new HomeWebView(this);
            webView.SetWebViewClient(homeView);

            var model = new HomeModel() { View = HomeView.LOGIN };
            var template = new RazorView() { Model = model };
            var page = template.GenerateString();
            
            webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
        }

        public override void OnBackPressed()
        {
            var model = new HomeModel() { View = homeView.requestedView };
            var template = new RazorView() { Model = model };
            var page = template.GenerateString();
            
            webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);
        }
    }
}
 
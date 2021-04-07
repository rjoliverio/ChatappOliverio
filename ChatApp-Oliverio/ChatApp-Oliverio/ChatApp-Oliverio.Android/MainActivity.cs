using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Java.Security;
using Plugin.FacebookClient;
using Android.Content;
using Xamarin.Facebook;
using Plugin.GoogleClient;
using Xamarin.Forms;

namespace ChatApp_Oliverio.Droid
{
    
    [Activity(Label = "ChatApp_Oliverio", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            var density = Resources.DisplayMetrics.Density;
            App.screenWidth = Resources.DisplayMetrics.WidthPixels / density;
            App.screenHeight = Resources.DisplayMetrics.HeightPixels / density;

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Phone)
                App.screenHeight = (16 * App.screenWidth) / 9;

            if (Xamarin.Forms.Device.Idiom == TargetIdiom.Tablet)
                App.screenWidth = (9 * App.screenHeight) / 16;

            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    System.Diagnostics.Debug.WriteLine("helloWorld");
                    System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(md.Digest()));
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            PrintHashKey(this);

            FacebookSdk.ApplicationId = "244197793511500";
            FacebookSdk.SdkInitialize(this);
            FacebookClientManager.Initialize(this);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            FirebaseApp.InitializeApp(this);
            base.OnCreate(savedInstanceState);

            GoogleClientManager.Initialize(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
  
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
            FacebookClientManager.OnActivityResult(requestCode, resultCode, intent);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, intent);

        }
        public static void PrintHashKey(Context pContext)
        {
            try
            {
                PackageInfo info = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.Signatures);
                foreach (var signature in info.Signatures)
                {
                    MessageDigest md = MessageDigest.GetInstance("SHA");
                    md.Update(signature.ToByteArray());
                    System.Diagnostics.Debug.WriteLine("HelloWorld");
                    System.Diagnostics.Debug.WriteLine(BitConverter.ToString(md.Digest()).Replace("-", ":"));
                }
            }
            catch (NoSuchAlgorithmException e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
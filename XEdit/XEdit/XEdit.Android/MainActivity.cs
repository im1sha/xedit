using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.IO;
using Android.Content;
using Xamarin.Forms;

namespace XEdit.Droid
{
    [Activity(Label = "XEdit", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation//,
        //ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait
        )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Instance = this;

            base.OnCreate(savedInstanceState);
            Forms.SetFlags("CollectionView_Experimental");

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        // Field, property, and method for Picture Picker
        public const int PICK_IMAGE_CODE = 1000;

        public const int WRITE_PERMISSION_REQUEST_CODE = 1001;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case WRITE_PERMISSION_REQUEST_CODE:
                {
                    if ((grantResults.Length > 0) && (grantResults[0] == Permission.Granted))
                    {             
                            
                    }
                    break;
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PICK_IMAGE_CODE)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Java.IO;
using Xamarin.Forms;
using XEdit.PlatformSpecific;
using XEdit.Droid;

[assembly: Dependency(typeof(Utils))]
namespace XEdit.Droid
{
    class Utils : IUtils
    {
        public bool AskForWriteStoragePermission()
        {
            //check permisson and require then if it's not granted 
            if (ContextCompat.CheckSelfPermission(Android.App.Application.Context,
                 Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance,
                    new string[] { Manifest.Permission.WriteExternalStorage },
                    MainActivity.WRITE_PERMISSION_REQUEST_CODE);

                return false; // MAY BE GRANTED BY USER NOW
            }

            return true; // GRANTED
        }   
    }
}
using System.Threading.Tasks;
using System.Linq;
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
using System.Collections.Generic;

[assembly: Dependency(typeof(Utils))]
namespace XEdit.Droid
{
    class Utils : IUtils
    {
        public bool AskForWriteStoragePermission()
        {
            return  AskForPermisson(new[] { PermissonsTypes.Write }, MainActivity.WRITE_PERMISSION_REQUEST_CODE);
        }

        public bool AskForReadStoragePermission()
        {
            return  AskForPermisson(new[] { PermissonsTypes.Read }, MainActivity.READ_PERMISSION_REQUEST_CODE);
        }

        public bool AskForCameraPermissons()
        {
            return  AskForPermisson(new[] { PermissonsTypes.Camera, PermissonsTypes.Read, PermissonsTypes.Write }, 
                MainActivity.MULTIPLE_CAMERA_PERMISSIONS_REQUEST_CODE);
        }

        private enum PermissonsTypes { Camera, Read, Write }

        private bool AskForPermisson(PermissonsTypes[] permissons, int requestCode)
        {
            List<string> permissionsStrings = new List<string>();

            if (permissons.Contains(PermissonsTypes.Read))
            {
                permissionsStrings.Add(Manifest.Permission.ReadExternalStorage);
            }
            else if (permissons.Contains(PermissonsTypes.Write))
            {
                permissionsStrings.Add(Manifest.Permission.WriteExternalStorage);
            }
            else /*if (permisson == PermissonsTypes.Camera)*/
            {
                permissionsStrings.Add(Manifest.Permission.Camera);
            }

            //check permisson and require then if it's not granted 
            string[] requiredPermissons = permissionsStrings.Where(item => 
                ContextCompat.CheckSelfPermission(Android.App.Application.Context, item) != 
                    (int)Permission.Granted).ToArray();

            if (requiredPermissons.Length > 0)
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance,
                    requiredPermissons,
                    requestCode);

                return false; // MAY BE GRANTED BY USER NOW
            }

            return true; // GRANTED
        }
    }
}
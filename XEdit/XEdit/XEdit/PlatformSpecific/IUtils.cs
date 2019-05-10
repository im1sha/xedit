using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XEdit.PlatformSpecific
{
    public interface IUtils
    {
         bool AskForWriteStoragePermission();
         bool AskForReadStoragePermission();
         bool AskForCameraPermissons();
    }
}

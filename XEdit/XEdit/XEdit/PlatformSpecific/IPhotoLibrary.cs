using System;
using System.IO;
using System.Threading.Tasks;

namespace XEdit.PlatformSpecific
{
    public interface IPhotoLibrary
    {
        Task<Stream> PickPhotoAsync();
        Task<bool> SavePhotoAsync(byte[] data, string folder, string filename);
    }
}


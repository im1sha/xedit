using System;
using System.IO;
using System.Threading.Tasks;

namespace XEdit.PicturePicker
{
    public interface IPicturePicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}

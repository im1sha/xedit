using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.PlatformSpecific;
using XEdit.TouchTracking;

namespace XEdit
{
    public class ImageWorker
    {
        private static readonly Lazy<ImageWorker> _instance = new Lazy<ImageWorker>(() => new ImageWorker());
        public static ImageWorker Instance { get => _instance.Value; }
        private ImageWorker() { }

        private List<SKBitmap> _backupStorage = new List<SKBitmap>();
        private static readonly int maxBackupStorageSize = 5;
        public SKBitmap _image = new SKBitmap();

        #region backup

        public Task CreateBackupImage()
        {
             return new Task(() => {
                 if (_backupStorage.Count >= maxBackupStorageSize)
                 {
                     MoveToTrash(_backupStorage[0]);
                     _backupStorage.RemoveAt(0);
                 }
                 _backupStorage.Add(CloneImage(_image));
             });
        }

        public Task RestoreImage()
        {
            return new Task(() => {
                MoveToTrash(_image);
                _image = CloneImage(_backupStorage[_backupStorage.Count - 1]);
                MoveToTrash(_backupStorage[_backupStorage.Count - 1]);
                _backupStorage.RemoveAt(_backupStorage.Count - 1);
            });
        }

        #endregion 

        public SKBitmap Image
        {
            get
            {
                lock (_image)
                {
                    return _image;
                }
            }
            set
            {
                lock (_image)
                {
                    MoveToTrash(_image);
                    _image = value;
                }
            }
        }
 
        public SKBitmap CloneImage(SKBitmap target)
        {
            if (target == null)
            {
                return null;
            }
            SKBitmap result = new SKBitmap(target.Info);
            lock (target)
            {
                target.CopyTo(result);
            }
            return result;
        }

        public async Task<bool> SaveImage()
        {
            bool success = false;
            SKBitmap bitmap = CloneImage(Image);

            if (bitmap == null)
            {
                return false;
            }

            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg;
            int quality = 100;

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                bitmap.Encode(wstream, imageFormat, quality);
                byte[] data = memStream.ToArray();
                if (data != null && data.Length != 0)
                {
                    bool isGranted = DependencyService.Get<IUtils>().AskForWriteStoragePermission();
                    if (isGranted)
                    {
                        success = await DependencyService.Get<IPhotoLibrary>().
                            SavePhotoAsync(data, "testFolder", DateTime.Now.ToBinary().ToString() + ".jpeg");
                    }
                }
            }

            MoveToTrash(bitmap);

            return success;
        }

        /// <summary>
        /// Assign null to target and forces garbage collection
        /// </summary>
        /// <param name="target">image to collect</param>
        private void MoveToTrash(SKBitmap target)
        {
            target = null;
            GC.Collect();
        }
    }
}



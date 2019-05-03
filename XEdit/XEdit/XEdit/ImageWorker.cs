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

        /// <summary>
        /// Image created when you select new visual handler
        /// </summary>   
        private SKBitmap _backupImage = new SKBitmap();

        /// <summary>
        /// Image states when working with image 
        /// </summary>
        private List<SKBitmap> _stateStorage = new List<SKBitmap>();
        private static readonly int maxStateStorageSize = 5;
        private SKBitmap _image = new SKBitmap();

        #region backup methods

        //public void AddBackupImage()
        //{
        //    _backupImage = CloneImage(_image);
        //}

        //public void RestoreFromBackupImage()
        //{
        //    MoveToTrash(_image);
        //    _image = CloneImage(_backupImage);
        //}

        public void AddImageState(SKBitmap img = null)
        {
            SKBitmap target;
            if (img == null)
            {
                target = _image;
            }
            else
            {
                target = img;
            }

            if (_stateStorage.Count >= maxStateStorageSize)
            {
                MoveToTrash(_stateStorage[0]);
                _stateStorage.RemoveAt(0);
            }

            _stateStorage.Add(CloneImage(target));
        }

        public void RestorePreviousImageState()
        {
            MoveToTrash(_image);
            if (_stateStorage.Count > 0)
            {
                _image = _stateStorage[_stateStorage.Count - 1];
                _stateStorage.RemoveAt(_stateStorage.Count - 1);
            }          
        }

        /// <summary>
        /// Clean up backup list
        /// </summary>
        public void CommitImage()
        {
            bool shouldCollect = false;
            lock (_stateStorage)
            {
                shouldCollect = _stateStorage.Count > 0;
                if (shouldCollect)
                { 
                    for (int i = 0; i < _stateStorage.Count ; i++)
                    {
                        _stateStorage[i] = null;
                    }
                    _stateStorage.RemoveRange(0, _stateStorage.Count );
                }
            }
            if (shouldCollect)
            {
                GC.Collect();
            }
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

        /// <summary>
        /// Clones SKBitmap using lock
        /// </summary>
        /// <param name="target">Image to copy</param>
        /// <returns>Cloned image</returns>
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

        public async Task<string> SaveImage()
        {
            bool success = false;

            string fileName = $"X{DateTime.Now.ToBinary().ToString()}.jpeg";

            SKBitmap bitmap = CloneImage(Image);

            if (bitmap == null)
            {
                return null;
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
                            SavePhotoAsync(data, "XEdit", fileName);
                    }
                }
            }

            MoveToTrash(bitmap);
            if (!success)
            {
                return null;
            }
            else
            {
                return fileName;
            }
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



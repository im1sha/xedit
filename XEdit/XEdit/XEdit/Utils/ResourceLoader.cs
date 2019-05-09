using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace XEdit.Utils
{
    public class ResourceLoader
    {
        public enum ImageFolder { Glass }

        private readonly Dictionary<ImageFolder, (string name, int totalImages)> ImageFolderDictionary;

        public int GetAmountOfImages(ImageFolder folder)
        {
            return ImageFolderDictionary.ContainsKey(folder) 
                ? ImageFolderDictionary[folder].totalImages 
                : 0;
        }

        public ResourceLoader()
        {
            ImageFolderDictionary = new Dictionary<ImageFolder, (string, int)>
            {
                { ImageFolder.Glass, ("Glass", 5) },
            };
        }

        public SKBitmap LoadSKBitmap(ImageFolder folder, int imageId)
        {
            if (!ImageFolderDictionary.ContainsKey(folder) || imageId >= ImageFolderDictionary[folder].totalImages || imageId < 0)
            {
                return null;
            }

            SKBitmap resourceBitmap = null;

            string resourceID = $"XEdit.Media.{ImageFolderDictionary[folder].name}.{imageId}.jpg";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }

            return resourceBitmap;
        }
    }
}

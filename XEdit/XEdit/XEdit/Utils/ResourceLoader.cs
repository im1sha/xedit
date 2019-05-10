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
        public enum ImageFolder { Glass, Image }

        private readonly Dictionary<ImageFolder, (string name, int totalImages, string extension)> ImageFolderDictionary;

        public int GetAmountOfImages(ImageFolder folder)
        {
            return ImageFolderDictionary.ContainsKey(folder) 
                ? ImageFolderDictionary[folder].totalImages 
                : 0;
        }

        public ResourceLoader()
        {
            ImageFolderDictionary = new Dictionary<ImageFolder, (string, int, string)>
            {
                { ImageFolder.Glass, ("Glass", 5, "jpg") },
                { ImageFolder.Image, ("Image", 3, "png") },
            };
        }

        public SKBitmap LoadSKBitmap(ImageFolder folder, int imageId)
        {
            if (!ImageFolderDictionary.ContainsKey(folder) || imageId >= ImageFolderDictionary[folder].totalImages || imageId < 0)
            {
                return null;
            }

            SKBitmap resourceBitmap = null;

            string resourceID = $"XEdit.Media.{ImageFolderDictionary[folder].name}." +
                $"{imageId}.{ImageFolderDictionary[folder].extension}";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }

            return resourceBitmap;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    class ImageManager
    {
        static readonly Lazy<ImageManager> instance = new Lazy<ImageManager>(() => new ImageManager());

        public static ImageManager Instance { get => instance.Value; }

        protected ImageManager() { }
    }
}

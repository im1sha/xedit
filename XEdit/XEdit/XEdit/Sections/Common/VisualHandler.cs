using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Sections
{
    public class VisualHandler
    {
        public string Name { get; }

        public string ImageUrl { get; }
    
        /// <summary>
        /// Should be called when Handler is selected to set view or to do calculations
        /// </summary>
        public Action Perform { get; }

        public Action Close { get; }

        public VisualHandler(string name, string url, Action perform, Action close)
        {
            Name = name;
            ImageUrl = url;
            Close = close;
            Perform = perform;
        }
    }
}



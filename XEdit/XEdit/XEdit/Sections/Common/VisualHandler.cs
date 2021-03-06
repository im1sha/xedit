﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Sections
{
    public class VisualHandler
    {
        public string Name { get; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// Should be called when Handler is selected to set view or to do calculations
        /// </summary>
        public Action Perform { get; }

        /// <summary>
        /// Closes without cancellation of uncommited changes(true) or with one(false)
        /// </summary>
        public Action<bool> Close { get; }

        public VisualHandler(string name, string url, Action perform, Action<bool> close)
        {           
            ImageUrl = url;           
            Name = name;
            Close = close;
            Perform = perform;
        }
    }
}



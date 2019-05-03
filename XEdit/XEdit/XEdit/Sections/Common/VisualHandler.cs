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
        /// Gets previous image (mb restores previous screen state)
        /// </summary>
        //public Action Rollback { get; }

        /// <summary>
        /// Verifies current image
        /// </summary>
        //public Action Commit { get; }

        /// <summary>
        /// Should be called when Handler is deactivated (pressed 'X' button / another handler selected)
        /// </summary>
        //public Action Exit { get; }

        /// <summary>
        /// Should be called when Handler is selected to set view or to do calculations
        /// </summary>
        public Action Perform { get; }

        public VisualHandler(string name, string url, Action performAction)
        {
            Name = name;
            ImageUrl = url;
            Perform = performAction;
        }
    }
}



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
        /// Action on cancellation 
        /// </summary>
        public Action Rollback { get; }

        /// <summary>
        /// Should be called when Handler is selected to set view or to do calculations
        /// </summary>
        public Action Perform { get; }

        /// <summary>
        /// Should be called when Handler is deactivated 
        /// </summary>
        public Action Exit { get; }


        public VisualHandler(string name, string url, Action performAction,    
            Action rollbackAction, Action exitAction)
        {
            Name = name;
            ImageUrl = url;
            Perform = performAction;
            Rollback = rollbackAction;
            Exit = exitAction;
        }
    }
}



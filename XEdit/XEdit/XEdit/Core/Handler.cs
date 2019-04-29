using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    public class Handler
    {
        public string Name { get; }
        public string ImageUrl { get; }

        /// <summary>
        /// Should be called when Handler is selected
        /// </summary>
        public Action<object> Perform { get; }

        /// <summary>
        /// Should be called when Handler is deactivated 
        /// </summary>
        public Action<object> Exit { get; }

        public Handler(string name, string url, Action<object> performAction, Action<object> exitAction)
        {
            Name = name;
            ImageUrl = url;
            Perform = performAction;
            Exit = exitAction;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    public class Handler
    {
        public string Name { get; }
        public string ImageUrl { get; }
        public Action<object> Start { get; }
        public Action<object> End { get; }

        public Handler(string name, string url, Action<object> startAction, Action<object> endAction)
        {
            Name = name;
            ImageUrl = url;
            Start = startAction;
            End = endAction;
        }
    }
}

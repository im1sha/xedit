using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Interaction
{
    public interface IHandler
    { 
        string Name { get; }
        string ImageUrl { get; }
        Action<object> GetAction(object sender, EventArgs args);
    }
}

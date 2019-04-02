using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Models
{
    public interface IFilter
    { 
        string Name { get; }
        string ImageUrl { get; }
        Action<object> Apply { get; }
    }
}

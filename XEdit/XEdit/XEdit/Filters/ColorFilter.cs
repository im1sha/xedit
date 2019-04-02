using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class ColorFilter : Models.IFilter
    {
        public string Name => "Color"; 

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> Apply => throw new NotImplementedException();
    }
}

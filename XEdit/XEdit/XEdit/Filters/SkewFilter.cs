using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class SkewFilter : Models.IFilter
    {
        public string Name => "Skew";

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> Apply => throw new NotImplementedException();
    }
}

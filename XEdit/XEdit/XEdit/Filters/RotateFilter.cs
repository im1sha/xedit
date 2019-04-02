using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class RotateFilter : Models.IFilter
    {
        public string Name => "Rotate";

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> Apply => throw new NotImplementedException();
    }
}

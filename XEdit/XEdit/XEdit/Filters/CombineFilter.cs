using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class CombineFilter : Models.IFilter
    {
        public string Name => "Combine";

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> Apply => throw new NotImplementedException();
    }
}

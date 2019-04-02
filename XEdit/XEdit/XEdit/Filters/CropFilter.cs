using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class CropFilter : Models.IFilter
    {
        public string Name => "Crop";

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> Apply => throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class SkewFilter : Interaction.IHandler
    {
        public string Name => "Skew";

        public string ImageUrl => throw new NotImplementedException();

        public Action GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

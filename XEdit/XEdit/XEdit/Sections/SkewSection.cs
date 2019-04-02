using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Sections
{
    class SkewSection : Interaction.IHandler
    {
        public string Name => "Skew";

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

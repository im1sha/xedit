using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class ColorFilter : Interaction.IHandler
    {
        public string Name => "Color"; 

        public string ImageUrl => throw new NotImplementedException();

        public Action GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

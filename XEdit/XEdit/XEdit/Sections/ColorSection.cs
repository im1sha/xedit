using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Sections
{
    class ColorSection : Interaction.IHandler
    {
        public string Name => "Color"; 

        public string ImageUrl => throw new NotImplementedException();

        public Action<object> GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

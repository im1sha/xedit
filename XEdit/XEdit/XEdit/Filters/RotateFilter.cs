using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class RotateFilter : Interaction.IHandler
    {
        public string Name => "Rotate";

        public string ImageUrl => throw new NotImplementedException();

        public Action GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Filters
{
    class CombineFilter : Interaction.IHandler
    {
        public string Name => "Combine";

        public string ImageUrl => throw new NotImplementedException();

        public Action GetAction(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

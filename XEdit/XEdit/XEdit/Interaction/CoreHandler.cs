using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Interaction
{
    public class CoreHandler : IHandler
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string ImageUrl => throw new NotImplementedException();

        public virtual Action<object> GetAction(object target, EventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}

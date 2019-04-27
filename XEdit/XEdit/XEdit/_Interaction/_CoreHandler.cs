using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Interaction
{
    public class _CoreHandler : _IHandler
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string ImageUrl => throw new NotImplementedException();

        public virtual Action<object> SelectAction(object target, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual Action<object> CancelAction(object target, EventArgs args)
        {
            return (obj) => { };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using XEdit.TouchTracking;

namespace XEdit.ViewModels
{
    class TouchWorker
    {
        private TouchEffect _touchEffect;

        private EventHandler<TouchActionEventArgs> _previousUpdateHandler =
            _standardUpdateHandler;

        private readonly static EventHandler<TouchActionEventArgs> _standardUpdateHandler =
            (sender, args) => { };

        public TouchWorker(TouchEffect effect)
        {
            _touchEffect = effect;
            _touchEffect.Capture = false;
            _touchEffect.TouchAction += _standardUpdateHandler;
        }
   
        public void SetUpdateHandler(EventHandler<TouchActionEventArgs> eh = null)
        {
            if (eh == null)
            {
                _touchEffect.Capture = false;
                eh = _standardUpdateHandler;
            }
            else
            {
                _touchEffect.Capture = true;
            }

            _touchEffect.TouchAction -= _previousUpdateHandler;
            _previousUpdateHandler = eh;
            _touchEffect.TouchAction += eh;
        }     
    }
}



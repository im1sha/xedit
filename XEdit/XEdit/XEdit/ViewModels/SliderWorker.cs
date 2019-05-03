using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XEdit.ViewModels
{
    public class SliderWorker
    {
        private Slider _slider;

        private EventHandler<ValueChangedEventArgs> _previousUpdateHandler = 
            _standardUpdateHandler;

        private readonly static EventHandler<ValueChangedEventArgs> _standardUpdateHandler =
            (sender, args) => { };

        private EventHandler _previousDragCompletedHandler =
            _standardDragCompletedHandler;

        private readonly static EventHandler _standardDragCompletedHandler =
            (sender, args) => { };

        public double SliderValue
        {
            get
            {
                lock (_slider)
                {
                    return _slider.Value;
                }
            }
            set
            {
                lock (_slider)
                { 
                    _slider.Value = value;
                }
            }
        }

        public void SetDefaultSliderValue() { SliderValue = 0; }

        public SliderWorker(Slider s)
        {
            _slider = s;
            _slider.ValueChanged += _standardUpdateHandler;
            _slider.DragCompleted += _standardDragCompletedHandler;
        }

        public void SetUpdateHandler(EventHandler<ValueChangedEventArgs> eh = null)
        {
            if (eh == null)
            {
                eh = _standardUpdateHandler;
            }
            _slider.ValueChanged -= _previousUpdateHandler;
            _previousUpdateHandler = eh;
            _slider.ValueChanged += eh;
        }

        public void SetDragCompletedHandler(EventHandler eh = null)
        {
            if (eh == null)
            {
                eh = _standardDragCompletedHandler;
            }
            _slider.DragCompleted -= _previousDragCompletedHandler;
            _previousDragCompletedHandler = eh;
            _slider.DragCompleted += eh;
        }
    }
}



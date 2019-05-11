using System;
using Xamarin.Forms;

namespace XEdit.TouchTracking
{
    public class TouchEffect : RoutingEffect
    {
        public event EventHandler<TouchActionEventArgs> TouchAction;

        public TouchEffect() : base("im1sha.TouchEffect")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(Element element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }

    }
}

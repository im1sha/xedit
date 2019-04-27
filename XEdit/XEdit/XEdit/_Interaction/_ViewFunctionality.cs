using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Interaction
{
    static class _ViewFunctionality
    {
        public static bool IsImageLoaded { get { return ResourceBitmap != null; } }

        public static SKBitmap ResourceBitmap { get; private set; }

        public static void SetBitmap(SKBitmap b) {
            if (b != null)
            {
                ResourceBitmap = b;
            }    
        }

        public static bool ContainsSkCanvasView(object target)
        {
            return IsTargetSuitable(target)
                && ((target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Count > 0)
                && ((target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children[0].GetType() == typeof(SKCanvasView));
        }

        public static bool IsTargetSuitable(object target)
        {
            return target is Xamarin.Forms.Layout<Xamarin.Forms.View>;
        }

        public static bool AddNewCanvaAsChild(object target, SKCanvasView canvasView) {
            if (IsTargetSuitable(target))
            {
                Xamarin.Forms.Layout<Xamarin.Forms.View> view = (target as Xamarin.Forms.Layout<Xamarin.Forms.View>);

                for (int i = 0; i < view.Children.Count; i++)
                {
                    view.Children.RemoveAt(i);
                }
                view.BackgroundColor = Color.Black;

                view.Children.Add(canvasView);
                return true;
            }
            return false;
        }
    }
}

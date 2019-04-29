using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Views
{
    public class _BindableSKCanvasView : SKCanvasView
    {
        public static readonly BindableProperty ColorProperty =
                BindableProperty.Create("Color",
                    typeof(SKColor),
                    typeof(_BindableSKCanvasView),
                    defaultValue: SKColors.Black,
                    defaultBindingMode: BindingMode.TwoWay,
                    propertyChanged: RedrawCanvas);

        public SKColor Color
        {
            get => (SKColor)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        private static void RedrawCanvas(BindableObject bindable, object oldvalue, object newvalue)
        {
            _BindableSKCanvasView bindableCanvas = bindable as _BindableSKCanvasView;
            bindableCanvas.InvalidateSurface();
        }
    }
}

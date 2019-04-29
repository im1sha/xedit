using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XEdit.Core;

namespace XEdit.Sections
{
    class Features : Core.CoreSection
    {
        //public static new bool VariableValues => true; // slider required

        //public override string Name { get; } = "Features";

        //public override Handler SelectedHandler
        //{
        //    get
        //    {
        //        return _selectedHandler;
        //    }
        //    set
        //    {
        //        if (_selectedHandler != value)
        //        {
        //            _selectedHandler?.Exit(null);
        //            _selectedHandler = value;
        //            OnPropertyChanged();
        //            _selectedHandler.Perform(null);
        //            _selectedHandler = null;
        //        }
        //    }
        //}

        //public Features()
        //{
        //    Handlers = new ObservableCollection<Handler>()
        //    {
        //        new Handler("Vertical",
        //            null,
        //            (obj) => { OnVerticalFlip(); return null; },
        //            (obj) => null),
        //        new Handler("Horizontal",
        //            null,
        //            (obj) => { OnHorizontalFlip(); return null; },
        //            (obj) => null),
        //    };
        //}


        //void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        //{
        //    canvasView.InvalidateSurface();
        //}

        //void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        //{
        //    SKImageInfo info = args.Info;
        //    SKSurface surface = args.Surface;
        //    SKCanvas canvas = surface.Canvas;

        //    canvas.Clear();

        //    // Find rectangle to fit bitmap
        //    float scale = Math.Min((float)info.Width / bitmap1.Width,
        //                           (float)info.Height / bitmap1.Height);

        //    SKRect rect = SKRect.Create(scale * bitmap1.Width,
        //                                scale * bitmap1.Height);

        //    float x = (info.Width - rect.Width) / 2;
        //    float y = (info.Height - rect.Height) / 2;

        //    rect.Offset(x, y);

        //    // Get progress value from Slider
        //    float progress = (float)progressSlider.Value;

        //    // Display two bitmaps with transparency
        //    using (SKPaint paint = new SKPaint())
        //    {
        //        paint.Color = paint.Color.WithAlpha((byte)(0xFF * (1 - progress)));
        //        canvas.DrawBitmap(bitmap1, rect, paint);

        //        paint.Color = paint.Color.WithAlpha((byte)(0xFF * progress));
        //        canvas.DrawBitmap(bitmap2, rect, paint);
        //    }
        //}

    }
}

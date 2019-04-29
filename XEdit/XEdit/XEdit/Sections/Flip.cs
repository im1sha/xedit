using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using XEdit.Extensions;
using XEdit.Core;

namespace XEdit.Sections
{
    public class Flip : CoreSection
    {


        //void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        //{
        //    SKImageInfo info = args.Info;
        //    SKSurface surface = args.Surface;
        //    SKCanvas canvas = surface.Canvas;

        //    canvas.Clear();

        //    canvas.DrawBitmap(bitmap, info.Rect, BitmapStretch.Uniform);
        //}

        //void OnFlipVerticalClicked(object sender, EventArgs args)
        //{
        //    SKBitmap flippedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);

        //    using (SKCanvas canvas = new SKCanvas(flippedBitmap))
        //    {
        //        canvas.Clear();
        //        canvas.Scale(-1, 1, bitmap.Width / 2, 0);
        //        canvas.DrawBitmap(bitmap, new SKPoint());
        //    }

        //    bitmap = flippedBitmap;
        //    canvasView.InvalidateSurface();
        //}

        //void OnFlipHorizontalClicked(object sender, EventArgs args)
        //{
        //    SKBitmap flippedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);

        //    using (SKCanvas canvas = new SKCanvas(flippedBitmap))
        //    {
        //        canvas.Clear();
        //        canvas.Scale(1, -1, 0, bitmap.Height / 2);
        //        canvas.DrawBitmap(bitmap, new SKPoint());
        //    }

        //    bitmap = flippedBitmap;
        //    canvasView.InvalidateSurface();
        //}
    }
}

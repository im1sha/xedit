using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Sections
{
    public static class SizeCalculator
    {
        public static (float scale, SKRect rect) GetScaleAndRect(SKSize canvasViewSize, SKBitmap bitmap) {
            SKRect rect;
            float scale; // determines image orientation
            if (canvasViewSize.Height / canvasViewSize.Width > bitmap.Height / bitmap.Width)
            {
                scale = bitmap.Width / (float)canvasViewSize.Width;
                rect = new SKRect(0,
                    (float)(canvasViewSize.Height - bitmap.Height / scale) / 2,
                    (float)canvasViewSize.Width,
                    (float)(canvasViewSize.Height + bitmap.Height / scale) / 2
                    );
            }
            else
            {
                scale = bitmap.Height / (float)canvasViewSize.Height;
                rect = new SKRect(
                    (float)(canvasViewSize.Width - bitmap.Width / scale) / 2,
                    0,
                    (float)(canvasViewSize.Width + bitmap.Width / scale) / 2,
                    (float)canvasViewSize.Height
                    );
            }

            return (scale, rect);
        }

    }
}

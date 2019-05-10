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
            if ((canvasViewSize.Height / canvasViewSize.Width) > (1.0 * bitmap.Height / bitmap.Width))
            {
                scale = bitmap.Width / canvasViewSize.Width;
                rect = new SKRect(0,
                    (canvasViewSize.Height - bitmap.Height / scale) / 2,
                    canvasViewSize.Width,
                    (canvasViewSize.Height + bitmap.Height / scale) / 2
                    );
            }
            else
            {
                scale = bitmap.Height / canvasViewSize.Height;
                rect = new SKRect(
                    (canvasViewSize.Width - bitmap.Width / scale) / 2,
                    0,
                    (canvasViewSize.Width + bitmap.Width / scale) / 2,
                    canvasViewSize.Height
                    );
            }

            return (scale, rect);
        }

      
    }
}

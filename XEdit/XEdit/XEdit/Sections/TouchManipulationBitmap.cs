using System;
using System.Collections.Generic;
using SkiaSharp;
using XEdit.TouchTracking;

namespace XEdit.Sections
{
    class TouchManipulationBitmap
    {
        private SKBitmap _bitmap;

        private Dictionary<long, TouchManipulationInfo> _touchDictionary =
            new Dictionary<long, TouchManipulationInfo>();

        public TouchManipulationBitmap(SKBitmap bitmap)
        {
            _bitmap = bitmap;
            Matrix = SKMatrix.MakeIdentity();

            TouchManager = new TouchManipulationManager
            {
                Mode = TouchManipulationMode.ScaleRotate
            };
        }

        public TouchManipulationManager TouchManager { set; get; } 

        public SKMatrix Matrix { set; get; }

        public void Paint(SKCanvas canvas)
        {
            canvas.Save();
            SKMatrix matrix = Matrix;
            canvas.Concat(ref matrix);
            canvas.DrawBitmap(_bitmap, 0, 0);
            canvas.Restore();
        }

        public bool HitTest(SKPoint location)
        {
            // Invert the matrix
            SKMatrix inverseMatrix;

            if (Matrix.TryInvert(out inverseMatrix))
            {
                // Transform the point using the inverted matrix
                SKPoint transformedPoint = inverseMatrix.MapPoint(location);

                // Check if it's in the untransformed bitmap rectangle
                SKRect rect = new SKRect(0, 0, _bitmap.Width, _bitmap.Height);
                return rect.Contains(transformedPoint);
            }
            return false;
        }

        public void ProcessTouchEvent(long id, TouchActionType type, SKPoint location)
        {
            switch (type)
            {
                case TouchActionType.Pressed:
                    _touchDictionary.Add(id, new TouchManipulationInfo
                    {
                        PreviousPoint = location,
                        NewPoint = location
                    });
                    break;

                case TouchActionType.Moved:
                    TouchManipulationInfo info = _touchDictionary[id];
                    info.NewPoint = location;
                    Manipulate();
                    info.PreviousPoint = info.NewPoint;
                    break;

                case TouchActionType.Released:
                    _touchDictionary[id].NewPoint = location;
                    Manipulate();
                    _touchDictionary.Remove(id);
                    break;

                case TouchActionType.Cancelled:
                    _touchDictionary.Remove(id);
                    break;
            }
        }

        void Manipulate()
        {
            TouchManipulationInfo[] infos = new TouchManipulationInfo[_touchDictionary.Count];
            _touchDictionary.Values.CopyTo(infos, 0);
            SKMatrix touchMatrix = SKMatrix.MakeIdentity();

            if (infos.Length == 1)
            {
                SKPoint prevPoint = infos[0].PreviousPoint;
                SKPoint newPoint = infos[0].NewPoint;
                SKPoint pivotPoint = Matrix.MapPoint(_bitmap.Width / 2, _bitmap.Height / 2);

                touchMatrix = TouchManager.OneFingerManipulate(prevPoint, newPoint, pivotPoint);
            }
            else if (infos.Length >= 2)
            {
                int pivotIndex = infos[0].NewPoint == infos[0].PreviousPoint ? 0 : 1;
                SKPoint pivotPoint = infos[pivotIndex].NewPoint;
                SKPoint newPoint = infos[1 - pivotIndex].NewPoint;
                SKPoint prevPoint = infos[1 - pivotIndex].PreviousPoint;

                touchMatrix = TouchManager.TwoFingerManipulate(prevPoint, newPoint, pivotPoint);
            }

            SKMatrix matrix = Matrix;
            SKMatrix.PostConcat(ref matrix, touchMatrix);
            Matrix = matrix;
        }
    }
}

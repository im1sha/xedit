using System;

using SkiaSharp;

namespace XEdit.Sections
{
    class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }
    }
}

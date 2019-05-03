using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class Transparency : BaseSection
    {
        readonly MainViewModel _mainVM;

        public override bool IsVariableValues() => true;

        public override string Name { get; } = "Transparency";

        private SKBitmap _localImageCopy;

        public Transparency(MainViewModel vm)
        {
            _mainVM = vm;

            Handlers = new ObservableCollection<VisualHandler>()
            {
                new VisualHandler("Self",
                    null,
                    () => {
                        _localImageCopy = _mainVM.ImageWorker.CloneImage(_mainVM.ImageWorker.Image);
                        _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
                        _mainVM.SliderWorker.SetUpdateHandler(OnSliderValueChanged);
                        _mainVM.SliderWorker.SetDragCompletedHandler(OnDragCompleted);
                    },
                    close: () => {
                        _mainVM.CanvasViewWorker.SetUpdateHandler();
                        _mainVM.SliderWorker.SetUpdateHandler();
                        _mainVM.SliderWorker.SetDragCompletedHandler();
                    }),
            };
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            _mainVM.CanvasViewWorker.Invalidate();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            using (canvas)
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                double progress = _mainVM.SliderWorker.SliderValue;
                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (1 - progress)));
                canvas.DrawBitmap(_localImageCopy, info.Rect, BitmapStretch.Uniform, paint:paint);
            }
        }

        private void OnDragCompleted(object sender, EventArgs args)
        {
            _mainVM.ImageWorker.AddImageState();

            SKBitmap bitmap = _mainVM.ImageWorker.Image;
            SKBitmap newBitmap = new SKBitmap(bitmap.Info);
            using (SKCanvas canvas = new SKCanvas(newBitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                double progress = _mainVM.SliderWorker.SliderValue;
                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (1 - progress)));
                canvas.DrawBitmap(_mainVM.ImageWorker.Image, new SKPoint(), paint: paint);
            }
            _mainVM.ImageWorker.Image = newBitmap;
            _mainVM.CanvasViewWorker.Invalidate();
        }
    }
}



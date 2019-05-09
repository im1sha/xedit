using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.Utils;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class Transparency : BaseSection
    {
        readonly MainViewModel _mainVM;
        readonly ResourceLoader _resourceLoader;

        public override bool IsVariableValues() => true;

        public override string Name { get; } = "Transparency";

        private SKBitmap _localImageCopy;
        private SKBitmap _filterImage;
        const double MAX_OPACITY = 0.7;

        public Transparency(MainViewModel vm)
        {
            _mainVM = vm;
            _resourceLoader = vm.ResourceLoader;

            Handlers = new ObservableCollection<VisualHandler>();

            var totalImages = _resourceLoader.GetAmountOfImages(ResourceLoader.ImageFolder.Glass);
            for (int i = 0; i < totalImages; i++)
            {
                Handlers.Add(new VisualHandler($"Glass {i + 1}", null, 
                    perform : GetHandler(i),
                    close : () =>
                    {
                        SaveImage();

                        _mainVM.CanvasViewWorker.SetUpdateHandler();
                        _mainVM.SliderWorker.SetDefaultSliderValue();
                        _mainVM.SliderWorker.SetUpdateHandler();
                        _mainVM.SliderWorker.SetDragCompletedHandler();
                    }
                ));
            }
        }

        Action GetHandler(int index) {
            return () =>
            {
                _localImageCopy = _mainVM.ImageWorker.CloneImage(_mainVM.ImageWorker.Image);

                _filterImage = _resourceLoader.LoadSKBitmap(ResourceLoader.ImageFolder.Glass, index);

               _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
               _mainVM.SliderWorker.SetUpdateHandler(OnSliderValueChanged);
               _mainVM.CanvasViewWorker.Invalidate();
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

                canvas.DrawBitmap(_localImageCopy, info.Rect, BitmapStretch.Uniform, paint: paint);
                (_, SKRect rect) = SizeCalculator.GetScaleAndRect(new SKSize(info.Rect.Width, info.Rect.Height), _localImageCopy);

                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (MAX_OPACITY * progress)));
                canvas.DrawBitmap(_filterImage, rect, BitmapStretch.Fill, paint:paint);
            }
        }

        private void SaveImage()
        {
            _mainVM.ImageWorker.AddImageState();

            SKBitmap bitmap = _mainVM.ImageWorker.Image;
            SKBitmap newBitmap = new SKBitmap(bitmap.Info);
            using (SKCanvas canvas = new SKCanvas(newBitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                double progress = _mainVM.SliderWorker.SliderValue;

                canvas.DrawBitmap(_localImageCopy, new SKPoint(), paint: paint);

                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (MAX_OPACITY * progress)));

                canvas.DrawBitmap(_filterImage, 
                    new SKRect(0, 0, _localImageCopy.Width, _localImageCopy.Height), 
                    BitmapStretch.Fill, 
                    paint: paint);

            }
            _mainVM.ImageWorker.Image = newBitmap;
            _mainVM.CanvasViewWorker.Invalidate();
        }
    }
}



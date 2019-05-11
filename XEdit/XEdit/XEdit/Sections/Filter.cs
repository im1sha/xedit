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
    class Filter : BaseSection
    {
        private readonly MainViewModel _mainVM;

        private enum FilterType { Grayscale, Pastel }

        public override string Name => "Filter";
        public override string ImageUrl => "XEdit.Media.Sections.Filters.filter.png";

        private SKBitmap _bitmap;

        public Filter(MainViewModel vm)
        {
            _mainVM = vm;

            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler(FilterType.Grayscale),
                CreateHandler(FilterType.Pastel),
            };
        }

        public override Command SelectCommand
        {
            get
            {
                return new Command(() =>
                {
                    _bitmap = _mainVM.ImageWorker.CloneImage(_mainVM.ImageWorker.Image);
                });
            }
        }

        private VisualHandler CreateHandler(FilterType filter)
        {
            string name = null;

            EventHandler<SKPaintSurfaceEventArgs> handler = null;

            string url = null;

            switch (filter)
            {
                case FilterType.Grayscale:
                    url = "XEdit.Media.Sections.Filters.grayscale.png";
                    handler = OnGrayscale;
                    name = FilterType.Grayscale.ToString();
                    break;
                case FilterType.Pastel:
                    url = "XEdit.Media.Sections.Filters.pastel.png";
                    handler = OnPastel;
                    name = FilterType.Pastel.ToString();
                    break;
                default:
                    break;
            } 

            return new VisualHandler(
                name: name,
                url: url,
                perform: () =>
                {
                    _mainVM.CanvasViewWorker.SetUpdateHandler(handler);
                    _mainVM.CanvasViewWorker.Invalidate();
                },
                close: (success) =>
                {
                }
                );
        }

        private void Draw(SKCanvas canvas, SKPaint paint, SKBitmap bitmap, SKRect rect)
        {
            canvas.DrawBitmap(bitmap,
                   new SKRect(0, 0, rect.Width, rect.Height),
                   BitmapStretch.Uniform, paint: paint);
        }

        private void OnPastel(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            using (canvas)
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                paint.ColorFilter = GetFilter(FilterType.Pastel);
                Draw(canvas, paint, _bitmap, info.Rect);
                SetState(_bitmap, FilterType.Pastel);
            }
        }
    
        private void OnGrayscale(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            using (canvas)
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                paint.ColorFilter = GetFilter(FilterType.Grayscale);
                Draw(canvas, paint, _bitmap, info.Rect);
                SetState(_bitmap, FilterType.Grayscale);
            }
        }

        private void SetState(SKBitmap bitmap, FilterType filter)
        {
            _mainVM.ImageWorker.AddImageState();

            SKBitmap newBitmap = new SKBitmap(bitmap.Info);

            using (SKCanvas canvas = new SKCanvas(newBitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear();
                paint.ColorFilter = GetFilter(filter);
                canvas.DrawBitmap(bitmap,
                    new SKRect(0, 0, bitmap.Info.Width, bitmap.Info.Height),
                    BitmapStretch.Uniform, paint: paint);
            }

            _mainVM.ImageWorker.Image = newBitmap;
            SelectedHandler = null;
            _mainVM.CanvasViewWorker.SetUpdateHandler();
        }

        SKColorFilter GetFilter(FilterType filter)
        {
            if (filter == FilterType.Grayscale)
            {
                return SKColorFilter.CreateColorMatrix(new float[]
                {
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0.21f, 0.72f, 0.07f, 0, 0,
                    0,     0,     0,     1, 0
                });
            }
            //pastel
            return SKColorFilter.CreateColorMatrix(new float[]
            {
                0.75f, 0.25f, 0.25f, 0, 0,
                0.25f, 0.75f, 0.25f, 0, 0,
                0.25f, 0.25f, 0.75f, 0, 0,
                0,     0,     0,     1, 0
            });
        }
    }
}

using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XEdit.Core;
using XEdit.Extensions;

namespace XEdit.Sections
{
    class Features : Core.CoreSection
    {
        public override bool IsVariableValues() => true;

        public override string Name { get; } = "Features";

        public override Handler SelectedHandler
        {
            get
            {
                return _selectedHandler;
            }
            set
            {
                if (_selectedHandler != value)
                {
                    _selectedHandler?.Exit(null);
                    _selectedHandler = value;
                    OnPropertyChanged();
                    _selectedHandler.Perform(null);
                }
            }
        }

        public override Command LeaveCommand
        {
            get => new Command(obj => {
                SelectedHandler?.Exit(null);
                _selectedHandler = null;
            });
        }

        public Features()
        {
            Handlers = new ObservableCollection<Handler>()
            {
                new Handler("Transparency",
                    null,
                    (obj) => {
                        AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler(OnCanvaUpdate);
                        AppDispatcher.Get<ImageManager>().SetSliderUpdateHandler(OnSliderValueChanged);
                    },
                    (obj) => {
                        AppDispatcher.Get<ImageManager>().SetSliderUpdateHandler();
                        AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler();
                        AppDispatcher.Get<ImageManager>().SliderValue = 0;
                    }),
            };
        }

        void OnCanvaUpdate(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawBitmap(AppDispatcher.Get<ImageManager>().TempBitmap, info.Rect, BitmapStretch.Uniform);
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {        
            SKBitmap bitmap = AppDispatcher.Get<ImageManager>().CloneImage();

            SKBitmap newBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
            using (SKCanvas canvas = new SKCanvas(newBitmap))           
            using (SKPaint paint = new SKPaint())
            {
                canvas.Clear();

                float progress = (float)AppDispatcher.Get<ImageManager>().SliderValue;

                paint.Color = paint.Color.WithAlpha(
                    (byte)(0xFF * (1 - progress)));

                canvas.DrawBitmap(bitmap, new SKPoint(), paint);
            }

            //AppDispatcher.Get<ImageManager>().SetImage(newBitmap);
            AppDispatcher.Get<ImageManager>().TempBitmap = newBitmap ;
            AppDispatcher.Get<ImageManager>().InvalidateCanvasView();

            bitmap = null;
            GC.Collect();
        }
    }
}

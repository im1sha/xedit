using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.TouchTracking;
using XEdit.Utils;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class Image : BaseSection
    {
        List<TouchManipulationBitmap> _bitmapCollection;
        Dictionary<long, TouchManipulationBitmap> _bitmapDictionary;

        private readonly ResourceLoader _resourceLoader;

        private readonly MainViewModel _mainVM;

        private SKBitmap _backgroundBitmap;

        public override string Name => "Image";

        public override Command SelectCommand
        {
            get => new Command(() => {
                _bitmapCollection = new List<TouchManipulationBitmap>();
                _bitmapDictionary = new Dictionary<long, TouchManipulationBitmap>();
            });
        }

        public Image(MainViewModel vm)
        {
            _mainVM = vm;
            _resourceLoader = _mainVM.ResourceLoader;

            Handlers = new ObservableCollection<VisualHandler>();

            var totalImages = _resourceLoader.GetAmountOfImages(ResourceLoader.ImageFolder.Image);

            for (int i = 0; i < totalImages; i++)
            {
                Handlers.Add(CreateHandler(i));             
            }        
        }

        private VisualHandler CreateHandler(int i)
        {
            string name = $"Image {i + 1}";

            return new VisualHandler(
                name: name,
                url: null,
                perform: () =>
                {             
                    _backgroundBitmap = _mainVM.ImageWorker.Image;
                    SKPoint position = new SKPoint();
                    _bitmapCollection.Add(new TouchManipulationBitmap(_resourceLoader.LoadSKBitmap(ResourceLoader.ImageFolder.Image, i))
                    {
                        Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
                    });

                    _mainVM.TouchWorker.SetUpdateHandler(OnTouchEffectAction);
                    _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
                    _mainVM.CanvasViewWorker.Invalidate();

                    SelectedHandler = null;
                },
                close: (success) => {
                    _mainVM.TouchWorker.SetUpdateHandler();
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    _mainVM.CanvasViewWorker.Invalidate();
                }
                );
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(_mainVM.CanvasViewWorker.ViewCanvasSizeWidth * pt.X / _mainVM.CanvasViewWorker.ViewWidth),
                            (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeHeight * pt.Y / _mainVM.CanvasViewWorker.ViewHeight));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    for (int i = _bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = _bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
                            // Move bitmap to end of collection
                            _bitmapCollection.Remove(bitmap);
                            _bitmapCollection.Add(bitmap);

                            // Do the touch processing
                            _bitmapDictionary.Add(args.Id, bitmap);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            _mainVM.CanvasViewWorker.Invalidate();
                            break;
                        }
                    }
                    break;

                case TouchActionType.Moved:
                    if (_bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = _bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        _mainVM.CanvasViewWorker.Invalidate();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (_bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = _bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        _bitmapDictionary.Remove(args.Id);
                        _mainVM.CanvasViewWorker.Invalidate();
                    }
                    break;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(_mainVM.ImageWorker.Image, info.Rect, BitmapStretch.Uniform);

            foreach (TouchManipulationBitmap bitmap in _bitmapCollection)
            {
                bitmap.Paint(canvas);
            }
        }

        void OnSave()
        {

        }
    }
}

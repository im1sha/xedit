//using SkiaSharp;
//using SkiaSharp.Views.Forms;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Text;
//using Xamarin.Forms;
//using XEdit.Extensions;
//using XEdit.ViewModels;

//namespace XEdit.Sections
//{
//    class Special : BaseSection
//    {
//        readonly MainViewModel _mainVM;

//        public override bool IsVariableValues() => true;

//        public override string Name { get; } = "Special";

//        public override VisualHandler SelectedHandler
//        {
//            get
//            {
//                return _selectedHandler;
//            }
//            set
//            {
//                if (_selectedHandler != value)
//                {
//                    _selectedHandler?.Exit();
//                    _selectedHandler = value;
//                    OnPropertyChanged();
//                    _selectedHandler.Perform();
//                }
//            }
//        }

//        public override Command LeaveCommand
//        {
//            get => new Command(obj => {
//                SelectedHandler?.Exit();
//                _selectedHandler = null;
//            });
//        }

//        public Special(MainViewModel vm)
//        {
//            _mainVM = vm;
       
//            Handlers = new ObservableCollection<VisualHandler>()
//            {
//                new VisualHandler("Transparency",
//                    null,
//                    () => {
//                        UniqueInstancesManager.Get<VisualControl>().SetCanvasUpdateHandler(OnCanvaUpdate);
//                        UniqueInstancesManager.Get<VisualControl>().SetSliderUpdateHandler(OnSliderValueChanged);
//                    },
//                    () => {
//                        UniqueInstancesManager.Get<VisualControl>().SetSliderUpdateHandler();
//                        UniqueInstancesManager.Get<VisualControl>().SetCanvasUpdateHandler();
//                        UniqueInstancesManager.Get<VisualControl>().SliderValue = 0;
//                    },null                   
//                ),
//            };
//        }

//        void OnCanvaUpdate(object sender, SKPaintSurfaceEventArgs args)
//        {
//            SKImageInfo info = args.Info;
//            SKSurface surface = args.Surface;
//            SKCanvas canvas = surface.Canvas;

//            canvas.Clear();

//            canvas.DrawBitmap(UniqueInstancesManager.Get<VisualControl>().TempBitmap, info.Rect, BitmapStretch.Uniform);
//        }

//        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
//        {        
//            SKBitmap bitmap = UniqueInstancesManager.Get<VisualControl>().CloneImage();

//            SKBitmap newBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
//            using (SKCanvas canvas = new SKCanvas(newBitmap))           
//            using (SKPaint paint = new SKPaint())
//            {
//                canvas.Clear();

//                float progress = (float)UniqueInstancesManager.Get<VisualControl>().SliderValue;

//                paint.Color = paint.Color.WithAlpha(
//                    (byte)(0xFF * (1 - progress)));

//                canvas.DrawBitmap(bitmap, new SKPoint(), paint);
//            }

//            //AppDispatcher.Get<ImageManager>().SetImage(newBitmap);
//            UniqueInstancesManager.Get<VisualControl>().TempBitmap = newBitmap ;
//            UniqueInstancesManager.Get<VisualControl>().InvalidateCanvasView();

//            bitmap = null;
//            GC.Collect();
//        }
//    }
//}

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
    class Crop: BaseSection
    {
        private readonly MainViewModel _mainVM;

        public override string Name => "Crop";

        public Crop(MainViewModel vm)
        {
            _mainVM = vm;

            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler()
            };
        }

        PhotoCropperCanvasView _photoCropper;

        private VisualHandler CreateHandler()
        {
            return new VisualHandler(
                name: "Free",
                url: null,
                perform: () =>
                {
                    _photoCropper = new PhotoCropperCanvasView(_mainVM.TouchWorker, _mainVM.ImageWorker.Image);
                    _mainVM.CanvasViewWorker.ChangeCanvas(_photoCropper);

                    //_mainVM.CanvasViewWorker.SetUpdateHandler();
                    // OnCrop();
                    //_mainVM.CanvasViewWorker.Invalidate();
                },
                close: () => {
                    _mainVM.ImageWorker.Image = _photoCropper.CroppedBitmap;
                    _mainVM.CanvasViewWorker.ChangeCanvas(new SKCanvasView());
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    _mainVM.CanvasViewWorker.Invalidate();
                }
                );
        }

        void OnCrop()
        {
            //_mainVM.ImageWorker.AddImageState();

            //SKBitmap bitmap = _mainVM.ImageWorker.Image;
            //SKBitmap flippedBitmap = new SKBitmap(bitmap.Info);
            //using (SKCanvas canvas = new SKCanvas(flippedBitmap))
            //{
            //    canvas.Clear();
            //    if (vertical)
            //    {
            //        canvas.Scale(-1, 1, bitmap.Width / 2, 0);
            //    }
            //    else
            //    {
            //        canvas.Scale(1, -1, 0, bitmap.Height / 2);
            //    }
            //    canvas.DrawBitmap(bitmap, new SKPoint());
            //}

            //_mainVM.ImageWorker.Image = flippedBitmap;  // set new image

            //// it should be no selected item bc flipping is not continuous action
            //SelectedHandler = null;
        }
    
       
    }
}

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

        public override string ImageUrl => "XEdit.Media.Sections.Crop.crop.png";

        public Crop(MainViewModel vm)
        {
            _mainVM = vm;

            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler()
            };
        }

        CropCanvasView _photoCropper;

        private VisualHandler CreateHandler()
        {
            return new VisualHandler(
                name: "Free",
                url: "XEdit.Media.Sections.Crop.free.png",
                perform: () =>
                {
                    _photoCropper = new CropCanvasView(_mainVM.TouchWorker, _mainVM.ImageWorker.Image);
                    _mainVM.CanvasViewWorker.ChangeCanvas(_photoCropper);
                },
                close: (success) => {
                    if (success)
                    {
                        _mainVM.ImageWorker.AddImageState();
                        _mainVM.ImageWorker.Image = _photoCropper.CroppedBitmap;                     
                    }
                    else
                    {
                    }
                    _mainVM.CanvasViewWorker.ChangeCanvas(new SKCanvasView());
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    _mainVM.TouchWorker.SetUpdateHandler();
                    _mainVM.CanvasViewWorker.Invalidate();          
                }         
                );
        }
    }
}

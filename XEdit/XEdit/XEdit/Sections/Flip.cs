using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using XEdit.Extensions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    public class Flip : BaseSection
    {
        private readonly MainViewModel _mainVM;
       
        public override string Name => "Flip";

        public Flip(MainViewModel vm)
        {
            _mainVM = vm;
       
            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler(true),
                CreateHandler(false)
            };
        }

        private VisualHandler CreateHandler(bool vertical)
        {
            return new VisualHandler(
                name: vertical ? "Vertical" : "Horizontal",
                url: null,
                performAction: () =>
                {
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    if (vertical)
                    {
                        OnVerticalFlip();
                    }
                    else
                    {
                        OnHorizontalFlip();
                    }
                    _mainVM.CanvasViewWorker.Invalidate();
                }
                );
        }

        void OnVerticalFlip()
        {
            OnFlip(true);
        }

        void OnHorizontalFlip()
        {
            OnFlip(false);
        }

        void OnFlip(bool vertical)
        {
            _mainVM.ImageWorker.AddImageState();

            SKBitmap bitmap = _mainVM.ImageWorker.Image;
            SKBitmap flippedBitmap = new SKBitmap(bitmap.Info);
            using (SKCanvas canvas = new SKCanvas(flippedBitmap))
            {
                canvas.Clear();
                if (vertical)
                {
                    canvas.Scale(-1, 1, bitmap.Width / 2, 0);
                }
                else
                {
                    canvas.Scale(1, -1, 0, bitmap.Height / 2);
                }
                canvas.DrawBitmap(bitmap, new SKPoint());
            }

            //task.Wait();                                // wait until image is copied
            _mainVM.ImageWorker.Image = flippedBitmap;  // set new image

            // it should be no selected item bc flipping is not continuous action
            _selectedHandler = null;
        }
    } 
}

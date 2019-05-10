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

        public override string ImageUrl => "XEdit.Media.Sections.Flip.rotate_and_flip.png";

        public Flip(MainViewModel vm)
        {
            _mainVM = vm;
       
            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler(true, true),
                CreateHandler(true, false),
                CreateHandler(false, true),
                CreateHandler(false, false),
            };
        }

        private VisualHandler CreateHandler(bool flip, bool verticalOrLeft)
        {
            string name = null;
            Action action = null;
            string url = null;
            if (flip && verticalOrLeft)
            {
                url = "XEdit.Media.Sections.Flip.vertical_flip.png";
                action = OnVerticalFlip;
                name = "Vertical Flip";
            }
            else if (flip && !verticalOrLeft)
            {
                url = "XEdit.Media.Sections.Flip.horizontal_flip.png";
                action = OnHorizontalFlip;
                name = "Horizontal Flip";
            }
            else if (!flip && verticalOrLeft)
            {
                url = "XEdit.Media.Sections.Flip.rotate_left.png";
                action = OnLeftRotate;
                name = "Left Rotate";
            }
            else
            {
                url = "XEdit.Media.Sections.Flip.rotate_right.png";
                action = OnRightRotate;
                name = "Right Rotate";
            }

            return new VisualHandler(
                name: name,
                url: url,
                perform: () =>
                {
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    action();
                    _mainVM.CanvasViewWorker.Invalidate();
                },
                close: (success) => { }
                );
        }


        private void OnRotate(bool left)
        {
            _mainVM.ImageWorker.AddImageState();

            SKBitmap bitmap = _mainVM.ImageWorker.Image;

            double radians = Math.PI * 0.5;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            SKBitmap newBitmap = new SKBitmap(rotatedWidth, rotatedHeight);

            using (SKCanvas canvas = new SKCanvas(newBitmap))
            {
                canvas.Clear(SKColors.LightPink);
                canvas.Translate(rotatedWidth / 2, rotatedHeight / 2);
                canvas.RotateDegrees(90);
                canvas.Translate(-originalWidth / 2, -originalHeight / 2);
                canvas.DrawBitmap(bitmap, new SKPoint());
            }

            _mainVM.ImageWorker.Image = newBitmap; 
            SelectedHandler = null;
        }

        private void OnRightRotate()
        {
            OnRotate(false);
        }

        private void OnLeftRotate()
        {
            OnRotate(true);
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

            _mainVM.ImageWorker.Image = flippedBitmap;  // set new image

            // it should be no selected item bc flipping is not continuous action
            SelectedHandler = null;
        }
    }    
}

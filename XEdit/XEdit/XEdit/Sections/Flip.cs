using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using XEdit.Extensions;
using XEdit.Core;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XEdit.Sections
{
    public class Flip : CoreSection
    {
        public override string Name => "Flip";

        public Flip()
        {
            Handlers = new ObservableCollection<Handler>()
            {
                CreateHandler(true),
                CreateHandler(false)
            };
        }

        Handler CreateHandler(bool vertical)
        {
            Action performAction;

            if (vertical)
            {
                performAction = () =>
                {
                     AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler();
                     OnVerticalFlip();
                     AppDispatcher.Get<ImageManager>().InvalidateCanvasView();
                };
            }
            else
            {
                performAction = () =>
                {
                    AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler();
                    OnHorizontalFlip();
                    AppDispatcher.Get<ImageManager>().InvalidateCanvasView();
                };
            }

            return new Handler("Vertical", null,
                performAction: performAction,
                commitAction: () => { },
                prepareAction: () => { },
                rollbackAction: () => { },
                exitAction: () => { }
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
            SKBitmap bitmap = AppDispatcher.Get<ImageManager>().CloneImage();

            SKBitmap flippedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
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

            AppDispatcher.Get<ImageManager>().SetImage(flippedBitmap);

            bitmap = null;
            GC.Collect();

            // it should be no selected item bc flipping is not continuos action
            _selectedHandler = null;
        }
    } 
}

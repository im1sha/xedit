using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using XEdit.Extensions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XEdit.Sections
{
    public class Flip : BaseSection
    {
        public override string Name => "Flip";

        public Flip()
        {
            Handlers = new ObservableCollection<VisualHandler>()
            {
                CreateHandler(true),
                CreateHandler(false)
            };
        }

        VisualHandler CreateHandler(bool vertical)
        {
            Action performAction;

            if (vertical)
            {
                performAction = () =>
                {
                     UniqueInstancesManager.Get<VisualControl>().SetCanvasUpdateHandler();
                     OnVerticalFlip();
                     UniqueInstancesManager.Get<VisualControl>().InvalidateCanvasView();
                };
            }
            else
            {
                performAction = () =>
                {
                    UniqueInstancesManager.Get<VisualControl>().SetCanvasUpdateHandler();
                    OnHorizontalFlip();
                    UniqueInstancesManager.Get<VisualControl>().InvalidateCanvasView();
                };
            }

            return new VisualHandler("Vertical", null,
                performAction: performAction,
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
            SKBitmap bitmap = UniqueInstancesManager.Get<VisualControl>().CloneImage();

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

            UniqueInstancesManager.Get<VisualControl>().OnStart(flippedBitmap);

            bitmap = null;
            GC.Collect();

            // it should be no selected item bc flipping is not continuos action
            _selectedHandler = null;
        }
    } 
}

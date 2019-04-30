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
                    _selectedHandler = null;
                }
            }
        }

        public Flip()
        {
            Handlers = new ObservableCollection<Handler>()
            { 
                new Handler("Vertical", 
                    null,
                    (obj) => { OnVerticalFlip(); AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler(); }, 
                    (obj) => { }),
                new Handler("Horizontal",
                    null,
                    (obj) => { OnHorizontalFlip(); AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler(); },
                    (obj) => { }),
            };
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
            AppDispatcher.Get<ImageManager>().UpdateCanvasView();

            bitmap = null;
            GC.Collect();
        }
    } 
}

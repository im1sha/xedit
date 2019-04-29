using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using XEdit.Core;

namespace XEdit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string Status { get; set; }

        public ObservableCollection<ISection> Sections { get; set; } = new ObservableCollection<ISection>();

        //private _ISection _previousSection;
        private ISection _selectedSection;
        public ISection SelectedSection
        {
            get => _selectedSection;           
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection = value;
                    OnPropertyChanged();
                }
            }
        }

        //      DeleteQuote = new Command<QuoteViewModel>(async vm => OnDeleteQuote(vm));
        //      CommandParameter="{Binding}"

        public ICommand SaveCommand
        {
            get
            {
                return new Command<SKCanvasView>((SKCanvasView skiaCanvas) =>
                {
                    AppDispatcher.Get<ImageManager>().Save(skiaCanvas);
                });
            }
        }




        public ICommand ApplyCommand
        {
            get
            {
                return new Command(canvas => { });
            }
        }
         
        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

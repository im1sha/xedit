using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using SkiaSharp;
using Xamarin.Forms;
using XEdit.Interaction;
using XEdit.Sections;

namespace XEdit.ViewModels
{
    public class FiltersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<_ISection> Sections { get; set; } = new ObservableCollection<_ISection>();

        //private _ISection _previousSection;
        private _ISection _selectedSection;
        public _ISection SelectedSection
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

        public ICommand ApplyCommand
        {
            get
            {
                // target is skiaWrapper
                return new Command((target) => {
                    if (_ViewFunctionality.IsImageLoaded)
                    {
                        //if (prevSection != null)
                        //{
                        //    prevSection.CancelCommand.Execute(target);
                        //    prevSection = null;
                        //}

                        //SelectedSection.SelectCommand.Execute(target);
                    }
                });
            }
        }

        public FiltersViewModel()
        {
            InitializeSections();
        }

        private void InitializeSections()
        {
            Sections.Add(new ColorSection());
            Sections.Add(new CropSection());
            Sections.Add(new RotateSection());
            _selectedSection = Sections[0];
        }
      
      
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

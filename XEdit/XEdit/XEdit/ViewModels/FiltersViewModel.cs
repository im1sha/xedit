using XEdit.Interaction;
using XEdit.Sections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XEdit.ViewModels
{
    public class FiltersViewModel : INotifyPropertyChanged
    {
        #region sections

        public ObservableCollection<IHandler> Sections { get; private set; }
            = new ObservableCollection<IHandler>();

        private IHandler selectedSection;
        public IHandler SelectedSection
        {
            get
            {
                return selectedSection;
            }
            set
            {
                if (selectedSection != value)
                {
                    selectedSection = value;
                    OnPropertyChanged();
                }
            }
        }

       // private 

        public ICommand SetFilterCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        public ICommand SetSectionCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        private void InitializeSections()
        {
            Sections.Add(new CropSection());
            Sections.Add(new CombineSection());
            Sections.Add(new ColorSection());
            Sections.Add(new RotateSection());
            Sections.Add(new SkewSection());
            SelectedSection = Sections.FirstOrDefault();
        }

        #endregion

        #region pictureLoader

        private readonly Handlers.PictureLoader pictureLoader 
            = new Handlers.PictureLoader();

        public ICommand LoadImageCommand
        {
            get
            {
                return new Command((object target) =>
                {
                    pictureLoader.GetAction(target, null) (null);
                });
            }
        }

        #endregion

        #region working with changes

        public ICommand ApplyChangesCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        public ICommand DiscardChangesCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        #endregion 
      
        public FiltersViewModel()
        {
            InitializeSections();
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

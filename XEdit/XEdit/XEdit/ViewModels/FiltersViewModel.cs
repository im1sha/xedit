using XEdit.Interaction;
using XEdit.Filters;
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
        public ObservableCollection<IHandler> Filters { get; private set; }
            = new ObservableCollection<IHandler>();

        private readonly Handlers.PictureLoader pictureLoader 
            = new Handlers.PictureLoader();

        private IHandler selectedFilter;
        public IHandler SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            set
            {
                if (selectedFilter != value)
                {
                    selectedFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        //<Button Command = "{Binding RemoveCommand}" CommandParameter="{Binding SelectedOne}">-</Button>

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

        public ICommand SetFilterCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        public ICommand SetFilterOptionCommand
        {
            get
            {
                return new Command(obj => { });
            }
        }

        public ICommand LoadImageCommand
        {
            get
            {
                return new Command((object target) =>
                {
                    pictureLoader.GetAction(target, null) ();
                });
            }    
        }



        public FiltersViewModel()
        {
            InitializeFilters();
        }

        private void InitializeFilters()
        {
            Filters.Add(new CropFilter());
            Filters.Add(new CombineFilter());
            Filters.Add(new ColorFilter());
            Filters.Add(new RotateFilter());
            Filters.Add(new SkewFilter());
            SelectedFilter = Filters.FirstOrDefault();
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

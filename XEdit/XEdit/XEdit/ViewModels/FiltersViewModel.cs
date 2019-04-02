using XEdit.Models;
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
        public ObservableCollection<IFilter> Filters { get; private set; } // to set

        private IFilter selectedFilter;
        public IFilter SelectedFilter
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

        // obj is selected filter
        public ICommand ApplyFilterCommand
        {
            get
            {
                return new Command(obj => {
                    
                });
            }
        }

        public FiltersViewModel()
        {
            InitializeFilters();
        }

        private void InitializeFilters()
        {
           // Filters.Add();

            
            //SelectedFilter = Filters.FirstOrDefault();
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

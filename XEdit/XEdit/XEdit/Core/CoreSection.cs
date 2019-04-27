using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XEdit.Core;

namespace XEdit.Interaction
{
    public class CoreSection : INotifyPropertyChanged, ISection
    {
        public virtual string Name => throw new NotImplementedException();
        public virtual string ImageUrl => throw new NotImplementedException();

        public ObservableCollection<Handler> Handlers { get; set; } = new ObservableCollection<Handler>();
        public Handler _selectedHandler;
        public Handler SelectedHandler
        {
            get
            {
                return _selectedHandler;
            }
            set
            {
                if (_selectedHandler != value)
                {
                    _selectedHandler = value;
                    OnPropertyChanged();
                }               
            }
        }
  
        public virtual Command SelectCommand
        {
            get => new Command(arg => SelectedHandler?.Start(arg));
        }

        public virtual Command LeaveCommand
        {
            get => new Command(arg => SelectedHandler?.End(arg));          
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XEdit.Core;

namespace XEdit.Core
{
    public class CoreSection : INotifyPropertyChanged, ISection
    {
        public virtual bool IsVariableValues() => false; // is slider required?
        public virtual string Name => throw new NotImplementedException();
        public virtual string ImageUrl => throw new NotImplementedException();

        public ObservableCollection<Handler> Handlers { get; set; } = new ObservableCollection<Handler>();
        public Handler _selectedHandler;
        public virtual Handler SelectedHandler
        {
            get => _selectedHandler;
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
            get => new Command(obj => { });
        }

        public virtual Command LeaveCommand
        {
            get => new Command(obj => { });          
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

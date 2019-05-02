using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Sections
{
    public class BaseSection : INotifyPropertyChanged, ISection
    {
        public virtual bool IsVariableValues() => false; // is slider required?

        public virtual string Name => throw new NotImplementedException();

        public virtual string ImageUrl => throw new NotImplementedException();

        public ObservableCollection<VisualHandler> Handlers { get; set; } =
            new ObservableCollection<VisualHandler>();

        public VisualHandler _selectedHandler;
        public virtual VisualHandler SelectedHandler
        {
            get => _selectedHandler;
            set
            {
                if (_selectedHandler != value)
                {
                    _selectedHandler = value;
                    OnPropertyChanged();
                    _selectedHandler.Perform(); // example: flip
                }               
            }
        }
  
        public virtual Command SelectCommand
        {
            get => new Command(() => { });
        }

        public virtual Command LeaveCommand
        {
            get => new Command(() => {
                SelectedHandler?.Exit();
                _selectedHandler = null;
            });
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

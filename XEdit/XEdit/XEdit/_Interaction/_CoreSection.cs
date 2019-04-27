using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Interaction
{
    public class _CoreSection : INotifyPropertyChanged, _ISection
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string ImageUrl => throw new NotImplementedException();

        public ObservableCollection<_IHandler> Handlers { get; set; } = new ObservableCollection<_IHandler>();
        public _IHandler selectedHandler;
        public _IHandler SelectedHandler
        {
            get
            {
                return selectedHandler;
            }
            set
            {
                selectedHandler = value;
                OnSelectedHandler();
                OnPropertyChanged();
            }
        }

        protected virtual void OnSelectedHandler() { }

        public virtual Command SelectCommand
        {
            get
            {
                return new Command((object target) => {
                    SelectedHandler?.SelectAction(target, null) (null);
                });
            }
        }

        public virtual Command CancelCommand
        {
            get
            {
                return new Command((object target) => {
                    SelectedHandler?.CancelAction(target, null)(null);
                });
            }
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

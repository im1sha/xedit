﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Interaction
{
    public class CoreSection : INotifyPropertyChanged, ISection
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual string ImageUrl => throw new NotImplementedException();

        public ObservableCollection<IHandler> Handlers { get; set; } = new ObservableCollection<IHandler>();
        public IHandler selectedHandler;
        public IHandler SelectedHandler
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
                    SelectedHandler?.GetAction(target, null) (null);
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

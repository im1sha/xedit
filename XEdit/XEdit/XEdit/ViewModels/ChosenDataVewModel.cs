using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace XEdit.ViewModels
{
    class ChosenDataViewModel : INotifyPropertyChanged
    {
        private Image image = null;
        public Image Image { get { return image; } set { image = value; OnPropertyChanged(); } } 
        public bool IsSet { get => Image != null; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

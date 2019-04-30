using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using XEdit.Core;

namespace XEdit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string Status { get; set; }

        public ObservableCollection<ISection> Sections { get; set; } = new ObservableCollection<ISection>();

        private bool _isVariableValues;
        public bool IsVariableValues
        {
            get => _isVariableValues;
            set
            {
                if (_isVariableValues != value)
                {
                    _isVariableValues = value;
                    OnPropertyChanged();
                }
            }
        }

        //private _ISection _previousSection;
        private ISection _selectedSection;
        public ISection SelectedSection
        {
            get => _selectedSection;           
            set
            {
                if (_selectedSection != value)
                {
                    _selectedSection?.LeaveCommand?.Execute(null);
                    _selectedSection = value;
                    OnPropertyChanged();
                    IsVariableValues = _selectedSection.IsVariableValues();
                    _selectedSection.SelectCommand.Execute(null);
                }
            }
        }

        //      CommandParameter="{Binding}"

        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await AppDispatcher.Get<ImageManager>().Save();
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }

        /// <summary>
        /// Sets backup picture. Backup picture will be used on CancelCommand call
        /// </summary>
        public ICommand CommitCommand
        {
            get
            {
                return new Command(() =>
                {
                });
            }
        }


        public MainViewModel()
        {
            Sections.Add(new Sections.Flip());
            Sections.Add(new Sections.Features());
            Sections.Add(new Sections.FingerPainting());
            SelectedSection = Sections.FirstOrDefault();
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



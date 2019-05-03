﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using XEdit.Sections;
using XEdit.TouchTracking;
using XEdit.Views;

namespace XEdit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ImageWorker ImageWorker { get; private set; } = UniqueInstancesManager.Get<ImageWorker>();
        public CanvasViewWorker CanvasViewWorker { get; private set; }
        public TouchWorker TouchWorker { get; private set; }
        public SliderWorker SliderWorker { get; private set; }

        public string Status { get; set; }

        public ObservableCollection<ISection> Sections { get; set; } =
            new ObservableCollection<ISection>();

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

        public MainViewModel()
        {
            Sections.Add(new Flip(this));
            //Sections.Add(new Sections.Special(this));
            Sections.Add(new Sections.FingerPainting(this));
            SelectedSection = Sections.FirstOrDefault();
        }

        // CommandParameter="{Binding}"

        public ICommand SaveCommand
        {
            get => new Command(async() =>
            {
                if (IsVariableValues)
                {
                    SliderWorker.SetDefaultSliderValue();
                }
                string name = await ImageWorker.SaveImage();
                MessagingCenter.Send(this, Messages.SaveSuccess, name);
            });
        }

        // '<-' was pressed
        public ICommand CancelCommand 
        {
            get => new Command(() =>
            {
                ImageWorker.RestorePreviousImageState();

                // mb check updatehandler
                CanvasViewWorker.Invalidate();
            });
        }

        // ok was pressed
        public ICommand CommitCommand 
        {
            get => new Command(() =>
            {
                ImageWorker.AddBackupImage();
            });            
        }

        public async Task OnPopScreen() { }

        public void OnViewCreated(
            SKCanvasView skiaCanvasView,
            Slider variableValuesSlider,
            TouchEffect touchTracker)
        {
            if (ImageWorker == null)
            {
                throw new ApplicationException("_imageWorker is not initialized");
            }
            CanvasViewWorker = new CanvasViewWorker(skiaCanvasView, ImageWorker);
            SliderWorker = new SliderWorker(variableValuesSlider);
            TouchWorker = new TouchWorker(touchTracker);
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



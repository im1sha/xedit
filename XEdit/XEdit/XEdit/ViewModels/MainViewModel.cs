using System;
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
using XEdit.Utils;
using XEdit.Views;

namespace XEdit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ImageWorker ImageWorker { get; private set; } = UniqueInstancesManager.Get<ImageWorker>();
        public ResourceLoader ResourceLoader { get; } = new ResourceLoader();

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
                    // closes handler and then asign null to it
                    _selectedSection?.LeaveCommand?.Execute(null);

                    _selectedSection = value;
                    OnPropertyChanged();

                    if (_selectedSection != null)
                    {
                        IsVariableValues = _selectedSection.IsVariableValues();
                        _selectedSection.SelectCommand?.Execute(null);
                    }             
                }
            }
        }

        public MainViewModel()
        {
            Sections.Add(new ColorFilters(this));
            Sections.Add(new Crop(this));
            Sections.Add(new Flip(this));
            Sections.Add(new Transparency(this));
            Sections.Add(new Sections.Image(this));
            Sections.Add(new Painting(this));
            SelectedSection = Sections.FirstOrDefault();
        }

        public ICommand SaveCommand
        {
            get => new Command(async() =>
            {
                CommitCommand.Execute(null);
                ImageWorker.DeleteStates();
                string name = await ImageWorker.SaveImage();
                MessagingCenter.Send(this, Messages.SaveSuccess, name);
            });
        }

        // '<-' was pressed
        public ICommand CancelCommand 
        {
            get => new Command(() =>
            {
                if (SelectedSection != null)
                {
                    SelectedSection.SelectedHandler?.Close(false);
                    SelectedSection.SelectedHandler = null;                 
                }

                SliderWorker.SetDefaultSliderValue();
                ImageWorker.RestorePreviousImageState();
                CanvasViewWorker.Invalidate();
            });
        }

        // ok was pressed
        public ICommand CommitCommand 
        {
            get => new Command(() =>
            {
                if ((SelectedSection != null) && (SelectedSection.SelectedHandler != null))
                {
                    SelectedSection.SelectedHandler.Close(true);
                    SelectedSection.SelectedHandler = null;
                }

                SliderWorker.SetDefaultSliderValue();
            });            
        }

        public void OnPopScreen()
        {
            ImageWorker.ResetData();
        }

        public void OnViewCreated(
            Layout<View> container,
            SKCanvasView skiaCanvasView,
            Slider variableValuesSlider,
            TouchEffect touchTracker)
        {
            if (ImageWorker == null)
            {
                throw new ApplicationException("_imageWorker is not initialized");
            }
            CanvasViewWorker = new CanvasViewWorker(container, skiaCanvasView, ImageWorker);
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



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

namespace XEdit.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ImageWorker _imageWorker = UniqueInstancesManager.Get<ImageWorker>();
        private CanvasViewWorker _canvasViewWorker;
        private TouchWorker _touchWorker;
        private SliderWorker _sliderWorker;

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
            Sections.Add(new Sections.Flip());
            Sections.Add(new Sections.Special());
            Sections.Add(new Sections.FingerPainting());
            SelectedSection = Sections.FirstOrDefault();
        }

        // CommandParameter="{Binding}"

        public ICommand SaveCommand
        {
            get => new Command(async () =>
            {
                await _imageWorker.SaveImage();
            });
        }

        // '<-' was pressed
        public ICommand CancelCommand 
        {
            get => new Command(async () =>
            {
            });
        }

        // ok was pressed
        public ICommand CommitCommand 
        {
            get => new Command(async () =>
            {
            });            
        }

        public async Task OnPopScreen()
        {
        }

        public void OnViewCreated(
            SKCanvasView skiaCanvasView,
            Slider variableValuesSlider,
            TouchEffect touchTracker)
        {
            if (_imageWorker == null)
            {
                throw new ApplicationException("_imageWorker is not initialized");
            }
            _canvasViewWorker = new CanvasViewWorker(skiaCanvasView, _imageWorker);
            _sliderWorker = new SliderWorker(variableValuesSlider);
            _touchWorker = new TouchWorker(touchTracker);
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



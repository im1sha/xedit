using XEdit.Interaction;
using XEdit.Sections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XEdit.Handlers;

namespace XEdit.ViewModels
{
    public class FiltersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ISection> Sections { get; set; }
            = new ObservableCollection<ISection>();
     
        private ISection selectedSection;
        public ISection SelectedSection
        {
            get
            {
                return selectedSection;
            }
            set
            {
                if (selectedSection != value)
                {
                    selectedSection = value;
                    OnSelectedSectionChanged();
                    OnPropertyChanged();
                }
            }
        }

        private void OnSelectedSectionChanged() { }

        public ICommand SetSectionCommand
        {
            get
            {
                return new Command(() => { /*SelectedSection.SelectCommand.Execute();*/ });
            }
        }

        public ICommand ButtonCommand {
            get
            {
                return new Command((t) => { SelectedSection.SelectCommand.Execute(t); });
            }
        }

        public ISection ImageLoader { get; } = new PictureLoader();

        private void InitializeSections()
        {
            Sections.Add(new ColorSection());
            Sections.Add(new CropSection());
            //Sections.Add(new CombineSection());            
            //Sections.Add(new RotateSection());
            //Sections.Add(new SkewSection());
            selectedSection = Sections[0];
        }
      
        public FiltersViewModel()
        {
            InitializeSections();
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

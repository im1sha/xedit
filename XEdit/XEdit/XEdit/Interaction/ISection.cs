using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Interaction
{
    public interface ISection
    {
        string Name { get; }
        string ImageUrl { get; }
        ObservableCollection<IHandler> Handlers { get; }
        IHandler SelectedHandler { get; }
        Command SelectCommand { get; }
        Command CancelCommand { get; }
    }
}

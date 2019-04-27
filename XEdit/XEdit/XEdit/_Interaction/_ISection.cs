using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Interaction
{
    public interface _ISection
    {
        string Name { get; }
        string ImageUrl { get; }
        ObservableCollection<_IHandler> Handlers { get; }
        _IHandler SelectedHandler { get; }
        Command SelectCommand { get; }
        Command CancelCommand { get; }
    }
}

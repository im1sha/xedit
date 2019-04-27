using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Core
{
    public interface ISection
    {
        string Name { get; }
        string ImageUrl { get; }

        ObservableCollection<Handler> Handlers { get; }
        Handler SelectedHandler { get; }

        /// <summary>
        /// Section is selected 
        /// </summary>
        Command SelectCommand { get; }

        /// <summary>
        /// Another section's been chosen
        /// </summary>
        Command LeaveCommand { get; }
    }
}

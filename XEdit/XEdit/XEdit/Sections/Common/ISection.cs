using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace XEdit.Sections
{
    public interface ISection
    {
        bool IsVariableValues(); // is slider required?

        string Name { get; }
        string ImageUrl { get; }

        ObservableCollection<VisualHandler> Handlers { get; }
        VisualHandler SelectedHandler { get; }

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

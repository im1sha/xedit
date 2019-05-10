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
        bool IsInteractive(); // section deal with multiple interactive objects and handles cancellation itself

        string Name { get; }
        string ImageUrl { get; }

        ObservableCollection<VisualHandler> Handlers { get; set; }
        VisualHandler SelectedHandler { get; set; }

        /// <summary>
        /// Section is selected 
        /// </summary>
        Command SelectCommand { get; }

        /// <summary>
        /// Another section's been chosen
        /// </summary>
        Command LeaveCommand { get; }

        Command CommitCommand { get; }

        Command CancelCommand { get; }
    }
}

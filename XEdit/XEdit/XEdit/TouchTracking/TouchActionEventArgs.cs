﻿using System;
using Xamarin.Forms;

namespace XEdit.TouchTracking
{
    public class TouchActionEventArgs : EventArgs
    {
        public TouchActionEventArgs(long id, TouchActionType type, Point location, bool isInContact)
        {
            Id = id;
            Type = type;
            Location = location;
            IsInContact = isInContact;
        }

        public long Id { private set; get; }

        public TouchActionType Type { set; get; }

        public Point Location { private set; get; }

        public bool IsInContact { private set; get; }
    }
}

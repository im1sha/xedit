using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace XEdit.Core
{
    public class Handler
    {
        public string Name { get; }

        public string ImageUrl { get; }

        /// <summary>
        /// Call this method before Perform to create backup image etc.
        /// </summary>
        public Action Prepare { get; } = async () =>
        {
            await AppDispatcher.Get<ImageManager>().CreateBackupImage();
        };

        /// <summary>
        /// Action on cancellation 
        /// </summary>
        public Action Rollback { get; }

        /// <summary>
        /// Should be called when Handler is selected to set view or to do calculations
        /// </summary>
        public Action Perform { get; }

        /// <summary>
        /// Should be called when Handler is deactivated 
        /// </summary>
        public Action Exit { get; }

        /// <summary>
        /// Save result and exit section 
        /// </summary>
        public Action Commit { get; }

        public Handler(string name, string url,
            Action performAction,    
            Action commitAction,
            Action prepareAction = null,
            Action rollbackAction = null, 
            Action exitAction = null)
        {
            Name = name;
            ImageUrl = url;

            Perform = performAction;
            Commit = commitAction;

            if (prepareAction != null)
            {
                Prepare = prepareAction;
            }

            Rollback = rollbackAction;
            Exit = exitAction;
        }
    }
}

﻿using System;

namespace Cyotek.Windows.Forms
{
    // Cyotek ImageBox
    // Copyright (c) 2010-2014 Cyotek.
    // http://cyotek.com
    // http://cyotek.com/blog/tag/imagebox

    // Licensed under the MIT License. See imagebox-license.txt for the full text.

    // If you use this control in your applications, attribution, donations or contributions are welcome.

    /// <summary>
    /// Describes the zoom action occuring
    /// </summary>
    [Flags]
    public enum ImageBoxZoomActions
    {
        /// <summary>
        /// No action.
        /// </summary>
        None = 0,

        /// <summary>
        /// The control is increasing the zoom.
        /// </summary>
        ZoomIn = 1,

        /// <summary>
        /// The control is decreasing the zoom.
        /// </summary>
        ZoomOut = 2,

        /// <summary>
        /// The control zoom was reset.
        /// </summary>
        ActualSize = 4
    }
}

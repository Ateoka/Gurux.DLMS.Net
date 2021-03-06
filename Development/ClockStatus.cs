﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gurux.DLMS
{
    /// <summary>
    /// Defines Clock status.
    /// </summary>
    [Flags]
    public enum ClockStatus
    {
        /// <summary>
        /// OK.
        /// </summary>
        None = 0,
        /// <summary>
        /// Invalid a value,
        /// </summary>
        InvalidValue = 0x1,
        /// <summary>
        /// Doubtful b value
        /// </summary>
        DoubtfulValue = 0x2,
        /// <summary>
        /// Different clock base c
        /// </summary>
        DifferentClockBase = 0x4,
        /// <summary>
        /// Invalid clock status d
        /// </summary>
        InvalidClockStatus = 0x8,
        /// <summary>
        /// Daylight saving active.
        /// </summary>
        DaylightSavingActive = 0x80,
        /// <summary>
        /// Clock status is skipped on write.
        /// </summary>
        Skip = 0xFF
    }
}

#region PageSummary
// *****************************************************************
// Project:        Logger
// Solution:       Logger
//
// Author:  MAQ Software
// Date:    January 20, 2017
// Description: Class for setting configuration properties for Event Logger
// Change History:
// Name                      Date                       Version         Description
// -------------------------------------------------------------------------------------------------------------
// Swapnil M               January 20, 2017           1.0.0.0         Event Logger Configuration Properties
// -------------------------------------------------------------------------------------------------------------
// Copyright (C) MAQ Software
// -------------------------------------------------------------------------------------------------------------
#endregion
namespace Inventory
{
    /// <summary>
    /// Class used to set configurable properties for Event Logging at run time
    /// </summary>
    public class EventLoggerConfig
    {
        /// <summary>
        /// Gets or sets the source of event
        /// </summary>
        public string EventSource { get; set; }

        /// <summary>
        /// Gets or sets the event log name
        /// </summary>
        public string EventLog { get; set; }

        /// <summary>
        /// Gets or sets the event id
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Constructor used to set configurable properties for Event Logging at run time
        /// </summary>
        /// <param name="source">Event Source (Application)</param>
        /// <param name="log">Event Log (MAQLogger)</param>
        /// <param name="id">Event Id (101)</param>
        public EventLoggerConfig(string source, string log, int id)
        {
            EventSource = source;
            EventLog = log;
            EventId = id;
        }
    }
}

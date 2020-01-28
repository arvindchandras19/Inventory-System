#region PageSummary
// *****************************************************************
// Project:        Logger
// Solution:       Logger
//
// Author:  MAQ Software
// Date:    January 20, 2017
// Description: Class for logging in Event Viewer
// Change History:
// Name                      Date                       Version        Description
// ---------------------------------------------------------------------------------------------------
// Swapnil M               January 20, 2017           1.0.0.0        Class to log in Event Viewer
// ---------------------------------------------------------------------------------------------------
// Copyright (C) MAQ Software
// ---------------------------------------------------------------------------------------------------
#endregion
namespace Inventory
{
    #region Using
    using System;
    using System.Diagnostics;
    #endregion
    /// <summary>
    /// Class for logging in Event Viewer
    /// </summary>
    public class EventLogger : IDisposable
    {
        private EventLog eventLog;
        private string eventSource, eventLogName;
        private int eventId;
        /// <summary>
        /// Constructor to initialize configurable properties
        /// </summary>
        /// <param name="config">Configuration details</param>
        public EventLogger(EventLoggerConfig config)
        {
            // Create an EventLog instance and assign its source
            eventLog = new EventLog();
            // Initialize Event Source, Event Log Name , Event ID
            if (null != config)
            {
                eventSource = config.EventSource;
                eventLogName = config.EventLog;
                eventId = config.EventId;
            }
        }
        /// <summary>
        /// Logs the specified exception type error
        /// </summary>
        /// <param name="exception">Exception received to log an error</param>
        public void LogException(Exception exception)
        {
            LogEventTypeException(LoggingDetails.GetFormattedExceptionString(exception), EventLogEntryType.Error);
        }
        /// <summary>
        /// Logs the specified information type error
        /// </summary>
        /// <param name="message">Message received to log an error</param>      
        public void LogInformation(string message)
        {
            LogEventTypeException(message, EventLogEntryType.Information);
        }
        /// <summary>
        /// Logs the specified warning type error
        /// </summary>
        /// <param name="message">Message received to log an error</param>
        public void LogWarning(string message)
        {
            LogEventTypeException(message, EventLogEntryType.Warning);
        }
        /// <summary>
        /// Method logs error message to table or event viewer
        /// </summary>
        /// <param name="errorMessage">Error message to be logged</param>
        /// <param name="eventLogEntryType">Type of the event log entry</param>
        private void LogEventTypeException(string errorMessage, EventLogEntryType eventLogEntryType)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    if (!EventLog.SourceExists(eventSource))
                    {
                        EventLog.CreateEventSource(eventSource, eventLogName);
                    }
                    // Setting the source
                    this.eventLog.Source = eventSource;
                    // Write an entry to the event log.
                    this.eventLog.WriteEntry(errorMessage, eventLogEntryType, eventId);
                }
            }
            catch(Exception)
            {
                throw; // throw exception to parent
            }
            finally
            {
                this.eventLog.Dispose();
            }
        }
        /// <summary>
        /// Disposable method used to dispose eventLog object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposable method used to dispose eventLog object
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // Free native resources here, if there are any
            if (disposing)
            {
                this.eventLog.Dispose();
            }
        }
    }
}
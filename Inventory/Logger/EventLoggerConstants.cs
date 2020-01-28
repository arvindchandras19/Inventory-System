#region PageSummary
// *****************************************************************
// Project:        Logger
// Solution:       Logger
//
// Author:  MAQ Software
// Date:    January 20, 2017
// Description: Class containing constant values for variables throughout the project 
// Change History:
// Name                      Date                       Version        Description
// ------------------------------------------------------------------------------------------------------------------------------------------
// Swapnil M               January 20, 2017           1.0.0.0        Class containing constant values for variables throughout the project 
// ------------------------------------------------------------------------------------------------------------------------------------------
// Copyright (C) MAQ Software
// ------------------------------------------------------------------------------------------------------------------------------------------
#endregion
namespace Inventory
{
    /// <summary>
    /// Class used to define the literal to be used throughout the API
    /// </summary>
    public static class Constants
    {
      
        #region Error Configurations
        /// <summary>
        ///  String literal for Log level Error
        /// </summary>
        internal const string LOG_LEVEL_ERROR = "Error";

        /// <summary>
        ///  String literal for Log level FailureAudit
        /// </summary>
        internal const string LOG_LEVEL_FAILURE_AUDIT = "FailureAudit";

        /// <summary>
        ///  String literal for Log level Success
        /// </summary>
        internal const string LOG_LEVEL_SUCCESS = "Success";

        /// <summary>
        ///  String literal for Log level Warning
        /// </summary>
        internal const string LOG_LEVEL_WARNING = "Warning";

        /// <summary>
        ///  String literal for Log level INFO
        /// </summary>
        internal const string LOG_LEVEL_INFO = "Info";


        /// <summary>
        /// String literal for Exception Message 
        /// </summary>
        internal const string LOGGER_EXCEPTION_MESSAGE = "Exception Message: {0}.{1}Exception Source: {2}.{1}Exception Inner Exception: {3}.{1}Exception Stack: {4};";
        #endregion
        #region Constants
        /// <summary>
        /// String Place holders
        /// </summary>
        internal const string PLACE_HOLDER = "{0} - {1}";
        internal const string OPENING_SQUARE_BRACKET = "[";
        internal const string CLOSING_SQUARE_BRACKET = "]";
        internal const string SPACE = " ";
        internal const string COLON = ":";
        #endregion
    }
}
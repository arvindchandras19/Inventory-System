#region PageSummary
// *****************************************************************
// Project:        Logger
// Solution:       Logger
//
// Author:  MAQ Software
// Date:    January 20, 2017
// Description: Factory class implemented to wrap the Logger Interface
// Change History:
// Name                      Date                       Version        Description
// --------------------------------------------------------------------------------------------------
// Swapnil M               January 20, 2017           1.0.0.0        Logger Abstract class
// --------------------------------------------------------------------------------------------------
// Copyright (C) MAQ Software
// --------------------------------------------------------------------------------------------------
#endregion

namespace Inventory
{
    #region Using
    using System;
    using System.Globalization;
    using System.Text;
    #endregion
    /// <summary>
    /// Logger abstract class for Exception and Message handling
    /// </summary>
    public static class LoggingDetails
    {
        /// <summary>
        /// Returns exception details
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal static string GetFormattedExceptionString(Exception exception)
        {
            string returnString = string.Empty;
            if (null != exception)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat(CultureInfo.InvariantCulture,
                                Constants.LOGGER_EXCEPTION_MESSAGE,
                                exception.Message,             //0
                                Environment.NewLine,           //1
                                exception.Source,              //2
                                exception.InnerException,      //3
                                exception.StackTrace           //4
                               );
                returnString = Convert.ToString(stringBuilder, CultureInfo.InvariantCulture);
                return returnString;
            }
            else
            {
                return returnString;
            }
        }
    }
}
using System;

namespace CGP.Foundation.ErrorModule.Repositiories
{
    public interface ILogger
    {
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        void LogError(string message, Exception ex);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogInfo(string message);

     
        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        void LogWarning(string message);

       
        /// <summary>
        /// Checks if debug mode is on or not
        /// </summary>
        /// <param name=""></param>
        bool IsDebugMode();
    }
}

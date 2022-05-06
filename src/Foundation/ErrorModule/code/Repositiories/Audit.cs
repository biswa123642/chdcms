using log4net;
using System.Diagnostics.CodeAnalysis;

namespace CGP.Foundation.ErrorModule.Repositiories
{

    /// <summary>
    /// Class to log all audit related data
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Audit : IAudit
    {
        public ILog _logger;

        /// <summary>
        /// Default class constructor
        /// </summary>
        public Audit()
        {
            _logger = LogManager.GetLogger(PlatformConstants.LogFile.DefaultAuditLogFile);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">Error or info message to log in file</param>
        public void Info(string message)
        {
            _logger.Info(UpdateMessage(message));
        }
        /// <summary>
        /// Passing method current state
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string UpdateMessage(string message)
        {
            string requestUrl = (Sitecore.Context.Request != null && Sitecore.Context.Request.ItemPath != null) ? Sitecore.Context.Request.ItemPath : string.Empty;
            string requestLanguage = (Sitecore.Context.Language != null && Sitecore.Context.Language.CultureInfo != null) ? Sitecore.Context.Language.CultureInfo.ToString() : string.Empty;
            message = Sitecore.Context.Site.Name.ToUpper() + " : " + requestUrl + " : " + requestLanguage + " : " + message;
            return message;
        }
    }
}
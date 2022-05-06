using log4net;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;
using Sitecore;
using System.Net;
using System.Net.Sockets;

namespace CGP.Foundation.ErrorModule.Repositiories
{
    [ExcludeFromCodeCoverage]
    public class Logger : ILogger
    {
        private readonly ILog _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            _logger = LogManager.GetLogger(PlatformConstants.LogFile.DefaultLogFile);
        }

        /// <summary>
        /// MiralCustomDefaultLogFileAppender
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public void LogError(string message, Exception ex)
        {
            message = UpdateMessage(message);
            string updatedMessage;
            updatedMessage = (message == "") ? (ex != null ? UpdateMessage(ex.Message) : string.Empty) : message;
            _logger.Error(updatedMessage, ex);
        }

        private string UpdateMessage(string message)
        {
            string requestUrl = (Context.Request != null && Sitecore.Context.Request.ItemPath != null) ? Context.Request.ItemPath : string.Empty;
            string requestLanguage = (Context.Language != null && Context.Language.CultureInfo != null) ? Context.Language.CultureInfo.ToString() : string.Empty;
            string date = DateTime.Today.Date.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("HH:mm:ss");
            if (HttpContext.Current != null && HttpContext.Current.Response != null)
            {
                string siteHostName = System.Convert.ToString(HttpContext.Current.Request.Url.Host);
                string hostIpAddress = GetHostIPAddress();
                string hostMachineName = System.Convert.ToString(Dns.GetHostName());
                string rawUrl = System.Convert.ToString(HttpContext.Current.Request.Url.AbsoluteUri);
                string clientIpAddress = System.Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                string clientHostName = System.Convert.ToString(HttpContext.Current.Request.UserHostName);
                var sessionId = HttpContext.Current != null && HttpContext.Current.Session != null ? HttpContext.Current.Session.SessionID : string.Empty;
                return (Context.Site != null && Context.Site.Name != null) ?
                Context.Site.Name.ToUpper() + " : " + requestUrl + " : " + requestLanguage + " : " + message + "\n Session Id : " + sessionId
                + "\n Date : " + date + "\n Time : " + time + "\n Site Host Name : " + siteHostName + "\n Host IP Address : " + hostIpAddress
                + "\n Host Machine Name : " + hostMachineName + "\n Raw Url : " + rawUrl + "\n Client IP Address : " + clientIpAddress
                + "\n Client Host Name : " + clientHostName
                : "SiteNameNotFound : " + message;
            }
            else
            {
                return (Context.Site != null && Context.Site.Name != null) ?
                Context.Site.Name.ToUpper() + " : " + requestUrl + " : " + requestLanguage + " : " + message
                + "\n Date : " + date + "\n Time : " + time : "SiteNameNotFound : " + message;
            }
        }

        private string GetHostIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogInfo(string message)
        {
            _logger.Info(UpdateMessage(message));
        }

        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public void LogDebug(string message, Exception ex)
        {
            string updatedMessage;
            updatedMessage = (message == "") ? (ex != null ? UpdateMessage(ex.Message) : string.Empty) : UpdateMessage(message);
            _logger.Debug(updatedMessage, ex);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogWarning(string message)
        {
            _logger.Warn(UpdateMessage(message));
        }

        public void LogDebug(string message)
        {
            _logger.Debug(UpdateMessage(message));
        }

        public bool IsDebugMode()
        {
            return _logger.IsDebugEnabled;
        }
    }
}
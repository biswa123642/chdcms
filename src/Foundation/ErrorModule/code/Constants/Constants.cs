using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace CGP.Foundation.ErrorModule
{
    [ExcludeFromCodeCoverage]
    public static class PlatformConstants
    {
        public static class LogFile
        {
            public static readonly string DefaultLogFile = "Sitecore.Diagnostics.CustomDefaultLogFileAppender";
            public static readonly string DefaultAuditLogFile = "LogFileName";
        }
    }
}
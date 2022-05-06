using System.Web;
using Sitecore.Caching;

namespace CGP.Foundation.SitecoreExtensions.Extensions
{
    public class CacheExtensions
    {
        private static Cache _oneWebCache = new Cache("OneWeb Cache", 1024000);
        public static bool SetBooleanCacheVariable(string strSessionName, bool blnValue)
        {
            _oneWebCache.Add(strSessionName, blnValue);
            return blnValue;
        }

        public static void SetObjectCacheVariable(string strSessionName, object value)
        {
            _oneWebCache.Add(strSessionName, value);
        }

        public static object GetObjectCacheVariable(string strSessionName)
        {
            return _oneWebCache.GetValue(strSessionName);
        }

        public static string SetStringCacheVariable(string strSessionName, string strValue)
        {
            _oneWebCache.Add(strSessionName, strValue);
            return strValue;
        }

        public static string GetStringCacheVariable(string strSessionName)
        {
            var stringVariable = _oneWebCache.GetValue(strSessionName);
            return stringVariable?.ToString();
        }

        public static bool? GetBooleanCacheVariable(string strSessionName)
        {
            var sessionValue = _oneWebCache.GetValue(strSessionName);
            if (sessionValue != null)
            {
                return (bool)sessionValue;
            }
            return null;
        }
    }

}
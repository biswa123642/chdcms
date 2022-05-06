using Scriban.Runtime;
using Sitecore.XA.Foundation.Scriban.Pipelines.GenerateScribanContext;

namespace CGP.Foundation.SitecoreExtensions.Pipelines.Scriban
{
    public class GetSitecoreSettings : IGenerateScribanContextProcessor
    {
        public void Process(GenerateScribanContextPipelineArgs args)
        {
            var getSetting = new delegateGetSetting(GetSettings);
            args.GlobalScriptObject.Import("sc_get_settings", getSetting);
        }
        public string GetSettings(string settingName)
        {
            return Sitecore.Configuration.Settings.GetSetting(settingName);
        }

        private delegate string delegateGetSetting(string settingName);
    }
}
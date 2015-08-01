using System;
using System.Collections.Specialized;
using System.Configuration;

namespace TinyFeed
{
    public class NuGetWebApiSettings : INuGetWebApiSettings
    {
        public const string DefaultAppSettingPrefix = "NuGet.Lucene.Web:";
        public const string DefaultRoutePathPrefix = "api/";
        private readonly string prefix;
        private readonly NameValueCollection settings;

        public NuGetWebApiSettings()
            : this(DefaultAppSettingPrefix)
        {
        }

        public NuGetWebApiSettings(string prefix)
            : this(prefix, ConfigurationManager.AppSettings)
        {
        }

        public NuGetWebApiSettings(string prefix, NameValueCollection settings)
        {
            this.prefix = prefix;
            this.settings = settings;
        }

        public bool ShowExceptionDetails
        {
            get { return GetFlagFromAppSetting("showExceptionDetails", false); }
        }

        public bool EnableCrossDomainRequests
        {
            get { return GetFlagFromAppSetting("enableCrossDomainRequests", false); }
        }

        public string RoutePathPrefix
        {
            get { return GetAppSetting("routePathPrefix", DefaultRoutePathPrefix); }
        }

        private bool GetFlagFromAppSetting(string key, bool defaultValue)
        {
            var flag = GetAppSetting(key, String.Empty);

            bool result;
            return Boolean.TryParse(flag ?? String.Empty, out result) ? result : defaultValue;
        }

        private string GetAppSetting(string key, string defaultValue)
        {
            var value = settings[GetAppSettingKey(key)];
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        private string GetAppSettingKey(string key)
        {
            return prefix + key;
        }
    }
}
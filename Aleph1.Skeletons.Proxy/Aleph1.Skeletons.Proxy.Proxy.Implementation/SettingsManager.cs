using System;
#if NET48
using System.Configuration;
#else
using Microsoft.Extensions.Configuration;
#endif

namespace Aleph1.Skeletons.Proxy.Proxy.Implementation
{
    internal static class SettingsManager
    {
        private static Uri _ServiceBaseUrl;
#if !NET48
		private static IConfiguration? _configuration;

		public static void Configure(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
#endif

        public static Uri ServiceBaseUrl
        {
            get
            {
                if (_ServiceBaseUrl == default)
                {
					string baseUrlString = GetSetting("ServiceBaseUrl");
                    if (string.IsNullOrWhiteSpace(baseUrlString))
                    {
                        throw new ArgumentNullException("ServiceBaseUrl");
                    }

                    if (!baseUrlString.EndsWith("/", StringComparison.InvariantCulture))
                    {
                        baseUrlString += '/';
                    }

                    _ServiceBaseUrl = new Uri(baseUrlString);
                }
                return _ServiceBaseUrl;
            }
        }

#if NET48
		private static string GetSetting(string key) => ConfigurationManager.AppSettings[key];
#else
		private static string GetSetting(string key)
		{
			if (_configuration == null)
			{
				throw new InvalidOperationException("SettingsManager.Configure must be called before accessing settings when targeting .NET.");
			}

			return _configuration[key] ?? string.Empty;
		}
#endif
    }
}

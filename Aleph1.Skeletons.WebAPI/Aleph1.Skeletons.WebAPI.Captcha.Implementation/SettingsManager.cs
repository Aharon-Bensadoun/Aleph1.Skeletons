using System;
#if NET48
using System.Configuration;
#else
using Microsoft.Extensions.Configuration;
#endif

namespace Aleph1.Skeletons.WebAPI.Captcha.Implementation
{
	/// <summary>Handle settings from web.config</summary>
	internal static class SettingsManager
	{
		private static Uri _captchaAPIUrl;
#if !NET48
		private static IConfiguration? _configuration;

		public static void Configure(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
#endif
		public static Uri CaptchaAPIUrl
		{
			get
			{
				if (_captchaAPIUrl == default)
				{
					_captchaAPIUrl = new Uri(GetSetting("CaptchaAPIUrl"));
				}
				return _captchaAPIUrl;
			}
		}

		private static string _captchaSecret;
		public static string CaptchaSecret
		{
			get
			{
				if (_captchaSecret == default)
				{
					_captchaSecret = GetSetting("CaptchaSecret");
				}
				return _captchaSecret;
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

using System;
using System;
using System.Configuration;
using System.Globalization;
#if !NET48
using Microsoft.Extensions.Configuration;
#endif

namespace Aleph1.Skeletons.WebAPI.Security.Implementation
{
	/// <summary>Handle settings from web.config</summary>
	internal static class SettingsManager
	{
		// randomly generated GUID for each project - you can change this to whatever you want
		public static string AppPrefix => "{5BEE28FC-635A-4BB3-A82F-611BB51900F9}";

		private static int? _ticketDurationMin;
		private static TimeSpan? _ticketDurationTimeSpan;
#if !NET48
		private static IConfiguration? _configuration;

		public static void Configure(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
#endif
		public static TimeSpan? TicketDurationTimeSpan
		{
			get
			{
				if (_ticketDurationMin == default)
				{
					_ticketDurationMin = int.Parse(GetSetting("TicketDurationMin"), CultureInfo.InvariantCulture);
					if (_ticketDurationMin.Value != 0)
					{
						_ticketDurationTimeSpan = TimeSpan.FromMinutes(_ticketDurationMin.Value);
					}
				}
				return _ticketDurationTimeSpan;
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

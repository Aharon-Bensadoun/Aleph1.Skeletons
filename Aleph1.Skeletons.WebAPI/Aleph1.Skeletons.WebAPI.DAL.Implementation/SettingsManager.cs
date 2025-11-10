using System;
#if NET48
using System.Configuration;
#else
using Microsoft.Extensions.Configuration;
#endif

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace Aleph1.Skeletons.WebAPI.DAL.Implementation
{
	internal static class SettingsManager
	{
		private static readonly Lazy<DbContextOptions<GenericContext>> lazyDbOptions = new(CreateDbContextOptions);
		public static DbContextOptions<GenericContext> DBOptions => lazyDbOptions.Value;

#if !NET48
		private static IConfiguration? _configuration;

		public static void Configure(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
#endif

		private static DbContextOptions<GenericContext> CreateDbContextOptions()
			=> new DbContextOptionsBuilder<GenericContext>()
				.UseSqlServer(GenericContextConnectionString)
				.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
				.Options;

		private static string _genericContextConnectionString;
		public static string GenericContextConnectionString
		{
			get
			{
				if (_genericContextConnectionString == default)
				{
					_genericContextConnectionString = GetSetting("GenericContextConnectionString");
				}
				return _genericContextConnectionString;
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

	internal sealed class GenericContextFactory : IDesignTimeDbContextFactory<GenericContext>
	{
		public GenericContext CreateDbContext(string[] args)
		{
			return new GenericContext(SettingsManager.DBOptions);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Aleph1.Logging;
using Aleph1.Skeletons.WebAPI.Captcha.Contracts;

namespace Aleph1.Skeletons.WebAPI.Captcha.Implementation
{
	internal sealed class GoogleRecaptcha : ICaptcha, IDisposable
	{
		private readonly HttpClient httpClient;
		public GoogleRecaptcha() => httpClient = new HttpClient();

		public void Dispose()
		{
			httpClient.Dispose();
		}

		[Logged]
		public async Task ValidateCaptcha(string captchaToken)
		{
			Dictionary<string, string> bodyUrlEncoded = new()
			{
				{ "secret", SettingsManager.CaptchaSecret },
				{ "response", captchaToken }
			};
			using FormUrlEncodedContent content = new(bodyUrlEncoded);
			HttpResponseMessage response = await httpClient.PostAsync(SettingsManager.CaptchaAPIUrl, content)
				.ConfigureAwait(false);
			CaptchaResponse? result = await response.Content.ReadFromJsonAsync<CaptchaResponse>()
				.ConfigureAwait(false);
			if (result == null)
			{
				throw new InvalidOperationException("Captcha verification returned an empty response.");
			}
			if (!result.Success)
			{
				throw new UnauthorizedAccessException(string.Join(",", result.ErrorCodes));
			}
		}
	}
}

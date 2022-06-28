using CardanoSharp.Blazor.Components.Interfaces;
using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components.JSInterop
{
	public class WebWalletInitialApi : IWebWalletInitialApi
	{
		private IJSRuntime _js;
		private string _webWalletName;

		public WebWalletInitialApi(IJSRuntime js, string webWalletName)
		{
			if (js == null)
				throw new ArgumentNullException(nameof(js));
			if (webWalletName == null || webWalletName.Length == 0)
				throw new ArgumentNullException(nameof(webWalletName));

			_js = js;
			_webWalletName = webWalletName;
		}

		public async ValueTask<IJSObjectReference> Enable()
		{
			return await _js.InvokeAsync<IJSObjectReference>($"cardano.{_webWalletName}.enable");
		}

		public async ValueTask<bool> IsEnabled()
		{
			return await _js.InvokeAsync<bool>($"cardano.{_webWalletName}.isEnabled");
		}

		public async ValueTask<string> ApiVersion()
		{
			return await _js.InvokeAsync<string>($"cardano.{_webWalletName}.apiVersion");
		}

		public async ValueTask<string> Name()
		{
			return await _js.InvokeAsync<string>($"cardano.{_webWalletName}.name");
		}

		public async ValueTask<string> Icon()
		{
			return await _js.InvokeAsync<string>($"cardano.{_webWalletName}.icon");
		}
	}
}
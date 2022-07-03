using CardanoSharp.Blazor.Components.Interfaces;
using CardanoSharp.Blazor.Components.JSInterop;
using CardanoSharp.Blazor.Components.Models;
using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components
{
	public class PluginState : WalletExtension
	{
		public bool Installed { get; }

		public IWebWalletInitialApi InitialApi { get; }

		public PluginState(WalletExtension extension, bool installed, IWebWalletInitialApi initialApi) : base(extension)
		{
			if (extension == null) throw new ArgumentNullException(nameof(extension));
			if (initialApi == null) throw new ArgumentNullException(nameof(initialApi));

			Installed = installed;
			InitialApi = initialApi;
		}

		public async ValueTask<WalletState?> ConnectAsync()
		{
			if (!Installed) return null;
			try
			{
				var apiJsObj = await InitialApi.Enable().ConfigureAwait(false);
				if (apiJsObj == null) return null;
				var api = new WebWalletApi(apiJsObj);
				var wallet = new WalletState(this, api);
				await wallet.InitAsync();
				return wallet;
			}
			catch
			{
				//valid user refusal can cause exception
				return null;
			}
		}
	}
}
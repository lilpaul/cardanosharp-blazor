using CardanoSharp.Blazor.Components.Interfaces;
using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components.JSInterop
{
    public class WebWalletInitialApi : IWebWalletInitialApi
    {
        private IJSRuntime _js;
        private string _walletKey;

        public WebWalletInitialApi(IJSRuntime js, string walletKey)
        {
            if (js == null)
                throw new ArgumentNullException(nameof(js));
            if (walletKey == null || walletKey.Length == 0)
                throw new ArgumentNullException(nameof(walletKey));

            _js = js;
            _walletKey = walletKey;
        }

        public async ValueTask<IJSObjectReference> Enable()
        {
            return await _js.InvokeAsync<IJSObjectReference>($"cardano.{_walletKey}.enable");
        }

        public async ValueTask<bool> IsEnabled()
        {
            return await _js.InvokeAsync<bool>($"cardano.{_walletKey}.isEnabled");
        }

        public async ValueTask<string> ApiVersion()
        {
            return await _js.InvokeAsync<string>($"cardano.{_walletKey}.apiVersion");
        }

        public async ValueTask<string> Name()
        {
            return await _js.InvokeAsync<string>($"cardano.{_walletKey}.name");
        }

        public async ValueTask<string> Icon()
        {
            return await _js.InvokeAsync<string>($"cardano.{_walletKey}.icon");
        }
    }
}
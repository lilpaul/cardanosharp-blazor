using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components
{
    public class WebWalletConnectorJsInterop : IAsyncDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
        private IJSObjectReference? _webWalletConnectorInteropObj;

        public WebWalletConnectorJsInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _moduleTask = new(() => _jsRuntime.InvokeAsync<IJSObjectReference>(
               "import", "./_content/CardanoSharp.Blazor.Components/WalletConnectorJsInterop.js").AsTask());
        }

        public async ValueTask Init(DotNetObjectReference<WalletConnector> webWalletConnectorObj)
        {
            var module = await _moduleTask.Value;
            _webWalletConnectorInteropObj = await module.InvokeAsync<IJSObjectReference>("createWebWalletConnectorInteropObj");

            await _webWalletConnectorInteropObj.InvokeVoidAsync("init", webWalletConnectorObj);
        }

        public async ValueTask<bool> IsWalletInstalled(string key)
        {
            CheckInitialized();
            return await _webWalletConnectorInteropObj!.InvokeAsync<bool>("isWalletInstalled", key);
        }

        private void CheckInitialized()
        {
            if (_webWalletConnectorInteropObj == null)
                throw new InvalidOperationException("Cannot call methods before Init has run successfully");
        }

        public async ValueTask DisposeAsync()
        {
            if (_webWalletConnectorInteropObj != null)
            {
                await _webWalletConnectorInteropObj.DisposeAsync();
            }

            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}
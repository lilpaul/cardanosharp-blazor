using CardanoSharp.Blazor.Components.Interfaces;
using CardanoSharp.Blazor.Components.Models;
using Microsoft.JSInterop;

namespace CardanoSharp.Blazor.Components.JSInterop
{
	public class WebWalletApi : IWebWalletApi
	{
		private IJSObjectReference _jsWalletObj;

		public WebWalletApi(IJSObjectReference jsWalletObj)
		{
			if (jsWalletObj == null)
				throw new ArgumentNullException(nameof(jsWalletObj));

			_jsWalletObj = jsWalletObj;
		}

		public async ValueTask<int> GetNetworkId()
		{
			return await _jsWalletObj.InvokeAsync<int>($"getNetworkId");
		}

		public async ValueTask<string[]> GetUtxos(string? amount = null, Paginate? paginate = null)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getUtxos", amount, paginate);
		}

		public async ValueTask<string[]> GetCollateral(CollateralParams collateralParams)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getCollateral", collateralParams);
		}

		public async ValueTask<string> GetBalance()
		{
			return await _jsWalletObj.InvokeAsync<string>($"getBalance");
		}

		public async ValueTask<string[]> GetUsedAddresses(Paginate? paginate = null)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getUsedAddresses", paginate);
		}

		public async ValueTask<string[]> GetUnusedAddresses()
        {
			return await _jsWalletObj.InvokeAsync<string[]>($"getUnusedAddresses");
		}

		public async ValueTask<string> GetChangeAddress()
		{
			return await _jsWalletObj.InvokeAsync<string>($"getChangeAddress");
		}

		public async ValueTask<string[]> GetRewardAddresses()
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getRewardAddresses");
		}

		public async ValueTask<string> SignTx(string tx, bool partialSign = false)
		{
			return await _jsWalletObj.InvokeAsync<string>($"signTx", tx, partialSign);
		}

		public async ValueTask<DataSignature> SignData(string addr, string payload)
		{
			return await _jsWalletObj.InvokeAsync<DataSignature>($"signData", addr, payload);
		}

		public async ValueTask<string> SubmitTx(string tx)
		{
			return await _jsWalletObj.InvokeAsync<string>($"submitTx", tx);
		}
	}
}
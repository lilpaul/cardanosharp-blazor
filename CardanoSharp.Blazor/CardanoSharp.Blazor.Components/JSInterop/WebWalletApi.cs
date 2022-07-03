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

		public async Task<int> GetNetworkId()
		{
			return await _jsWalletObj.InvokeAsync<int>($"getNetworkId");
		}

		public async Task<string[]> GetUtxos(string? amount = null, Paginate? paginate = null)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getUtxos", amount, paginate);
		}

		public async Task<string[]> GetCollateral(CollateralParams collateralParams)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getCollateral", collateralParams);
		}

		public async Task<string> GetBalance()
		{
			return await _jsWalletObj.InvokeAsync<string>($"getBalance");
		}

		public async Task<string[]> GetUsedAddresses(Paginate? paginate = null)
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getUsedAddresses", paginate);
		}

		public async Task<string[]> GetUnusedAddresses()
        {
			return await _jsWalletObj.InvokeAsync<string[]>($"getUnusedAddresses");
		}

		public async Task<string> GetChangeAddress()
		{
			return await _jsWalletObj.InvokeAsync<string>($"getChangeAddress");
		}

		public async Task<string[]> GetRewardAddresses()
		{
			return await _jsWalletObj.InvokeAsync<string[]>($"getRewardAddresses");
		}

		public async Task<string> SignTx(string tx, bool partialSign = false)
		{
			return await _jsWalletObj.InvokeAsync<string>($"signTx", tx, partialSign);
		}

		public async Task<DataSignature> SignData(string addr, string payload)
		{
			return await _jsWalletObj.InvokeAsync<DataSignature>($"signData", addr, payload);
		}

		public async Task<string> SubmitTx(string tx)
		{
			return await _jsWalletObj.InvokeAsync<string>($"submitTx", tx);
		}
	}
}
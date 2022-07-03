using CardanoSharp.Blazor.Components.Interfaces;
using CardanoSharp.Blazor.Components.Models;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Models.Transactions;
using System.Globalization;

namespace CardanoSharp.Blazor.Components
{
	public class WalletState : WalletExtension
	{
		public IWebWalletApi Api { get; }

		public NetworkType Network { get; }

		public string ChangeAddress { get; private set; } = "";

		public ulong Balance { get; private set; }

		public ulong TokenPreservation { get; private set; }

		public List<Asset> Assets { get; private set; } = new List<Asset>();

		public bool Initialized
		{
			get
			{
				return !string.IsNullOrEmpty(ChangeAddress);
			}
		}

		public WalletState(WalletExtension walletExtension, IWebWalletApi api)
		{
			if (walletExtension == null) throw new ArgumentNullException(nameof(walletExtension));
			if (api == null) throw new ArgumentNullException(nameof(api));
			Api = api;
		}

		public async ValueTask InitAsync()
		{
			ChangeAddress = await Api.GetChangeAddress().ConfigureAwait(false);
			await RefreshBalanceAsync();
			return;
		}

		public async ValueTask RefreshBalanceAsync()
		{
			var balanceCbor = await Api.GetBalance().ConfigureAwait(false);
			var balanceValue = balanceCbor.HexToByteArray().DeserializeTransactionOutputValue();
			TokenPreservation = balanceValue.MultiAsset.CalculateMinUtxoLovelace();
			Balance = balanceValue.Coin;
			Assets.Clear();
			if (balanceValue.MultiAsset != null && balanceValue.MultiAsset.Count > 0)
			{
				balanceValue.MultiAsset.SelectMany(p => p.Value.Token.Select(a => new Asset()
				{
					PolicyId = p.Key.ToStringHex(),
					Name = a.Key.ToString(),
					Quantity = a.Value
				})).ToList();
			}
		}

		public async ValueTask<bool> ConnectedWalletChanged()
		{
			var apiChangeAddress = await Api.GetChangeAddress().ConfigureAwait(false);
			return !String.Equals(apiChangeAddress, ChangeAddress, StringComparison.OrdinalIgnoreCase);
		}

		public string BalanceAdaPortion
		{
			get
			{
				var temp = (Balance / 1000000).ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
				return temp.Substring(0, temp.IndexOf('.'));
			}
		}

		public string BalanceLovelacePortion
		{
			get
			{
				return (Balance % 1000000).ToString("D6");
			}
		}

		public string TokenPreservationAdaPortion
		{
			get
			{
				var temp = (TokenPreservation / 1000000).ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
				return temp.Substring(0, temp.IndexOf('.'));
			}
		}

		public string TokenPreservationLovelacePortion
		{
			get
			{
				return (TokenPreservation % 1000000).ToString("D6");
			}
		}

		public string Currency
		{
			get
			{
				if (Network == NetworkType.Mainnet)
				{
					return "₳";
				}
				else if (Network == NetworkType.Testnet)
				{
					return "t₳";
				}
				return "";
			}
		}
	}
}
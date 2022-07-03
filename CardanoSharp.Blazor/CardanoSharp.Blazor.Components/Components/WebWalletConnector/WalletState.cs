using CardanoSharp.Blazor.Components.Interfaces;
using CardanoSharp.Blazor.Components.Models;
using CardanoSharp.Blazor.Components.Utils;
using CardanoSharp.Wallet.Enums;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Models.Addresses;
using CardanoSharp.Wallet.Models.Transactions;
using System.Globalization;

namespace CardanoSharp.Blazor.Components
{
	public class WalletState : WalletExtension
	{
		public IWebWalletApi Api { get; }

		public NetworkType Network { get; private set; }

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

		public WalletState(WalletExtension walletExtension, IWebWalletApi api) : base(walletExtension)
		{
			if (walletExtension == null) throw new ArgumentNullException(nameof(walletExtension));
			if (api == null) throw new ArgumentNullException(nameof(api));
			Api = api;
		}

		public async ValueTask RefreshWalletAsync()
		{
			var networkId = await Api.GetNetworkId();
			Network = GetNetworkFromId(networkId);
			var changeAddressHex = await Api.GetChangeAddress().ConfigureAwait(false);
			ChangeAddress = GetAddressBech(Network, changeAddressHex);
			var balanceValue = new TransactionOutputValue();
			var balanceCbor = await Api.GetBalance().ConfigureAwait(false);
			if (!string.IsNullOrEmpty(balanceCbor) && balanceCbor != "00")
			{
				balanceValue = balanceCbor.HexToByteArray().DeserializeTransactionOutputValue();
			}

			TokenPreservation = 0;
			if (balanceValue.MultiAsset != null && balanceValue.MultiAsset.Count > 0)
			{
				TokenPreservation = balanceValue.MultiAsset.CalculateMinUtxoLovelace();
			}

			Balance = balanceValue.Coin;

			Assets.Clear();
			if (balanceValue.MultiAsset != null && balanceValue.MultiAsset.Count > 0)
			{
				Assets.AddRange(
					balanceValue.MultiAsset.SelectMany(p => p.Value.Token.Select(a => new Asset()
					{
						PolicyId = p.Key.ToStringHex(),
						Name = a.Key.ToString(),
						Quantity = a.Value
					})).ToList()
					);
			}
		}

		private string GetAddressBech(NetworkType network, string addressHex)
		{
			return new Address(
				ComponentUtils.GetPrefix(AddressType.Base, Network),
				addressHex.HexToByteArray()
			).ToString();
		}

		private NetworkType GetNetworkFromId(int networkId)
		{
			switch (networkId)
			{
				case 0: return NetworkType.Testnet;
				case 1: return NetworkType.Mainnet;
				default: return NetworkType.Unknown;
			}
		}

	}
}
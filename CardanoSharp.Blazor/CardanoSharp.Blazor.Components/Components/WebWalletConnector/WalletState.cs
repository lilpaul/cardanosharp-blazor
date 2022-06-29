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

        public string ChangeAddress { get; } = "";

        public ulong Balance { get; }

        public ulong TokenPreservation { get; }

        public List<Asset> Assets { get; } = new List<Asset>();

        public WalletState(WalletExtension walletExtension, IWebWalletApi api, string balanceCbor, string changeAddress)
        {
            if (walletExtension == null) throw new ArgumentNullException(nameof(walletExtension));
            if (api == null) throw new ArgumentNullException(nameof(api));
            if (balanceCbor == null) throw new ArgumentNullException(nameof(balanceCbor));
            if (balanceCbor.Length < 2) throw new ArgumentException(nameof(balanceCbor), "must be at least 2 characters long");
            if (changeAddress == null) throw new ArgumentNullException(nameof(changeAddress));
            if (changeAddress.Length < 10) throw new ArgumentException(nameof(changeAddress), "must be at least 10 characters long");

            Api = api;
            ChangeAddress = changeAddress;
            var balanceValue = balanceCbor.HexToByteArray().DeserializeTransactionOutputValue();
            TokenPreservation = balanceValue.MultiAsset.CalculateMinUtxoLovelace();
            Balance = balanceValue.Coin;
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
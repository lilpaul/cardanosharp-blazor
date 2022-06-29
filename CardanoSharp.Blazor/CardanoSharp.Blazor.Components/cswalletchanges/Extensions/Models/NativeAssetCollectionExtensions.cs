using CardanoSharp.Wallet.Models.Transactions;

namespace CardanoSharp.Wallet.Extensions
{
    public static class NativeAssetCollectionExtensions
    {
        public static IEnumerable<Asset> ToAssetList(this Dictionary<byte[], NativeAsset> tokens)
        {
            return tokens.SelectMany(
                x => x.Value.Token.Select(
                    y => new Asset() { PolicyId = x.Key.ToStringHex(), Name = y.Key.ToStringHex(), Quantity = y.Value }
                    )
                );
        }
    }
}
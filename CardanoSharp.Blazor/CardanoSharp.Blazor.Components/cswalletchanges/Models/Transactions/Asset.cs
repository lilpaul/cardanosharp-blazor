namespace CardanoSharp.Wallet.Models.Transactions
{
    public class Asset
    {
        public string? PolicyId { get; set; }

        public string? Name { get; set; }

        public ulong Quantity { get; set; }
    }
}
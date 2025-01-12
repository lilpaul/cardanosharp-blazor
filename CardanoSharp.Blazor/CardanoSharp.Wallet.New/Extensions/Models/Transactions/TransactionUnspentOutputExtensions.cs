﻿using CardanoSharp.Wallet.Models.Transactions;
using PeterO.Cbor2;

namespace CardanoSharp.Wallet.Extensions.Models.Transactions
{
    public static class TransactionUnspentOutputExtensions
    {
        public static CBORObject GetCBOR(this TransactionUnspentOutput transactionUnspentOutput)
        {
            return CBORObject.NewArray()
                .Add(transactionUnspentOutput.Input.GetCBOR())
                .Add(transactionUnspentOutput.Output.GetCBOR());
        }

        public static TransactionUnspentOutput GetTransactionUnspentOutput(this CBORObject transactionUnspentOutputCbor)
        {
            //validation
            if (transactionUnspentOutputCbor == null)
            {
                throw new ArgumentNullException(nameof(transactionUnspentOutputCbor));
            }
            if (transactionUnspentOutputCbor.Type != CBORType.Array)
            {
                throw new ArgumentException("transactionUnspentOutputCbor is not expected type CBORType.Array");
            }
            if (transactionUnspentOutputCbor.Values.Count != 2)
            {
                throw new ArgumentException("transactionInputCbor unexpected number elements (expected 2)");
            }

            //get data
            var utxo = new TransactionUnspentOutput();
            utxo.Input = transactionUnspentOutputCbor[0].GetTransactionInput();
            utxo.Output = transactionUnspentOutputCbor[1].GetTransactionOutput();

            //return
            return utxo;
        }

        public static byte[] Serialize(this TransactionUnspentOutput transactionUnspentOutput)
        {
            return transactionUnspentOutput.GetCBOR().EncodeToBytes();
        }

        public static TransactionUnspentOutput DeserializeTransactionUnspentOutput(this byte[] bytes)
        {
            return CBORObject.DecodeFromBytes(bytes).GetTransactionUnspentOutput();
        }

    }
}
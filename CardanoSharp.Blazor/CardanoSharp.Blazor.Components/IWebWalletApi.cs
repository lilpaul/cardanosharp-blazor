﻿using CardanoSharp.Blazor.Components.Models;

namespace CardanoSharp.Blazor.Components
{
	/// <summary>
	/// CIP30 Compliant full interface as per https://cips.cardano.org/cips/cip30/
	/// </summary>
	public interface IWebWalletApi
	{
		/// <summary>
		/// api.getNetworkId(): Promise\
		/// Errors: APIError
		/// Returns the network id of the currently connected account. 0 is testnet and 1 is mainnet but other networks
		/// can possibly be returned by wallets. Those other network ID values are not governed by this document.
		/// This result will stay the same unless the connected account has changed.
		/// </summary>
		/// <returns></returns>
		ValueTask<int> GetNetworkId();

		/// <summary>
		/// api.getUtxos(amount: cbor\ = undefined, paginate: Paginate = undefined): Promise\
		/// Errors: APIError, PaginateError
		/// If amount is undefined, this shall return a list of all UTXOs(unspent transaction outputs) controlled
		/// by the wallet.If amount is not undefined, this request shall be limited to just the UTXOs that are required
		/// to reach the combined ADA/multiasset value target specified in amount, and if this cannot be attained,
		/// null shall be returned.The results can be further paginated by paginate if it is not undefined.
		/// </summary>
		/// <param name="amount">A hex-encoded string representing CBOR</param>
		/// <param name="paginate"></param>
		/// <returns></returns>
		ValueTask<string[]> GetUtxos(string? amount = null, Paginate? paginate = null);

		/// <summary>
		/// api.getCollateral(params: { amount: cbor\ }): Promise\
		/// Errors: APIError
		/// The function takes a required object with parameters. With a single required parameter for now: amount.
		/// (NOTE: some wallets may be ignoring the amount parameter, in which case it might be possible to call the
		/// function without it, but this behavior is not recommended!).
		/// Reasons why the amount parameter is required:
		/// 1. Dapps must be motivated to understand what they are doing with the collateral,
		///    in case they decide to handle it manually.
		/// 2. Depending on the specific wallet implementation, requesting more collateral than necessarily might
		///    worsen the user experience with that dapp, requiring the wallet to make explicit wallet reorganisation
		///    when it is not necessary and can be avoided.
		/// 3. If dapps don't understand how much collateral they actually need to make their transactions work -
		///    they are placing more user funds than necessary in risk.
		/// So requiring the amount parameter would be a by-spec behavior for a wallet. Not requiring it is possible,
		/// but not specified, so dapps should not rely on that and the behavior is not recommended....
		/// (check class summary link for full description)
		/// </summary>
		/// <param name="collateralParams"></param>
		/// <returns></returns>
		ValueTask<string[]> GetCollateral(CollateralParams collateralParams);

		/// <summary>
		/// api.getBalance(): Promise\>
		/// Errors: APIError
		/// Returns the total balance available of the wallet. This is the same as summing the results of api.getUtxos(),
		/// but it is both useful to dApps and likely already maintained by the implementing wallet in a more efficient
		/// manner so it has been included in the API as well.
		/// </summary>
		/// <returns></returns>
		ValueTask<string> GetBalance();

		/// <summary>
		/// api.getUsedAddresses(paginate: Paginate = undefined): Promise\
		/// Errors: APIError
		/// Returns a list of all used (included in some on-chain transaction) addresses controlled by the wallet.
		/// The results can be further paginated by paginate if it is not undefined.
		/// </summary>
		/// <param name="paginate"></param>
		/// <returns></returns>
		ValueTask<string[]> GetUsedAddresses(Paginate? paginate = null);

		/// <summary>
		/// api.getChangeAddress(): Promise\
		/// Errors: APIError
		/// Returns an address owned by the wallet that should be used as a change address to return leftover assets
		/// during transaction creation back to the connected wallet. This can be used as a generic receive address as well.
		/// </summary>
		/// <returns></returns>
		ValueTask<string> GetChangeAddress();

		/// <summary>
		/// api.getRewardAddresses(): Promise\
		/// Errors: APIError
		/// Returns the reward addresses owned by the wallet. This can return multiple addresses e.g. CIP-0018.
		/// </summary>
		/// <returns></returns>
		ValueTask<string[]> GetRewardAddresses();

		/// <summary>
		/// api.signTx(tx: cbor\, partialSign: bool = false): Promise\>
		/// Errors: APIError, TxSignError
		/// Requests that a user sign the unsigned portions of the supplied transaction. The wallet should ask the
		/// user for permission, and if given, try to sign the supplied body and return a signed transaction.
		/// If partialSign is true, the wallet only tries to sign what it can. If partialSign is false and the wallet
		/// could not sign the entire transaction, TxSignError shall be returned with the ProofGeneration code. Likewise
		/// if the user declined in either case it shall return the UserDeclined code. Only the portions of the witness
		/// set that were signed as a result of this call are returned to encourage dApps to verify the contents returned
		/// by this endpoint while building the final transaction.
		/// </summary>
		/// <param name="tx">A hex-encoded string representing CBOR</param>
		/// <param name="partialSign"></param>
		/// <returns></returns>
		ValueTask<string> SignTx(string tx, bool partialSign = false);

		/// <summary>
		/// api.signData(addr: Address, payload: Bytes): Promise\
		/// Errors: APIError, DataSignError
		/// This endpoint utilizes the CIP-0008 signing spec for standardization/safety reasons.
		/// It allows the dApp to request the user to sign a payload conforming to said spec.
		/// The user's consent should be requested and the message to sign shown to the user.
		/// The payment key from addr will be used for base, enterprise and pointer addresses to determine the
		/// EdDSA25519 key used. The staking key will be used for reward addresses... more at url in class summary.
		/// </summary>
		/// <param name="addr">A string represnting an address in either bech32 format, or hex-encoded bytes.</param>
		/// <param name="payload">A hex-encoded string of the corresponding bytes.</param>
		/// <returns></returns>
		ValueTask<DataSignature> SignData(string addr, string payload);

		/// <summary>
		/// api.submitTx(tx: cbor\): Promise\
		/// Errors: APIError, TxSendError
		/// As wallets should already have this ability, we allow dApps to request that a transaction be sent through it.
		/// If the wallet accepts the transaction and tries to send it, it shall return the transaction id for the dApp
		/// to track. The wallet is free to return the TxSendError with code Refused if they do not wish to send it,
		/// or Failure if there was an error in sending it (e.g. preliminary checks failed on signatures).
		/// </summary>
		/// <param name="tx">A hex-encoded string representing CBOR</param>
		/// <returns></returns>
		ValueTask<string> SubmitTx(string tx);
	}
}
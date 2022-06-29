export function createWebWalletConnectorInteropObj() {
	return {
		connectedWallet: null,

		init: (dotNetRef) => {
			document.onkeydown = function (e) {
				e = e || window.event;
				if (e.key === "Escape") {
					//console.log("Escape");
					dotNetRef.invokeMethodAsync("HideConnectWalletDialog");
				}
			};
		},

		isWalletInstalled: function (walletkey) {
			try {
				return (Object.keys(window.cardano).indexOf(walletkey) > -1);
			} catch (e) {
				return false;
			}
		},
	}
}
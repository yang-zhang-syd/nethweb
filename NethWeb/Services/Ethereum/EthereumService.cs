using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.TransactionReceipts;
using Nethereum.Web3;

namespace NethWeb.Services.Ethereum
{
    public class EthereumService : IEthereumService
    {
        private const int UnLockAccountDurationInSeconds = 120;

        private async Task unlockAccount(Web3 web3, string publicKey, string password)
        {
            Nethereum.Hex.HexTypes.HexBigInteger accountUnlockTime = new Nethereum.Hex.HexTypes.HexBigInteger(120);
            var account = await web3.Personal.UnlockAccount.SendRequestAsync(publicKey, password, accountUnlockTime);
        }

        public async Task<string> CreateNewAccount(Web3 web3, string password, BigInteger? chainId = null)
        {
            return await web3.Personal.NewAccount.SendRequestAsync(password, chainId);
        }

        public async Task<HexBigInteger> GetAccountBalance(Web3 web3, string publicKey)
        {
            var balance = await web3.Eth.GetBalance.SendRequestAsync(publicKey);
            return balance;
        }

        public async Task<TransactionReceipt> TransferFund(Web3 web3, string senderAddr, string senderPassword,
            string receiverAddr, decimal amount)
        {
            var result = await web3.Personal.UnlockAccount.SendRequestAsync(senderAddr, senderPassword, UnLockAccountDurationInSeconds);
            if (result != true) throw new Exception("Account not unlocked");

            var amountInWei = Web3.Convert.ToWei(amount);

            var pollingService = new TransactionReceiptPollingService(web3.TransactionManager);
            var receipt = await pollingService.SendRequestAndWaitForReceiptAsync(new TransactionInput
            {
                From = senderAddr,
                To = receiverAddr,
                Value = new HexBigInteger(amountInWei)
            });

            return receipt;
        }
    }
}

using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace NethWeb.Services.Ethereum
{
    public interface IEthereumService
    {
        Task<HexBigInteger> GetAccountBalance(Web3 web3, string publicKey);

        Task<TransactionReceipt> TransferFund(Web3 web3, string senderAddr, string senderPassword, string receiverAddr, decimal amount);

        Task<string> CreateNewAccount(Web3 web3, string password, BigInteger? chainId = null);
    }
}

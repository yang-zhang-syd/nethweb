using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Nethereum.Geth;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using NethWeb.Models;
using NethWeb.Services.Ethereum;

namespace NethWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Ethereum")]
    public class EthereumController : Controller
    {
        private readonly IEthereumService _ethereumServices;
        private readonly string _ethUrl;

        public EthereumController(IConfiguration configuration, IEthereumService ethereumServices)
        {
            _ethereumServices = ethereumServices;
            _ethUrl = configuration["EthereumUrl"];
        }

        [HttpPost("CreateNewAccount")]
        public async Task<string> CreateNewAccount(string password)
        {
            var web3 = new Web3Geth(_ethUrl);
            return await _ethereumServices.CreateNewAccount(web3, password);
        }

        [HttpPost("GetAccountBalance")]
        public async Task<AccountBalance> GetAccountBalance(string publicKey)
        {
            var web3 = new Web3Geth(_ethUrl);
            var balance = await _ethereumServices.GetAccountBalance(web3, publicKey);
            return new AccountBalance { PublicAddress = publicKey, Balance = Web3.Convert.FromWei(balance) };
        }

        [HttpPost("GetAccountBalances")]
        public async Task<Dictionary<string, decimal>> GetAccountBalances([FromBody] List<string> publicKeys)
        {
            var balances = new Dictionary<string, decimal>();
            var web3 = new Web3Geth(_ethUrl);

            foreach (var key in publicKeys)
            {
                var balance = await _ethereumServices.GetAccountBalance(web3, key);
                balances.TryAdd(key, Web3.Convert.FromWei(balance));
            }

            return balances;
        }

        [HttpPost("TransferEther")]
        public async Task<TransactionReceipt> TransferEther(string fromAddr, string password, string toAddr, decimal amount)
        {
            var web3 = new Web3Geth(_ethUrl);
            var receipt = await _ethereumServices.TransferFund(web3, fromAddr, password, toAddr, amount);
            return receipt;
        }
    }
}

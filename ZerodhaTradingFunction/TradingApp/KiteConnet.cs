using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KiteConnect;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace TradingApp
{
    public class KiteConnectClient
    {
        private readonly Kite _kite;
        private readonly SecretClient _secretClient;

        public KiteConnectClient()
        {
            _secretClient = new SecretClient(new Uri("https://your-key-vault-name.vault.azure.net/"), new DefaultAzureCredential());
            var apiKey = GetSecretValueAsync("KiteApiKey").Result;
            _kite = new Kite(apiKey);
        }

        private async Task<string> GetSecretValueAsync(string secretName)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }

        public async Task LoginAndAuthenticateAsync(string requestToken)
        {
            var apiSecret = await GetSecretValueAsync("KiteApiSecret");
            User user = _kite.GenerateSession(requestToken, apiSecret);
            _kite.SetAccessToken(user.AccessToken);
        }
        public Dictionary<string, LTP> GetLastTradedPrice(string[] tradingSymbols)
        {
            var ltp = _kite.GetLTP(tradingSymbols);
            var ltpQuote = new Dictionary<string, LTP>();
            foreach (var item in ltp)
            {
                ltpQuote.Add(item.Key, (LTP)item.Value);
            }
            return ltpQuote;
        }


        public void Logout()
        {
            // Assuming _kite has a method InvalidateAccessToken
            // to invalidate the current access token, effectively logging out the user.
            // Check the documentation for the correct method to log out, if available.
            _kite.InvalidateAccessToken();
        }

    }
}

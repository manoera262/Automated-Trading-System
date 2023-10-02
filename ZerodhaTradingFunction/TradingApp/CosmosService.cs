using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp;

public class CosmosDbService
{
    private Container _container;
    private readonly SecretClient _secretClient;

    public CosmosDbService()
    {
        _secretClient = new SecretClient(new Uri("https://your-key-vault-name.vault.azure.net/"), new DefaultAzureCredential());
        var endPointURL = GetSecretValueAsync("endPointURL").Result;
        var primaryKey = GetSecretValueAsync("primarykey").Result;

        var client = new CosmosClient(endPointURL, primaryKey);
        var database = client.GetDatabase("YourDatabaseName");
        _container = database.GetContainer("YourContainerName");
    }

    

    private async Task<string> GetSecretValueAsync(string secretName)
    {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
    }

    public async Task AddItemAsync(dynamic tradeData)
    {
        await _container.CreateItemAsync(tradeData, new PartitionKey(tradeData.id.ToString()));
    }

    public async Task<dynamic> GetItemAsync(Guid id)
    {
        try
        {
            ItemResponse<dynamic> response = await _container.ReadItemAsync<dynamic>(id.ToString(), new PartitionKey(id.ToString()));
            return response.Resource;   
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }
}

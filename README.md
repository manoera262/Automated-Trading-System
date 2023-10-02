# Automated-Trading-System

This project aims to automate trading operations based on predefined business logic. It interfaces with the Zerodha Kite Connect API to monitor stock prices, authenticate users, and execute buy/sell orders. Additionally, it leverages Azure services for secure configuration management, logging, and data storage.

## Required Azure Resources

1. **Resource Group**:
   - A container that holds related resources for an Azure solution.

2. **Azure Function App**:
   - Hosts the trading logic that interfaces with the Zerodha Kite Connect API.
   
3. **Application Insights**:
   - Monitors the performance and usage of the Function App.

4. **Azure Key Vault**:
   - Stores sensitive information such as the Zerodha API keys and database credentials.
   - Manually add secrets for `endPointURL`, `primarykey`, `KiteApiKey`, and `KiteApiSecret`.

5. **Azure Cosmos DB (NoSQL API)**:
   - Stores trade data and other relevant information.
   - Create a database and container for storing trade data.

6. **Action Group**:
   - Defines a collection of actions to execute when specific alerts are triggered.

## Manual Configurations

- **Azure Key Vault**:
   - After the Key Vault is created, add the following secrets manually:
      - `endPointURL`: The endpoint URL of your data source.
      - `primarykey`: The primary key to authenticate with your data source.
      - `KiteApiKey`: The API key for Zerodha Kite Connect.
      - `KiteApiSecret`: The API secret for Zerodha Kite Connect.
    
   - Add system generated Manged identity of the function app created to the access policies of the keyvault with get secrets permission.

- **Azure Cosmos DB**:
   - After the Cosmos DB account is created:
      - Create a database named `TradingDatabase`.
      - In `TradingDatabase`, create a container named `TradeData` with a partition key of `/partitionKey`.

- **Azure Function App**:
   - Configure the Function App settings to include the necessary environment variables and connection strings.
   - Deploy your trading logic code to the Function App.

- **Application Insights**:
   - Ensure that the Instrumentation Key of Application Insights is configured in the Function App settings for logging and monitoring.

- **Action Group**:
   - Configure the action group to notify the necessary parties or trigger other actions based on your alert criteria.

## Deployment

- The resources can be created and configured manually through the Azure portal, or automatically through scripts using the Azure CLI or Azure PowerShell.
- The trading logic should be deployed to the Azure Function App either through the Azure portal or using Azure DevOps pipelines.

## Monitoring and Maintenance

- Regularly review the logs and metrics in Application Insights to ensure the trading system is performing as expected.
- Update the Azure Function App with new trading logic as necessary.
- Review and update the alerts and actions in the Action Group to ensure they remain relevant and effective.

## Security Considerations

- Ensure that the Azure Key Vault and Cosmos DB are configured with the appropriate level of access control.
- Regularly review and rotate the secrets in Azure Key Vault.


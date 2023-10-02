using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TradingApp
{
    public static class MainFunction

    { 
        [FunctionName("MainFunction")]
        public static async Task RunAsync(
            [TimerTrigger("0/5 * * * * *")] TimerInfo myTimer,
            ILogger log )
        {
            log.LogInformation("Trading Function Last Run" + DateTime.Now);
            // Place any logic you want to run every 5 seconds here

            try
            {
                DriveFunction driveFunction = new DriveFunction();
                await driveFunction.ExecuteTradingLogicAsync();
            }
            catch (Exception ex)
            {
                //log alerts would be created on this Message and action groups will be alerted for any failure.
                log.LogError($"Trading Function Failure: {ex.Message}");
            }

        }
    }
}

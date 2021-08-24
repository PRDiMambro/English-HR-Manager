using System;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace English_HR_Manager
{
    public static class Function_BankHolidays
    {
        [FunctionName("BankHolidays")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                         "get",
                         Route = "bank-holidays/{year:int?}")]
            HttpRequest req,
            int? year)
        {
            string responseMessage = null;
            Holiday holidays = new Holiday();

            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://www.gov.uk/bank-holidays.json");
                holidays = JsonConvert.DeserializeObject<Holiday>(json);
            }

            string csv = String.Join(Environment.NewLine,
                                     holidays.EnglandAndWales.Events
                                             .Where(x => x.EventDate.GetValueOrDefault().Year == year)
                                             .OrderByDescending(x => x.EventDate.GetValueOrDefault())
                                             .Select(x => $"{x.Title}," +
                                                          $"{x.EventDate.ToString()}," +
                                                          $"{x.Notes}," +
                                                          $"{x.Bunting.ToString()}"
                                                    ).ToArray()
                                     );

            string scs = "DefaultEndpointsProtocol=https;" +
                         "AccountName=englishhrmanager;" +
                         "AccountKey=9O8WgXFUyoMxjB3BrjIhHGmtJkYrjnkz0ec5LKgP0aGO+e+BTq0eUaJ4S/P4QOvgA8OmD8VsCiVqkxo+269weg==;" +
                         "BlobEndpoint=https://englishhrmanager.blob.core.windows.net/;TableEndpoint=https://englishhrmanager.table.core.windows.net/;QueueEndpoint=https://englishhrmanager.queue.core.windows.net/;FileEndpoint=https://englishhrmanager.file.core.windows.net/";
            toBlob(scs, "bankholidays", "BankHolidays.csv", csv);

            responseMessage = csv;
            return await Task.FromResult(new OkObjectResult(responseMessage));
        }
        public static void toBlob(string storageConnectionString,
                       string containerString,
                       string blobName,
                       string text)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerString);
            var blob = container.GetBlockBlobReference(blobName);
            using (CloudBlobStream x = blob.OpenWriteAsync().Result)
            {
                x.Write(System.Text.Encoding.Default.GetBytes(text.ToString() + "\n"));
                x.Flush();
                x.Close();
            }
        }
    }
}

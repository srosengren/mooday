using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Mooday
{
    public static class SetMood
    {
        [FunctionName("SetMood")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "moods")] HttpRequest req,
            [Table("moods"), StorageAccount("AzureWebJobsStorage")] ICollector<MoodTableStorageItem> moodTableOutput,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SetMoodModel data = JsonConvert.DeserializeObject<SetMoodModel>(requestBody);

            moodTableOutput.Add(new MoodTableStorageItem
            {
                PartitionKey = "test@test.com",
                RowKey = $"{data.DateYYYYMMDD}-{(int)data.Type}",
                MoodLevel = data.MoodLevel,
                Comment = data.Comment
            });

            return new OkResult();
        }
    }

    public class SetMoodModel
    {
        public string DateYYYYMMDD { get; set; }
        public MoodType Type { get; set; }
        public int MoodLevel { get; set; }
        public string Comment { get; set; }
    }
}

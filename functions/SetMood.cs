using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;

namespace Mooday
{
    public static class SetMood
    {
        [FunctionName("SetMood")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "moods/{dateYYYYMMDD}/{type}")] HttpRequest req,
            string dateYYYYMMDD,
            string type,
            [Table("moods")]CloudTable moods,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SetMoodModel data = JsonConvert.DeserializeObject<SetMoodModel>(requestBody);

            await moods.ExecuteAsync(TableOperation.InsertOrReplace(new MoodTableStorageItem
            {
                PartitionKey = "test@test.com",
                RowKey = $"{dateYYYYMMDD}-{type}",
                MoodLevel = data.MoodLevel,
                Comment = data.Comment
            }));

            return new OkResult();
        }
    }

    public class SetMoodModel
    {
        public int MoodLevel { get; set; }
        public string Comment { get; set; }
    }
}

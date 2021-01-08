using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System;

namespace Mooday
{
    public static class GetMoods
    {
        [FunctionName("GetMoods")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "moods")] HttpRequest req,
            [Table("moods")] CloudTable moods,
            ILogger log)
        {

            TableQuery<MoodTableStorageItem> query = new TableQuery<MoodTableStorageItem>()
                .Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "test@test.com")
                );

            var userMoods = new List<MoodQueryResult>();
            foreach (MoodTableStorageItem entity in await moods.ExecuteQuerySegmentedAsync(query, null))
            {
                userMoods.Add(new MoodQueryResult
                {
                    DateYYYYMMDD = entity.RowKey.Substring(0, entity.RowKey.IndexOf('-')),
                    Type = (MoodType)Enum.Parse(typeof(MoodType), entity.RowKey.Substring(entity.RowKey.IndexOf('-') + 1)),
                    MoodLevel = entity.MoodLevel,
                    Comment = entity.Comment
                });
            }

            return new OkObjectResult(userMoods);
        }
    }

    public class MoodQueryResult
    {
        public string DateYYYYMMDD { get; set; }
        public MoodType Type { get; set; }
        public int MoodLevel { get; set; }
        public string Comment { get; set; }
    }
}

using Microsoft.Azure.Cosmos.Table;

namespace Mooday
{
    public class MoodTableStorageItem: TableEntity
    {
        public int MoodLevel { get; set; }
        public string Comment { get; set; }
    }
}
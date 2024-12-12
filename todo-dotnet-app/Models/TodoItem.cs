using Azure;
using Azure.Data.Tables;

namespace TodoApp.Models
{
    public class TodoItem : ITableEntity
    {
        public string PartitionKey { get; set; } = "TodoPartition"; // Default partition for simplicity
        public string RowKey { get; set; } = Guid.NewGuid().ToString(); // Unique identifier for each item
        public string Name { get; set; } = string.Empty;
        public bool IsComplete { get; set; } = false;

        // Azure Table Storage specific properties
        public ETag ETag { get; set; } = ETag.All; // Optimistic concurrency
        public DateTimeOffset? Timestamp { get; set; } // Automatically managed by Table Storage
    }
}

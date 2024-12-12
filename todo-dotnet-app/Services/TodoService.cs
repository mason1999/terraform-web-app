// using Azure;
// using Azure.Data.Tables;
// using TodoApp.Models;

// namespace TodoApp.Services
// {
//     public class TodoService
//     {
//         private readonly TableClient _tableClient;

//         public TodoService(TableClient tableClient)
//         {
//             _tableClient = tableClient;
//         }

//         public async Task<IAsyncEnumerable<TodoItem>> GetAllItemsAsync()
//         {
//             // Directly return the asynchronous query
//             return _tableClient.QueryAsync<TodoItem>();
//         }

//         public async Task<TodoItem?> GetItemAsync(string rowKey)
//         {
//             try
//             {
//                 var response = await _tableClient.GetEntityAsync<TodoItem>("TodoPartition", rowKey);
//                 return response.Value;
//             }
//             catch (RequestFailedException ex) when (ex.Status == 404)
//             {
//                 return null; // Item not found
//             }
//         }

//         public async Task AddItemAsync(TodoItem item)
//         {
//             await _tableClient.AddEntityAsync(item);
//         }

//         public async Task UpdateItemAsync(TodoItem item)
//         {
//             // Update the entity using the correct ETag type
//             await _tableClient.UpdateEntityAsync(item, item.ETag);
//         }

//         public async Task DeleteItemAsync(string rowKey)
//         {
//             await _tableClient.DeleteEntityAsync("TodoPartition", rowKey);
//         }
//     }
// }

using Azure;
using Azure.Data.Tables;
using TodoApp.Models;

namespace TodoApp.Services
{
    public class TodoService
    {
        private readonly TableClient _tableClient;

        public TodoService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        // Return Task<AsyncPageable<TodoItem>> instead of Task<IAsyncEnumerable<TodoItem>>
        public Task<AsyncPageable<TodoItem>> GetAllItemsAsync()
        {
            // QueryAsync returns an AsyncPageable, not an IAsyncEnumerable
            return Task.FromResult(_tableClient.QueryAsync<TodoItem>());
        }

        public async Task<TodoItem?> GetItemAsync(string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<TodoItem>("TodoPartition", rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null; // Item not found
            }
        }

        public async Task AddItemAsync(TodoItem item)
        {
            await _tableClient.AddEntityAsync(item);
        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            // Update the entity using the correct ETag type
            await _tableClient.UpdateEntityAsync(item, item.ETag);
        }

        public async Task DeleteItemAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync("TodoPartition", rowKey);
        }
    }
}

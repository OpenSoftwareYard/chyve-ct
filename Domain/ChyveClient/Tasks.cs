using System;
using System.Net.Http.Json;
using ChyveClient.Models;

namespace ChyveClient;

public partial class Client
{
    public static async Task<IEnumerable<TaskHandle>?> GetTaskDetails(Uri baseUri, string accessToken, IEnumerable<TaskHandle> taskHandles)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;

        var requestUri = $"/tasks?api_key={accessToken}";

        requestUri = taskHandles.Aggregate(requestUri, (current, taskHandle) => current + $"&task_id={taskHandle.Id}");

        Console.WriteLine("Getting task details: {0}", $"{baseUri}{requestUri}");

        var returnedTaskHandles = await httpClient.GetFromJsonAsync<IEnumerable<TaskHandle>>(
            requestUri
            );

        return returnedTaskHandles;
    }
}
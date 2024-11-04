using System;
using System.Net.Http.Json;
using ChyveClient.Models;

namespace ChyveClient;

public partial class Client
{
    public static async Task<TaskHandle?> GetTaskDetails(Uri baseUri, string accessToken, TaskHandle taskHandle)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = baseUri;

        Console.WriteLine("Getting task details: {0}", $"{baseUri}tasks?api_key={accessToken}&task_id={taskHandle.Id}");

        var taskHandles = await httpClient.GetFromJsonAsync<IEnumerable<TaskHandle>>(
            $"/tasks?api_key={accessToken}&task_id={taskHandle.Id}"
            );

        return taskHandles?.FirstOrDefault();
    }
}
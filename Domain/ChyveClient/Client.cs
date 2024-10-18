using Microsoft.Net.Http.Headers;

namespace ChyveClient;

public partial class Client
{
    private readonly Uri _uri;
    private readonly string _accessToken;
    private readonly HttpClient _httpClient;

    public Client(Uri uri, string accessToken, HttpClient httpClient)
    {
        _uri = uri;
        _accessToken = accessToken;
        _httpClient = httpClient;

        _httpClient.BaseAddress = _uri;
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {_accessToken}");
    }
}

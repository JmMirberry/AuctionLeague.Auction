using System.Net.Http.Json;
using AuctionLeague.Fpl.Models;
using Microsoft.Extensions.Options;

namespace AuctionLeague.Fpl;

public class FplClient : IFplClient
{
    private readonly FplSettings _settings;
    private readonly HttpClient _client;

    public FplClient(IOptions<FplSettings> settings, HttpClient factory)
    {
        _settings = settings.Value;
        _client = factory;
    }
    public async Task<FplResponse> GetAllFplData()
    {
        var response = await _client.GetAsync(_settings.FplApiUrl);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<FplResponse>();
    }
}
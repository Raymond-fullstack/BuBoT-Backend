using System;

namespace backend.services;

public class RasaService
{
  private readonly HttpClient _http;

  public RasaService(HttpClient http)
  {
    _http = http;
  }

  public async Task<List<string>> SendMessage(string senderId, string message)
  {
    var response = await _http.PostAsJsonAsync(
        "http://localhost:5005/webhooks/rest/webhook",
        new
        {
          sender = senderId,
          message = message
        });

    var data = await response.Content
        .ReadFromJsonAsync<List<RasaResponse>>();

    return data?.Select(x => x.Text).ToList() ?? new List<string>();
  }
}

public class RasaResponse
{
  public string Text { get; set; } = null!;
}
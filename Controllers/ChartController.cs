using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ChatController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            return BadRequest(new { reply = "❌ API key OpenAI chưa được cấu hình." });
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var body = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
            new { role = "user", content = request.Message }
        }
        };

        var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", body);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(new
            {
                reply = $"❌ Lỗi từ API OpenAI (HTTP {(int)response.StatusCode}): {responseContent}"
            });
        }

        try
        {
            var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var reply = result?.Choices?.FirstOrDefault()?.Message?.Content;
            return Ok(new { reply });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                reply = $"❌ Lỗi khi phân tích phản hồi từ OpenAI: {ex.Message}",
                raw = responseContent
            });
        }
    }
}


    public class ChatRequest
{
    public string? Message { get; set; }
}

public class OpenAIResponse
{
    public List<Choice>? Choices { get; set; }    
}

public class Choice
{
    public Message? Message { get; set; }
}

public class Message
{
    public string?    Role { get; set; }
    public string? Content { get; set; }
}

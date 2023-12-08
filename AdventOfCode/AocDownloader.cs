namespace AdventOfCode;

public static class AocDownloader
{
    private static readonly HttpClientHandler _httpClientHandler = new() {  UseCookies = false };
    private static readonly HttpClient _httpClient = new( _httpClientHandler );
    private static readonly string _session = Environment.GetEnvironmentVariable("AOC_SESSION") ?? throw new ArgumentNullException("AOC_SESSION", "Needs environment variable AOC_SESSION");
    private static readonly string _savePath = $"{AppContext.BaseDirectory}\\Inputs";

    public static TextReader GetInput(int year, int day)
    {
        var filePath = Path.Join(_savePath, $"{day:D2}.txt");
        if (File.Exists(filePath))
        {
            return File.OpenText(filePath);
        }

        var httpInput = GetHttpInput(year, day);
        var strInput = httpInput.ReadAsStringAsync().Result;

        File.WriteAllText(filePath, strInput);
        return new StringReader(strInput);
    }

    private static HttpContent GetHttpInput(int year, int day)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://adventofcode.com/{year}/day/{day}/input"),
        };
        request.Headers.Add("Cookie", $"session={_session}");
        request.Headers.Add("User-Agent", "github.com/floriskruisselbrink by floris -at- vloris.nl");

        var response = _httpClient.Send(request);

        // TODO: check success
        return response.Content;
    }
}

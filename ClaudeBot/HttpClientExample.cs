using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace ClaudeBot
{
    public class HttpClientExample
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> Send(string prompt)
        {
            // İstek URL'si
            var url = "https://claude.ai/api/organizations/9c7f098b-2403-41da-8433-b8876c7201e5/chat_conversations/81971786-f264-4633-84ed-a9aed3e4b60a/completion";

            // Dinamik prompt değeri
            string promptValue = prompt;

            // Gönderilecek veri
            var requestData = new
            {
                prompt = promptValue,
                parent_message_uuid = "1a5650a5-d31d-46e4-9f36-320b673d406e",
                timezone = "Europe/Istanbul",
                attachments = new string[] { },
                files = new string[] { },
                rendering_mode = "raw"
            };

            // JSON içeriği
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Başlıkları ayarla
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Opera\";v=\"112\"");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 OPR/112.0.0.0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/json");
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept", "text/event-stream, text/event-stream");
            client.DefaultRequestHeaders.TryAddWithoutValidation("baggage", "sentry-environment=production,sentry-release=7a9ced39a05fb9b7ab0e3fdb3d806a457a461a9d,sentry-public_key=58e9b9d0fc244061a1b54fe288b0e483,sentry-trace_id=66f8a140a2ca422ebe258f19437e30fe");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sentry-trace", "66f8a140a2ca422ebe258f19437e30fe-b17195e9a6bab7b3-0");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");
            client.DefaultRequestHeaders.TryAddWithoutValidation("origin", "https://claude.ai");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-site", "same-origin");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-mode", "cors");
            client.DefaultRequestHeaders.TryAddWithoutValidation("sec-fetch-dest", "empty");
            client.DefaultRequestHeaders.TryAddWithoutValidation("referer", "https://claude.ai/chat/81971786-f264-4633-84ed-a9aed3e4b60a");
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept-encoding", "gzip, deflate, br, zstd");
            client.DefaultRequestHeaders.TryAddWithoutValidation("accept-language", "tr-TR,tr;q=0.9,en-US;q=0.8,en;q=0.7");
            //client.DefaultRequestHeaders.TryAddWithoutValidation("cookie",);
            client.DefaultRequestHeaders.TryAddWithoutValidation("priority", "u=1, i");

            try
            {
                // POST isteği gönder
                HttpResponseMessage response = await client.PostAsync(url, content);

                // Yanıtı oku
                var responseStream = await response.Content.ReadAsStreamAsync();

                // Yanıtı Brotli ile çöz
                using (var decompressedStream = new BrotliStream(responseStream, CompressionMode.Decompress))
                using (var reader = new StreamReader(decompressedStream, Encoding.UTF8))
                {
                    string responseBody = await reader.ReadToEndAsync();
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Response Body: {responseBody}");

                    string pattern = @"(?<=\""completion\"":\"")(.*?)(?=\"")";
                    var matches = Regex.Matches(responseBody, pattern);

                    // Tüm 'completion' içeriklerini topla
                    string allCompletions = string.Join("", matches);
                    return allCompletions;


                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return "bos";

            }
        }
    }
}

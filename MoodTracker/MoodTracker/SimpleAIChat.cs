using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoodTracker
{
    public class SimpleAIChat
    {
        private readonly string _apiKey; // 替换为你的 DeepSeek Key
        private const string ApiUrl = "https://api.deepseek.com/chat/completions";
        private readonly HttpClient _httpClient;

        public SimpleAIChat(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(180);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> GetChatResponse(string message)
        {
            var requestBody = new
            {
                model = "deepseek-chat",
                messages = new[]
        {
            new {
                role = "system",
                content = "You are a helpful assistant. Always format your responses using Markdown " +
                         "for better readability. Use headings, lists, and code blocks when appropriate."
            },
            new { role = "user", content = message }
        },
                stream = false
            };

            try
            {
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ApiUrl, content).ConfigureAwait(false);

                response.EnsureSuccessStatusCode(); // 如果状态码不成功会抛出异常

                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<DeepSeekResponse>(responseContent);

                var lines = (result.choices.FirstOrDefault()?.message.content ?? string.Empty)
                    .Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                return string.Join("\r\n",lines);
            }
            catch (TaskCanceledException)
            {
                throw new Exception("请求超时，请检查网络连接或API可用性");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API请求失败: {ex.Message}");
            }
        }

        // DeepSeek API 响应结构
        private class DeepSeekResponse
        {
            public string id { get; set; }
            public string @object { get; set; }
            public long created { get; set; }
            public Choice[] choices { get; set; }
            public Usage usage { get; set; }
        }

        private class Choice
        {
            public int index { get; set; }
            public Message message { get; set; }
            public string finish_reason { get; set; }
        }

        private class Message
        {
            public string role { get; set; }
            public string content { get; set; }
        }

        private class Usage
        {
            public int prompt_tokens { get; set; }
            public int completion_tokens { get; set; }
            public int total_tokens { get; set; }
        }
    }
    }

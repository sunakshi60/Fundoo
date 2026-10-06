using BussinessLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public class MessageClient : IMessageClient
    {
        private readonly HttpClient _httpClient;

        public MessageClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var request = new
            {
                email = email,
                subject = subject,
                body = body
            };

            await _httpClient.PostAsJsonAsync(
                "api/Message/send-email",
                request);
        }
    }
}

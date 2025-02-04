using System;
using RestSharp;
using System.Threading.Tasks;
using DrinkConnect.Utils;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DrinkConnect.Services
{
    public class EmailService
    {
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    // note - mailtrap may not send to gmail emails,
    // works fine for my university email
    public async Task<bool> SendEmailAsync(string recipient, string subject, string message)
    {
        var client = new RestClient("https://send.api.mailtrap.io/api/send");

        var request = new RestRequest();
        request.AddHeader("Authorization", $"Bearer {_emailSettings.ApiKey}");
        request.AddHeader("Content-Type", "application/json");

        var emailPayload = new
        {
            from = new
            {
                email = _emailSettings.FromEmail,
                name = _emailSettings.FromName
            },
            to = new[]
            {
                new { email = recipient }
            },
            subject = subject,
            text = message,
            category = "Integration Test"
        };

        request.AddJsonBody(emailPayload);

        var response = await client.PostAsync(request);

        return response?.IsSuccessful ?? false;
    }
}

}
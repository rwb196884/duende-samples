// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TokenExchange.Api;

public class GreetingsController : ControllerBase
{
    [HttpGet("{**catch-all}", Name = "Greet")]
    [ProducesResponseType(typeof(GreetingsResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        string message;
        var sub = User.FindFirst("sub");

        if (!User.Identity.IsAuthenticated)
        {
            message = "Hello, anonymous caller";
        }
        else if (sub != null)
        {
            var userName = User.FindFirst("name");
            message = $"Hello user, {userName.Value}";
        }
        else
        {
            var client = User.FindFirst("client_id");
            message = $"Hello client, {client.Value}";
        }

        var response = new
        {
            path = Request.Path.Value,
            message = message,
            time = DateTime.UtcNow.ToString(),
            headers = Request.Headers.ToDictionary(x => x.Key, x => string.Join(',', x))
        };

        return Ok(response);
    }

    public class GreetingsResponse
    {
        public string? Url { get; set; }
        public string? Path { get; set; }
        public string? Message { get; set; }

        public string? Time { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
    }

}

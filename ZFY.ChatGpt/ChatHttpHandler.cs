﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFY.ChatGpt
{
    public class ChatHttpHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", $"Bearer {Constants.ApiKey}");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
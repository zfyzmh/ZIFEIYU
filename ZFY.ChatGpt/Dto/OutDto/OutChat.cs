﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFY.ChatGpt.Dto.OutDto
{
    public class OutChat
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("usage")]
        public Usage Usage { get; set; }

        [JsonProperty("choices")]
        public Choice[] Choices { get; set; }
    }

    public class Choice
    {
        [JsonProperty("message")]
        public ChatMessage? Message { get; set; }

        [JsonProperty("finish_reason")]
        public string? FinishReason { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }
}
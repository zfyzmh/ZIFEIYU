﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net;
using ZIFEIYU.DataBase;
using ZIFEIYU.Entity;
using ZIFEIYU.Model;
using ZIFEIYU.Services;

namespace ZIFEIYU.Pages
{
    public class ChatBase : ComponentBase
    {
        public ChatBase()
        {
        }

        [Inject]
        public ChatGPTServices ChatGPTServices { get; set; }

        [Inject]
        public ZFYDatabase ZFYDatabase { get; set; }

        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        public string HelperText { get; set; }

        public DialogueOutput DialogueOutput { get; set; }
        public MudTextField<string> singleLineReference;
        public List<DialogueMessage> Messages { get; set; } = new List<DialogueMessage>();
        public bool _processing = false;

        public async Task Send()
        {
            var web = new WebClient();
            
            web.OpenReadCompleted += Web_OpenReadCompleted;
            web.OpenReadAsync(new Uri("http://localhost:21167/Test"));
            if (!string.IsNullOrWhiteSpace(HelperText))
            {
                Messages.Add(new DialogueMessage() { Role = "user", Content = HelperText });
                HelperText = "";
                _processing = true;
                StateHasChanged();
                UpdateScroll("IndexBody");
                DialogueOutput = await ChatGPTServices.SendDialogue(new DialogueInput(Messages));
                Messages.Add(DialogueOutput.Choices[0].Message);
                StateHasChanged();
                UpdateScroll("IndexBody");
                _processing = false;
            }
        }

        private static void Web_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            using (var sr = new StreamReader(e.Result))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public async Task Test()
        {
            await ZFYDatabase.Database.InsertAsync(new Test() { MyProperty = "测试" });
            var aa = await ZFYDatabase.Database.Table<Test>().FirstAsync();
        }

        private void UpdateScroll(string id)
        {
            jsRuntime.InvokeAsync<object>("updateScroll", id);
        }

        /*public async Task KeyDown(KeyboardEventArgs keyboardEventArgs)
		{
			if (keyboardEventArgs.Code == "Enter" && !_processing) {
				await Send();
				HelperText = "";
				StateHasChanged();
			};
		}*/
    }
}
﻿using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using ZFY.ChatGpt.Dto.InputDto;
using ZfyChatGPT.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using System.Text;

namespace ZfyChatGPT.Pages
{
    public partial class CreatingPaint
    {
        public bool _processing;

        private bool _isDispose = false;
        [Inject] public ImagesServices ImagesServices { get; set; }

        [Inject] public IJSRuntime jSRuntime { get; set; }
        [Inject] public ISnackbar Snackbar { get; set; }

        public string CreateImageTxt { get; set; }

        public int ImageNumber { get; set; } = 2;

        public string HelperText { get; set; }

        public List<string> Images = new List<string>();

        public bool DialogVisible { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await InitHistory();

            await base.OnInitializedAsync();
        }

        private async Task InitHistory()
        {
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await jSRuntime.InvokeAsync<Task>("UpdateScroll", "IndexBody");

            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Send()
        {
            if (!string.IsNullOrWhiteSpace(HelperText))
            {
                try
                {
                    ImagesServices.IsManualCancellation = false;
                    await CreateImage();
                    _processing = false;
                    if (!_isDispose) await jSRuntime.InvokeAsync<Task>("UpdateScroll", "IndexBody");
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    Snackbar.Add(ex.Message, Severity.Warning);
                }
            }
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <returns></returns>
        public async Task CreateImage()
        {
            DialogVisible = false;
            _processing = true;
            StateHasChanged();
            var outImage = await ImagesServices.CreateImages(new InCreateImages(HelperText) { N = ImageNumber });
            if (ImagesServices.IsManualCancellation) return;

            if (outImage != null)
            {
                Images.AddRange(outImage.Data.Select(m => m.Url).ToList());
                DialogVisible = false;
                _processing = false;
                StateHasChanged();
            }
        }

        public async Task Reset()
        {
            Images.Clear();
            StateHasChanged();
        }

        public async Task Stop()
        {
            await Task.Delay(50);

            ImagesServices.IsManualCancellation = true;
            await ImagesServices.Stop();
            _processing = false;
            StateHasChanged();
        }

        public void Dispose()
        {
            _isDispose = true;
        }

        public async Task Download(string url)
        {
            CancellationToken cancellationToken = new CancellationToken();
            await SaveFile(cancellationToken);
        }

        private async Task SaveFile(CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(Encoding.Default.GetBytes("Hello from the Community Toolkit!"));
            var fileSaverResult = await FileSaver.Default.SaveAsync("test.txt", stream, cancellationToken);
            if (fileSaverResult.IsSuccessful)
            {
                await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
            }
            else
            {
                await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
            }
        }

        public async Task ClearHistory()
        {
            StateHasChanged();
        }

        public async Task Delete()
        {
            StateHasChanged();
        }

        public void SavePrepositive()
        {
        }

        public void Retry()
        {
        }
    }
}
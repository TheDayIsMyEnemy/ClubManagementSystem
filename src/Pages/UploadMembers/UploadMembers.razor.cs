using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;

namespace ClubManagementSystem.Pages.UploadMembers
{
    public class UploadMembersBase : ComponentBase
    {
        private static string _defaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";

        [Inject]
        private NavigationManager _navigationManager { get; set; } = null!;

        [Inject]
        private IHttpClientFactory _clientFactory { get; set; } = null!;

        [Inject]
        private ISnackbar _snackBar { get; set; } = null!;

        protected bool Clearing { get; set; }
        protected string DragClass = _defaultDragClass;
        protected IBrowserFile? File { get; set; }

        protected void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            File = e.File;
        }

        protected async Task Clear()
        {
            Clearing = true;
            File = null;
            ClearDragClass();
            await Task.Delay(100);
            Clearing = false;
        }

        protected async Task Upload()
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var requestUri = $"{_navigationManager.BaseUri}api/UploadMembers";
                var contentName = "members";
                var maxAllowedSize = 1 * 1024 * 1024 * 1024;

                var fileContent = new StreamContent(File!.OpenReadStream(maxAllowedSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(File.ContentType);
                content.Add(fileContent, contentName, File.Name);

                var client = _clientFactory.CreateClient();

                var response = await client.PostAsync(requestUri, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _snackBar.Add(responseContent, Severity.Error);
                else
                    _snackBar.Add($"{responseContent} members added.", Severity.Success);
            }
            catch
            {
                _snackBar.Add(Messages.Error, Severity.Error);
            }

            File = null;
            ClearDragClass();
        }

        protected void SetDragClass()
        {
            DragClass = $"{_defaultDragClass} mud-border-primary";
        }

        protected void ClearDragClass()
        {
            DragClass = _defaultDragClass;
        }
    }
}

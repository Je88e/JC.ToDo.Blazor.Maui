using AntDesign;
using Blazor.Model;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Blazor.UI.Pages
{
    public partial class TaskInfo
    {
        [Inject]
        public HttpClient Http { get; set; }

        [Inject]
        public MessageService MsgSvr { get; set; }

        TaskDto taskDto;

        bool isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            taskDto = await Http.GetFromJsonAsync<TaskDto>($"api/Task/GetTaskDto?taskId={base.Options.TaskId}");
            await base.OnInitializedAsync();
        }

        async void OnSave()
        {
            var result = await Http.PostAsJsonAsync<TaskDto>($"api/Task/SaveTask", taskDto);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                DrawerRef<TaskDto> drawerRef = base.FeedbackRef as DrawerRef<TaskDto>;
                await drawerRef!.CloseAsync(taskDto);
            }
            else
            {
                MsgSvr.Error($"请求发生错误 {result.StatusCode}");
            }
        }

        async void OnCancel()
        {
            await base.CloseFeedbackAsync();
        }

    }
}

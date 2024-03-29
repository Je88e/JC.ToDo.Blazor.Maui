﻿using AntDesign;
using Blazor.Model.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;


namespace Blazor.UI.Pages
{
    public partial class NewTask
    {
        [Inject] public MessageService MsgSrv { get; set; }
        [Inject] public HttpClient Http { get; set; }

        [Parameter] public EventCallback<TaskDto> OnInserted { get; set; }
        [Parameter] public Func<TaskDto> NewTaskFunc { get; set; }
        [Parameter] public RenderFragment<TaskDto> ChildContent { get; set; }

        //新的任务
        private TaskDto newTask { get; set; }
        private bool isNewLoading { get; set; }

        protected override void OnInitialized()
        {
            newTask = NewTaskFunc?.Invoke();
            base.OnInitialized();
        }

        private async void OnInsertKey(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                if (string.IsNullOrWhiteSpace(newTask.Title))
                {
                    MsgSrv.Error($"标题必须填写");
                    return;
                }
                isNewLoading = true;
                var result = await Http.PostAsJsonAsync<TaskDto>($"api/Task/SaveTask", newTask);
                if (result.IsSuccessStatusCode)
                {
                    newTask.TaskId = await result.Content.ReadFromJsonAsync<Guid>();
                    await Task.Delay(1000);
                    if (OnInserted.HasDelegate) await OnInserted.InvokeAsync(newTask);

                    newTask = NewTaskFunc?.Invoke();
                }
                else
                {
                    MsgSrv.Error($"请求发生错误 {result.StatusCode}");
                }
                isNewLoading = false;
                StateHasChanged();
            }
        }
    }
}

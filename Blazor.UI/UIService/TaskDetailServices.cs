using AntDesign;
using Blazor.Model.Dto;
using Blazor.UI.Pages;

namespace Blazor.UI
{
    public class TaskDetailServices
    {
        public DrawerService DrawerSvr { get; set; }

        public TaskDetailServices(DrawerService drawerSvr)
        {
            DrawerSvr = drawerSvr;
        }

        public async Task EditTask(TaskDto taskDto, List<TaskDto> datas)
        {
            var taskItem = await DrawerSvr.CreateDialogAsync<TaskInfo, TaskDto, TaskDto>(taskDto, title: taskDto.Title, width: 450);
            if (taskItem == null) return;
            var index = datas.FindIndex(x => x.TaskId == taskItem.TaskId);
            datas[index] = taskItem;
        }
    }
}

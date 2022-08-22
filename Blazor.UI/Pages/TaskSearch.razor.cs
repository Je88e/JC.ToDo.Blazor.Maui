using Blazor.Model.Dto;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Blazor.UI.Pages
{
    public partial class TaskSearch
    {
        //7、	查询待办
        [Inject] public HttpClient Http { get; set; }

        private bool isLoading = false;

        List<TaskDto> datas = new List<TaskDto>();

        private string queryTitle;

        private int total = 0;

        private async Task OnSearch()
        {
            await OnQuery(1, 10, new List<SortFieldName>());
        }

        private async Task OnChange(AntDesign.TableModels.QueryModel<TaskDto> queryModel)
        {
            await OnQuery(
                queryModel.PageIndex,
                queryModel.PageSize,
                queryModel.SortModel.OrderBy(x => x.Priority)
                .Select(x => new SortFieldName() { SortField = x.FieldName, SortOrder = x.Sort ?? "ascend" }).ToList()
                );
        }

        private async Task OnQuery(int pageIndex, int pageSize, List<SortFieldName> sort)
        {
            isLoading = true;
            var req = new GetSearchReq()
            {
                QueryTitle = queryTitle,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Sorts = sort,
            };
            var httpRsp = await Http.PostAsJsonAsync<GetSearchReq>($"api/Task/GetSearch", req);
            var result = await httpRsp.Content.ReadFromJsonAsync<GetSearchRsp>();
            datas = result.Data;
            total = result.Total;

            isLoading = false;
        }

        //8、	查看详细服务
        [Inject] public TaskDetailServices TaskSrv { get; set; }

        private async Task OnDetail(TaskDto taskDto)
        {
            await TaskSrv.EditTask(taskDto, datas);
        }
    }
}

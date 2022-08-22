using Blazor.Entity;
using Blazor.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChartController
    {
        TodoContext Context;

        public ChartController(TodoContext context)
        {
            Context = context;
        }

        [HttpGet]
        //每日待办数量
        public List<ChartAmountDto> GetAmountDto()
        {
            return Context.Task.GroupBy(x => new { x.PlanTime, x.IsImportant }).Select(x => new ChartAmountDto()
            {
                Day = x.Key.PlanTime.ToString("yy-MM-dd"),
                Type = x.Key.IsImportant ? "普通" : "重要",
                Value = x.Count(),
            }).ToList();

        }
    }
}

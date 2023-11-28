using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ERSZ.Components
{
    public class MainMenuComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string template = "", string currentItem = "")
        {
            return await Task.FromResult<IViewComponentResult>(View(template, currentItem));
        }
    }
}

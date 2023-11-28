using ERSZ.Infrastructure.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace ERSZ.Controllers
{
    public class ManageController : Controller
    {
        public ManageController()
        {
        }

        public IActionResult ExpiredInfo(int id, long longId, string stringId, string fileContainer, string submitUrl, string returnUrl, bool otherBool, int? OtherId)
        {
            var model = new ExpiredInfoVM()
            {
                Id = id,
                LongId = longId,
                StringId = stringId,
                ExpireSubmitUrl = submitUrl,
                FileContainerName = fileContainer,
                ReturnUrl = returnUrl,
            };
            
            return PartialView(model);
        }
    }
}

using ERSZ.Components;
using ERSZ.Core.Contracts;
using ERSZ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEkDitrictService ekDitrictService;

        public HomeController(ILogger<HomeController> logger, IEkDitrictService _ekDitrictService)
        {
            ekDitrictService = _ekDitrictService;
            _logger = logger;
        }

        [MenuItem("home")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewBag.CourtTypeList = await ekDitrictService.AllDistrictCourtsInformation();
            return View();
        }

        /*public IActionResult dlb()
        {
            var model = new EntitySelectVM();
            return View("SelectCourts", model);
        }*/

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

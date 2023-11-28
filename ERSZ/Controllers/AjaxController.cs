using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace ERSZ.Controllers
{
    public class AjaxController : Controller
    {
        private readonly INomenclatureService nomenclatureService;
        private readonly IEkDitrictService ekDitrictService;
        protected readonly ILogOperationService logOperation;

        public AjaxController(
            INomenclatureService _nomenclatureService,
            IEkDitrictService _ekDitrictService,
            ILogOperationService _logOperation)
        {
            nomenclatureService = _nomenclatureService;
            ekDitrictService = _ekDitrictService;
            logOperation = _logOperation;
        }

        [HttpGet]
        public IActionResult SearchEkatte(string query)
        {
            return new JsonResult(nomenclatureService.GetEkatte(query));
        }

        [HttpGet]
        public IActionResult GetEkatte(string id)
        {
            var ekatte = nomenclatureService.GetEkatteById(id);

            if (ekatte == null)
            {
                return BadRequest();
            }

            return new JsonResult(ekatte);
        }

        [HttpGet]

        public async Task<IActionResult> GetDistrictCourts(string mapId = "")
        {

            var courtsByDistrictMapId = await ekDitrictService.AllDistrictCourtsInformationByMapId(mapId ?? "");

            if (courtsByDistrictMapId == null)
            {
                return BadRequest();
            }

            return new JsonResult(courtsByDistrictMapId);
        }



        #region История на промените

        /// <summary>
        /// Извличане на история на промените за даден обект
        /// </summary>
        /// <param name="controller_name"></param>
        /// <param name="action_name"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public JsonResult Get_LogOperation(string controller_name, string action_name, string objectId)
        {
            var model = logOperation.SelectList(controller_name, action_name, objectId)
                .ToList()
                .Select(i => new
                {
                    oper_date = i.DateWrt.ToString("dd.MM.yyyy HH:mm"),
                    user = i.UserWrt,
                    oper = i.Operation,
                    id = i.Id
                });
            return Json(model);
        }

        /// <summary>
        /// Данни за обект от история на промените
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Get_LogOperationHTML(long id)
        {
            var data = logOperation.GetUserData(id);
            return Json(data);
        }


        #endregion
    }
}
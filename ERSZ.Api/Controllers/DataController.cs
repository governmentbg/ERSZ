using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ERSZ.Core.Contracts;
using ERSZ.Infrastructure.Contracts.Data;
using ERSZ.Infrastructure.Constants;

namespace ERSZ.Api.Controllers
{
    [Route("data")]
    public class DataController : BaseController
    {
        private readonly IDataService _dataService;
        private readonly ILogger<DataController> _logger;

        public DataController(
            IDataService dataService,
            ILogger<DataController> logger)
        {
            _dataService = dataService;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/text")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult Get()
        {
            return Content("ERSZ.API works!");
        }

        [HttpPost]
        [Route("SubmitInsertCase")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitInsertCase(ErszCaseModel model)
        {
            var saveresult = await _dataService.InsertCaseData(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitUpdateCase")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitUpdateCase(ErszCaseModel model)
        {
            var saveresult = await _dataService.UpdateCaseData(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitCaseDismissal")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitCaseDismissal(ErszCaseDismissalModel model)
        {
            var saveresult = await _dataService.InsertCaseDismissal(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitCaseSelectionProtokol")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitCaseSelectionProtokol(ErszCaseSelectionProtokolModel model)
        {
            //return Ok(new ErszResponseModel() { ResultCode = "test", Message = "Saved" });
            var saveresult = await _dataService.InsertCaseSelectionProtokol(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitCaseSession")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitCaseSession(ErszCaseSessionModel model)
        {
            //return Ok(new ErszResponseModel() { ResultCode = "test", Message = "Saved" });
            var saveresult = await _dataService.InsertCaseSession(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitCaseSessionAct")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitCaseSessionAct(ErszCaseSessionActModel model)
        {
            var saveresult = await _dataService.InsertCaseSessionAct(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("SubmitCaseSessionAmount")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> SubmitCaseSessionAmount(ErszCaseSessionAmountModel model)
        {
            //return Ok(new ErszResponseModel() { ResultCode = "test", Message = "Saved" });
            var saveresult = await _dataService.InsertCaseSessionAmount(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }

        [HttpPost]
        [Route("UpdateCaseSessionAmount")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ErszResponseModel))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErszResponseModel))]
        public async Task<IActionResult> UpdateCaseSessionAmount(ErszCaseSessionAmountModel model)
        {
            //return Ok(new ErszResponseModel() { ResultCode = "test", Message = "Saved" });
            var saveresult = await _dataService.UpdateCaseSessionAmount(model);
            if (!saveresult.IsSuccessfull)
            {
                return new JsonResult(new ErszResponseModel()
                {
                    ResultCode = saveresult.ErrorCode,
                    Message = saveresult.ErrorMessage
                });
            }
            return Ok(new ErszResponseModel()
            {
                ResultCode = ApiConstants.ResponseCode.Ok,
                Message = saveresult.ErrorMessage
            });
        }
    }
}


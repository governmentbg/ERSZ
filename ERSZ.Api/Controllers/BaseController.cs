using Microsoft.AspNetCore.Mvc;

namespace ERSZ.Api.Controllers
{
    /// <summary>
    /// API  - Базов контролер 
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BaseController : ControllerBase
    {
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lt.Project.Web.Controllers
{
    /// <summary>
    /// Consul健康检查
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet("Check")]
        public IActionResult Check() => Ok("ok");
    }
}

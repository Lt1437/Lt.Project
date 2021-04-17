using Lt.Project.Domain.IRepository;
using Lt.Project.Domain.TestCore.Entity;
using Lt.Project.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Lt.Project.Web.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRepository<Test> _testRepository;

        private readonly IRedisCacheManager _redisCacheManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="testRepository"></param>
        public TestController(IRepository<Test> testRepository, IRedisCacheManager redisCacheManager)
        {
            _testRepository = testRepository;
            _redisCacheManager = redisCacheManager;
        }

        /// <summary>
        /// 返回Success字符串
        /// </summary>
        /// <returns></returns>
        [HttpGet("Success")]
        public string Success()
        {
            return "Success";
        }

        /// <summary>
        /// 无权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("NotNeedAuth")]
        public string NotNeedAuth()
        {
            return "无权限验证，正常访问";
        }

        /// <summary>
        /// 有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("NeedAuth")]
        [Authorize]
        public string NeedAuth()
        {
            return "权限验证才可访问";
        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("AddTest")]
        public int AddTest(Test entity)
        {
            return _testRepository.Add(entity);
        }

        /// <summary>
        /// 测试redis
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpGet("Redis")]
        public async Task<IActionResult> Redis(string Name)
        {
            var key = $"Redis{Name}";
            Test test = new Test();
            if (_redisCacheManager.Get<object>(key) != null)
            {
                test = _redisCacheManager.Get<Test>(key);
            }
            else
            {
                test = new Test
                {
                    Name = Name,
                    Age = 18
                };
                _redisCacheManager.Set(key, test, TimeSpan.FromHours(2));//缓存2小时
            }

            return Ok(test);
        }
    }
}
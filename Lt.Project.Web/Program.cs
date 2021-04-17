using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Lt.Project.Web
{
    /// <summary>
    /// 程序入口
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 入口方法
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1、通过CreateHostBuilder(args)方法产生一个IhostBuilder的实例builder。
            //2、通过builder.Build方法产生一个Ihost的实例host。
            //3、通过host.Run方法，开始运行Web项目，此时可以响应各种请求了。
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 创建并配置主机
        /// </summary>
        /// <param name="args"></param>
        /// 1、使用预配置默认值初始化HostBuilder类的新实例builder（读取appsettings.json）
        /// 2、webBuilder.UseStartup<Startup>();读取startup类配置信息
        /// 3、通过builder.ConfigureWebHostDefaults(Action <IWebHostBuilder >)方法将IHostBuilder实例转变为一个Web主机性质的webbuilder（IWebHostBuilder)
        /// <returns></returns>
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)//创建WebHostBuilder
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup.Startup>();//指定启动类，用于依赖注入和中间件注册。
        //        });
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)//创建WebHostBuilder。
            .UseStartup<Startup.Startup>();//指定启动类，用于依赖注入和中间件注册。
    }
}
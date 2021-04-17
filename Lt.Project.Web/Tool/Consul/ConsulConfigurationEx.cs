using Consul;
using Microsoft.Extensions.Configuration;
using System;

namespace Lt.Project.Web.Tool.Consul
{
    /// <summary>
    /// Consul扩展类
    /// </summary>
    public static class ConsulConfigurationEx
    {
        /// <summary>
        /// Configuration 扩展方法ip和端口走的命令窗体控制台， Consul 配置走的是appsettings.json 文件
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegister(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(
                  (ConsulClientConfiguration c) =>
                  {
                      c.Address = new Uri(configuration["ServiceDiscovery:Consul:Address"]); //Consul服务中心地址
                      c.Datacenter = configuration["ServiceDiscovery:Consul:DataCenter"]; //指定数据中心，如果未提供，则默认为代理的数据中心。
                  }
              );
            string ip = configuration["ServiceDiscovery:BindAddress"];//部署到不同服务器的时候不能写成127.0.0.1或者0.0.0.0,因为这是让服务消费者调用的地址
            int port = int.Parse(configuration["ServiceDiscovery:Port"]);
            string checkUrl = configuration["ServiceDiscovery:Consul:CheckUrl"];
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(), //服务编号，不可重复
                Name = configuration["ServiceDiscovery:ServiceName"], //服务名称
                Port = port, //本程序的端口号
                Address = ip, //本程序的IP地址
                Tags = new string[] { "V1.0" },//标签
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), //服务停止后多久注销
                    Interval = TimeSpan.FromSeconds(5), //服务健康检查间隔
                    Timeout = TimeSpan.FromSeconds(5), //检查超时的时间
                    HTTP = $"http://{ip}:{port}{checkUrl}",//健康检查地址, //检查的地址
                }
            });;
        }
    }
}
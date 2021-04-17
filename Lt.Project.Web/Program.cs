using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Lt.Project.Web
{
    /// <summary>
    /// �������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ��ڷ���
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //1��ͨ��CreateHostBuilder(args)��������һ��IhostBuilder��ʵ��builder��
            //2��ͨ��builder.Build��������һ��Ihost��ʵ��host��
            //3��ͨ��host.Run��������ʼ����Web��Ŀ����ʱ������Ӧ���������ˡ�
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="args"></param>
        /// 1��ʹ��Ԥ����Ĭ��ֵ��ʼ��HostBuilder�����ʵ��builder����ȡappsettings.json��
        /// 2��webBuilder.UseStartup<Startup>();��ȡstartup��������Ϣ
        /// 3��ͨ��builder.ConfigureWebHostDefaults(Action <IWebHostBuilder >)������IHostBuilderʵ��ת��Ϊһ��Web�������ʵ�webbuilder��IWebHostBuilder)
        /// <returns></returns>
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)//����WebHostBuilder
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup.Startup>();//ָ�������࣬��������ע����м��ע�ᡣ
        //        });
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)//����WebHostBuilder��
            .UseStartup<Startup.Startup>();//ָ�������࣬��������ע����м��ע�ᡣ
    }
}
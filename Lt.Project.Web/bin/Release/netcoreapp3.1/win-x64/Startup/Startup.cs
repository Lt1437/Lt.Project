using Autofac;
using Autofac.Extensions.DependencyInjection;
using Lt.Project.Domain.IRepository;
using Lt.Project.EntityFrameworkCore.EntityFrameworkCore;
using Lt.Project.EntityFrameworkCore.Repository;
using Lt.Project.Redis;
using Lt.Project.Web.Tool.Consul;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Lt.Project.Web.Startup
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 承载注入实现的对象
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Autofac容器
        /// </summary>
        public static IContainer ApplicationContainer { get; set; }

        /// <summary>
        /// 此方法由运行时调用。使用此方法向容器添加服务
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region 初始

            //添加对控制器和与 API 相关的功能，但不是视图或页面的支持
            services.AddControllers();

            #endregion 初始

            #region 新增swagger

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Web API",
                    Description = "Web API"
                });
                // 为 Swagger 设置xml文档注释路径,需要显示哪个项目中的中文注释，就将xml加进来
                var xmlList = new List<string>()
                {
                    //"Lt.Project.Application.xml",
                    "Lt.Project.Domain.xml",
                    "Lt.Project.Web.xml"
                };

                foreach (var xmlFile in xmlList)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    option.IncludeXmlComments(xmlPath);
                }
            });

            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30),//允许token有效期误差范围
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = Const.Address,//Audience
                        ValidIssuer = Const.Address,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey))//拿到SecurityKey
                    };
                });

            #endregion 新增swagger

            #region 新增数据库读取配置

            //连接 mysql 数据库，添加数据库上下文，读取配置文件
            services.AddDbContext<CoreDbContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("Default"), e => e.MigrationsAssembly("Lt.Project.EntityFrameworkCore")));

            #endregion 新增数据库读取配置

            #region Autofac容器配置

            //初始化容器
            var builder = new ContainerBuilder();

            //管道寄居
            builder.Populate(services);

            //注册业务，将需要注册的项目放进来
            builder.RegisterAssemblyTypes(Assembly.Load("Lt.Project.EntityFrameworkCore")).AsImplementedInterfaces();//注册ef
            builder.RegisterAssemblyTypes(Assembly.Load("Lt.Project.Redis")).AsImplementedInterfaces();//注册redis

            //注册仓储，所有IRepository接口到Repository的映射
            //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            
            //构造
            ApplicationContainer = builder.Build();

            //将AutoFac反馈到管道中
            return new AutofacServiceProvider(ApplicationContainer);

            #endregion
        }

        /// <summary>
        /// 此方法由运行时调用。使用此方法配置HTTP请求管道
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //添加jwt认证（明确是你谁，确认是不是合法用户。常用的认证方式有用户名密码认证）
            //所以UseAuthentication要在UseAuthorization前面执行
            app.UseAuthentication();

            #region 初始

            //只会在Development环境下被调用
            if (env.IsDevelopment())
            {
                //当exception发生的时候, 这段程序就会处理它
                app.UseDeveloperExceptionPage();
            }
            //正式环境中
            else
            {
                app.UseExceptionHandler();
            }
            //The two steps are set up by app.UseRouting() and app.UseEndpoints().
            //The former will register the middleware that runs the logic to determine the route.
            //The latter will then execute that route
            //前者app.UseRouting()将注册运行逻辑的中间件以确定路由。后者app.UseEndpoints()随后将执行该路由
            app.UseRouting();

            //授权（明确你是否有某个权限。当用户需要使用某个功能的时候，系统需要校验用户是否需要这个功能的权限。）
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //添加了对属性路由的控制器支持
                endpoints.MapControllers();
            });

            #endregion 初始

            #region 新增swagger

            app.UseSwagger();
            //启用中间件服务
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "My Demo API (V 1.0)");
            });

            #endregion 新增swagger

            #region 新增consul

            Configuration.ConsulRegister();

            #endregion 新增consul
        }
    }
}
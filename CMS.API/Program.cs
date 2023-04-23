using Autofac;
using Autofac.Extensions.DependencyInjection;
using CMS.API.Extensions;
using CMS.Common.Helper;
using CMS.Models;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var MyAllowSpecificOrigins = "_MyAllowSubdomainPolicy";

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// 添加Swagger
builder.Services.AddSwaggerGen(options => {
    // Swagger文档注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
    // 添加校验
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "请输入授权Token：Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
// Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var tokenModel = builder.Configuration.GetSection("Jwt").Get<TokenModelJwt>();
    var secretByte = Encoding.UTF8.GetBytes(tokenModel.Secret);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = tokenModel.Issuer,

        ValidateAudience = true,
        ValidAudience = tokenModel.Audience,

        ValidateLifetime = true,

        IssuerSigningKey = new SymmetricSecurityKey(secretByte)
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
            //context.HandleResponse();

            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换

            //自定义返回的数据类型
            //context.Response.ContentType = "text/plain";
            ////自定义返回状态码，默认为401 我这里改成 200
            ////context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ////输出Json数据结果
            //context.Response.WriteAsync("expired");
            return Task.FromResult(0);// 返回异步结果，如果不带返回值使用Task.FromResult(0) 或 Task.FromResult(null)
        },
        // 403
        OnForbidden = context =>
        {
            //context.Response.ContentType = "text/plain";
            ////自定义返回状态码，默认为401 我这里改成 200
            ////context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ////输出Json数据结果
            //context.Response.WriteAsync("expired");
            return Task.FromResult(0);
        }
    };
});

// 跨域配置
builder.Services.AddCors(options => options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(o => true) // =AllowAnyOrigin()
            .AllowCredentials();
    })
);
//扩展方法,执行的是Extensions下面的SqlsugarSetup.AddSqlsugarSetup方法
builder.Services.SqlSugarSetup(builder.Configuration);

// Autofac依赖注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//  autofac
var hostBuilder = builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    /*builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();*/
    //  注册Service
    var assemblyServices = Assembly.Load("CMS.Services");
    var assemblyRepository = Assembly.Load("CMS.Repository");
    //  通过反射找到所有带Service的类进行注册
    builder.RegisterAssemblyTypes(assemblyServices).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
    //  注册Repository
    builder.RegisterAssemblyTypes(assemblyRepository).Where(a => a.Name.EndsWith("Repository")).AsSelf();
});

//  错误响应
builder.Services.AddMvc().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = c =>
    {
        Console.WriteLine(c);
        //处理错误消息拼接成字符串
        var errors = string.Join("&", c.ModelState.Values.Where(v => v.Errors.Count > 0)
          .SelectMany(v => v.Errors).Select(v => v.ErrorMessage).Where(v => v.IndexOf("field is required.") == -1));

        return new BadRequestObjectResult("error");
    };
});

/*//  字段校验注入
builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(Assembly.Load("验证类所在的位置"));
});*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();

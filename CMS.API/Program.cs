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
// ���Swagger
builder.Services.AddSwaggerGen(options => {
    // Swagger�ĵ�ע��
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);
    // ���У��
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "��������ȨToken��Bearer Token",
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
            //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ�����������ҪŶ������
            //context.HandleResponse();

            //�Զ����Լ���Ҫ���ص����ݽ����������Ҫ���ص���Json����ͨ������Newtonsoft.Json�����ת��

            //�Զ��巵�ص���������
            //context.Response.ContentType = "text/plain";
            ////�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
            ////context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ////���Json���ݽ��
            //context.Response.WriteAsync("expired");
            return Task.FromResult(0);// �����첽����������������ֵʹ��Task.FromResult(0) �� Task.FromResult(null)
        },
        // 403
        OnForbidden = context =>
        {
            //context.Response.ContentType = "text/plain";
            ////�Զ��巵��״̬�룬Ĭ��Ϊ401 ������ĳ� 200
            ////context.Response.StatusCode = StatusCodes.Status200OK;
            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ////���Json���ݽ��
            //context.Response.WriteAsync("expired");
            return Task.FromResult(0);
        }
    };
});

// ��������
builder.Services.AddCors(options => options.AddPolicy(MyAllowSpecificOrigins,
    builder =>
    {
        builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(o => true) // =AllowAnyOrigin()
            .AllowCredentials();
    })
);
//��չ����,ִ�е���Extensions�����SqlsugarSetup.AddSqlsugarSetup����
builder.Services.SqlSugarSetup(builder.Configuration);

// Autofac����ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//  autofac
var hostBuilder = builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    /*builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();*/
    //  ע��Service
    var assemblyServices = Assembly.Load("CMS.Services");
    var assemblyRepository = Assembly.Load("CMS.Repository");
    //  ͨ�������ҵ����д�Service�������ע��
    builder.RegisterAssemblyTypes(assemblyServices).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
    //  ע��Repository
    builder.RegisterAssemblyTypes(assemblyRepository).Where(a => a.Name.EndsWith("Repository")).AsSelf();
});

//  ������Ӧ
builder.Services.AddMvc().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = c =>
    {
        Console.WriteLine(c);
        //���������Ϣƴ�ӳ��ַ���
        var errors = string.Join("&", c.ModelState.Values.Where(v => v.Errors.Count > 0)
          .SelectMany(v => v.Errors).Select(v => v.ErrorMessage).Where(v => v.IndexOf("field is required.") == -1));

        return new BadRequestObjectResult("error");
    };
});

/*//  �ֶ�У��ע��
builder.Services.AddFluentValidation(opt =>
{
    opt.RegisterValidatorsFromAssembly(Assembly.Load("��֤�����ڵ�λ��"));
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

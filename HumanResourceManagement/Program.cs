
using HumanResourceManagement.Data;
using HumanResourceManagement.Middlewares;
using HumanResourceManagement.Services.Auth;
using HumanResourceManagement.Services.Departments;
using HumanResourceManagement.Services.Employees;
using HumanResourceManagement.Services.Users;
using HumanResourceManagement.Services.Attendances;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HumanResourceManagement.Services.Candidates;
using HumanResourceManagement.Services.Recruitments;
using HumanResourceManagement.Services.LeaveRequests;
using HumanResourceManagement.Services.Performances;
using HumanResourceManagement.Services.Dashboards;

namespace HumanResourceManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //JWT
            var jwt = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwt["Key"]!)
                )
                };
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                )
            );

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<DepartmentService>();
            builder.Services.AddScoped<AttendanceService>();
            builder.Services.AddScoped<CandidateService>();
            builder.Services.AddScoped<RecruitmentService>();
            builder.Services.AddScoped<LeaveRequestService>();
            builder.Services.AddScoped<PerformanceService>();
            builder.Services.AddScoped<DashboardService>();

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

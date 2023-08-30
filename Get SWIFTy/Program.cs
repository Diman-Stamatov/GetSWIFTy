using Get_SWIFTy.Database;
using Get_SWIFTy.Database.Interface;
using Get_SWIFTy.Helpers;
using Get_SWIFTy.Service;
using Get_SWIFTy.Service.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

namespace Get_SWIFTy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddControllers(
                options => options.InputFormatters.Insert(
                    options.InputFormatters.Count, new PlainTextFormatter()));

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "GetSwiftyAPI", Version = "v1" });
            });

            builder.Services.AddScoped<ISwiftDbAccess, SwiftDbAccess>();
            builder.Services.AddScoped<ISwiftServices, SwiftServices>();

            var app = builder.Build();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GetSwifty API V1");
                options.RoutePrefix = "api/swagger";
            });

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();

            });
            

            app.Run();
        }
    }
}
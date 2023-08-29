using Get_SWIFTy.Service;
using Get_SWIFTy.Service.Interface;
using Microsoft.OpenApi.Models;

namespace Get_SWIFTy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "GetSwiftyAPI", Version = "v1" });
            });

            builder.Services.AddScoped<ISwiftService, SwiftService>();

            var app = builder.Build();

            app.UseRouting();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "GetSwifty API V1");
                options.RoutePrefix = "api/swagger";
            });

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapDefaultControllerRoute();

            });

            app.Run();
        }
    }
}
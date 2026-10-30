using Domain;
using Infrastructure;
using Application;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using Presentation.Grpc.Interceptors;
using Presentation.Grpc.GrpcServices;
using Presentation.Rest.Middleware;
using Presentation.Rest.Controllers;

namespace Presentation.Rest
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {

            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            //REST
            services.AddControllers().AddJsonOptions(conf => conf.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ProductController>();

            //Grpc
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<LoggingInterceptor>();
                options.Interceptors.Add<ExceptionHandlingInterceptor>();
                options.Interceptors.Add<ValidationInterceptor>();
            }).AddJsonTranscoding();


            if (_env.IsDevelopment())
            {
                services.AddGrpcSwagger();
                services.AddSwaggerGen();
                services.AddEndpointsApiExplorer();
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {


            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Middleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapGrpcService<ProductGrpcService>();
                }
            );
        }
    }
}

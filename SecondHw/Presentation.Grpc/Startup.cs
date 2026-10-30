using Application;
using Domain;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;

namespace Presentation.Grpc
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

            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddControllers();


            services.AddFluentValidationAutoValidation();

            if (_env.IsDevelopment())
            {
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

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}

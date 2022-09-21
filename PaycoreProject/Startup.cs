using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaycoreProject.Authenticate;
using PaycoreProject.Extensions;
using PaycoreProject.Helpers;
using PaycoreProject.Model;
using PaycoreProject.Repository;
using PaycoreProject.Services.Abstract;
using PaycoreProject.Services.Concrete;
using PaycoreProject.Validators;
using QueueManagement.Mail;
using QueueManagement.RabbitMQ;

namespace PaycoreProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<UserValidator>());

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            services.AddJwtBearerAuthentication(Configuration);

            services.AddCustomizeSwagger();
            var connStr = Configuration.GetConnectionString("PostgreSqlConnection");
            services.AddNHibernatePosgreSql(connStr);
            services.AddScoped<IHibernateRepository<User>, HibernateRepository<User>>();
            services.AddScoped<IHibernateRepository<Category>, HibernateRepository<Category>>();
            services.AddScoped<IHibernateRepository<Product>, HibernateRepository<Product>>();
            services.AddScoped<IHibernateRepository<GiveOffer>, HibernateRepository<GiveOffer>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaycoreProject", Version = "v1" });
            //});
            var mapperconfig = new MapperConfiguration(cfg =>
            { 
            cfg.AddProfile(new AutoMapperProfile());
            });
            services.AddSingleton(mapperconfig.CreateMapper());
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddSingleton<MailService>();
            services.AddSingleton<RabbitMQService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaycoreProject v1"));
            }

           
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
           
            //app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

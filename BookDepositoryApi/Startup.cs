using BookDepositoryApi.Interfaces;
using BookDepositoryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Настройка подключения к базе данных
        services.AddDbContext<BookContext>(options =>
            options.UseSqlite("Data Source=books.db"));

        services.AddScoped<IBookCatalog, DbBookRepository>();
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookDepository API", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        // Включение Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookDepository API V1");
            c.RoutePrefix = string.Empty; // Настройка корневого URL для Swagger UI
        });
    }
}
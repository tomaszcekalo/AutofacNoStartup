//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
//----------------------------------------

using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacNoStartup;

var appsettings = $"appsettings.{System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json";

var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddJsonFile(
                    appsettings,
                    optional: true)
                .AddEnvironmentVariables()
                .Build();

var host = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json");
    })
    .ConfigureWebHostDefaults(webHostBuilder =>
    {
        webHostBuilder.Configure(app =>
        {
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapControllers());
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseMiddleware<ExceptionHandlerMiddleware>();
        });

        webHostBuilder.ConfigureServices(services =>
        {
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddHttpClient<ISalesManagoService, FakeSalesManagoService>();
            //services.Configure<SalesManagoSettings>(options =>
            //{
            //    options.ApiSecret = "a";
            //    options.ClientId = "b";
            //    options.Endpoint = "c";
            //    options.Owner = "d";
            //});
            //services.AddOptions<SalesManagoSettings>("SalesManago");
            //services.Configure<SalesManagoSettings>(options => configuration.GetSection("MySettings").Bind(options));
            //var configuration = Configuration.GetSection("SalesManago");
            //services.AddSalesManago(webHostBuilder.Configuration.GetSection("SalesManago"));
        });
    })
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        //builder.RegisterType<SalesManagoSettings>();
        //builder.RegisterType<FakeSalesManagoService>().As<ISalesManagoService>();

        var config = configuration.GetSection("SalesManago").Get<SalesManagoSettings>();
        builder.Register(c => new FakeSalesManagoService(config, c.Resolve<HttpClient>()))
            .As<ISalesManagoService>();
    }
    )
    .Build();
await host.RunAsync();
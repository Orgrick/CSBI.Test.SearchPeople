using AutoMapper;
using CSBI.Test.API.DependencyInjection;
using CSBI.Test.API.Filters;
using CSBI.Test.API.Mappings;
using CSBI.Test.DataAccess.EF;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

var srv = builder.Services;
var cnf = builder.Configuration;

builder.Host.UseSerilog((context, services, configuration) => configuration
.ReadFrom.Configuration(cnf));

srv.AddApplication(opt =>
{
    opt.ConnectionString = cnf.GetConnectionString("SQLServer");
});

srv.AddAutoMapper(opt => opt.AddProfile<MappingProfile>());
srv.AddHealthChecks();

srv.AddControllers(opt => opt.Filters.Add<ExceptionFilter>()).AddNewtonsoftJson();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;

    try
    {
        var ctx = sp.GetRequiredService<ClientsContext>();
        ctx.Database.EnsureCreated();
        var mapper = sp.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An error occurred while app initialization");
    }
}

app.Run();

using Serilog;
using IoTControlTower.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureAPI(configuration);
builder.Services.AddInfrastructureJWT(configuration);
builder.Services.AddInfrastructureSwagger(configuration);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (true)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IotControTower.API v1"));
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(opt => opt.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowAnyOrigin());
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();

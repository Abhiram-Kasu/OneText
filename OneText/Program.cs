using OneText.Application.Hubs;
using OneText.Application.Startup;
using Spark.Library.Config;
using Spark.Library.Environment;

EnvManager.LoadConfig();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetupSparkConfig();

builder.Services.AddAppServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(options => options.SetIsOriginAllowed(_ => true).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapHub<MessagingHub>("/chat/realtime");

app.Services.RegisterScheduledJobs();
app.Services.RegisterEvents();



app.Run();

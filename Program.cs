using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // Add controllers for Web API

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
var url = $"http://0.0.0.0:{port}";
var target = Environment.GetEnvironmentVariable("TARGET") ?? "World";

FirebaseAdmin.FirebaseApp.Create(new FirebaseAdmin.AppOptions()
{
    Credential = GoogleCredential.FromJson("{\"type\": \"api_key\", \"api_key\": \"AIzaSyD76gpTaHk0pTjSY4KFzEp6RUEWWKdRQAc\"}")
});

builder.Services.AddSingleton(FirebaseAdmin.FirebaseApp.DefaultInstance);

var app = builder.Build();

app.MapGet("/", () => $"Hello {target}!");

app.MapControllers(); // Map controllers for Web API
app.Run(url);
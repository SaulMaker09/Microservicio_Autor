using Tienda.Microservicios.Autor.Api.extensions; // si lo necesitas

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Swagger básico
builder.Services.AddSwaggerGen();           // Swagger generator

builder.Services.AddCustomServices(builder.Configuration); // Tu DI personalizada

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Puerto de tu app React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });

    options.AddPolicy("PermitirTodo", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();
app.UseCors("PermitirTodo");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();       // Activa el middleware Swagger
    app.UseSwaggerUI();     // Habilita la UI (https://localhost:xxxx/swagger)
}

app.UseCors(); // Agregar antes de UseAuthorization()


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR INTERNO: " + ex.Message);
        throw; // sigue lanzando para que dev devuelva el 500
    }
});


app.Run();

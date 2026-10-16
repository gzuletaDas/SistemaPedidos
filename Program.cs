using Microsoft.EntityFrameworkCore;
using SistemaPedidos.Data;
using SistemaPedidos.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Acceso a datos: el DbContext apunta a SQL Server ---
var cadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Falta la connection string 'DefaultConnection' en appsettings.json.");

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(cadenaConexion));

// --- Inyección de dependencias: cada servicio detrás de su interfaz ---
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// --- Controllers y documentación ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new()
    {
        Title = "Sistema de Pedidos API",
        Version = "v1",
        Description = "Proyecto integrador: Clientes, Productos y Pedidos sobre ASP.NET Core + EF Core + SQL Server."
    });

    // Incluye los comentarios XML (<summary>) en Swagger.
    var archivoXml = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var rutaXml = Path.Combine(AppContext.BaseDirectory, archivoXml);
    if (File.Exists(rutaXml))
    {
        opciones.IncludeXmlComments(rutaXml);
    }
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema de Pedidos API v1");
        opciones.RoutePrefix = "swagger";
    });

    // En desarrollo, la raíz redirige a la documentación interactiva.
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

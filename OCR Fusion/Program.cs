using OCR_Fusion;
using OCR_Fusion.Database;
using System.Reflection;
using System.Runtime.CompilerServices;

var assembly = Assembly.GetExecutingAssembly();

foreach (var type in assembly.GetTypes()) {
    var registerAttribute = type.GetCustomAttribute<RegisterAttribute>();

    if (registerAttribute != null) {
        Multiplex.Register(type, registerAttribute);
    }
    
}


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy => {
                          policy.WithOrigins("*");
                      });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


MangoCRUD db = new("test");
Utils.SetDatabaseInterface(db);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

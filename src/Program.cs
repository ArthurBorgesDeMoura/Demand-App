
using IDemandApp.Endpoints.Costumers;
using IDemandApp.Endpoints.Orders;
using IDemandApp.Endpoints.Products;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:ConnectionString"]);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("App", "IDemandApp")
        .WriteTo.Console()
        .WriteTo.MSSqlServer(
    connectionString: builder.Configuration["Database:ConnectionString"],
    sinkOptions: new MSSqlServerSinkOptions()
    {
        AutoCreateSqlTable = true,
        TableName = "Logs"
    });

});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
    options.AddPolicy("EmployeePolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("EmployeeCode"));
    options.AddPolicy("CostumerPolicy", p =>
        p.RequireAuthenticatedUser().RequireClaim("Cpf"));
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["Security:Issuer"],
        ValidAudience = builder.Configuration["Security:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Security:Key"]))
    };
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handle);

app.MapMethods(CostumerPost.Template, CostumerPost.Methods, CostumerPost.Handle);
app.MapMethods(CostumerGet.Template, CostumerGet.Methods, CostumerGet.Handle);

app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handle);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handle);

app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handle);
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handle);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handle);

app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handle);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handle);
app.MapMethods(ProductGetById.Template, ProductGetById.Methods, ProductGetById.Handle);
app.MapMethods(ProductGetShowcase.Template, ProductGetShowcase.Methods, ProductGetShowcase.Handle);
app.MapMethods(ProductGetReport.Template, ProductGetReport.Methods, ProductGetReport.Handle);

app.MapMethods(OrderPost.Template, OrderPost.Methods, OrderPost.Handle);
app.MapMethods(OrderGetByCostumerId.Template, OrderGetByCostumerId.Methods, OrderGetByCostumerId.Handle);
app.MapMethods(OrderGetById.Template, OrderGetById.Methods, OrderGetById.Handle);
app.UseExceptionHandler("/error");
//app.Map("/error", (HttpContext http) =>
//{
//    var error = http.Features?.Get<IExceptionHandlerFeature>()?.Error;
//    if (error != null)
//    {
//        if (error is SqlException)
//            return Results.Problem(title: "Database is Down", statusCode: 500);
//        if (error is BadHttpRequestException ex2)
//            return Results.Problem(title: "Failed to read parameter, see all the information sent", statusCode: 422);
//        if (error is Exception ex)
//            return Results.Problem(title: ex.Message, statusCode: 500);
        
//    }
//    return Results.Problem(title: "An error occured", statusCode: 500);
//});

app.Run();

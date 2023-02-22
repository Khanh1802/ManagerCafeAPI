using ManagerCafe.Applications.Profiles;
using ManagerCafe.Applications.Service;
using ManagerCafe.Contracts.Dtos.UsersDtos.ValidateUserDto;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Domain.Repositories;
using ManagerCafe.Share.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(option => option
    .AddDefaultPolicy(x => x
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));
//C1:
//var connectionString = builder.Configuration.GetConnectionString("ManagerCafe");
//builder.Services.AddDbContext<ManagerCafeDbContext>(options =>
//{
//    options.UseSqlServer(connectionString);
//});

//C2:
builder.Services.AddDbContextPool<ManagerCafeDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("ManagerCafe")));

builder.Services.Configure<Setting>(builder.Configuration.GetSection("AppSettings"));
var serectKey = builder.Configuration["AppSettings:SecretKey"];
var serectKeyBytes = Encoding.UTF8.GetBytes(serectKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(otp =>
otp.TokenValidationParameters = new TokenValidationParameters
{
    // tu cap Token
    ValidateIssuer = false,
    ValidateAudience = false,

    TryAllIssuerSigningKeys = true,
    IssuerSigningKey = new SymmetricSecurityKey(serectKeyBytes),
    ClockSkew = TimeSpan.Zero
});

builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IWareHouseRepository, WareHouseRepository>();
builder.Services.AddTransient<IWareHouseService, WareHouseService>();
builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IInventoryTransactionRepository, InventoryTransactionRepository>();
builder.Services.AddTransient<IInventoryTransactionService, InventoryTransactionService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserTypeRepository, UserTypeRepository>();
builder.Services.AddTransient<IUserTypeService, UserTypeService>();
builder.Services.AddTransient<IUserValidate, UserValidate>();
builder.Services.AddTransient<IUserCacheService, UserCacheService>();
builder.Services.AddTransient<IOrderCacheService, OrderCacheService>();
builder.Services.AddTransient<IOrderDetailCacheService, OrderDetailCacheService>();
builder.Services.AddTransient<IOrderDetailService, OrderDetailService>();
builder.Services.AddAutoMapper(typeof(ProductProfile));
builder.Services.AddAutoMapper(typeof(WareHouseProfile));
builder.Services.AddAutoMapper(typeof(InventoryProfile));
builder.Services.AddAutoMapper(typeof(InventoryTransactionProfile));
builder.Services.AddAutoMapper(typeof(UserTypeProfile));
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
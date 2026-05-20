using Supabase;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregamos el servicio de Razor Pages
builder.Services.AddRazorPages();

// 2. CONFIGURACIÓN DE SUPABASE 
var supabaseUrl = "https://oqisxyppfjcwkiqwzljs.supabase.co";
var supabaseKey = "sb_publishable_nag9nKokxueldG_virISCA_esDFonav";

// Esto permite que todos tus archivos de C# puedan usar Supabase
builder.Services.AddScoped(_ => new Supabase.Client(supabaseUrl, supabaseKey));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
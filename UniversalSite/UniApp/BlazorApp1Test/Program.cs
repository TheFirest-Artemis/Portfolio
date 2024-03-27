using BlazorApp1Test.Components;
using System.Net;

namespace BlazorApp1Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddHttpClient();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapGet("/weathercoord", (string place) => {
                if (place != "")
                {
                    string line;
                    string words;
                    WebRequest request = WebRequest.Create($"https://geocode-maps.yandex.ru/1.x/?apikey=f86e21d4-d60c-4f96-b1f6-03c0639a82ee&geocode={place}");
                    WebResponse response = request.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            line = reader.ReadToEnd();
                            try
                            {
                                words = (line.Split("<pos>")[1]).Split("</pos>")[0];
                            }
                            catch (Exception)
                            {
                                return "не найдено";
                                throw;
                            }
                        }
                    }
                    return words;
                }
                return "";
            });

            app.Run();
        }
    }
}

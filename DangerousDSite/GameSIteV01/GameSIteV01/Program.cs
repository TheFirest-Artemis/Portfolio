using GameSIteV01.Data;
using GameSIteV01.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace GameSIteV01
{

    public class Program
    {

        public static void Main(string[] args)
        {
            var people = new List<Person>
            {
    new Person("artem@gmail.com", "12345", "Firest"),
    new Person("bob@gmail.com", "55555", "Lox")
};

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = "/login");

            builder.Services.AddDbContext<AplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
            );

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            /*app.MapGet("/login1", async (HttpContext context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                // html-форма для ввода логина/пароля
                string loginForm = @"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8' />
                </head>
                <body>
                    <h2>Login Form</h2>
                    <form method='post'>
                        <p>
                            <label>Email</label><br />
                            <input name='email' />
                        </p>
                        <p>
                            <label>Password</label><br />
                            <input type='password' name='password' />
                        </p>
                        <input type='submit' value='Login' />
                    </form>
                </body>
                </html>";
                await context.Response.WriteAsync(loginForm);
            });*/

            app.MapPost("/login", async (string? returnUrl, HttpContext context) =>
            {
                // получаем из формы email и пароль
                var form = context.Request.Form;
                // если email и/или пароль не установлены, посылаем статусный код ошибки 400
                if (!form.ContainsKey("email/username") || !form.ContainsKey("password"))
                    return Results.BadRequest("Email/Username и/или пароль не установлены");

                string emailUsername = form["email/username"];
                string password = form["password"];

                SqlConnection connection = new SqlConnection("Server=ms-sql-9.in-solve.ru;Database=1gb_gamehack;User Id=1gb_firest;Password=edd32f4b678;");
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM [1gb_gamehack].[dbo].[Users] WHERE Email LIKE '{emailUsername}' OR UserName LIKE '{emailUsername}' AND Password LIKE '{password}'";
                SqlDataReader reader = command.ExecuteReader();

                // находим пользователя 
                //Person? person = people.FirstOrDefault(p => (p.Email == emailUsername && p.Password == password) || (p.Username == emailUsername && p.Password == password));
                // если пользователь не найден, отправляем статусный код 401
                if (!reader.HasRows) return Results.Redirect("/login");
                reader.Read();

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, reader.GetValue(1).ToString()), new Claim(ClaimTypes.Email, reader.GetValue(2).ToString()) };
                // создаем объект ClaimsIdentity
                reader.Close();
                connection.Close();
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                // установка аутентификационных куки
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Results.Redirect(returnUrl ?? "/");
            });

            app.Map("/logingame", (string emailUsername, string password) =>
            {
                // находим пользователя 
                //Person? person = people.FirstOrDefault(p => (p.Email == emailUsername && p.Password == password) || (p.Username == emailUsername && p.Password == password));
                SqlConnection connection = new SqlConnection("Server=ms-sql-9.in-solve.ru;Database=1gb_gamehack;User Id=1gb_firest;Password=edd32f4b678;");
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM [1gb_gamehack].[dbo].[Users] WHERE Email LIKE '{emailUsername}' OR UserName LIKE '{emailUsername}' AND Password LIKE '{password}'";
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();

                PersonAuth personAuth = new PersonAuth();

                if (!reader.HasRows) { personAuth.answerCode = false; }
                else { personAuth.answerCode = true; personAuth.email = reader.GetValue(2).ToString(); personAuth.username = reader.GetValue(1).ToString(); }
                reader.Close();
                connection.Close();

                return JsonConvert.SerializeObject(personAuth);
            });


            app.MapGet("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.Redirect("/");
            });

            app.Map("/data", [Authorize] () => "Hello world!");

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }

    record class Person(string Email, string Password, string Username);

    class PersonAuth
    {
        public bool answerCode = false;
        public string email;
        public string username;
    }
}
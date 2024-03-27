using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameSIteV01.Data;
using GameSIteV01.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authentication;

namespace GameSIteV01.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public IList<UserModel> UserModel { get; set; } = default!;

        public void OnGet()
        {

        }

//        public async Task<IResult> Login(string? returnUrl, HttpContext context)
//        {
//            var people = new List<Person>
//            {
//    new Person("artem@gmail.com", "12345", "Firest"),
//    new Person("bob@gmail.com", "55555", "Lox")
//};
//            var form = context.Request.Form;

//            string email = form["email"];
//            string password = form["password"];

//            // находим пользователя 
//            Person? person = people.FirstOrDefault(p => p.Email == email && p.Password == password);

//            //Person person = await db.Users.FirstOrDefaultAsync(u => u.Email == context.Request..Email && u.Password == model.Password);
//                if (person != null)
//                {
//                    if (person is null) return Results.Unauthorized();

//                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, person.Email) };
//                    // создаем объект ClaimsIdentity
//                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
//                    // установка аутентификационных куки
//                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
//                    return Results.Redirect(returnUrl ?? "/");
//                }
//                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
//            return Results.Unauthorized();
//        }
    }
}

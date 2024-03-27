using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GameSIteV01.Data;
using GameSIteV01.Models;
using GameSIteV01.Migrations;
using Microsoft.Data.SqlClient;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace GameSIteV01.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly AplicationDbContext _context;

        public RegistrationModel(AplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserModel UserModel { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //string referal = UserModel.UserName + ";\n;" + UserModel.Email;
            //byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(referal);
            //byte[] outputBytes = new byte[inputBytes.Length];
            //string key = "podjjlkdfbjhbdfhkfvbuzdsvceycvjdhzbshcujhzhcsdufviyag7ifgawfuywbafubakncbuwyyegr672qw";
            //byte[] _key = System.Text.Encoding.Unicode.GetBytes(key);
            //for (int i = 0; i < inputBytes.Length; i++) { outputBytes[i] = (byte)(inputBytes[i] ^ _key[i % _key.Length]); }
            //referal = Convert.ToBase64String(outputBytes);

            UserModel.RegistrationDate = DateTime.Now;
            if (/*!ModelState.IsValid || */_context.Users == null || UserModel == null)
            {
                return Page();
            }

            SqlConnection connection = new SqlConnection("Server=ms-sql-9.in-solve.ru;Database=1gb_gamehack;User Id=1gb_firest;Password=edd32f4b678;");
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = $"SELECT * FROM [1gb_gamehack].[dbo].[Users] WHERE Email LIKE '{UserModel.Email}' OR UserName LIKE '{UserModel.UserName}'";
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            if (reader.HasRows)
            {
                reader.Close();
                connection.Close();
                return RedirectToPage("/index");
            }

            _context.Users.Add(UserModel);
            await _context.SaveChangesAsync();

            reader.Close();
            connection.Close();

            return RedirectToPage("/Login");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GameSIteV01.Data;
using GameSIteV01.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameSIteV01.Pages
{
    public class UserAccountModel : PageModel
    {
        private readonly GameSIteV01.Data.AplicationDbContext _context;

        public UserAccountModel(GameSIteV01.Data.AplicationDbContext context)
        {
            _context = context;
        }

      public UserModel UserModel { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync()
        {
            string username = "";
                if (User.Identity.IsAuthenticated && _context.Users != null)
                {
                    username = User.Identity?.Name;
                }
                else
                {
                    return (IActionResult)Results.Redirect("/login");
                }

            var usermodel = await _context.Users.FirstOrDefaultAsync(m => m.UserName == username);
            if (usermodel == null)
            {
                return NotFound();
            }
            else 
            {
                UserModel = usermodel;
            }
            return Page();
        }

        //public void OnGet()
        //{
        //    return Results.Redirect("/login")
        //}
    }
}

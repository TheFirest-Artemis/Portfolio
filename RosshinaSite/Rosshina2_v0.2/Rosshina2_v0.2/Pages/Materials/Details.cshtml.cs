using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rosshina2_v0._2.Data;
using Rosshina2_v0._2.Models;

namespace Rosshina2_v0._2.Pages.Materials
{
    public class DetailsModel : PageModel
    {
        private readonly Rosshina2_v0._2.Data.ShinaContext _context;

        public DetailsModel(Rosshina2_v0._2.Data.ShinaContext context)
        {
            _context = context;
        }

        public MaterialsModel MaterialsModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.MaterialsModels == null)
            {
                return NotFound();
            }

            var materialsmodel = await _context.MaterialsModels.FirstOrDefaultAsync(m => m.Id == id);
            if (materialsmodel == null)
            {
                return NotFound();
            }
            else
            {
                MaterialsModel = materialsmodel;
            }
            return Page();
        }

    }
}

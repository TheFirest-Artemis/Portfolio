using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosshina2_v0._2.Data;
using Rosshina2_v0._2.Models;

namespace Rosshina2_v0._2.Pages.Materials
{
    public class IndexModel : PageModel
    {
        private readonly Rosshina2_v0._2.Data.ShinaContext _context;

        public IndexModel(Rosshina2_v0._2.Data.ShinaContext context)
        {
            _context = context;
        }

        public IList<MaterialsModel> MaterialsModel { get; set; } = default!;

        public SelectList? Type { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ProductType { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            IQueryable<string> typeQuery = from m in _context.MaterialsModels
            orderby m.Type
                                           select m.Type;

            var products = from m in _context.MaterialsModels
                           select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(ProductType))
            {
                products = products.Where(x => x.Type == ProductType);
            }

            Type = new SelectList(await typeQuery.Distinct().ToListAsync());

            MaterialsModel = await products.ToListAsync();
        }
    }
}

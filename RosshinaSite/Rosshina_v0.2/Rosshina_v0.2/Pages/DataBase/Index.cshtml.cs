using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rosshina_v0._2.Data;
using Rosshina_v0._2.Models;

namespace Rosshina_v0._2.Pages.DataBase
{
    public class IndexModel : PageModel
    {
        private readonly Rosshina_v0._2.Data.ShinaContext _context;

        public IndexModel(Rosshina_v0._2.Data.ShinaContext context)
        {
            _context = context;
        }

        public IList<CompaniesModel> CompaniesModel { get;set; } = default!;

        public SelectList? Type { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? ProductType { get; set; }

        public SelectList? Manufacturer { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? CompanyManufacturer { get; set; }

        public SelectList? Trademark { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? CompanyTrademark { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            bool isEng = (string)RouteData.Values["lang"] == "eng";
            //if (_context.CompaniesModels != null)
            //{
            //    CompaniesModel = await _context.CompaniesModels.ToListAsync();
            //}
            IQueryable<string> manufacturerQuery = from m in _context.CompaniesModels
                                            orderby m.Manufacturer
                                            select m.Manufacturer;

            IQueryable<string> trademarkQuery = from m in _context.CompaniesModels
                                                orderby m.Trademark
                                                select m.Trademark;

            IQueryable<string> typeQuery;
            if (isEng)
            {
                typeQuery = from m in _context.CompaniesModels
                                               orderby m.Type
                                               select m.Type;
            }
            else
            {
                typeQuery = from m in _context.CompaniesModels
                                               orderby m.TypeRus
                                               select m.TypeRus;
            }

            var products = from m in _context.CompaniesModels
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                products = products.Where(s => s.GeneralizedInfo.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(CompanyManufacturer))
            {
                products = products.Where(x => x.Manufacturer == CompanyManufacturer);
            }

            if (!string.IsNullOrEmpty(CompanyTrademark))
            {
                products = products.Where(x => x.Trademark == CompanyTrademark);
            }

            if (isEng)
            {
                if (!string.IsNullOrEmpty(ProductType))
                {
                    products = products.Where(x => x.Type == ProductType);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ProductType))
                {
                    products = products.Where(x => x.TypeRus == ProductType);
                }
            }

            Type = new SelectList(await typeQuery.Distinct().ToListAsync());

            Trademark = new SelectList(await trademarkQuery.Distinct().ToListAsync());

            Manufacturer = new SelectList(await manufacturerQuery.Distinct().ToListAsync());

            CompaniesModel = await products.ToListAsync();
        }
    }
}

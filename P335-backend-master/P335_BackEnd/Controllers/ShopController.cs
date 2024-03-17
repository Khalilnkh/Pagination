using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P335_BackEnd.Data;
using P335_BackEnd.Models;
using P335_BackEnd.Services;

namespace P335_BackEnd.Controllers
{
    public class ShopController : Controller
    {
        private AppDbContext _dbContext;

        public ShopController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int page, string order)
        {
            if (page <= 0) page = 1;

            int productsPerPage = 3;
            var productCount = _dbContext.Products.Count();

            int totalPageCount = (int)Math.Ceiling(((decimal)productCount / productsPerPage));

            ViewBag.SaleProducts = _dbContext.Products.Include(x => x.SaleProduct).ToList();

            var model = new ShopIndexVM();

            if (order == "asc")
            {
                model.Products = _dbContext.Products
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * productsPerPage)
                    .Take(productsPerPage)
                    .ToList();
            }
            else
            {
                model.Products = _dbContext.Products
                    .OrderByDescending(x => x.Id)
                    .Skip((page - 1) * productsPerPage)
                    .Take(productsPerPage)
                    .ToList();
            }

            ViewBag.Order = order;
            model.TotalPageCount = totalPageCount;
            model.CurrentPage = page;

            return View(model);
        }
    }
}
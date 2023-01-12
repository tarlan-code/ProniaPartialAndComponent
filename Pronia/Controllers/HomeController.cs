using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pronia.DAL;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IQueryable<Product> featuredproducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).Take(4).AsQueryable();
            IQueryable<Product> latestproducts = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).OrderByDescending(o => o.Date).Take(8).AsQueryable();

            HomeVM homeVM = new HomeVM
            {
                MainSliders = _context.MainSliders,
                ShippingAreas = _context.ShippingAreas.ToList(),
                TestimonialArea = _context.TestimonialAreas.FirstOrDefault(),
                Testimonials = _context.Testimonials,
                Brands = _context.Brands,
                Banners = _context.Banners.OrderBy(o => o.Order),
                FeaturedProducts = featuredproducts,
                LatestProducts = latestproducts
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult LoadProducts(int skip,int take)
        {


            var products = _context.Products.Where(p => p.IsDeleted == false).Include(p => p.ProductImages).Skip(skip).Take(take);
           
      
            return PartialView("_ProductPartial", products);
        }

        [HttpPost]
        public IActionResult QuickView(int? Id)
        {
            if (Id is null || Id <= 0) return BadRequest(); 

            var product = _context.Products.Include(p => p.ProductColors).ThenInclude(pc => pc.Color).Include(p => p.ProductSizes).ThenInclude(ps => ps.Size).Include(p => p.ProductImages).FirstOrDefault(p => p.Id == Id);

            if(product is null) return NotFound();
            return PartialView("_QuickViewPartial",product);
        }


    }
}
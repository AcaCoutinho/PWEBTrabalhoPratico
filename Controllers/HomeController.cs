using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWEBTP.Data;
using PWEBTP.Models;
using System.Diagnostics;

namespace PWEBTP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Carros.Where(c => c.Disponivel == true && c.EmDestaque == true).ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
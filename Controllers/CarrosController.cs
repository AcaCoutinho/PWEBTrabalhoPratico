using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PWEBTP.Data;
using PWEBTP.Models;
using PWEBTP.Models.Helpers;
using PWEBTP.Models.ViewModels;

namespace PWEBTP.Controllers
{
    public class CarrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarrosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carroes
        public async Task<IActionResult> Index(bool? disponivel)
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias.ToList(), "Id", "Name");

            if(disponivel != null)
            {
                if(disponivel == true)
                {
                    ViewData["Title"] = "Lista de Carros Ativos";
                }
                else
                {
                    ViewData["Title"] = "Lista de Carros Inativos";
                }
                return View(await _context.Carros.Include("Categoria").Where(c => c.Disponivel == disponivel).ToListAsync());
            }
            ViewData["Title"] = "Lista de Carros"; 
            return View(await _context.Carros.Include("Categoria").ToListAsync());
        }

        // GET: Carroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carros == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.Include("Categoria").FirstOrDefaultAsync(m => m.Id == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        // GET: Carroes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Name");
            return View();
        }

        // POST: Carroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Preco,Descricao,Disponivel,EmDestaque,CategoriaId")] Carro carro)
        {
            ViewData["ListaDeCategorias"] = new SelectList(_context.Categorias.ToList(), "Id", "Name");

            ModelState.Remove(nameof(carro.Categoria));
            if (ModelState.IsValid)
            {
                _context.Add(carro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carro);
        }

        // GET: Carroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carros == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.FindAsync(id);
            if (carro == null)
            {
                return NotFound();
            }
            return View(carro);
        }

        // POST: Carroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Funcionario, Gestor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Preco,Descricao,Disponivel,EmDestaque")] Carro carro)
        {
            if (id != carro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarroExists(carro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(carro);
        }

        // GET: Carroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carros == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carro == null)
            {
                return NotFound();
            }

            return View(carro);
        }

        [HttpPost]
        public async Task<IActionResult> Search([Bind("TextoAPesquisar")]
            PesquisaCarroViewModel pesquisaCarro, int CategoriaId)
        {
            if (string.IsNullOrWhiteSpace(pesquisaCarro.TextoAPesquisar))
            {
                pesquisaCarro.ListaDeCarros = await _context.Carros.Include("Categoria").
                    Where(c => c.CategoriaId == CategoriaId).ToListAsync();
            }
            else
            {

                pesquisaCarro.ListaDeCarros = await _context.Carros.Include("Categoria").Where(
                    c => c.Name.Contains(pesquisaCarro.TextoAPesquisar)
                    || c.Descricao.Contains(pesquisaCarro.TextoAPesquisar)
                    || c.CategoriaId == CategoriaId
                    ).ToListAsync();

            }
            pesquisaCarro.NumResultados = pesquisaCarro.ListaDeCarros.Count;

            ViewData["Title"] = "Os nossos Carros";

            return View(pesquisaCarro);
        }

        public async Task<IActionResult> Carrinho()
        {
            var carrinhoDeCompras = HttpContext.Session.GetJson<Carrinho>("CarrinhoDeCompras") ?? new Carrinho();
            return View(carrinhoDeCompras);
        }
  
        public IActionResult Comprar(int? id)
        {

            var carro = _context.Carros.Where(u => u.Id == id).FirstOrDefault();

            if (carro == null)
                return RedirectToAction(nameof(Index));

            var CarrinhoDeCompras = HttpContext.Session.GetJson<Carrinho>("CarrinhoDeCompras") ?? new Carrinho();

            CarrinhoDeCompras.AddItem(carro, 1);

            HttpContext.Session.SetJson("CarrinhoDeCompras", CarrinhoDeCompras);

            return RedirectToAction("Carrinho");
        }

        public async Task<IActionResult> AlterarQuantidadeCarrinhoItem(int? carroId, int? quantidade)
        {
            if (carroId == null || _context.Carros == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.Include("Categoria")
                .FirstOrDefaultAsync(m => m.Id == carroId);
           //var carro = await _context.Carros.Include(c => c.Categoria).FirstOrDefaultAsync(c => c.Id == carroId);

            if (carro == null)
            {
                return NotFound();
            }

            var carrinhoDeCompras = HttpContext.Session.GetJson<Carrinho>("CarrinhoDeCompras") ?? new Carrinho();
            //carrinhoDeCompras.AddItem(carro, quantidade.GetValueOrDefault());
            var item = carrinhoDeCompras.items.First(i => i.CarroId == carroId);
            if (item == null)
            {
                return NotFound();
            }

            item.Quantidade += quantidade.GetValueOrDefault();
            if (item.Quantidade <= 0)
            {
                return await RemoverCarrinhoItem(carroId);
            }

            HttpContext.Session.SetJson("CarrinhoDeCompras", carrinhoDeCompras);
            return RedirectToAction(nameof(Carrinho));
        }

        public async Task<IActionResult> RemoverCarrinhoItem(int? carroId)
        {
            if (carroId == null || _context.Carros == null)
            {
                return NotFound();
            }

            var carro = await _context.Carros.Include("Categoria")
                .FirstOrDefaultAsync(m => m.Id == carroId);
            if (carro == null)
            {
                return NotFound();
            }

            var carrinhoDeCompras = HttpContext.Session.GetJson<Carrinho>("CarrinhoDeCompras") ?? new Carrinho();
            carrinhoDeCompras.RemoveItem(carro);

            HttpContext.Session.SetJson("CarrinhoDeCompras", carrinhoDeCompras);
            return RedirectToAction(nameof(Carrinho));
        }


        // POST: Carroes/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carros == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Carros'  is null.");
            }
            var carro = await _context.Carros.FindAsync(id);
            if (carro != null)
            {
                _context.Carros.Remove(carro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarroExists(int id)
        {
            return _context.Carros.Any(e => e.Id == id);
        }
    }
}

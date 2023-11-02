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

namespace PWEBTP.Controllers
{
    [Authorize(Roles = "Admin, Gestor, Funcionario")]
    public class FuncionariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FuncionariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Funcionarios
        public async Task<IActionResult> Index()
        {
            var funcionarios = _context.Funcionarios.Include(f => f.Gestor).Include(f => f.Gestor.Empresa);
            return View(await funcionarios.ToListAsync());
        }

        // GET: Funcionarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.Funcionarios == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Gestor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public IActionResult Create()
        {
            ViewData["GestorId"] = new SelectList(_context.Gestores.ToList(), "Id", "Name");
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GestorId")] Funcionario funcionario)
        {
            ViewData["GestorId"] = new SelectList(_context.Gestores.ToList(), "Id", "Name");

            var gestor = await _context.Gestores.FindAsync(funcionario.GestorId);

            ModelState.Remove(nameof(funcionario.Reservas));
            ModelState.Remove(nameof(funcionario.Gestor));
            ModelState.Remove(nameof(funcionario.ApplicationUser));

            if (ModelState.IsValid)
            {
                await CriaConta(funcionario);
                funcionario.Gestor = gestor;

                _context.Add(funcionario);

                gestor.Funcionarios.Add(funcionario);
                _context.Update(gestor);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GestorId"] = new SelectList(_context.Gestores.ToList(), "Id", "Name", funcionario.GestorId);
            return View(funcionario);
        }

        public async Task CriaConta(Funcionario funcionario)
        {
            var gestor = _context.Gestores.Find(funcionario.GestorId);
            var empresa = _context.Empresas.Find(gestor.EmpresaId);

            var FuncionarioConta = new ApplicationUser
            {
                UserName = funcionario.Name.ToLower() + "Employer" + "@" + empresa.Name.ToLower() + ".pt",
                Email = funcionario.Name.ToLower() + "Employer" + "@" + empresa.Name.ToLower() + ".pt",
                PrimeiroNome = empresa.Name,
                UltimoNome = funcionario.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await _userManager.FindByEmailAsync(FuncionarioConta.Email);
            if (user == null)
            {
                await _userManager.CreateAsync(FuncionarioConta, "Is3C..00");
                await _userManager.AddToRoleAsync(FuncionarioConta, Roles.Funcionario.ToString());
                funcionario.ApplicationUser = FuncionarioConta;
                await _context.SaveChangesAsync();
            }
        }

        // GET: Funcionarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Funcionarios == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }
            ViewData["GestorId"] = new SelectList(_context.Gestores, "Id", "Name", funcionario.GestorId);
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GestorId")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
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
            ViewData["GestorId"] = new SelectList(_context.Gestores, "Id", "Id", funcionario.GestorId);
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Funcionarios == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionarios
                .Include(f => f.Gestor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Funcionarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Funcionarios'  is null.");
            }
            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario != null)
            {
                await DeleteConta(funcionario);
                _context.Funcionarios.Remove(funcionario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task DeleteConta(Funcionario funcionario)
        {
            var empresa = _context.Empresas.Find(_context.Gestores.Find(funcionario.GestorId));

            var user = await _userManager.FindByEmailAsync(funcionario.Name.ToLower() + "Employer" + "@" + empresa.Name.ToLower() + ".pt");

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.DeleteAsync(user);
        }
        private bool FuncionarioExists(int id)
        {
          return _context.Funcionarios.Any(e => e.Id == id);
        }
    }
}

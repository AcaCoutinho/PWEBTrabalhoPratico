using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    [Authorize(Roles = "Admin")]
    public class GestorsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public GestorsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Gestors
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Gestores.Include(g => g.Empresa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gestors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gestores == null)
            {
                return NotFound();
            }

            var gestor = await _context.Gestores
                .Include(g => g.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gestor == null)
            {
                return NotFound();
            }

            return View(gestor);
        }

        // GET: Gestors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gestores == null)
            {
                return NotFound();
            }

            var gestor = await _context.Gestores.FindAsync(id);
            if (gestor == null)
            {
                return NotFound();
            }
            return View(gestor);
        }

        // POST: Gestors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,EmpresaId")] Gestor gestor)
        {
            if (id != gestor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gestor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GestorExists(gestor.Id))
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
            ViewData["EmpresaId"] = new SelectList(_context.Empresas, "Id", "Id", gestor.EmpresaId);
            return View(gestor);
        }

        // GET: Gestors/Delete/5

        public IActionResult Create()
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,EmpresaId")] Gestor gestor)
        {
            ViewData["EmpresaId"] = new SelectList(_context.Empresas.ToList(), "Id", "Name");
            
            ModelState.Remove(nameof(gestor.Funcionarios));
            ModelState.Remove(nameof(gestor.Empresa));
            ModelState.Remove(nameof(gestor.User));
            if (ModelState.IsValid)
            {
                await CriaConta(gestor);
               
                _context.Add(gestor);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gestor);
        }


        public async Task CriaConta(Gestor gestor)
        {
            var empresa = _context.Empresas.Find(gestor.EmpresaId);

            var GestorConta = new ApplicationUser
            {
                UserName = gestor.Name.ToLower() + "@" + empresa.Name.ToLower() + ".pt",
                Email = gestor.Name.ToLower() + "@" + empresa.Name.ToLower() + ".pt",
                PrimeiroNome = empresa.Name,
                UltimoNome = gestor.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var user = await _userManager.FindByEmailAsync(GestorConta.Email);
            if(user == null)
            {
                await _userManager.CreateAsync(GestorConta, "Is3C..00");
                await _userManager.AddToRoleAsync(GestorConta, Roles.Gestor.ToString());
                gestor.User = GestorConta;
                await _context.SaveChangesAsync();
            }
        }
    
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Gestores == null)
            {
                return NotFound();
            }

            var gestor = await _context.Gestores
                .Include(g => g.Empresa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gestor == null)
            {
                return NotFound();
            }

            return View(gestor);
        }

        // POST: Gestors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gestores == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Gestores'  is null.");
            }

            var gestor = await _context.Gestores.FindAsync(id);
            if (gestor != null)
            {
                await DeleteConta(gestor);
                _context.Gestores.Remove(gestor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task DeleteConta(Gestor gestor)
        {
            var empresa = _context.Empresas.Find(gestor.EmpresaId);

            var user = await _userManager.FindByEmailAsync(gestor.Name.ToLower() + "@" + empresa.Name.ToLower() + ".pt");

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.DeleteAsync(user);
        } 

        private bool GestorExists(int id)
        {
          return _context.Gestores.Any(e => e.Id == id);
        }
    }
}

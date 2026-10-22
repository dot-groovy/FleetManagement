using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoFrota.Models;
namespace GestaoFrota.Controllers
{
    [Authorize]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor - recebe acesso à base de dados
        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista todos os veículos com o motorista associado
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Veiculos.Include(v => v.Motorista);
            return View(await applicationDbContext.ToListAsync());
        }

        // Mostra os detalhes de um veículo
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var veiculo = await _context.Veiculos
                .Include(v => v.Motorista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }
            return View(veiculo);
        }

        // Mostra o formulário para criar um veículo
        public IActionResult Create()
        {
            var motoristas = _context.Motoristas.OrderBy(m => m.Nome).ToList();
            var items = new List<SelectListItem> { new SelectListItem("-- Selecione um Motorista --", "") };
            items.AddRange(motoristas.Select(m => new SelectListItem(m.Nome, m.Id.ToString())));
            ViewData["MotoristaId"] = items;
            if (motoristas.Count == 0)
                ViewData["MensagemMotorista"] = "Não há motoristas. Crie primeiro um Motorista.";
            return View();
        }

        // Guarda o novo veículo na base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Matricula,Marca,Modelo,MotoristaId")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var motoristasPost = _context.Motoristas.OrderBy(m => m.Nome).ToList();
            var itemsPost = new List<SelectListItem> { new SelectListItem("-- Selecione um Motorista --", "") };
            itemsPost.AddRange(motoristasPost.Select(m => new SelectListItem(m.Nome, m.Id.ToString())));
            ViewData["MotoristaId"] = itemsPost;
            return View(veiculo);
        }

        // Mostra o formulário para editar um veículo
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
            {
                return NotFound();
            }
            ViewData["MotoristaId"] = new SelectList(_context.Motoristas, "Id", "Nome", veiculo.MotoristaId);
            return View(veiculo);
        }

        // Guarda as alterações do veículo na base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Matricula,Marca,Modelo,MotoristaId")] Veiculo veiculo)
        {
            if (id != veiculo.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.Id))
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
            ViewData["MotoristaId"] = new SelectList(_context.Motoristas, "Id", "Nome", veiculo.MotoristaId);
            return View(veiculo);
        }

        // Mostra a página de confirmação para apagar 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var veiculo = await _context.Veiculos
                .Include(v => v.Motorista)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }
            return View(veiculo);
        }

        // Apaga o veículo da base de dados  - o trigger regista em LogVeiculos
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Verifica se o veículo existe na base de dados
        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}

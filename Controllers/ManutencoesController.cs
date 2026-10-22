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
    public class ManutencoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Construtor - recebe acesso à base de dados
        public ManutencoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista todas as manutenções com o veículo associado
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Manutencoes.Include(m => m.Veiculo);
            return View(await applicationDbContext.ToListAsync());
        }

        // Mostra os detalhes de uma manutenção
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manutencao = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manutencao == null)
            {
                return NotFound();
            }
            return View(manutencao);
        }

        // Mostra o formulário para criar uma manutenção
        public IActionResult Create()
        {
            var veiculos = _context.Veiculos.OrderBy(v => v.Matricula).ToList();
            var items = new List<SelectListItem> { new SelectListItem("-- Selecione um Veículo --", "") };
            items.AddRange(veiculos.Select(v => new SelectListItem(v.Matricula, v.Id.ToString())));
            ViewData["VeiculoId"] = items;
            if (veiculos.Count == 0)
                ViewData["MensagemVeiculo"] = "Crie primeiro um Veículo.";
            return View();
        }

        // Guarda a nova manutenção na base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Data,Tipo,Custo,VeiculoId")] Manutencao manutencao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manutencao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var veiculosPost = _context.Veiculos.OrderBy(v => v.Matricula).ToList();
            var itemsPost = new List<SelectListItem> { new SelectListItem("-- Selecione um Veículo --", "") };
            itemsPost.AddRange(veiculosPost.Select(v => new SelectListItem(v.Matricula, v.Id.ToString())));
            ViewData["VeiculoId"] = itemsPost;
            if (veiculosPost.Count == 0)
                ViewData["MensagemVeiculo"] = "Crie primeiro um Veículo.";
            return View(manutencao);
        }

        // Mostra o formulário para editar uma manutenção
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null)
            {
                return NotFound();
            }
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Matricula", manutencao.VeiculoId);
            return View(manutencao);
        }

        // Guarda as alterações da manutenção na base de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Data,Tipo,Custo,VeiculoId")] Manutencao manutencao)
        {
            if (id != manutencao.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manutencao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManutencaoExists(manutencao.Id))
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
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Matricula", manutencao.VeiculoId);
            return View(manutencao);
        }

        // Mostra a página de confirmação para apagar 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manutencao = await _context.Manutencoes
                .Include(m => m.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manutencao == null)
            {
                return NotFound();
            }
            return View(manutencao);
        }

        // Apaga a manutenção da base de dados 
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao != null)
            {
                _context.Manutencoes.Remove(manutencao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Lista manutenções de um veículo usando a Stored Procedure
        public async Task<IActionResult> PorVeiculo(int? id)
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
            var dados = await _context.Manutencoes
                .FromSqlRaw("EXEC sp_GetManutencoesPorVeiculo {0}", id)
                .ToListAsync();
            ViewData["Veiculo"] = veiculo;
            return View(dados);
        }

        // Verifica se a manutenção existe na base de dados
        private bool ManutencaoExists(int id)
        {
            return _context.Manutencoes.Any(e => e.Id == id);
        }
    }
}

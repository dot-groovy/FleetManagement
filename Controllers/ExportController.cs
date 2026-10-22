using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoFrota.Data;
using GestaoFrota.Models;
using GestaoFrota.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
namespace GestaoFrota.Controllers;
[Authorize]
public class ExportController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    // Construtor - recebe acesso à base de dados e ao serviço de email
    public ExportController(ApplicationDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // Mostra a página de exportação com as opções de PDF e email
    public IActionResult Index()
    {
        return View();
    }

    // Gera o PDF e envia por email para o endereço indicado
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnviarEmail(string tipo, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["MensagemErro"] = "Introduza um email válido.";
            return RedirectToAction(nameof(Index));
        }
        tipo = tipo?.ToLower() ?? "";
        byte[] pdfBytes;
        string fileName;
        switch (tipo)
        {
            case "motorista":
            case "motoristas":
                var motoristas = await _context.Motoristas.OrderBy(m => m.Nome).ToListAsync();
                pdfBytes = GenerateMotoristasPdf(motoristas);
                fileName = "Motoristas.pdf";
                break;
            case "veiculo":
            case "veiculos":
                var veiculos = await _context.Veiculos.Include(v => v.Motorista).OrderBy(v => v.Matricula).ToListAsync();
                pdfBytes = GenerateVeiculosPdf(veiculos);
                fileName = "Veiculos.pdf";
                break;
            case "manutencao":
            case "manutencoes":
                var manutencoes = await _context.Manutencoes.Include(m => m.Veiculo).OrderByDescending(m => m.Data).ToListAsync();
                pdfBytes = GenerateManutencoesPdf(manutencoes);
                fileName = "Manutencoes.pdf";
                break;
            default:
                return RedirectToAction(nameof(Index));
        }
        try
        {
            var subject = $"Exportação - {fileName}";
            var body = $"<p>Em anexo segue o documento <strong>{fileName}</strong>.</p>";
            await _emailService.SendEmailWithAttachmentAsync(email.Trim(), subject, body, pdfBytes, fileName);
            TempData["MensagemSucesso"] = $"Enviado para o email: {email.Trim()}";
        }
        catch (Exception)
        {
            TempData["MensagemErro"] = "Erro ao enviar email. Verifique se o smtp4dev está em execução.";
        }
        return RedirectToAction(nameof(Index));
    }

    // Gera o PDF e faz o download direto no browser
    public async Task<IActionResult> ExportPdf(string tipo)
    {
        tipo = tipo?.ToLower() ?? "";
        byte[] pdfBytes;
        string fileName;
        switch (tipo)
        {
            case "motorista":
            case "motoristas":
                var motoristas = await _context.Motoristas.OrderBy(m => m.Nome).ToListAsync();
                pdfBytes = GenerateMotoristasPdf(motoristas);
                fileName = "Motoristas.pdf";
                break;
            case "veiculo":
            case "veiculos":
                var veiculos = await _context.Veiculos.Include(v => v.Motorista).OrderBy(v => v.Matricula).ToListAsync();
                pdfBytes = GenerateVeiculosPdf(veiculos);
                fileName = "Veiculos.pdf";
                break;
            case "manutencao":
            case "manutencoes":
                var manutencoes = await _context.Manutencoes.Include(m => m.Veiculo).OrderByDescending(m => m.Data).ToListAsync();
                pdfBytes = GenerateManutencoesPdf(manutencoes);
                fileName = "Manutencoes.pdf";
                break;
            default:
                return RedirectToAction(nameof(Index));
        }
        return File(pdfBytes, "application/pdf", fileName);
    }

    // Cria o ficheiro PDF com a lista de motoristas
    private static byte[] GenerateMotoristasPdf(List<Motorista> motoristas)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Header().Text("Motoristas - Gestão de Frota").FontSize(20).Bold();
                page.Content().Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Id").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Nome").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("NIF").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Carta").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Telefone").Bold();
                        });
                        foreach (var m in motoristas)
                        {
                            table.Cell().Padding(5).Text(m.Id.ToString());
                            table.Cell().Padding(5).Text(m.Nome);
                            table.Cell().Padding(5).Text(m.NIF);
                            table.Cell().Padding(5).Text(m.Carta);
                            table.Cell().Padding(5).Text(m.Telefone);
                        }
                    });
                });
            });
        });
        return document.GeneratePdf();
    }

    // Cria o ficheiro PDF com a lista de veículos
    private static byte[] GenerateVeiculosPdf(List<Veiculo> veiculos)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Header().Text("Veículos - Gestão de Frota").FontSize(20).Bold();
                page.Content().Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Id").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Matrícula").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Marca").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Modelo").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Motorista").Bold();
                        });
                        foreach (var v in veiculos)
                        {
                            table.Cell().Padding(5).Text(v.Id.ToString());
                            table.Cell().Padding(5).Text(v.Matricula);
                            table.Cell().Padding(5).Text(v.Marca);
                            table.Cell().Padding(5).Text(v.Modelo);
                            table.Cell().Padding(5).Text(v.Motorista?.Nome ?? "-");
                        }
                    });
                });
            });
        });
        return document.GeneratePdf();
    }

    // Cria o ficheiro PDF com a lista de manutenções
    private static byte[] GenerateManutencoesPdf(List<Manutencao> manutencoes)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.Header().Text("Manutenções - Gestão de Frota").FontSize(20).Bold();
                page.Content().Column(col =>
                {
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Id").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Data").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tipo").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Custo").Bold();
                            header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Veículo").Bold();
                        });
                        foreach (var m in manutencoes)
                        {
                            table.Cell().Padding(5).Text(m.Id.ToString());
                            table.Cell().Padding(5).Text(m.Data.ToString("dd/MM/yyyy"));
                            table.Cell().Padding(5).Text(m.Tipo);
                            table.Cell().Padding(5).Text(m.Custo.ToString("C2"));
                            table.Cell().Padding(5).Text(m.Veiculo?.Matricula ?? "-");
                        }
                    });
                });
            });
        });
        return document.GeneratePdf();
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GestaoFrota.Models;
namespace GestaoFrota.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    // Construtor - recebe o serviço de logs
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Mostra a página inicial da aplicação
    public IActionResult Index()
    {
        return View();
    }

    // Mostra a página de privacidade
    public IActionResult Privacy()
    {
        return View();
    }

    // Mostra mensagem quando o utilizador não tem permissão
    public IActionResult AccessDenied()
    {
        return View();
    }

    // Mostra a página de erro com o id do pedido
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

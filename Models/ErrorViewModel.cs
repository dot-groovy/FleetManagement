namespace GestaoFrota.Models;

// Modelo usado na página de erro para mostrar o id do pedido
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    // Indica se deve mostrar o id do pedido na página de erro
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

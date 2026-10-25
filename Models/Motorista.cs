using System.ComponentModel.DataAnnotations;
namespace GestaoFrota.Models;

// Modelo que representa um motorista da frota
public class Motorista
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O Nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O NIF é obrigatório")]
    [StringLength(9)]
    public string NIF { get; set; } = null!;

    [Required(ErrorMessage = "A Carta de condução é obrigatória")]
    [StringLength(50)]
    public string Carta { get; set; } = null!;

    [Required(ErrorMessage = "O Telefone é obrigatório")]
    [StringLength(20)]
    public string Telefone { get; set; } = null!;

    // Lista de veículos associados a este motorista
    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}

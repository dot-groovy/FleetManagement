using System.ComponentModel.DataAnnotations;
namespace GestaoFrota.Models;

// Modelo que representa um veículo da frota
public class Veiculo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A Matrícula é obrigatória")]
    [StringLength(20)]
    public string Matricula { get; set; } = null!;

    [Required(ErrorMessage = "A Marca é obrigatória")]
    [StringLength(50)]
    public string Marca { get; set; } = null!;

    [Required(ErrorMessage = "O Modelo é obrigatório")]
    [StringLength(50)]
    public string Modelo { get; set; } = null!;

    [Required(ErrorMessage = "Selecione um Motorista")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um Motorista")]
    public int MotoristaId { get; set; }

    // Motorista responsável por este veículo
    public Motorista? Motorista { get; set; }

    // Lista de manutenções feitas neste veículo
    public ICollection<Manutencao> Manutencoes { get; set; } = new List<Manutencao>();
}

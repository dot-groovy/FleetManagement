using System.ComponentModel.DataAnnotations;
namespace GestaoFrota.Models;

// Modelo que representa uma manutenção de um veículo
public class Manutencao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A Data é obrigatória")]
    public DateTime Data { get; set; }

    [Required(ErrorMessage = "O Tipo é obrigatório")]
    [StringLength(100)]
    public string Tipo { get; set; } = null!;

    [Required(ErrorMessage = "O Custo é obrigatório")]
    [Range(0, 999999.99, ErrorMessage = "O Custo deve ser entre 0 e 999999,99")]
    public decimal Custo { get; set; }

    [Required(ErrorMessage = "Selecione um Veículo")]
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um Veículo")]
    public int VeiculoId { get; set; }

    // Veículo a que esta manutenção pertence
    public Veiculo? Veiculo { get; set; }
}

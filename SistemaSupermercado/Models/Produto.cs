namespace SistemaSupermercado.Models;

public class Produto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Categoria { get; set; } = "";
    public int QuantidadeEstoque { get; set; }
    public decimal Preco { get; set; }
    public DateTime DataCadastro { get; set; }
}

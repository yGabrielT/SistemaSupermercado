namespace SistemaSupermercado.Models;

public class ItemCarrinho
{
    public int ProdutoId { get; set; }
    public string Produto { get; set; } = "";
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Total => Quantidade * PrecoUnitario;
}

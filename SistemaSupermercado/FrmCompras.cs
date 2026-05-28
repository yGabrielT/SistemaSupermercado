using Microsoft.Data.SqlClient;
using SistemaSupermercado.Database;
using SistemaSupermercado.Models;
using System.Data;
using System.Text;

namespace SistemaSupermercado;

public class FrmCompras : Form
{
    ComboBox cmbProdutos = new ComboBox();
    NumericUpDown numQuantidade = new NumericUpDown();
    DataGridView gridCarrinho = new DataGridView();
    Label lblTotal = new Label();
    List<ItemCarrinho> carrinho = new List<ItemCarrinho>();

    public FrmCompras()
    {
        Text = "Nova Compra";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);

        Label titulo = new Label
        {
            Text = "Nova Compra",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Left = 20, Top = 15, AutoSize = true
        };
        Controls.Add(titulo);

        // Linha de seleção de produto
        Label lblProduto = new Label { Text = "Produto:", Left = 30, Top = 83, Width = 80 };
        cmbProdutos.SetBounds(110, 80, 380, 26);
        cmbProdutos.DropDownStyle = ComboBoxStyle.DropDownList;

        Label lblQtd = new Label { Text = "Qtd:", Left = 510, Top = 83, Width = 40 };
        numQuantidade.SetBounds(550, 80, 80, 26);
        numQuantidade.Minimum = 1;
        numQuantidade.Maximum = 10000;

        Button btnAdicionar = new Button { Text = "➕ Adicionar", Left = 650, Top = 75, Width = 130, Height = 36 };
        btnAdicionar.Click += (s, e) => AdicionarProduto();

        // Grid carrinho
        gridCarrinho.SetBounds(30, 130, 820, 300);
        gridCarrinho.ReadOnly = true;
        gridCarrinho.AllowUserToAddRows = false;
        gridCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridCarrinho.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridCarrinho.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

        // Total
        lblTotal.Text = "Total: R$ 0,00";
        lblTotal.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTotal.Left = 30;
        lblTotal.Top = 448;
        lblTotal.Width = 300;
        lblTotal.AutoSize = true;

        // Botões de ação
        Button btnRemover   = new Button { Text = "🗑️ Remover Item",    Left = 320, Top = 445, Width = 150, Height = 40 };
        Button btnFinalizar = new Button { Text = "✅ Finalizar Compra", Left = 485, Top = 445, Width = 170, Height = 40 };
        Button btnLimpar    = new Button { Text = "🧹 Limpar Carrinho",  Left = 670, Top = 445, Width = 160, Height = 40 };

        btnRemover.Click   += (s, e) => RemoverItem();
        btnFinalizar.Click += (s, e) => FinalizarCompra();
        btnLimpar.Click    += (s, e) => { carrinho.Clear(); AtualizarCarrinho(); };

        Controls.AddRange(new Control[]
        {
            lblProduto, cmbProdutos, lblQtd, numQuantidade, btnAdicionar,
            gridCarrinho, lblTotal, btnRemover, btnFinalizar, btnLimpar
        });

        CarregarProdutos();
        AtualizarCarrinho();
    }

    void CarregarProdutos()
    {
        DataTable tabela = Banco.Consultar("SELECT Id, Nome, Preco, QuantidadeEstoque FROM Produtos WHERE QuantidadeEstoque > 0 ORDER BY Nome");
        cmbProdutos.DataSource = tabela;
        cmbProdutos.DisplayMember = "Nome";
        cmbProdutos.ValueMember = "Id";
    }

    void AdicionarProduto()
    {
        if (cmbProdutos.SelectedValue == null || cmbProdutos.DataSource == null)
        {
            MessageBox.Show("Cadastre um produto primeiro.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DataRowView linha = (DataRowView)cmbProdutos.SelectedItem!;
        int produtoId = Convert.ToInt32(linha["Id"]);
        string nome = linha["Nome"].ToString()!;
        decimal preco = Convert.ToDecimal(linha["Preco"]);
        int estoque = Convert.ToInt32(linha["QuantidadeEstoque"]);
        int qtd = (int)numQuantidade.Value;

        int qtdNoCarrinho = carrinho.Where(x => x.ProdutoId == produtoId).Sum(x => x.Quantidade);
        if (qtd + qtdNoCarrinho > estoque)
        {
            MessageBox.Show($"Quantidade solicitada ({qtd + qtdNoCarrinho}) maior que o estoque disponível ({estoque}).",
                "Estoque insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ItemCarrinho? existente = carrinho.FirstOrDefault(x => x.ProdutoId == produtoId);
        if (existente != null)
            existente.Quantidade += qtd;
        else
            carrinho.Add(new ItemCarrinho { ProdutoId = produtoId, Produto = nome, Quantidade = qtd, PrecoUnitario = preco });

        AtualizarCarrinho();
    }

    void AtualizarCarrinho()
    {
        gridCarrinho.DataSource = null;
        gridCarrinho.DataSource = carrinho.Select(x => new
        {
            x.ProdutoId,
            Produto = x.Produto,
            Quantidade = x.Quantidade,
            PrecoUnitario = x.PrecoUnitario.ToString("C2"),
            Total = x.Total.ToString("C2")
        }).ToList();

        if (gridCarrinho.Columns.Contains("ProdutoId"))
            gridCarrinho.Columns["ProdutoId"].Visible = false;

        decimal total = carrinho.Sum(x => x.Total);
        lblTotal.Text = "Total: " + total.ToString("C2");
    }

    void RemoverItem()
    {
        if (gridCarrinho.CurrentRow == null || !gridCarrinho.Columns.Contains("ProdutoId")) return;
        int produtoId = Convert.ToInt32(gridCarrinho.CurrentRow.Cells["ProdutoId"].Value);
        ItemCarrinho? item = carrinho.FirstOrDefault(x => x.ProdutoId == produtoId);
        if (item != null) carrinho.Remove(item);
        AtualizarCarrinho();
    }

    void FinalizarCompra()
    {
        if (carrinho.Count == 0)
        {
            MessageBox.Show("Adicione produtos ao carrinho.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // ✅ Copia o carrinho ANTES de limpar para gerar a nota corretamente
        List<ItemCarrinho> itensDaNota = carrinho.Select(x => new ItemCarrinho
        {
            ProdutoId = x.ProdutoId,
            Produto = x.Produto,
            Quantidade = x.Quantidade,
            PrecoUnitario = x.PrecoUnitario
        }).ToList();

        int vendaId;
        decimal total = carrinho.Sum(x => x.Total);

        using SqlConnection con = new SqlConnection(Banco.Conexao);
        con.Open();
        SqlTransaction transacao = con.BeginTransaction();

        try
        {
            string sqlVenda = "INSERT INTO Vendas (DataVenda, Total) OUTPUT INSERTED.Id VALUES (@DataVenda, @Total)";
            using SqlCommand cmdVenda = new SqlCommand(sqlVenda, con, transacao);
            cmdVenda.Parameters.AddWithValue("@DataVenda", DateTime.Now);
            cmdVenda.Parameters.AddWithValue("@Total", total);
            vendaId = Convert.ToInt32(cmdVenda.ExecuteScalar());

            foreach (ItemCarrinho item in carrinho)
            {
                string sqlItem = "INSERT INTO ItensVenda (VendaId, ProdutoId, Quantidade, PrecoUnitario, TotalItem) VALUES (@VendaId, @ProdutoId, @Quantidade, @PrecoUnitario, @TotalItem)";
                using SqlCommand cmdItem = new SqlCommand(sqlItem, con, transacao);
                cmdItem.Parameters.AddWithValue("@VendaId", vendaId);
                cmdItem.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                cmdItem.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                cmdItem.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                cmdItem.Parameters.AddWithValue("@TotalItem", item.Total);
                cmdItem.ExecuteNonQuery();

                string sqlEstoque = "UPDATE Produtos SET QuantidadeEstoque = QuantidadeEstoque - @Quantidade WHERE Id=@ProdutoId";
                using SqlCommand cmdEstoque = new SqlCommand(sqlEstoque, con, transacao);
                cmdEstoque.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                cmdEstoque.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                cmdEstoque.ExecuteNonQuery();
            }

            transacao.Commit();
        }
        catch (Exception ex)
        {
            transacao.Rollback();
            MessageBox.Show("Erro ao finalizar compra: " + ex.Message, "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Limpa o carrinho e atualiza a tela
        carrinho.Clear();
        AtualizarCarrinho();
        CarregarProdutos();

        MessageBox.Show("Compra finalizada com sucesso! Nº " + vendaId.ToString("000000"),
            "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // ✅ Gera a nota com os itens copiados antes de limpar
        string nota = GerarNota(vendaId, itensDaNota);
        new FrmNota(nota).ShowDialog();
    }

    string GerarNota(int vendaId, List<ItemCarrinho> itens)
    {
        StringBuilder nota = new StringBuilder();
        nota.AppendLine("========================================");
        nota.AppendLine("         SUPERMERCADO EXEMPLO           ");
        nota.AppendLine("      Rua das Flores, 123 - Centro      ");
        nota.AppendLine("    CNPJ: 12.345.678/0001-90            ");
        nota.AppendLine("========================================");
        nota.AppendLine("             NOTA DE COMPRA             ");
        nota.AppendLine("Nº da Nota: " + vendaId.ToString("000000"));
        nota.AppendLine("Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        nota.AppendLine("========================================");

        foreach (ItemCarrinho item in itens)
        {
            nota.AppendLine(item.Produto);
            nota.AppendLine($"  Qtd: {item.Quantidade}  x  R$ {item.PrecoUnitario:0.00}  =  R$ {item.Total:0.00}");
            nota.AppendLine("----------------------------------------");
        }

        nota.AppendLine();
        nota.AppendLine($"TOTAL DA COMPRA: R$ {itens.Sum(x => x.Total):0.00}");
        nota.AppendLine("========================================");
        nota.AppendLine("        Obrigado e volte sempre!        ");
        nota.AppendLine("========================================");
        return nota.ToString();
    }
}

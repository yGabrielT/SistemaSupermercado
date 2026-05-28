using Microsoft.Data.SqlClient;
using SistemaSupermercado.Database;
using System.Data;

namespace SistemaSupermercado;

public class FrmProdutos : Form
{
    TextBox txtId = new TextBox();
    TextBox txtCodigo = new TextBox();
    TextBox txtNome = new TextBox();
    ComboBox cmbCategoria = new ComboBox();
    NumericUpDown numEstoque = new NumericUpDown();
    TextBox txtPreco = new TextBox();
    TextBox txtPesquisar = new TextBox();
    DataGridView grid = new DataGridView();

    public FrmProdutos()
    {
        Text = "Cadastro de Produtos";
        Width = 920;
        Height = 660;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(920, 660);

        Label titulo = new Label
        {
            Text = "Cadastro de Produtos",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Left = 20, Top = 15, AutoSize = true
        };
        Controls.Add(titulo);

        txtId.Visible = false;

        // Formulário
        CriarLabel("Código:", 30, 70);      txtCodigo.SetBounds(160, 68, 180, 26);  Controls.Add(txtCodigo);
        CriarLabel("Nome:", 30, 108);       txtNome.SetBounds(160, 106, 380, 26);   Controls.Add(txtNome);
        CriarLabel("Categoria:", 30, 146);  cmbCategoria.SetBounds(160, 144, 200, 26); Controls.Add(cmbCategoria);
        CriarLabel("Estoque:", 30, 184);    numEstoque.SetBounds(160, 182, 100, 26); numEstoque.Maximum = 100000; Controls.Add(numEstoque);
        CriarLabel("Preço (R$):", 30, 222); txtPreco.SetBounds(160, 220, 130, 26);  Controls.Add(txtPreco);

        cmbCategoria.Items.AddRange(new string[] { "Alimentos", "Bebidas", "Limpeza", "Higiene", "Outros" });
        cmbCategoria.SelectedIndex = 0;
        cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;

        // Botões CRUD
        Button btnCadastrar = new Button { Text = "➕ Cadastrar", Left = 30,  Top = 270, Width = 130, Height = 40 };
        Button btnAtualizar = new Button { Text = "✏️ Atualizar",  Left = 175, Top = 270, Width = 130, Height = 40 };
        Button btnExcluir   = new Button { Text = "🗑️ Excluir",   Left = 320, Top = 270, Width = 130, Height = 40 };
        Button btnLimpar    = new Button { Text = "🧹 Limpar",    Left = 465, Top = 270, Width = 130, Height = 40 };

        btnCadastrar.Click += (s, e) => Cadastrar();
        btnAtualizar.Click += (s, e) => Atualizar();
        btnExcluir.Click   += (s, e) => Excluir();
        btnLimpar.Click    += (s, e) => Limpar();

        Controls.AddRange(new Control[] { btnCadastrar, btnAtualizar, btnExcluir, btnLimpar });

        // Pesquisa
        Label lblPesq = new Label { Text = "🔍 Pesquisar:", Left = 30, Top = 330, Width = 100, AutoSize = true };
        txtPesquisar.SetBounds(135, 327, 400, 26);
        txtPesquisar.PlaceholderText = "Nome ou código do produto...";
        txtPesquisar.TextChanged += (s, e) => Listar();
        Controls.Add(lblPesq);
        Controls.Add(txtPesquisar);

        // Grid
        grid.SetBounds(30, 365, 840, 220);
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        grid.CellClick += Grid_CellClick;
        Controls.Add(grid);

        Listar();
    }

    void CriarLabel(string texto, int x, int y)
    {
        Controls.Add(new Label { Text = texto, Left = x, Top = y + 3, Width = 120 });
    }

    bool Validar()
    {
        if (txtCodigo.Text.Trim() == "" || txtNome.Text.Trim() == "" || txtPreco.Text.Trim() == "")
        {
            MessageBox.Show("Preencha todos os campos obrigatórios.", "Atenção",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        try { Banco.ParseDecimal(txtPreco.Text); }
        catch
        {
            MessageBox.Show("Preço inválido. Use apenas números, ex: 12.50 ou 12,50",
                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    void Cadastrar()
    {
        if (!Validar()) return;
        using SqlConnection con = new SqlConnection(Banco.Conexao);
        con.Open();
        string sql = "INSERT INTO Produtos (Codigo, Nome, Categoria, QuantidadeEstoque, Preco, DataCadastro) VALUES (@Codigo, @Nome, @Categoria, @Estoque, @Preco, @Data)";
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
        cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
        cmd.Parameters.AddWithValue("@Categoria", cmbCategoria.Text);
        cmd.Parameters.AddWithValue("@Estoque", (int)numEstoque.Value);
        cmd.Parameters.AddWithValue("@Preco", Banco.ParseDecimal(txtPreco.Text));
        cmd.Parameters.AddWithValue("@Data", DateTime.Now);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Limpar(); Listar();
    }

    void Atualizar()
    {
        if (txtId.Text == "") { MessageBox.Show("Selecione um produto na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (!Validar()) return;
        using SqlConnection con = new SqlConnection(Banco.Conexao);
        con.Open();
        string sql = "UPDATE Produtos SET Codigo=@Codigo, Nome=@Nome, Categoria=@Categoria, QuantidadeEstoque=@Estoque, Preco=@Preco WHERE Id=@Id";
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
        cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text.Trim());
        cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
        cmd.Parameters.AddWithValue("@Categoria", cmbCategoria.Text);
        cmd.Parameters.AddWithValue("@Estoque", (int)numEstoque.Value);
        cmd.Parameters.AddWithValue("@Preco", Banco.ParseDecimal(txtPreco.Text));
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Limpar(); Listar();
    }

    void Excluir()
    {
        if (txtId.Text == "") { MessageBox.Show("Selecione um produto na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (MessageBox.Show("Deseja excluir este produto?", "Confirmar exclusão",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

        using SqlConnection con = new SqlConnection(Banco.Conexao);
        con.Open();
        // Verifica se há vendas vinculadas
        using SqlCommand chk = new SqlCommand("SELECT COUNT(*) FROM ItensVenda WHERE ProdutoId=@Id", con);
        chk.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
        int vinculados = (int)chk.ExecuteScalar();
        if (vinculados > 0)
        {
            MessageBox.Show("Este produto possui vendas vinculadas e não pode ser excluído.", "Não permitido",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using SqlCommand cmd = new SqlCommand("DELETE FROM Produtos WHERE Id=@Id", con);
        cmd.Parameters.AddWithValue("@Id", int.Parse(txtId.Text));
        cmd.ExecuteNonQuery();
        MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Limpar(); Listar();
    }

    void Listar()
    {
        string sql = "SELECT Id, Codigo, Nome, Categoria, QuantidadeEstoque AS Estoque, Preco, DataCadastro FROM Produtos WHERE Nome LIKE @Filtro OR Codigo LIKE @Filtro ORDER BY Nome";
        var parametros = new Microsoft.Data.SqlClient.SqlParameter[]
        {
            new Microsoft.Data.SqlClient.SqlParameter("@Filtro", $"%{txtPesquisar.Text.Trim()}%")
        };
        grid.DataSource = Banco.Consultar(sql, parametros);
        if (grid.Columns.Contains("Id")) grid.Columns["Id"].Visible = false;
        if (grid.Columns.Contains("DataCadastro"))
        {
            grid.Columns["DataCadastro"].DefaultCellStyle.Format = "dd/MM/yyyy";
            grid.Columns["DataCadastro"].HeaderText = "Cadastro";
        }
        if (grid.Columns.Contains("Preco"))
        {
            grid.Columns["Preco"].DefaultCellStyle.Format = "C2";
            grid.Columns["Preco"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        if (grid.Columns.Contains("Estoque"))
            grid.Columns["Estoque"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        DataGridViewRow linha = grid.Rows[e.RowIndex];
        txtId.Text = linha.Cells["Id"].Value.ToString();
        txtCodigo.Text = linha.Cells["Codigo"].Value.ToString();
        txtNome.Text = linha.Cells["Nome"].Value.ToString();
        cmbCategoria.Text = linha.Cells["Categoria"].Value.ToString();
        numEstoque.Value = Convert.ToDecimal(linha.Cells["Estoque"].Value);
        txtPreco.Text = Convert.ToDecimal(linha.Cells["Preco"].Value).ToString("0.00");
    }

    void Limpar()
    {
        txtId.Clear(); txtCodigo.Clear(); txtNome.Clear(); txtPreco.Clear();
        numEstoque.Value = 0; cmbCategoria.SelectedIndex = 0;
        grid.ClearSelection();
    }
}

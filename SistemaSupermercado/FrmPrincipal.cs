namespace SistemaSupermercado;

public class FrmPrincipal : Form
{
    public FrmPrincipal()
    {
        Text = "Supermercado - Sistema de Gestão";
        Width = 440;
        Height = 340;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.WhiteSmoke;

        Label titulo = new Label
        {
            Text = "🛒 Sistema de Supermercado",
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 100, 180),
            AutoSize = true,
            Left = 30,
            Top = 28
        };

        Label subtitulo = new Label
        {
            Text = "Selecione uma opção abaixo",
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Gray,
            AutoSize = true,
            Left = 30,
            Top = 65
        };

        Button btnProdutos = new Button
        {
            Text = "📦  Cadastro de Produtos",
            Left = 80, Top = 100, Width = 260, Height = 50,
            Font = new Font("Segoe UI", 11),
            FlatStyle = FlatStyle.Flat
        };
        Button btnCompras = new Button
        {
            Text = "🛍️  Nova Compra",
            Left = 80, Top = 165, Width = 260, Height = 50,
            Font = new Font("Segoe UI", 11),
            FlatStyle = FlatStyle.Flat
        };
        Button btnSair = new Button
        {
            Text = "✖️  Sair",
            Left = 80, Top = 235, Width = 260, Height = 38,
            Font = new Font("Segoe UI", 10),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.DarkRed
        };

        btnProdutos.Click += (s, e) => new FrmProdutos().ShowDialog();
        btnCompras.Click  += (s, e) => new FrmCompras().ShowDialog();
        btnSair.Click     += (s, e) => Close();

        Controls.AddRange(new Control[] { titulo, subtitulo, btnProdutos, btnCompras, btnSair });
    }
}

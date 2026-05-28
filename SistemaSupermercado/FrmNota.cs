namespace SistemaSupermercado;

public class FrmNota : Form
{
    TextBox txtNota = new TextBox();

    public FrmNota(string nota)
    {
        Text = "Nota de Compra";
        Width = 520;
        Height = 650;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        Label titulo = new Label
        {
            Text = "🧾 Nota de Compra",
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            Left = 20, Top = 12, AutoSize = true
        };
        Controls.Add(titulo);

        txtNota.Multiline = true;
        txtNota.ScrollBars = ScrollBars.Vertical;
        txtNota.Font = new Font("Consolas", 10);
        txtNota.SetBounds(20, 45, 460, 500);
        txtNota.Text = nota;
        txtNota.ReadOnly = true;          // ✅ impede edição acidental
        txtNota.BackColor = Color.White;  // mantém visual limpo mesmo com ReadOnly
        Controls.Add(txtNota);

        Button btnSalvar  = new Button { Text = "💾 Salvar TXT", Left = 20,  Top = 560, Width = 140, Height = 40 };
        Button btnImprimir = new Button { Text = "🖨️ Imprimir",  Left = 175, Top = 560, Width = 140, Height = 40 };
        Button btnFechar  = new Button { Text = "✖️ Fechar",     Left = 335, Top = 560, Width = 140, Height = 40 };

        btnSalvar.Click   += (s, e) => SalvarNota();
        btnImprimir.Click += (s, e) => ImprimirNota();
        btnFechar.Click   += (s, e) => Close();

        Controls.AddRange(new Control[] { btnSalvar, btnImprimir, btnFechar });
    }

    void SalvarNota()
    {
        SaveFileDialog salvar = new SaveFileDialog
        {
            Filter = "Arquivo de Texto|*.txt",
            FileName = $"nota_compra_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
        };

        if (salvar.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(salvar.FileName, txtNota.Text, System.Text.Encoding.UTF8);
            MessageBox.Show("Nota salva com sucesso!\n" + salvar.FileName, "Salvo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    void ImprimirNota()
    {
        using PrintDialog pd = new PrintDialog();
        using System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
        string conteudo = txtNota.Text;

        doc.PrintPage += (sender, e) =>
        {
            if (e.Graphics == null) return;
            Font fonte = new Font("Consolas", 9);
            float y = e.MarginBounds.Top;
            foreach (string linha in conteudo.Split('\n'))
            {
                e.Graphics.DrawString(linha.TrimEnd('\r'), fonte, Brushes.Black, e.MarginBounds.Left, y);
                y += fonte.GetHeight(e.Graphics);
                if (y > e.MarginBounds.Bottom) break;
            }
        };

        pd.Document = doc;
        if (pd.ShowDialog() == DialogResult.OK)
            doc.Print();
    }
}

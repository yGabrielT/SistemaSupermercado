using SistemaSupermercado.Database;

namespace SistemaSupermercado;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        try
        {
            Banco.InicializarBanco();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Erro ao iniciar o banco de dados:\n" + ex.Message,
                "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        Application.Run(new FrmPrincipal());
    }
}

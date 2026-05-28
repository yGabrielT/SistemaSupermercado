using Microsoft.Data.SqlClient;
using System.Data;

namespace SistemaSupermercado.Database;

public static class Banco
{
    private const string ConexaoMaster = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
    public const string Conexao = @"Server=(localdb)\MSSQLLocalDB;Database=SupermercadoDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void InicializarBanco()
    {
        using (SqlConnection con = new SqlConnection(ConexaoMaster))
        {
            con.Open();
            new SqlCommand("IF DB_ID('SupermercadoDB') IS NULL CREATE DATABASE SupermercadoDB", con).ExecuteNonQuery();
        }

        using (SqlConnection con = new SqlConnection(Conexao))
        {
            con.Open();
            string sql = @"
IF OBJECT_ID('Produtos', 'U') IS NULL
CREATE TABLE Produtos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Categoria VARCHAR(50) NOT NULL,
    QuantidadeEstoque INT NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    DataCadastro DATETIME NOT NULL
);

IF OBJECT_ID('Vendas', 'U') IS NULL
CREATE TABLE Vendas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DataVenda DATETIME NOT NULL,
    Total DECIMAL(10,2) NOT NULL
);

IF OBJECT_ID('ItensVenda', 'U') IS NULL
CREATE TABLE ItensVenda (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VendaId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    TotalItem DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VendaId) REFERENCES Vendas(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);";
            new SqlCommand(sql, con).ExecuteNonQuery();
        }
    }

    // Consulta simples (sem parâmetros)
    public static DataTable Consultar(string sql)
    {
        using SqlConnection con = new SqlConnection(Conexao);
        using SqlDataAdapter da = new SqlDataAdapter(sql, con);
        DataTable tabela = new DataTable();
        da.Fill(tabela);
        return tabela;
    }

    // Consulta com parâmetros (evita SQL injection)
    public static DataTable Consultar(string sql, SqlParameter[] parametros)
    {
        using SqlConnection con = new SqlConnection(Conexao);
        using SqlCommand cmd = new SqlCommand(sql, con);
        cmd.Parameters.AddRange(parametros);
        using SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable tabela = new DataTable();
        da.Fill(tabela);
        return tabela;
    }

    // Converte string de preço para decimal independente do locale
    public static decimal ParseDecimal(string valor)
    {
        valor = valor.Trim().Replace(" ", "");
        // Se tem vírgula e ponto, remove o separador de milhar e normaliza decimal
        if (valor.Contains(',') && valor.Contains('.'))
        {
            // Verifica qual vem por último (é o decimal)
            if (valor.LastIndexOf(',') > valor.LastIndexOf('.'))
                valor = valor.Replace(".", "").Replace(",", ".");
            else
                valor = valor.Replace(",", "");
        }
        else if (valor.Contains(','))
        {
            valor = valor.Replace(",", ".");
        }
        return decimal.Parse(valor, System.Globalization.CultureInfo.InvariantCulture);
    }
}

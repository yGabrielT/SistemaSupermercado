# Sistema de Supermercado

## Integrantes
Luis Felipe Pereira da Silva - 923106793
Otavio Almeida de Souza - 923108724
João Victor Caires Xavier - 923103128
Carlos Eduardo Pietoso Ribeiro Silva - 923107805
Gustavo Dias de Assis Silva - 923101111
Gabriel Santos de Moraes - 922208819
Mattheus Nascimento Araujo - 922207616
Victor Gustavo Silva Cabral - 923115594
Gabriel Trindade Dias – 924118662
João Victor patrício do nascimento - 3024107981

## Descrição
Sistema desktop desenvolvido em C# com Windows Forms para gerenciamento de supermercado.

O sistema permite:
- Cadastrar produtos;
- Atualizar produtos;
- Excluir produtos;
- Listar produtos;
- Realizar compras/vendas;
- Adicionar múltiplos produtos no carrinho;
- Calcular o total da compra;
- Emitir uma nota simples da venda.

## Tecnologias utilizadas
- C#
- Windows Forms
- Visual Studio
- SQL Server LocalDB
- Microsoft.Data.SqlClient

## Banco de dados
O sistema utiliza SQL Server LocalDB.

Ao iniciar o sistema, ele tenta criar automaticamente o banco `SupermercadoDB` e as tabelas:
- Produtos
- Vendas
- ItensVenda

Também existe um script SQL na pasta `SQL/criar_banco.sql`.

## Como executar
1. Abra o Visual Studio.
2. Clique em `Open a project or solution`.
3. Selecione o arquivo `SistemaSupermercado.sln`.
4. Aguarde o Visual Studio restaurar os pacotes NuGet.
5. Execute o projeto clicando em `Start`.

## Observação
Caso o SQL Server LocalDB não esteja instalado, instale pelo Visual Studio Installer ou altere a string de conexão no arquivo:

`SistemaSupermercado/Database/Banco.cs`
```
 private const string ConexaoMaster = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
 public const string Conexao = @"Server=(localdb)\MSSQLLocalDB;Database=SupermercadoDB;Trusted_Connection=True;TrustServerCertificate=True;";
```
Caso esteja usando o localdb so troque o `(localdb)` pelo o nome do seu sistema
Outras alternativas:
- SQLDeveloper so seria necessário o nome do sistema e remove o `\MSSQLLocalDB`
- SQLExpress so seria necessário o nome do sistema e troca o `\MSSQLLocalDB` por `\SQLEXPRESS`

## Usuário e senha
Este sistema não possui tela de login.

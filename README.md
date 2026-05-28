# Sistema de Supermercado

## Integrantes
- Coloque aqui o nome dos integrantes do grupo.

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

## Usuário e senha
Este sistema não possui tela de login.

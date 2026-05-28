CREATE DATABASE SupermercadoDB;
GO

USE SupermercadoDB;
GO

CREATE TABLE Produtos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Categoria VARCHAR(50) NOT NULL,
    QuantidadeEstoque INT NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    DataCadastro DATETIME NOT NULL
);

CREATE TABLE Vendas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DataVenda DATETIME NOT NULL,
    Total DECIMAL(10,2) NOT NULL
);

CREATE TABLE ItensVenda (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VendaId INT NOT NULL,
    ProdutoId INT NOT NULL,
    Quantidade INT NOT NULL,
    PrecoUnitario DECIMAL(10,2) NOT NULL,
    TotalItem DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VendaId) REFERENCES Vendas(Id),
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id)
);

INSERT INTO Produtos (Codigo, Nome, Categoria, QuantidadeEstoque, Preco, DataCadastro) VALUES
('000123', 'Arroz Branco 5kg', 'Alimentos', 50, 23.90, GETDATE()),
('000124', 'Feijao Carioca 1kg', 'Alimentos', 80, 7.50, GETDATE()),
('000125', 'Acucar Cristal 1kg', 'Alimentos', 100, 4.30, GETDATE()),
('000126', 'Oleo de Soja 900ml', 'Alimentos', 60, 6.89, GETDATE()),
('000127', 'Cafe Torrado 500g', 'Bebidas', 40, 15.90, GETDATE());

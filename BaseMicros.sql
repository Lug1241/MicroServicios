CREATE DATABASE ClientesDB;
GO

USE ClientesDB;
GO

-- Tabla Persona
CREATE TABLE Persona (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Genero VARCHAR(20) NOT NULL,
    Edad INT NOT NULL,
    Identificacion VARCHAR(20) UNIQUE NOT NULL,
    Direccion VARCHAR(200) NOT NULL,
    Telefono VARCHAR(20) NOT NULL
);
GO

-- Tabla Cliente
CREATE TABLE Cliente (
    Id INT PRIMARY KEY,
    Contraseña VARCHAR(100) NOT NULL,
    Estado BIT NOT NULL,

    CONSTRAINT FK_Cliente_Persona
        FOREIGN KEY (Id)
        REFERENCES Persona(Id)
);
GO


-- Base de datos Cuentas
CREATE DATABASE CuentasDB;
GO

USE CuentasDB;
GO

-- Tabla ClienteReferencia
CREATE TABLE ClienteReferencia (
    ClienteId INT PRIMARY KEY,
    Estado BIT NOT NULL,
    Nombre VARCHAR(100) NOT NULL
);
GO

-- Tabla Cuenta
CREATE TABLE Cuenta (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NumeroCuenta INT UNIQUE NOT NULL,
    TipoCuenta VARCHAR(20) NOT NULL,
    SaldoInicial DECIMAL(18,4) NOT NULL,
    Estado BIT NOT NULL,
    ClienteId INT NOT NULL,

    CONSTRAINT FK_Cliente_Cuenta
        FOREIGN KEY (ClienteId)
        REFERENCES ClienteReferencia(ClienteId)
);
GO

-- Tabla Movimiento
CREATE TABLE Movimiento (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME2 NOT NULL DEFAULT GETDATE(),
    TipoMovimiento VARCHAR(20) NOT NULL,
    Valor DECIMAL(18,4) NOT NULL,
    Saldo DECIMAL(18,4) NOT NULL,
    CuentaId INT NOT NULL,

    CONSTRAINT FK_Movimiento_Cuenta
        FOREIGN KEY (CuentaId)
        REFERENCES Cuenta(Id)
);
GO
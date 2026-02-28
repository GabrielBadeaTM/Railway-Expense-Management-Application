USE [Depou Feroviar]
GO

CREATE TABLE [dbo].[Judete](
    [idJudet] INT IDENTITY(1,1) PRIMARY KEY,
    [numeJudet] NVARCHAR(50) NOT NULL
);

CREATE TABLE [dbo].[Orase](
    [idOras] INT IDENTITY(1,1) PRIMARY KEY,
    [idJudet] INT NOT NULL,
    [numeOras] NVARCHAR(50) NOT NULL
);

CREATE TABLE [dbo].[Adrese](
    [idAdresa] INT IDENTITY(1,1) PRIMARY KEY,
    [idOras] INT NOT NULL,
    [strada] NVARCHAR(50) NOT NULL,
    [numar] NVARCHAR(10) NOT NULL,
    [codPostal] NVARCHAR(6) NULL
);

CREATE TABLE [dbo].[PersoaneContact](
    [idPersoanaContact] INT IDENTITY(1,1) PRIMARY KEY,
    [nume] NVARCHAR(50) NOT NULL,
    [prenume] NVARCHAR(50) NOT NULL,
    [telefon] NVARCHAR(20) NOT NULL,
    [email] NVARCHAR(50) NOT NULL
);

CREATE TABLE [dbo].[Firme](
    [idFirma] INT IDENTITY(1,1) PRIMARY KEY,
    [idPersoanaContact] INT NOT NULL,
    [idAdresa] INT NOT NULL,
    [numeFirma] VARCHAR(50) NOT NULL,
    [codFiscal] NVARCHAR(20) NOT NULL,
    [registruComercial] NVARCHAR(50) NOT NULL,
    [tipRelatie] CHAR(1) NOT NULL
);

CREATE TABLE [dbo].[CentreCost](
    [idCentruCost] INT IDENTITY(1,1) PRIMARY KEY,
    [denumireCentruCost] NVARCHAR(50) NOT NULL
);

CREATE TABLE [dbo].[Facturi](
    [idFactura] INT IDENTITY(1,1) PRIMARY KEY,
    [idCentruCost] INT NOT NULL,
    [idFurnizor] INT NOT NULL,
    [dataFactura] DATE NOT NULL,
    [numarFactura] NVARCHAR(50) NOT NULL,
    [sumaTotal] NUMERIC(18,2) NOT NULL
);

CREATE TABLE [dbo].[PuncteLucru](
    [idPunctLucru] INT IDENTITY(1,1) PRIMARY KEY,
    [idOras] INT NOT NULL
);

CREATE TABLE [dbo].[Locomotive](
    [idLocomotiva] INT IDENTITY(1,1) PRIMARY KEY,
    [idPunctLucru] INT NOT NULL,
    [numeLocomotiva] NVARCHAR(20) NOT NULL,
    [nrEuro] NVARCHAR(50) NULL,
    [tip] NVARCHAR(50) NOT NULL,
    [serie] NVARCHAR(50) NULL,
    [putere] DECIMAL(18,2) NOT NULL
);

CREATE TABLE [dbo].[Piese](
    [idPiesa] INT IDENTITY(1,1) PRIMARY KEY,
    [denumirePiesa] NVARCHAR(50) NOT NULL,
    [codPiesa] NVARCHAR(20) NULL,
    [unitateMasuraPiesa] NVARCHAR(50) NULL,
    [descrierePiesa] NVARCHAR(255) NULL
);

CREATE TABLE [dbo].[Vagoane](
    [idVagon] INT IDENTITY(1,1) PRIMARY KEY,
    [idProprietar] INT NOT NULL,
    [numarVagon] NVARCHAR(50) NOT NULL
);

CREATE TABLE [dbo].[Servicii](
    [idServiciu] INT IDENTITY(1,1) PRIMARY KEY,
    [denumireServiciu] NVARCHAR(50) NOT NULL,
    [descriereServiciu] NVARCHAR(255) NULL,
    [tipServiciu] NVARCHAR(50) NULL
);

CREATE TABLE [dbo].[PieseLocomotive](
    [idTranzactiePL] INT IDENTITY(1,1) PRIMARY KEY,
    [idPiesa] INT NOT NULL,
    [idLocomotiva] INT NULL,
    [idFactura] INT NOT NULL,
    [cantitate] DECIMAL(18,2) NOT NULL CHECK (cantitate > 0),
    [pretUnitar] DECIMAL(18,2) NOT NULL CHECK (pretUnitar > 0),
    CONSTRAINT UQ_PieseLocomotive UNIQUE(idPiesa,idLocomotiva,idFactura)
);

CREATE TABLE [dbo].[PieseVagoane](
    [idTranzactie] INT IDENTITY(1,1) PRIMARY KEY,
    [idPiesa] INT NOT NULL,
    [idVagon] INT NOT NULL,
    [idFactura] INT NOT NULL,
    [cantitate] DECIMAL(18,2) NOT NULL CHECK (cantitate > 0),
    [pretUnitar] DECIMAL(18,2) NOT NULL CHECK (pretUnitar > 0),
    CONSTRAINT UQ_PieseVagoane UNIQUE(idPiesa,idVagon,idFactura)
);

CREATE TABLE [dbo].[ServiciiPrestate](
    [idTranzactieSP] INT IDENTITY(1,1) PRIMARY KEY,
    [idServiciu] INT NOT NULL,
    [idFactura] INT NOT NULL,
    [idLocomotiva] INT NULL,
    [cost] DECIMAL(18,2) NOT NULL,
    [observatii] NVARCHAR(255) NULL
);

-- FOREIGN KEYS

ALTER TABLE Orase ADD FOREIGN KEY (idJudet) REFERENCES Judete(idJudet);
ALTER TABLE Adrese ADD FOREIGN KEY (idOras) REFERENCES Orase(idOras);
ALTER TABLE Firme ADD FOREIGN KEY (idPersoanaContact) REFERENCES PersoaneContact(idPersoanaContact);
ALTER TABLE Firme ADD FOREIGN KEY (idAdresa) REFERENCES Adrese(idAdresa);
ALTER TABLE Facturi ADD FOREIGN KEY (idCentruCost) REFERENCES CentreCost(idCentruCost);
ALTER TABLE Facturi ADD FOREIGN KEY (idFurnizor) REFERENCES Firme(idFirma);
ALTER TABLE PuncteLucru ADD FOREIGN KEY (idOras) REFERENCES Orase(idOras);
ALTER TABLE Locomotive ADD FOREIGN KEY (idPunctLucru) REFERENCES PuncteLucru(idPunctLucru);
ALTER TABLE Vagoane ADD FOREIGN KEY (idProprietar) REFERENCES Firme(idFirma);
ALTER TABLE PieseLocomotive ADD FOREIGN KEY (idPiesa) REFERENCES Piese(idPiesa);
ALTER TABLE PieseLocomotive ADD FOREIGN KEY (idLocomotiva) REFERENCES Locomotive(idLocomotiva);
ALTER TABLE PieseLocomotive ADD FOREIGN KEY (idFactura) REFERENCES Facturi(idFactura);
ALTER TABLE PieseVagoane ADD FOREIGN KEY (idPiesa) REFERENCES Piese(idPiesa);
ALTER TABLE PieseVagoane ADD FOREIGN KEY (idVagon) REFERENCES Vagoane(idVagon);
ALTER TABLE PieseVagoane ADD FOREIGN KEY (idFactura) REFERENCES Facturi(idFactura);
ALTER TABLE ServiciiPrestate ADD FOREIGN KEY (idServiciu) REFERENCES Servicii(idServiciu);
ALTER TABLE ServiciiPrestate ADD FOREIGN KEY (idFactura) REFERENCES Facturi(idFactura);
ALTER TABLE ServiciiPrestate ADD FOREIGN KEY (idLocomotiva) REFERENCES Locomotive(idLocomotiva);

INSERT INTO [dbo].[CentreCost] ([denumireCentruCost]) VALUES (N'Întreținere Locomotive');
INSERT INTO [dbo].[CentreCost] ([denumireCentruCost]) VALUES (N'Întreținere Vagoane');
INSERT INTO [dbo].[CentreCost] ([denumireCentruCost]) VALUES (N'Central');
INSERT INTO [dbo].[CentreCost] ([denumireCentruCost]) VALUES (N'Manevre');
GO
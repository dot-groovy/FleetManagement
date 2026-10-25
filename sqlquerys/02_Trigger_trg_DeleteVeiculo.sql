USE GestaoFrota;
GO

IF OBJECT_ID(N'dbo.LogVeiculos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LogVeiculos
    (
        Id        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        VeiculoId INT               NOT NULL,
        DataAcao  DATETIME          NOT NULL DEFAULT GETDATE()
    );
END
GO

IF OBJECT_ID(N'dbo.trg_DeleteVeiculo', N'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_DeleteVeiculo;
GO

CREATE TRIGGER dbo.trg_DeleteVeiculo
ON dbo.Veiculos
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON; --mensagem de contagens não sao enviadas

    INSERT INTO dbo.LogVeiculos (VeiculoId)
    SELECT d.Id
    FROM deleted AS d;
END;
GO

--ver registros mais recentes.
SELECT * FROM dbo.LogVeiculos
ORDER BY DataAcao DESC;


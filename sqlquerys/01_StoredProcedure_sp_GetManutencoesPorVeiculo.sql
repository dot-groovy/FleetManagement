USE GestaoFrota;
GO

IF OBJECT_ID(N'dbo.sp_GetManutencoesPorVeiculo', N'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetManutencoesPorVeiculo;
GO

CREATE PROCEDURE dbo.sp_GetManutencoesPorVeiculo
    @VeiculoId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Manutencoes
    WHERE VeiculoId = @VeiculoId
    ORDER BY [Data] DESC;
END;
GO

--todas as manutenções realizadas 
EXEC dbo.sp_GetManutencoesPorVeiculo;
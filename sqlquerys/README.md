# Scripts SQL — GestaoFrota

Scripts a executar **uma vez** na base de dados `GestaoFrota` (depois das migrations EF
terem criado as tabelas Motoristas / Veiculos / Manutencoes / AspNet*).

Ordem recomendada:

1. `01_StoredProcedure_sp_GetManutencoesPorVeiculo.sql`
   - Cria a Stored Procedure `dbo.sp_GetManutencoesPorVeiculo`.
   - Usada pela aplicacao em `ManutencoesController.PorVeiculo`.
2. `02_Trigger_trg_DeleteVeiculo.sql`
   - Cria a tabela `dbo.LogVeiculos` e o trigger `dbo.trg_DeleteVeiculo`.
   - Sempre que apagares um veiculo, fica registado em `LogVeiculos`.


Como executar (SSMS):

- File -> Open -> File... -> escolher o script
- Confirmar que esta a apontar para a base `GestaoFrota`
- Execute (F5)

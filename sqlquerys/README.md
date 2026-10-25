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
 ------------------------------------------------------------------------------
Trigger — trg_DeleteVeiculo
O que faz: quando apagas um veículo da tabela Veiculos, o SQL Server corre sozinho e grava um registo na tabela LogVeiculos com:

VeiculoId — Id do veículo que foi apagado
DataAcao — data/hora do delete
É um log/auditoria: o veículo desaparece de Veiculos, mas fica prova de que existiu e quando foi eliminado.

Stored Procedure — sp_GetManutencoesPorVeiculo
O que faz: devolve todas as manutenções de um veículo, da data mais recente para a mais antiga.

Recebe o Id do veículo (@VeiculoId)
Lê a tabela Manutencoes e filtra por esse veículo
Na app: em Veículos → Detalhes → “Ver Manutenções (SP)” — o ManutencoesController.PorVeiculo chama esta SP em vez de LINQ normal.
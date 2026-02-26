---

### Arquivo: `docs/03-banco-dados.md`

```markdown
# 3. Motor de Segurança e Banco de Dados (SQL Server)

O Microsoft SQL Server atua não apenas como repositório de dados, mas como o **Cérebro de Segurança e Intertravamento** da planta. Grande parte do esforço computacional de validação física ocorre via T-SQL para garantir latência mínima e evitar choques mecânicos na operação.

## 3.1 Views de Consistência (Safety Lógico)
Antes de uma rota ser iniciada, as funções do SQL validam se o cenário físico permite a operação. UDFs como `fn_Consistency_I_Route_Tripper`, `_Feeder`, e `_Damper` comparam:
* **Valor Desejado (`REF_POS.Value`):** A posição que o maquinário *deve* estar.
* **Valor Atual (`MED_CON.Value`):** A posição que a *Tag* do PLC diz que ele está agora.

Se os valores diferirem, as *Views* (ex: `vw_Route_Consistency`) retornam um `BoolVeredict = 0`, bloqueando a UI de liberar a rota.

## 3.2 Matrizes Geométricas
* **Restrição de Origem (`bkr`):** Um Virador de Vagões atende uma única rota por vez. A lógica `fn_Consistency_I_Route_Rule` impede agendamento simultâneo.
* **Pátios e Balizas:** Bloqueio de máquinas caso operem no mesmo local (risco de colisão).

## 3.3 Regras de Produção e Rateio
Para integração com o sistema corporativo GPV, *Stored Procedures* rodam ciclicamente:
* `sp_ProductionRoute_Discharge_Upd`: Subtrai o `InitialLoad` das leituras da balança para calcular a carga transportada.
* `sp_Production_All_Shift_Closing`: Rotina automatizada que fecha as produções ativas a cada 6 horas (00h, 06h, 12h, 18h).

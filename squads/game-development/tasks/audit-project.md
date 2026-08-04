# Task: Audit Project

## task_name
Auditar o estado real de um projeto de jogo

## status
ready

## responsible_executor
`@game-chief`, com apoio de `@unity-gameplay-engineer` e `@game-qa-balance`

## input

1. Repositório e branch alvo.
2. Engine e versão.
3. Documentação de design disponível.
4. Build mais recente, quando existir.
5. Issues, PRs e backlog ativos.

## output

Relatório baseado em evidências contendo:

1. Estado executável atual.
2. Sistemas implementados, integrados e validados.
3. Cenas, prefabs, dados e assets existentes.
4. Bloqueios técnicos e de conteúdo.
5. Dívidas de arquitetura.
6. Lacunas de QA.
7. Próxima build recomendada.
8. Backlog priorizado por risco.

## action_items

1. Confirmar branch e commit auditado.
2. Identificar versão da engine e pacotes.
3. Verificar compilação.
4. Inspecionar Build Settings e cenas.
5. Mapear o fluxo do menu ao fim da experiência.
6. Comparar scripts com configuração em cenas e prefabs.
7. Verificar save, game over, reinício e conclusão.
8. Inspecionar arte, áudio e narrativa integrados.
9. Revisar issues e PRs abertas.
10. Classificar cada item como planejado, programado, integrado, testado ou pronto.

## acceptance_criteria

1. Nenhuma porcentagem sem critério explícito.
2. Código isolado não pode ser marcado como integrado.
3. Todo bloqueio crítico deve ter evidência e ação recomendada.
4. A próxima meta precisa terminar em uma build verificável.
5. Riscos e dependências precisam estar ordenados.

## quality_gate

O relatório deve permitir que outra pessoa decida o que fazer na próxima semana sem precisar reinterpretar o repositório inteiro.

## handoff

Entregar ao `@game-chief` para iniciar `*plan-vertical-slice`.

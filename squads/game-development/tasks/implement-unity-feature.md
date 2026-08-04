# Task: Implement Unity Feature

## task_name
Implementar e integrar uma funcionalidade no Unity

## status
ready

## responsible_executor
`@unity-gameplay-engineer`

## input

1. Especificação aprovada de game design.
2. Critérios de aceite.
3. Cena, prefab ou sistema alvo.
4. Dependências de dados, arte, áudio e narrativa.

## output

Funcionalidade compilada, configurada, integrada, testada e documentada.

## action_items

1. Confirmar a versão do Unity e os pacotes usados.
2. Identificar fonte de verdade e sistemas consumidores.
3. Mapear cenas, prefabs e ScriptableObjects afetados.
4. Implementar a menor mudança funcional.
5. Configurar referências e valores no Inspector.
6. Adicionar validações de null e configuração.
7. Compilar sem erros.
8. Testar em Play Mode.
9. Testar em build quando o fluxo permitir.
10. Executar casos de falha e regressão.
11. Registrar evidência e atualizar documentação.

## acceptance_criteria

1. Não existem erros de compilação.
2. Referências obrigatórias estão configuradas.
3. A funcionalidade está presente em cena ou prefab.
4. O comportamento atende a especificação.
5. Casos extremos não quebram o fluxo principal.
6. O código não cria nova fonte de verdade concorrente.
7. QA consegue reproduzir o teste.

## risks

1. Script implementado sem GameObject ou referência em cena.
2. Dependência circular entre managers.
3. Estado persistente duplicado.
4. Funcionar apenas no Editor.
5. Mudança em prefab sem aplicar ou versionar overrides.

## quality_gate

Usar `checklists/unity-integration-checklist.md` antes do handoff.

## handoff

Entregar a build ou cena testável ao `@game-qa-balance`.

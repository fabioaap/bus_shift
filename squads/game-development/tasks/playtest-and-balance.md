# Task: Playtest and Balance

## task_name
Planejar playtest, registrar bugs e ajustar balanceamento

## status
ready

## responsible_executor
`@game-qa-balance`

## input

1. Build identificada por versão e commit.
2. Objetivo da rodada.
3. Sistemas e riscos incluídos.
4. Hipóteses de dificuldade ou experiência.

## output

Relatório de teste com evidências, bugs, métricas, decisões e retestes necessários.

## action_items

1. Definir perfis e tamanho da amostra.
2. Criar roteiro sem ensinar respostas indevidas.
3. Executar smoke test antes do playtest.
4. Registrar conclusão, falhas, tempo, tensão e uso de ferramentas.
5. Separar bug, confusão, frustração e dificuldade intencional.
6. Priorizar bugs por severidade e frequência.
7. Escolher uma variável por experimento de balanceamento.
8. Comparar valor anterior e valor testado.
9. Recomendar manter, ajustar ou reverter.
10. Retestar correções e executar regressão.

## acceptance_criteria

1. Cada resultado aponta para build e ambiente.
2. Bugs possuem passos de reprodução.
3. Decisões de balanceamento possuem hipótese e métrica.
4. Bugs P0 impedem liberação.
5. Correções só são fechadas após reteste.
6. O relatório distingue percepção de causa técnica.

## severity

1. P0: impede início, progressão, save, conclusão ou causa crash.
2. P1: quebra sistema principal ou gera perda grave de experiência.
3. P2: impacto moderado com alternativa disponível.
4. P3: problema cosmético ou melhoria.

## quality_gate

Nenhuma etapa de produção pode ser declarada concluída sem executar seu checklist de saída.

## handoff

1. Bugs técnicos para `@unity-gameplay-engineer`.
2. Problemas de regra para `@game-designer`.
3. Contradições para `@lore-architect`.
4. Problemas de legibilidade para `@visual-art-director`.
5. Decisão de escopo para `@game-chief`.

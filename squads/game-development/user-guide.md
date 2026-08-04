# Guia de Uso do Game Development Squad

## Entrada principal

Ative o agente de direção:

```text
@game-chief
```

Depois execute:

```text
*audit-project
```

O resultado deve separar claramente o que está planejado, programado, integrado, testado e pronto.

## Começando o Bus Shift

A primeira sequência recomendada é:

```text
@game-chief *audit-project
@game-chief *plan-vertical-slice
@game-designer *design-system
@unity-gameplay-engineer *integration-audit
@game-qa-balance *smoke
```

## Quando usar cada agente

### Game Chief

Use para decisões de escopo, prioridades, dependências, cronograma de builds e coordenação.

### Game Designer

Use para core loop, mecânicas, progressão, dificuldade, feedback e especificações.

### Unity Gameplay Engineer

Use para C#, cenas, prefabs, ScriptableObjects, input, física, build, profiling e correções técnicas.

### Lore Architect

Use para timeline, personagens, diálogos, documentos, eventos narrativos, finais e continuidade.

### Visual Art Director

Use para style guide, briefs de modelagem, materiais, iluminação, VFX, UI e priorização de assets.

### Game QA Balance

Use para smoke tests, bug reports, regressão, playtests, métricas e decisões de balanceamento.

## Regra de conclusão

Uma atividade não está concluída apenas porque existe um arquivo ou script.

A entrega precisa atender às condições relevantes:

1. Compila.
2. Está configurada.
3. Está integrada.
4. Pode ser executada.
5. Foi testada.
6. Possui evidência.
7. Está documentada.

## Vertical slice do Bus Shift

A meta inicial deve conter:

1. Menu principal.
2. Dia 1 manhã e noite.
3. Rota completa.
4. Embarque e desembarque.
5. Dois fantasmas.
6. Tensão, observação e contramedidas.
7. Game Over e reinício.
8. Save e continue.
9. Conclusão temporária.
10. Build Windows.

## Handoffs

Toda entrega termina informando:

1. O que foi concluído.
2. O que não foi concluído.
3. Evidência disponível.
4. Riscos remanescentes.
5. Próximo agente responsável.

## Controle de escopo

Durante a vertical slice, novas ideias devem ser registradas no backlog. Só entram na build atual se resolverem um bloqueio ou forem necessárias para provar o core loop.

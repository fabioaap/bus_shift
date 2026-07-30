# Game Development Squad

Squad AIOX para planejar, construir, integrar, testar e concluir jogos digitais.

O primeiro projeto atendido é o Bus Shift, um jogo de horror psicológico em primeira pessoa desenvolvido em Unity 6.

## Agentes

| Agente | Responsabilidade |
|---|---|
| `@game-chief` | Direção, escopo, prioridades, dependências e orquestração |
| `@game-designer` | Core loop, mecânicas, progressão, economia e balanceamento |
| `@unity-gameplay-engineer` | Unity, C#, cenas, prefabs, integração, build e performance |
| `@lore-architect` | Lore, narrativa, personagens, diálogos e continuidade |
| `@visual-art-director` | Direção visual, UI, materiais, iluminação, VFX e asset briefs |
| `@game-qa-balance` | Testes, bugs, métricas, dificuldade e critérios de release |

## Fluxo recomendado

1. Ative `@game-chief`.
2. Execute `*audit-project` para consolidar o estado real do jogo.
3. Execute `*plan-vertical-slice` para definir a próxima build jogável.
4. O chief distribui o trabalho aos especialistas.
5. Cada entrega precisa produzir evidência verificável, não apenas documentação.
6. O QA valida a integração antes de liberar a próxima etapa.

## Comandos principais

```text
@game-chief
*help
*audit-project
*plan-vertical-slice
*finish-game
*status
```

Comandos especializados:

```text
@game-designer *design-system
@unity-gameplay-engineer *implement-feature
@lore-architect *develop-lore
@visual-art-director *art-direction
@game-qa-balance *playtest
```

## Princípio central

Código isolado não conta como funcionalidade concluída. Uma funcionalidade só está pronta quando está integrada em cena, executável, testada e documentada.

## Estrutura

```text
squads/game-development/
  agents/
  tasks/
  workflows/
  checklists/
  data/
  config.yaml
  README.md
  user-guide.md
```

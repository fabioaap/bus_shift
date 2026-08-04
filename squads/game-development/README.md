# Game Development Squad

Squad AIOX para planejar, construir, integrar, testar e concluir jogos digitais.

O primeiro projeto atendido é o Bus Shift, um jogo de horror psicológico em primeira pessoa desenvolvido em Unity 6.

## Compatibilidade

| Componente | Versão |
|---|---|
| AIOX Core | 5.3.0 |
| Game Development Squad | 0.2.0 |
| Unity | 6 |
| Plataforma inicial | Windows PC |
| Idioma operacional | Português do Brasil |

A instalação canônica do framework fica em `.aiox-core/`. A árvore antiga `.aios-core/`, versão 4.0.3, foi preservada temporariamente nesta PR para uma etapa separada de migração e remoção segura de referências legadas.

## Agentes

| Agente | Responsabilidade |
|---|---|
| `@game-chief` | Direção, escopo, prioridades, dependências e orquestração |
| `@game-designer` | Core loop, mecânicas, progressão, economia e balanceamento |
| `@unity-gameplay-engineer` | Unity, C#, cenas, prefabs, integração, build e performance |
| `@lore-architect` | Lore, narrativa, personagens, diálogos e continuidade |
| `@visual-art-director` | Direção visual, auditoria de assets, UI, materiais, iluminação, VFX e briefs |
| `@game-qa-balance` | Testes, bugs, métricas, dificuldade e critérios de release |

## Fluxo recomendado

1. Ative `@game-chief`.
2. Execute `*audit-project` para consolidar o estado real do jogo.
3. Execute `*plan-vertical-slice` para definir a próxima build jogável.
4. Execute `@visual-art-director *asset-audit` antes de comprar ou produzir assets finais.
5. O chief distribui o trabalho aos especialistas.
6. Cada entrega precisa produzir evidência verificável, não apenas documentação.
7. O QA valida a integração antes de liberar a próxima etapa.

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
@visual-art-director *asset-audit
@game-qa-balance *playtest
```

## Auditoria de assets

O comando `*asset-audit` aplica a task `tasks/audit-game-assets.md` e deve ocorrer antes de aquisição, modelagem final ou substituição de conteúdo existente.

Para a Build 0.1.0 do Bus Shift, a ferramenta local correspondente é:

```text
Bus Shift > Assets > Generate Build 0.1.0 Audit Report
```

Ela gera:

```text
docs/game/art/BUILD_0_1_0_ASSET_AUDIT_GENERATED.md
```

O inventário automático não aprova qualidade visual. Escala, pivôs, rig, colliders, materiais, licenças e uso na cena ainda precisam de inspeção real no Unity Editor.

## Princípios centrais

1. Código isolado não conta como funcionalidade concluída.
2. Uma funcionalidade só está pronta quando está integrada em cena, executável, testada e documentada.
3. Assets existentes devem ser auditados antes de compra ou produção.
4. Vendor assets não devem ser modificados diretamente; adaptações vivem em conteúdo first-party.
5. Todo asset distribuído precisa de origem e licença registradas.

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

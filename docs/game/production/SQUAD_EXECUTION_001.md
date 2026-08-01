# Execução 001 do Game Development Squad

## Identificação

| Campo | Valor |
|---|---|
| Projeto | Bus Shift |
| Branch | `agent/aiox-5-3-game-squad` |
| Workflow | `wf-finish-game` |
| Fase atual | 1, Vertical Slice |
| Meta | Build 0.1.0, Dia 1 jogável |
| Início | 1 de agosto de 2026 |

## Agentes invocados

1. `game-chief`
2. `game-designer`
3. `unity-gameplay-engineer`
4. `lore-architect`
5. `visual-art-director`
6. `game-qa-balance`

## Decisões consolidadas

### Produto

O projeto não deve iniciar alpha testing do jogo completo enquanto não houver uma vertical slice executável.

A primeira entrega validável é a Build 0.1.0 do Dia 1.

### Narrativa

1. Dale Mercer continua sendo o protagonista canônico.
2. Harrison Stone apresenta a rota e proíbe o canal dois.
3. Emma é o primeiro contato sobrenatural claro.
4. Thomas aparece somente como prenúncio sonoro no rádio.
5. Marcus, Grace, Oliver e a manifestação completa de Thomas permanecem para dias posteriores.
6. A cena técnica `Day1Night` será exibida como `Afternoon Shift` na slice.

### Gameplay

1. Emma possui seis segundos de reação no Dia 1.
2. Do Dia 2 em diante, Emma retorna à janela crítica de dois segundos.
3. O prenúncio do rádio não causa game over.
4. Ignorar o rádio aplica tensão uma única vez.
5. Tensão máxima causa game over exatamente uma vez.

### Arquitetura

1. `GameManager` é a fonte única de estado.
2. `GameStateManager` é um adaptador legado.
3. `CurrentTension` cresce de zero a um.
4. `RemainingSanity` é derivada por `1 - CurrentTension`.
5. Finais usam sanidade restante.
6. Concluir o Dia 5 produz `Victory`, não `GameOver`.
7. Ausência da cena de créditos retorna ao menu com segurança.

## Entregáveis produzidos

### Planejamento e governança

1. `docs/game/production/BUILD_0_1_0_VERTICAL_SLICE.md`
2. `docs/game/technical/ADR_001_GAME_STATE_AND_TENSION.md`
3. Épico #71
4. Issues #72 a #76

### Narrativa e design

1. `docs/game/design/BUILD_0_1_0_THREAT_MATRIX.md`
2. `docs/game/narrative/BUILD_0_1_0_NARRATIVE_SCRIPT.md`
3. `docs/game/art/BUILD_0_1_0_VISUAL_BRIEF.md`

### QA

1. `docs/game/testing/BUILD_0_1_0_TEST_MATRIX.md`

### Ferramentas Unity

1. `VerticalSliceBuildValidator.cs`
2. `VerticalSlicePlaceholderFlowBuilder.cs`
3. `VerticalSliceSceneController.cs`
4. `docs/game/production/VERTICAL_SLICE_UNITY_TOOLING.md`

### Gameplay e core

1. Onboarding progressivo de Emma.
2. Prenúncio seguro de Thomas no rádio.
3. Fonte única de estado.
4. Semântica explícita de tensão e sanidade restante.
5. Fluxo corrigido de vitória e finais.

## Estado das issues da Build 0.1.0

| Issue | Pacote | Estado real |
|---:|---|---|
| #71 | Épico | Em andamento |
| #72 | Fundação executável | Código preparado, aguarda execução na Unity |
| #73 | Rota e paradas | Especificado, implementação não iniciada |
| #74 | Emma, rádio, tensão e falha | Implementação parcial, aguarda integração em cena |
| #75 | Lore, visual e áudio | Roteiro e brief concluídos, integração pendente |
| #76 | QA e build | Matriz concluída, execução bloqueada pela build |

## Bloqueio externo atual

As cenas, prefabs e outros assets Unity são versionados por Git LFS.

O conector do GitHub consegue modificar scripts e documentação, mas não consegue abrir o Unity Editor para:

1. Importar e compilar os scripts C#.
2. Executar o gerador de cenas.
3. Salvar assets `.unity` e prefabs com referências do Inspector.
4. Gerar o executável Windows.
5. Executar o smoke test real.

## Próxima ação dentro da Unity

1. Fazer checkout da branch `agent/aiox-5-3-game-squad`.
2. Abrir a pasta `game` no Unity 6.
3. Aguardar a compilação.
4. Corrigir qualquer erro de compilação encontrado.
5. Executar `Bus Shift > Vertical Slice > Build Navigable Placeholder Flow`.
6. Versionar as cinco cenas e o novo `EditorBuildSettings.asset`.
7. Gerar uma Development Build para Windows.
8. Executar a Fase A da matriz de QA.

## Gate para iniciar rota e paradas

O pacote #73 só entra em implementação de cena depois que:

1. As cinco cenas existem.
2. O Build Settings está preenchido.
3. O fluxo placeholder completa um ciclo fora do Editor.
4. Não há crash ou erro P0 na fundação.

## Resultado desta execução

O squad foi utilizado para auditar, decidir, especificar e implementar a primeira camada do projeto.

A branch agora contém uma direção coerente para terminar o jogo e código preparatório suficiente para produzir a primeira build navegável assim que for aberta na Unity.

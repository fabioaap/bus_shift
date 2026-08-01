# Execução 001 do Game Development Squad

## Identificação

| Campo | Valor |
|---|---|
| Projeto | Bus Shift |
| Branch auditada | `agent/aiox-5-3-game-squad` |
| Commit inicial da auditoria | `b11b31c0492e389ad74e6cec83dd052fc572820c` |
| Workflow | `wf-finish-game` |
| Fase | 0, Diagnóstico |
| Meta imediata | Build 0.1.0, vertical slice completa do Dia 1 |
| Data | 1 de agosto de 2026 |

## Agentes invocados

### Game Chief

Responsável por consolidar evidências, corrigir a ordem do roadmap e definir a próxima build verificável.

Decisão: o projeto não deve iniciar alpha testing do jogo completo. A prioridade passa a ser uma vertical slice executável do Dia 1.

### Game Designer

Responsável por fechar o loop mínimo da Build 0.1.0.

Decisão: a slice precisa provar direção, embarque, desembarque, tensão, ameaça, contramedida, falha, reinício e conclusão.

### Unity Gameplay Engineer

Responsável por avaliar compilação, cenas, Build Settings, integração e fluxo executável.

Constatação crítica: `game/ProjectSettings/EditorBuildSettings.asset` não possui cenas registradas. O sistema de transição espera dez cenas nomeadas de `Day1Morning` até `Day5Night`, mas nenhuma está disponível no build.

### Lore Architect

Responsável por restringir a narrativa ao conteúdo necessário para validar o Dia 1.

Decisão: a Build 0.1.0 terá apenas a introdução do motorista, sinais do acidente anterior, duas presenças sobrenaturais e um encerramento temporário. Expansões de lore ficam bloqueadas até o loop ser validado.

### Visual Art Director

Responsável por definir o mínimo visual necessário para tornar a slice legível.

Decisão: usar placeholders coerentes e substituir apenas o que impedir leitura, navegação ou identificação das ameaças. Arte final do restante dos dias não entra na slice.

### Game QA and Balance

Responsável por transformar a slice em uma experiência testável.

Decisão: primeiro validar uma partida completa fora do Editor. Só depois iniciar rodadas de playtest e balanceamento.

## Estado real identificado

| Área | Estado | Evidência | Consequência |
|---|---|---|---|
| Framework de agentes | Atualizado na branch | Estrutura `.aiox-core` e squad adicionados | Pronto para orientar o trabalho |
| Cenas no Build Settings | Bloqueado | Lista `m_Scenes` vazia | Não há build navegável configurada |
| Transição entre períodos | Programada | `SceneTransitionManager` espera dez cenas | Código não prova integração |
| Sistemas de gameplay | Parcialmente programados | Scripts de core, fantasmas, tensão e progressão existem | Precisa validação em cena e prefab |
| Conteúdo de cinco dias | Não validado | Roadmap afirma avanço, mas não há build registrada | Percentuais antigos não são confiáveis |
| Arte final | Incompleta | Issues de ônibus, rota, NPCs, materiais e animações abertas | Slice deve aceitar placeholders |
| Áudio final | Incompleto | SFX, trilha, ambiente e voiceover abertos | Usar áudio temporário na slice |
| QA | Não iniciado de forma válida | Issue de alpha depende de jogo completável | Testes precisam começar pela Build 0.1.0 |
| Release | Não iniciado | Sem build Windows validada | Bloqueado até a slice funcionar |

## Bloqueios priorizados

### P0.1, cenas e Build Settings

Criar e registrar as cenas mínimas da slice.

1. `Bootstrap`
2. `MainMenu`
3. `Day1Morning`
4. `Day1Night`
5. `SliceEnding`

### P0.2, fluxo executável

Garantir o caminho completo:

`Bootstrap` → `MainMenu` → `Day1Morning` → `Day1Night` → `SliceEnding` → `MainMenu`

### P0.3, fonte única de estado

O projeto possui mais de um gerenciador de estado. A slice deve escolher uma única autoridade para estado de jogo, pausa, game over e conclusão.

### P0.4, semântica de tensão e finais

O valor chamado de sanidade cresce com tensão e dispara game over no máximo. A lógica dos finais precisa usar a mesma semântica, evitando tratar tensão alta como resultado positivo.

### P0.5, conclusão do Dia 1

A conclusão da noite precisa ir para um encerramento temporário da slice, não para um fluxo inexistente de cinco dias.

### P0.6, build Windows

A slice somente será considerada integrada quando iniciar e terminar fora do Unity Editor.

## Roadmap corrigido

### Etapa 1, Build 0.1.0

Objetivo: provar o loop completo do Dia 1 com placeholders.

### Etapa 2, Core validado

Objetivo: testar compreensão, tensão, contramedidas, dificuldade e ritmo.

### Etapa 3, expansão para cinco dias

Objetivo: reutilizar a arquitetura validada e adicionar progressão, fantasmas e variações.

### Etapa 4, conteúdo final

Objetivo: substituir placeholders por arte, animação, áudio e narrativa finais.

### Etapa 5, alpha, performance, beta e release

Objetivo: executar as fases já previstas somente quando existir um jogo completável.

## Critério de saída da fase 0

A fase 0 está concluída quando:

1. O relatório está versionado.
2. A Build 0.1.0 possui escopo fechado.
3. O backlog da slice está criado.
4. As tarefas fora da slice estão explicitamente bloqueadas.
5. O próximo trabalho técnico é executável sem reinterpretar o repositório.

## Handoff

Próximo executor: `game-chief` com contribuição de todo o squad.

Próxima tarefa: `plan-vertical-slice`.

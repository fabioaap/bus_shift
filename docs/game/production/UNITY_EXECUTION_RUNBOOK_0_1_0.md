# Runbook de Execução Unity — Build 0.1.0

## Objetivo

Transformar a fundação textual e os scripts já produzidos pelo Game Development Squad em uma vertical slice realmente executável do Dia 1, sem mascarar erros de compilação, sobrescrever cenas intencionais ou declarar a build pronta sem evidência.

## Escopo desta rodada

Esta execução cobre somente:

1. Compilação do projeto na Unity 6.
2. Materialização do fluxo placeholder da vertical slice.
3. Registro das cenas no Build Settings.
4. Smoke test do fluxo navegável.
5. Geração de uma Development Build para Windows.
6. Registro das evidências e dos bloqueios encontrados.

Rota definitiva, passageiros, integração visual completa, iluminação final, áudio final e balanceamento avançado ficam fora desta rodada até a fundação passar no gate.

## Agentes responsáveis

| Agente | Responsabilidade nesta execução |
|---|---|
| `game-chief` | Controlar escopo, gates, bloqueios e decisão de avançar ou interromper |
| `unity-gameplay-engineer` | Resolver compilação, cenas, Build Settings, referências e build Windows |
| `game-qa-balance` | Executar smoke test, registrar evidências e classificar defeitos |
| `game-designer` | Validar se a sequência placeholder representa o loop mínimo do Dia 1 |
| `lore-architect` | Conferir nomes, ordem narrativa, Emma e prenúncio de Thomas |
| `visual-art-director` | Auditar assets importados sem antecipar polish ou arte final |

## Pré-condições

1. Branch ativa: `agent/aiox-5-3-game-squad`.
2. Working tree limpa antes de abrir a Unity.
3. Git LFS instalado e arquivos LFS baixados.
4. Projeto aberto pela pasta `game`.
5. Versão compatível da Unity 6 instalada.
6. Backup ou commit local antes de qualquer alteração manual em cenas existentes.

## Procedimento

### 1. Preparar a cópia local

```bash
git checkout agent/aiox-5-3-game-squad
git pull --ff-only
git lfs install
git lfs pull
git status
```

Critério de aceite:

- A branch correta está ativa.
- Não existem alterações locais inesperadas.
- Os arquivos LFS não permanecem como ponteiros de texto.

### 2. Abrir e compilar

1. Abrir a pasta `game` no Unity Hub.
2. Aguardar a importação completa.
3. Aguardar a compilação de todos os assemblies.
4. Abrir o Console e ativar a visualização de erros.
5. Não executar o gerador de cenas enquanto existir erro de compilação.

Critério de aceite:

- Zero erro de compilação C#.
- Warnings são registrados, mas só bloqueiam se indicarem referência nula, API obsoleta crítica ou asset ausente no fluxo principal.

Se houver erro:

1. Copiar a mensagem completa.
2. Registrar arquivo, linha e stack trace.
3. Corrigir a causa mínima.
4. Recompilar.
5. Não desativar scripts para forçar uma build verde.

### 3. Gerar o fluxo placeholder

Executar no menu da Unity:

```text
Bus Shift > Vertical Slice > Build Navigable Placeholder Flow
```

O comando deve preparar ou atualizar:

1. `Bootstrap`
2. `MainMenu`
3. `Day1Morning`
4. `Day1Night`
5. `SliceEnding`

Critério de aceite:

- As cinco cenas existem.
- Cada cena abre sem erro.
- O controlador temporário da slice está presente onde previsto.
- Nenhuma cena existente com trabalho intencional foi sobrescrita sem revisão.

### 4. Validar a prontidão da build

Executar:

```text
Bus Shift > Vertical Slice > Validate Build Readiness
```

Critério de aceite:

- As cinco cenas estão registradas no Build Settings na ordem correta.
- O validador não reporta Build Settings vazio.
- Não existe duplicação funcional de `GameManager` e `GameStateManager`.
- Os Player Settings necessários para uma Development Build foram revisados.

### 5. Smoke test no Play Mode

Executar o fluxo completo:

```text
Bootstrap -> MainMenu -> Day1Morning -> Day1Night -> SliceEnding -> MainMenu
```

Controles temporários esperados:

- `Enter` ou `Space`: avançar.
- `Escape`: voltar ao menu.
- `R`: reiniciar a cena atual.

Validar também:

1. Pause e retomada não deixam `Time.timeScale` incorreto.
2. Reinício não duplica managers persistentes.
3. Game over de tensão não dispara repetidamente.
4. Emma usa a janela de seis segundos no Dia 1.
5. O rádio prenuncia Thomas sem revelar o nome, sem instanciá-lo e sem causar game over.
6. `Day1Night` aparece para o jogador como turno da tarde na slice.
7. A conclusão da slice usa `SliceEnding`, não o sistema de final completo de cinco dias.

### 6. Gerar a Development Build para Windows

1. Selecionar Windows como plataforma.
2. Ativar `Development Build`.
3. Manter `Script Debugging` apenas se necessário para investigar um defeito.
4. Gerar a build em uma pasta fora de `Assets`.
5. Executar o `.exe` fora do Editor.

Critério de aceite:

- O executável inicia.
- O ciclo completo da slice funciona fora do Editor.
- Não há crash, tela permanentemente preta ou bloqueio de navegação.
- O `Player.log` não contém erro P0 ou P1 no fluxo principal.

### 7. Classificar resultados

| Severidade | Definição | Decisão |
|---|---|---|
| P0 | Projeto não compila, não inicia ou perde dados | Interromper e corrigir antes de qualquer avanço |
| P1 | Fluxo principal quebra, cena não carrega ou estado diverge | Corrigir antes de iniciar rota e paradas |
| P2 | Função secundária incorreta com contorno possível | Registrar e priorizar na mesma milestone |
| P3 | Problema visual, texto ou polish sem impacto no loop | Registrar para rodada posterior |

### 8. Versionar somente evidências válidas

Revisar antes do commit:

```bash
git status
git diff -- game/ProjectSettings/EditorBuildSettings.asset
git diff --stat
```

Arquivos esperados podem incluir:

1. Cenas `.unity` geradas ou corrigidas.
2. Arquivos `.meta` correspondentes.
3. `game/ProjectSettings/EditorBuildSettings.asset`.
4. Correções mínimas de scripts necessárias para compilar.
5. Registro textual da execução e dos defeitos.

Não versionar:

1. `Library/`
2. `Temp/`
3. `Logs/`
4. Builds locais completas.
5. Arquivos de IDE ou cache do sistema.

Mensagem de commit recomendada:

```text
feat(game): materialize and validate build 0.1.0 placeholder flow
```

## Gate de saída

A fundação só é considerada concluída quando todos os itens abaixo forem verdadeiros:

1. O projeto compila na Unity 6 sem erro.
2. As cinco cenas estão materializadas e registradas.
3. O fluxo placeholder completa um ciclo no Play Mode.
4. O mesmo ciclo funciona na Development Build do Windows.
5. Não existe defeito P0 ou P1 aberto na fundação.
6. As alterações geradas foram revisadas antes do commit.
7. O resultado foi anexado às issues #72 e #76.

## Próximo pacote após aprovação

Somente depois desse gate o squad inicia a implementação da issue #73:

1. Rota jogável.
2. Paradas.
3. Progressão do turno.
4. Embarque e desembarque mínimos.
5. Integração gradual de Emma e do rádio sobre o loop real.

## Regra de interrupção

Se a Unity revelar incompatibilidade de versão, referências quebradas em massa, assets LFS ausentes ou cenas existentes que seriam sobrescritas pelo gerador, a execução deve parar, preservar os arquivos e registrar o diagnóstico antes de qualquer regeneração destrutiva.

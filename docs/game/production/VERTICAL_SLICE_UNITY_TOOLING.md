# Ferramentas Unity da Build 0.1.0

## Objetivo

Criar uma fundação executável e navegável para a vertical slice antes de integrar direção, passageiros e fantasmas.

## Requisitos

1. Abrir a pasta `game` no Unity 6.
2. Aguardar a importação e a compilação dos scripts.
3. Resolver qualquer erro de compilação anterior ao uso das ferramentas.
4. Trabalhar na branch `agent/aiox-5-3-game-squad` ou em uma branch criada a partir dela.

## Comando principal

No menu da Unity, executar:

`Bus Shift > Vertical Slice > Build Navigable Placeholder Flow`

O comando executa as seguintes ações:

1. Solicita o salvamento da cena atual.
2. Cria as cenas ausentes.
3. Cria uma câmera e elementos mínimos para cenas placeholder.
4. Adiciona o controlador temporário de navegação.
5. Registra as cenas no Build Settings.
6. Valida a fundação da Build 0.1.0.
7. Restaura a cena aberta anteriormente quando possível.

## Cenas geradas

| Ordem | Cena | Próxima cena |
|---:|---|---|
| 0 | `Bootstrap` | `MainMenu` |
| 1 | `MainMenu` | `Day1Morning` |
| 2 | `Day1Morning` | `Day1Night` |
| 3 | `Day1Night` | `SliceEnding` |
| 4 | `SliceEnding` | `MainMenu` |

## Controles temporários

| Entrada | Ação |
|---|---|
| Enter ou Espaço | Avançar para a próxima cena |
| R | Reiniciar a cena atual |
| Esc | Retornar ao menu |

`Bootstrap` avança automaticamente para `MainMenu`.

## Comandos auxiliares

### Validate Build Readiness

`Bus Shift > Vertical Slice > Validate Build Readiness`

Verifica:

1. Se as cinco cenas existem.
2. Se estão habilitadas no Build Settings.
3. Se o projeto ainda possui os dois gerenciadores de estado.
4. Se o Player Settings ainda usa os nomes genéricos do seed.
5. Se o Build Settings continua vazio.

### Register Required Scenes

`Bus Shift > Vertical Slice > Register Required Scenes`

Registra as cinco cenas quando os assets já existem.

### Create Missing Placeholder Scenes

`Bus Shift > Vertical Slice > Create Missing Placeholder Scenes`

Cria somente as cenas que ainda não existem e registra o conjunto no Build Settings.

## Smoke test

### Preparação

1. Abrir `File > Build Profiles` ou o fluxo equivalente da versão instalada.
2. Selecionar Windows.
3. Ativar Development Build.
4. Gerar o executável em uma pasta fora de `Assets`.

### Roteiro

1. Abrir o executável.
2. Confirmar que `Bootstrap` carrega `MainMenu`.
3. Pressionar Enter e confirmar `Day1Morning`.
4. Pressionar R e confirmar o reinício de `Day1Morning`.
5. Pressionar Enter e confirmar `Day1Night`.
6. Pressionar Enter e confirmar `SliceEnding`.
7. Pressionar Enter e confirmar o retorno ao `MainMenu`.
8. Em `Day1Morning` ou `Day1Night`, pressionar Esc e confirmar o retorno ao menu.
9. Fechar e abrir novamente o executável.
10. Repetir o fluxo uma segunda vez.

## Evidências a registrar na issue #72

1. Versão exata da Unity.
2. Resultado da compilação.
3. Diff de `EditorBuildSettings.asset`.
4. Caminhos das cinco cenas.
5. Log da build.
6. Sistema operacional usado.
7. Capturas do menu, manhã, noite e encerramento.
8. Bugs encontrados.

## Limite da ferramenta

O fluxo placeholder prova somente que o projeto consegue gerar e navegar uma build.

Ele não prova:

1. Direção do ônibus.
2. Rota e paradas.
3. Passageiros.
4. Fantasmas.
5. Tensão.
6. Save e continue.
7. Qualidade visual.
8. Qualidade de áudio.

Esses itens permanecem nas issues #73 a #76.

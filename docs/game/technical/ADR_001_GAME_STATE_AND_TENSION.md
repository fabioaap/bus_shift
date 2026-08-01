# ADR 001, estado global e semântica de tensão

## Status

Aceito para a Build 0.1.0 e para a evolução do jogo completo.

## Contexto

O projeto possuía duas fontes independentes de estado:

1. `GameManager.CurrentState`
2. `GameStateManager.CurrentState`

As duas classes podiam receber transições diferentes, produzindo divergência entre pausa, game over, cutscenes e UI.

O sistema chamado `SanitySystem` também armazenava um valor que aumentava com tensão e atingia game over em `1.0`, mas `EndingManager` interpretava valores altos como sanidade alta e selecionava o melhor final.

Além disso:

1. `SanitySystem.OnGameOver` não possuía assinante encontrado no repositório.
2. `DayManager` alterava o estado para `GameOver` depois de concluir o Dia 5.
3. `EndingManager` solicitava uma cutscene por evento, mas nenhum componente consumia o pedido.
4. A cena `Credits` era carregada sem verificar sua presença no Build Settings.

## Decisão

### Autoridade de estado

`GameManager` é a única fonte de verdade para estado global.

Responsabilidades:

1. Estado atual.
2. Transições.
3. `Time.timeScale`.
4. Motivo do último game over.
5. Entrada em jogo, pausa, retorno, game over e vitória.

`GameStateManager` permanece como adaptador de compatibilidade.

Ele:

1. Não armazena estado próprio.
2. Encaminha operações para `GameManager`.
3. Repassa o evento legado com estado anterior e seguinte.

### Semântica de tensão

A variável canônica é `CurrentTension`.

| Valor | Significado |
|---:|---|
| `0.0` | Controle total |
| `0.5` | Tensão intermediária |
| `1.0` | Limite e game over |

`RemainingSanity` é derivada por:

`1.0 - CurrentTension`

O alias legado `CurrentSanity` continua disponível temporariamente, mas representa tensão e está marcado como obsoleto.

### Game over por tensão

Quando `CurrentTension` atinge `1.0`:

1. O evento legado é emitido.
2. `GameManager.TriggerGameOver` é chamado diretamente.
3. Uma trava impede disparos duplicados.
4. Reinicializar um período libera a trava.

### Finais

`EndingManager` usa `RemainingSanity`.

1. Sanidade restante alta pode produzir final bom.
2. Sanidade restante intermediária produz final neutro.
3. Sanidade restante baixa ou morte por fantasma produz final ruim.

O gerenciador chama `CutsceneManager` diretamente quando disponível.

Ao terminar a cutscene:

1. Carrega `Credits` quando a cena existe.
2. Retorna com segurança ao `MainMenu` quando a cena ainda não existe.

### Conclusão do jogo

Terminar o Dia 5:

1. Define `IsGameComplete`.
2. Entra em `Victory`.
3. Emite `OnGameCompleted`.
4. Permite que o ending assuma o fluxo cinematográfico.

Nunca entra em `GameOver` por concluir o jogo.

## Consequências positivas

1. Pausa e game over não podem divergir entre managers.
2. UI e sistemas podem escolher tensão ou sanidade restante explicitamente.
3. Finais deixam de usar a escala invertida.
4. Game over por tensão não depende de um assinante externo.
5. A conclusão do jogo tem significado correto.
6. Integrações legadas continuam compilando durante a migração.

## Consequências e dívidas

1. `GameStateManager` deve ser removido após a migração completa das cenas.
2. `SaveData.Sanity` ainda contém tensão por compatibilidade e deve ser renomeado em uma migração versionada.
3. HUDs inscritos em `OnSanityChanged` recebem tensão e precisam migrar para `OnTensionChanged` ou `OnRemainingSanityChanged`.
4. As cenas precisam possuir somente um `GameManager` persistente.
5. O fluxo de cutscene ainda precisa ser validado dentro da Unity.
6. A cena `Credits` ainda precisa ser criada e registrada para o jogo completo.

## Critérios de validação

1. Estado não diverge após pausa e retorno.
2. `Time.timeScale` volta a um após game over.
3. Tensão máxima dispara um único game over.
4. Reiniciar o período permite nova tentativa.
5. Tensão alta nunca produz final bom.
6. Concluir o Dia 5 nunca produz game over.
7. A ausência de `Credits` retorna ao menu sem exceção de cena ausente.

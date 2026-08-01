# Bus Shift, matriz de testes da Build 0.1.0

## Objetivo

Validar a vertical slice do Dia 1 como experiência executável, compreensível e repetível fora do Unity Editor.

## Escopo de validação

1. Inicialização da aplicação.
2. Navegação entre cenas.
3. Menu, reinício e retorno.
4. Rota da manhã e da tarde.
5. Três paradas.
6. Embarque e desembarque.
7. Prenúncio de Thomas pelo rádio.
8. Primeiro contato com Emma.
9. Panel Lock.
10. Tensão, game over e reinício.
11. Save e continue.
12. Encerramento da slice.
13. Estabilidade da build Windows.

## Classificação de severidade

| Severidade | Definição | Exemplo |
|---|---|---|
| P0 | Impede iniciar, continuar ou concluir a slice | Crash, tela preta, cena ausente, save corrompido |
| P1 | Mecânica obrigatória falha ou produz resultado incorreto | Emma não pode ser neutralizada, parada não conclui |
| P2 | Problema relevante com alternativa disponível | Legenda atrasada, feedback pouco claro |
| P3 | Polish sem impacto no fluxo principal | Alinhamento visual, pequeno pop de áudio |

## Ambientes mínimos

| Ambiente | Sistema | Configuração |
|---|---|---|
| A | Windows 11 | 1920 por 1080, teclado e mouse |
| B | Windows 10 ou 11 | Resolução diferente, hardware secundário disponível |

A versão exata da Unity, GPU, CPU, memória e resolução deve ser registrada em cada execução.

## Fase A, fundação navegável

### QA 001, inicialização pelo executável

**Pré condição:** Build Windows gerada com `Bootstrap` como primeira cena.

**Passos:**

1. Fechar o Unity Editor.
2. Executar o arquivo do jogo.
3. Observar a inicialização.

**Resultado esperado:**

1. Não ocorre crash.
2. `Bootstrap` carrega `MainMenu`.
3. Não há cena vazia ou erro de cena ausente.
4. O menu responde ao teclado e mouse.

**Severidade de falha:** P0.

### QA 002, fluxo completo das cenas placeholder

**Passos:**

1. Iniciar em `MainMenu`.
2. Avançar para `Day1Morning`.
3. Avançar para `Day1Night`.
4. Avançar para `SliceEnding`.
5. Retornar ao `MainMenu`.

**Resultado esperado:**

1. Cada cena carrega uma única vez.
2. A ordem é correta.
3. Nenhuma transição fica presa em tela preta.
4. O retorno ao menu não cria objetos persistentes duplicados.

**Severidade de falha:** P0.

### QA 003, reinício da cena

**Passos:**

1. Entrar em `Day1Morning`.
2. Reiniciar a cena.
3. Repetir em `Day1Night`.

**Resultado esperado:**

1. A cena recarrega.
2. O input continua funcional.
3. Eventos e objetos não duplicam.
4. O estado temporário do período é reiniciado.

**Severidade de falha:** P0 quando bloqueia o fluxo; P1 quando duplica eventos.

### QA 004, retorno ao menu

**Passos:**

1. Entrar em `Day1Morning`.
2. Retornar ao menu.
3. Iniciar novamente.
4. Repetir a partir de `Day1Night`.

**Resultado esperado:**

1. O menu carrega corretamente.
2. Um novo jogo não herda tensão, passageiros ou eventos da tentativa anterior.
3. Não há objetos persistentes duplicados.

**Severidade de falha:** P0 ou P1.

## Fase B, loop da rota

### QA 010, início da rota matinal

**Pré condição:** Novo jogo iniciado.

**Resultado esperado:**

1. A cena exibe `DAY 1` e `MORNING SHIFT`.
2. O objetivo inicial é compreensível.
3. O ônibus inicia em condição segura.
4. O jogador recebe controle somente após a apresentação necessária.

### QA 011, parada 1

**Passos:**

1. Conduzir até Elm and Depot.
2. Parar na área indicada.
3. Abrir a porta.
4. Aguardar embarque.
5. Fechar a porta.

**Resultado esperado:**

1. A parada reconhece posição e velocidade adequadas.
2. Passageiros embarcam uma única vez.
3. A porta informa estado aberto e fechado.
4. O próximo objetivo é exibido.

### QA 012, parada 2 e risada de Emma

**Resultado esperado:**

1. O evento ocorre após a saída da parada.
2. A risada é audível e possui legenda ou apoio visual.
3. Nenhuma criança viva aparece como origem da risada.
4. O evento não interrompe a direção em situação impossível.
5. Emma não se manifesta visualmente durante a manhã.

### QA 013, parada 3 e conclusão da manhã

**Resultado esperado:**

1. A rota não conclui antes das três paradas.
2. Todas as crianças desembarcam.
3. A conclusão dispara uma única transição.
4. A interface seguinte exibe `AFTERNOON SHIFT`, mesmo que a cena técnica seja `Day1Night`.

## Fase C, rádio e prenúncio de Thomas

### QA 020, canal dois no início

**Passos:**

1. Ligar o Bus 104.
2. Observar o rádio.
3. Aguardar a instrução contextual.
4. Restaurar o canal quatro.

**Resultado esperado:**

1. O rádio muda para o canal dois.
2. A estática inicia antes da instrução.
3. A voz não recebe identificação de personagem.
4. Restaurar o canal quatro encerra a interferência imediatamente.
5. O evento não causa game over.

### QA 021, segunda interferência sem instrução completa

**Resultado esperado:**

1. A interferência retorna na rota da tarde.
2. O tutorial completo não se repete.
3. Ignorar o evento aumenta a tensão somente uma vez.
4. Responder corretamente remove o risco.
5. Thomas não aparece visualmente.

## Fase D, Emma

### QA 030, prenúncio da manifestação

**Resultado esperado:**

1. A risada progride em intensidade.
2. O sinal começa antes da janela de falha.
3. Emma aparece à direita do motorista.
4. A mão avança em direção ao painel.
5. O primeiro encontro indica o Panel Lock.

### QA 031, sucesso com Panel Lock

**Passos:**

1. Aguardar Emma manifestar se.
2. Ativar Panel Lock dentro da janela.

**Resultado esperado:**

1. O Panel Lock neutraliza Emma.
2. O feedback é imediato.
3. Emma desaparece por corte ou interferência curta.
4. A tensão não atinge o limite.
5. O diálogo das crianças vivas ocorre uma única vez.
6. A rota continua.

### QA 032, falha contra Emma

**Passos:**

1. Aguardar Emma manifestar se.
2. Não ativar Panel Lock.

**Resultado esperado:**

1. A janela inicial dura aproximadamente seis segundos.
2. Emma alcança o painel.
3. A tensão atinge o limite ou o estado de falha definido é disparado.
4. O game over ocorre uma única vez.
5. Nenhuma morte gráfica de criança é exibida.
6. O menu de reinício responde.

### QA 033, reinício após Emma

**Resultado esperado:**

1. Reiniciar retorna ao começo da rota da tarde.
2. Tensão volta ao valor inicial.
3. Emma pode surgir novamente.
4. Eventos da tentativa anterior não permanecem inscritos.
5. A instrução contextual pode reaparecer em versão curta.

## Fase E, tensão

### QA 040, escala crescente

**Resultado esperado:**

1. A tensão inicia próxima de zero.
2. Eventos negativos aumentam o valor.
3. Contramedidas podem reduzir ou interromper o aumento.
4. O valor permanece entre zero e um.
5. Um valor alto nunca é interpretado como resultado positivo.

### QA 041, feedback visual da tensão

**Resultado esperado:**

1. A progressão visual é contínua.
2. A cena não muda abruptamente entre estágios.
3. A estrada e as ações obrigatórias permanecem legíveis.
4. O pulso vermelho aparece somente em risco alto.

### QA 042, tensão máxima

**Resultado esperado:**

1. O limite dispara game over.
2. O game over ocorre somente uma vez.
3. O input de gameplay é bloqueado após a falha.
4. Reiniciar ou retornar ao menu funciona.

## Fase F, save e continue

### QA 050, save após a manhã

**Resultado esperado:**

1. Concluir a manhã grava o período seguinte.
2. Fechar o jogo não remove o progresso.
3. `Continue` fica disponível.

### QA 051, continue na tarde

**Resultado esperado:**

1. Continue carrega a rota da tarde.
2. A tensão da tentativa anterior não é restaurada quando a regra da slice determina reinício.
3. O rádio e Emma podem executar seus eventos normalmente.
4. Nenhum evento da manhã é repetido indevidamente.

### QA 052, novo jogo com save existente

**Resultado esperado:**

1. Novo jogo solicita confirmação quando necessário.
2. Confirmar inicia a manhã.
3. O save anterior é substituído de maneira previsível.

## Fase G, encerramento

### QA 060, conclusão da rota da tarde

**Resultado esperado:**

1. A rota exige as três paradas.
2. Emma precisa ter sido resolvida.
3. A transição para `SliceEnding` ocorre uma única vez.

### QA 061, encerramento narrativo

**Resultado esperado:**

1. O ônibus está vazio.
2. O banco molhado é visível.
3. O rádio desligado emite um pulso de estática.
4. A fala `Five names` possui legenda sem identificação.
5. A forma no retrovisor permanece ambígua.
6. O texto de fim da vertical slice é exibido.

### QA 062, retorno e replay

**Resultado esperado:**

1. Retornar ao menu funciona.
2. Jogar novamente inicia a manhã.
3. Nenhum objeto ou evento é duplicado.

## Fase H, regressão técnica

### QA 070, cinco ciclos consecutivos

Executar o fluxo completo cinco vezes na mesma sessão.

**Resultado esperado:**

1. Sem crash.
2. Sem aumento progressivo de objetos persistentes.
3. Sem duplicação de áudio.
4. Sem duplicação de eventos.
5. Sem degradação perceptível de desempenho.

### QA 071, fechar durante carregamento

**Resultado esperado:**

1. Nenhum save fica corrompido.
2. Abrir novamente permite iniciar ou continuar.

### QA 072, resoluções diferentes

**Resultado esperado:**

1. Menu e HUD permanecem visíveis.
2. Legendas não cortam.
3. Panel Lock e rádio continuam identificáveis.
4. O retrovisor não perde a área necessária de leitura.

## Registro das cinco partidas internas

| Sessão | Jogador | Ambiente | Concluiu | Tempo | Game overs | Emma reconhecida | Rádio reconhecido | Tensão 1 a 5 | Clareza 1 a 5 | Bugs |
|---:|---|---|---|---:|---:|---|---|---:|---:|---|
| 1 |  |  |  |  |  |  |  |  |  |  |
| 2 |  |  |  |  |  |  |  |  |  |  |
| 3 |  |  |  |  |  |  |  |  |  |  |
| 4 |  |  |  |  |  |  |  |  |  |  |
| 5 |  |  |  |  |  |  |  |  |  |  |

## Modelo de bug

### Título

`[Build 0.1.0][P0 a P3][Sistema] Resumo observável`

### Campos

1. Build e commit.
2. Ambiente.
3. Pré condição.
4. Passos para reproduzir.
5. Resultado atual.
6. Resultado esperado.
7. Frequência.
8. Evidência.
9. Hipótese técnica, quando houver.
10. Regressão ou problema novo.

## Critério final de aprovação

A Build 0.1.0 está apta para decisão do core loop quando:

1. Todos os casos P0 passam.
2. Nenhum bug P0 permanece aberto.
3. Bugs P1 possuem correção ou decisão registrada.
4. Cinco partidas completas foram executadas.
5. Pelo menos quatro de cinco jogadores neutralizam Emma após o primeiro tutorial.
6. Pelo menos quatro de cinco jogadores respondem ao rádio na segunda ocorrência.
7. O caminho principal é completável sem ferramenta do Editor.
8. O squad registra a decisão de manter, ajustar ou descartar o core loop.

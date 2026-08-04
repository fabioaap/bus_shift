# Bus Shift, roteiro narrativo da Build 0.1.0

## Status

Roteiro de implementação da vertical slice do Dia 1.

Fonte canônica principal: `docs/game/narrative/story.md`.

## Princípios

1. Dale começa o turno porque precisa do trabalho, não porque quer investigar fantasmas.
2. Harrison Stone minimiza o passado do ônibus sem explicar o acidente.
3. O rádio no canal dois funciona como prenúncio de Thomas.
4. Emma é o primeiro fantasma visto com clareza.
5. As crianças vivas reagem ao comportamento de Dale, não ao sobrenatural.
6. O Dia 1 termina com negação pragmática, não com investigação aberta.
7. O jornal e os nomes das cinco crianças permanecem para o Dia 2.

## Cena 0, Bootstrap

### Função

Inicializar sistemas e carregar o menu.

### Conteúdo visível

Tela preta.

Som distante de motor a diesel tentando pegar.

Uma breve estática de rádio.

Nenhum texto narrativo obrigatório.

## Cena 1, MainMenu

### Título

`BUS SHIFT`

### Opções mínimas

1. New Shift
2. Continue
3. Settings
4. Exit

Na Build 0.1.0, `Continue` pode permanecer desabilitado até o save do período ser integrado.

### Atmosfera

1. Ônibus 104 estacionado sob luz fria.
2. Névoa baixa.
3. Rádio desligado com um pulso ocasional de estática.
4. Nenhum fantasma claramente visível.

## Cena 2, Day1Morning

### Identificação exibida

`DAY 1`

`MORNING SHIFT`

`RAVENSWOOD ELEMENTARY, 6:47 AM`

### Sequência A, Stone entrega a rota

Stone espera perto de um Crown Victoria. Ele não estende a mão.

**Stone:**

> Mr. Mercer. Good, you're early.

Stone entrega o cartão plastificado da rota e toca duas vezes no papel.

**Stone:**

> Morning pickup starts at Elm and Depot. You finish at the school. Afternoon is the reverse.

Ele aponta para o rádio.

**Stone:**

> Dispatch is channel four. Don't use channel two.

**Dale:**

> What's on channel two?

Stone ajeita o casaco antes de responder.

**Stone:**

> Nothing you need.

Ele olha para o Bus 104.

**Stone:**

> The bus is old. It has quirks. Earl says it's road ready, so it's road ready. Keep the kids safe and keep to schedule.

**Dale:**

> What happened to the last driver?

**Stone:**

> He stopped driving.

Stone segue em direção à escola antes de Dale responder.

### Sequência B, inspeção do Bus 104

Elementos ambientais obrigatórios:

1. Pintura amarela oxidada.
2. Amassado no para choque traseiro.
3. Adesivo desbotado `PRECIOUS CARGO ON BOARD`.
4. Cheiro de vinil, óleo antigo e metal molhado.
5. Mancha no canto inferior esquerdo do retrovisor.
6. Banco do motorista rangendo.

Dale senta, ajusta o espelho e gira a chave.

O motor pega na segunda tentativa.

O rádio liga sozinho no canal dois.

Estática com um fragmento quase humano surge por quatro segundos.

Objetivo contextual:

`RETURN RADIO TO CHANNEL 4`

Após a ação correta, a estática desaparece.

**Dale:**

> Alright then. Let's go to work.

### Sequência C, rota matinal

#### Parada 1, Elm and Depot

Função:

1. Ensinar parada segura.
2. Ensinar abertura e fechamento da porta.
3. Embarcar o primeiro grupo.
4. Estabelecer comportamento normal das crianças.

#### Parada 2, Birch Avenue

Após o embarque e a saída da parada:

Uma risada curta surge atrás da cabeça de Dale.

> hihihi

Dale ajusta o retrovisor.

Todas as crianças vivas estão sentadas.

Nenhuma ri.

Uma corrente fria passa pela cabine.

**Dale:**

> Long night. That's all.

A risada não se repete durante a manhã.

#### Parada 3, chegada à escola

Objetivo:

1. Estacionar na área correta.
2. Abrir a porta.
3. Desembarcar todas as crianças.
4. Finalizar o turno.

Transição:

Fade para preto.

Texto:

`AFTERNOON SHIFT, 3:18 PM`

O nome técnico da próxima cena permanece `Day1Night` durante a Build 0.1.0.

## Cena 3, Day1Night

### Identificação exibida

`DAY 1`

`AFTERNOON SHIFT`

`DENSE FOG ADVISORY`

### Sequência A, prenúncio no rádio

Durante a saída da escola, o canal quatro apresenta interferência.

Por um instante, o visor muda para canal dois.

Uma voz infantil quase inaudível começa uma frase sem terminar.

> ...and the bus came to the bridge...

Nenhum nome é exibido.

O jogador precisa restaurar o canal quatro sem instrução completa.

Ignorar por alguns segundos aumenta levemente a tensão, mas não causa game over.

### Sequência B, primeira risada

Após a primeira parada:

> hihihi

A risada surge atrás de Dale.

Quando ele olha pelo retrovisor, não há ninguém fora do padrão.

### Sequência C, deslocamento da risada

Após a segunda parada:

A risada surge diretamente à direita do motorista.

Ela passa de suave para média.

O ar esfria.

Uma interferência curta percorre o painel.

### Sequência D, manifestação de Emma

Emma aparece ao lado do motorista.

Direção visual:

1. Menina de dez anos.
2. Pele pálida como papel molhado.
3. Roupas do início dos anos 1990.
4. Starter jacket.
5. Jeans acid wash.
6. Tênis com velcro.
7. Cabelo e roupas levemente úmidos.
8. Sorriso que aumenta sem movimento natural do restante do rosto.

Emma aproxima a mão do controle da porta.

**Emma:**

> Red button.

Objetivo contextual do primeiro encontro:

`LOCK THE CONTROL PANEL`

A janela inicial é de seis segundos.

### Resultado de sucesso

Dale cobre ou bloqueia o painel.

**Dale:**

> Don't touch that. Sit down. Now.

Emma não recua fisicamente.

Ela desaparece como uma mudança abrupta de canal.

Silêncio.

Todas as crianças vivas olham para Dale.

**Girl, row three:**

> Who's he yelling at?

**Boy behind her:**

> My mom said they hire anybody for this job now.

Dale retira lentamente a mão do painel.

**Dale:**

> Eyes front. We're almost there.

### Resultado de falha

Emma alcança o painel.

A porta tenta abrir ou o sistema entra em estado crítico controlado.

A tensão atinge o limite e o jogo exibe game over.

Opções:

1. Restart Afternoon Shift
2. Return to Main Menu

A falha não deve mostrar morte gráfica de crianças.

### Sequência E, última parada

Após neutralizar Emma:

1. Concluir a terceira parada.
2. Desembarcar as crianças.
3. Retornar ao depot.
4. Estacionar e desligar o motor.

## Cena 4, SliceEnding

### Função

Encerrar o Dia 1 sem antecipar as revelações do Dia 2.

### Sequência

Ônibus vazio no depot.

Dale confere o corredor antes de sair.

Um banco na fileira traseira está molhado.

Não há janela aberta.

O rádio, já desligado, emite um único pulso de estática.

Uma voz infantil quase incompreensível diz:

> Five names.

Dale olha para o rádio.

Nada.

Ele pega o casaco e desce.

**Dale, baixo:**

> Old bus. Old wiring.

A porta fecha.

A câmera permanece dentro do ônibus por dois segundos.

No retrovisor, uma forma infantil parece ocupar um banco, mas desaparece antes de confirmação.

Fade para preto.

### Texto de desenvolvimento

`END OF BUILD 0.1.0 VERTICAL SLICE`

`THE ROUTE CONTINUES IN DAY 2`

Opções:

1. Return to Main Menu
2. Play Again

## Texto de game over

Título:

`SHIFT ENDED`

Mensagem:

`You lost control of the route.`

Ações:

1. Restart Shift
2. Main Menu

## Regras de implementação

1. Diálogos não podem interromper o controle do ônibus em situação perigosa.
2. Legendas devem estar disponíveis para rádio, risadas e falas.
3. A fala de Thomas no Dia 1 não recebe identificação de personagem.
4. O tutorial de Emma aparece apenas na primeira tentativa válida.
5. Após reinício, o jogador pode rever uma versão curta da instrução.
6. O roteiro deve funcionar com texto e áudio temporários.
7. O conteúdo do jornal, Victor Graves e os cinco nomes completos permanece fora da Build 0.1.0.
8. A interface deve usar `Afternoon Shift`, apesar do nome técnico `Day1Night`.

## Critérios narrativos de aceite

1. O jogador entende que Dale precisa do emprego.
2. Stone parece esconder algo sem confessar o acidente.
3. O canal dois é percebido como proibido e anormal.
4. Emma é reconhecida como o primeiro contato claro.
5. As crianças vivas não enxergam Emma.
6. Dale termina o dia ainda tentando racionalizar o ocorrido.
7. O final cria curiosidade sobre cinco crianças sem revelar o mistério completo.

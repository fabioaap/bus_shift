# Bus Shift, brief visual da Build 0.1.0

## Objetivo

Dar legibilidade, atmosfera e identidade à vertical slice do Dia 1 sem bloquear a implementação por falta de arte final.

Fonte visual principal: `docs/game/art/style-guide.md`.

## Princípio central

O horror nasce da corrupção de um espaço familiar.

A Build 0.1.0 deve parecer deliberadamente low poly, envelhecida e opressiva. Placeholder não significa aleatório: toda forma, luz e cor temporária precisa respeitar a leitura final do jogo.

## Cenas

### Bootstrap

1. Fundo preto.
2. Nenhum elemento 3D obrigatório.
3. Som de motor tentando pegar.
4. Pulso curto de estática.
5. Transição rápida para o menu.

### MainMenu

Composição:

1. Bus 104 em três quartos frontal.
2. Farol ou poste lateral criando recorte parcial.
3. Névoa baixa.
4. Escola ou depot apenas sugerido ao fundo.
5. Grande área negativa para título e menu.

Paleta:

1. Azul marinho profundo `#0A0E1A`.
2. Sombra quase preta `#060810`.
3. Amarelo ônibus desbotado `#C8A84B`.
4. Laranja de poste `#C87832`.
5. Branco HUD `#E0E0D8`.

Movimento:

1. Névoa lenta.
2. Flicker discreto do poste.
3. Rádio com luz intermitente rara.
4. Nada que confirme um fantasma.

### Day1Morning

Meta:

Apresentar um mundo comum, porém gasto e frio.

Luz:

1. Céu cinza pesado.
2. Direcional difusa.
3. Sombras suaves, sem sensação de conforto.
4. Temperatura levemente fria.

Paleta:

1. Amarelo ônibus desbotado `#C8A84B`.
2. Amarelo oxidado `#A07830`.
3. Cinza céu `#8A9099`.
4. Verde murcho `#5A6B4A`.
5. Asfalto velho `#3D3D3D`.

Elementos obrigatórios:

1. Cartão da rota.
2. Rádio com visor de canal.
3. Panel Lock identificável.
4. Retrovisor com mancha no canto inferior esquerdo.
5. Adesivo `PRECIOUS CARGO ON BOARD`.
6. Três paradas visualmente distintas.
7. Crianças vivas com silhuetas legíveis.
8. Banco do motorista e painel funcionais.

### Day1Night, exibido como Afternoon Shift

Meta:

Transformar a mesma rota em um espaço de incerteza sem parecer uma noite completa.

Luz:

1. Sol baixo e laranja enfraquecido.
2. Névoa densa.
3. Interior mais escuro que o exterior.
4. Faróis já perceptíveis.
5. Contraste maior no painel e no corredor.

Regra:

A cena técnica se chama `Day1Night`, mas a direção deve parecer final de tarde sob alerta de neblina.

### SliceEnding

1. Depot quase vazio.
2. Iluminação fluorescente distante.
3. Interior do ônibus mais escuro que na rota.
4. Banco traseiro molhado.
5. Rádio apagado com pulso de estática.
6. Forma ambígua no retrovisor por menos de um segundo.

## Bus 104

### Silhueta

1. 1989 International AmTran.
2. Proporção longa e pesada.
3. Capô e frente reconhecíveis.
4. Pintura amarela oxidada.
5. Para choque traseiro amassado.

### Interior mínimo

1. Cabine do motorista.
2. Painel com porta, rádio e Panel Lock.
3. Retrovisor interno.
4. Corredor central.
5. Pelo menos cinco fileiras funcionais para a slice.
6. Porta dianteira animável.
7. Janelas com leitura de neblina e reflexo.

### Placeholder permitido

1. Exterior adquirido ou simplificado.
2. Bancos repetidos por instância.
3. Texturas simples com variação de roughness.
4. Mãos do motorista ausentes na primeira build navegável.
5. Crianças vivas representadas por modelos base com variações de material.

### Placeholder não permitido

1. Cubo sem proporção de ônibus.
2. Painel sem diferenciação entre ações.
3. Retrovisor incapaz de mostrar a cabine.
4. Porta sem feedback de estado.
5. Objetos com cores fora da paleta que confundam interação.

## Emma

### Aparência

1. Criança de dez anos.
2. Silhueta angular e levemente incompleta.
3. Starter jacket.
4. Jeans acid wash.
5. Tênis com velcro.
6. Materiais azul cinza dessaturados.
7. Superfície úmida.
8. Opacidade entre 60% e 75%.
9. Fresnel discreto nas bordas.
10. Olhos claros sem pupila definida.

### Movimento

1. Poucos quadros ou movimento intencionalmente quebrado.
2. Corpo quase imóvel.
3. Mão avançando em etapas para o painel.
4. Sorriso aumentando sem animação facial natural.
5. Desaparecimento por corte, não por fade suave.

### Estados visuais

| Estado | Leitura |
|---|---|
| Prenúncio | Vinheta fria e risada sem modelo visível |
| Manifestação | Silhueta à direita do motorista |
| Janela crítica | Mão próxima do botão vermelho e pulso âmbar no Panel Lock |
| Sucesso | Corte visual com pequeno ruído de canal |
| Falha | Interferência forte, painel vermelho e perda de controle |

## Thomas, presença implícita

Thomas não recebe modelo visível na Build 0.1.0.

Feedback permitido:

1. Visor do rádio mudando para canal dois.
2. Estática com padrão.
3. Pequeno ruído cromático no painel.
4. Reflexo impossível no vidro sem forma identificável.
5. Sussurro vindo do fundo do ônibus.

Feedback proibido:

1. Nome de Thomas.
2. Retrato ou silhueta clara.
3. Manifestação completa.
4. Game over específico de Thomas.

## Três paradas

### Parada 1, Elm and Depot

1. Área mais industrial.
2. Cerca, galpão ou depósito.
3. Luz fria.
4. Função de tutorial.

### Parada 2, Birch Avenue

1. Casas antigas.
2. Árvore sem folhas ou vegetação murcha.
3. Espaço onde ocorre a primeira risada.
4. Composição que permita perceber que não há criança fora do padrão.

### Parada 3, Ravenswood Elementary

1. Fachada de tijolo vermelho escurecido.
2. Bandeira desgastada.
3. Área de desembarque clara.
4. Stone ou funcionários apenas quando necessário.

## UI e HUD

### Princípios

1. HUD quase invisível em estado calmo.
2. Texto sem serifa e peso leve.
3. Opacidade máxima de 80%.
4. Cor somente quando comunica estado.
5. Objetivos temporários próximos ao ponto de atenção.

### Elementos mínimos

1. Objetivo atual.
2. Estado da porta.
3. Canal do rádio.
4. Estado do Panel Lock.
5. Indicador de tensão.
6. Marcador de parada.
7. Legendas.
8. Game over.
9. Tela de conclusão.

### Tensão

| Faixa | Tratamento |
|---|---|
| 0% a 25% | Paleta normal |
| 26% a 50% | Dessaturação leve e vinheta suave |
| 51% a 75% | Contraste maior, vinheta e aberração discreta |
| 76% a 100% | Quase monocromático, pulso vermelho e grain |

A transição precisa ser contínua.

## Áudio visualizado

Todo sinal sonoro essencial deve possuir apoio visual:

1. Canal dois piscando no rádio.
2. Legenda direcional para sussurro.
3. Ícone discreto ou vibração visual para risada.
4. Mudança de intensidade no Panel Lock durante Emma.

## Lista de assets da slice

### P0

1. Bus 104 exterior e interior utilizáveis.
2. Painel do motorista.
3. Rádio.
4. Panel Lock.
5. Retrovisor.
6. Porta dianteira.
7. Três pontos de parada.
8. Escola.
9. Emma placeholder coerente.
10. Crianças vivas base.
11. Névoa.
12. UI funcional.

### P1

1. Stone placeholder.
2. Cartão da rota.
3. Adesivo do ônibus.
4. Banco molhado para o final.
5. Variações de roupa das crianças.
6. Efeitos de tensão.
7. Animação quebrada de Emma.

### Fora da slice

1. Modelos finais dos cinco fantasmas.
2. Interior completo de vinte bancos.
3. Todos os bairros de Ravenswood.
4. Victor Graves.
5. Cutscenes finais.
6. Arte do jornal do Dia 2.
7. Assets do ARG.

## Critérios visuais de aceite

1. O jogador diferencia manhã e tarde em uma captura sem HUD.
2. Rádio, Panel Lock, porta e retrovisor são identificáveis em até cinco segundos.
3. Emma é legível sem parecer uma criança viva.
4. O ônibus parece velho e funcional, não abandonado a ponto de ser impossível operar.
5. A névoa reduz visão sem esconder a rota obrigatória.
6. A UI não compete com a estrada.
7. Nenhum placeholder introduz uma cor ou forma que pareça uma mecânica inexistente.
8. A tensão visual cresce sem mudança abrupta de estágio.

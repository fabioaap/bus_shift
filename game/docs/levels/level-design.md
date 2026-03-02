# Bus Shift — Level Design: Rota do Ônibus 104

> **Status:** Draft v1.0 | **Engine:** Unity 6 (URP) | **Estilo:** Low Poly Horror 1ª Pessoa
> **Localização fictícia:** Ravenswood, Pennsylvania, USA
> **Referência de Issue:** #10 (Gray Box Prototype)

---

## 1. Visão Geral da Rota

### 1.1 Sumário Executivo

O **Ônibus 104** percorre um circuito circular de **9,2 km** pelos diferentes bairros de Ravenswood, com partida e chegada sempre na **Ravenswood Elementary School**. A rota cobre cinco pontos de parada distintos, atravessando zonas residenciais, industriais, naturais e comerciais — criando uma progressão intencional de segurança para perigo, do familiar para o sombrio.

O design da rota não é aleatório: cada trecho existe por razões narrativas. O segmento mais perigoso do circuito — entre Ironmill Industrial e Darkwood Park — passa a poucos metros da **Ravenswood Gorge Road**, exato local onde Victor Graves dirigiu o Ônibus 104 para um precipício em 13 de março de 1998.

### 1.2 Dados Gerais

| Atributo | Valor |
|---|---|
| Comprimento total do circuito | 9,2 km |
| Tempo de circuito completo (dia) | ~7 minutos reais |
| Tempo de circuito completo (noite) | ~8–9 minutos reais (neblina reduz velocidade) |
| Número de paradas | 5 (incluindo escola) |
| Número de segmentos | 5 |
| Velocidade média do ônibus | ~50 km/h (zonas comerciais/industriais) / ~30 km/h (zonas residenciais) |
| Velocidade máxima permitida sem penalidade | 60 km/h |
| Tempo real por período | 7–10 minutos |
| Dias de gameplay | 5 dias × 2 períodos = 10 sessões |

### 1.3 Visão Aérea da Rota (Diagrama Textual)

```
        [2] MAPLE HEIGHTS
         ↑           ↓
[1] ESCOLA ←→→→→→→→→→→
         ↑           ↓
         ↑      [3] IRONMILL
         ↑           ↓
[5] TOWN CENTER    [4] DARKWOOD PARK
         ↑           ↓
         ←←←←←←←←←←←←
```

**Sentido único:** A rota percorre sempre no mesmo sentido circular.
`Escola → Maple Heights → Ironmill → Darkwood Park → Town Center → Escola`

### 1.4 Contexto Narrativo da Rota

Em 13 de março de 1998, o motorista **Victor Graves** desviou desta mesma rota no Segmento 3 (Ironmill → Darkwood Park), tomou a **Ravenswood Gorge Road** — um desvio não autorizado — e acelerou o Ônibus 104 em direção ao precipício. Das 5 crianças a bordo naquele dia, nenhuma sobreviveu. O corpo de Victor jamais foi encontrado.

Hoje, 26 anos depois, o novo motorista **Ray Morgan** percorre a mesma rota — sem saber disso.

---

## 2. Os 5 Pontos de Parada

### Parada 1 — Ravenswood Elementary School *(Base Operacional)*

**Endereço fictício:** 400 Hollowbrook Drive, Ravenswood, PA 18300
**Tipo:** Escola pública / ponto de partida e chegada
**Posição na rota:** Início e fim do circuito (Km 0 / Km 9,2)

#### Ambiente

A escola é um edifício de tijolos vermelho-escuros de dois andares, construído nos anos 1960, com janelas largas e um estacionamento de asfalto trincado. Um mastro com bandeira americana ocupa o centro da entrada. O estacionamento lateral acomoda o Ônibus 104 e dois outros veículos escolares.

Ao redor: calçada larga, faixas de pedestres desgastadas, arbustos podados de forma geométrica e um painel de avisos externo com cartazes que mudam a cada dia (elementos de lore). O escritório do Diretor Harrison Siqueira Stone fica no segundo andar — visível da cabine do motorista quando o ônibus para na posição de embarque.

#### Crianças por Período

| Período | Ação | Quantidade |
|---|---|---|
| Manhã (chegada) | Desembarcam na escola | 10 crianças |
| Tarde/Noite (partida) | Embarcam para ir para casa | 10 crianças |

#### Importância Narrativa

- **Dia 1 (Manhã):** Diretor Stone aparece na entrada, acena brevemente para o motorista. Primeira e única interação direta durante o início do jogo.
- **Dia 2:** Cartaz no painel externo menciona "aniversário de formatura" sem especificar o ano — referência velada ao ano de 1998.
- **Dia 4:** Luz do escritório de Stone permanece acesa mesmo durante a noite. Sombra se move na janela.
- **Dia 5:** A bandeira no mastro está rasgada e parcialmente caída. Ninguém a corrigiu.

#### Horários de Chegada/Partida

| Período | Chegada | Partida |
|---|---|---|
| Manhã | 07:30 (destino final) | 07:15 (partida da base) |
| Tarde/Noite | 15:30 (partida da base) | ~15:45 (retorno após rota completa) |

#### Peculiaridades Visuais: Dia vs. Noite

| Elemento | Manhã | Tarde/Noite |
|---|---|---|
| Iluminação | Luz natural suave, céu nublado característico de PA | Postes de sódio laranja-amarelados, sombras longas |
| Pátio | Movimentado, crianças e pais | Silencioso, um ou dois funcionários |
| Escritório de Stone | Janela iluminada (trabalho matinal) | Janela escura (Dias 1–3) / iluminada (Dias 4–5) |
| Painel de avisos | Legível, cores normais | Ilegível na escuridão — textos distorcidos ao usar farol |

---

### Parada 2 — Maple Heights Residential *(O Bairro Tranquilo)*

**Endereço fictício:** Junção de Maple Ridge Road com Elmwood Avenue, Ravenswood, PA 18302
**Tipo:** Bairro residencial de classe média
**Posição na rota:** Km 2,1 (Segmento 1)

#### Ambiente

Rua residencial com casas de madeira pintadas em tons de bege, azul-claro e branco. Calçadas limpas com árvores bordo (*maple*) que no outono ficam vermelho-alaranjadas — criando um contraste visual quente com a paleta fria do restante do jogo. Caixas de correio numeradas, cercas de madeira branca, carros nas garagens.

O ponto de ônibus é um abrigo simples de metal e vidro com banco de concreto. Do lado oposto da rua há uma casa com janela sempre iluminada (Dias 1–3) que escurece abruptamente no Dia 4.

#### Crianças por Período

| Período | Ação | Quantidade |
|---|---|---|
| Manhã | Embarcam no ponto | 3 crianças |
| Tarde/Noite | Desembarcam no ponto | 3 crianças |

#### Importância Narrativa

- **Personagem associado:** **Grace Harper** (Observadora) morava em Maple Heights. A casa do número 14 da Elmwood Avenue (visível ao parar) é onde ela vivia.
- **Dia 3:** Uma criança viva de Maple Heights menciona casualmente que "aquela casa ali ficou vazia há muito tempo" — sem saber que estava falando da casa de Grace.
- **Dia 5 (Noite):** A janela da casa de Grace acende por exatamente 3 segundos ao o ônibus passar, depois apaga.

#### Horários de Chegada

| Período | Chegada | Permanência |
|---|---|---|
| Manhã | 07:18 | ~25 segundos (embarque) |
| Tarde/Noite | 15:33 | ~25 segundos (desembarque) |

#### Peculiaridades Visuais: Dia vs. Noite

| Elemento | Manhã | Tarde/Noite |
|---|---|---|
| Árvores bordo | Coloridas, suaves | Silhuetas negras e ramificadas, sem movimento de folhas |
| Calçadas | Crianças caminhando, pais esperando | Vazias; apenas a luz branca dos postes |
| Casa de Grace | Fachada normal, jardim simples | Escura (Dias 1–3); luz acende 3s no Dia 5 |
| Abrigo do ponto | Vidro translúcido, anúncio colorido | Vidro sujo reflete o farol, anúncio irreconhecível |

---

### Parada 3 — Ironmill Industrial *(A Zona Sombria)*

**Endereço fictício:** 890 Ironmill Boulevard, esquina com Factory Row, Ravenswood, PA 18305
**Tipo:** Área industrial/fabril semi-abandonada
**Posição na rota:** Km 4,5 (Segmento 2)

#### Ambiente

A parada fica na frente do portão de uma antiga fundição de ferro — a **Ravenswood Ironmill Co.**, fundada em 1923, parcialmente ativa. O edifício principal é cinza escuro com janelas gradeadas, chaminés que às vezes soltam vapor (ou fumaça — o jogador não tem como saber), e uma cerca de arame com concertina parcialmente derrubada.

O ponto de ônibus aqui não tem abrigo — apenas um poste de metal com a placa "BUS STOP 3" parcialmente enferrujada. A calçada é estreita e rachada, com ervas daninhas nas fissuras. Ao fundo, um depósito abandonado com janelas tapadas.

#### Crianças por Período

| Período | Ação | Quantidade |
|---|---|---|
| Manhã | Embarcam no ponto | 2 crianças |
| Tarde/Noite | Desembarcam no ponto | 2 crianças |

#### Importância Narrativa

- **Personagem associado:** **Marcus Reid** (Explorador) frequentava a área industrial da Ironmill após o horário escolar. Ele adentrava a fábrica abandonada pelo setor sul, ignorando as placas de "ENTRADA PROIBIDA".
- **Dia 2:** Ao chegar neste ponto, o rádio do ônibus emite estática breve. Dura menos de 1 segundo — parece falha técnica.
- **Dia 3:** Uma janela gradeada da fábrica está destrancada e aberta, balançando levemente. Não estava assim no Dia 1.
- **Dia 4 (Noite):** Uma silhueta infantil visível por meio segundo na janela do depósito abandonado. Desaparece imediatamente. O retrovisor não a captura.
- **Easter Egg:** No depósito, na parede visível pela janela aberta (Dia 3+), há uma sequência de números rabiscados: `13 03 1998`. Apenas visível com o farol ligado.

#### Horários de Chegada

| Período | Chegada | Permanência |
|---|---|---|
| Manhã | 07:22 | ~20 segundos |
| Tarde/Noite | 15:37 | ~20 segundos |

#### Peculiaridades Visuais: Dia vs. Noite

| Elemento | Manhã | Tarde/Noite |
|---|---|---|
| Chaminés | Vapor branco suave, luz difusa | Vapor/fumaça alaranjada pelo reflexo dos postes; parece fogo |
| Portão da fábrica | Fechado, correntes visíveis | Correntes parecem mais frouxas; luz interna vaza pelas frestas |
| Depósito abandonado | Janelas escuras, nada de anormal | Silhuetas de objetos visíveis; no Dia 4, silhueta de criança |
| Iluminação geral | Cinza industrial, funcional | Único poste com luz âmbar fraca; sombras longas e irregulares |

---

### Parada 4 — Darkwood Park *(O Parque do Lago)*

**Endereço fictício:** Entrada Norte do Darkwood Park, Gorge Road Junction, Ravenswood, PA 18307
**Tipo:** Parque público com lago — área natural perturbadora
**Posição na rota:** Km 6,3 (Segmento 3)

#### Ambiente

O ponto fica na entrada norte do **Darkwood Park**, um parque municipal construído às margens do **Blackmere Lake** — um lago de águas escuras que raramente reflete o céu com clareza. A vegetação é densa: pinheiros altos e juníperos formam barreiras naturais que bloqueiam a visão para além das primeiras fileiras de árvores.

A entrada do parque tem um arco de metal enferrujado com o nome "DARKWOOD PARK" em letras que perderam a tinta. Uma placa de madeira ao lado lista atividades: pesca, trilha, área de piquenique. A placa está visivelmente mal conservada.

**Detalhe crítico:** A apenas 800 metros deste ponto, a estrada se bifurca. A direita leva para dentro do parque; a esquerda, para a **Ravenswood Gorge Road** — o trecho onde ocorreu o acidente de 1998. O jogador nunca toma essa bifurcação, mas o mapa (TAB) mostra as duas opções. Um sinal de trânsito com "GORGE ROAD" está visível na bifurcação durante a noite.

#### Crianças por Período

| Período | Ação | Quantidade |
|---|---|---|
| Manhã | Embarcam no ponto | 2 crianças |
| Tarde/Noite | Desembarcam no ponto | 2 crianças |

#### Importância Narrativa

- **Personagem associado principal:** **Thomas Sanders** (Narrador) costumava contar histórias para grupos de crianças na beira do Blackmere Lake. Seus "contos às margens" eram famosos — e temidos — entre os alunos da Ravenswood Elementary.
- **Personagem associado secundário:** **Oliver Crane** (Artista) desenhava o lago com frequência. Registros de seus desenhos aparecem como easter eggs.
- **Conexão direta com o acidente:** Este é o ponto da rota mais próximo do precipício. No Dia 3+, o jogador que usar o mapa (TAB) neste trecho verá uma marcação não-oficial no mapa: um "X" vermelho na Ravenswood Gorge Road, sem legenda.
- **Dia 4:** Uma crianças viva menciona ao desembarcar: *"Minha mãe diz pra não olhar pro lago depois das 5 da tarde."*
- **Dia 5 (Noite):** O lago reflete uma luz que não existe — como se houvesse luar quando o céu está fechado.

#### Horários de Chegada

| Período | Chegada | Permanência |
|---|---|---|
| Manhã | 07:25 | ~25 segundos |
| Tarde/Noite | 15:40 | ~25 segundos |

#### Peculiaridades Visuais: Dia vs. Noite

| Elemento | Manhã | Tarde/Noite |
|---|---|---|
| Lago (visível pela entrada) | Água escura, estática, sem reflexo claro | Reflexos irregulares sem fonte de luz explicável |
| Arco de entrada | Enferrujado mas legível | Na sombra, as letras "DARKWOOD" ficam com partes apagadas: "D_RKW__D" |
| Vegetação | Densa, verde-escura, estática | Ramos se movem sem vento (Dias 3–5) |
| Placa da Gorge Road | Visível, normal | Iluminada pelo farol: texto parece tremer levemente |
| Temperatura percebida | Normal (névoa matinal leve) | Névoa pesada que sobe do solo nos Dias 4–5 |

---

### Parada 5 — Ravenswood Town Center *(O Centro Comercial)*

**Endereço fictício:** 120 Central Avenue, esquina com Merchant Street, Ravenswood, PA 18301
**Tipo:** Centro comercial urbano da cidade
**Posição na rota:** Km 7,8 (Segmento 4)

#### Ambiente

O único ponto verdadeiramente urbano da rota. A Central Avenue tem lojas térreas: uma farmácia, uma banca de jornal, uma loja de conveniência aberta 24h, uma lanchonete familiar (*Patty's Diner*) e uma livraria usada. O calçadão é de pedra irregular, com bancos de parque espalhados.

O ponto de ônibus aqui é o mais bem conservado da rota: abrigo coberto com banco, mapa do transporte público afixado (que o jogador pode ler brevemente quando abre o mapa TAB), e um painel digital que mostra horários — o único elemento visivelmente moderno em Ravenswood.

A *Patty's Diner* é o coração social informal da cidade. Seu letreiro neon vermelho e branco é visível de longe à noite.

#### Crianças por Período

| Período | Ação | Quantidade |
|---|---|---|
| Manhã | Embarcam no ponto | 3 crianças |
| Tarde/Noite | Desembarcam no ponto | 3 crianças |

#### Importância Narrativa

- **Personagem associado:** **Emma Lynch** (Burladora) fazia suas pegadinhas nas lojas e calçadões do Town Center. A loja de conveniência da esquina tem uma janela com um antigo aviso manuscrito: *"Proibida a entrada de menores desacompanhados"* — colocado por causa de Emma, segundo a história oral da cidade.
- **Dia 2:** A banca de jornal exibe uma manchete velha enquadrada na vitrine: *"RAVENSWOOD MOURNS: 5 CHILDREN LOST IN BUS ACCIDENT — March 1998"*. Legível apenas com o farol ligado à noite.
- **Dia 3:** A *Patty's Diner* tem uma mesa ocupada visível pela janela — duas figuras sentadas. Ao passar rápido, parecem crianças com roupas antigas.
- **Dia 5:** O painel digital de horários do ponto exibe `13/03/1998` no lugar do horário atual. Se o jogador usar TAB (mapa) neste momento, o mapa mostra uma rota alternativa que não existe — passando pela Gorge Road.

#### Horários de Chegada

| Período | Chegada | Permanência |
|---|---|---|
| Manhã | 07:28 | ~30 segundos |
| Tarde/Noite | 15:43 | ~30 segundos |

#### Peculiaridades Visuais: Dia vs. Noite

| Elemento | Manhã | Tarde/Noite |
|---|---|---|
| Letreiro Patty's Diner | Apagado (almoço só abre às 11h) | Neon vermelho/branco pulsante — única cor quente da rota |
| Lojas | Abertas, movimento normal | Fechadas; grades metálicas abaixadas |
| Painel digital do ponto | Exibe horários corretos | Exibe horários corretos (Dias 1–4) / `13/03/1998` (Dia 5) |
| Calçadão | 2–3 pedestres visíveis | Completamente vazio exceto por sombras que não têm origem |
| Banca de jornal | Virada para o interior, invisível | Manchete de 1998 visível com farol ativo |

---

## 3. Segmentos da Rota

### Visão Geral dos Segmentos

| # | De → Para | Distância | Tempo (Dia) | Tempo (Noite) |
|---|---|---|---|---|
| S1 | Escola → Maple Heights | 2,1 km | ~1m 45s | ~2m 10s |
| S2 | Maple Heights → Ironmill | 2,4 km | ~2m 00s | ~2m 30s |
| S3 | Ironmill → Darkwood Park | 1,8 km | ~1m 30s | ~2m 00s |
| S4 | Darkwood Park → Town Center | 1,5 km | ~1m 10s | ~1m 20s |
| S5 | Town Center → Escola | 1,4 km | ~1m 00s | ~1m 10s |
| **Total** | **Circuito Completo** | **9,2 km** | **~7m 25s** | **~9m 10s** |

---

### Segmento 1 — Escola → Maple Heights

**Terreno:** Plano. Rua de asfalto em bom estado. Duas faixas no mesmo sentido.
**Curvatura:** Baixa. Uma curva suave à esquerda no Km 0,9 (saída do bairro escolar).

**Elementos Visuais Chave:**
- Início: portão de saída da escola, Grade de ferro cinza
- Meio: zona residencial densa, casas menores, calçadas arborizadas
- Fim: Junção de Maple Ridge Road — ponto de maior visibilidade da rota (sem obstáculos nas laterais)

**Perigos Noturnos:**
- Grace Harper pode aparecer no retrovisor no Segmento 1 a partir do Dia 2
- Céu fechado reduz visibilidade a ~25 metros sem farol ativo
- Uma rua transversal sem sinal (Elm Cross) cruza no Km 0,7 — sem obstáculo de gameplay, mas pode surpreender o jogador durante o dia

**Intensidade Narrativa:** ⚪⚪⚪ (Baixa) — Zona de conforto intencional

---

### Segmento 2 — Maple Heights → Ironmill

**Terreno:** Plano com leve aclive de 3% no final (subida industrial). Asfalto deteriorado após Km 3,2.
**Curvatura:** Média. Uma curva fechada à direita no Km 3,5 (entrada na zona industrial) exige redução de velocidade.

**Elementos Visuais Chave:**
- Km 2,1–3,0: Transição residencial-industrial. Casas dão lugar a galpões e muros de bloco de concreto
- Km 3,0–4,5: Zona industrial. Postes mais espaçados, iluminação pior. Calçadas sem abrigo
- Detalhe: trilhos de trem atravessam a via no Km 4,0 (sem barreira ativa — apenas cruzamento nivelado)

**Perigos Noturnos:**
- Cruzamento de trilhos no Km 4,0: sem sinalização de trem em gameplay, mas som distante de apito pode ser ouvido à noite (Marcus pode estar associado)
- Postes de luz mais espaçados = necessidade de usar farol com mais frequência
- Marcus Reid começa a se manifestar neste segmento a partir do Dia 2 (troca de lugar no interior do ônibus)

**Intensidade Narrativa:** 🟡🟡⚪ (Média) — Aumento gradual de tensão

---

### Segmento 3 — Ironmill → Darkwood Park *(Segmento Crítico)*

**Terreno:** Descida moderada de 6–8%. Estrada de montanha. Asfalto com marcas de desgaste nas bordas. Sem acostamento nos primeiros 800 metros.
**Curvatura:** Alta. Três curvas fechadas em sequência (Km 4,8, 5,1, 5,5). A segunda curva (Km 5,1) é a mais perigosa — limite de velocidade 25 km/h.

**Elementos Visuais Chave:**
- Km 4,5–5,0: Vegetação fecha sobre a via. Dossel de pinheiros reduce luz natural mesmo de manhã
- Km 5,1: Bifurcação com sinalização. **GORGE ROAD** (esquerda, bloqueada por correntes) / **DARKWOOD PARK** (direita). Rota oficial segue à direita
- Km 5,1 (Gorge Road): Correntes de aço fecham a entrada. Um sinal vermelho: *"ROAD CLOSED — DANGER OF FALLING — RAVENSWOOD COUNTY"*
- Km 5,5–6,3: Estrada paralela à borda do parque. Lago ocasionalmente visível entre as árvores

**Perigos Noturnos:**
- **Este é o segmento de maior tensão da rota a partir do Dia 3**
- A bifurcação da Gorge Road é invisível sem farol ativo à noite
- Dia 4+: as correntes da Gorge Road aparecem abertas na primeira vez que o jogador passa (fecham normalmente quando o ônibus para na Parada 4 e retorna)
- Thomas Sanders pode sussurrar durante este segmento — sua voz é mais audível aqui do que em qualquer outro trecho
- Neblina progressiva por dia: Dia 1 (0%), Dia 2 (15%), Dia 3 (30%), Dia 4 (50%), Dia 5 (70%)

**Intensidade Narrativa:** 🔴🔴🔴 (Máxima) — Coração emocional do jogo

---

### Segmento 4 — Darkwood Park → Town Center

**Terreno:** Descida suave de 3%. Retorno à área urbana. Asfalto em bom estado.
**Curvatura:** Baixa. Uma curva aberta no Km 7,0 (retorno à malha urbana).

**Elementos Visuais Chave:**
- Km 6,3–7,0: Transição parque-urbano. A vegetação do parque ainda beira a via por ~500m
- Km 7,0: Primeiro semáforo da rota (único semáforo — sempre verde durante o dia; no Dia 5 à noite, pisca âmbar)
- Km 7,8: Town Center, letreiro da *Patty's Diner* visível

**Perigos Noturnos:**
- Emma Lynch frequentemente se manifesta neste segmento (risadas nos Dias 3–5)
- Semáforo em âmbar no Dia 5 cria dúvida: parar ou passar?

**Intensidade Narrativa:** 🟠🟠⚪ (Média-Alta) — Descida de tensão mas presença crescente de Emma

---

### Segmento 5 — Town Center → Escola

**Terreno:** Plano. Via urbana principal. Duas faixas. Bem iluminado (comparado ao resto da rota).
**Curvatura:** Muito baixa. Reta de quase 1 km com uma única curva suave no final.

**Elementos Visuais Chave:**
- Km 7,8–8,5: Central Avenue. Letreiros apagados das lojas fechadas à noite
- Km 8,5–9,2: Zona de transição urbana-escolar. A escola aparece ao fundo no horizonte
- Chegada: Portão da escola. Luz do escritório de Stone (variável por dia)

**Perigos Noturnos:**
- Oliver Crane pode deixar desenhos no vidro traseiro visíveis neste último trecho
- Final do período: a tensão acumulada no ESPAÇO torna qualquer estímulo mais impactante
- Dia 5: na última chegada, a placa "RAVENSWOOD ELEMENTARY SCHOOL" tem uma segunda linha abaixo (ilegível) que claramente não é oficial

**Intensidade Narrativa:** 🟡⚪⚪ (Baixa-Média) — Falsa segurança de "já cheguei"

---

## 4. Sistema de Waypoints

### 4.1 Funcionamento Técnico

A rota do Ônibus 104 é controlada por um sistema de **waypoints invisíveis** posicionados ao longo do asfalto. O sistema funciona em duas camadas:

**Camada de Guia (GuideWaypoints):**
Pontos a cada **50 metros** ao longo da rota oficial. Eles não geram feedback direto ao jogador, mas são usados pelo `RouteManager.cs` para:
- Calcular progresso percentual da rota
- Determinar velocidade recomendada em curvas (dados passados ao `BusController.cs`)
- Disparar eventos de ambiente (neblina, som, manifestações)

**Camada de Controle (ControlWaypoints):**
Pontos críticos com tolerância e penalidade definidas. São os pontos que o jogador *deve* passar.

### 4.2 Tabela de Waypoints Críticos

| ID | Localização | Km | Tolerância Lateral | Ação se Desviado |
|---|---|---|---|---|
| WP-01 | Saída da escola | 0,1 | ±4,0 m | Aviso sonoro suave |
| WP-02 | Curva Elm Cross | 0,7 | ±3,0 m | Aviso sonoro |
| WP-03 | Entrada Maple Heights | 2,0 | ±2,5 m | Aviso + tensão +2% |
| WP-04 | Curva industrial | 3,5 | ±2,0 m | Aviso + tensão +3% |
| WP-05 | Cruzamento de trilhos | 4,0 | ±3,5 m | Aviso sonoro |
| WP-06 | **Bifurcação Gorge Road** | 5,1 | ±1,5 m | **Tensão +10% / Game Over se seguir Gorge Road** |
| WP-07 | Curva do parque | 5,5 | ±2,0 m | Aviso + tensão +3% |
| WP-08 | Semáforo Town Center | 7,0 | ±4,0 m | Sem penalidade |
| WP-09 | Entrada escola (chegada) | 9,1 | ±2,5 m | Período concluído |

### 4.3 Tolerâncias de Desvio

| Tipo de Desvio | Tolerância | Consequência |
|---|---|---|
| Desvio lateral leve | < 2 m da via | Sem penalidade |
| Desvio lateral moderado | 2–4 m da via | Aviso sonoro (1 beep) |
| Desvio lateral grave | > 4 m da via | Tensão +5% + reset posicional |
| Seguir Gorge Road (WP-06) | Qualquer distância | Tensão +10% imediato; após 200m → **Game Over narrativo** |
| Velocidade excessiva em curva | > 60 km/h | Risco de colisão lateral |

### 4.4 Penalidades e Recompensas de Tempo

| Situação | Efeito na Tensão | Efeito no Tempo |
|---|---|---|
| Chegar a parada dentro do horário (±30s) | -1% tensão | Neutro |
| Chegar com atraso (31–60s) | +2% tensão | Aviso de Oliver Crane ativado |
| Chegar com atraso grave (>60s) | +5% tensão | Oliver Crane manifesta-se imediatamente |
| Completar circuito com tempo sobrando | -3% tensão carregada para próximo período | Bônus narrativo (fragmento de lore) |
| Bater em obstáculo (poste, cerca) | +4% tensão | Atraso de 10–15s |
| Bater com Grace Harper bloqueando visão | +8% tensão | Atraso de 20s + penalidade especial |

---

## 5. Design Dia vs. Noite

### 5.1 Tabela Comparativa por Segmento

| Segmento | Elemento | Manhã | Tarde/Noite |
|---|---|---|---|
| **S1 Escola→Maple** | Iluminação | Luz natural difusa (céu cinza PA) | Postes de sódio; sombras amareladas |
| | Visibilidade | ~80m à frente | ~30m sem farol / ~55m com farol |
| | Fantasmas | Grace: inativa | Grace: retrovisor a partir Dia 2 |
| | Neblina | Leve (10% opacidade) | Moderada a densa (30–70% por dia) |
| **S2 Maple→Ironmill** | Iluminação | Industrial, cinza | Postes espaçados; zonas de escuridão total |
| | Visibilidade | ~70m | ~20m sem farol / ~45m com farol |
| | Fantasmas | Marcus: inativo | Marcus: começa troca de lugares (Dia 2+) |
| | Ambiente sonoro | Motor + vento leve | Motor + metal se contraindo + chiado fraco |
| **S3 Ironmill→Darkwood** | Iluminação | Dossel filtra luz; semi-sombra mesmo de manhã | Quase completa escuridão entre postes |
| | Visibilidade | ~60m | ~15m sem farol / ~40m com farol |
| | Fantasmas | Thomas: sussurros inaudíveis | Thomas: sussurros audíveis (Dia 2+); intensidade por dia |
| | Gorge Road | Correntes visíveis, fechadas | Correntes podem aparecer abertas (Dia 4+) |
| | Neblina | 15–25% | 40–70% dependendo do dia |
| **S4 Darkwood→Town** | Iluminação | Transição escuro → luz urbana | Letreiro Patty's Diner; contraste dramático |
| | Visibilidade | ~70m | ~35m sem farol / ~55m com farol |
| | Fantasmas | Emma: inativa | Emma: risadas (Dia 3+) |
| | Temperatura | Normal | Queda visual: vapor de boca do motorista (Dia 4+) |
| **S5 Town→Escola** | Iluminação | Mais iluminado da rota | Razoavelmente iluminado — "falsa segurança" |
| | Visibilidade | ~85m | ~45m |
| | Fantasmas | Oliver: inativo | Oliver: desenhos no vidro (Dia 3+) |
| | Escola ao fundo | Visível, normal | Escritório de Stone: escuro (Dias 1–3) / iluminado (Dias 4–5) |

### 5.2 Progressão de Intensidade por Dia

| Dia | Período | Neblina Máxima | Fantasmas Ativos | Tensão Inicial |
|---|---|---|---|---|
| 1 | Manhã | 10% | Nenhum | 5% |
| 1 | Tarde/Noite | 25% | Grace (passiva) | 10% |
| 2 | Manhã | 15% | Grace, Marcus (leves) | 10% |
| 2 | Tarde/Noite | 35% | Grace, Marcus, Thomas | 15% |
| 3 | Manhã | 20% | Todos (moderados) | 15% |
| 3 | Tarde/Noite | 50% | Todos (ativos) | 20% |
| 4 | Manhã | 25% | Todos (ativos) | 25% |
| 4 | Tarde/Noite | 65% | Todos (agressivos) | 30% |
| 5 | Manhã | 30% | Todos (máximo) | 30% |
| 5 | Tarde/Noite | 70% | Todos (confronto final) | 35% |

---

## 6. Elementos Narrativos no Ambiente

### 6.1 Story Objects por Dia

Objetos e eventos ambientais que aparecem progressivamente, construindo o lore sem cutscenes:

| Dia | Local | Story Object | Descrição |
|---|---|---|---|
| 1 | Escola | Cartaz no painel externo | "RAVENSWOOD ELEMENTARY — HONOR STUDENTS 1997–98" — nome das 5 crianças listados |
| 1 | S3 (bifurcação) | Sinal "GORGE ROAD" | Primeira vez que o jogador vê a referência ao local do acidente |
| 2 | Town Center | Manchete enquadrada (banca) | Titular do acidente de 1998 (legível apenas com farol à noite) |
| 2 | Ironmill | Janela destrancada no depósito | Sugere que alguém (ou algo) entrou recentemente |
| 3 | Darkwood Park | "X" vermelho no mapa (TAB) | Marca não-oficial sobre a Gorge Road; sem legenda |
| 3 | Patty's Diner (S4) | Figuras na janela | Silhuetas de crianças com roupas antigas — 2 figuras |
| 4 | S3 (Gorge Road) | Correntes abertas | As correntes que bloqueiam a Gorge Road aparecem soltas |
| 4 | Ironmill | Números na parede (farol) | "13 03 1998" rabiscado no depósito abandonado |
| 4 | Ônibus (banco motorista) | Canto do diário | Uma quina de um caderninho surge sob o banco do motorista — interação: TAB revela data |
| 5 | Ônibus | Diário de Victor Graves | Completo — revelação da intenção de Victor e última entrada |
| 5 | Escola (chegada) | Placa com texto extra | Segunda linha ilegível sob o nome da escola |
| 5 | Town Center | Painel digital | Exibe `13/03/1998` no lugar do horário |

### 6.2 Conexões Personagem × Local

| Personagem | Local Primário | Local Secundário | Como se Manifesta no Local |
|---|---|---|---|
| **Marcus Reid** | Ironmill Industrial (S2) | S3 (entrada do parque) | Silhueta na janela; estática no rádio na parada 3 |
| **Emma Lynch** | Town Center (S4–S5) | Interior do ônibus (painel) | Risadas; janela da loja de conveniência com aviso antigo |
| **Thomas Sanders** | Darkwood Park (S3) | S3 inteiro | Sussurros aumentam em intensidade ao longo do segmento mais perigoso |
| **Grace Harper** | Maple Heights (S1) | Retrovisor (todos os segmentos) | Casa na Elmwood Avenue; luz na janela no Dia 5 |
| **Oliver Crane** | Darkwood Park / S5 | Interior do ônibus | Desenhos aparecem no vidro traseiro; rabiscos na parede do depósito |
| **Victor Graves** | Bifurcação Gorge Road | Banco do motorista | Diário (Dia 4–5); pegadas de neblina densa no Dia 5 |
| **Harrison Stone** | Escola (escritório) | Invisível fora da escola | Luz do escritório como indicador de estado do lore |

### 6.3 Easter Eggs de Lore

| Easter Egg | Local | Como Descobrir | Conteúdo |
|---|---|---|---|
| **"Honor Students 1997–98"** | Painel escolar | Observar antes de partir no Dia 1 | Lista os 5 nomes das crianças — Marcus, Emma, Thomas, Grace, Oliver |
| **Manchete de 1998** | Banca, Town Center | Usar farol voltado para a banca à noite, Dia 2+ | *"RAVENSWOOD MOURNS: 5 CHILDREN LOST IN BUS ACCIDENT"* |
| **`13 03 1998` na parede** | Depósito Ironmill | Usar farol com janela aberta, Dia 3 | Números que revelam a data do acidente |
| **"X" vermelho no mapa** | Mapa (TAB), S3 | Abrir mapa durante o Segmento 3, Dia 3+ | Localização exata do precipício — sem texto |
| **Aviso da loja** | Town Center | Parar próximo à loja e usar farol, qualquer noite | *"Proibida a entrada de menores desacompanhados"* — alusão a Emma |
| **Lago refletindo luz falsa** | Darkwood Park | Observar o lago ao parar na Parada 4, noite Dia 5 | Reflexo de luz que não tem origem — Grace ou Thomas? |
| **Última entrada do diário** | Banco do motorista | Encontrar diário (Dia 4) e ler completamente (Dia 5) | *"Amanhã é o último dia. Ou deles, ou meu. Não importa mais qual dos dois. Perdoem-me."* |
| **Painel `13/03/1998`** | Town Center, Dia 5 noite | Parar na Parada 5 no período final | Substitui horários por data do acidente |

---

## 7. Gray Box — Protótipo Mínimo *(Issue #10)*

### 7.1 Escopo do Protótipo

O gray box tem objetivo único: **validar o loop de gameplay da rota** — dirigir, parar, embarcar/desembarcar, perceber tensão. Narrativa e horror ficam para fora nesta fase.

**[AUTO-DECISION]** Quantos pontos incluir no gray box? → **3 pontos** (Escola + Maple Heights + Ironmill), cobrindo 4,5 km e os dois primeiros segmentos. Razão: cobre o loop mínimo de partida + 2 paradas + retorno à escola sem entrar na zona de alta complexidade (S3).

### 7.2 O Que Construir Primeiro

| Prioridade | Elemento | Justificativa |
|---|---|---|
| 🔴 P1 | Geometria da via (S1 + S2 apenas) | Sem rota, nada funciona |
| 🔴 P1 | Ônibus funcional (WASD + física básica) | Core loop |
| 🔴 P1 | Parada 1 (Escola) — gray box | Ponto de partida e chegada |
| 🔴 P1 | Parada 2 (Maple Heights) — gray box | Primeira parada real |
| 🔴 P1 | Parada 3 (Ironmill) — gray box | Segunda parada; testa S2 |
| 🟡 P2 | Sistema de waypoints (WP-01 a WP-05) | Valida aderência à rota |
| 🟡 P2 | Sistema de horário básico (relógio SPACE) | Valida mechânica de tempo |
| 🟡 P2 | HUD mínimo: velocidade + tensão percentual | Feedback básico |
| 🟡 P2 | 5 crianças vivas (placeholder cubo) | Valida embarque/desembarque com tecla T |
| 🟢 P3 | Skybox (dia/noite) | Contexto visual mínimo |
| 🟢 P3 | Iluminação básica por período | Distingue manhã de noite visualmente |

### 7.3 Assets Mínimos Necessários

| Asset | Tipo | Nível de Fidelidade no Gray Box |
|---|---|---|
| Ônibus 104 | Mesh + collider | Cubo alongado com câmera 1ª pessoa na posição do motorista |
| Via S1+S2 | Plane subdividido | Plano cinza com bordas marcadas em branco |
| Escola | Cubo simples | Cubo 4x2x3 com label "ESCOLA" |
| Maple Heights (ponto) | Cubo pequeno | Cubo 0,5x1x0,5 com label "PARADA 2" |
| Ironmill (ponto) | Cubo pequeno | Cubo 0,5x1x0,5 com label "PARADA 3" |
| Crianças vivas | Cápsulas | 5 cápsulas amarelas por ponto de parada |
| Waypoints | Invisible colliders | Colliders simples — sem mesh visível |
| Skybox | Default Unity | Skybox nativo, período switcha para escuro |

### 7.4 Critérios de Aceite do Protótipo

Para o protótipo ser aprovado e avançar para a fase de arte Low Poly, **todos os critérios abaixo** devem ser atendidos:

| Critério | Como Testar | Condição de Aprovação |
|---|---|---|
| **Loop completo** | Iniciar no ponto 1, ir ao 2, ir ao 3, voltar ao 1 | Completar sem travar nem cair pela geometria |
| **Embarque/desembarque** | Chegar a cada parada e pressionar T | Crianças entram e saem corretamente |
| **Sistema de horário** | Pressionar SPACE em qualquer ponto | Relógio exibe horário e % tensão corretos |
| **Mapa (TAB)** | Pressionar TAB enquanto dirige | Mapa abre, bloqueia parte da visão, fecha com TAB |
| **Waypoints ativos** | Desviar 5m da via em WP-03 | Aviso sonoro dispara; tensão sobe 2% |
| **Ciclo dia/noite** | Completar período manhã e tarde | Visual muda entre os dois períodos |
| **Tensão persistente** | Acumular tensão e iniciar novo período | Parte do valor da tensão é carregado ao próximo período |
| **Performance** | Medir FPS na cena gray box | ≥60 FPS estável em hardware alvo (GTX 1060 equivalente) |

### 7.5 O Que Fica Fora do Protótipo (Explicitamente)

- Nenhum fantasma ativo
- Nenhum story object ou easter egg
- Nenhuma narrativa, diálogo ou cutscene
- Segmentos S3, S4, S5 (Darkwood, Town Center, retorno completo)
- Iluminação avançada, neblina volumétrica, shaders Low Poly finais
- Sistema de game over
- Sons ambientes avançados

---

## 8. Referências e Notas de Desenvolvimento

### 8.1 Documentos Relacionados

| Documento | Localização | Relevância |
|---|---|---|
| GDD Principal | `game/docs/GDD.md` | Visão macro do jogo |
| Personagens | `game/docs/narrative/characters.md` | Lore de todos os personagens |
| Mecânicas | `game/docs/design/mechanics.md` | Controles detalhados |
| Balanceamento | `game/docs/design/balancing.md` | Tabela de tensão por dia |

### 8.2 Decisões de Design Documentadas

| Decisão | Escolha | Razão |
|---|---|---|
| Rota circular vs. linear | **Circular** | Cria senso de ciclo que espelha o ciclo narrativo da maldição |
| Sentido único vs. bidirecional | **Sentido único** | Reduz complexidade de waypoints; manhã e tarde seguem mesma direção |
| 5 paradas vs. mais | **5 paradas** | Uma por personagem fantasma — cada parada "pertence" a uma criança |
| Gorge Road: jogável vs. barricada | **Barricada (com abertura narrativa)** | Game Over se percorrida, mas correntes abertas no Dia 4 cria tensão sem forçar o desvio |
| Neblina por dia vs. por período | **Por dia** (com modificador de período) | Progressão mais legível para o jogador |

### 8.3 Questões em Aberto

- [ ] **Velocidade do ônibus**: 50 km/h como média produz 7 min de rota. Validar se sensação de "dirigir" é satisfatória nessa velocidade com a escala dos cenários Low Poly.
- [ ] **Cruzamento de trilhos (Km 4,0)**: Trem como elemento de gameplay (Marcus) ou apenas visual? Decisão pendente para fase de arte.
- [ ] **Mapa (TAB) interativo**: O "X" vermelho e a rota fantasma do Dia 5 exigem mapa dinâmico — confirmar viabilidade técnica com o arquiteto antes da implementação.
- [ ] **Painel digital do Town Center**: Substituir texto no Dia 5 exige shader ou UI separada — definir abordagem técnica.

---

*— Atlas, investigando a verdade 🔎*
*Documento gerado em suporte ao desenvolvimento de Bus Shift | Ravenswood, PA — O Ônibus 104 ainda está rodando.*

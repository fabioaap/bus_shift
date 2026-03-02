# Bus Shift — Documentação de Trilha Sonora (Music)

> **Jogo:** Bus Shift | **Engine:** Unity 6 | **Versão do doc:** 1.0
> **Última atualização:** —
> **Status de produção:** 🔴 Não iniciado

---

## Visão Geral

Este documento define todas as **músicas, trilhas adaptativas e diretrizes de composição** para Bus Shift.

### Filosofia Sonora

Bus Shift é um jogo sobre **erosão psicológica gradual** — não sobre jumpscares barulhentos.
A trilha deve refletir isso: começa quase silenciosa e **infecta** o jogador lentamente, como a maldição infecta Ray Morgan.

> **Princípio central:** O silêncio estratégico é mais assustador que qualquer dissonância.
> A música nunca deve competir com os SFX dos fantasmas — ela é o chão que sustenta o terror, não o terror em si.

### Convenção de Nomenclatura

```
mus_[contexto]_[estado]
```

---

## 1. Tracks Necessárias

| # | ID | Nome | Uso no Jogo | Duração Est. | Loop | BPM | Humor | MVP |
|---|----|------|-------------|:------------:|:----:|:---:|-------|:---:|
| 01 | `mus_menu_theme` | Tema do Menu Principal | Tela de menu, créditos, loading | 3–4 min | ✅ | 52 | Melancólico, contemplativo, melancolia infantil | ✅ Sim |
| 02 | `mus_gameplay_day_calm` | Gameplay Dia — Calmo | Dia 1–3, tensão 0–40% | 4–5 min | ✅ | 60 | Tênue, ambíguo, mundo normal com algo errado | ✅ Sim |
| 03 | `mus_gameplay_day_tense` | Gameplay Dia — Tenso | Dia 3–5, tensão 50%+ | 4–5 min | ✅ | 72 | Desconforto crescente, strings irregulares | ✅ Sim |
| 04 | `mus_gameplay_night_calm` | Gameplay Noite — Calmo | Noite com tensão 0–40% | 4–5 min | ✅ | 55 | Isolamento, vazio, presença invisível | ✅ Sim |
| 05 | `mus_gameplay_night_tense` | Gameplay Noite — Tenso | Noite com tensão 50%+ | 4–5 min | ✅ | 68 | Horror ascendente, pico de pressão | ✅ Sim |
| 06 | `mus_ghost_attack_stinger` | Stinger de Aparição | Fantasma se manifesta (jumpscare) | 2–3 seg | ❌ | — | Impacto, choque, corte abrupto | ✅ Sim |
| 07 | `mus_ending_escape` | Final A — A Fuga | Cutscene Final A ("A Última Parada") | 2–3 min | ❌ | 58 | Alívio que dói — sobrevivência sem redenção | ✅ Sim |
| 08 | `mus_ending_redemption` | Final B — A Redenção | Cutscene Final B (libertação das crianças) | 2–3 min | ❌ | 64 | Esperança frágil, resolução emotiva, paz | ✅ Sim |
| 09 | `mus_ending_cycle` | Final C — O Ciclo | Cutscene Final C (condenação) | 1,5–2 min | ❌ | 80→120 | Terror puro, dissonância total, inevitável | ✅ Sim |
| 10 | `mus_gameover` | Jingle de Game Over | Tela de game over | 4–6 seg | ❌ | — | Falha, peso, sem esperança | ✅ Sim |
| 11 | `mus_day_transition` | Transição de Período | Entre manhã → noite → manhã | 15–20 seg | ❌ | — | Mudança de estado, ambíguo | ⬜ Não |
| 12 | `mus_narrative_reveal` | Revelação Narrativa | Dia 4: rádio com nomes das crianças mortas | 60–90 seg | ❌ | 48 | Choque emocional, congelamento, incredulidade | ✅ Sim |

---

## 2. Sistema de Layers Adaptativo

Bus Shift usa um sistema de **música em camadas (layered music)** que responde em tempo real ao nível de tensão do jogador (0–100%) e ao período do dia.

### Como funciona

O AudioMixer do Unity recebe o parâmetro `TensionLevel` (float 0.0–1.0) e habilita/desabilita layers independentes que estão **em sincronia rítmica e harmônica** umas com as outras.

Cada layer é gravada como um arquivo separado para ser mixada em runtime:

```
mus_gameplay_day_calm_L1.ogg  ← sempre toca
mus_gameplay_day_calm_L2.ogg  ← fade in a partir de 25%
mus_gameplay_day_calm_L3.ogg  ← fade in a partir de 50%
mus_gameplay_day_calm_L4.ogg  ← fade in a partir de 75%
```

> **Regra de ouro:** Cada layer deve funcionar sozinha E combinada com as anteriores.
> Nunca adicione uma layer que "quebre" a harmonia das layers anteriores.

---

### Layer 1 — Base Ambiente (0–100% tensão)

**Sempre ativa.** Nunca para durante o gameplay.

| Parâmetro | Especificação |
|-----------|---------------|
| Instrumentação | Piano preparado, cordas em sul ponticello pppp, textura de pad de sintetizador analógico |
| Caráter | Melodia simples em modo menor — evoca infância distante, memória dolorosa |
| Volume | Base -18 LUFS |
| Nota harmônica | Tônica em Dó menor (base para todas as outras layers) |
| Comprimento do loop | Deve ser múltiplo de 8 compassos para sincronia perfeita |
| Notas de produção | Reverb longo (pre-delay 30ms, decay 4s); sem bateria ou percussão nesta layer |

**Objetivo:** O jogador deve sentir que há algo ligeiramente errado desde o início, mas não saber exatamente o quê.

---

### Layer 2 — Pulsação Baixa (25–100% tensão)

**Ativa quando tensão ≥ 25%.** Fade in suave ao longo de 8 segundos.

| Parâmetro | Especificação |
|-----------|---------------|
| Instrumentação | Low-end percussivo (bombo suave, low tom), contrabaixo arco em tremolo, pedal de Fá grave |
| Caráter | Pulsação orgânica — como batimento cardíaco irregular; não é um ritmo de dança, é biológico |
| BPM | Sincronizado com Layer 1; batida a cada 2 compassos |
| Volume relativo | -6 dB em relação à Layer 1 quando entrando; pode chegar a -2 dB em 50% tensão |
| Notas de produção | Sidechain leve com Layer 1 para criar "respiração"; Low-pass filter 200 Hz no bombo |

**Objetivo:** O jogador começa a sentir peso, pressão, sem saber de onde vem.

---

### Layer 3 — Tensão Harmônica (50–100% tensão)

**Ativa quando tensão ≥ 50%.** Fade in ao longo de 5 segundos.

| Parâmetro | Especificação |
|-----------|---------------|
| Instrumentação | Violinos em cluster dissonante (semi-tons adjacentes), cello em col legno, piano com preparação extrema (papel no interior) |
| Caráter | Dissonância controlada — não é ruído, é harmonia em colapso. Adiciona tensão sem resolver. |
| Intervalos | Cluster de 2ª menor e 7ª maior; tritono estratégico no momento de maior tensão |
| Progressão | Não resolve — cada frase termina em suspensão, criando expectativa não preenchida |
| Volume relativo | Sobe de -12 dB (entrada) para -4 dB (tensão 75%) |
| Notas de produção | Use bow pressure e extended techniques; o som deve ser físico, não eletrônico |

**Objetivo:** Desconforto real. O jogador quer que a música resolva, mas ela não resolve.

---

### Layer 4 — Horror Total (75–100% tensão)

**Ativa quando tensão ≥ 75%.** Fade in agressivo ao longo de 3 segundos.

| Parâmetro | Especificação |
|-----------|---------------|
| Instrumentação | Sintetizadores de ruído branco filtrado, vozes infantis processadas (gravadas com crianças, filtro robótico + pitch shift), contrabaixo de orquestra em harmônicos |
| Caráter | Horror puro e imediato. Esta layer deve fazer o jogador querer desligar o jogo. |
| Vozes | Fragmentos das falas dos fantasmas processadas (Emma's "hihihi", Thomas sussurrando) transformadas em textura musical |
| LFO | Modulação lenta de volume (1 ciclo a cada 4s) — simula respiração de algo grande |
| Volume relativo | Sobe de -8 dB (entrada) para 0 dB relativo (100% tensão) |
| Notas de produção | Alinhamento com tensão: em 100%, Layer 4 e Layer 3 devem dominar o mix. Layer 1 sempre audível como "fantasma" do que foi. |

**Objetivo:** Estado de sobrevivência. O jogador sente que algo ruim está inevitavelmente chegando.

---

### Mapa de Transição do Sistema Adaptativo

```
TENSÃO:     0%         25%         50%         75%        100%
            |           |           |           |           |
Layer 1:  ████████████████████████████████████████████████████  (sempre)
Layer 2:              ████████████████████████████████████████
Layer 3:                          ████████████████████████████
Layer 4:                                      ████████████████

VOLUME:   ppp        pp-p        mp-mf        mf-f         ff
```

---

### Tabela de Configuração por Track

Cada track de gameplay deve ter suas 4 layers gravadas separadamente:

| Track Base | L1 (arquivo) | L2 (arquivo) | L3 (arquivo) | L4 (arquivo) |
|------------|-------------|-------------|-------------|-------------|
| `mus_gameplay_day_calm` | `..._L1.ogg` | `..._L2.ogg` | `..._L3.ogg` | `..._L4.ogg` |
| `mus_gameplay_day_tense` | `..._L1.ogg` | `..._L2.ogg` | `..._L3.ogg` | `..._L4.ogg` |
| `mus_gameplay_night_calm` | `..._L1.ogg` | `..._L2.ogg` | `..._L3.ogg` | `..._L4.ogg` |
| `mus_gameplay_night_tense` | `..._L1.ogg` | `..._L2.ogg` | `..._L3.ogg` | `..._L4.ogg` |

> **Total de arquivos de gameplay:** 4 tracks × 4 layers = **16 arquivos de áudio**

---

## 3. Descrições Detalhadas das Tracks

### `mus_menu_theme` — Tema do Menu Principal

**Conceito:** O jogador está na tela de menu sem saber nada sobre o jogo ainda. A música deve
gerar curiosidade e melancolia — não terror explícito. Deve soar como uma memória de infância
que ficou malassombrada sem que você saiba por quê.

**Estrutura sugerida:**
```
0:00–0:30  Introdução — piano solo, melodia simples em modo Dórico
0:30–1:30  Desenvolvimento — strings entram em pp, harmonizando a melodia
1:30–2:30  Clímax melancólico — todos os instrumentos, dinâmica mf
2:30–3:00  Dissolução — piano retorna sozinho, última nota não resolve
3:00→      Loop suave de volta para 0:00
```

**Instrumentação:** Piano de concerto (ressonância longa), string quartet (violino, viola, cello), opcional: caixa de música distante e reverberada.

**Tom:** Imagine a trilha de um filme sobre crianças que nunca cresceram. Belo e errado ao mesmo tempo.

---

### `mus_gameplay_day_calm` — Gameplay Dia, Tensão Baixa

**Conceito:** O motorista dirigindo em Ravenswood nos primeiros dias. Cidade normal, céu cinza,
crianças no banco de trás. Algo está levemente errado, mas não dá para nomear.

**Caráter:** Minimalista, esparso. Mais silêncio do que som. Cada nota importa.

**Referência de humor:** Imagine olhar pela janela do ônibus e ver uma criança parada na calçada
olhando fixamente para você. Ela sorri. O ônibus já passou. Você não para.

---

### `mus_gameplay_night_calm` — Gameplay Noite, Tensão Baixa

**Conceito:** Noite em Ravenswood. A cidade ficou quieta. As crianças estão nos assentos.
O motor do ônibus é o único barulho familiar. Esta track deve fazer o jogador sentir que
está completamente sozinho — e que esse isolamento é um perigo.

**Diferença do dia:** Mais sub-bass, mais espaço entre as notas, reverb mais longo.
A sensação de distância aumenta à noite.

---

### `mus_ghost_attack_stinger` — Stinger de Aparição

**Conceito:** Um stinger de 2–3 segundos que dispara quando um fantasma se manifesta abruptamente.

**Estrutura:**
```
0s     — Silêncio (corte de toda a música de gameplay)
0.1s   — Impacto: cluster de cordas fff + percussão impactante
0.5s   — Decay: reverb longo da percussão, strings em fade out rápido
2.0s   — Silêncio absoluto antes da música de gameplay retomar
```

**Crítico:** O silêncio ANTES do stinger é tão importante quanto o stinger em si.
Fazer um ducking de -30dB em toda a música por 0.1s antes do impacto para maximizar o choque.

**Variações:** Gravar 3 versões levemente diferentes (pitch, timing) para evitar
repetição previsível. Sistema de reprodução aleatório no Unity.

---

### `mus_ending_escape` — Final A: A Fuga

**Conceito:** Ray Morgan saiu do ônibus e está caminhando pela estrada escura. Ele sobreviveu.
Ele não resolveu nada. Ele vai embora.

Esta música é sobre a vitória mais triste possível — a de simplesmente continuar vivo.

**Estrutura:**
```
0:00–0:30   Silêncio quase total — apenas drone grave, Ray andando
0:30–1:00   Piano entra com tema do menu, mas mais simples, mais nu
1:00–2:00   Strings entram suavemente — alívio que dói, não é alegria
2:00–2:30   Dissolução — música some gradualmente enquanto Ray some na estrada
```

**Última nota:** Não resolve. Acorde aberto. O jogador carrega a ambiguidade.

---

### `mus_ending_redemption` — Final B: A Redenção

**Conceito:** As cinco crianças foram libertadas. Marcus, Emma, Thomas, Grace e Oliver.
Pela primeira vez em décadas, elas podem descansar.

Esta é a música mais esperançosa do jogo — e ainda assim deve guardar um peso de
tudo que custou chegar até aqui.

**Estrutura:**
```
0:00–0:20   Silêncio
0:20–1:00   Piano — tema mais simples que o do menu, quase como uma cantiga de ninar
1:00–2:00   Uma a uma, uma nota de cada instrumento de corda entra — como vozes se despedindo
2:00–2:30   Todos os instrumentos em uníssono, uma última vez, em Ré Maior (única vez no jogo em modo maior)
2:30–3:00   Dissolução para silêncio
```

**Nota harmônica:** Esta é a ÚNICA música do jogo em modo maior. Reservar Ré Maior exclusivamente
para este momento torna a resolução genuinamente emocionante.

---

### `mus_ending_cycle` — Final C: O Ciclo

**Conceito:** Ray perdeu o controle. Ele está acelerando em direção ao precipício.
As crianças estão encorajando. O acidente vai se repetir.

Esta música não tem resolução porque não há resolução. É apenas o inevitável chegando.

**Estrutura:**
```
0:00–0:20   BPM 80 — gameplay normal distorcido, familiar mas errado
0:20–0:60   BPM sobe progressivamente para 110 — as layers todas ativadas
0:60–1:20   BPM 120 — caos total, todas as layers em fff
1:20–1:30   Silêncio absoluto (1s) — o ônibus saiu da estrada
1:30–1:40   Impacto + eco de canyon (mesmo que sfx_narrative_accident_echo)
1:40→       Silêncio. A manchete de jornal aparece em silence.
```

**Automação de BPM:** Usar Timeline do Unity para aceleração de BPM em tempo real.
Alternativa: gravar versões em 80, 95 e 120 BPM e crossfade.

---

### `mus_gameover` — Jingle de Game Over

**Conceito:** 4–6 segundos. Curto, pesado, sem drama excessivo.
Não deve irritar em repetições (o jogador vai morrer muitas vezes).

**Estrutura:**
```
0s    — Acorde de cello grave, fortissimo
1s    — Decay natural — reverb longo
4–6s  — Silêncio
```

**Regra:** Sem melodia. Sem ritmo. Apenas presença e ausência.

---

### `mus_narrative_reveal` — Revelação Narrativa (Dia 4)

**Conceito:** O rádio toca os nomes das 5 crianças mortas. O jogador percebe que as crianças
que vê todos os dias estão mortas há 1 ano.

Esta música deve fazer o mundo do jogo mudar irreversivelmente.

**Estrutura:**
```
0:00–0:15   Fundo de rádio estático (diegético) — a notícia sendo lida
0:15–0:45   Piano entra muito suavemente sob a fala do locutor
0:45–1:00   Strings adicionam peso — mas ainda muito contido
1:00–1:30   Momento de silêncio emocional — as crianças sendo nomeadas
```

**Crítico:** A música nunca deve sobrepor a voz do locutor. É suporte, não protagonismo.

---

## 4. Referências de OST

Estas referências guiam o humor, técnicas de composição e abordagem de produção.
**Importante:** Usar como inspiração de linguagem e abordagem, não como fonte de plágio.

---

### REF 01 — Outlast (2013) — Samuel Laflamme

**Relevância:** Alta — ambiente de horror psicológico com tensão sustentada.

| O que usar | Como aplicar |
|------------|--------------|
| Uso de silêncio estratégico como ferramenta de medo | Tratar o silêncio como nota musical ativa, não ausência |
| Strings em técnicas estendidas (col legno, sul ponticello) | Camadas 3 e 4 do sistema adaptativo |
| Transições abruptas de dinâmica (pppp → fff) | `mus_ghost_attack_stinger` |
| Drones de baixa frequência como tensão subliminar | Sub-bass persistente nas layers noturnas |

---

### REF 02 — Alien: Isolation (2014) — Ridley Scott / Sega Scoring

**Relevância:** Alta — sistema adaptativo de música respondendo a ameaças.

| O que usar | Como aplicar |
|------------|--------------|
| Música que "escuta" o jogador — responde em tempo real | Modelo de referência para o sistema de 4 layers |
| Orquestração pesada e dissonante mas nunca aleatória | Layer 4: cada nota tem intenção harmônica |
| Transições suaves entre estados de tensão | Crossfade de 3–8 segundos entre layers |
| Uso de solo de brass como stinger de ameaça | `mus_ghost_attack_stinger`: explorar French horn sforzando |

---

### REF 03 — Phasmophobia (2020) — musica ambiente procedural

**Relevância:** Média — atmosfera de investigação paranoica.

| O que usar | Como aplicar |
|------------|--------------|
| Ambiências que soam quase como silêncio | Layer 1 de todas as tracks de gameplay |
| Sons que estão no limiar do audível | Vozes de Thomas Sanders processadas na Layer 4 |
| Sensação de "nunca saber o que vem a seguir" | Evitar padrões rítmicos óbvios no gameplay |

---

### REF 04 — Five Nights at Freddy's 2 (2014)

**Relevância:** Alta — referência direta do estilo do jogo.

| O que usar | Como aplicar |
|------------|--------------|
| Som de ambiente de corredor vazio + chiado eletrônico | `sfx_grace_camera_static` em sfx.md; influência na Layer 2 noturna |
| Silêncio que precede o jumpscare como construção de medo | Estrutura do `mus_ghost_attack_stinger` |
| Distinção clara entre "seguro" e "em perigo" via áudio | Diferença perceptível entre Layer 1 e Layer 4 ativada |
| Stingers curtos e brutais de game over | `mus_gameover`: menos de 6 segundos, sem melodia |

---

### REF 05 — Layers of Fear (2016) — Arek Reikowski

**Relevância:** Alta — degradação psicológica sonificada.

| O que usar | Como aplicar |
|------------|--------------|
| Progressão musical que reflete deterioração mental do protagonista | Tracks do Dia 5 devem soar como versões quebradas das tracks do Dia 1 |
| Piano preparado como instrumento de horror | Layer 1 e `mus_menu_theme` |
| Dinâmicas extremas sem aviso | `mus_ending_cycle` — BPM crescente |
| Melodia reconhecível que gradualmente se distorce | Variações do tema do menu nas tracks de gameplay dos dias finais |

---

### REF 06 — Silent Hill 2 (2001) — Akira Yamaoka

**Relevância:** Muito alta — psicologia, trauma, culpa sonificados.

| O que usar | Como aplicar |
|------------|--------------|
| Melodia simples e melancólica como núcleo emocional | Tema do piano do `mus_menu_theme` e `mus_ending_escape` |
| Guitarra elétrica processada como textura de horror industrial | Optional layer adicional para dias 4–5 (guitar slide distorcida) |
| Dissonância que vem da harmonia, não do ruído | Layer 3: clusters são de notas reais, não ruído branco |
| Finais musicais que não resolvem | Todas as tracks terminam em suspensão, nunca em tônica |

---

### REF 07 — The Conjuring 2 (2016) — Joseph Bishara

**Relevância:** Média — técnicas de stinger e scoring de horror cinematográfico.

| O que usar | Como aplicar |
|------------|--------------|
| Vozes humanas processadas como instrumento de terror | Layer 4: vozes de crianças transformadas em textura |
| Micro-timing — suspensão de frações de segundo antes do impacto | Timing do `mus_ghost_attack_stinger`: 0.1s de silêncio antes do impacto |
| Orquestra convencional em uso não-convencional | Referência para técnicas estendidas nas strings |

---

### REF 08 — Martha is Dead (2022) — Leonardo Ceriotti

**Relevância:** Alta — horror de guerra psicológico com componente emocional forte.

| O que usar | Como aplicar |
|------------|--------------|
| Música que honra personagens traumatizados sem explorar o trauma | Abordagem para os temas de final (A, B, C) — respeito emocional |
| Folk elements mesclados com horror | `mus_menu_theme`: considerar viola solo em vez de violin para timbre mais terroso |
| Uso de silêncio nos finais para deixar o jogador processar | `mus_ending_escape` e `mus_ending_redemption`: não apressar a dissolução |

---

## 5. Diretrizes de Composição

### 5.1 Instrumentação Recomendada

#### Instrumentos de Base (todas as tracks)

| Instrumento | Papel | Notas |
|-------------|-------|-------|
| Piano preparado | Melodia principal + textura | Preparações com papel/clipe para timbre percussivo |
| Quarteto de cordas | Harmonia + tensão | Violin I, Violin II, Viola, Cello |
| Contrabaixo | Pedal de baixo | Arco, não pizzicato; harmônicos em momentos de horror |
| Pad analógico | Textura de fundo | Sintetizador vintage (Juno, Moog); não digital demais |

#### Instrumentos de Tensão (Layers 3–4)

| Instrumento | Papel | Notas |
|-------------|-------|-------|
| Vozes infantis processadas | Horror personalizado | Gravar crianças reais (8–12 anos); processamento heavy em pós |
| Percussão orquestral | Pulsação e impacto | Tam-tam, bass drum sem ressonância cortada |
| Sintetizador de ruído filtrado | Textura de fundo | Noise + bandpass lento — simula interferência eletromagnética |
| Whistling (assovio) | Elemento melódico perturbador | Assovio humano processado — ativa memória de infância de forma inquietante |

#### Instrumentos Proibidos

> ❌ **Nunca usar nestas tracks:**
> - Jump scare clichê (violino em glissando ascendente muito rápido — usado demais)
> - Bateria eletrônica com kick na 1
> - Pad de synth muito digital / EDM
> - Qualquer instrumento que soe "moderno" ou "clean"

---

### 5.2 BPM por Contexto

| Contexto | BPM | Sensação | Notas |
|----------|:---:|----------|-------|
| Menu principal | 52 | Contemplação, tempo parado | Sem percussão; o "batimento" é o piano |
| Gameplay dia — calmo | 58–62 | Normalidade levemente errada | Batida em 2, não em 1 |
| Gameplay dia — tenso | 68–75 | Desconforto crescente | Batida irregular, off-beat |
| Gameplay noite — calmo | 54–58 | Isolamento, espaço vazio | Muito esparso; pausas longas entre notas |
| Gameplay noite — tenso | 64–72 | Perigo iminente | Ritmo errático, como respiração controlada antes de gritar |
| Final A (Fuga) | 56–60 | Alívio pesado | Rubato — o tempo pode flutuar como memória |
| Final B (Redenção) | 62–66 | Esperança frágil | Regulado, estável — primeira vez que o tempo "acerta" |
| Final C (Ciclo) | 80→120 | Inevitabilidade acelerada | Automação crescente de BPM |

---

### 5.3 Tonalidade e Harmonia

| Track | Tonalidade base | Notas |
|-------|----------------|-------|
| Todas (exceto Final B) | Dó menor | Base unificada; permite transições harmônicas suaves |
| Final B (Redenção) | Ré Maior | Único momento em modo maior no jogo — impacto emocional reservado |
| Layer 4 | Atonal / Cluster | Sem tonalidade definida; harmonia em colapso |
| `mus_narrative_reveal` | Lá♭ menor | Distante de Dó menor para sensação de "mundo virou" |

---

### 5.4 Regras de Transição

#### Transição de Tensão (Gameplay)

```
Tensão sobe:    Fade in da próxima layer em T segundos
Tensão desce:   Fade out da layer ativa em T×2 segundos (descida mais lenta que subida)

Valores de T por layer:
  Layer 2: T = 8 segundos
  Layer 3: T = 5 segundos
  Layer 4: T = 3 segundos
```

**Rationale:** A tensão sobe rápido (perigo imediato) mas desce devagar (o medo persiste).

---

#### Transição Dia → Noite

```
1. Fade out da track de dia em 4 segundos
2. Play de mus_day_transition (15–20s)
3. Fade in da track de noite durante os últimos 8s do mus_day_transition
4. mus_day_transition termina — apenas track noturna ativa
```

---

#### Transição Entre Stinger e Gameplay

```
1. Stinger dispara: todas as layers sofrem ducking de -30dB em 0.05s
2. Stinger toca (2–3s)
3. Após o stinger: fade in da layer correspondente ao nível de tensão atual em 2s
```

---

#### Transição para Cutscene de Final

```
1. Gameplay music fade out em 3 segundos
2. 1 segundo de silêncio absoluto
3. Track do final começa
```

---

### 5.5 Regras Gerais de Produção

| Regra | Detalhe |
|-------|---------|
| **Sincronização de loops** | Todos os loops de gameplay devem ter duração de múltiplos de 8 compassos para sincronizar entre layers |
| **Avoid ear fatigue** | Nenhuma layer deve ter elementos acima de 4kHz com volume sustentado — faixas altas reservadas para stingers |
| **Consistência de espaço** | Reverb igual em todas as layers da mesma track (mesmo "ambiente acústico") |
| **No hard endings** | Loops devem ter fade de 0.3s no ponto de loop para eliminar clicks |
| **Reserva de headroom** | Deixar -6dB de headroom em cada arquivo entregue; normalização final feita na engine |
| **Diegético vs. não-diegético** | `sfx_tool_radio_playing` é diegético (música no mundo do jogo). Todas as tracks de gameplay são não-diegéticas. Não misturar os dois mundos exceto no rádio. |

---

## 6. Estrutura de Arquivos de Entrega

```
audio/
└── music/
    ├── menu/
    │   └── mus_menu_theme.ogg
    ├── gameplay/
    │   ├── mus_gameplay_day_calm_L1.ogg
    │   ├── mus_gameplay_day_calm_L2.ogg
    │   ├── mus_gameplay_day_calm_L3.ogg
    │   ├── mus_gameplay_day_calm_L4.ogg
    │   ├── mus_gameplay_day_tense_L1.ogg
    │   ├── mus_gameplay_day_tense_L2.ogg
    │   ├── mus_gameplay_day_tense_L3.ogg
    │   ├── mus_gameplay_day_tense_L4.ogg
    │   ├── mus_gameplay_night_calm_L1.ogg
    │   ├── mus_gameplay_night_calm_L2.ogg
    │   ├── mus_gameplay_night_calm_L3.ogg
    │   ├── mus_gameplay_night_calm_L4.ogg
    │   ├── mus_gameplay_night_tense_L1.ogg
    │   ├── mus_gameplay_night_tense_L2.ogg
    │   ├── mus_gameplay_night_tense_L3.ogg
    │   └── mus_gameplay_night_tense_L4.ogg
    ├── stingers/
    │   ├── mus_ghost_attack_stinger_v1.ogg
    │   ├── mus_ghost_attack_stinger_v2.ogg
    │   └── mus_ghost_attack_stinger_v3.ogg
    ├── endings/
    │   ├── mus_ending_escape.ogg
    │   ├── mus_ending_redemption.ogg
    │   └── mus_ending_cycle.ogg
    ├── narrative/
    │   ├── mus_narrative_reveal.ogg
    │   └── mus_day_transition.ogg
    └── ui/
        └── mus_gameover.ogg
```

---

## 7. Checklist de Produção

### Menu e Narrativa
- [ ] `mus_menu_theme` — composição e mixagem
- [ ] `mus_narrative_reveal` — composição e sincronização com locução (Dia 4)
- [ ] `mus_day_transition` — composição

### Gameplay — Dia
- [ ] `mus_gameplay_day_calm_L1`
- [ ] `mus_gameplay_day_calm_L2`
- [ ] `mus_gameplay_day_calm_L3`
- [ ] `mus_gameplay_day_calm_L4`
- [ ] `mus_gameplay_day_tense_L1`
- [ ] `mus_gameplay_day_tense_L2`
- [ ] `mus_gameplay_day_tense_L3`
- [ ] `mus_gameplay_day_tense_L4`
- [ ] Teste de sincronia entre todas as layers do dia

### Gameplay — Noite
- [ ] `mus_gameplay_night_calm_L1`
- [ ] `mus_gameplay_night_calm_L2`
- [ ] `mus_gameplay_night_calm_L3`
- [ ] `mus_gameplay_night_calm_L4`
- [ ] `mus_gameplay_night_tense_L1`
- [ ] `mus_gameplay_night_tense_L2`
- [ ] `mus_gameplay_night_tense_L3`
- [ ] `mus_gameplay_night_tense_L4`
- [ ] Teste de sincronia entre todas as layers da noite

### Stingers
- [ ] `mus_ghost_attack_stinger_v1`
- [ ] `mus_ghost_attack_stinger_v2`
- [ ] `mus_ghost_attack_stinger_v3`
- [ ] Validação: silêncio pré-stinger de 0.1s correto
- [ ] Validação: 3 versões soam diferentes o suficiente para não serem previsíveis

### Finais
- [ ] `mus_ending_escape` — composição e revisão emocional
- [ ] `mus_ending_redemption` — composição; confirmar uso de Ré Maior
- [ ] `mus_ending_cycle` — composição; automação de BPM 80→120
- [ ] Revisão: cada final soando claramente distinto dos outros

### UI
- [ ] `mus_gameover` — composição (≤ 6 segundos)

### Integração Unity
- [ ] AudioMixer Groups configurados (Music, Gameplay, Narrative, UI)
- [ ] Parâmetro `TensionLevel` exposto e conectado ao sistema de layers
- [ ] Sistema de fade in/out de layers testado em gameplay real
- [ ] Stinger system: ducking automático de -30dB em 0.05s testado
- [ ] Todas as tracks de loop validadas (sem clicks no ponto de loop)
- [ ] Teste de transição dia→noite com `mus_day_transition`
- [ ] Teste de transição gameplay→cutscene de final

---

## 8. Notas Adicionais

### Sobre as Vozes Infantis na Layer 4

As vozes utilizadas na Layer 4 devem ser gravadas com crianças reais em sessão controlada,
lendo textos neutros. O processamento que as transforma em textura de horror é feito em pós-produção.

> **Consideração ética:** Garantir que as crianças e responsáveis saibam que o material
> será processado para uso em jogo de terror. Não gravar crianças em estado emocional real de medo.

### Sobre a Versão Diegética do Rádio

`sfx_tool_radio_playing` (documentado em sfx.md) é a música que toca no rádio do ônibus
quando o jogador usa a tecla R. Deve ser uma versão simplificada e processada (filtro AM)
de um tema musical neutro — pode ser uma versão anterior ao processamento do `mus_menu_theme`,
sugerindo que a melodia existia antes do horror.

### Sobre o Final B e Ré Maior

O uso de Ré Maior exclusivamente no Final B é uma decisão arquitetural da trilha.
**Não usar Ré Maior em nenhuma outra track do jogo.** A resolução harmônica genuína
deve ser única para que o jogador a reconheça emocionalmente mesmo sem teoria musical.

---

*Documentação de Trilha Sonora — Bus Shift | Ravenswood, 1998*

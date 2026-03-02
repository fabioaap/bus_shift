# Bus Shift — Documentação de Efeitos Sonoros (SFX)

> **Jogo:** Bus Shift | **Engine:** Unity 6 | **Versão do doc:** 1.0
> **Última atualização:** —
> **Status de produção:** 🔴 Não iniciado

---

## Visão Geral

Este documento é o checklist mestre de todos os efeitos sonoros necessários para **Bus Shift**.
Cada SFX está descrito com nomenclatura padronizada, especificações técnicas e notas de implementação.

### Convenção de Nomenclatura

```
sfx_[categoria]_[nome]
```

| Prefixo | Categoria |
|---------|-----------|
| `sfx_bus_` | Motor, mecânica e chassis do ônibus |
| `sfx_door_` | Portas e mecanismos de abertura |
| `sfx_emma_` | Fantasma: Emma Lynch |
| `sfx_thomas_` | Fantasma: Thomas Sanders |
| `sfx_marcus_` | Fantasma: Marcus Reid |
| `sfx_grace_` | Fantasma: Grace Harper |
| `sfx_oliver_` | Fantasma: Oliver Crane |
| `sfx_tool_` | Contramedidas do motorista |
| `sfx_ui_` | Feedback de interface e sistema |
| `sfx_kids_` | Crianças vivas passageiras |
| `sfx_amb_day_` | Ambiente externo — período diurno |
| `sfx_amb_night_` | Ambiente externo — período noturno |

### Especificações Técnicas Globais

| Parâmetro | Valor recomendado |
|-----------|-------------------|
| Sample rate | 44.100 Hz |
| Bit depth | 16 bit (exports finais) / 24 bit (masters) |
| Formato entrega | `.wav` (masters) → `.ogg` (Unity runtime) |
| Loudness alvo | -18 LUFS (integrado) |
| True Peak máximo | -1 dBTP |
| Mono vs Stereo | Conforme coluna `Canal` de cada SFX |

---

## 1. Motor do Ônibus

> Sons do Ônibus 104 — veículo velho, cansado, com motor gutural e mecânica enferrujada.
> **Referência de caráter:** Motor diesel dos anos 80, mais rugido do que potência.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 01 | `sfx_bus_idle_low` | `sfx_bus_idle_low.ogg` | Motor em marcha lenta, tensão baixa (0–40% tensão) | Loop | Mono | ✅ Sim | Volume dinâmico via FMOD/AudioMixer |
| 02 | `sfx_bus_idle_high` | `sfx_bus_idle_high.ogg` | Motor em marcha lenta, tensão alta (75%+ tensão) — som mais irregular, falhas sutis | Loop | Mono | ✅ Sim | Transição crossfade a partir de `idle_low` |
| 03 | `sfx_bus_accelerate` | `sfx_bus_accelerate.ogg` | Motor acelerando progressivamente (W pressionado) | Loop | Mono | ✅ Sim | Pitch sobe com velocidade |
| 04 | `sfx_bus_decelerate` | `sfx_bus_decelerate.ogg` | Motor desacelerando (W solto, velocidade caindo) | Loop | Mono | ✅ Sim | Pitch desce com velocidade |
| 05 | `sfx_bus_brake_hard` | `sfx_bus_brake_hard.ogg` | Freio brusco — rangido de freios + motor engasgando | One-shot | Mono | ✅ Sim | Disparar quando desacelerar > 15 km/h em <0,5s |
| 06 | `sfx_bus_reverse` | `sfx_bus_reverse.ogg` | Marcha a ré — som grave e hesitante | Loop | Mono | ✅ Sim | Ativa quando S pressionado com ônibus parado |
| 07 | `sfx_bus_ignition_start` | `sfx_bus_ignition_start.ogg` | Girar a chave — motor tentando ligar, engasga uma vez antes de pegar | One-shot | Stereo | ✅ Sim | Cena de intro Dia 1; rádio liga em estática no final |
| 08 | `sfx_bus_ignition_fail` | `sfx_bus_ignition_fail.ogg` | Motor tentando ligar e falhando (eventos de horror Dia 4–5) | One-shot | Stereo | ⬜ Não | Acionado narrativamente em momentos específicos |
| 09 | `sfx_bus_engine_sputter` | `sfx_bus_engine_sputter.ogg` | Motor engasgando brevemente — "falta" aleatória 1–3s | One-shot | Mono | ⬜ Não | Frequência aumenta com tensão; reforça sensação de veículo mal-assombrado |
| 10 | `sfx_bus_collision` | `sfx_bus_collision.ogg` | Impacto de colisão — metal amassando, vidro vibrando | One-shot | Stereo | ✅ Sim | Condição de derrota: CRIANÇA 2 (Emma) abre a porta; ou colisão na estrada |

---

## 2. Portas do Ônibus

> Porta dianteira do passageiro + mecanismo da cabine do motorista. Sons ásperos, desgastados.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 11 | `sfx_door_open_normal` | `sfx_door_open_normal.ogg` | Porta abrindo — chiado de pistão pneumático velho | One-shot | Stereo | ✅ Sim | Tecla T próxima a ponto de ônibus |
| 12 | `sfx_door_close_normal` | `sfx_door_close_normal.ogg` | Porta fechando — batida pesada de metal + pressão de ar | One-shot | Stereo | ✅ Sim | Após crianças embarcarem/desembarcarem |
| 13 | `sfx_door_mechanism_creak` | `sfx_door_mechanism_creak.ogg` | Rangido do mecanismo da porta — metal enferrujado | One-shot | Mono | ⬜ Não | Variação aleatória sobreposta ao abrir/fechar |
| 14 | `sfx_door_open_ghost` | `sfx_door_open_ghost.ogg` | Porta abrindo sozinha — lenta, rangendo, inquietante | One-shot | Stereo | ✅ Sim | Evento de ataque de Emma Lynch (CRIANÇA 2) |
| 15 | `sfx_door_panel_button` | `sfx_door_panel_button.ogg` | Clique de botão no painel de controle da porta | One-shot | Mono | ✅ Sim | Interação com painel; também som de tentativa de sabotagem de Emma |
| 16 | `sfx_door_lock_engage` | `sfx_door_lock_engage.ogg` | Trava de porta engatando — clique mecânico firme | One-shot | Mono | ✅ Sim | Confirmação sonora da contramedida Trava do Painel (SHIFT) |

---

## 3. Fantasmas

### 3.1 Emma Lynch — "A Burladora" (CRIANÇA 2)

> Risada característica "hihihi" — aguda, debochada, mal-comportada.
> Contramedida: Trava do Painel (SHIFT). Timer crítico: 2 segundos.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 17 | `sfx_emma_laugh_low` | `sfx_emma_laugh_low.ogg` | "hihihi" — baixo, distante, quase inaudível. Sinal de ativação inicial | One-shot | Mono | ✅ Sim | Primeira fileira de aviso; volume ~30% do canal |
| 18 | `sfx_emma_laugh_mid` | `sfx_emma_laugh_mid.ogg` | "hihihi" — médio, mais próximo, debochado. Aviso intermediário | One-shot | Mono | ✅ Sim | Emma se aproxima da cabine; volume ~65% |
| 19 | `sfx_emma_laugh_high` | `sfx_emma_laugh_high.ogg` | "hihihi" — alto, agudo, rasgado. **TRIGGER** de ataque ao painel | One-shot | Stereo | ✅ Sim | Inicia janela de 2s para o jogador usar SHIFT; posicionamento 3D: lateral direita do motorista |
| 20 | `sfx_emma_appear` | `sfx_emma_appear.ogg` | Som de materialização — ar deslocado, estática breve | One-shot | Stereo | ✅ Sim | Emma aparece ao lado do motorista; deve ser perturbador, não um jumpscare total |
| 21 | `sfx_emma_attack_success` | `sfx_emma_attack_success.ogg` | Botão pressionado por Emma — beep eletrônico distorcido + chiado | One-shot | Stereo | ✅ Sim | Emma apertou o botão; transição para `sfx_bus_collision` ou game over |
| 22 | `sfx_emma_repelled` | `sfx_emma_repelled.ogg` | Emma sendo afastada pela trava — grito frustrado, curto | One-shot | Mono | ✅ Sim | Contramedida bem-sucedida (SHIFT a tempo) |

---

### 3.2 Thomas Sanders — "O Narrador" (CRIANÇA 3)

> Voz de menino sussurrando histórias sombrias — começa quase inaudível e escala para gritos.
> Contramedida: Rádio (R). Efeito cascata de 0–8 segundos.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 23 | `sfx_thomas_whisper_low` | `sfx_thomas_whisper_low.ogg` | Sussurro de história de terror — fase 1 (0–4s). Mal audível, íntimo | Loop | Mono | ✅ Sim | Volume cresce linearmente; reverb longo |
| 24 | `sfx_thomas_whisper_mid` | `sfx_thomas_whisper_mid.ogg` | Sussurro — fase 2 (4–8s). Tom mais intenso, cadência hipnótica | Loop | Mono | ✅ Sim | Crossfade de `low` → `mid` na marca dos 4s |
| 25 | `sfx_thomas_whisper_critical` | `sfx_thomas_whisper_critical.ogg` | Narração em voz alta — fase 3 (8s+). Game over se não interrompida | Loop | Stereo | ✅ Sim | Deve soar como se viesse de dentro do crânio do jogador; bass boost leve |
| 26 | `sfx_thomas_narrative_radio` | `sfx_thomas_narrative_radio.ogg` | Voz no rádio: notícia das 5 crianças mortas (Dia 4) | One-shot | Stereo | ✅ Sim | Evento narrativo fixo; qualidade de rádio AM (filtro EQ passa-banda 200–4000 Hz) |
| 27 | `sfx_thomas_scare_children` | `sfx_thomas_scare_children.ogg` | Reação das crianças vivas sendo assustadas — sufoco coletivo de medo | One-shot | Stereo | ✅ Sim | Disparado quando tensão das crianças vivas aumenta por Thomas |
| 28 | `sfx_thomas_silenced` | `sfx_thomas_silenced.ogg` | Narração abruptamente cortada pelo rádio — silêncio tenso | One-shot | Mono | ✅ Sim | Contramedida bem-sucedida (R); silêncio de 0,5s antes da música do rádio |

---

### 3.3 Marcus Reid — "O Invasor" (CRIANÇA 1)

> Sons de movimento no corredor — passos, deslocamento de assentos, presença se aproximando.
> Contramedida: Microfone (Q segurar 3s).

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 29 | `sfx_marcus_move_seat` | `sfx_marcus_move_seat.ogg` | Assento sendo deslocado — rangido de couro velho, metal raspando | One-shot | Mono | ✅ Sim | Cada avanço de fileira; posicionamento 3D muda conforme distância |
| 30 | `sfx_marcus_footstep` | `sfx_marcus_footstep.ogg` | Passos no corredor metálico — leves, de criança, irregulares | One-shot | Mono | ✅ Sim | Variar pitch aleatoriamente ±10% entre repetições |
| 31 | `sfx_marcus_teleport` | `sfx_marcus_teleport.ogg` | Som de "salto" entre posições — distorção de espaço breve, whoosh perturbador | One-shot | Stereo | ✅ Sim | Marcus se teleporta instantaneamente; deve soar como espaço se dobrando |
| 32 | `sfx_marcus_breathe` | `sfx_marcus_breathe.ogg` | Respiração de Marcus muito próxima — na nuca do motorista | One-shot | Stereo | ⬜ Não | Disparado quando Marcus está na 1ª fila; pan centrado, baixo volume, muito íntimo |
| 33 | `sfx_marcus_repelled` | `sfx_marcus_repelled.ogg` | Marcus recuando — bufada de impaciência, passos apressados para o fundo | One-shot | Mono | ✅ Sim | Microfone bem-sucedido (Q completo) |

---

### 3.4 Grace Harper — "A Observadora" (CRIANÇA 4)

> Presença inquietante na câmera — chiado eletrônico, silêncio que pesa mais que qualquer som.
> Sem contramedida direta; aumenta dificuldade de condução.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 34 | `sfx_grace_camera_static` | `sfx_grace_camera_static.ogg` | Chiado da câmera aumentando — estática eletrônica crescente | Loop | Mono | ✅ Sim | Intensidade aumenta conforme Grace se fixa na câmera; controlado por parâmetro de tensão |
| 35 | `sfx_grace_camera_glitch` | `sfx_grace_camera_glitch.ogg` | Glitch abrupto na câmera — snap de imagem, chiado breve e agressivo | One-shot | Stereo | ✅ Sim | Variação aleatória sobreposta ao loop de estática; 1–3x por minuto |
| 36 | `sfx_grace_silence` | `sfx_grace_silence.ogg` | Silêncio pesado — todos os outros sons ambientes abaixam abruptamente | One-shot | Stereo | ⬜ Não | Usar ducking de AudioMixer; Grace "absorve" o som ao redor quando observa |
| 37 | `sfx_grace_stare` | `sfx_grace_stare.ogg` | Tom de tensão puro — drone de baixa frequência, quase infrassônico | Loop | Stereo | ⬜ Não | Ativo enquanto Grace está bloqueando a câmera; sub-bass 40–60 Hz |

---

### 3.5 Oliver Crane — "O Artista" (CRIANÇA 5)

> Sons de giz/lápis rabiscando superfícies do ônibus — obsessivo, ritmado, inquietante.
> Contramedida: Microfone (Q). Penalidade: +7s cooldown.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 38 | `sfx_oliver_draw_soft` | `sfx_oliver_draw_soft.ogg` | Rabisco em superfície — giz/carvão em metal, cadência lenta e ritmada | Loop | Mono | ✅ Sim | Oliver está desenhando ativamente; sutileza inicialmente |
| 39 | `sfx_oliver_draw_frantic` | `sfx_oliver_draw_frantic.ogg` | Rabisco frenético — ritmo acelerado, múltiplas superfícies ao mesmo tempo | Loop | Mono | ✅ Sim | Fase avançada do desenho; intensidade proporcional ao medo das crianças vivas |
| 40 | `sfx_oliver_erase` | `sfx_oliver_erase.ogg` | Tentativa de apagar — borracha/palma raspando metal rapidamente | One-shot | Mono | ⬜ Não | Sons quando Oliver responde à intervenção do microfone |
| 41 | `sfx_oliver_repelled` | `sfx_oliver_repelled.ogg` | Oliver parando de desenhar — silêncio súbito, giz caindo no chão | One-shot | Mono | ✅ Sim | Microfone bem-sucedido; o silêncio é o próprio som |
| 42 | `sfx_oliver_children_react` | `sfx_oliver_children_react.ogg` | Reação das crianças vivas ao ver os desenhos — sufoco, cochichos de susto | One-shot | Stereo | ✅ Sim | Momento em que desenhos perturbam os passageiros; diferente de `sfx_thomas_scare_children` — mais contido |

---

## 4. Contramedidas do Motorista

> Sons das ferramentas que o jogador usa para intervir nos ataques das crianças assombradas.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 43 | `sfx_tool_mic_charge` | `sfx_tool_mic_charge.ogg` | Microfone sendo pressionado e seguro (Q hold) — clique inicial | One-shot | Mono | ✅ Sim | Instante de pressionar Q; seguido de `mic_voice` |
| 44 | `sfx_tool_mic_voice` | `sfx_tool_mic_voice.ogg` | Voz do motorista no microfone: *"Sentem nos lugares! AGORA!"* — autoritária, filtro de microfone | One-shot | Stereo | ✅ Sim | Disparado ao completar 3s de hold em Q; qualidade PA de ônibus (saturação leve) |
| 45 | `sfx_tool_mic_feedback` | `sfx_tool_mic_feedback.ogg` | Feedback agudo breve do microfone — chiado de 0,3s | One-shot | Mono | ⬜ Não | Variação aleatória ~20% das ativações; adiciona realismo ao equipamento velho |
| 46 | `sfx_tool_panel_lock` | `sfx_tool_panel_lock.ogg` | Trava do painel engatando — clique mecânico firme + som de solenóide | One-shot | Mono | ✅ Sim | SHIFT click; imediato, deve soar rápido e definitivo |
| 47 | `sfx_tool_panel_unlock` | `sfx_tool_panel_unlock.ogg` | Trava liberando após 3 segundos — clique de desbloqueio | One-shot | Mono | ✅ Sim | Após duração da trava; sinal de que SHIFT pode ser usado novamente |
| 48 | `sfx_tool_radio_tune` | `sfx_tool_radio_tune.ogg` | Motorista mexendo no rádio — dial girando, estática, até encontrar sinal | One-shot | Stereo | ✅ Sim | Tecla R; deve durar ~1.5s antes da música começar |
| 49 | `sfx_tool_radio_playing` | `sfx_tool_radio_playing.ogg` | Música tocando no rádio — melodia simples, quente, contraste com o terror | Loop | Stereo | ✅ Sim | Ver music.md → referência `mus_radio_diegetic`; distorcida levemente por filtro AM |
| 50 | `sfx_tool_headlight_click` | `sfx_tool_headlight_click.ogg` | Clique de acender o farol — interruptor elétrico velho | One-shot | Mono | ⬜ Não | Tecla F; Dias de noite |

---

## 5. UI e Feedback do Sistema

> Sons de interface, cooldowns, estados de jogo e transições.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 51 | `sfx_ui_cooldown_ready` | `sfx_ui_cooldown_ready.ogg` | Ping suave — ferramenta está disponível novamente | One-shot | Mono | ✅ Sim | Som neutro, não intrusivo; volume ~60% dos demais |
| 52 | `sfx_ui_tension_increase` | `sfx_ui_tension_increase.ogg` | Pulso de tensão — grave, breve, como batimento cardíaco acelerado | One-shot | Stereo | ✅ Sim | A cada ganho de tensão significativo (+10%); disparar com delay de 0,3s |
| 53 | `sfx_ui_tension_critical` | `sfx_ui_tension_critical.ogg` | Aviso de tensão alta — tone duplo urgente (75%+) | Loop | Stereo | ✅ Sim | Loop enquanto tensão ≥ 75%; não toca durante cutscenes |
| 54 | `sfx_ui_gameover` | `sfx_ui_gameover.ogg` | Stinger de game over — impacto distorcido, reverb longo, fade | One-shot | Stereo | ✅ Sim | Veja também `mus_gameover` em music.md |
| 55 | `sfx_ui_day_transition` | `sfx_ui_day_transition.ogg` | Som de transição entre dia e noite — relógio + respiração + fade reverb | One-shot | Stereo | ✅ Sim | Tela de transição entre períodos |
| 56 | `sfx_ui_watch_open` | `sfx_ui_watch_open.ogg` | Som de olhar para o relógio (SPACE) — foco, tique-taque breve | One-shot | Mono | ⬜ Não | Exibe tensão e horário; mínimo, não deve quebrar imersão |

---

## 6. Crianças Vivas — Passageiras

> As ~10 crianças vivas que tomam o ônibus. Sons de comportamento normal contrastam com o sobrenatural.
> Função: criar paranoia (quem é real?), medir impacto das assombrações, escalar tensão.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 57 | `sfx_kids_chatter_calm` | `sfx_kids_chatter_calm.ogg` | Murmúrio coletivo de crianças — conversas sobrepostas, relaxadas | Loop | Stereo | ✅ Sim | Estado padrão; volume baixo; som de dia 1–2 |
| 58 | `sfx_kids_chatter_nervous` | `sfx_kids_chatter_nervous.ogg` | Murmúrio nervoso — vozes contidas, sussurros assustados | Loop | Stereo | ✅ Sim | Thomas ou Oliver ativos; tensão 40–60% |
| 59 | `sfx_kids_scream_fear` | `sfx_kids_scream_fear.ogg` | Grito coletivo de susto — breve, genuíno, não ensaiado | One-shot | Stereo | ✅ Sim | Jumpscare controlado ou Thomas fase 3; +tensão imediata |
| 60 | `sfx_kids_scream_panic` | `sfx_kids_scream_panic.ogg` | Grito de pânico total — prolongado, caótico, várias vozes | One-shot | Stereo | ✅ Sim | Thomas atinge 8s sem intervenção; prelúdio de game over |
| 61 | `sfx_kids_whisper_motorist` | `sfx_kids_whisper_motorist.ogg` | Criança sussurra sobre o motorista: *"O motorista é maluco..."* | One-shot | Stereo | ⬜ Não | Dia 1 pós-jumpscare; detalhado no storytelling; 3D posicionado na primeira fileira |
| 62 | `sfx_kids_embark` | `sfx_kids_embark.ogg` | Crianças subindo no ônibus — passos, mochilas batendo, conversas | One-shot | Stereo | ✅ Sim | T próximo ao ponto; sons em camadas |
| 63 | `sfx_kids_disembark` | `sfx_kids_disembark.ogg` | Crianças descendo — passos saindo, porta | One-shot | Stereo | ✅ Sim | T em desembarque; assimetrico ao embarque — mais silêncio depois |
| 64 | `sfx_kids_silence_unnatural` | `sfx_kids_silence_unnatural.ogg` | Silêncio abrupto das crianças vivas — todas param de falar de repente | One-shot | Stereo | ✅ Sim | Momento após jumpscare Dia 1; silêncio processado levemente (reverb curto) |

---

## 7. Ambiente Externo — Dia

> Sons da cidade de Ravenswood e da estrada durante os períodos diurnos (manhã/tarde).
> Tom: normalidade que mascara o horror por baixo.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 65 | `sfx_amb_day_wind_light` | `sfx_amb_day_wind_light.ogg` | Vento leve — brisa urbana passando pelos vidros do ônibus | Loop | Stereo | ✅ Sim | Camada base do ambiente dia; muito sutil |
| 66 | `sfx_amb_day_birds` | `sfx_amb_day_birds.ogg` | Pássaros ao longe — urbanos (pardais, pombos), não floresta | Loop | Stereo | ⬜ Não | Apenas Dias 1–2; ausentes nos dias finais (sutileza narrativa) |
| 67 | `sfx_amb_day_traffic` | `sfx_amb_day_traffic.ogg` | Tráfego distante — carros, buzinas suaves, normalidade | Loop | Stereo | ✅ Sim | Volume reduzido nos dias 4–5 (cidade parece mais vazia) |
| 68 | `sfx_amb_day_school_bell` | `sfx_amb_day_school_bell.ogg` | Sino da escola — metálico, simples, eco curto | One-shot | Stereo | ⬜ Não | Trigger de chegada e saída dos horários 07:00 / 18:00 |
| 69 | `sfx_amb_day_fog_wind` | `sfx_amb_day_fog_wind.ogg` | Vento denso em névoa — mais carregado, úmido, diferente | Loop | Stereo | ✅ Sim | Dias 3–5 quando névoa aumenta; substitui `wind_light` |
| 70 | `sfx_amb_day_gravel_road` | `sfx_amb_day_gravel_road.ogg` | Cascalho e asfalto ruim — ônibus passando por via deteriorada | Loop | Mono | ⬜ Não | Segmentos específicos da rota; textura de solo |

---

## 8. Ambiente Externo — Noite

> Sons de Ravenswood nos períodos noturnos. Tom: isolamento, abandono, presença invisível.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 71 | `sfx_amb_night_crickets` | `sfx_amb_night_crickets.ogg` | Grilos — som de insetos noturnos, contínuo | Loop | Stereo | ✅ Sim | Base do ambiente noturno Dias 1–2; volume cai nos dias finais |
| 72 | `sfx_amb_night_wind_strong` | `sfx_amb_night_wind_strong.ogg` | Vento forte noturno — assobia nos vidros, galhos batendo no teto | Loop | Stereo | ✅ Sim | Ambiente base para todos os períodos noturnos |
| 73 | `sfx_amb_night_branches` | `sfx_amb_night_branches.ogg` | Galhos raspando o exterior do ônibus — rangido irregular | One-shot | Stereo | ⬜ Não | Eventos aleatórios em trechos arborizados; assusta o jogador |
| 74 | `sfx_amb_night_silence_heavy` | `sfx_amb_night_silence_heavy.ogg` | "Silêncio pesado" — todas as camadas abaixam, drone sub-bass sutil | Loop | Stereo | ✅ Sim | Momentos de clímax de tensão noturna; não é ausência de som, é presença do nada |
| 75 | `sfx_amb_night_distant_dog` | `sfx_amb_night_distant_dog.ogg` | Cachorro uivando ao longe — soa perdido, triste, distante | One-shot | Stereo | ⬜ Não | Elemento de isolamento; dias 3–5 |
| 76 | `sfx_amb_night_road_gorge` | `sfx_amb_night_road_gorge.ogg` | Som específico da Ravenswood Gorge Road — vento no precipício, eco surreal | Loop | Stereo | ✅ Sim | Segmento especial da estrada onde o acidente aconteceu; só noite |
| 77 | `sfx_amb_night_radio_static` | `sfx_amb_night_radio_static.ogg` | Estática do rádio — interferência noturna, crackling suave | Loop | Mono | ✅ Sim | Rádio desligado mas chiando sutilmente; reforça horror diegético |
| 78 | `sfx_amb_night_thunder_distant` | `sfx_amb_night_thunder_distant.ogg` | Trovão ao longe — sem chuva, só eletricidade no ar | One-shot | Stereo | ⬜ Não | Momentos dramáticos; Dias 4–5 |

---

## 9. Sons Narrativos Especiais

> SFX vinculados diretamente a eventos da história. Não são genéricos — cada um tem contexto específico.

| # | ID | Nome do Arquivo | Descrição | Tipo | Canal | MVP | Notas |
|---|----|----------------|-----------|------|-------|-----|-------|
| 79 | `sfx_narrative_victor_voice` | `sfx_narrative_victor_voice.ogg` | Voz de Victor Graves pelo rádio: *"Não continue... saia enquanto pode..."* — distante, eco, filtro de rádio antigo | One-shot | Stereo | ✅ Sim | Dia 2; evento único; voz masculina adulta, cansada |
| 80 | `sfx_narrative_accident_echo` | `sfx_narrative_accident_echo.ogg` | Eco sonoro do acidente de 1998 — metal, vidro, silêncio | One-shot | Stereo | ✅ Sim | Final C (O Ciclo); idêntico ao `sfx_bus_collision` mas com reverb de canyon + eco longo |
| 81 | `sfx_narrative_key_throw` | `sfx_narrative_key_throw.ogg` | Chaves do ônibus sendo jogadas nas mãos — clique metálico | One-shot | Mono | ⬜ Não | Cena de intro Dia 1 com Siqueira |
| 82 | `sfx_narrative_photo_reveal` | `sfx_narrative_photo_reveal.ogg` | Momento de revelação no mural — batida de coração, chiado leve | One-shot | Stereo | ✅ Sim | Dia 5: jogador vê fotos das crianças mortas; impacto emocional máximo |
| 83 | `sfx_narrative_diary_pages` | `sfx_narrative_diary_pages.ogg` | Páginas do diário de Victor sendo folheadas — papel velho | One-shot | Mono | ⬜ Não | Dia 4: descoberta do diário sob o banco |
| 84 | `sfx_narrative_ending_walk` | `sfx_narrative_ending_walk.ogg` | Passos saindo do ônibus para a estrada escura — Final A (Fuga) | One-shot | Stereo | ✅ Sim | Motorista descendo e caminhando; grave, resoluto, sem pressa |
| 85 | `sfx_narrative_spirits_manifest` | `sfx_narrative_spirits_manifest.ogg` | Cinco espíritos se manifestando simultaneamente — camadas de todos os sons dos fantasmas juntos | One-shot | Stereo | ✅ Sim | Cutscene final (todos os 3 finais) — só difere no mix |

---

## Sumário de Produção

### Contagem por Categoria

| Categoria | Quantidade | MVP | Opcionais |
|-----------|-----------|-----|-----------|
| Motor do Ônibus | 10 | 8 | 2 |
| Portas | 6 | 5 | 1 |
| Emma Lynch | 6 | 6 | 0 |
| Thomas Sanders | 6 | 6 | 0 |
| Marcus Reid | 5 | 4 | 1 |
| Grace Harper | 4 | 2 | 2 |
| Oliver Crane | 5 | 4 | 1 |
| Contramedidas | 8 | 7 | 1 |
| UI / Feedback | 6 | 4 | 2 |
| Crianças Vivas | 8 | 6 | 2 |
| Ambiente Dia | 6 | 3 | 3 |
| Ambiente Noite | 8 | 5 | 3 |
| Narrativos Especiais | 7 | 5 | 2 |
| **TOTAL** | **85** | **65** | **20** |

---

### Checklist de Produção

#### 🚌 Motor do Ônibus
- [ ] `sfx_bus_idle_low`
- [ ] `sfx_bus_idle_high`
- [ ] `sfx_bus_accelerate`
- [ ] `sfx_bus_decelerate`
- [ ] `sfx_bus_brake_hard`
- [ ] `sfx_bus_reverse`
- [ ] `sfx_bus_ignition_start`
- [ ] `sfx_bus_ignition_fail`
- [ ] `sfx_bus_engine_sputter`
- [ ] `sfx_bus_collision`

#### 🚪 Portas
- [ ] `sfx_door_open_normal`
- [ ] `sfx_door_close_normal`
- [ ] `sfx_door_mechanism_creak`
- [ ] `sfx_door_open_ghost`
- [ ] `sfx_door_panel_button`
- [ ] `sfx_door_lock_engage`

#### 👻 Emma Lynch
- [ ] `sfx_emma_laugh_low`
- [ ] `sfx_emma_laugh_mid`
- [ ] `sfx_emma_laugh_high`
- [ ] `sfx_emma_appear`
- [ ] `sfx_emma_attack_success`
- [ ] `sfx_emma_repelled`

#### 👻 Thomas Sanders
- [ ] `sfx_thomas_whisper_low`
- [ ] `sfx_thomas_whisper_mid`
- [ ] `sfx_thomas_whisper_critical`
- [ ] `sfx_thomas_narrative_radio`
- [ ] `sfx_thomas_scare_children`
- [ ] `sfx_thomas_silenced`

#### 👻 Marcus Reid
- [ ] `sfx_marcus_move_seat`
- [ ] `sfx_marcus_footstep`
- [ ] `sfx_marcus_teleport`
- [ ] `sfx_marcus_breathe`
- [ ] `sfx_marcus_repelled`

#### 👻 Grace Harper
- [ ] `sfx_grace_camera_static`
- [ ] `sfx_grace_camera_glitch`
- [ ] `sfx_grace_silence`
- [ ] `sfx_grace_stare`

#### 👻 Oliver Crane
- [ ] `sfx_oliver_draw_soft`
- [ ] `sfx_oliver_draw_frantic`
- [ ] `sfx_oliver_erase`
- [ ] `sfx_oliver_repelled`
- [ ] `sfx_oliver_children_react`

#### 🎙️ Contramedidas
- [ ] `sfx_tool_mic_charge`
- [ ] `sfx_tool_mic_voice`
- [ ] `sfx_tool_mic_feedback`
- [ ] `sfx_tool_panel_lock`
- [ ] `sfx_tool_panel_unlock`
- [ ] `sfx_tool_radio_tune`
- [ ] `sfx_tool_radio_playing`
- [ ] `sfx_tool_headlight_click`

#### 🖥️ UI / Feedback
- [ ] `sfx_ui_cooldown_ready`
- [ ] `sfx_ui_tension_increase`
- [ ] `sfx_ui_tension_critical`
- [ ] `sfx_ui_gameover`
- [ ] `sfx_ui_day_transition`
- [ ] `sfx_ui_watch_open`

#### 🧒 Crianças Vivas
- [ ] `sfx_kids_chatter_calm`
- [ ] `sfx_kids_chatter_nervous`
- [ ] `sfx_kids_scream_fear`
- [ ] `sfx_kids_scream_panic`
- [ ] `sfx_kids_whisper_motorist`
- [ ] `sfx_kids_embark`
- [ ] `sfx_kids_disembark`
- [ ] `sfx_kids_silence_unnatural`

#### ☀️ Ambiente Dia
- [ ] `sfx_amb_day_wind_light`
- [ ] `sfx_amb_day_birds`
- [ ] `sfx_amb_day_traffic`
- [ ] `sfx_amb_day_school_bell`
- [ ] `sfx_amb_day_fog_wind`
- [ ] `sfx_amb_day_gravel_road`

#### 🌙 Ambiente Noite
- [ ] `sfx_amb_night_crickets`
- [ ] `sfx_amb_night_wind_strong`
- [ ] `sfx_amb_night_branches`
- [ ] `sfx_amb_night_silence_heavy`
- [ ] `sfx_amb_night_distant_dog`
- [ ] `sfx_amb_night_road_gorge`
- [ ] `sfx_amb_night_radio_static`
- [ ] `sfx_amb_night_thunder_distant`

#### 📖 Narrativos Especiais
- [ ] `sfx_narrative_victor_voice`
- [ ] `sfx_narrative_accident_echo`
- [ ] `sfx_narrative_key_throw`
- [ ] `sfx_narrative_photo_reveal`
- [ ] `sfx_narrative_diary_pages`
- [ ] `sfx_narrative_ending_walk`
- [ ] `sfx_narrative_spirits_manifest`

---

## Notas de Implementação Unity 6

### AudioMixer Groups recomendados

```
Master
├── Music         (trilha adaptativa)
├── SFX
│   ├── Bus       (motor, portas)
│   ├── Ghosts    (todos os fantasmas)
│   ├── Tools     (contramedidas)
│   └── Ambient   (dia/noite)
└── Voice
    ├── Narrative (Victor, Siqueira)
    └── Kids      (crianças vivas + assombradas)
```

### Parâmetros de exposição no AudioMixer (para scripting)

| Parâmetro | Uso |
|-----------|-----|
| `TensionLevel` | 0.0–1.0 → controla volume/pitch de ghosts e ambient |
| `BusEngineRPM` | 0.0–1.0 → controla pitch dos loops do motor |
| `NightMultiplier` | 0.0 (dia) / 1.0 (noite) → altera camadas ambientes |
| `CameraStaticLevel` | 0.0–1.0 → intensidade do chiado da Grace |

### Áudio Posicional 3D

Todos os sons de fantasmas devem usar **Audio Source 3D** com as seguintes configurações:
- Spatial Blend: **1.0** (full 3D)
- Volume Rolloff: **Logarithmic** (comportamento realista de espaço fechado)
- Min/Max Distance: **0.5m / 15m** (espaço do ônibus)

Sons de UI e música: Spatial Blend **0.0** (2D global).

---

*Documentação de SFX — Bus Shift | Ravenswood, 1998*

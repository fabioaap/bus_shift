# Bus Shift — Inventário de Assets

**Versão:** 1.0  
**Data:** 2026  
**Responsável:** Direção de Arte / Equipe de Assets  
**Status:** 🟡 Em Definição  
**Issue:** #6

> **Como usar este documento:** Cada asset possui checkbox `[ ]`. Marque `[x]` quando o asset estiver concluído e entregue para integração. Colunas de Prioridade: **MVP** = necessário para a demo jogável | **Polish** = necessário para o release | **Optional** = nice-to-have pós-release.

---

## Índice

1. [Modelos 3D — Veículo](#1-modelos-3d--veículo)
2. [Modelos 3D — Personagens](#2-modelos-3d--personagens)
3. [Modelos 3D — Ambiente](#3-modelos-3d--ambiente)
4. [Modelos 3D — Props e Interativos](#4-modelos-3d--props-e-interativos)
5. [Texturas](#5-texturas)
6. [Animações](#6-animações)
7. [UI Elements](#7-ui-elements)
8. [Partículas e VFX](#8-partículas-e-vfx)
9. [Resumo de Escopo](#9-resumo-de-escopo)

---

## 1. Modelos 3D — Veículo

> **Referência obrigatória:** `docs/game/references/onibus-104-visual.md` e `docs/game/references/interior-onibus-pov-jogador.md`  
> **Regra:** Ônibus deve parecer "velho e doente". Ferrugem visível, pintura descascando. Cor base `#C8A84B` → oxidação `#A07830` → ferrugem `#7A3B1E`.

---

### 1.1 Ônibus 104 — Exterior

| Campo | Valor |
|-------|-------|
| **Nome** | `bus_104_exterior` |
| **Descrição** | Carroceria completa do ônibus escolar anos 90, amarelo desbotado/oxidado, janelas laterais, porta dianteira e traseira, faróis, lanternas traseiras. Visto em cutscenes e transições de cena. |
| **Poly Count** | 2.000 – 3.500 triângulos |
| **LOD** | LOD0: 3.500 tri | LOD1: 1.200 tri (cutscenes distantes) |
| **Prioridade** | **MVP** |
| **Dependências** | Textura diffuse `bus_exterior_diffuse` (2K), textura emissive para faróis `bus_headlights_emissive` (1K), rig simples (portas) |
| **Estimativa** | 18–24h de modelagem + 6h texturização |
| **Status** | [ ] Em fila |

**Notas de Arte:**
- Separar geometria dos faróis como sub-mesh para animação de liga/desliga (tecla F)
- Porta dianteira deve ter pivot correto para animação open/close
- Ferrugem concentrada nas bordas inferiores, rodas e painel lateral
- Número "104" em plano geométrico simples, sem texture atlas complexo

---

### 1.2 Ônibus 104 — Interior Completo (Cockpit + Corredor)

| Campo | Valor |
|-------|-------|
| **Nome** | `bus_104_interior` |
| **Descrição** | Interior completo: cockpit do motorista (volante, painel, assentos), corredor central, 10 fileiras de bancos duplos, janelas laterais, teto, piso desgastado, corrimãos. Este é o espaço de 95% do gameplay. |
| **Poly Count** | 3.000 – 5.000 triângulos total |
| **LOD** | Não aplicável (sempre em câmera 1ª pessoa, distâncias curtas) |
| **Prioridade** | **MVP** |
| **Dependências** | Textura diffuse interior `bus_interior_diffuse` (2K), materiais separados por zona (cockpit / corredor / janelas), iluminação baked para estado normal |
| **Estimativa** | 24–32h de modelagem + 8h texturização + 4h setup de iluminação |
| **Status** | [ ] Em fila |

**Notas de Arte:**
- Cockpit deve ter todos os props interativos como sub-meshes separados (ver Seção 4)
- Corredor: escuridão gradiente em direção ao fundo — a iluminação decide o horror
- Vidro frontal: material semi-transparente com sujeira/embaçamento via shader
- Retrovisor interno: posicionado para que POV do jogador veja exatamente o corredor
- Piso: vinil desgastado, marcas de pisada, levemente reflexivo

---

## 2. Modelos 3D — Personagens

> **Regra Geral Fantasmas:** Opacidade mesh 60–75% (shader translúcido). Sem normal maps. Flat shading. Fresnel edge glow em `#B8C8D8`. Cabeça proporcionalmente grande (ratio 1:4). Olhos sem íris — só branco `#E8F0F8`.  
> **Regra Motorista:** Aparência progride visualmente ao longo dos 5 dias (ver style-guide seção 5.3). Apenas mãos + parcial torso visíveis em gameplay 1ª pessoa.

---

### 2.1 Ray Morgan — Motorista (Mãos / 1ª Pessoa)

| Campo | Valor |
|-------|-------|
| **Nome** | `ray_morgan_firstperson` |
| **Descrição** | Apenas mãos e parte dos antebraços visíveis (POV 1ª pessoa). Farda de motorista, manga comprida. Variações de material por dia (5 estados de deterioração). |
| **Poly Count** | 800 – 1.200 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura diffuse `ray_hands_day1-5` (5 variações, 1K cada), rig de mãos (bones de dedos para animações de interação), blendshapes não necessários |
| **Estimativa** | 8h modelagem + 4h rig + 6h variações de material (5 dias) |
| **Status** | [ ] Em fila |

**Notas de Arte:**
- Dia 1: mãos limpas, farda cor `#8B7355`
- Dia 5: mãos com leve tremor (animação), manchas escuras `#1A0E08`
- Unhas visíveis mas simples (faces planas)

---

### 2.2 Ray Morgan — Corpo Completo (Cutscenes / Game Over)

| Campo | Valor |
|-------|-------|
| **Nome** | `ray_morgan_fullbody` |
| **Descrição** | Modelo completo do motorista para uso em cutscenes de início/fim de dia, telas de game over e créditos. |
| **Poly Count** | 1.500 – 2.500 triângulos |
| **LOD** | LOD1: 600 tri |
| **Prioridade** | **Polish** |
| **Dependências** | Rig completo (humano, ~30 bones), textura `ray_body_diffuse` (1K), 5 variações de material por dia |
| **Estimativa** | 14h modelagem + 6h rig + 4h variações |
| **Status** | [ ] Em fila |

---

### 2.3 Marcus Reid — "O Invasor" (Fantasma)

| Campo | Valor |
|-------|-------|
| **Nome** | `ghost_marcus_reid` |
| **Descrição** | Criança fantasma masculino, ~10 anos, aparência anos 90. Jaqueta bomber azul sobre camiseta, número no peito (silhueta geométrica), tênis com stripes. Postura levemente curvada, ombros caídos. |
| **Poly Count** | 600 – 900 triângulos |
| **LOD** | LOD1: 300 tri |
| **Prioridade** | **MVP** |
| **Dependências** | Shader translúcido com Fresnel glow, textura flat `ghost_marcus_diffuse` (512), partículas de gotículas (sistema compartilhado), ponto de luz azul embutido `#6A80A0` |
| **Estimativa** | 8h modelagem + 3h shader setup |
| **Status** | [ ] Em fila |

**Mecânica:** Teleporta de assento em assento em direção ao cockpit. Não caminha — aparece/desaparece.  
**Contramedida:** Microfone (tecla Q)

---

### 2.4 Emma Lynch — "A Burlona" (Fantasma)

| Campo | Valor |
|-------|-------|
| **Nome** | `ghost_emma_lynch` |
| **Descrição** | Criança fantasma feminina, ~9 anos. Vestido xadrez anos 90 (padrão geométrico simplificado), meia alta, laço no cabelo levemente torto. Sorrisão perturbador. |
| **Poly Count** | 600 – 900 triângulos |
| **LOD** | LOD1: 300 tri |
| **Prioridade** | **MVP** |
| **Dependências** | Shader translúcido, textura flat `ghost_emma_diffuse` (512), rig com bones de braço (animação de alcançar botões) |
| **Estimativa** | 8h modelagem + 3h shader + 2h rig específico |
| **Status** | [ ] Em fila |

**Mecânica:** Corre em direção ao painel de controle para apertar botões aleatórios.  
**Contramedida:** Trava do painel (tecla SHIFT)

---

### 2.5 Thomas Sanders — "O Narrador" (Fantasma)

| Campo | Valor |
|-------|-------|
| **Nome** | `ghost_thomas_sanders` |
| **Descrição** | Criança fantasma masculino, ~11 anos. Camiseta de banda (silhueta simples no peito), calça cargo, distintivo escolar no pescoço. Boca aberta em silêncio perturbador, cavidade escura `#080C10`. |
| **Poly Count** | 600 – 900 triângulos |
| **LOD** | LOD1: 300 tri |
| **Prioridade** | **MVP** |
| **Dependências** | Shader translúcido, textura flat `ghost_thomas_diffuse` (512), sem rig complexo (flutua, movimentos mínimos) |
| **Estimativa** | 7h modelagem + 3h shader |
| **Status** | [ ] Em fila |

**Mecânica:** Flutua pelo corredor sussurrando. Aumenta tensão passivamente se ignorado.  
**Contramedida:** Rádio (tecla R)

---

### 2.6 Grace Harper — "A Observadora" (Fantasma)

| Campo | Valor |
|-------|-------|
| **Nome** | `ghost_grace_harper` |
| **Descrição** | Criança fantasma feminina, ~10 anos. Moletom com capuz (capuz levantado, rosto semi-oculto), calça jeans, mochila escolar sempre nas costas. Postura completamente imóvel exceto por micro-movimentos de cabeça. |
| **Poly Count** | 600 – 900 triângulos |
| **LOD** | LOD1: 300 tri |
| **Prioridade** | **MVP** |
| **Dependências** | Shader translúcido, textura flat `ghost_grace_diffuse` (512), rig minimal (pescoço/cabeça para tracking do retrovisor), material emissive de olhos |
| **Estimativa** | 8h modelagem + 3h shader + 2h rig de cabeça |
| **Status** | [ ] Em fila |

**Mecânica:** Aparece apenas no retrovisor, olhando direto para o jogador. Invisível fora do retrovisor.  
**Contramedida:** Câmera de segurança (tecla C)

---

### 2.7 Oliver Crane — "O Artista" (Fantasma)

| Campo | Valor |
|-------|-------|
| **Nome** | `ghost_oliver_crane` |
| **Descrição** | Criança fantasma masculino, ~10 anos. Moletom com manchas de tinta (vertex color diferenciado), dedos marcados de tinta, lápis na orelha, caderno na mão esquerda. |
| **Poly Count** | 600 – 900 triângulos |
| **LOD** | LOD1: 300 tri |
| **Prioridade** | **MVP** |
| **Dependências** | Shader translúcido, textura flat `ghost_oliver_diffuse` (512), prop do caderno como sub-mesh, rig de mão direita (animação de escrita) |
| **Estimativa** | 9h modelagem + 3h shader + 3h rig mão |
| **Status** | [ ] Em fila |

**Mecânica:** Aparece e "desenha" mensagens no vidro/painel. Cada desenho aumenta tensão gradualmente.  
**Contramedida:** Rádio (tecla R) — música "apaga" as mensagens.

---

### 2.8 Diretor Harrison Siqueira Stone

| Campo | Valor |
|-------|-------|
| **Nome** | `npc_director_stone` |
| **Descrição** | Adulto, ~55 anos, postura ereta imperturbável. Terno azul-marinho `#1A1A2A`, gravata `#8A2020`, cabelo branco `#D8D4CC`, óculos metálicos finos. Expressão neutra, nunca relaxada. |
| **Poly Count** | 1.200 – 1.800 triângulos |
| **LOD** | LOD1: 500 tri |
| **Prioridade** | **Polish** *(MVP se cutscenes de dia 1 forem incluídas na demo)* |
| **Dependências** | Rig humano completo, textura `director_diffuse` (1K), shader normal (não fantasma) |
| **Estimativa** | 12h modelagem + 5h rig |
| **Status** | [ ] Em fila |

---

### 2.9 Earl Hayes — Mecânico

| Campo | Valor |
|-------|-------|
| **Nome** | `npc_mechanic_hayes` |
| **Descrição** | Adulto idoso, ~65 anos, postura curvada (contraste intencional com o Diretor). Macacão preto de graxa `#2A2820`, mãos com vertex color de óleo `#0A0808`, barba branca desalinhada, boné puído. |
| **Poly Count** | 1.000 – 1.500 triângulos |
| **LOD** | LOD1: 500 tri |
| **Prioridade** | **Polish** |
| **Dependências** | Rig humano completo, textura `mechanic_diffuse` (1K) |
| **Estimativa** | 10h modelagem + 5h rig |
| **Status** | [ ] Em fila |

---

### 2.10 Criança Viva — Mesh Base (NPC)

| Campo | Valor |
|-------|-------|
| **Nome** | `child_npc_base` |
| **Descrição** | Mesh base de criança (~8-11 anos) para instanciação. ~10 instâncias simultâneas no ônibus com variações de cor de material (swap de material, não de mesh). Aparência genérica, sem detalhes individuais. |
| **Poly Count** | 300 – 500 triângulos |
| **LOD** | LOD1: 150 tri (crianças nos bancos do fundo) |
| **Prioridade** | **MVP** |
| **Dependências** | Rig humano simplificado (~15 bones), 4–5 variações de material `child_mat_A/B/C/D/E` (512 cada), animações idle compartilhadas |
| **Estimativa** | 6h modelagem base + 2h rig + 2h variações de material |
| **Status** | [ ] Em fila |

**Nota:** NÃO criar modelos únicos por criança. Usar instâncias do mesmo mesh com material swap. Performance crítica (~10 instâncias na cena).

---

## 3. Modelos 3D — Ambiente

> **Estilo:** Low poly estilizado. Faces planas bem-vindas. Sem subdivisão desnecessária.  
> **Paleta:** Tons frios dessaturados para dia. Azul-marinho escuro para noite.

---

### 3.1 Ravenswood Elementary School — Fachada

| Campo | Valor |
|-------|-------|
| **Nome** | `env_school_ravenswood` |
| **Descrição** | Fachada frontal da escola: prédio de 2 andares, estilo anos 70-80, tijolo desgastado, janelas grades metálicas, porta principal dupla, placa da escola, escadas de entrada. Não é necessário interior. |
| **Poly Count** | 2.500 – 4.000 triângulos |
| **LOD** | LOD1: 1.000 tri (cenas distantes) |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `school_diffuse` (2K), textura `school_windows_emissive` (1K — janelas iluminadas à noite) |
| **Estimativa** | 16h modelagem + 6h texturização |
| **Status** | [ ] Em fila |

---

### 3.2 Estacionamento da Escola

| Campo | Valor |
|-------|-------|
| **Nome** | `env_school_parking` |
| **Descrição** | Área plana de asfalto desgastado com marcações apagadas, postes de luz, mureta baixa lateral, calçada. Local da cutscene inicial (Dia 1). |
| **Poly Count** | 800 – 1.200 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `parking_diffuse` (1K), postes de luz como sub-objetos separados |
| **Estimativa** | 6h modelagem + 3h texturização |
| **Status** | [ ] Em fila |

---

### 3.3 Segmento de Estrada (Tile)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_road_segment_straight` / `env_road_segment_curve` |
| **Descrição** | Tiles reutilizáveis de estrada asfaltada: segmento reto (15m) e curva (90°). Sistema de tiles para compor a rota completa de 5 pontos. |
| **Poly Count** | 400 – 800 triângulos por tile |
| **Prioridade** | **MVP** |
| **Dependências** | Textura tileável `road_diffuse` (1K), marcações de pista apagadas no material |
| **Estimativa** | 4h modelagem (ambos os tiles) + 2h texturização |
| **Status** | [ ] Em fila |

---

### 3.4 Ponto de Ônibus (x5)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_bus_stop` |
| **Descrição** | Estrutura de parada de ônibus anos 90: telhado simples, poste com sinal, banco de espera (opcional). Um modelo base reutilizado nos 5 pontos com variações mínimas de desgaste. |
| **Poly Count** | 300 – 500 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `bus_stop_diffuse` (512), sinal como sub-mesh para variação de numeração |
| **Estimativa** | 4h modelagem + 2h variações |
| **Status** | [ ] Em fila |

---

### 3.5 Casas de Fundo (Residencial)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_house_type_A` / `env_house_type_B` / `env_house_type_C` |
| **Descrição** | 3 variações de casa residencial para compor o pano de fundo da rota. Apenas fachada + telhado. Sem interior. Estilo subúrbio americano anos 90, levemente deterioradas. |
| **Poly Count** | 200 – 600 triângulos por variação |
| **Prioridade** | **MVP** |
| **Dependências** | Textura compartilhada `house_diffuse` (1K, atlas com as 3 variações), material emissive `house_windows_night` (janelas iluminadas) |
| **Estimativa** | 6h modelagem (3 tipos) + 3h texturização |
| **Status** | [ ] Em fila |

---

### 3.6 Árvore Morta (Sem Folhas)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_tree_dead` (variações: `_A`, `_B`, `_C`) |
| **Descrição** | Árvore sem folhas, galhos angulosos e nus. Atmosfera de inverno/morte. 3 variações de silhueta para não repetição visual. Usadas densamente ao longo da rota, especialmente à noite. |
| **Poly Count** | 150 – 300 triângulos por variação |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `tree_diffuse` (512, atlas 3 variações) |
| **Estimativa** | 4h modelagem (3 variações) + 1h texturização |
| **Status** | [ ] Em fila |

---

### 3.7 Poste de Luz (Sódio Antigo)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_streetlight_sodium` |
| **Descrição** | Poste de luz estilo anos 90, luminária de sódio (luz laranja `#C87832`). Usado ao longo da rota para criar "ilhas de luz" na escuridão noturna. |
| **Poly Count** | 150 – 250 triângulos |
| **Prioridade** | **Polish** |
| **Dependências** | Textura `streetlight_diffuse` (512), material emissive da luminária, Point Light embutido (raio ~8m) |
| **Estimativa** | 3h modelagem + 1h setup de luz |
| **Status** | [ ] Em fila |

---

### 3.8 Igreja (Pano de Fundo)

| Campo | Valor |
|-------|-------|
| **Nome** | `env_church_background` |
| **Descrição** | Igreja pequena com cruz no topo, estilo americano. Vista de fundo na rota. Sem detalhes internos. Referência: mapa da vila (`mapa-vila-visual.md`). |
| **Poly Count** | 400 – 800 triângulos |
| **Prioridade** | **Optional** |
| **Dependências** | Textura `church_diffuse` (512) |
| **Estimativa** | 5h modelagem + 2h texturização |
| **Status** | [ ] Em fila |

---

### 3.9 Skybox / Céu

| Campo | Valor |
|-------|-------|
| **Nome** | `skybox_overcast_day` / `skybox_night_fog` |
| **Descrição** | Duas versões de skybox: dia nublado opressivo (cinza pesado, sem sol visível) e noite com neblina densa (azul-marinho quase preto). Unity skybox material, não modelo 3D. |
| **Prioridade** | **MVP** |
| **Dependências** | Shader de skybox customizado URP, integração com sistema de fog |
| **Estimativa** | 4h cada |
| **Status** | [ ] Em fila |

---

## 4. Modelos 3D — Props e Interativos

> **Todos os props do cockpit devem ter pivot points corretos para animação de interação.**  
> **Referência de proporção:** Interior do ônibus (`interior-onibus-pov-jogador.md`).

---

### 4.1 Painel de Controle do Motorista

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_dashboard` |
| **Descrição** | Painel completo do cockpit: conjunto de botões, chaves, indicadores analógicos, velocímetro (anos 90). Desgastado, plástico amarelado. Centro das interações SHIFT (trava) e visibilidade geral do cockpit. |
| **Poly Count** | 600 – 1.000 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `dashboard_diffuse` (1K), material emissive para LEDs de status, sub-mesh para botão de trava (animação SHIFT) |
| **Estimativa** | 8h modelagem + 4h texturização |
| **Status** | [ ] Em fila |

---

### 4.2 Volante

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_steering_wheel` |
| **Descrição** | Volante clássico de ônibus, diâmetro grande, desgastado. Rotação nos eixos A/D do jogador. |
| **Poly Count** | 200 – 350 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `steering_wheel_diffuse` (512), rig de rotação (pivot no centro) |
| **Estimativa** | 4h modelagem + 2h texturização + 1h rig |
| **Status** | [ ] Em fila |

---

### 4.3 Espelho Retrovisor Interno

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_rearview_mirror` |
| **Descrição** | Espelho retrovisor central do ônibus. Superfície refletiva (Render Texture ou Camera Preview). Levemente distorcido, mancha permanente que não sai. Crítico para aparição de Grace Harper. |
| **Poly Count** | 80 – 150 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Material de espelho (Render Texture — câmera secundária aponta para corredor), shader de distorção sutil, textura `mirror_dirt_overlay` (512) |
| **Estimativa** | 3h modelagem + 4h setup de Render Texture + 2h shader |
| **Status** | [ ] Em fila |

**Nota técnica:** A câmera do retrovisor é uma câmera Unity secundária renderizando no material do espelho. Grace Harper só aparece nesta câmera.

---

### 4.4 Monitor de Câmera de Segurança (Televisor)

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_security_monitor` |
| **Descrição** | Televisor CRT pequeno (estilo anos 90) preso ao painel do motorista. Exibe a câmera de segurança do corredor com ruído (grain, scan lines). Interagível via tecla C. |
| **Poly Count** | 150 – 250 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Material de tela com Render Texture, shader de ruído CRT (scan lines, grain, cor verde-fósforo `#30A830`), animação de "tapinha" (interação C) |
| **Estimativa** | 4h modelagem + 5h shader CRT + 2h animação |
| **Status** | [ ] Em fila |

---

### 4.5 Rádio do Ônibus

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_bus_radio` |
| **Descrição** | Rádio analógico embutido no painel, botões e dial de sintonia, antena. Anos 90. Interagível via tecla R. |
| **Poly Count** | 100 – 200 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `radio_diffuse` (512), material emissive para LED de "ON", animação de giro do dial |
| **Estimativa** | 4h modelagem + 2h texturização |
| **Status** | [ ] Em fila |

---

### 4.6 Microfone de PA (Sistema de Comunicação Interna)

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_pa_microphone` |
| **Descrição** | Microfone de suporte preso ao painel (tipo rádio policial anos 90), cabo espiral. Interagível via tecla Q (segurar 3s). |
| **Poly Count** | 80 – 150 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `microphone_diffuse` (512), animação de pegar/soltar (hold Q) |
| **Estimativa** | 3h modelagem + 2h texturização + 1h animação |
| **Status** | [ ] Em fila |

---

### 4.7 Relógio de Pulso

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_wristwatch` |
| **Descrição** | Relógio analógico no pulso do motorista (visível ao pressionar SPACE). Face simples, pulseira de couro desgastada. Exibe hora atual do jogo + indicador visual de tensão (a ponte entre o HUD e o mundo diegético). |
| **Poly Count** | 60 – 100 triângulos |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `watch_diffuse` (512), material emissive para ponteiros (leve), animação de olhar para o pulso (câmera + braço) |
| **Estimativa** | 3h modelagem + 2h texturização + 2h animação de câmera |
| **Status** | [ ] Em fila |

---

### 4.8 Mapa (Prop Físico)

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_route_map` |
| **Descrição** | Mapa impresso em papel envelhecido/queimado nas bordas, estilo vintage horror. Aparece como overlay ao pressionar TAB (bloqueia parte da visão). Referência: `mapa-vila-visual.md`. |
| **Poly Count** | 30 – 60 triângulos (apenas um plano com material) |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `map_base` (2K — detalhe importante de UI), textura `map_route_overlay` (1K — rota em vermelho dinâmica por dia), animação de abrir/fechar o mapa |
| **Estimativa** | 1h modelagem + 4h arte da textura do mapa + 2h sistema de overlay dinâmico |
| **Status** | [ ] Em fila |

---

### 4.9 Bancos do Ônibus (Por Fileira)

| Campo | Valor |
|-------|-------|
| **Nome** | `prop_bus_seat_row` |
| **Descrição** | Banco duplo de ônibus escolar (vinil verde/marrom desgastado), encosto metálico. ~10 fileiras no corredor. Instanciados — um modelo, múltiplas instâncias. |
| **Poly Count** | 150 – 250 triângulos por fileira |
| **Prioridade** | **MVP** |
| **Dependências** | Textura `seat_diffuse` (512), 2 variações de material (desgaste diferente) |
| **Estimativa** | 4h modelagem + 2h variações |
| **Status** | [ ] Em fila |

---

### 4.10 Janelas do Ônibus (Material)

| Campo | Valor |
|-------|-------|
| **Nome** | `mat_bus_window` |
| **Descrição** | Material de vidro semi-transparente para janelas laterais do ônibus. Sujo, embaçado, levemente reflexivo. Não é um modelo separado — é o material aplicado à geometria das janelas já no interior. |
| **Poly Count** | 30 – 50 triângulos por janela (já incluído no `bus_104_interior`) |
| **Prioridade** | **MVP** |
| **Dependências** | Shader URP com transparência, textura de sujeira `window_dirt_overlay` (512), reflexo environment probe |
| **Estimativa** | 3h shader setup |
| **Status** | [ ] Em fila |

---

## 5. Texturas

> **Nomenclatura padrão:** `[objeto]_[tipo].[ext]` — ex: `bus_exterior_diffuse.png`  
> **Formatos:** PNG para diffuse/emissive, EXR para normal maps  
> **Paleta:** Seguir rigorosamente as swatches do style-guide (seção 2)

---

### 5.1 Texturas de Veículo

| Asset | Tipo | Resolução | Prioridade | Notas |
|-------|------|-----------|------------|-------|
| `bus_exterior_diffuse` | Diffuse | 2K | MVP | Amarelo desbotado `#C8A84B`, ferrugem `#7A3B1E`, sujeira |
| `bus_exterior_roughness` | Roughness | 1K | Polish | Alta rugosidade nas ferrugem, baixa no vidro |
| `bus_headlights_emissive` | Emissive | 1K | MVP | Ligado/desligado por material swap |
| `bus_interior_diffuse` | Diffuse | 2K | MVP | Vinil desgastado, metal envelhecido, plastico amarelado |
| `bus_interior_dirt_overlay` | Overlay | 1K | Polish | Camada de sujeira adicional |
| `window_dirt_overlay` | Overlay | 512 | MVP | Sujeira e embaçamento do vidro |

---

### 5.2 Texturas de Personagens

| Asset | Tipo | Resolução | Prioridade | Notas |
|-------|------|-----------|------------|-------|
| `ray_hands_day1` → `day5` | Diffuse | 1K × 5 | MVP | Deterioração progressiva |
| `ray_body_diffuse` | Diffuse | 1K | Polish | Corpo completo para cutscenes |
| `ghost_marcus_diffuse` | Diffuse | 512 | MVP | Flat shading, sem detalhe excessivo |
| `ghost_emma_diffuse` | Diffuse | 512 | MVP | Flat shading |
| `ghost_thomas_diffuse` | Diffuse | 512 | MVP | Flat shading |
| `ghost_grace_diffuse` | Diffuse | 512 | MVP | Flat shading |
| `ghost_oliver_diffuse` | Diffuse | 512 | MVP | Manchas de tinta como vertex color |
| `director_diffuse` | Diffuse | 1K | Polish | Terno escuro formal |
| `mechanic_diffuse` | Diffuse | 1K | Polish | Macacão com graxa |
| `child_mat_A/B/C/D/E` | Diffuse | 512 × 5 | MVP | 5 variações de cor de roupa |

---

### 5.3 Texturas de Ambiente

| Asset | Tipo | Resolução | Prioridade | Notas |
|-------|------|-----------|------------|-------|
| `school_diffuse` | Diffuse | 2K | MVP | Tijolo desgastado, janelas gradeadas |
| `school_windows_emissive` | Emissive | 1K | Polish | Janelas iluminadas à noite |
| `parking_diffuse` | Diffuse | 1K | MVP | Asfalto desgastado, marcações apagadas |
| `road_diffuse` (tileável) | Diffuse | 1K | MVP | Asfalto velho `#3D3D3D`, tileável seamless |
| `house_diffuse` (atlas 3×) | Diffuse | 1K | MVP | Atlas com 3 variações de casa |
| `house_windows_night` | Emissive | 512 | Polish | Janelas com luz quente |
| `tree_diffuse` (atlas 3×) | Diffuse | 512 | MVP | Atlas com 3 variações de árvore morta |
| `streetlight_diffuse` | Diffuse | 512 | Polish | Metal envelhecido |
| `church_diffuse` | Diffuse | 512 | Optional | |
| `bus_stop_diffuse` | Diffuse | 512 | MVP | |

---

### 5.4 Texturas de Props

| Asset | Tipo | Resolução | Prioridade | Notas |
|-------|------|-----------|------------|-------|
| `dashboard_diffuse` | Diffuse | 1K | MVP | Plastico envelhecido, botões desgastados |
| `dashboard_emissive` | Emissive | 512 | MVP | LEDs de status (verde/âmbar/vermelho) |
| `steering_wheel_diffuse` | Diffuse | 512 | MVP | Borracha gasta, marcas de mãos |
| `mirror_dirt_overlay` | Overlay | 512 | MVP | Mancha que não sai do retrovisor |
| `radio_diffuse` | Diffuse | 512 | MVP | |
| `microphone_diffuse` | Diffuse | 512 | MVP | |
| `watch_diffuse` | Diffuse | 512 | MVP | |
| `seat_diffuse` | Diffuse | 512 | MVP | 2 variações de desgaste |
| `map_base` | Diffuse | 2K | MVP | Arte do mapa vintage, manchas de sangue |
| `map_route_overlay` | Diffuse | 1K | MVP | Rota em vermelho — variação por dia (5 versões) |

---

## 6. Animações

> **Regra dos Fantasmas (CRÍTICO):** Framerate de animação 8–12 fps. Sem ease in/out — snap direto de pose. Sem piscar — olhos sempre abertos. Movimentos uncanny, marionete.  
> **Nomenclatura:** `[personagem]_[ação]` — ex: `marcus_teleport_appear`

---

### 6.1 Marcus Reid — "O Invasor"

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `marcus_idle` | Tremor leve e irregular nos membros, loop imperfeito | Loop 8fps | ~24f | MVP |
| `marcus_teleport_appear` | Materializa no assento — fade in + snap de postura | One-shot 8fps | ~12f | MVP |
| `marcus_teleport_disappear` | Desmaterializa — fade out rápido + fragmentação | One-shot 8fps | ~8f | MVP |
| `marcus_advance_seat` | Sequência de appear/disappear para fileira seguinte | One-shot | — | MVP |
| `marcus_proximity_alert` | Mais próximo do cockpit — tilt de cabeça agressivo | One-shot 8fps | ~16f | MVP |

---

### 6.2 Emma Lynch — "A Burlona"

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `emma_idle` | Balanço leve, sorrisão estático, dedos tamborilando | Loop 8fps | ~32f | MVP |
| `emma_laugh` | Corpo chacoalha com a risadinha (hihihi) — mecânico | One-shot 8fps | ~20f | MVP |
| `emma_rush_to_panel` | Corrida jerky pelo corredor em direção ao painel | One-shot 8fps | ~30f | MVP |
| `emma_reach_buttons` | Braços esticados alcançando os botões do painel | One-shot 8fps | ~16f | MVP |
| `emma_press_buttons` | Aperta botões aleatórios com animação de dedo | One-shot 8fps | ~12f | MVP |
| `emma_defeated` | Ao ser contida pela trava — recua com gestos frustrados | One-shot 8fps | ~20f | MVP |

---

### 6.3 Thomas Sanders — "O Narrador"

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `thomas_idle` | Flutua levemente, boca aberta, cabeça inclinada | Loop 8fps | ~48f | MVP |
| `thomas_float` | Movimento de levitação lento pelo corredor | Loop 8fps | ~60f | MVP |
| `thomas_whisper` | Boca em movimento exagerado sem som — uncanny | Loop 8fps | ~16f | MVP |
| `thomas_scream` | Abertura súbita e exagerada da boca — jump scare opcional | One-shot 8fps | ~8f | Polish |
| `thomas_appear` | Materializa no corredor a partir de névoa | One-shot 8fps | ~16f | MVP |
| `thomas_disappear` | Dissolve para névoa ao ser contido pelo rádio | One-shot 8fps | ~16f | MVP |

---

### 6.4 Grace Harper — "A Observadora"

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `grace_idle` | Completamente imóvel exceto micro-tilt de cabeça | Loop 8fps | ~60f | MVP |
| `grace_appear_mirror` | Surge no reflexo do retrovisor — fade in lento | One-shot 8fps | ~24f | MVP |
| `grace_stare` | Olha direto para o POV — cabeça acompanha retrovisor | Loop 8fps | ~30f | MVP |
| `grace_head_track` | Micro-rotação de cabeça seguindo o retrovisor | Procedural (código) | — | MVP |
| `grace_disappear` | Dissolve do retrovisor | One-shot 8fps | ~16f | MVP |
| `grace_proximity` | À medida que tensão sobe, ela se aproxima no espelho | Blend tree | — | Polish |

---

### 6.5 Oliver Crane — "O Artista"

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `oliver_idle` | Segura caderno, dedos tamborilando, head tilt ocasional | Loop 8fps | ~40f | MVP |
| `oliver_appear` | Materializa sentado com caderno já na mão | One-shot 8fps | ~16f | MVP |
| `oliver_draw` | Mão direita move sobre o caderno — escrita/desenho | Loop 8fps | ~24f | MVP |
| `oliver_draw_glass` | Aparece de pé e "desenha" no vidro/superfícies | One-shot 8fps | ~30f | MVP |
| `oliver_erase` | Apaga o que escreveu ao ser contido pelo rádio | One-shot 8fps | ~16f | MVP |
| `oliver_disappear` | Dissolve junto com as mensagens desenhadas | One-shot 8fps | ~20f | MVP |

---

### 6.6 Ray Morgan — Motorista

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `ray_hands_drive_idle` | Mãos no volante, leve tensão muscular | Loop 30fps | ~60f | MVP |
| `ray_hands_steer_left/right` | Giro do volante A/D | Loop 30fps | ~20f | MVP |
| `ray_hands_brake` | Pressão no pedal (pés, se visíveis) | Loop 30fps | ~12f | MVP |
| `ray_look_mirror` | Foco no retrovisor (tecla E) | One-shot 30fps | ~15f | MVP |
| `ray_look_watch` | Olha para o pulso (tecla SPACE) | One-shot 30fps | ~20f | MVP |
| `ray_microphone_use` | Pega microfone, fala, solta (tecla Q) | One-shot 30fps | ~40f | MVP |
| `ray_panel_lock` | Aperta botão de trava (tecla SHIFT) | One-shot 30fps | ~12f | MVP |
| `ray_radio_adjust` | Mexe no dial do rádio (tecla R) | One-shot 30fps | ~20f | MVP |
| `ray_headlights_toggle` | Liga/desliga farol (tecla F) | One-shot 30fps | ~10f | MVP |
| `ray_day1_idle` → `day5_idle` | Deterioração progressiva (tremor aumenta) | Loop 30fps | 5 variações | Polish |

---

### 6.7 Crianças Vivas (NPCs)

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `child_idle_seated` | Sentado, pequenos movimentos — mexer mochila, olhar pela janela | Loop 30fps | ~90f | MVP |
| `child_talking` | Vira para o lado, boca abrindo/fechando (sem áudio lip-sync) | Loop 30fps | ~40f | Polish |
| `child_scared` | Postura recolhida, olhando para frente com medo | Loop 30fps | ~40f | Polish |
| `child_board_bus` | Sobe a escada e senta (ponto de embarque) | One-shot 30fps | ~60f | MVP |
| `child_exit_bus` | Levanta, vai para a porta, desce | One-shot 30fps | ~60f | MVP |

---

### 6.8 Portas do Ônibus e Ambiente

| Animação | Descrição | Tipo | Frames | Prioridade |
|----------|-----------|------|--------|------------|
| `door_bus_open` | Porta dianteira do ônibus abre (tecla T) | One-shot 30fps | ~20f | MVP |
| `door_bus_close` | Porta dianteira fecha | One-shot 30fps | ~20f | MVP |
| `headlights_flicker` | Faróis piscando em tensão >75% — irregular, não senoidal | Procedural | — | MVP |
| `interior_lights_flicker` | Luzes internas piscam em momentos de alta tensão | Procedural | — | Polish |

---

## 7. UI Elements

> **Filosofia:** UI quase invisível em calmaria, cresce em urgência. Opacidade máxima 80%. Nunca parece overlay moderno de videogame.  
> **Tamanho de referência:** Resolução alvo 1920×1080.

---

### 7.1 HUD — Indicadores In-Game

| Asset | Descrição | Tamanho | Formato | Prioridade |
|-------|-----------|---------|---------|------------|
| `hud_tension_bar` | Barra vertical fina (2px) canto sup. direito. Cor: `#404038` → `#C89030` → `#9A2020` → `#CC1010` | 4×120px | Shader/código | MVP |
| `hud_tension_pulse` | Efeito de pulso da barra quando >75% — irregular | Animação | Shader | MVP |
| `hud_clock_text` | Texto HH:MM canto sup. esquerdo. Fonte IBM Plex Mono, opacidade 60% | — | UI Text | MVP |
| `hud_clock_urgent` | Versão vermelha `#9A2020` do relógio quando atrasado | — | Material swap | MVP |

---

### 7.2 HUD — Ícones de Cooldown (7 ferramentas)

> Fileira horizontal, canto inferior centralizado. 24×24px máx. Ícone de silhueta.

| Asset | Ferramenta | Tecla | Prioridade |
|-------|-----------|-------|------------|
| `icon_microphone` | Microfone PA | Q | MVP |
| `icon_panel_lock` | Trava do painel | SHIFT | MVP |
| `icon_radio` | Rádio | R | MVP |
| `icon_camera` | Câmera de segurança | C | MVP |
| `icon_headlights` | Farol | F | MVP |
| `icon_watch` | Relógio de pulso | SPACE | MVP |
| `icon_bus_stop` | Embarque/Desembarque | T | MVP |
| `hud_cooldown_arc` | Arco de cooldown ao redor do ícone (`#C89030` preenchendo) | — | MVP |
| `hud_cooldown_ready_pulse` | Pulse de disponibilidade (escala 1.0→1.15→1.0) | — | MVP |

---

### 7.3 Menus

| Asset | Descrição | Prioridade | Notas |
|-------|-----------|------------|-------|
| `menu_main_bg` | Background do menu principal — tela do ônibus na escuridão, lua ao fundo | MVP | Pode ser render da cena |
| `menu_main_title` | Logo/título "BUS SHIFT" — tipografia Bebas Neue, tom `#D4C878` | MVP | |
| `menu_btn_play` | Botão "INICIAR" | MVP | |
| `menu_btn_continue` | Botão "CONTINUAR" | MVP | |
| `menu_btn_settings` | Botão "CONFIGURAÇÕES" | Polish | |
| `menu_btn_quit` | Botão "SAIR" | MVP | |
| `menu_pause_overlay` | Overlay escuro `#0A0A0A` 60% + opções de pausa | MVP | |
| `menu_pause_resume` | Botão "RETOMAR" | MVP | |
| `menu_pause_restart` | Botão "RECOMEÇAR DIA" | MVP | |
| `menu_pause_quit` | Botão "MENU PRINCIPAL" | MVP | |
| `menu_day_transition` | Tela de transição entre dias (fade black + "DIA X") | MVP | |

---

### 7.4 Telas de Fim de Jogo

| Asset | Descrição | Prioridade | Notas |
|-------|-----------|------------|-------|
| `screen_game_over` | Fade para `#080810` + texto simples branco `#D8D8D0`. Silêncio 2s antes. | MVP | Sem drama visual |
| `screen_ending_1_good` | Tela/cutscene do final bom (referência: `final-1-bom-illustration.md`) | Polish | |
| `screen_ending_2_neutral` | Tela do final neutro | Polish | |
| `screen_ending_3_bad` | Tela/cutscene do final ruim (referência: `final-3-ruim-desfecho.md`) | Polish | |
| `screen_credits` | Tela de créditos — fonte Special Elite, rolagem lenta | Optional | |

---

### 7.5 Tipografia

| Uso | Fonte | Weight | Obs. | Prioridade |
|-----|-------|--------|------|------------|
| HUD / Dados (relógio, cooldown) | **IBM Plex Mono** | Regular 400 | Monospaced, leitura rápida | MVP |
| HUD alternativo | **Share Tech Mono** | Regular | Fallback para IBM Plex Mono | MVP |
| Diálogos (Diretor/Mecânico) | **Inter** | Light 300 | Sem serifa, não agressiva | MVP |
| Diálogos alternativo | **Roboto** | Light | Fallback para Inter | MVP |
| Textos narrativos (diário, notas) | **Special Elite** | Regular | Typewriter feel, anos 90 | Polish |
| Notas alternativo | **Courier Prime** | Regular | Fallback para Special Elite | Polish |
| Títulos de fase / "DIA X" | **Bebas Neue** | — | Condensed, impacto, maiúsculas | MVP |

> ⚠️ **Proibido:** Fontes decorativas de horror (blood dripping, etc.) — quebram o horror psicológico sério.

---

### 7.6 Overlay de Câmera de Segurança (CRT)

| Asset | Descrição | Prioridade |
|-------|-----------|------------|
| `overlay_crt_scanlines` | Linhas de scan horizontais estilo CRT (opacidade 20-30%) | MVP |
| `overlay_crt_noise` | Grain/ruído em movimento, cor verde-fósforo `#30A830` | MVP |
| `overlay_crt_static` | Estática pesada quando câmera com interferência | MVP |
| `overlay_crt_vignette` | Vinheta de tela CRT arqueada nas bordas | Polish |

---

## 8. Partículas e VFX

> **Limites de performance:** Ver style-guide seção 3.6. Nunca exceder os budgets definidos.  
> **Regra:** Efeitos de fantasma são sutis — sugerem sobrenatural sem gritar.

---

### 8.1 Efeitos de Fantasma

| Asset | Descrição | Limite Partículas | Prioridade | Notas de Implementação |
|-------|-----------|-------------------|------------|------------------------|
| `vfx_ghost_glow` | Brilho etéreo ao redor do fantasma (Fresnel + partículas micro) | Máx. 20 por fantasma | MVP | Pontos de luz `#8AB8D8`, escala micro, flutuando e descendo |
| `vfx_ghost_appear` | Materialização — partículas convergindo para o mesh | Máx. 30 (one-shot) | MVP | Duração ~0.8s |
| `vfx_ghost_disappear` | Desmaterialização — partículas divergindo do mesh | Máx. 30 (one-shot) | MVP | Duração ~0.6s |
| `vfx_ghost_droplets` | 3–5 gotículas esféricas flutuando próximas ao fantasma, descendo lentamente | 5 por fantasma | Polish | Visual "molhado" dos fantasmas |
| `vfx_ghost_teleport` | Flash breve de luz `#B8C8D8` no ponto de aparição de Marcus | Luz, não partículas | MVP | Duração 0.1s |
| `vfx_oliver_draw_trail` | Rastro luminoso na superfície enquanto Oliver "escreve" | Máx. 15 | MVP | Trail renderer, cor `#8AB8D8` |

---

### 8.2 Efeitos de Tensão / Pós-Processamento

| Asset | Descrição | Trigger | Prioridade |
|-------|-----------|---------|------------|
| `pp_desaturation` | Dessaturação progressiva: 0% (calmo) → -70% (terror) | Contínuo por % tensão | MVP |
| `pp_vignette` | Vinheta crescente: 0% → 45% → 80% | Estágios 2-4 | MVP |
| `pp_chromatic_aberration` | Aberração cromática sutil nas bordas | Estágio 3+ (>50%) | Polish |
| `pp_film_grain` | Grain de filme: 0% → 25% de intensidade | Estágio 4 (>75%) | MVP |
| `pp_color_temperature` | Temperatura de cor fria crescente | Contínuo | MVP |
| `pp_edge_bleed_red` | Pulso vermelho `#8A1010` nas bordas da tela | Tensão >90% | MVP |
| `pp_motion_blur_subtle` | Leve motion blur temporal | Estágio 4 | Polish |
| `pp_shadow_boost` | Boost de contraste de sombra +20% | Estágio 3+ | Polish |

---

### 8.3 Efeitos do Ônibus

| Asset | Descrição | Tipo | Prioridade | Notas |
|-------|-----------|------|------------|-------|
| `vfx_exhaust_smoke` | Fumaça do escapamento traseiro durante aceleração | Loop (partícula) | Polish | Máx. 20 partículas, cor cinza escuro `#2A2828` |
| `vfx_headlight_volumetric` | Luz volumétrica dos faróis (`#D4C878`, cone 35°, 25-30m) | Volume light | MVP | Neblina visível dentro do cone |
| `vfx_headlight_fog_scatter` | Partículas de neblina dentro do cone dos faróis | Máx. 30 | Polish | Reforça a volumetria |
| `vfx_engine_smoke_damage` | Fumaça leve do motor em dias avançados (Dia 3-5) | Loop leve | Optional | Narrativo: ônibus "adoecendo" |
| `vfx_interior_dust` | Partículas de poeira flutuando no interior (cockpit) | Máx. 30 | Polish | Luz penetrando pelas janelas |

---

### 8.4 Efeitos de Câmera / Interface Diegética

| Asset | Descrição | Tipo | Prioridade |
|-------|-----------|------|------------|
| `vfx_camera_glitch_spark` | Faíscas de interferência elétrica no monitor CRT | Máx. 15, vida curta | Polish |
| `vfx_camera_static_burst` | Estática repentina na câmera de segurança (c/ SFX) | Shader animado | MVP |
| `vfx_mirror_distortion` | Distorção sutil no retrovisor quando Grace presente | Shader UV distort | MVP |
| `vfx_fog_ground` | Névoa baixa no chão da estrada à noite | Fog shader (não partícula) | MVP |

---

## 9. Resumo de Escopo

> **SP (Story Points):** 1 SP ≈ 4 horas de trabalho de um artista pleno.  
> **MVP Total:** Estimativa para uma demo jogável funcional.  
> **Release Total:** Estimativa para produto final polido.

---

| Categoria | Nº de Assets | SP (MVP) | SP (Polish/Release) | Prioridade Geral |
|-----------|-------------|----------|---------------------|------------------|
| **Modelos 3D — Veículo** | 2 (int + ext) | 14 SP | 18 SP | 🔴 MVP |
| **Modelos 3D — Personagens (Fantasmas)** | 5 | 20 SP | 26 SP | 🔴 MVP |
| **Modelos 3D — Personagens (Humanos)** | 4 + 1 base NPC | 18 SP | 24 SP | 🟡 Mix |
| **Modelos 3D — Ambiente** | ~12 tipos | 16 SP | 22 SP | 🔴 MVP |
| **Modelos 3D — Props** | 10 | 14 SP | 18 SP | 🔴 MVP |
| **Texturas** | ~45 assets | 18 SP | 28 SP | 🔴 MVP |
| **Animações — Fantasmas** | ~25 clipes | 22 SP | 30 SP | 🔴 MVP |
| **Animações — Motorista (1ª pessoa)** | 10 clipes | 8 SP | 12 SP | 🔴 MVP |
| **Animações — NPCs / Outros** | ~10 clipes | 6 SP | 10 SP | 🟡 Mix |
| **UI Elements (HUD + Menus)** | ~35 assets | 10 SP | 16 SP | 🔴 MVP |
| **Partículas e VFX** | ~20 efeitos | 10 SP | 18 SP | 🟡 Mix |
| **TOTAL** | **~174 assets** | **156 SP** | **222 SP** |  |

---

### Estimativa de Tempo

| Cenário | SP Total | Tempo (1 artista) | Tempo (2 artistas) |
|---------|----------|-------------------|--------------------|
| **Demo MVP** | 156 SP | ~78 dias úteis (~4 meses) | ~39 dias úteis (~2 meses) |
| **Release Completo** | 222 SP | ~111 dias úteis (~6 meses) | ~56 dias úteis (~3 meses) |

---

### Checklist de Consistência Visual (Usar em Todo Asset)

- [ ] Poly count dentro do range do style-guide (seção 3)
- [ ] Paleta de cores limitada às swatches documentadas
- [ ] Nenhuma textura detalhada em fantasmas (flat shading apenas)
- [ ] Animações de fantasmas em 8–12fps, sem ease curves
- [ ] Shader de translucidez aplicado a fantasmas (60–75% opacidade)
- [ ] Fresnel edge glow em `#B8C8D8` em todos os fantasmas
- [ ] UI elementos com opacidade máxima de 80%
- [ ] Pivot points corretos em todos os props interativos
- [ ] LODs criados para personagens e assets de ambiente

---

*Assets Inventory — Bus Shift | Direção de Arte*  
*Gerado em 2026 | Referências: style-guide.md, onibus-104-visual.md, interior-onibus-pov-jogador.md*

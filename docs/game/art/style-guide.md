# Bus Shift — Style Guide Visual

**Versão:** 1.0  
**Data:** 2026  
**Responsável:** Direção de Arte  
**Status:** 🟡 Em Definição

---

## Índice

1. [Filosofia Visual](#1-filosofia-visual)
2. [Paleta de Cores](#2-paleta-de-cores)
3. [Guidelines de Poly Count](#3-guidelines-de-poly-count)
4. [Iluminação](#4-iluminação)
5. [Estilo dos Personagens](#5-estilo-dos-personagens)
6. [Interface e HUD](#6-interface-e-hud)
7. [Referências Visuais](#7-referências-visuais)

---

## 1. Filosofia Visual

### 1.1 Princípio Central: Less is More — E Mais Medo

Bus Shift é deliberadamente **incompleto** no visual. Cada polígono que falta é espaço para a imaginação do jogador construir seu próprio horror. O low poly não é uma limitação técnica — é uma **escolha narrativa**.

> *"O que você não vê claramente é sempre mais aterrorizante do que o que é mostrado."*

O Ônibus 104 deve parecer **velho e doente**. A pintura amarela descascando revela ferrugem como feridas não cicatrizadas. O ambiente ao redor respira abandono. A iluminação decide o que é seguro ou não.

### 1.2 Os Três Pilares Visuais

| Pilar | Descrição | Como Aplicar |
|-------|-----------|--------------|
| **Familiaridade Corrompida** | O cenário é um ônibus escolar — algo seguro e mundano. O horror vem da corrupção desse espaço familiar. | Detalhes normais do ônibus + detalhes sutilmente errados |
| **Dessaturação como Narrativa** | Cor = sanidade. Conforme a tensão sobe, o mundo perde cor. | Shader de pós-processamento que dessatura progressivamente |
| **Geometria que Mente** | Formas simples criam silhuetas ambíguas. Um fantasma low poly é mais assustador porque o cérebro "completa" a forma. | Personagens fantasmas com formas angulosas, propositalmente incompletas |

### 1.3 Influências Primárias

- **Five Nights at Freddy's (FNAF):** Câmeras de segurança, espaços confinados, mecânica de vigilância, terror que vem do desconhecido fora do campo de visão
- **Slender: The Eight Pages:** Minimalismo extremo, tensão sem música, o simples ato de virar a câmera como mecânica de horror
- **Dredge:** Atmosfera opressiva com paleta dessaturada, low poly como linguagem artística intencional, horror que existe na periferia da visão
- **Phasmophobia:** Horror cooperativo que usa iluminação e som como ferramentas de tensão, ambientes familiares tornados aterrorizantes

### 1.4 O Que EVITAR

- ❌ Texturas hiper-detalhadas ou realistas — quebram o estilo
- ❌ Animações fluidas para os fantasmas — deve ser uncanny e mecânico
- ❌ Cores saturadas brilhantes (exceto como sinal de alerta crítico)
- ❌ UI chamativa ou colorida — interfere na imersão
- ❌ Iluminação ambiente uniforme durante a noite — mata a tensão

---

## 2. Paleta de Cores

### 2.1 Paleta — Dia

A cena diurna é propositalmente **opressiva e sem vida**, não alegre. O sol não aquece — filtra através de nuvens pesadas. É o presságio de algo que vai piorar.

| Nome | Hex | Uso |
|------|-----|-----|
| Amarelo Ônibus Desbotado | `#C8A84B` | Carroceria do ônibus — tom base |
| Amarelo Oxidado | `#A07830` | Partes envelhecidas, ferrugem incipiente |
| Ferrugem Exposta | `#7A3B1E` | Buracos de ferrugem, bordas danificadas |
| Cinza Céu Pesado | `#8A9099` | Céu diurno — sem azul puro |
| Cinza Nuvem Fechada | `#B0B8C0` | Nuvens de cobertura |
| Asfalto Velho | `#3D3D3D` | Estrada principal |
| Asfalto Desgastado | `#525252` | Marcas apagadas, desgaste |
| Calçada Cinza | `#6B6B6B` | Passeio público, paradas de ônibus |
| Verde Murcho | `#5A6B4A` | Vegetação lateral — nunca vibrante |
| Marrom Terra Seco | `#6B4E2A` | Terra, terra batida |

**Regra do dia:** Sem cores puras ou saturadas. Tudo deve parecer como uma foto antiga levemente descolorida.

---

### 2.2 Paleta — Noite

A noite é o ambiente principal de terror. Alto contraste, sombras que **escondem ativamente**, luzes artificiais que revelam muito pouco.

| Nome | Hex | Uso |
|------|-----|-----|
| Azul-Marinho Profundo | `#0A0E1A` | Céu noturno, sombras densas |
| Azul Noite Aberto | `#141E30` | Áreas expostas ao céu |
| Sombra Quase Preta | `#060810` | Interior de sombras, becos, fundos de cena |
| Azul Asfalto Noturno | `#1A1F2E` | Estrada à noite, superfícies molhadas |
| Cinza Neblina Noturna | `#2A3040` | Fog de distância, névoa ambiente |
| Amarelo Farol Frio | `#D4C878` | Cone de luz dos faróis do ônibus |
| Branco Luz Artificial | `#E8EBF0` | Postes de luz, janelas de prédios distantes |
| Laranja Sódio Poste | `#C87832` | Postes de sódio antigos (anos 90) |
| Verde Bioluminescente | `#2A4A3A` | Reflexos em superfícies molhadas, vegetação noturna |
| Preto Profundo | `#030507` | Áreas sem nenhuma luz — usar com intenção |

**Regra da noite:** O jogador deve sempre ter dúvida se algo está se movendo nas sombras. Manter 60-70% da cena abaixo de 15% de luminosidade.

---

### 2.3 Paleta — UI / HUD

O HUD deve ser **quase invisível** quando a situação está calma e **ganhar presença** conforme a tensão aumenta. Nunca deve parecer um overlay de videogame moderno.

| Nome | Hex | Uso |
|------|-----|-----|
| Branco HUD Base | `#E0E0D8` | Texto e ícones em repouso |
| Cinza HUD Suave | `#909088` | Bordas de elementos, backgrounds de painel |
| Verde Status OK | `#4A7A5A` | Cooldowns disponíveis, ações liberadas |
| Âmbar Alerta | `#C89030` | Cooldown em progresso, atenção necessária |
| Vermelho Perigo | `#9A2020` | Tensão alta, intervenção urgente |
| Vermelho Crítico Pulsante | `#CC1010` | Game over iminente — deve piscar |
| Preto UI Background | `#0A0A0A` | Background de painéis |
| Branco Estático | `#F0EEE8` | Texto de diálogos do Diretor/Mecânico |

**Regra da UI:** Opacidade máxima de 80% em qualquer elemento de HUD. Fonte sem serifa, weight leve, nunca grita.

---

### 2.4 Paleta — Fantasmas (As Cinco Crianças)

Os fantasmas são **azul-cinza dessaturados** com leve bioluminescência própria. Parecem "molhados" — como se acabassem de sair da chuva, ou de baixo d'água.

| Nome | Hex | Uso |
|------|-----|-----|
| Azul Fantasma Base | `#6A80A0` | Tom de pele dos fantasmas |
| Cinza Translúcido Claro | `#8A9AB0` | Roupas anos 90 — cor base |
| Branco Azulado Etéreo | `#B8C8D8` | Highlights, bordas de silhueta |
| Azul Emissivo Fraco | `#4A6888` | Glow de presença fantasma |
| Azul-Cinza Profundo | `#2A3848` | Sombras internas dos fantasmas |
| Branco Olhos | `#E8F0F8` | Olhos (sem pupila definida — só branco) |
| Negro Vazio | `#080C10` | Cavidades, boca aberta de Thomas Sanders |

**Regra dos fantasmas:**
- Opacidade do mesh entre **60-75%** — translúcidos, nunca sólidos
- Shader customizado com **Fresnel edge glow** em `#B8C8D8`
- Sem textura detalhada — apenas gradientes de cores acima
- Partículas ocasionais: pequenos pontos de luz `#8AB8D8` flutuando (10-20 partículas, escala micro)

---

### 2.5 Paleta — Tensão (Progressão de Sanidade)

A cor do mundo muda com a tensão do jogador. Este é um sistema de pós-processamento em 4 estágios.

#### Estágio 1 — Calmo (0–25% tensão)
- Paleta normal conforme acima
- Sem efeitos adicionais
- Saturação: 100%

#### Estágio 2 — Alerta (26–50% tensão)
- Dessaturação leve: **-15%** em toda a cena
- Leve vinheta nas bordas da tela: `#0A0A0A` com 20% opacidade
- Temperatura de cor: levemente mais fria (+10 azul)
- Flicker ocasional na iluminação (1-2x por minuto)

#### Estágio 3 — Paranoia (51–75% tensão)
- Dessaturação: **-40%** — cores começam a parecer memórias
- Vinheta pesada: `#050508` com 45% opacidade
- Aberração cromática sutil nas bordas
- Temperatura: fria, quase monocromática
- Flicker de luz mais frequente (1x a cada 20 segundos)
- Sombras ficam mais profundas: boost de contraste +20%

#### Estágio 4 — Terror (76–100% tensão)
- Dessaturação quase total: **-70%** — o mundo é quase preto e branco
- Único elemento com cor: pulso vermelho `#8A1010` nas bordas da tela
- Ruído visual (grain) adicionado: 15-25% de intensidade
- Tempo parece mais lento (levíssimo motion blur)
- Temperatura extremamente fria
- Tom vermelho `#3A0808` invade as sombras

> **Nota de implementação:** Este sistema deve ser gradual e contínuo, não por "snapshots" de estágio. O jogador não deve notar a transição — apenas sentir o desconforto crescer.

---

## 3. Guidelines de Poly Count

O estilo low poly de Bus Shift é **low poly estilizado**, não low poly acidental. Cada modelo deve parecer que foi esculpido com intenção. Faces planas são bem-vindas — evite subdivisão desnecessária.

### 3.1 Referência de Escala

**Target de performance:** < 100 draw calls por frame | < 2GB RAM | 60+ FPS

---

### 3.2 Personagens Principais

| Personagem | Poly Count Recomendado | LOD 1 (distância) |
|------------|----------------------|-------------------|
| Motorista (mãos + partes visíveis em 1ª pessoa) | 800 – 1.200 triângulos | Não necessário (1ª pessoa) |
| Motorista (cutscenes/telas de game over) | 1.500 – 2.500 triângulos | 600 tri (LOD1) |
| Cada Fantasma (criança) | 600 – 900 triângulos | 300 tri (LOD1) |
| Diretor Harrison Stone | 1.200 – 1.800 triângulos | 500 tri (LOD1) |
| Mecânico | 1.000 – 1.500 triângulos | 500 tri (LOD1) |

**Prioridade de detalhe:** Rosto e mãos têm mais polígonos; corpo é mais simplificado.

---

### 3.3 NPCs Secundários (Crianças Vivas)

| Elemento | Poly Count |
|----------|-----------|
| Criança viva (instanciada, ~10 no ônibus) | 300 – 500 triângulos |
| Variações de roupa (shared mesh + swap de material) | Reutilizar mesh base |

**Importante:** Crianças vivas são instâncias do mesmo mesh base com variações de cor de material. Não criar modelos únicos para cada criança.

---

### 3.4 Ambiente e Cenário

| Elemento | Poly Count |
|----------|-----------|
| Interior do ônibus (cockpit + corredor) | 3.000 – 5.000 triângulos total |
| Exterior do ônibus 104 (visto em cutscenes) | 2.000 – 3.500 triângulos |
| Prédios de rua (plano de fundo) | 200 – 600 tri por prédio |
| Escola Ravenswood Elementary (fachada) | 2.500 – 4.000 triângulos |
| Estacionamento da escola | 800 – 1.200 triângulos |
| Parada de ônibus | 300 – 500 triângulos |
| Segmento de estrada (tile) | 400 – 800 triângulos |
| Árvore (morta/sem folhas) | 150 – 300 triângulos |

---

### 3.5 Props e Objetos Interativos

| Objeto | Poly Count |
|--------|-----------|
| Painel do motorista (área principal de interação) | 600 – 1.000 triângulos |
| Retrovisor interno | 80 – 150 triângulos |
| Rádio do ônibus | 100 – 200 triângulos |
| Microfone de PA | 80 – 150 triângulos |
| Relógio do painel | 60 – 100 triângulos |
| Bancos do ônibus (por unidade) | 150 – 250 triângulos |
| Janelas (planos com material) | 30 – 50 triângulos |

---

### 3.6 Partículas e Efeitos Visuais

| Efeito | Limite de Partículas |
|--------|---------------------|
| Poeira dentro do ônibus | Máx. 30 partículas simultâneas |
| Brilho etéreo dos fantasmas | Máx. 20 partículas por fantasma |
| Neblina volumétrica (fog) | Usar Shader de fog, não partículas |
| Respingos de chuva (se houver) | Máx. 50 partículas por frame |
| Faíscas/interferência elétrica (câmera) | Máx. 15 partículas, vida curta |

---

## 4. Iluminação

### 4.1 Regra Geral

Bus Shift usa **URP (Universal Render Pipeline)** com iluminação em tempo real para elementos principais e baked lighting para ambiente estático. A intenção é sempre: a luz **define o que é seguro**. Onde não há luz, há perigo.

---

### 4.2 Cena Diurna — "Opressão Cotidiana"

- **Luz direcional (sol):** Ângulo baixo, temperatura fria `#D8DCE0`, intensidade 0.7 (não estourando)
- **Ambiente (skybox):** Completamente encoberto, sem sombra dura do sol
- **Shadow Type:** Soft shadows, penumbra ampla
- **Fog:** Leve neblina diurna `#A8B0B8`, start distance 40m, end distance 150m
- **Vibe:** Como um dia nublado de inverno. O sol não traz conforto.
- **Interior do ônibus (dia):** Luz difusa de janelas, sem sombras dramáticas. Tudo visível, nada escondido — mas a visibilidade cria pressão social (você pode ver tudo... inclusive elas).

---

### 4.3 Cena Noturna — "O Horror Começa Aqui"

- **Luz direcional (lua):** Intensidade baixíssima 0.05–0.1, temperatura `#8090B0` (azul frio), ângulo alto
- **Ambiente global:** Quase zero — o mundo é escuridão com ilhas de luz
- **Shadow Type:** Hard shadows — sombras nítidas e cortantes
- **Intensidade do contraste:** Alta — diferença entre luz e sombra deve ser brutal
- **Interior do ônibus (noite):** Luzes de emergência fracas `#404030`, luz do painel `#303820`. O corredor atrás do motorista é escuridão. O que está lá atrás?

> **Regra de ouro noturna:** Se o jogador consegue ver claramente mais de 30% do interior do ônibus sem usar ferramentas, a iluminação está errada — ajustar para mais escuro.

---

### 4.4 Faróis do Ônibus

Os faróis são uma **mecânica de jogo** (tecla F) além de elemento visual. Devem ter personalidade.

- **Tipo:** Cone de luz volumétrica (VolumetricLight no URP com pacote adicional ou fog scattering)
- **Cor:** `#D4C878` — amarelo levemente frio, não branco puro (faróis velhos)
- **Ângulo do cone:** 35° de abertura
- **Alcance:** 25-30m de distância efetiva
- **Penumbra:** Bordas difusas, não cortantes
- **Efeito de fog:** A neblina deve ser **visível** dentro do cone — criar densidade de partícula leve dentro do frustum do farol
- **Quando DESLIGADO:** O mundo à frente some. Literalmente. Isso deve ser aterrorizante.
- **Flicker de tensão:** Com tensão >75%, os faróis piscam de forma irregular (não senoidal — aleatório e perturbador)

---

### 4.5 Luz dos Fantasmas

Cada fantasma emite uma **luz ambiente fraca** que os torna visíveis sem revelar completamente.

- **Tipo:** Point light com range curto (1.5–2.5m)
- **Cor:** `#6A8AB8` — azul-branco frio
- **Intensidade:** 0.3–0.5 (sutil, não ilumina o ambiente — só o próprio fantasma)
- **Shadows:** OFF para a luz do fantasma — não deve projetar sombras (seria realista demais)
- **Flicker:** Oscilação suave de 0.8–1.2 de intensidade em ciclo irregular (não senoidal)
- **Quando se aproxima do motorista:** Intensidade aumenta para 0.8, cor levemente para `#8AB0D0`

---

### 4.6 Neblina (Fog) como Instrumento de Tensão

A neblina não é decoração — é uma ferramenta de horror ativo.

| Situação | Tipo de Fog | Cor | Densidade |
|----------|-------------|-----|-----------|
| Dia normal | Linear | `#A8B0B8` | Leve (start 40m) |
| Noite calma | Exponential | `#0C1020` | Média (density 0.015) |
| Tensão alta (>60%) | Exponential² | `#080C18` | Alta (density 0.03) |
| Momento de aparição | Exponential² | `#060810` | Muito alta — world shrinks |

> A neblina faz o mundo parecer menor e mais claustrofóbico conforme o terror aumenta. O jogador não pode ver os fantasmas vindo de longe — eles **emergem** da névoa.

---

## 5. Estilo dos Personagens

### 5.1 Fantasmas — As Cinco Crianças

#### Proporções e Silhueta
- **Cabeça:** Proporcionalmente maior que o corpo — ratio 1:4 (cabeça/corpo) vs 1:7 humano normal
- **Olhos:** Maiores que o normal, sem íris definida — apenas branco `#E8F0F8`
- **Pescoço:** Levemente mais longo — creates uncanny valley no low poly
- **Membros:** Finos, levemente desproporcionais — braços um pouco longos demais
- **Postura:** Ligeiramente curvada, ombros caídos — nunca postura ereta e confiante

#### Roupas Anos 90 (Detalhes Mínimos mas Reconhecíveis)

| Personagem | Roupa Base | Detalhe Identificador |
|------------|-----------|----------------------|
| **Marcus Reid** (O Invasor) | Jaqueta bomber azul sobre camiseta | Número no peito, tênis com stripes |
| **Emma Lynch** (A Burlona) | Vestido xadrez anos 90, meia alta | Laço no cabelo levemente torto |
| **Thomas Sanders** (O Narrador) | Camiseta de banda (silhueta), calça cargo | Distintivo escolar no pescoço |
| **Grace Harper** (A Observadora) | Moletom com capuz, calça jeans | Mochila escolar sempre pendurada |
| **Oliver Crane** (O Artista) | Moletom com manchas de tinta, dedos marcados | Lápis na orelha, caderno na mão |

**Regra de roupas:** Reconhecíveis à distância por silhueta. Não adicionar logotipos ou texto — apenas formas geométricas simples que sugerem o detalhe.

#### Visual "Molhado"
- **Shader:** Specular alto (`0.7–0.9`) em toda a superfície dos personagens fantasmas
- **Normal Map:** Ausente — superfície completamente flat (low poly intencional)
- **Vertex Color:** Leve gradiente de branco-azulado nas bordas superiores (simula umidade)
- **Gotículas:** 3–5 partículas esféricas pequenas flutuando próximas ao personagem, descendo lentamente

---

### 5.2 Animações — A Regra do "Uncanny Jerky"

**Princípio:** Os fantasmas NÃO foram animados por um animator humano. Eles se movem como se fossem marionetes com fios.

- **Framerate de animação:** 8–12 fps (não 24/30 fps dos personagens normais) — posterização de movimento
- **Sem inbetweening suave:** Transições de pose sem ease in/ease out — snap direto
- **Tilt de cabeça:** Movimento de cabeça frequente, ligeiramente além do natural
- **Piscar:** Nunca pisca — olhos sempre abertos
- **Idle:** Tremor muito leve e irregular nos membros (não loop perfeito)
- **Deslocamento:** Glide em vez de walk — os pés não interagem com o chão de forma convincente

> **Marcus (O Invasor)** move de assento para assento de forma teleportada — não animate o percurso, apenas o destino.  
> **Grace (A Observadora)** tem micro-movimentos de cabeça que seguem o retrovisor mesmo quando o jogador não está olhando.

---

### 5.3 Motorista — Progressão Visual ao Longo dos 5 Dias

O motorista é o espelho do estado psicológico do jogador.

| Dia | Aparência | Detalhes Visuais |
|-----|-----------|-----------------|
| **Dia 1** | Profissional, levemente cansado | Uniforme limpo `#8B7355` (caqui), barba feita, olheiras leves |
| **Dia 2** | Claramente exausto | Barba de 2 dias, colarinho aberto, olheiras marcadas `#3D2A1A` |
| **Dia 3** | Perturbado | Mancha na farda (café? sangue?), cabelo despenteado, olhar mais vazio |
| **Dia 4** | Desconstruindo | Gravata (se usava) inexistente, manchas mais visíveis, tremor nas mãos (animação) |
| **Dia 5** | À beira do colapso | Farda rasgada/aberta, suor visível (shader), olhos vermelhos `#8A3A2A` nos whites |

**Cor da farda por dia:**
- Dia 1: `#8B7355` (caqui limpo)
- Dia 2: `#7A6445`
- Dia 3: `#6B5838` (manchas `#2A1A0E`)
- Dia 4: `#5C4A2C`
- Dia 5: `#4A3820` (com manchas escuras `#1A0E08`)

---

### 5.4 NPCs Secundários

**Diretor Harrison Stone:**
- Terno `#1A1A2A` (azul-marinho quase preto), gravata `#8A2020`
- Expressão neutra imperturbável — sem microexpressões
- Cabelo branco `#D8D4CC`, óculos metálicos finos
- Postura: sempre ereta, formal — nunca relaxada

**Mecânico:**
- Macacão `#2A2820` (preto de graxa), mãos com textura de óleo `#0A0808`
- Velho, curvado — postura contrária ao Diretor
- Barba branca desalinhada, boné puído

---

## 6. Interface e HUD

### 6.1 Filosofia da UI

A interface de Bus Shift deve parecer que **não existe**. O jogador deve esquecer que tem uma HUD. Ela só deve aparecer quando o jogador precisa — e quando aparece em estado de alerta, deve causar desconforto.

### 6.2 Elementos do HUD

#### Indicador de Tensão
- **Posição:** Canto superior direito, pequeno
- **Forma:** Linha vertical fina (2px de largura) — como uma rachadura
- **Cor em repouso:** `#404038` — quase invisível
- **Cor em alerta:** Gradiente de `#C89030` → `#9A2020` → `#CC1010`
- **Animação:** Pulsa suavemente quando >75% (pulso irregular, não senoidal)
- **Label:** Nenhum — sem texto "TENSÃO" ou "SANIDADE". A cor fala por si.

#### Indicadores de Cooldown (Ferramentas)
- **Posição:** Fileira horizontal no canto inferior, centralizada
- **Estilo:** Ícones minimalistas (silhuetas) com arco de cooldown ao redor
- **Cor disponível:** `#4A7A5A` (verde discreto)
- **Cor em cooldown:** `#909088` (cinza) com arco preenchendo em `#C89030`
- **Tamanho:** Pequenos — 24x24px máximo
- **Feedback:** Leve pulse quando uma ferramenta fica disponível (escala 1.0 → 1.15 → 1.0)

#### Relógio / Horário da Rota
- **Posição:** Canto superior esquerdo
- **Estilo:** Texto monospaced `#E0E0D8`, opacidade 60%
- **Formato:** `HH:MM` — simples
- **Urgência:** Vermelho `#9A2020` e tamanho aumenta quando atrasado

#### Indicador de Câmera (Retrovisor)
- **Posição:** Retrovisor físico visível no modelo 3D do cockpit — não overlay
- **Interferência:** Quando câmera com ruído, imagem granulada com scan lines leves
- **Cor do glitch:** Verde fósforo `#30A830` para o efeito de câmera antiga

### 6.3 Tipografia

| Uso | Fonte Recomendada | Weight | Características |
|-----|-------------------|--------|----------------|
| HUD / Dados | **IBM Plex Mono** ou **Share Tech Mono** | Regular (400) | Monospaced, leitura rápida |
| Diálogos (Diretor/Mecânico) | **Inter** ou **Roboto** | Light (300) | Sem serifa, não agressiva |
| Textos narrativos (diário, notas) | **Special Elite** ou **Courier Prime** | Regular | Typewriter feel — anos 90 |
| Títulos de fase/dia | **Bebas Neue** ou similar condensed | — | Impacto, maiúsculas, espaçamento largo |

**Regra de tipografia:** Nunca usar fonte decorativa/horror (tipo blood dripping) — é kitch e quebra a seriedade do horror psicológico.

### 6.4 Tela de Game Over

- **Fundo:** Fade para `#080810` completo
- **Texto:** Simples, branco `#D8D8D0`, sem drama
- **Sem animação de morte elaborada** — o silêncio e o fade são o horror
- **Som:** Silêncio absoluto por 2 segundos antes de qualquer UI aparecer

---

## 7. Referências Visuais

### 7.1 Jogos

---

#### 🎮 Five Nights at Freddy's (Scott Cawthon, 2014)
**O que usar:**
- Mecânica de câmeras de segurança como sistema de vigilância
- Terror que vem do *off-screen* — o que está fora do campo de visão
- Interface diegética (o painel de controle faz parte do mundo)
- Iluminação de câmera noturna: granulada, lavada, sem cores
- Tensão por gerenciamento de recursos limitados (bateria → cooldowns)

**Aplicação em Bus Shift:** O retrovisor e a câmera funcionam como as câmeras do FNAF. O jogador escolhe onde olhar — e escolher errado tem consequências.

---

#### 🎮 Slender: The Eight Pages (Parsec Productions, 2012)
**O que usar:**
- Minimalismo extremo: a ausência de trilha sonora cria tensão
- O simples ato de virar a câmera como mecânica de horror
- Estética de câmera com ruído e interferência
- Fog denso como limitador intencional de visibilidade
- Um único inimigo simples pode ser mais aterrorizante que mil

**Aplicação em Bus Shift:** A neblina densa, o silêncio e os apareceamentos súbitos sem música dramática.

---

#### 🎮 Phasmophobia (Kinetic Games, 2020)
**O que usar:**
- Ambiente familiar (casa normal) se tornando aterrorizante
- Iluminação de lanterna e visão noturna como mecânica
- Evidências gradativas — o horror se constrói lentamente
- Som como ferramenta primária de horror (passos, sussurros)
- Tensão psicológica entre jogadores que discordam do que viram

**Aplicação em Bus Shift:** O interior do ônibus é o "mapa" de Phasmophobia. Evidências graduais dos fantasmas. Sons antes de aparições visuais.

---

#### 🎮 Dredge (Black Salt Games, 2023)
**O que usar:**
- Low poly como linguagem artística intencional e bonita
- Paleta dessaturada que ainda é visualmente coerente e agradável
- Horror que existe na periferia — o que aparece no canto do olho
- Sistema de sanidade que muda a percepção visual do mundo
- Atmosfera opressiva sem ser explicitamente gore

**Aplicação em Bus Shift:** O sistema de tensão/dessaturação é diretamente inspirado em Dredge. O horror periférico dos fantasmas aparecendo nas bordas da câmera.

---

#### 🎮 Outlast (Red Barrels, 2013)
**O que usar:**
- Câmera de visão noturna como ferramenta com custo (bateria)
- Confinamento em espaço fechado como amplificador de terror
- Impotência do protagonista — não pode lutar, só sobreviver
- Documentação interna (notas, arquivos) revelando lore gradualmente

**Aplicação em Bus Shift:** O motorista não pode atacar os fantasmas. Só gerenciar e sobreviver. Os diários/notas encontráveis revelam Victor Graves.

---

#### 🎮 Alien: Isolation (Creative Assembly, 2014)
**O que usar:**
- Estética retro-futurista de tela CRT/analógica para interfaces
- Som ambiente como elemento de construção de tensão (nenhuma música → mais medo)
- Movimento de câmera do inimigo que o jogador pode prever parcialmente
- Feedback visual de "alerta" gradual e preciso

**Aplicação em Bus Shift:** A estética de câmera analógica do retrovisor. A lógica de "comportamento previsível mas não controlável" dos fantasmas.

---

#### 🎮 Visage (SadSquare Studio, 2020)
**O que usar:**
- Horror psicológico que distorce o ambiente quando sanidade cai
- Pós-processamento crescente: aberração cromática, grain, vinheta
- Fantasmas que seguem lógica própria e incompreensível
- Ambientes domésticos corrompidos — o familiar se torna ameaçador

**Aplicação em Bus Shift:** O sistema de pós-processamento de tensão (Estágios 1-4 descritos na seção 2.5) é diretamente inspirado no sistema de insanidade de Visage.

---

### 7.2 Filmes e Séries (para Atmosfera e Referência de Direção)

---

#### 🎬 The Fog (John Carpenter, 1980)
**O que usar:**
- Neblina como personagem ativo que traz o perigo
- Horror que vem de dentro do nevoeiro — invisível até o último segundo
- Iluminação de lanterna e luz artificial isolada no escuro

---

#### 🎬 It Follows (David Robert Mitchell, 2014)
**O que usar:**
- Entidade que se aproxima lentamente mas inevitavelmente
- Sensação de que algo está sempre seguindo — mesmo sem vê-lo
- Fotografia dessaturada, suburbana, anos 80/90, desolada

**Aplicação em Bus Shift:** Marcus Reid (O Invasor) que avança fileira por fileira é diretamente análogo à entidade de It Follows.

---

#### 🎬 The Shining (Stanley Kubrick, 1980)
**O que usar:**
- Isolamento que corrói sanidade
- Crianças como elemento de horror — inocência corrompida
- Câmera que segue o protagonista em espaços fechados (steadicam/POV)
- Ambiente familiar e familiar que se torna labiríntico e perigoso

---

#### 🎬 Carriers / The Mist (Frank Darabont, 2007)
**O que usar:**
- Paleta completamente dessaturada, mundo sem cor
- Ameaça invisível no nevoeiro
- Tensão entre personagens confinados
- Atmosfera de fim inevitável

---

#### 📺 Are You Afraid of the Dark? (Série Nickelodeon, anos 90)
**O que usar:**
- Estética visual específica dos anos 90 em contexto de horror infantil
- Fantasmas de crianças com roupas características da época
- Horror "sério" em contexto que deveria ser seguro (escola, ônibus)
- Referência direta para as roupas e aparência das cinco crianças

---

## Apêndice — Checklist de Consistência Visual

Use este checklist ao criar cada asset:

- [ ] Poly count dentro do range definido para o tipo de asset
- [ ] Paleta de cores limitada às swatches documentadas neste guia
- [ ] Nenhuma textura detalhada em personagens fantasmas (flat shading)
- [ ] Animações de fantasmas em 8-12fps, sem ease curves
- [ ] Iluminação ambiente não excede limites de brightness por período (dia/noite)
- [ ] UI elementos com opacidade máxima de 80%
- [ ] Shader de translucidez aplicado a fantasmas (60-75% opacidade)
- [ ] Fog configurado conforme o estado de tensão atual

---

## Changelog

### v1.0 — 2026
- Criação inicial completa do Style Guide
- Paletas de cores definidas para todos os contextos
- Guidelines de poly count estabelecidas
- Sistema de progressão visual de tensão documentado
- Referências visuais com 10+ entradas anotadas

---

*Style Guide — Bus Shift | Direção de Arte*

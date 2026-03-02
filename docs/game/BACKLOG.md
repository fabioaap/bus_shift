# Bus Shift - Product Backlog

**Última Atualização:** 2026-03-01
**Projeto:** Bus Shift - Horror Survival Game
**Status:** Pre-Production

---

## 📋 Estrutura do Backlog

```
BACKLOG.md (este arquivo)
├── Épicos
├── User Stories por Épico
└── Technical Tasks
```

---

## 🎯 Épicos (Major Features)

### Epic 1: 📖 Documentação e Design
**Objetivo:** Completar toda documentação de design necessária para desenvolvimento
**Prioridade:** 🔴 CRÍTICA
**Estimativa:** 40 story points

### Epic 2: 🎮 Mecânicas Core (MVP)
**Objetivo:** Implementar mecânicas básicas de direção e interação
**Prioridade:** 🔴 CRÍTICA
**Estimativa:** 80 story points

### Epic 3: 👻 Sistema de Assombrações
**Objetivo:** Implementar as 3 crianças fantasmas e suas mecânicas
**Prioridade:** 🟠 ALTA
**Estimativa:** 60 story points

### Epic 4: 📈 Sistema de Progressão
**Objetivo:** Implementar tensão, balanceamento e progressão de dias
**Prioridade:** 🟠 ALTA
**Estimativa:** 50 story points

### Epic 5: 🎨 Arte e Ambiente
**Objetivo:** Criar todos os assets visuais (Low Poly)
**Prioridade:** 🟡 MÉDIA
**Estimativa:** 100 story points

### Epic 6: 🔊 Áudio e Atmosfera
**Objetivo:** Implementar trilha sonora, SFX e áudio atmosférico
**Prioridade:** 🟡 MÉDIA
**Estimativa:** 40 story points

### Epic 7: 📚 Narrativa e Cutscenes
**Objetivo:** Implementar história completa (5 dias + 3 finais)
**Prioridade:** 🟠 ALTA
**Estimativa:** 70 story points

### Epic 8: 🧪 Testes e Polimento
**Objetivo:** QA completo, balanceamento e polimento final
**Prioridade:** 🟢 BAIXA (última fase)
**Estimativa:** 60 story points

---

## 📖 EPIC 1: Documentação e Design

### 🔴 CRÍTICO

#### #1 - Definir Nomes e Identidades
**Tipo:** Documentation
**Prioridade:** P0
**Estimativa:** 2 SP

**Descrição:**
Definir nomes oficiais para:
- Escola (substituir [NOME DA ESCOLA])
- 3 Crianças assombradas
- Diretor (completar nome de Siqueira)
- Motorista anterior
- Cidade/localização

**Critérios de Aceite:**
- [ ] Nome da escola definido e atualizado em toda documentação
- [ ] 3 crianças têm nomes e backstories básicas
- [ ] NPCs nomeados em characters.md

**Arquivo:** `docs/game/narrative/characters.md`

---

#### #2 - Completar Narrativa (Dias 2-5)
**Tipo:** Documentation
**Prioridade:** P0
**Estimativa:** 8 SP

**Descrição:**
Desenvolver narrativa completa dos dias 2 a 5:
- Dia 2: Revelações fragmentadas
- Dia 3: A verdade enterrada
- Dia 4: Escalada
- Dia 5: Última parada

**Critérios de Aceite:**
- [ ] Cada dia tem narrativa manhã + tarde
- [ ] Progressão lógica de revelações
- [ ] Eventos chave definidos por dia
- [ ] Diálogos essenciais escritos

**Arquivo:** `docs/game/narrative/story.md`

---

#### #3 - Definir Backstory do Acidente Original
**Tipo:** Documentation
**Prioridade:** P0
**Estimativa:** 3 SP

**Descrição:**
Criar lore completo sobre o acidente:
- Ano do acidente
- Causa (mecânica/humana/sobrenatural)
- Vítimas (número, identidades)
- Motorista original (culpado? morto?)
- Por que apenas 3 crianças assombram?

**Critérios de Aceite:**
- [ ] Timeline definida
- [ ] Causa clara e consistente
- [ ] Motivações das assombrações justificadas
- [ ] Documentado em story.md

**Arquivo:** `docs/game/narrative/story.md` (seção Lore Fragments)

---

#### #4 - Criar Style Guide Visual (Low Poly)
**Tipo:** Documentation
**Prioridade:** P1
**Estimativa:** 5 SP

**Descrição:**
Definir estilo visual Low Poly consistente:
- Paleta de cores (Dia vs Noite)
- Contagem de polígonos (ranges)
- Referências visuais
- Iluminação e sombras
- Tratamento de texturas

**Critérios de Aceite:**
- [ ] Documento style-guide.md completo
- [ ] Moodboard com 10+ referências
- [ ] Paleta definida (hex codes)
- [ ] Guidelines de poly count

**Arquivo:** `docs/game/art/style-guide.md`

---

#### #5 - Especificação Técnica Completa
**Tipo:** Documentation
**Prioridade:** P1
**Estimativa:** 5 SP

**Descrição:**
Criar especificação técnica detalhada:
- Engine escolhida (Unity/Unreal/Godot)
- Arquitetura de código (padrões)
- Sistema de save/load
- Performance targets (FPS, memória)
- Plataformas alvo (PC/Console/Mobile)

**Critérios de Aceite:**
- [ ] Engine definida com justificativa
- [ ] Architecture.md completo
- [ ] Performance targets documentados
- [ ] Tech stack definido

**Arquivo:** `docs/game/technical/architecture.md`

---

### 🟡 MÉDIO

#### #6 - Criar Asset List Completa
**Tipo:** Documentation
**Prioridade:** P2
**Estimativa:** 3 SP

**Descrição:**
Inventariar todos os assets necessários:
- Modelos 3D (ônibus, personagens, ambiente)
- Texturas
- Animações
- UI elements
- Partículas

**Critérios de Aceite:**
- [ ] Lista completa em assets.md
- [ ] Priorização (MVP vs Polish)
- [ ] Estimativas de tempo de criação

**Arquivo:** `docs/game/art/assets.md`

---

#### #7 - Plano de Testes Detalhado
**Tipo:** Documentation
**Prioridade:** P2
**Estimativa:** 3 SP

**Descrição:**
Criar plano de testes completo:
- Test cases por mecânica
- Matriz de compatibilidade
- Procedimentos de QA
- Ferramentas de teste

**Critérios de Aceite:**
- [ ] test-plan.md completo
- [ ] Test cases para cada mecânica
- [ ] Critérios de aceitação definidos

**Arquivo:** `docs/game/testing/test-plan.md`

---

## 🎮 EPIC 2: Mecânicas Core (MVP)

### 🔴 CRÍTICO

#### #8 - Setup do Projeto
**Tipo:** Technical Setup
**Prioridade:** P0
**Estimativa:** 5 SP
**Dependências:** #5

**Descrição:**
Configurar projeto na engine escolhida:
- Criar projeto base
- Setup de controle de versão
- Estrutura de pastas
- Pipeline de build básico

**Critérios de Aceite:**
- [ ] Projeto criado e buildando
- [ ] Git configurado (.gitignore, LFS)
- [ ] Estrutura de pastas padronizada
- [ ] README técnico criado

---

#### #9 - Implementar Controles de Direção
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #8

**Descrição:**
Implementar mecânica de direção do ônibus:
- Inputs WASD
- Física de veículo realista (pesado)
- Aceleração/frenagem/ré
- Steering suave

**Critérios de Aceite:**
- [ ] Ônibus responde a WASD
- [ ] Física de veículo pesado funciona
- [ ] Feedback sonoro básico (motor)
- [ ] Testes unitários passam

**Referência:** `docs/game/design/mechanics.md` (Direção do Ônibus)

---

#### #10 - Protótipo de Rota Simples
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #9

**Descrição:**
Criar rota teste com 3 pontos de parada:
- Estrada básica (gray box)
- 3 pontos de ônibus
- Waypoints/GPS básico
- Trigger zones para pontos

**Critérios de Aceite:**
- [ ] Rota navegável do início ao fim
- [ ] Pontos de parada detectáveis
- [ ] Sistema de waypoint funcional
- [ ] Mapa básico mostra localização

---

#### #11 - Sistema de Embarque/Desembarque
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #10

**Descrição:**
Implementar interação com pontos:
- Tecla T abre/fecha porta
- Detecção de proximidade ao ponto
- Animação básica de porta
- Crianças entram/saem (placeholder)

**Critérios de Aceite:**
- [ ] T funciona apenas próximo ao ponto
- [ ] Feedback visual de possibilidade de interação
- [ ] Animação de porta suave
- [ ] Contador de passageiros atualiza

**Referência:** `docs/game/design/mechanics.md` (Ponto de Ônibus)

---

#### #12 - Sistema de Tempo e Relógio
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 5 SP
**Dependências:** #10

**Descrição:**
Implementar sistema de tempo in-game:
- Relógio que avança (06:00-07:00 manhã)
- Tecla SPACE mostra relógio (5s)
- Cooldown de 20s
- Win/Lose conditions baseadas em tempo

**Critérios de Aceite:**
- [ ] Tempo avança corretamente
- [ ] UI de relógio funcional
- [ ] Cooldown funciona
- [ ] Fail state ao exceder tempo limite

**Referência:** `docs/game/design/mechanics.md` (Relógio)

---

### 🟠 ALTA

#### #13 - Implementar Mapa Interativo
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #10

**Descrição:**
Criar mapa que o jogador pode consultar:
- Tecla TAB para toggle
- Mostra rota e localização atual
- Trade-off: bloqueia parte da visão
- Pontos de parada marcados

**Critérios de Aceite:**
- [ ] TAB abre/fecha mapa
- [ ] Localização em tempo real
- [ ] Visão parcialmente bloqueada
- [ ] Pontos de parada visíveis

**Referência:** `docs/game/design/mechanics.md` (Mapa)

---

#### #14 - Sistema de Retrovisor
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 5 SP
**Dependências:** #9

**Descrição:**
Implementar retrovisor funcional:
- Tecla E (hold) foca no retrovisor
- Camera DOF/blur na estrada ao focar
- Renderização do interior do ônibus
- Mancha distorce reflexo

**Critérios de Aceite:**
- [ ] E muda foco para retrovisor
- [ ] Visão da rua desfocada ao focar
- [ ] Interior visível no reflexo
- [ ] Soltar E retorna foco normal

**Referência:** `docs/game/design/mechanics.md` (Retrovisor)

---

#### #15 - Sistema de Câmera de Segurança
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #9

**Descrição:**
Modelar monitor de câmera com chiado:
- Tecla C dá "tapinha" (5s de clareza)
- Cooldown de 15s
- Efeito de estática/chiado
- Renderização da parte traseira do ônibus

**Critérios de Aceite:**
- [ ] C limpa estática por 5s
- [ ] Cooldown de 15s funciona
- [ ] Efeito visual de estática
- [ ] Mostra interior traseiro

**Referência:** `docs/game/design/mechanics.md` (Câmera)

---

#### #16 - Sistema de Farol
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 5 SP
**Dependências:** #9

**Descrição:**
Implementar farol do ônibus:
- Tecla F liga farol por 15s
- Cooldown de 20s
- Ilumina estrada (período noturno)
- Feedback visual claro

**Critérios de Aceite:**
- [ ] F ativa farol por 15s
- [ ] Cooldown de 20s
- [ ] Iluminação aumenta visivelmente
- [ ] Som de clique ao ativar

**Referência:** `docs/game/design/mechanics.md` (Farol)

---

## 👻 EPIC 3: Sistema de Assombrações

### 🔴 CRÍTICO

#### #17 - Modelar e Animar CRIANÇA 1
**Tipo:** Art + Animation
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #4, #19

**Descrição:**
Criar modelo e animações da CRIANÇA 1:
- Modelo Low Poly (menino pálido, anos 90)
- Rig básico
- Animação: idle, aparecer/desaparecer, troca de lugar
- Material translúcido

**Critérios de Aceite:**
- [ ] Modelo completo e dentro do style guide
- [ ] 3 animações funcionais
- [ ] Material shader funciona
- [ ] Integrado na engine

---

#### #18 - Implementar Comportamento CRIANÇA 1
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #17

**Descrição:**
Sistema de IA da CRIANÇA 1:
- Aparece em locais aleatórios
- Troca de lugar constantemente
- Som característico "hihihi"
- Gatilho baseado em tensão

**Critérios de Aceite:**
- [ ] Aparece em intervalos definidos
- [ ] Muda de posição visivelmente
- [ ] Som sincronizado com aparição
- [ ] Responde ao microfone (contramedida)

**Referência:** `docs/game/narrative/characters.md` (CRIANÇA 1)

---

#### #19 - Modelar e Animar CRIANÇA 2
**Tipo:** Art + Animation
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #4

**Descrição:**
Criar modelo CRÍTICO (Game Over):
- Menino pálido, roupas úmidas
- Mão estendida em direção ao painel
- Animação: aparecer ao lado, alcançar botões
- Olhar fixo/vazio

**Critérios de Aceite:**
- [ ] Modelo aterrorizante e polido
- [ ] Animação de alcançar botões fluida
- [ ] Material "úmido" convincente
- [ ] Jumpscare efetivo testado

---

#### #20 - Implementar Comportamento CRIANÇA 2 (Game Over)
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #19

**Descrição:**
IA CRÍTICA com Game Over:
- Aparece ao lado do motorista
- Tenta pressionar "botão vermelho"
- 2-3 segundos para responder
- Falha = Game Over imediato

**Critérios de Aceite:**
- [ ] Aparição súbita ao lado
- [ ] Timer de 2-3s visível
- [ ] Trava de painel cancela ação
- [ ] Game Over funciona se falhar

**Referência:** `docs/game/design/mechanics.md` (Tabela de Ameaças)

---

#### #21 - Modelar e Animar CRIANÇA 3
**Tipo:** Art + Animation
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #4

**Descrição:**
Criar modelo etéreo:
- Boca aberta em "grito silencioso"
- Forma mais distorcida/fantasmagórica
- Animação: flutuação, pulsação
- Menos sólido que outras 2

**Critérios de Aceite:**
- [ ] Visual etéreo convincente
- [ ] Animação "glitchy"/distorcida
- [ ] Fica predominantemente no fundo
- [ ] Shader de distorção funcional

---

#### #22 - Implementar Comportamento CRIANÇA 3 (Áudio)
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #21

**Descrição:**
Sistema de ataque sonoro:
- Emite ruído perturbador + estática
- Distorce sons ambientes
- Dispara em intervalos
- Rádio como contramedida

**Critérios de Aceite:**
- [ ] Som causa desconforto (testado)
- [ ] Distorção de áudio ambiente funciona
- [ ] Visual glitch sutil ao ativar
- [ ] Rádio cancela efeito

**Referência:** `docs/game/narrative/characters.md` (CRIANÇA 3)

---

### 🟠 ALTA

#### #23 - Sistema de Contramedidas (Objetos de Intervenção)
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 13 SP
**Dependências:** #18, #20, #22

**Descrição:**
Implementar 3 ferramentas de defesa:
- **Microfone (Q, 3s hold):** vs CRIANÇA 1
- **Trava Painel (SHIFT):** vs CRIANÇA 2
- **Rádio (R):** vs CRIANÇA 3

Cada um com cooldown específico.

**Critérios de Aceite:**
- [ ] 3 inputs funcionam como especificado
- [ ] Cooldowns (15s, 20s, 25s) corretos
- [ ] Feedback visual de disponibilidade
- [ ] Efetivo contra assombração correta

**Referência:** `docs/game/design/mechanics.md` (Objetos de Intervenção)

---

#### #24 - UI de Cooldowns
**Tipo:** Feature - UI
**Prioridade:** P1
**Estimativa:** 5 SP
**Dependências:** #23

**Descrição:**
Interface mostrando status das ferramentas:
- Ícones das 3 contramedidas
- Indicador de cooldown (circular/linear)
- Highlight quando disponível
- Opacidade quando em cooldown

**Critérios de Aceite:**
- [ ] 3 ícones sempre visíveis
- [ ] Cooldown animado claramente
- [ ] Feedback tátil ao usar (som/screen shake)
- [ ] Design não intrusivo

---

## 📈 EPIC 4: Sistema de Progressão

### 🔴 CRÍTICO

#### #25 - Sistema de Tensão/Insanidade
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #12

**Descrição:**
Implementar sistema central de tensão:
- Valor inicial por dia + período (tabela)
- Modificadores (+10%, +15%, etc.)
- Aumento gradual se errar contramedidas
- Decremento se finalizar com tempo sobrando

**Critérios de Aceite:**
- [ ] Tensão inicia nos valores da tabela
- [ ] Erros aumentam tensão corretamente
- [ ] UI mostra % de tensão (no relógio)
- [ ] Game Over ao atingir 100%

**Referência:** `docs/game/design/balancing.md` (Sistema de Tensão)

---

#### #26 - Efeitos Visuais de Tensão
**Tipo:** Feature - Polish
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #25

**Descrição:**
Feedback visual baseado no nível de tensão:
- 0-25%: Normal
- 26-50%: Distorção nas bordas
- 51-75%: Visão periférica escurecida
- 76-99%: Visão túnel, dessaturação
- 100%: Tela vermelha pulsante

**Critérios de Aceite:**
- [ ] 5 estados visuais distintos
- [ ] Transição suave entre estados
- [ ] Performance não afetada
- [ ] Ajustável para acessibilidade

**Referência:** `docs/game/design/balancing.md` (Balanceamento Visual/Sonoro)

---

#### #27 - Efeitos Sonoros de Tensão
**Tipo:** Feature - Audio
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #25

**Descrição:**
Áudio que responde à tensão:
- 0-25%: Ambiente tranquilo
- 26-50%: Batimentos cardíacos baixos
- 51-75%: Sussurros + batimentos altos
- 76-99%: Gritos abafados + estática
- 100%: Som estridente

**Critérios de Aceite:**
- [ ] Áudio escala com tensão
- [ ] Mixagem não sobrepõe elementos críticos
- [ ] Teste com jogadores (efetivo?)
- [ ] Volume ajustável separadamente

**Referência:** `docs/game/design/balancing.md` (Indicadores de Tensão)

---

#### #28 - Sistema de Dias e Períodos
**Tipo:** Feature
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #12, #25

**Descrição:**
Gerenciador de progressão 5 dias × 2 períodos:
- Manhã (06:00-07:00)
- Tarde (18:00-19:30)
- Salvar progresso entre dias
- Transições narrativas

**Critérios de Aceite:**
- [ ] 10 períodos jogáveis sequencialmente
- [ ] Save/Load funciona entre períodos
- [ ] Tensão inicial correta por período
- [ ] UI mostra dia/período atual

**Referência:** `docs/game/design/balancing.md` (Progressão)

---

### 🟠 ALTA

#### #29 - Balanceamento de Dificuldade
**Tipo:** Balancing
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #18, #20, #22, #25

**Descrição:**
Ajustar frequência de ataques por dia:
- Dia 1: 1-2 ataques/período (tutorial)
- Dia 2: 2-3 ataques
- Dia 3: Ataques combinados
- Dia 4: 3-4 ataques, cooldowns críticos
- Dia 5: Ataques simultâneos

**Critérios de Aceite:**
- [ ] Curva de dificuldade testada
- [ ] Taxa de conclusão dentro das metas
- [ ] Logs de telemetria implementados
- [ ] Ajustes iterativos documentados

**Referência:** `docs/game/design/balancing.md` (Curva de Dificuldade)

---

#### #30 - Sistema de Save/Load
**Tipo:** Feature
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #28

**Descrição:**
Persistência de progresso:
- Auto-save ao completar período
- Slots de save manual (3x)
- Salva: dia, tensão, progresso narrativo
- Tela de continue/new game

**Critérios de Aceite:**
- [ ] Auto-save ao fim de cada período
- [ ] Load restaura estado corretamente
- [ ] UI de gerenciamento de saves
- [ ] Compatibilidade entre versões

---

## 🎨 EPIC 5: Arte e Ambiente

### 🔴 CRÍTICO

#### #31 - Modelar Ônibus 104
**Tipo:** Art
**Prioridade:** P0
**Estimativa:** 20 SP
**Dependências:** #4

**Descrição:**
Criar modelo principal do jogo:
- Exterior Low Poly (pintura amarela descascada)
- Interior completo (cockpit + bancos)
- Detalhes: ferrugem, mancha no retrovisor
- LODs para performance

**Critérios de Aceite:**
- [ ] Modelo dentro do poly budget
- [ ] Interior navegável em primeira pessoa
- [ ] Detalhes atmosféricos presentes
- [ ] 3 LODs criados

---

#### #32 - Modelar Ambiente - Estacionamento Escola
**Tipo:** Art
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #4

**Descrição:**
Cenário inicial (Dia 1 manhã):
- Prédio da escola (fachada básica)
- Estacionamento
- 1-2 árvores
- Céu cinzento

**Critérios de Aceite:**
- [ ] Suficiente para cutscene introdutória
- [ ] Atmosfera desolada/cinzenta
- [ ] Performance estável (60 FPS target)
- [ ] Iluminação básica funcional

---

#### #33 - Modelar Rota Completa (5 Pontos + Estrada)
**Tipo:** Art
**Prioridade:** P0
**Estimativa:** 20 SP
**Dependências:** #4, #10

**Descrição:**
Criar ambiente da rota:
- Estrada asfaltada com sinalização
- 5 pontos de ônibus distintos
- Árvores e vegetação (Low Poly)
- Casas/prédios ao fundo (distantes)

**Critérios de Aceite:**
- [ ] Rota completa navegável
- [ ] Cada ponto visualmente distinto
- [ ] Atmosfera adequada (Dia vs Noite)
- [ ] Otimizado (draw calls baixos)

---

#### #34 - Iluminação - Dia vs Noite
**Tipo:** Art - Lighting
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #32, #33

**Descrição:**
Criar 2 setups de iluminação:
- **Dia:** Céu cinzento, sombras suaves
- **Noite:** Escuridão, postes de luz, farol necessário

**Critérios de Aceite:**
- [ ] Iluminação dinâmica funciona
- [ ] Noite genuinamente dificulta visão
- [ ] Farol do ônibus efetivo à noite
- [ ] Performance mantida

---

### 🟠 ALTA

#### #35 - Modelar Crianças Vivas (NPCs)
**Tipo:** Art
**Prioridade:** P1
**Estimativa:** 13 SP
**Dependências:** #4

**Descrição:**
Criar ~5 modelos de crianças normais:
- Variação de roupas/cabelos
- Mochilas
- Rigs básicos para animações
- Expressões neutras

**Critérios de Aceite:**
- [ ] Pelo menos 3 modelos únicos
- [ ] Podem ser instanciados sem lag
- [ ] Contraste claro vs assombrações
- [ ] Animações: sentar, conversar

---

#### #36 - Criar Materiais e Texturas
**Tipo:** Art
**Prioridade:** P1
**Estimativa:** 13 SP
**Dependências:** #31, #33

**Descrição:**
Texturizar todos os assets:
- Ônibus (pintura descascada, ferrugem)
- Estrada e ambiente
- Personagens
- Paleta de cores consistente

**Critérios de Aceite:**
- [ ] Todas as texturas dentro do style guide
- [ ] Resolução otimizada (1K/2K)
- [ ] Materiais PBR básicos
- [ ] Tiling mínimo visível

---

#### #37 - UI/UX - HUD e Menus
**Tipo:** UI/UX
**Prioridade:** P1
**Estimativa:** 13 SP
**Dependências:** #12, #24

**Descrição:**
Criar interface completa:
- HUD in-game (mínimo)
- Menu principal
- Tela de pause
- UI de relógio e tensão
- Telas de finais

**Critérios de Aceite:**
- [ ] Navegação intuitiva testada
- [ ] Estilo visual coerente (Low Poly)
- [ ] Acessibilidade (tamanho de fonte)
- [ ] Todos os menus funcionais

---

## 🔊 EPIC 6: Áudio e Atmosfera

### 🔴 CRÍTICO

#### #38 - SFX Essenciais
**Tipo:** Audio
**Prioridade:** P0
**Estimativa:** 13 SP

**Descrição:**
Criar efeitos sonoros obrigatórios:
- Motor do ônibus (idle, acelerar, frear)
- Porta abrindo/fechando
- Sons de contramedidas (microfone, trava, rádio)
- "Hihihi" da CRIANÇA 1
- Ruído da CRIANÇA 3

**Critérios de Aceite:**
- [ ] Todos os sons listados implementados
- [ ] Mixagem balanceada
- [ ] Feedback tátil (punchy)
- [ ] Formatos otimizados

---

#### #39 - Trilha Sonora - Temas Principais
**Tipo:** Audio - Music
**Prioridade:** P0
**Estimativa:** 20 SP

**Descrição:**
Compor/licenciar música:
- Tema principal (menus)
- Música ambiente (gameplay)
- Música de tensão elevada
- Temas para os 3 finais

**Critérios de Aceite:**
- [ ] Pelo menos 4 tracks
- [ ] Responde dinamicamente à tensão
- [ ] Loops suaves
- [ ] Direitos de uso claros

---

### 🟠 ALTA

#### #40 - Áudio Ambiente
**Tipo:** Audio
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #38

**Descrição:**
Sons atmosféricos:
- Vento, pássaros (dia)
- Grilos, folhas (noite)
- Estática do rádio
- Sussurros distantes

**Critérios de Aceite:**
- [ ] 10+ sons ambiente
- [ ] Posicionamento 3D funcional
- [ ] Contribui para atmosfera
- [ ] Não cansa a audição

---

#### #41 - Sistema de Áudio Dinâmico
**Tipo:** Feature - Audio
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #27, #39

**Descrição:**
Música e áudio reagem ao gameplay:
- Intensidade escala com tensão
- Layers adicionados progressivamente
- Transições suaves entre estados
- Sincronização com eventos

**Critérios de Aceite:**
- [ ] Sistema de layers funcional
- [ ] Transições imperceptíveis
- [ ] Performance otimizada
- [ ] Mixagem profissional

---

## 📚 EPIC 7: Narrativa e Cutscenes

### 🔴 CRÍTICO

#### #42 - Cutscene - Introdução (Dia 1 Manhã)
**Tipo:** Narrative - Cutscene
**Prioridade:** P0
**Estimativa:** 13 SP
**Dependências:** #2, #32

**Descrição:**
Primeira cutscene com Diretor Siqueira:
- Diálogo completo implementado
- Camera cinematográfica
- Animações de personagens
- Transição para gameplay

**Critérios de Aceite:**
- [ ] Duração 2-3 minutos
- [ ] Diálogo legendado
- [ ] Skipável (após primeira vez)
- [ ] Estabelece tom do jogo

**Referência:** `docs/game/narrative/story.md` (Dia 1 Manhã)

---

#### #43 - Jumpscare - CRIANÇA 2 (Dia 1 Tarde)
**Tipo:** Narrative - Event
**Prioridade:** P0
**Estimativa:** 8 SP
**Dependências:** #19, #42

**Descrição:**
Primeiro jumpscare controlado:
- Menino pálido aparece ao lado
- QTE: apertar SHIFT ou Game Over
- Reação das crianças vivas
- Momento marcante

**Critérios de Aceite:**
- [ ] Jumpscare efetivo (testado)
- [ ] QTE justo (2-3s)
- [ ] Diálogo das crianças após
- [ ] Introduz mecânica CRIANÇA 2

**Referência:** `docs/game/narrative/story.md` (Dia 1 Tarde)

---

#### #44 - Implementar 3 Finais
**Tipo:** Narrative - Endings
**Prioridade:** P0
**Estimativa:** 20 SP
**Dependências:** #2, #28

**Descrição:**
Criar cutscenes dos 3 finais:
- **Bom:** "A Última Parada" (~3 min)
- **Médio:** "Aposentadoria Precoce" (~2 min)
- **Ruim:** "O Ciclo" (~2 min)

**Critérios de Aceite:**
- [ ] 3 cutscenes distintas
- [ ] Triggering correto baseado em score
- [ ] Impacto emocional testado
- [ ] Créditos após finais

**Referência:** `docs/game/narrative/endings.md`

---

### 🟠 ALTA

#### #45 - Eventos Narrativos (Dias 2-5)
**Tipo:** Narrative
**Prioridade:** P1
**Estimativa:** 20 SP
**Dependências:** #2, #28

**Descrição:**
Implementar progressão narrativa:
- Diálogos/monólogos internos por dia
- Revelações graduais sobre acidente
- Collectibles (documentos, jornais?)
- Reações do protagonista

**Critérios de Aceite:**
- [ ] Cada dia tem eventos únicos
- [ ] Lore se revela progressivamente
- [ ] Opcional: collectibles implementados
- [ ] Diálogos legendados

---

#### #46 - Dialogos de NPCs
**Tipo:** Narrative
**Prioridade:** P1
**Estimativa:** 8 SP
**Dependências:** #42

**Descrição:**
Implementar interações com NPCs:
- Diretor Siqueira (dias posteriores)
- Mecânico (se encontrar)
- Crianças vivas (comentários)

**Critérios de Aceite:**
- [ ] Sistema de diálogo funcional
- [ ] Voices ou legendas
- [ ] Diálogos contextuais
- [ ] Skipável

---

## 🧪 EPIC 8: Testes e Polimento

### 🟢 ÚLTIMA FASE

#### #47 - Alpha Testing Interno
**Tipo:** QA
**Prioridade:** P2
**Estimativa:** 13 SP
**Dependências:** Todos críticos completos

**Descrição:**
Primeira rodada de testes:
- Playthrough completo (5 dias)
- Bug hunting
- Balance feedback
- Performance profiling

**Critérios de Aceite:**
- [ ] 10+ playthroughs completos
- [ ] Bugs críticos catalogados
- [ ] Métricas de dificuldade coletadas
- [ ] Relatório de performance

---

#### #48 - Balanceamento Final
**Tipo:** Balancing
**Prioridade:** P2
**Estimativa:** 8 SP
**Dependências:** #47

**Descrição:**
Ajustes baseados em dados:
- Tensão inicial por dia
- Cooldowns das ferramentas
- Frequência de ataques
- Tempo limite dos períodos

**Critérios de Aceite:**
- [ ] Taxas de conclusão dentro das metas
- [ ] Curva de dificuldade satisfatória
- [ ] Feedback de testers positivo
- [ ] Mudanças documentadas

**Referência:** `docs/game/design/balancing.md` (Teste de Balanceamento)

---

#### #49 - Polish Pass - Visual
**Tipo:** Polish
**Prioridade:** P2
**Estimativa:** 13 SP
**Dependências:** #47

**Descrição:**
Refinamento visual:
- Partículas (poeira, chuva?)
- Post-processing (color grading)
- Detalhes ambientes
- Animações secundárias

**Critérios de Aceite:**
- [ ] Visual coeso e polido
- [ ] Performance mantida
- [ ] Feedback "juice" adequado
- [ ] Comparação antes/depois

---

#### #50 - Polish Pass - Audio
**Tipo:** Polish
**Prioridade:** P2
**Estimativa:** 8 SP
**Dependências:** #47

**Descrição:**
Refinamento sonoro:
- Mixagem profissional
- Reverb e espacialização
- Foley adicional
- Mastering da música

**Critérios de Aceite:**
- [ ] Mixagem balanceada
- [ ] Áudio contribui para imersão
- [ ] Sem clipping ou distorção
- [ ] Aprovação de sound designer

---

#### #51 - Beta Testing Externo
**Tipo:** QA
**Prioridade:** P2
**Estimativa:** 20 SP
**Dependências:** #48, #49, #50

**Descrição:**
Testes com jogadores externos:
- 20+ testers
- Múltiplas configurações de hardware
- Feedback estruturado (formulários)
- Telemetria completa

**Critérios de Aceite:**
- [ ] Mínimo 20 playthroughs completos
- [ ] Bugs críticos = 0
- [ ] Feedback compilado e analisado
- [ ] Ajustes finais implementados

---

#### #52 - Preparar Release Build
**Tipo:** Technical
**Prioridade:** P2
**Estimativa:** 8 SP
**Dependências:** #51

**Descrição:**
Build final para lançamento:
- Otimização de assets
- Compressão de builds
- Testes de instalação
- Store pages (Steam/etc)

**Critérios de Aceite:**
- [ ] Build instala sem erros
- [ ] Tamanho otimizado (<2GB?)
- [ ] Achievements funcionais (se aplicável)
- [ ] Store assets preparados

---

## 📊 Resumo de Estimativas

| Épico | Story Points | Prioridade |
|-------|-------------|-----------|
| 1. Documentação | 29 SP | 🔴 CRÍTICA |
| 2. Mecânicas Core | 70 SP | 🔴 CRÍTICA |
| 3. Assombrações | 96 SP | 🔴 CRÍTICA |
| 4. Progressão | 66 SP | 🔴 CRÍTICA |
| 5. Arte | 113 SP | 🟠 ALTA |
| 6. Áudio | 49 SP | 🟠 ALTA |
| 7. Narrativa | 69 SP | 🟠 ALTA |
| 8. Testes | 70 SP | 🟢 ÚLTIMA FASE |
| **TOTAL** | **562 SP** | |

**Velocity estimada:** 20-30 SP/sprint
**Duração estimada:** 19-28 sprints (~5-7 meses)

---

## 🎯 Milestones

### Milestone 1: Documentação Completa (Sprint 1-2)
- [ ] Todos os issues do Epic 1 completos
- [ ] GDD finalizado
- [ ] Stakeholders aprovam

### Milestone 2: Vertical Slice - Dia 1 (Sprint 3-8)
- [ ] Direção funcional
- [ ] Rota básica com 3 pontos
- [ ] 1 assombração implementada
- [ ] Narrativa Dia 1 completa
- **DEMO PLAYÁVEL**

### Milestone 3: MVP - 5 Dias Completos (Sprint 9-16)
- [ ] 3 assombrações funcionais
- [ ] 5 dias × 2 períodos
- [ ] Sistema de tensão completo
- [ ] Arte placeholder/básica
- **ALPHA TESTÁVEL**

### Milestone 4: Content Complete (Sprint 17-22)
- [ ] Toda arte finalizada
- [ ] Todos os áudios implementados
- [ ] 3 finais funcionais
- [ ] Polish pass inicial
- **BETA BUILD**

### Milestone 5: Gold Master (Sprint 23-28)
- [ ] Todos os bugs críticos corrigidos
- [ ] Balanceamento finalizado
- [ ] Testes externos completos
- [ ] Release build preparado
- **GAME RELEASED**

---

## 📝 Notas de Implementação

### Priorização Sugerida

**Sprint 1-2:** Epic 1 completo (Documentação)
**Sprint 3-5:** #8-12 (Setup + Mecânicas Básicas)
**Sprint 6-8:** #17-20 (Primeira Assombração)
**Sprint 9-11:** #21-23 (3 Assombrações + Contramedidas)
**Sprint 12-14:** #25-28 (Progressão e Dias)
**Sprint 15-18:** Epic 5 (Arte)
**Sprint 19-22:** Epic 7 (Narrativa)
**Sprint 23-25:** Epic 8 (Testes)
**Sprint 26-28:** Polish e Release

### Dependências Críticas

1. **#4 (Style Guide)** bloqueia toda a arte
2. **#8 (Setup)** bloqueia toda implementação
3. **#2 (Narrativa completa)** necessária antes de cutscenes
4. **MVP (#28)** antes de testes externos

---

**Última revisão:** 2026-03-01
**Mantenedor:** [Nome do PM/Lead]

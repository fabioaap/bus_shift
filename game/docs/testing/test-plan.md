# Bus Shift — Plano de Testes Completo

> **Versão:** 1.0.0  
> **Responsável:** Quinn (QA Lead)  
> **Engine:** Unity 6000.3.2f1 | **Plataforma:** Windows (Steam)  
> **Última Atualização:** 2026-03-01  
> **Status:** 🟡 Em Elaboração

---

## Índice

1. [Objetivos de Teste](#1-objetivos-de-teste)
2. [Test Cases por Mecânica](#2-test-cases-por-mecânica)
   - 2.1 [Controles de Direção](#21-controles-de-direção)
   - 2.2 [Sistema de Embarque e Desembarque](#22-sistema-de-embarque-e-desembarque)
   - 2.3 [Ferramentas de Observação](#23-ferramentas-de-observação)
   - 2.4 [Contramedidas e Cooldowns](#24-contramedidas-e-cooldowns)
   - 2.5 [Fantasmas — Triggers e Comportamentos](#25-fantasmas--triggers-e-comportamentos)
   - 2.6 [Sistema de Tensão](#26-sistema-de-tensão)
   - 2.7 [Progressão de Dias e Períodos](#27-progressão-de-dias-e-períodos)
   - 2.8 [Sistema de Save e Load](#28-sistema-de-save-e-load)
   - 2.9 [Finais do Jogo](#29-finais-do-jogo)
3. [Matriz de Compatibilidade](#3-matriz-de-compatibilidade)
4. [Procedimentos de QA](#4-procedimentos-de-qa)
5. [Ferramentas de Teste](#5-ferramentas-de-teste)
6. [Critérios de Aceite por Sistema](#6-critérios-de-aceite-por-sistema)

---

## 1. Objetivos de Teste

### 1.1 Propósito
Garantir que todas as mecânicas de **Bus Shift** funcionem conforme especificado em `mechanics.md` e `balancing.md`, que a curva de dificuldade atinja as metas de balanceamento, e que a experiência de horror survival entregue tensão progressiva e consistente ao longo dos 5 dias × 2 períodos.

### 1.2 Escopo

| Categoria | Incluído | Excluído |
|-----------|----------|----------|
| Mecânicas de jogo (driving, tools, ghosts) | ✅ | — |
| Sistema de tensão e balanceamento | ✅ | — |
| Progressão narrativa (5 dias) | ✅ | Conteúdo de texto/roteiro |
| 3 finais possíveis | ✅ | — |
| Save/Load | ✅ | Backup cloud (Steam) |
| Compatibilidade Windows 10/11 | ✅ | macOS/Linux |
| Performance (60fps, <2GB RAM) | ✅ | Mobile |
| Resolução 720p, 1080p, 1440p | ✅ | 4K (não mapeado) |
| Acessibilidade | ❌ | Fora do escopo v1.0 |

### 1.3 Métricas de Sucesso (Metas de Balanceamento)

| Métrica | Meta | Fonte |
|---------|------|-------|
| Taxa de conclusão — Dia 1 | ≥ 90% | `balancing.md` |
| Taxa de conclusão — Dia 3 | 60–70% | `balancing.md` |
| Taxa de conclusão — Dia 5 | 30–40% | `balancing.md` |
| Tempo médio por período | ~12 min (máx 15 min) | `balancing.md` |
| Tensão média ao final do período (jogador competente) | 70–80% | `balancing.md` |
| Taxa de crash (Game Over por CRIANÇA 2) | < 50% dos jogadores novos Dia 1 | QA estimativa |
| Framerate estável | ≥ 60 FPS constante | `known-issues.md` TECH-005 |
| Uso de RAM | < 2 GB | `known-issues.md` TECH-005 |
| Todos os 3 finais acessíveis | 100% dos caminhos válidos | Game Design |
| Zero bugs P0 abertos ao lançamento | 0 | Política QA |

### 1.4 Critérios de Entrada/Saída de Testes

| Critério | Entrada (pode testar) | Saída (aprovado) |
|----------|----------------------|------------------|
| Build compilável sem erros | ✅ obrigatório | ✅ obrigatório |
| Todos P0 resolvidos | ✅ obrigatório | ✅ obrigatório |
| Todos P1 resolvidos ou mitigados | Não obrigatório entrada | ✅ obrigatório saída |
| Documentação de design atualizada | Recomendado | ✅ obrigatório |
| Smoke test passando | ✅ obrigatório | ✅ obrigatório |

---

## 2. Test Cases por Mecânica

### Legenda de Prioridade

| Prioridade | Descrição |
|------------|-----------|
| **P0** | Crítico — falha bloqueia jogabilidade ou causa crash |
| **P1** | Alto — funcionalidade principal com impacto severo |
| **P2** | Médio — impacto moderado, workaround possível |
| **P3** | Baixo — cosmético, melhoria de UX |

---

### 2.1 Controles de Direção

**Sistema:** W/A/S/D — Física de veículo pesado (ônibus escolar)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-001** | Jogo iniciado, veículo parado | 1. Pressionar e segurar **W** | Ônibus acelera progressivamente; som de motor aumenta; animação de movimento ativa | P0 |
| **TC-002** | Ônibus em movimento (~30 km/h) | 1. Pressionar **S** | Velocidade diminui gradualmente (freio); ônibus não faz ré imediato | P0 |
| **TC-003** | Ônibus completamente parado | 1. Pressionar **S** | Ônibus inicia movimento de ré; som de marcha ré ativo | P0 |
| **TC-004** | Ônibus em movimento para frente | 1. Pressionar **A** | Ônibus vira à esquerda; resistência de curva perceptível (física veículo pesado) | P0 |
| **TC-005** | Ônibus em movimento para frente | 1. Pressionar **D** | Ônibus vira à direita; resistência de curva perceptível | P0 |
| **TC-006** | Ônibus em movimento | 1. Pressionar **A** e **W** simultaneamente | Ônibus acelera enquanto vira à esquerda; sem conflito de input | P1 |
| **TC-007** | Ônibus em movimento | 1. Pressionar **D** e **W** simultaneamente | Ônibus acelera enquanto vira à direita; sem conflito de input | P1 |
| **TC-008** | Ônibus em velocidade máxima | 1. Soltar **W** | Ônibus desacelera gradualmente (inércia de veículo pesado) | P1 |
| **TC-009** | Ônibus em movimento de ré | 1. Pressionar **A** | Steering reverso coerente com perspectiva de câmera de ré | P1 |
| **TC-010** | Ônibus parado em ladeira | 1. Soltar todos os controles | Ônibus mantém posição (freio de estacionamento implícito) | P2 |
| **TC-011** | Ônibus colidindo com obstáculo fixo | 1. Acelerar em direção a parede/obstáculo | Colisão registrada; som de impacto; sem atravessar geometria | P0 |
| **TC-012** | Período noturno sem farol ativo | 1. Dirigir na rota normalmente | Visibilidade reduzida confirmada; objetos a frente difíceis de ver; HUD não indica caminho | P1 |

---

### 2.2 Sistema de Embarque e Desembarque

**Sistema:** Tecla T — interação com pontos de ônibus (bus stops)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-013** | Ônibus parado próximo ao ponto válido (raio de interação) | 1. Pressionar **T** | Porta abre (animação + som); crianças entram/saem automaticamente | P0 |
| **TC-014** | Ônibus em movimento a > 50m do ponto | 1. Pressionar **T** | Nenhuma ação; sem animação; sem som de porta | P1 |
| **TC-015** | Ônibus parado, sem ponto próximo | 1. Pressionar **T** | Nenhuma ação; feedback visual/sonoro sutil de "sem ponto" | P2 |
| **TC-016** | Período MANHÃ (embarque) — parado no ponto | 1. Pressionar **T** | Criança(s) embarcam; contador de passageiros atualiza | P0 |
| **TC-017** | Período NOITE (desembarque) — parado no ponto correto | 1. Pressionar **T** | Criança(s) desembarcam no ponto correto; progresso de rota atualiza | P0 |
| **TC-018** | Período NOITE — tentativa de desembarque no ponto errado | 1. Parar no ponto errado e pressionar **T** | Porta pode abrir, mas criança não desembarca OU indicação de ponto incorreto | P1 |
| **TC-019** | Porta aberta durante evento de CRIANÇA 2 (Emma) | 1. Abrir porta **T** quando Emma está ativa | Emma pode usar a porta aberta como vetor de ataque — verificar se behavior de Emma escala | P1 |
| **TC-020** | Todos os 5 pontos de embarque (manhã) completados | 1. Embarcar crianças em todos os 5 pontos | Rota de manhã marcada como completa; transição para garage/período | P0 |

---

### 2.3 Ferramentas de Observação

#### 2.3.1 Retrovisor (E)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-021** | Jogo em andamento, ônibus em movimento | 1. Segurar **E** | Câmera foca no retrovisor; visão traseira do ônibus exibida; visão frontal reduzida | P1 |
| **TC-022** | Segurando **E** (retrovisor ativo) | 1. Soltar **E** | Câmera retorna à posição frontal normal; sem delay perceptível | P1 |
| **TC-023** | Grace Harper ativa (Observadora) | 1. Tentar usar retrovisor **E** | Retrovisor bloqueado/distorcido por Grace; comportamento de bloqueio confirmado | P0 |
| **TC-024** | Retrovisor ativo durante curva | 1. Segurar **E** e pressionar **A** ou **D** | Controles de direção mantêm responsividade; retrovisor não interfere no steering | P1 |

#### 2.3.2 Câmera de Segurança (C)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-025** | Câmera com chiado (distorção ativa), cooldown zero | 1. Pressionar **C** | Imagem fica nítida por 5 segundos; som de "tapa no monitor" reproduzido | P1 |
| **TC-026** | Imediatamente após usar **C** | 1. Pressionar **C** novamente | Nenhuma ação; indicador de cooldown exibido (15s) | P1 |
| **TC-027** | Aguardar 15 segundos após uso de **C** | 1. Pressionar **C** novamente | Cooldown zerado; câmera pode ser usada novamente | P1 |
| **TC-028** | Grace Harper ativa (Observadora) | 1. Pressionar **C** | Câmera bloqueada/corrompida por Grace; sem claridade mesmo após tapa | P0 |
| **TC-029** | Câmera ativa (5s de claridade) | 1. Verificar comportamento de fantasmas visíveis | Atividade de fantasmas detectável durante janela de claridade; informação útil ao jogador | P2 |

#### 2.3.3 Farol (F)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-030** | Período noturno, cooldown do farol zerado | 1. Pressionar **F** | Ambiente iluminado por 15 segundos; som de clique; visibilidade aumenta | P1 |
| **TC-031** | Farol ativo (dentro dos 15s) | 1. Pressionar **F** novamente | Nenhuma ação; cooldown em andamento (20s total) | P1 |
| **TC-032** | Período diurno (manhã) | 1. Pressionar **F** | Efeito mínimo ou sem efeito visual notável; não deve causar erro | P2 |
| **TC-033** | Aguardar 20 segundos após ativação do farol | 1. Pressionar **F** | Farol disponível novamente; cooldown completo | P1 |

#### 2.3.4 Relógio / Tensão (SPACE)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-034** | Cooldown do relógio zerado | 1. Segurar **SPACE** por 5 segundos | UI overlay aparece com horário atual e nível de tensão atual (%) | P1 |
| **TC-035** | Soltar **SPACE** antes dos 5s | 1. Pressionar e soltar **SPACE** em 2s | UI overlay não aparece; cooldown NÃO deve ser consumido | P1 |
| **TC-036** | Após UI de relógio aparecer | 1. Verificar dados exibidos | Hora in-game correta para o período (06:00–07:00 manhã / 18:00–19:30 noite) + tensão % precisa | P2 |
| **TC-037** | Imediatamente após uso completo do relógio | 1. Tentar usar SPACE novamente | Cooldown de 20s ativo; UI não reaparece | P1 |

#### 2.3.5 Mapa (TAB)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-038** | Mapa fechado | 1. Pressionar **TAB** | Mapa abre; rota atual e posição do ônibus visíveis; visão da estrada parcialmente bloqueada | P1 |
| **TC-039** | Mapa aberto | 1. Pressionar **TAB** novamente | Mapa fecha; visão normal restaurada | P1 |
| **TC-040** | Mapa aberto enquanto dirige | 1. Abrir mapa com **TAB** e continuar usando **W/A/S/D** | Controles de direção ainda funcionam com mapa aberto; trade-off de visão confirmado | P1 |
| **TC-041** | Mapa aberto, fantasma ativo | 1. Verificar se fantasmas aparecem no mapa | Fantasmas NÃO devem aparecer no mapa (informação oculta para manter tensão) | P2 |

---

### 2.4 Contramedidas e Cooldowns

#### 2.4.1 Microfone — Q (vs Marcus Reid / CRIANÇA 1)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-042** | Marcus ativo (trocando de lugar), cooldown Q zerado | 1. Segurar **Q** por 3 segundos | Som de microfone + voz do motorista reproduzidos; Marcus para de trocar de lugar temporariamente | P0 |
| **TC-043** | Soltar **Q** antes de 3 segundos | 1. Pressionar **Q** e soltar em 1.5s | Microfone não ativa; sem efeito em Marcus; cooldown NÃO consumido | P1 |
| **TC-044** | Marcus ativo, Q segurado 3s com sucesso | 1. Verificar impacto na tensão | Tensão NÃO aumenta (resposta bem-sucedida evita o +10% de penalidade) | P0 |
| **TC-045** | Marcus ativo, jogador ignora por > 8s | 1. Não usar Q dentro da janela de resposta (5–8s) | Tensão aumenta +10%; Marcus avança para próxima etapa de invasão | P0 |
| **TC-046** | Imediatamente após ativação do microfone | 1. Tentar usar **Q** novamente | Cooldown de 20s ativo; microfone não responde | P1 |
| **TC-047** | Aguardar 20 segundos após microfone | 1. Usar **Q** novamente | Microfone disponível; Marcus pode ser respondido novamente | P1 |

#### 2.4.2 Trava do Painel — SHIFT (vs Emma Lynch / CRIANÇA 2)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-048** | Emma ativa (tentando apertar botões), cooldown SHIFT zerado | 1. Pressionar **SHIFT** dentro de 2 segundos | Painel bloqueado por 3s; som de trava; indicador visual no painel; Emma impedida | P0 |
| **TC-049** | Emma ativa, jogador demora > 2s para reagir | 1. Não pressionar SHIFT dentro de 2 segundos | GAME OVER imediato; tela de game over exibida com contexto de Emma | P0 |
| **TC-050** | SHIFT ativado com sucesso | 1. Verificar indicador de cooldown | Cooldown de 15s visível no HUD; trava não pode ser reativada durante cooldown | P0 |
| **TC-051** | Trava em cooldown (15s), Emma ataca novamente | 1. Tentar pressionar **SHIFT** durante cooldown | Trava não responde; situação de risco máximo — verificar se o jogo é fair (Emma não deveria atacar 2x no mesmo cooldown) | P1 |
| **TC-052** | Aguardar 15 segundos após uso de SHIFT | 1. Pressionar **SHIFT** | Trava disponível novamente; indicador de cooldown zerado | P0 |

#### 2.4.3 Rádio — R (vs Thomas Sanders / CRIANÇA 3)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-053** | Thomas ativo (sussurros escalando), cooldown R zerado | 1. Pressionar **R** | Música ambiente toca; estática/sussurros diminuem; Thomas interrompido | P0 |
| **TC-054** | Thomas ativo, jogador ignora por > 8s | 1. Não usar R dentro de 8-10s | Tensão aumenta +15% após janela expirar | P0 |
| **TC-055** | Rádio ativado com sucesso | 1. Verificar cooldown após uso | Cooldown de 25s ativo (o mais longo do jogo); indicador no HUD | P1 |
| **TC-056** | Thomas ativo, Rádio em cooldown (25s) | 1. Tentar usar **R** durante cooldown | Rádio não ativa; tensão acumula; verificar se escalada de Thomas respeita o cooldown (fair play) | P1 |
| **TC-057** | Cenário: CRIANÇA 1 e CRIANÇA 3 atacam simultaneamente | 1. Decidir entre usar **Q** (+10% se falhar) ou **R** (+15% se falhar) | Sistema permite livre escolha do jogador; ambas penalidades aplicadas corretamente se ambas falharem | P1 |

#### 2.4.4 Validação Cruzada de Cooldowns

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-058** | Todas as contramedidas em cooldown | 1. Verificar estado do HUD | HUD exibe corretamente os 3 cooldowns simultaneamente sem sobreposição visual | P1 |
| **TC-059** | Usar todas as ferramentas em sequência rápida | 1. Q → SHIFT → R em sequência | Cada cooldown inicia individualmente; não há interferência entre cooldowns de ferramentas diferentes | P1 |
| **TC-060** | Build limpa, início de partida | 1. Verificar estado inicial de cooldowns | Todas as ferramentas disponíveis (cooldown = 0) no início de cada período | P1 |

---

### 2.5 Fantasmas — Triggers e Comportamentos

#### 2.5.1 Marcus Reid — "O Invasor" (CRIANÇA 1)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-061** | Dia 1 manhã ativo, Marcus presente no ônibus | 1. Observar comportamento via retrovisor **E** | Marcus troca de assento periodicamente; movimento detectável no retrovisor | P0 |
| **TC-062** | Marcus em modo ativo (avançando para cabine) | 1. Observar via retrovisor por 5+ segundos sem reagir | Marcus avança progressivamente em direção à cabine do motorista | P0 |
| **TC-063** | Marcus alcança a cabine (falha total) | 1. Deixar Marcus avançar sem resposta | Tensão aumenta substancialmente (confirmar valor exato com implementação); jumpsc are/evento assustador | P0 |
| **TC-064** | Marcus respondido via microfone (TC-042) | 1. Observar Marcus após resposta bem-sucedida | Marcus retorna ao fundo do ônibus; período de "trégua" antes de novo ataque | P1 |

#### 2.5.2 Emma Lynch — "A Burladora" (CRIANÇA 2)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-065** | Dia 1 noite — primeira aparição de Emma | 1. Aguardar trigger de Emma | Primeiro evento de QTE com janela de 2 segundos; feedback audiovisual claro antes do SHIFT | P0 |
| **TC-066** | Emma tenta abrir porta com SHIFT disponível | 1. Pressionar **SHIFT** no momento correto | Emma bloqueada; porta não abre; jogo continua; indicador de sucesso visual | P0 |
| **TC-067** | Emma tenta apertar botões do painel | 1. Observar o que acontece se falhar | Game Over imediato; animação de Emma apertando o botão + som + tela game over | P0 |
| **TC-068** | Emma — verificar timing de trigger | 1. Cronometrar intervalo entre ataques de Emma em diferentes dias | Intervalo deve respeitar cooldown da TRAVA (15s) — Emma não deve atacar 2x no cooldown de 15s no mesmo dia | P1 |

#### 2.5.3 Thomas Sanders — "O Narrador" (CRIANÇA 3)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-069** | Thomas presente, período 0–4s de ataque | 1. Ouvir o áudio ativamente | Sussurros baixos e ocasionais; tensão sonora inicial; ainda gerenciável | P1 |
| **TC-070** | Thomas presente, período 4–8s de ataque | 1. Ouvir o áudio ativamente | Sussurros intensificam visivelmente; urgência comunicada ao jogador | P1 |
| **TC-071** | Thomas presente, período > 8s sem resposta | 1. Não usar R por 8+ segundos | Sussurros em nível máximo; +15% de tensão aplicado; possível jump scare sonoro | P0 |
| **TC-072** | Thomas respondido com rádio dentro de 0–4s | 1. Usar **R** na fase inicial | Thomas suprimido; sem penalidade de tensão | P1 |

#### 2.5.4 Grace Harper — "A Observadora"

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-073** | Grace ativa no ônibus | 1. Tentar usar retrovisor **E** | Retrovisor bloqueado ou distorcido (feed corrompido/estática); Grace impede observação | P0 |
| **TC-074** | Grace ativa no ônibus | 1. Tentar usar câmera de segurança **C** | Câmera bloqueada ou corrompida; sem claridade mesmo com cooldown zerado | P0 |
| **TC-075** | Grace ativa, ambas ferramentas de observação bloqueadas | 1. Verificar se existe contermeasure para Grace | Verificar se documentação define contramedida para Grace; se não, confirmar que é comportamento intencional (design tension) | P1 |
| **TC-076** | Grace inativa (não está "observando" no momento) | 1. Usar retrovisor **E** normalmente | Retrovisor funciona normalmente; Grace não bloqueia quando inativa | P1 |

#### 2.5.5 Oliver Crane — "O Artista"

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-077** | Oliver ativo no ônibus | 1. Observar janelas/superfícies do ônibus | Desenhos aparecem gradualmente nas superfícies do ônibus (janelas, bancos) | P1 |
| **TC-078** | Oliver ativo — efeito nas crianças vivas | 1. Verificar estado das crianças vivas no ônibus | Crianças vivas reagem com medo/agitação aos desenhos de Oliver; possível aumento de tensão | P1 |
| **TC-079** | Oliver ativo — verificar trigger de susto | 1. Olhar diretamente para desenho de Oliver | Jumpscare ou evento de susto ao interagir com obra de Oliver | P2 |

---

### 2.6 Sistema de Tensão

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-080** | Início do Dia 1 Manhã | 1. Iniciar período e verificar tensão | Tensão inicial = **5%** conforme tabela de balanceamento | P0 |
| **TC-081** | Início do Dia 1 Noite | 1. Iniciar período e verificar tensão | Tensão inicial = **10%** conforme tabela de balanceamento | P0 |
| **TC-082** | Início do Dia 3 Manhã | 1. Iniciar período e verificar tensão | Tensão inicial = **15%** conforme tabela de balanceamento | P0 |
| **TC-083** | Início do Dia 5 Noite (máxima dificuldade) | 1. Iniciar período e verificar tensão | Tensão inicial = **35%** conforme tabela de balanceamento | P0 |
| **TC-084** | Final de período anterior (tensão final = 60%) | 1. Iniciar próximo período | Tensão carregada = 30% da tensão final anterior (60% × 30% = 18%) + tensão inicial do período | P0 |
| **TC-085** | Tensão em 50%, falha em responder CRIANÇA 1 | 1. Deixar Marcus vencer | Tensão sobe para **60%** (+10%); feedback visual atualizado | P0 |
| **TC-086** | Tensão em 80%, falha em responder CRIANÇA 3 | 1. Deixar Thomas vencer (>8s) | Tensão sobe para **95%** (+15%); efeitos visuais de alta tensão ativados | P0 |
| **TC-087** | Tensão em 99% | 1. Provocar qualquer falha adicional | Tensão atinge 100%; **Game Over** — tela vermelha pulsante + som estridente | P0 |
| **TC-088** | Tensão em 85%, completar rota com 2 minutos sobrando | 1. Finalizar período antes do limite | Tensão reduzida em **-5%** (bônus de performance); UI confirma "bônus de tempo" | P1 |
| **TC-089** | Atraso de 3 minutos na rota | 1. Demorar 3 minutos além do esperado | Tensão aumenta **+6%** (+2% por minuto × 3) | P1 |
| **TC-090** | Tensão entre 26–50% | 1. Observar efeitos visuais | Leve distorção nas bordas da tela; batimentos cardíacos baixos no áudio | P1 |
| **TC-091** | Tensão entre 51–75% | 1. Observar efeitos visuais | Visão periférica escurecida; sussurros + batimentos altos no áudio | P1 |
| **TC-092** | Tensão entre 76–99% | 1. Observar efeitos visuais | Visão túnel ativa; cores dessaturadas; gritos abafados + estática no áudio | P1 |
| **TC-093** | Uso incorreto de ferramenta (ex: R quando não é Thomas) | 1. Pressionar R sem Thomas ativo | +5 segundos perdidos na rota (penalidade de tempo); tensão não aumenta diretamente | P2 |

---

### 2.7 Progressão de Dias e Períodos

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-094** | Dia 1 Manhã completo (5 pontos de embarque OK) | 1. Finalizar rota de manhã | Transição para Dia 1 Noite; tela de progresso exibida; narrativa de Dia 1 desbloqueada | P0 |
| **TC-095** | Dia 1 Noite completo | 1. Finalizar rota de noite | Transição para Dia 2; "Nova informação narrativa sobre acidente" desbloqueada | P0 |
| **TC-096** | Dia 2 completo | 1. Verificar conteúdo desbloqueado | "Nova informação sobre acidente" exibida ou acessível | P1 |
| **TC-097** | Dia 3 completo | 1. Verificar conteúdo desbloqueado | "Revelação sobre identidade das crianças" exibida ou acessível | P1 |
| **TC-098** | Dia 4 completo | 1. Verificar conteúdo desbloqueado | "Informações sobre motorista anterior (Victor Graves)" exibidas | P1 |
| **TC-099** | Dia 5 Manhã — curva de dificuldade máxima | 1. Verificar comportamento de ataques | Ataques simultâneos possíveis (ex: Marcus + Thomas ao mesmo tempo); frequência 3-4 por período | P0 |
| **TC-100** | Falha em completar rota dentro do tempo (MANHÃ) | 1. Deixar temporizador expirar | Condição de derrota por tempo; tela de game over com motivo "rota não completada" | P0 |
| **TC-101** | Início do Dia 2 | 1. Verificar frequência de ataques | 2–3 ataques por período (Dia 1 = 1-2, Dia 2 = 2-3) | P1 |
| **TC-102** | Dias 4-5 ativos | 1. Verificar se cooldowns são críticos | Situações onde todos os cooldowns estão ativos simultaneamente são possíveis; decisões estratégicas forçadas | P1 |

---

### 2.8 Sistema de Save e Load

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-103** | Dia 2 completo | 1. Fechar o jogo completamente | Progresso salvo; ao reabrir, opção de continuar do Dia 3 disponível | P0 |
| **TC-104** | Save no final do Dia 3 | 1. Carregar save | Dia 4 inicia com tensão inicial correta (25% manhã / 30% noite); histórico narrativo preservado | P0 |
| **TC-105** | Save existente no Dia 3 | 1. Iniciar novo jogo | Opção "Novo Jogo" disponível; save anterior não é sobrescrito automaticamente (confirmação) | P1 |
| **TC-106** | Arquivo de save corrompido/deletado | 1. Abrir jogo sem save válido | Jogo inicia do Dia 1; sem crash; mensagem de "nenhum save encontrado" | P1 |
| **TC-107** | Save no meio de um período (se suportado) | 1. Verificar se save mid-period está implementado | Se suportado: período retoma do início do mesmo período. Se não suportado: documentar comportamento | P2 |

---

### 2.9 Finais do Jogo

**Contexto:** 3 finais possíveis — Fuga, Redenção e Ciclo (desbloqueados após Dia 5)

| ID | Precondição | Passos | Resultado Esperado | Prioridade |
|----|-------------|--------|--------------------|------------|
| **TC-108** | Dia 5 completo — caminho de Fuga | 1. Completar Dia 5 pela rota de Fuga (condições a confirmar com design) | Final "Fuga" exibido com cinemática/narrativa específica; créditos únicos | P0 |
| **TC-109** | Dia 5 completo — caminho de Redenção | 1. Completar Dia 5 pela rota de Redenção (condições a confirmar) | Final "Redenção" exibido com cinemática/narrativa específica; créditos únicos | P0 |
| **TC-110** | Dia 5 completo — caminho do Ciclo | 1. Completar Dia 5 pela rota do Ciclo (condições a confirmar) | Final "Ciclo" exibido com cinemática/narrativa específica; créditos únicos | P0 |
| **TC-111** | Todos os 3 finais disponíveis | 1. Verificar se os 3 caminhos são mutuamente exclusivos por save | Cada save só pode obter um final por playthrough; novo jogo necessário para outros finais | P1 |
| **TC-112** | Final obtido | 1. Verificar opção de novo jogo após créditos | Opção de reiniciar do Dia 1 oferecida; save do final preservado | P1 |
| **TC-113** | Verificar acessibilidade dos finais | 1. Confirmar que as condições de cada final são atingíveis por design | Nenhum final deve ser matematicamente impossível dado o balanceamento de tensão | P0 |

---

## 3. Matriz de Compatibilidade

### 3.1 Sistemas Operacionais

| SO | Versão | Status Alvo | Notas |
|----|--------|-------------|-------|
| Windows 10 | 21H2 (64-bit) | ✅ Suportado | Plataforma primária |
| Windows 10 | 22H2 (64-bit) | ✅ Suportado | Versão mais recente W10 |
| Windows 11 | 22H2 (64-bit) | ✅ Suportado | Plataforma recomendada |
| Windows 11 | 23H2 (64-bit) | ✅ Suportado | — |
| Windows 10 (32-bit) | Qualquer | ❌ Não suportado | Unity 6 exige 64-bit |
| macOS | Qualquer | ❌ Fora do escopo | Potencial v2.0 |
| Linux | Qualquer | ❌ Fora do escopo | Potencial v2.0 |

**Procedimento de teste:** Executar smoke test completo (Seção 4.1) em cada SO suportado antes de cada release candidate.

---

### 3.2 Hardware Mínimo vs. Recomendado

| Componente | Mínimo | Recomendado | Ideal |
|------------|--------|-------------|-------|
| **CPU** | Intel i5-8400 / AMD Ryzen 5 2600 | Intel i7-10700K / Ryzen 7 3700X | i7-12700K / Ryzen 9 5900X |
| **GPU** | NVIDIA GTX 1060 6GB / AMD RX 580 | NVIDIA RTX 2060 / RX 5700 XT | RTX 3070 / RX 6800 XT |
| **RAM** | 8 GB | 16 GB | 32 GB |
| **VRAM** | 4 GB | 8 GB | 12 GB |
| **Armazenamento** | 10 GB HDD | 10 GB SSD | 10 GB NVMe SSD |
| **DirectX** | DirectX 11 | DirectX 12 | DirectX 12 |
| **SO** | Windows 10 64-bit | Windows 10/11 64-bit | Windows 11 64-bit |
| **Target FPS** | 30 FPS (mínimo funcional) | 60 FPS estável | 60+ FPS |

**Critério de aceite performance:**
- Hardware recomendado: **60 FPS estável**, sem drops abaixo de 55 FPS
- Hardware mínimo: **30 FPS funcional**, drops tolerados em cenas de alta densidade
- RAM em uso: **< 2 GB** em hardware recomendado (TECH-005)

---

### 3.3 Matriz de Resolução

| Resolução | Proporção | Status | Notas de QA |
|-----------|-----------|--------|-------------|
| 1280×720 (720p) | 16:9 | ✅ Suportado | HUD deve permanecer legível; UI não pode overflow |
| 1920×1080 (1080p) | 16:9 | ✅ Suportado — **Principal** | Resolução de referência para todos os testes |
| 2560×1440 (1440p) | 16:9 | ✅ Suportado | Verificar renderização de assets; sem artefatos de upscale |
| 3840×2160 (4K) | 16:9 | ⚠️ Não testado v1.0 | Fora do escopo inicial |
| 2560×1080 (ultrawide) | 21:9 | ⚠️ Não suportado v1.0 | Verificar se o jogo inicia sem crash |
| 1366×768 | 16:9 | ✅ Suportado | Comum em laptops; verificar HUD de cooldowns |

**Itens críticos por resolução:**
- Indicadores de cooldown (Q/SHIFT/R) devem ser legíveis em 720p
- Mapa (TAB) deve ser usável em 720p
- UI de tensão (%) deve ter contraste adequado em todas resoluções
- Retrovisor e câmera de segurança devem exibir imagem coerente em 1440p

---

## 4. Procedimentos de QA

### 4.1 Checklist de Smoke Test por Build

Executar a cada nova build antes de testes completos. Tempo estimado: **15–20 minutos**.

```
SMOKE TEST — BUS SHIFT
Build: ___________  Data: ___________  Testador: ___________

INICIALIZAÇÃO
[ ] Jogo abre sem crash
[ ] Menu principal carrega corretamente
[ ] Novo jogo inicia sem erro
[ ] Dia 1 Manhã carrega

CONTROLES BÁSICOS
[ ] W acelera ônibus
[ ] S freia e faz ré
[ ] A/D direcionam corretamente
[ ] T abre porta próximo ao ponto
[ ] TAB abre/fecha mapa

FERRAMENTAS
[ ] E ativa retrovisor
[ ] C ativa câmera (cooldown 15s)
[ ] F ativa farol (cooldown 20s)
[ ] SPACE (5s) exibe tensão
[ ] Q (3s) ativa microfone (cooldown 20s)
[ ] SHIFT ativa trava (cooldown 15s)
[ ] R ativa rádio (cooldown 25s)

FANTASMAS (Dia 1)
[ ] Marcus aparece e troca de lugar
[ ] Marcus respondido pelo microfone funciona
[ ] Emma aparece com QTE de 2s
[ ] SHIFT bloqueia Emma com sucesso
[ ] Falha em Emma = Game Over

SISTEMA DE TENSÃO
[ ] Tensão inicial 5% (Dia 1 Manhã)
[ ] Tensão aumenta após falha com Marcus (+10%)
[ ] Game Over a 100%
[ ] Efeitos visuais de tensão aparecem (≥50%)

SAVE/LOAD
[ ] Progresso salvo ao fim do período
[ ] Load recupera estado correto

RESULTADO: [ ] PASSOU  [ ] FALHOU
Notas: _________________________________
```

---

### 4.2 Procedimento de Report de Bugs

Todos os bugs devem ser reportados em `docs/game/testing/known-issues.md` seguindo o template:

```markdown
### [ID]: [Título breve]

**Severidade:** [P0/P1/P2/P3]
**Status:** [Open/Investigating/In Progress/Fixed/Won't Fix]
**Data:** YYYY-MM-DD
**Build:** [Versão da build]
**Testador:** [Nome]

**Descrição:**
[Descrição clara e objetiva do problema]

**Passos para Reproduzir:**
1. [Passo 1 — estado inicial]
2. [Passo 2 — ação executada]
3. [Passo 3 — observar resultado incorreto]

**Resultado Esperado:**
[O que deveria acontecer segundo design/spec]

**Resultado Atual:**
[O que está acontecendo]

**Evidência:**
- Screenshot/vídeo: [link ou path]
- Logs Unity: [se aplicável]

**Ambiente:**
- SO: Windows [10/11] [versão]
- GPU: [modelo]
- RAM: [GB]
- Resolução: [resolução]
- Build: [versão]

**Frequência:** [Sempre / Intermitente (X/10) / Raramente]
**Workaround:** [Se existe]
```

---

### 4.3 Priorização de Bugs

| Nível | Título | Critério | SLA de Resolução | Exemplos |
|-------|--------|----------|-----------------|---------|
| **P0** | Crítico | Crash, Game Over indevido, impossibilidade de progredir, perda de save | **Bloqueia release** — corrigir imediatamente | Crash ao Dia 3, Emma mata sem trigger correto, save corrompido |
| **P1** | Alto | Mecânica principal quebrada, cooldown errado, tensão com valor incorreto | Corrigir antes da release | Cooldown de SHIFT errado, tensão inicial errada no Dia 4 |
| **P2** | Médio | Funcionalidade secundária com comportamento inesperado, visual errado | Corrigir na sprint seguinte | Farol não desliga após 15s, mapa com ícone errado |
| **P3** | Baixo | Cosmético, texto errado, UI desalinhada | Backlog — corrigir se houver tempo | Fonte errada no menu, som levemente atrasado |

**Regra de ouro:**
- Zero P0 abertos → Release permitida
- Zero P1 abertos (ou todos mitigados com workaround documentado) → Release permitida
- P2/P3 podem ser carregados para pós-launch como updates

---

## 5. Ferramentas de Teste

### 5.1 Unity Test Framework (Testes Unitários e de Integração)

| Tipo de Teste | Ferramenta | Escopo | Quando Rodar |
|--------------|------------|--------|--------------|
| Testes Unitários | Unity Test Runner (EditMode) | Lógica de tensão, cooldown timers, spawn de fantasmas | A cada commit |
| Testes de Integração | Unity Test Runner (PlayMode) | Fluxo completo de período, save/load, transição de dias | A cada build |
| Testes de Performance | Unity Profiler | FPS, memória RAM, draw calls | A cada release candidate |
| Testes de Regressão | Unity Test Runner (full suite) | Todas as mecânicas | Antes de cada RC |

**Suites de teste recomendadas (Unity EditMode):**

```csharp
// Exemplo de estrutura de testes unitários
[TestFixture]
public class TensionSystemTests
{
    [Test] public void TensionInitial_Day1Morning_Is5Percent() { }
    [Test] public void TensionInitial_Day5Night_Is35Percent() { }
    [Test] public void FailChild1_AddsExactly10Percent() { }
    [Test] public void FailChild3_AddsExactly15Percent() { }
    [Test] public void TensionAt100_TriggersGameOver() { }
    [Test] public void CompleteOnTime_Reduces5Percent() { }
    [Test] public void CarryoverTension_Is30PercentOfPrevious() { }
}

[TestFixture]
public class CooldownSystemTests
{
    [Test] public void PanelLock_Cooldown_Is15Seconds() { }
    [Test] public void Microphone_Cooldown_Is20Seconds() { }
    [Test] public void Radio_Cooldown_Is25Seconds() { }
    [Test] public void Headlight_Cooldown_Is20Seconds() { }
    [Test] public void Camera_Cooldown_Is15Seconds() { }
    [Test] public void AllCooldowns_StartAtZero_NewPeriod() { }
}
```

---

### 5.2 Playtesting Manual

| Sessão | Foco | Perfil do Testador | Frequência |
|--------|------|-------------------|------------|
| **Alpha Internal** | Todas as mecânicas, bugs P0 | Dev/QA (conhecimento do jogo) | Toda nova feature |
| **Alpha Playtest** | Curva de aprendizado Dia 1, legibilidade dos controles | Testadores externos sem experiência prévia | Build semanal |
| **Beta Playtest** | Curva de dificuldade Dias 3-5, balanceamento | Mix de gamers hardcore e casuais | Antes de RC |
| **Pre-Launch RC** | Smoke test completo, finais, save/load | QA dedicado + beta testers selecionados | RC final |

**Métricas a coletar no playtesting:**
- Taxa de conclusão por dia (meta: 90% D1, 60-70% D3, 30-40% D5)
- Número de Game Overs por Emma por sessão
- Tempo médio por período (meta: ~12 min)
- Tensão média ao final do período (meta: 70-80%)
- Pontos de frustração (onde o jogador para de jogar)
- Descoberta orgânica dos controles (sem tutorial explícito)

---

### 5.3 Telemetria In-Game

**Eventos a rastrear (opt-in do jogador):**

| Evento | Dados Coletados | Finalidade |
|--------|----------------|------------|
| `game_over` | Causa (CRIANÇA 1/2/3, tensão, tempo), dia, período, tensão no momento | Identificar pontos de frustração |
| `tool_used` | Ferramenta, dia, período, tensão, resultado (sucesso/falha) | Validar uso de ferramentas |
| `ghost_attack` | Fantasma, dia, período, resultado (respondido/ignorado), tempo de resposta | Calibrar dificuldade |
| `period_complete` | Dia, período, tempo real gasto, tensão final | Métricas de balanceamento |
| `final_reached` | Final obtido (Fuga/Redenção/Ciclo), total de horas jogadas | Acessibilidade dos finais |
| `session_abandoned` | Último checkpoint atingido, tempo de sessão | Retenção |

**Implementação sugerida:** Unity Analytics (ou solução local para privacidade) com eventos assíncronos não-bloqueantes.

---

## 6. Critérios de Aceite por Sistema

| Sistema | Critério de Aceite | Ferramenta de Verificação | Status |
|---------|-------------------|--------------------------|--------|
| **Controles de Direção** | W/A/S/D responde em < 16ms (1 frame a 60fps); física de veículo pesado perceptível | Unity Profiler + Playtesting | 🔲 Pending |
| **Embarque/Desembarque** | T só ativa em raio válido de ponto; animação + som em 100% dos casos | Unity Test Runner (PlayMode) | 🔲 Pending |
| **Retrovisor (E)** | Foco instantâneo; bloqueio por Grace 100% dos casos quando ativa | Playtesting Manual | 🔲 Pending |
| **Câmera de Segurança (C)** | 5s de claridade; cooldown 15s preciso (±0.1s) | Unity Test Runner (EditMode) | 🔲 Pending |
| **Farol (F)** | 15s de iluminação; cooldown 20s preciso (±0.1s); sem efeito no período diurno | Unity Test Runner (EditMode) | 🔲 Pending |
| **Relógio (SPACE)** | UI exibe hora e tensão corretos; 5s hold required; cooldown 20s | Unity Test Runner (EditMode) | 🔲 Pending |
| **Mapa (TAB)** | Toggle instantâneo; rota e posição corretas; controles de direção preservados | Playtesting Manual | 🔲 Pending |
| **Microfone — Q (Marcus)** | 3s hold; Marcus suprimido 100% das vezes; cooldown 20s; +10% tensão se falhar | Unity Test Runner + Playtesting | 🔲 Pending |
| **Trava — SHIFT (Emma)** | Janela de 2s precisa; Game Over 100% se falhar; cooldown 15s; Emma não ataca 2x no cooldown | Unity Test Runner + Playtesting | 🔲 Pending |
| **Rádio — R (Thomas)** | Thomas suprimido; cooldown 25s; +15% tensão se falhar | Unity Test Runner + Playtesting | 🔲 Pending |
| **Marcus Reid** | Troca de lugar detectável; avanço para cabine se ignorado; retrocede após microfone | Playtesting Manual | 🔲 Pending |
| **Emma Lynch** | QTE de 2s preciso; Game Over imediato na falha; não ataca durante cooldown SHIFT | Playtesting Manual + Profiler | 🔲 Pending |
| **Thomas Sanders** | Escalada em 3 fases (0-4s, 4-8s, 8s+) audível e distinguível | Playtesting Manual | 🔲 Pending |
| **Grace Harper** | Retrovisor E e câmera C bloqueados quando Grace ativa | Playtesting Manual | 🔲 Pending |
| **Oliver Crane** | Desenhos aparecem progressivamente; crianças vivas reagem | Playtesting Manual | 🔲 Pending |
| **Sistema de Tensão — Valores Iniciais** | Valores exatos por dia/período conforme tabela (5%, 10%, 15%, 20%, 25%, 30%, 35%) | Unity Test Runner (EditMode) | 🔲 Pending |
| **Sistema de Tensão — Modificadores** | +10% CRIANÇA 1, +15% CRIANÇA 3, +2%/min atraso, -5% bônus tempo, 30% carryover | Unity Test Runner (EditMode) | 🔲 Pending |
| **Sistema de Tensão — Visual Feedback** | 4 faixas de efeitos (0-25, 26-50, 51-75, 76-99%) sem transição abrupta | Playtesting Manual | 🔲 Pending |
| **Sistema de Tensão — Game Over** | 100% de tensão = Game Over imediato sem falsos positivos | Unity Test Runner (EditMode) | 🔲 Pending |
| **Progressão de Dias** | 10 períodos (5 dias × 2) transitam corretamente; conteúdo narrativo desbloqueado | Unity Test Runner (PlayMode) | 🔲 Pending |
| **Frequência de Ataques** | Dia 1: 1-2, Dia 2: 2-3, Dia 3: combinados, Dia 4: 3-4, Dia 5: simultâneos | Telemetria + Playtesting | 🔲 Pending |
| **Sistema de Save/Load** | Progresso salvo ao fim de cada período; load restaura estado completo; sem corrupção | Unity Test Runner (PlayMode) | 🔲 Pending |
| **3 Finais** | Todos 3 acessíveis por caminhos válidos; cinemáticas únicas; não intercambiáveis | Playtesting Manual (3 runs completas) | 🔲 Pending |
| **Performance — 60 FPS** | ≥ 60 FPS em hardware recomendado; ≥ 30 FPS em hardware mínimo | Unity Profiler | 🔲 Pending |
| **Performance — RAM** | < 2 GB em hardware recomendado | Unity Profiler / Task Manager | 🔲 Pending |
| **Compatibilidade — Windows 10** | Smoke test completo aprovado no Win 10 21H2 e 22H2 | Smoke Test Checklist | 🔲 Pending |
| **Compatibilidade — Windows 11** | Smoke test completo aprovado no Win 11 22H2 e 23H2 | Smoke Test Checklist | 🔲 Pending |
| **Resolução 720p** | HUD legível; sem overflow; jogo jogável | Playtesting Manual | 🔲 Pending |
| **Resolução 1080p** | Referência; sem artefatos visuais | Playtesting Manual | 🔲 Pending |
| **Resolução 1440p** | Assets renderizam sem artefatos de upscale | Playtesting Manual | 🔲 Pending |
| **Métricas de Balanceamento** | Taxas de conclusão dentro das metas (90%/60-70%/30-40%); tempo ~12min | Telemetria + Playtesting | 🔲 Pending |

---

## Apêndices

### A. Mapeamento de Test Cases por Fantasma

| Fantasma | Test Cases Cobertos |
|----------|---------------------|
| Marcus Reid (CRIANÇA 1) | TC-042 a TC-047, TC-061 a TC-064 |
| Emma Lynch (CRIANÇA 2) | TC-048 a TC-052, TC-065 a TC-068 |
| Thomas Sanders (CRIANÇA 3) | TC-053 a TC-057, TC-069 a TC-072 |
| Grace Harper | TC-073 a TC-076 |
| Oliver Crane | TC-077 a TC-079 |

### B. Mapeamento de Test Cases por Sistema

| Sistema | Test Cases Cobertos |
|---------|---------------------|
| Controles de Direção | TC-001 a TC-012 |
| Embarque/Desembarque | TC-013 a TC-020 |
| Retrovisor (E) | TC-021 a TC-024 |
| Câmera (C) | TC-025 a TC-029 |
| Farol (F) | TC-030 a TC-033 |
| Relógio (SPACE) | TC-034 a TC-037 |
| Mapa (TAB) | TC-038 a TC-041 |
| Microfone (Q) | TC-042 a TC-047 |
| Trava (SHIFT) | TC-048 a TC-052 |
| Rádio (R) | TC-053 a TC-057 |
| Validação Cruzada Cooldowns | TC-058 a TC-060 |
| Sistema de Tensão | TC-080 a TC-093 |
| Progressão de Dias | TC-094 a TC-102 |
| Save/Load | TC-103 a TC-107 |
| Finais do Jogo | TC-108 a TC-113 |

### C. Contagem de Test Cases

| Prioridade | Quantidade |
|------------|-----------|
| P0 | 38 |
| P1 | 55 |
| P2 | 12 |
| P3 | 0 |
| **Total** | **113** |

### D. Dependências Pendentes de Design

Os seguintes itens em `known-issues.md` podem impactar test cases:

| ID | Impacto nos Testes |
|----|-------------------|
| DESIGN-001 | Controles — esquema completo pode adicionar novos TCs |
| DESIGN-002 | Core loop — pode redefinir condições de vitória |
| DESIGN-003 | Mecânica de "loucura" — pode criar sistema paralelo ao de tensão |

> ⚠️ **Nota:** TC-108 a TC-110 (finais) dependem da definição completa das condições de trigger de cada final. Quando DESIGN-002 e o sistema de finais forem detalhados, estes test cases devem ser revisados e expandidos.

---

*— Quinn, guardião da qualidade 🛡️*  
*Test Plan v1.0.0 — Bus Shift | Unity 6000.3.2f1 | Windows (Steam)*

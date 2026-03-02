# Bus Shift - Mecânicas do Jogo

## Mecânicas Principais

### 1. Direção do Ônibus
**Descrição:** Controle completo do veículo escolar em ambiente 3D.

**Controles:**
- **W** - Acelerar
- **A** - Virar à esquerda
- **S** - Frear / Ré (quando parado)
- **D** - Virar à direita

**Regras:**
- S diminui velocidade quando em movimento
- S faz ré quando veículo está parado
- Física realista de veículo pesado

**Feedback:** Som do motor, resistência nas curvas, modelo 3D responde aos controles

---

### 2. Ponto de Ônibus
**Descrição:** Interação com pontos de embarque/desembarque.

**Controles:**
- **T** - Abrir porta (próximo ao ponto)

**Regras:**
- Só funciona quando próximo a um ponto válido
- Apertar longe do ponto não tem efeito
- Crianças embarcam/desembarcam automaticamente

**Feedback:** Som da porta, animação de abertura, crianças entram/saem

---

## Objetos de Auxílio

### Mapa
**Controles:** TAB (toggle on/off)
**Função:** Visualizar rota e localização
**Trade-off:** Bloqueia parte da visão da rua
**Cooldown:** Nenhum

---

### Retrovisor
**Controles:** E (segurar para focar)
**Função:** Observar comportamento das crianças no fundo do ônibus
**Trade-off:** Tira atenção da estrada
**Cooldown:** Nenhum

---

### Câmera
**Controles:** C (tapinha no monitor)
**Função:** Reduz chiado da câmera por 5 segundos
**Cooldown:** 15 segundos
**Feedback Visual:** Imagem fica mais nítida temporariamente

---

### Farol
**Controles:** F
**Função:** Ilumina a estrada por 15 segundos (período noturno)
**Cooldown:** 20 segundos
**Feedback:** Ambiente fica mais visível, som de clique

---

### Relógio
**Controles:** SPACE (segurar por 5s)
**Função:** Mostra horário atual e nível de tensão
**Cooldown:** 20 segundos
**Feedback:** UI overlay com informações

---

## Objetos de Intervenção (Anti-Assombração)

### Microfone
**Controles:** Q (segurar por 3 segundos)
**Alvo:** CRIANÇA 1 (troca de lugares)
**Efeito:** Ordena que crianças parem de trocar de lugar
**Cooldown:** 20 segundos
**Feedback:** Som de microfone + voz do motorista

---

### Trava do Painel
**Controles:** SHIFT
**Alvo:** CRIANÇA 2 (manipulação de botões)
**Efeito:** Bloqueia painel por 3 segundos
**Cooldown:** 15 segundos
**Feedback:** Som de trava + indicador visual no painel

---

### Rádio
**Controles:** R
**Alvo:** CRIANÇA 3 (ruído sonoro)
**Efeito:** Toca música, reduz ruído paranormal
**Cooldown:** 25 segundos
**Feedback:** Música ambiente, estática diminui

---

## Tabela de Ameaças vs Contramedidas

| Assombração | Comportamento | Contramedida | Tempo Resposta | Penalidade (falha) |
|-------------|---------------|--------------|----------------|-------------------|
| **CRIANÇA 1** | Troca lugares constantemente | Microfone (Q - 3s) | 5-8 segundos | +10% tensão |
| **CRIANÇA 2** | Tenta abrir porta/mexer painel | Trava (SHIFT) | 2-3 segundos | Acidente / Game Over |
| **CRIANÇA 3** | Emite ruído perturbador | Rádio (R) | 8-10 segundos | +15% tensão |

---

## Mecânicas de Progressão

### Sistema de Tensão
- Tensão inicial aumenta por dia/período
- Erros do jogador aumentam tensão
- Demora em responder ataques = +tensão
- Finalizar com tempo sobrando = -tensão reduzida
- Parte da tensão é mantida entre períodos

### Condições de Vitória
- Completar rota dentro do tempo limite
- Manter tensão abaixo de 100%
- Levar crianças aos destinos corretos

### Condições de Derrota
- Tensão atinge 100%
- Acidente grave (CRIANÇA 2 abre porta)
- Falha em completar rota no tempo

---

## Sistema de Cooldowns

| Ferramenta | Cooldown | Duração Efeito | Prioridade Uso |
|------------|----------|----------------|----------------|
| Trava Painel | 15s | 3s | 🔴 CRÍTICA |
| Câmera | 15s | 5s | 🟡 MÉDIA |
| Microfone | 20s | Instantâneo | 🟡 MÉDIA |
| Relógio | 20s | 5s | 🟢 BAIXA |
| Farol | 20s | 15s | 🟢 BAIXA |
| Rádio | 25s | Instantâneo | 🟠 ALTA |

## 🧪 Testing

### Casos de Teste
1. [Caso de teste 1]
2. [Caso de teste 2]

---

*Mechanics Documentation - Bus Shift*

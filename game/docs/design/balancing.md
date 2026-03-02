# Bus Shift - Balanceamento e Progressão

## Sistema de Tensão/Insanidade

### Tabela de Tensão Inicial por Dia

| Período | DIA 1 | DIA 2 | DIA 3 | DIA 4 | DIA 5 |
|---------|-------|-------|-------|-------|-------|
| **MANHÃ** | 5% | 10% | 15% | 25% | 30% |
| **NOITE** | 10% | 15% | 20% | 30% | 35% |

### Modificadores de Tensão

| Evento | Impacto na Tensão | Notas |
|--------|-------------------|-------|
| **Erro em responder CRIANÇA 1** | +10% | Permite movimento paranormal |
| **Erro em responder CRIANÇA 2** | GAME OVER | Crítico - porta abre |
| **Erro em responder CRIANÇA 3** | +15% | Ruído prolongado |
| **Atraso na rota** | +2% por minuto | Acumula gradualmente |
| **Finalizar com tempo sobrando** | -5% | Bônus performance |
| **Parte da tensão passada** | 30% da tensão final | Entre períodos |

### Curva de Dificuldade

```
Dia 1: Tutorial implícito, 1-2 ataques por período
Dia 2: Frequência aumenta, 2-3 ataques
Dia 3: Ataques combinados começam
Dia 4: Alta frequência, 3-4 ataques, cooldowns críticos
Dia 5: Máxima dificuldade, ataques simultâneos possíveis
```

---

## Progressão de Recompensas

### Desbloqueio de Conteúdo

| Dia Completo | Recompensa |
|--------------|------------|
| Dia 1 | Continuação da história |
| Dia 2 | Nova informação narrativa sobre acidente |
| Dia 3 | Revelação sobre identidade das crianças |
| Dia 4 | Informações sobre motorista anterior |
| Dia 5 | Acesso aos 3 finais possíveis |

---

## Balanceamento de Tempo

### Tempo Limite por Período

| Período | Tempo Real | Tempo In-Game | Pontos de Parada |
|---------|------------|---------------|------------------|
| MANHÃ | 10-12 min | 06:00-07:00 | 5 pontos (embarque) |
| NOITE | 12-15 min | 18:00-19:30 | 5 pontos (desembarque) |

### Penalidades de Tempo

- Cada ataque não respondido: +15 segundos perdidos
- Uso incorreto de ferramenta: +5 segundos
- Crash ou acidente menor: +30 segundos

---

## Economia de Cooldowns

### Análise de Trade-offs

**Cenário Crítico:** CRIANÇA 2 ataca (2-3s para responder)
- **Trava Painel** deve estar sempre disponível
- Cooldown mais curto (15s) reflete prioridade
- Falha = Game Over imediato

**Cenário Moderado:** CRIANÇA 1 e 3 atacam juntas
- Jogador deve priorizar: Microfone (20s) ou Rádio (25s)?
- Decisão estratégica: +10% vs +15% tensão
- Gerenciamento de recursos crítico

**Ferramentas de Informação:** Relógio e Farol
- Cooldowns mais longos (20s)
- Uso opcional, não obrigatório
- Facilitam gameplay mas não são essenciais

---

## Curva de Aprendizado

### Fase 1 - Tutorial Implícito (Dia 1 Manhã)
- Apenas familiarização com controles
- 1 ataque simples (CRIANÇA 1)
- Explicação visual dos controles

### Fase 2 - Introdução ao Perigo (Dia 1 Noite)
- Primeiro jumpscare (menino pálido)
- Introdução da CRIANÇA 2 (evento QTE)
- Jogador aprende consequências

### Fase 3 - Mecânicas Completas (Dias 2-3)
- Todas as 3 crianças ativas
- Ataques individuais
- Foco em gerenciamento de cooldowns

### Fase 4 - Maestria (Dias 4-5)
- Ataques simultâneos
- Decisões sob pressão
- Máxima tensão inicial

---

## Balanceamento Visual/Sonoro

### Indicadores de Tensão

| Nível Tensão | Efeitos Visuais | Efeitos Sonoros |
|--------------|-----------------|-----------------|
| 0-25% | Normal | Fundo ambiente tranquilo |
| 26-50% | Leve distorção nas bordas | Batimentos cardíacos baixos |
| 51-75% | Visão periférica escurecida | Sussurros + batimentos altos |
| 76-99% | Visão túnel, cores dessaturadas | Gritos abafados + estática |
| 100% | Tela vermelha pulsante | Som estridente |

---

## Teste de Balanceamento

### Métricas-Alvo

- **Taxa de Conclusão Dia 1:** 90%+ (tutorial)
- **Taxa de Conclusão Dia 3:** 60-70% (curva média)
- **Taxa de Conclusão Dia 5:** 30-40% (desafiador)
- **Tempo médio por período:** 12 minutos
- **Tensão média ao final:** 70-80% (jogador competente)

### Ajustes Possíveis

- Se taxa Dia 5 < 20%: reduzir tensão inicial noite
- Se taxa Dia 1 < 80%: aumentar tempo de resposta CRIANÇA 2
- Se tempo médio > 15 min: reduzir pontos de parada ou distâncias

# Architect-First Skill

**Ativação:** `@skill:architect-first` ou no chat: "usando skill architect-first"

---

## Overview

Skill que implementa a filosofia **"Arquitetura perfeita, execução pragmática, qualidade garantida por testes"**.

Use quando: decisões arquiteturais, novos features, refatorações, ou quality gates precisam ser aplicados.

---

## Core Philosophy

### Mantra
```
Arquitetura perfeita → execução pragmática → qualidade garantida por testes
```

### Quality Gates

#### NON-NEGOTIABLE (STOP se violado):
- ✅ **Architecture**: Design completo e documentação ANTES de código
- ✅ **Documentation**: Deve preceder e acompanhar implementação
- ✅ **Capability Preservation**: Nunca perder capacidade vs versões anteriores
- ✅ **Zero Coupling**: Expansion packs independentes
- ✅ **Multi-Agent Validation**: Decisões estruturais validadas por múltiplas perspectivas

#### NEGOTIABLE (com escape hatch):
- ⚠️ **Code Style**: Aceitável se backed por testes
- ⚠️ **Feature Completeness**: 80% aceitável SE core use case funciona
- ⚠️ **Quick & Dirty Code**: Permitido ONLY com test plan e logging mínimo

---

## Decision Tree

```
Nova Feature/Task
    ↓
É decisão estrutural/arquitetural?
    │
    ├─ YES → [Architecture Flow]
    │         1. Map Before Modify
    │         2. Multi-Agent Validation
    │         3. Design Documentation
    │         4. Gold Standard Baseline
    │         5. Zero Coupling Validation
    │         6. → Implementação
    │
    └─ NO  → [Execution Flow]
              1. Pre-Implementation Checklist
              2. Test-Driven Safety Net
              3. Implementação
              4. Documentation
```

---

## Architecture Flow (Decisões Estruturais)

### 1. Map Before Modify
**STOP e documente completamente:**
```
- [ ] Estado atual documentado
- [ ] Todas dependências identificadas
- [ ] Touch points mapeados
- [ ] Diagramas arquiteturais criados
- [ ] Architecture checklist carregado
```

### 2. Multi-Agent Validation
**Apresente opções A/B/C com trade-offs:**

```markdown
## Opção A: [Nome]
**Pros:**
- Vantagem 1
- Vantagem 2

**Cons:**
- Desvantagem 1
- Desvantagem 2

**Complexity:** Baixa/Média/Alta
**Maintenance:** Fácil/Médio/Difícil
**Risk:** Baixo/Médio/Alto

## Opção B: [Nome]
...

## Opção C: [Nome]
...

## Recomendação
[Opção X] porque [justificativa baseada em trade-offs]
```

**Validar com:**
- Product Owner (alinhamento de negócio)
- Architect (soundness técnica)
- QA (testability)
- Dev (feasibility)

### 3. Design Documentation
**Antes de qualquer código:**

```markdown
# Architecture Design: [Feature]

## System Architecture
[Diagramas de componentes]

## Component Interactions
[Como componentes se comunicam]

## Data Flows
[Como dados fluem pelo sistema]

## Configuration Schema
```yaml
feature:
  enabled: true
  options:
    maxRetries: 3
    timeout: 5000
```

## Integration Points
[Como integra com sistemas existentes]

## Security Considerations
[Implications de segurança]

## Performance Implications
[Impacto esperado em performance]
```

### 4. Gold Standard Baseline
**Validação crítica:**
```
❓ Nova design mantém TODAS capacidades anteriores?
❓ Há perda de funcionalidade?
❓ Granularidade mantida ou aumentada?

⛔ SE capability loss detectada → STOP
   → Redesenhar OU justificar explicitamente a perda
```

### 5. Zero Coupling Validation
**Garantir independência de módulos:**
```bash
# Validar que expansion packs são independentes
# Sem dependências hardcoded entre módulos
# Configs externalizadas em YAML

# Teste: remover módulo não deve quebrar core
```

### 6. Proceder com Implementação
**Apenas agora:** escrever código seguindo Execution Flow

---

## Execution Flow (Implementação)

### 1. Pre-Implementation Checklist
```
- [ ] Arquitetura documentada e validada?
- [ ] Core use case claramente definido?
- [ ] Configuração externalizada (YAML)?
- [ ] Estratégia de testes definida?
- [ ] Logging points identificados?
```

### 2. Test-Driven Safety Net
**Defina test plan PRIMEIRO:**
```typescript
// ANTES de implementar
describe('Feature', () => {
  it('handles happy path', () => {})
  it('handles edge case X', () => {})
  it('handles error scenario Y', () => {})
  it('maintains capability Z', () => {})
})
```

**Testes permitem temporary imperfection (escape hatch)**

### 3. Implementation Style

#### ✅ ACCEPTABLE:
```
✓ "Ugly" code COM comprehensive tests
✓ 80% completeness SE core case funciona
✓ Quick implementation COM test plan + logging
```

#### ❌ REJECTED:
```
✗ "Ugly" code SEM tests
✗ Capability loss sem justificativa explícita
✗ Hardcoded mutable values (devem ser YAML)
✗ Deploy sem core case funcionando
```

### 4. Configuration Externalization

**Sempre YAML, nunca hardcoded:**
```yaml
# ✅ CORRETO - config.yaml
feature:
  enabled: true
  maxRetries: 3
  timeout: 5000
  endpoints:
    - https://api.example.com
    - https://api-backup.example.com

# Código lê do config
import config from '@/config/feature.yaml'
const retries = config.feature.maxRetries
```

```typescript
// ❌ INCORRETO - Hardcoded
const MAX_RETRIES = 3  // Imutável? Pode mudar no futuro?
const TIMEOUT = 5000
```

---

## Stop Rules (Hard Boundaries)

**STOP IMEDIATAMENTE se detectar:**

| Violação | Ação |
|----------|------|
| ⛔ Capability loss vs baseline | Redesenhar ou justificar explicitamente |
| ⛔ Decisão estrutural sem validação multi-agent | Apresentar opções A/B/C para validação |
| ⛔ Coupling entre módulos | Refatorar para independence |
| ⛔ Missing architectural documentation | Documentar antes de continuar |
| ⛔ Quick & dirty SEM test plan e logs | Adicionar testes + logging |
| ⛔ Hardcoded mutable configuration | Externalizar para YAML |

---

## Heuristics (Decision Rules)

1. **Never Lose Capability**: Acumular, nunca reduzir
2. **Architect Before Build**: Design/docs antes de código, sempre
3. **Zero Coupling, Max Modularity**: Independent expansion packs
4. **Config > Hardcoding**: Externalizar YAML para valores mutáveis
5. **Map Before Modify**: Documentar estrutura antes de mudar
6. **Binary Decision Post-Validation**: Fast execution após validação arquitetural
7. **Speed via Automation**: Não via shortcuts
8. **Quality Escape Hatch**: Testes permitem temporary imperfection

---

## Risk Mitigation

| Risk | Mitigation |
|------|------------|
| Excessive planning | Time-box + POC mandatory antes de full formalization |
| Perfectionism cascade | Rule of 3: simple pilot → 2 iterations → formalize |
| Premature configuration | Generalize apenas após ≥2 cenários reais |
| Context switching | Checklist de pending items em cada pivot |

---

## Integration com GitHub Copilot

### No Chat (Architecture Phase):
```
@workspace com skill architect-first:

Analisar impacto de migração de REST para GraphQL

Sistema atual: Express + REST API
Motivation: Frontend precisa query flexibility

Fornecer:
1. Map do estado atual
2. Opções A/B/C com trade-offs
3. Recomendação justificada
4. Design documentation
5. Migration plan
```

### No Chat (Validation Phase):
```
@workspace usando architect-first:

Validar este design mantém capability preservation:
- [Design atual]

Verificar:
1. Todas capacidades anteriores mantidas?
2. Gold standard baseline OK?
3. Zero coupling validado?
4. Config externalizada?
```

### No código (comment):
```typescript
/**
 * @skill:architect-first
 * @phase: architecture-flow
 * 
 * Map Before Modify:
 * - Current: Monolithic auth service
 * - New: Microservice auth + OAuth provider
 * - Dependencies: User service, Session store, API Gateway
 * 
 * Validation: Pending multi-agent review
 * Options: A/B/C documented in doc/architecture/auth-redesign.md
 */
```

---

## Acceptance Criteria

### ✅ Will Accept:
- "Ugly" code COM comprehensive tests
- 80% features SE core case coberto
- Large refactors que aumentam flexibility
- Extensive documentation se ensina customization

### ❌ Will Reject:
- "Ugly" code SEM tests
- Capability loss sem justificativa explícita
- Hardcoded mutable values
- Deployment sem core case functioning

---

## Templates

### Architecture Decision Record (ADR)
```markdown
# ADR: [Número] - [Título]

**Date:** YYYY-MM-DD
**Status:** Proposed | Accepted | Deprecated

## Context
[Por que essa decisão é necessária]

## Decision
[O que foi decidido]

## Options Considered
### Option A
**Pros:** ...
**Cons:** ...

### Option B
**Pros:** ...
**Cons:** ...

## Rationale
[Por que escolhemos essa opção]

## Consequences
**Positivas:**
- ...

**Negativas:**
- ...

**Risks:**
- ...
```

---

## Quick Reference

**Starting new feature:**
```
1. É decisão estrutural? → Architecture Flow
2. Não? → Pre-Implementation Checklist
3. Map estado atual (se architecture flow)
4. Apresentar opções A/B/C
5. Multi-agent validation
6. Document design
7. Validate capability preservation
8. Validate zero coupling
9. Implementar com test plan
10. Externalizar configs
```

**Refactoring existing code:**
```
1. Map Before Modify (document current state)
2. Identify capability baseline
3. Design refactoring maintaining capabilities
4. Add tests before refactoring
5. Refactor incrementally
6. Validate tests still pass
7. Update documentation
```

---

*Architect-First Skill - GitHub Copilot Edition v1.0*
*"Design once, implement many times"*

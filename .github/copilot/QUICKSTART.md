# Quick Start Guide - AIOS com GitHub Copilot

Guia rápido para migração do Claude Code para GitHub Copilot.

---

## 🚀 Migration Summary

### O que mudou?

| Claude Code | GitHub Copilot |
|-------------|----------------|
| `@agent-name` | `@context:agent-name` ou menção no chat |
| `*comando` | Conversa natural no chat |
| Agentes autônomos spawning | Assistente interativo com contextos |
| Memória persistente projeto | Sessão de chat |
| Tools (Read, Write, Bash) | Ferramentas VS Code integradas |

### O que foi preservado?

✅ Todos os agentes (como contextos)
✅ Todas as skills
✅ Constitution e princípios
✅ Templates e checklists
✅ Workflows e processos

---

## 📁 Nova Estrutura

```
.github/
├── copilot-instructions.md          # ⭐ Arquivo principal - LEIA PRIMEIRO
└── copilot/                          
    ├── README.md                     # Guia de uso detalhado
    ├── constitution.md               # Princípios fundamentais
    ├── agents/                       # Contextos de agentes
    │   ├── aios-dev.md              # Desenvolvedor (Dex)
    │   ├── aios-architect.md        # Arquiteto (Aria)
    │   ├── aios-qa.md               # QA (Quinn)
    │   ├── aios-devops.md           # DevOps (Gage)
    │   ├── aios-data-engineer.md    # Database (Dara)
    │   └── aios-additional.md       # PO, SM, PM, Analyst, UX
    └── skills/                       # Habilidades especializadas
        ├── architect-first.md        # Filosofia Architect-First
        └── additional-skills.md      # SYNAPSE, MCP Builder, Squad Creator
```

---

## 🎯 Como Usar Contextos

### No código (comentário especial):
```typescript
// @context:aios-dev
// Implementar autenticação de usuário [Story 2.1]

export function authenticate(credentials: Credentials) {
  // Copilot vai sugerir código seguindo:
  // - IDS Protocol
  // - TypeScript strict
  // - Error handling
  // - Imports absolutos
}
```

### No Copilot Chat:
```
@workspace usando contexto aios-dev:

Story: 2.1 - User Authentication
IDS Protocol: ON

Implementar login flow com:
1. Validação de credenciais
2. Token JWT
3. Error handling robusto
4. Testes unitários
```

### Exemplos práticos:

#### Desenvolvimento
```
@workspace contexto aios-dev, implementar feature X baseado na Story 2.1
```

#### Arquitetura
```
@workspace como aios-architect, analisar impacto de migração GraphQL
```

#### Code Review
```
@workspace contexto aios-qa, revisar PR #42 verificando:
- Todos AC implementados?
- Testes adequados?
- Quality gates OK?
```

#### Git Operations
```
@workspace como aios-devops, executar pre-push gates e push feature-branch
```

#### Database
```
@workspace contexto aios-data-engineer, criar migration para workflows table
```

---

## 🛠 Como Usar Skills

### Architect-First
```
@workspace com skill architect-first:

Análise arquitetural para Feature X

Processo:
1. Map Before Modify (estado atual)
2. Apresentar opções A/B/C com trade-offs
3. Validação multi-perspectiva
4. Design documentation
5. Gold standard baseline check
6. Zero coupling validation
```

### MCP Builder
```
@workspace usando skill mcp-builder:

Criar MCP server para Notion API

Tools prioritários:
- search_pages
- create_page
- update_block

Requirements:
- Workflow-oriented
- Agent-friendly errors
- Type-safe (Zod)
```

### SYNAPSE
```
@workspace skill synapse:

Criar domínio para React Performance:
- Patterns: memo, useMemo, useCallback
- Anti-patterns: premature optimization
- Tools: React DevTools
```

---

## 📋 Workflows Comuns

### 1. Nova Feature (Story-Driven)

```
Passo 1: Leia a story
@workspace ler docs/stories/active/2.1.md

Passo 2: Design arquitetural
@workspace contexto aios-architect, analisar impacto e desenhar solução

Passo 3: Implementação
@workspace como aios-dev:
- Aplicar IDS Protocol
- Implementar ACs
- Escrever testes
- Quality gates

Passo 4: Review
@workspace contexto aios-qa, revisar Story 2.1

Passo 5: Deploy
@workspace como aios-devops, pre-push gates e push
```

### 2. Code Review

```
@workspace usando contexto aios-qa:

Revisar Pull Request #X

Checklist:
✓ Constitution compliance (CLI First, Quality First)
✓ IDS Protocol aplicado
✓ Absolute imports
✓ TypeScript strict (sem 'any')
✓ Tests cobrem ACs
✓ Quality gates: lint, typecheck, tests, build

Gate Decision: APPROVED / NEEDS_WORK / FAIL
```

### 3. Database Changes

```
@workspace contexto aios-data-engineer:

Criar migration para feature X

Requirements:
- Schema design KISS compliant
- RLS policies (users see only own)
- Rollback script
- Indexes strategy
- Validation queries

Deliverables:
1. migration.sql
2. rollback.sql
3. Schema documentation
```

---

## 🚨 Troubleshooting

### Copilot não respeita contextos?

**Soluções:**
1. Reinicie VS Code
2. Seja mais explícito na menção:
   ```
   @workspace usando contexto aios-dev, com IDS Protocol ativo, TypeScript strict mode...
   ```
3. Use comentários inline no código:
   ```typescript
   // @context:aios-dev
   // @principle: absolute-imports, typescript-strict
   ```

### Como saber se está funcionando?

**Indicadores:**
- Copilot sugere imports absolutos (`@/...`)
- Evita `any` em TypeScript
- Adiciona error handling automaticamente
- Pergunta sobre Story ID ao sugerir código
- Menciona Constitution principles em review

### Quality gates falhando?

```bash
# Execute localmente e veja os erros
npm run lint:fix          # Auto-fix o que for possível
npm run typecheck         # Ver erros de tipo
npm test -- --verbose     # Ver testes falhando detalhadamente

# Delegue correções se necessário
@workspace contexto aios-dev, corrigir issues do typecheck
```

---

##  ⚡ Power Tips

### 1. Combine contextos + skills
```
@workspace como aios-architect com skill architect-first:
Análise completa de Feature X
```

### 2. Use checklists integrados
```
@workspace contexto aios-qa:
Aplicar quality gate checklist completo em Story 2.1
```

### 3. Aproveite auto-complete inteligente
Ao digitar código com contexto ativo, Copilot automaticamente:
- Usa imports absolutos
- Aplica TypeScript strict
- Adiciona error handling
- Segue naming conventions

### 4. Peça análise multi-perspectiva
```
@workspace análise multi-contexto:

aios-architect: Impacto técnico
aios-po: Alinhamento de negócio
aios-qa: Testability
aios-dev: Effort estimation

Para Feature: X
```

---

## 📚 Recursos

### Leitura Essencial (ordem sugerida):
1. [copilot-instructions.md](.github/copilot-instructions.md) - ⭐ COMECE AQUI
2. [constitution.md](.github/copilot/constitution.md) - Princípios fundamentais
3. [README.md](.github/copilot/README.md) - Guia de uso detalhado

### Por Necessidade:
- **Desenvolvedor:** [agents/aios-dev.md](.github/copilot/agents/aios-dev.md)
- **Arquiteto:** [agents/aios-architect.md](.github/copilot/agents/aios-architect.md)
- **QA:** [agents/aios-qa.md](.github/copilot/agents/aios-qa.md)
- **DevOps:** [agents/aios-devops.md](.github/copilot/agents/aios-devops.md)
- **Database:** [agents/aios-data-engineer.md](.github/copilot/agents/aios-data-engineer.md)

### Skills avançadas:
- [architect-first.md](.github/copilot/skills/architect-first.md)
- [additional-skills.md](.github/copilot/skills/additional-skills.md)

---

## 🎓 Learning Path

### Dia 1: Basics
- [ ] Ler copilot-instructions.md
- [ ] Ler constitution.md
- [ ] Testar um contexto (aios-dev)
- [ ] Implementar pequena feature com IDS Protocol

### Dia 2: Workflows
- [ ] Story-driven development completo
- [ ] Code review com aios-qa
- [ ] Git workflow com aios-devops

### Dia 3: Advanced
- [ ] Architect-first skill
- [ ] Multi-Agent validation
- [ ] Design architecture decision

### Semana 2+
- [ ] Criar custom skills
- [ ] Criar custom domains (SYNAPSE)
- [ ] Otimizar workflows pessoais

---

## 💬 Quick Reference Card

```
┌─────────────────────────────────────────────────────────┐
│ AIOS GitHub Copilot - Quick Reference                   │
├─────────────────────────────────────────────────────────┤
│                                                          │
│ CONTEXTOS (use no chat ou código):                      │
│  @workspace contexto aios-dev         - Implementação   │
│  @workspace como aios-architect       - Arquitetura     │
│  @workspace contexto aios-qa          - Qualidade       │
│  @workspace como aios-devops          - Git/CI/CD       │
│                                                          │
│ SKILLS (workflows especializados):                      │
│  com skill architect-first            - Design          │
│  usando skill mcp-builder            - MCP servers      │
│  skill synapse                       - Context mgmt     │
│                                                          │
│ CONSTITUTION (princípios):                              │
│  1. CLI First        → CLI antes de UI                  │
│  2. Agent Authority  → DevOps = único git push          │
│  3. Story-Driven     → Sempre com story ID              │
│  4. No Invention     → Search → Reuse → Adapt → Create  │
│  5. Quality First    → Gates obrigatórios               │
│  6. Absolute Imports → @/ sempre, ../ nunca            │
│                                                          │
│ QUALITY GATES (pre-push):                               │
│  npm run lint && typecheck && test                      │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

---

## 🤝 Support

### Dúvidas?
1. Consulte [copilot-instructions.md](.github/copilot-instructions.md)
2. Verifique [constitution.md](.github/copilot/constitution.md)
3. Leia agent/skill específico

### Problemas?
1. Check troubleshooting neste guia
2. Valide configuração VS Code
3. Teste em arquivo isolado

### Contribuir?
1. Siga [CONTRIBUTING.md](../../CONTRIBUTING.md)
2. Use contextos apropriados
3. Quality gates obrigatórios
4. Reference Story ID

---

*Quick Start Guide v1.0 - AIOS + GitHub Copilot*
*Enjoy your enhanced development experience! 🚀*

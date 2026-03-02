# Synkra AIOS Constitution

> **Version:** 1.0.0 | **GitHub Copilot Edition**

Este documento define os princípios fundamentais e inegociáveis do Synkra AIOS. Todos os contextos de Copilot, desenvolvimento, e code reviews DEVEM respeitar estes princípios.

---

## Core Principles

### I. CLI First (NON-NEGOTIABLE)

O CLI é a fonte da verdade onde toda inteligência, execução, e automação vivem.

**Regras:**
- ✅ MUST: Toda funcionalidade nova DEVE funcionar 100% via CLI antes de qualquer UI
- ✅ MUST: Dashboards apenas observam, NUNCA controlam ou tomam decisões
- ✅ MUST: A UI NUNCA é requisito para operação do sistema
- ✅ MUST: Ao decidir onde implementar, sempre CLI > Observability > UI

**Hierarquia:**
```
CLI (Máxima) → Observability (Secundária) → UI (Terciária)
```

**Quando o Copilot sugerir código:**
- Sempre priorize implementações CLI
- Se sugerir UI, garanta que o CLI já funciona 100%
- Questione propostas que invertem essa hierarquia

---

### II. Agent Authority (NON-NEGOTIABLE)

Cada contexto tem responsabilidades exclusivas que não podem ser violadas.

**Exclusividades:**

| Autoridade | Contexto Exclusivo |
|------------|-------------------|
| git push | `aios-devops` |
| PR creation | `aios-devops` |
| Release/Tag | `aios-devops` |
| Story creation | `aios-sm`, `aios-po` |
| Architecture decisions | `aios-architect` |
| Quality verdicts | `aios-qa` |

**Quando usar contextos:**
- Use comentário `@context:nome` para ativar especialidade
- No chat: "usando contexto aios-dev, implemente..."
- Respeite as autoridades exclusivas

---

### III. Story-Driven Development (MUST)

Todo desenvolvimento começa e termina com uma story.

**Regras:**
- ✅ MUST: Nenhum código é escrito sem uma story associada em `docs/stories/`
- ✅ MUST: Stories DEVEM ter acceptance criteria claros antes de implementação
- ✅ MUST: Progresso DEVE ser rastreado via checkboxes na story
- ✅ MUST: File List DEVE ser mantida atualizada na story
- 💡 SHOULD: Stories seguem: criar → implementar → validar → deploy

**Workflow:**
```
docs/stories/active/{story-id}.md
  ↓
Implementação com referência [Story X.Y]
  ↓
Tests + Quality Gates
  ↓
Commit: "feat: feature name [Story X.Y]"
```

**Quando codificar:**
- Sempre identifique qual story está implementando
- Referencie story ID em commits
- Atualize checkboxes conforme progresso

---

### IV. No Invention (MUST)

Especificações não inventam - apenas derivam dos requisitos.

**Regras:**
- ✅ MUST: Todo código DEVE rastrear para requirement documentado
- ❌ MUST NOT: Adicionar features não presentes nos requisitos
- ❌ MUST NOT: Assumir detalhes de implementação não pesquisados
- ❌ MUST NOT: Especificar tecnologias não validadas

**IDS Protocol (Invent/Discover/Decide):**
Antes de criar qualquer arquivo novo:

1. **SEARCH FIRST**: Busque similar em `squads/`, componentes, código existente
2. **DECIDE**: REUSE / ADAPT / CREATE (justificado)
3. **LOG**: Documente decisão no commit ou story

**Quando o Copilot sugerir código novo:**
- Primeiro busque se já existe algo similar
- Prefira reusar ou adaptar código existente
- Só crie novo se justificado

---

### V. Quality First (MUST)

Qualidade não é negociável. Todo código passa por múltiplos gates antes de merge.

**Pre-Push Quality Gates:**
```bash
npm run lint        # ESLint - MUST pass
npm run typecheck   # TypeScript - MUST pass
npm test            # Jest - MUST pass
npm run build       # Build - MUST succeed
```

**Regras:**
- ✅ MUST: Todos os checks passam sem erros
- ✅ MUST: Story status é "Done" ou "Ready for Review"
- 💡 SHOULD: Cobertura de testes não diminui

**Quando escrever código:**
- Execute lint e typecheck frequentemente
- Escreva testes junto com implementação
- Valide build antes de commit

---

### VI. Absolute Imports (SHOULD)

Imports relativos criam acoplamento e dificultam refatoração.

**Regras:**
- 💡 SHOULD: Sempre usar imports absolutos com alias `@/`
- ⚠️ SHOULD NOT: Usar imports relativos (`../../../`)
- ℹ️ EXCEPTION: Imports dentro do mesmo módulo/feature podem ser relativos

**Exemplos:**
```typescript
// ✅ CORRETO
import { useStore } from '@/stores/feature/store'
import { Component } from '@/components/ui/component'

// ❌ INCORRETO  
import { useStore } from '../../../stores/feature/store'
import { Component } from '../../ui/component'

// ℹ️ ACEITÁVEL (mesmo módulo)
import { helper } from './helpers'
```

**Ordem de imports:**
1. React/core libraries
2. External libraries
3. UI components
4. Utilities
5. Stores
6. Feature imports
7. CSS imports

---

## TypeScript Standards

### Strict Typing
```typescript
// ✅ CORRETO
interface UserProps {
  name: string
  age: number
  onUpdate: (user: User) => void
}

// ❌ INCORRETO - never use 'any'
interface UserProps {
  name: any
  age: any
  onUpdate: any
}
```

### Error Handling
```typescript
// ✅ CORRETO
try {
  await operation()
} catch (error) {
  logger.error(`Failed to ${operation}`, { error })
  throw new Error(
    `Failed to ${operation}: ${
      error instanceof Error ? error.message : 'Unknown'
    }`
  )
}

// ❌ INCORRETO - swallow errors silently
try {
  await operation()
} catch (error) {
  // empty catch
}
```

---

## Naming Conventions

| Tipo | Convenção | Exemplo |
|------|-----------|---------|
| Componentes | PascalCase | `WorkflowList` |
| Hooks | prefixo `use` | `useWorkflowOperations` |
| Arquivos | kebab-case | `workflow-list.tsx` |
| Constantes | SCREAMING_SNAKE_CASE | `MAX_RETRIES` |
| Interfaces | PascalCase + sufixo | `WorkflowListProps` |
| Funções | camelCase | `handleSubmit` |
| CSS Classes | kebab-case | `btn-primary` |

---

## Git Conventions

### Commits (Conventional Commits)
```bash
feat: add user authentication [Story 2.1]
fix: resolve login timeout issue [Story 2.1]
docs: update API documentation
test: add integration tests for auth
chore: update dependencies
refactor: simplify user service logic
```

Formato: `<type>: <description> [Story X.Y]`

**Types:**
- `feat:` - Nova funcionalidade
- `fix:` - Correção de bug
- `docs:` - Documentação
- `test:` - Testes
- `chore:` - Manutenção
- `refactor:` - Refatoração sem mudança funcional

### Branches
```bash
main                    # Branch principal
feat/feature-name       # Features
fix/bug-name           # Correções
docs/doc-name          # Documentação
```

---

## Copilot Integration

### Ativação de Princípios no Chat

```
@workspace seguindo Constitution:
- CLI First: implemente primeiro a função CLI
- No Invention: busque código similar antes
- Quality First: inclua testes

Implemente feature X
```

### Code Review com Constitution

```
@workspace como aios-qa, revise este código verificando:
1. CLI First - UI só se CLI funciona?
2. Story-Driven - Story ID referenciado?
3. No Invention - Reutilizou código existente?
4. Quality First - Testes incluídos?
5. Absolute Imports - Sem imports relativos?
6. TypeScript - Sem 'any'?
```

### Auto-Correção

O Copilot pode auto-corrigir violações comuns:

```javascript
// @context:aios-dev
// @auto-fix: absolute-imports, typescript-strict

// O Copilot vai sugerir automaticamente imports absolutos
// e tipos estritos ao escrever código
```

---

## Compliance & Enforcement

### Automated Checks (Pre-commit/Pre-push)

```bash
# Executado automaticamente em pre-push
npm run lint              # ESLint - bloqueia violações
npm run typecheck         # TypeScript - bloqueia 'any'
npm test                  # Jest - bloqueia falhas
```

### Manual Review (Pull Request)

Checklist de PR:
- [ ] Story ID referenciado no título/descrição
- [ ] CLI First respeitado (se aplicável)
- [ ] Imports absolutos usados
- [ ] TypeScript strict (sem `any`)
- [ ] Testes adicionados
- [ ] Docs atualizadas
- [ ] Quality gates passam

---

## Quick Reference Card

```
┌─────────────────────────────────────────────────────────┐
│ AIOS Constitution - Quick Reference                     │
├─────────────────────────────────────────────────────────┤
│                                                          │
│ 1. CLI First        → Funcionalidade CLI antes de UI    │
│ 2. Agent Authority  → Respeite autoridades exclusivas   │
│ 3. Story-Driven     → Código começa com story           │
│ 4. No Invention     → Search → Reuse → Adapt → Create   │
│ 5. Quality First    → Lint + TypeCheck + Test + Build   │
│ 6. Absolute Imports → @/ sempre, ../../../ nunca       │
│                                                          │
│ TypeScript: No 'any' | Strict types | Error handling    │
│ Naming: Components=PascalCase | files=kebab-case        │
│ Git: feat/fix/docs: message [Story X.Y]                 │
│                                                          │
└─────────────────────────────────────────────────────────┘
```

---

*Synkra AIOS Constitution for GitHub Copilot v1.0*
*CLI First | Observability Second | UI Third*

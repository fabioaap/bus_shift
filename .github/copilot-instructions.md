# GitHub Copilot - AIOS Configuration

Este arquivo configura o GitHub Copilot para trabalhar no repositório Synkra AIOS utilizando o sistema de contextos e skills.

---

## 🎯 Quick Start

### Ativação de Contextos
```
@workspace usando contexto aios-dev, implemente feature X
@workspace como aios-architect, analise impacto de Y
@workspace contexto aios-qa, revise este código
```

### Ativação de Skills
```
@workspace com skill architect-first, desenhe arquitetura
@workspace usando skill mcp-builder, crie servidor MCP
@workspace skill synapse, gerencie domínios
```

---

## 📚 Constitution (Princípios Fundamentais)

**Ver:** [constitution.md](.github/copilot/constitution.md) - Documento completo

### Princípios Essenciais (NON-NEGOTIABLE)

1. **CLI First** 
   - CLI é fonte da verdade
   - Funcionalidade CLI antes de UI
   - Hierarquia: CLI > Observability > UI

2. **Agent Authority**
   - Apenas `aios-devops` faz git push
   - Respeite autoridades exclusivas

3. **Story-Driven Development**
   - Todo código começa com story em `docs/stories/`
   - Reference Story ID em commits

4. **No Invention (IDS Protocol)**
   - Search → Reuse → Adapt → Create
   - Nunca invente, sempre derive de requirements

5. **Quality First**
   - `npm run lint && typecheck && test` deve passar
   - Pre-push quality gates obrigatórios

6. **Absolute Imports**
   - Sempre `@/...` nunca `../../../`

---

## 🤖 Agentes Disponíveis

**Ver:** [agents/](.github/copilot/agents/) - Detalhes completos

| Contexto | Persona | Especialidade | Push Git? |
|----------|---------|---------------|-----------|
| `aios-dev` | Dex | Implementação de código | ❌ |
| `aios-architect` | Aria | Arquitetura e design | ❌ |
| `aios-qa` | Quinn | Testes e qualidade | ❌ |
| `aios-devops` | Gage | CI/CD, git operations | ✅ ÚNICO |
| `aios-data-engineer` | Dara | Database, migrations, RLS | ❌ |
| `aios-po` | Pax | Product Owner, stories | ❌ |
| `aios-sm` | River | Scrum Master, sprints | ❌ |
| `aios-pm` | Morgan | Product Manager, PRDs | ❌ |
| `aios-analyst` | Alex | Research, análise | ❌ |
| `aios-ux` | Uma | UI/UX, design system | ❌ |

### Links Rápidos
- [Dev Context](agents/aios-dev.md) - Implementação com IDS Protocol
- [Architect Context](agents/aios-architect.md) - Architect-First workflow
- [QA Context](agents/aios-qa.md) - Quality gates e reviews
- [DevOps Context](agents/aios-devops.md) - Git, CI/CD, deploys
- [Data Engineer Context](agents/aios-data-engineer.md) - Database design
- [Additional Agents](agents/aios-additional.md) - PO, SM, PM, Analyst, UX

---

## 🛠 Skills Disponíveis

**Ver:** [skills/](.github/copilot/skills/) - Detalhes completos

| Skill | Quando Usar | Arquivo |
|-------|-------------|---------|
| `architect-first` | Decisões arquiteturais, novos features | [architect-first.md](skills/architect-first.md) |
| `synapse` | Gestão de contexto, domínios | [additional-skills.md](skills/additional-skills.md#synapse) |
| `mcp-builder` | Criar servidores MCP | [additional-skills.md](skills/additional-skills.md#mcp-builder) |
| `skill-creator` | Criar novas skills | [additional-skills.md](skills/additional-skills.md#skill-creator) |
| `squad` | Criar squads de especialistas | [additional-skills.md](skills/additional-skills.md#squad-creator) |

---

## 📝 Padrões de Código

### TypeScript Standards

```typescript
// ✅ CORRETO - Strict typing, absolute imports

import { useStore } from '@/stores/workflow'
import { Button } from '@/components/ui/button'

interface WorkflowProps {
  id: string
  onComplete: (result: WorkflowResult) => Promise<void>
}

export async function processWorkflow(props: WorkflowProps): Promise<Result> {
  try {
    const result = await workflowService.process(props.id)
    await props.onComplete(result)
    return { success: true, data: result }
  } catch (error) {
    logger.error('Failed to process workflow', { error, id: props.id })
    throw new Error(
      `Workflow processing failed: ${
        error instanceof Error ? error.message : 'Unknown error'
      }`
    )
  }
}
```

```typescript
// ❌ INCORRETO - Relative imports, 'any', no error handling

import { useStore } from '../../../stores/workflow'  // relative import

interface WorkflowProps {
  id: any                                              // 'any'
  onComplete: any                                      // 'any'
}

export function processWorkflow(props: any) {          // 'any'
  const result = workflowService.process(props.id)     // no await, no try/catch
  props.onComplete(result)                             // no await
  return true                                          // imprecise type
}
```

### Naming Conventions

```typescript
// Components
export function WorkflowList() {}          // PascalCase

// Hooks  
export function useWorkflowOperations() {} // use + camelCase

// Constants
export const MAX_RETRY_ATTEMPTS = 3        // SCREAMING_SNAKE_CASE

// Functions
function handleSubmit() {}                 // camelCase

// Files: workflow-list.tsx                // kebab-case
```

### Imports Order

```typescript
// 1. React/core
import React, { useState } from 'react'

// 2. External libraries
import { z } from 'zod'
import axios from 'axios'

// 3. UI components
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'

// 4. Utilities
import { logger } from '@/lib/logger'
import { api } from '@/lib/api'

// 5. Stores
import { useWorkflowStore } from '@/stores/workflow'

// 6. Feature imports
import { WorkflowService } from '@/features/workflow/service'

// 7. CSS
import './workflow.css'
```

---

## 🧪 Quality Gates

### Pre-Commit (Local)
```bash
npm run lint              # ESLint - must pass
npm run typecheck         # TypeScript - must pass
```

### Pre-Push (Mandatory)
```bash
npm run lint              # ESLint
npm run typecheck         # TypeScript
npm test                  # Jest - all tests pass
npm run build             # Build succeeds
```

**Se QUALQUER falhar:** NÃO push até corrigir.

---

## 📋 Workflows Comuns

### 1. Implementar Nova Feature

```
1. @workspace ler story em docs/stories/active/{id}.md

2. @workspace usando contexto aios-architect:
   Analisar impacto da feature, desenhar arquitetura

3. @workspace contexto aios-dev com IDS Protocol:
   - Buscar código similar (Search)
   - Decidir: Reuse/Adapt/Create
   - Implementar feature
   - Escrever testes
   - Quality gates

4. @workspace como aios-qa:
   Revisar código, validar AC, executar gates

5. @workspace contexto aios-devops:
   Commit, pre-push gates, push to branch
```

### 2. Code Review

```
@workspace usando contexto aios-qa:

Revisar Pull Request #X

Verificar:
1. Constitution compliance (CLI First, Story-Driven, No Invention)
2. Quality gates passam (lint, typecheck, tests)
3. Absolute imports usados
4. TypeScript strict (sem 'any')
5. Testes cobrem AC
6. Documentação atualizada

Gate Decision: APPROVED / NEEDS_WORK / FAIL
```

### 3. Database Migration

```
@workspace contexto aios-data-engineer:

Criar migration para adicionar tabela 'workflows'

Schema:
- id (uuid, PK)
- name (text, not null)
- user_id (uuid, FK users)
- created_at (timestamptz)

Incluir:
1. Migration script
2. Rollback script
3. RLS policies (users see only own)
4. Indexes (user_id, created_at)
5. Validation queries
```

### 4. Architecture Decision

```
@workspace com skill architect-first:

Decisão: Migrar de REST para GraphQL

Workflow:
1. Map Before Modify (estado atual)
2. Apresentar Opções A/B/C com trade-offs
3. Multi-Agent Validation (PO, Architect, QA, Dev)
4. Design Documentation
5. Gold Standard (manter capabilities)
6. Zero Coupling
7. Implementação
```

---

## 🔧 Git Workflow

### Commit Messages (Conventional Commits)

```bash
# Format
<type>: <description> [Story X.Y]

# Examples
feat: add user authentication [Story 2.1]
fix: resolve login timeout issue [Story 2.1]
docs: update API documentation
test: add integration tests for auth
refactor: simplify user service logic
chore: update dependencies
perf: optimize query performance
```

### Branch Strategy

```bash
main                    # Production branch
feat/feature-name       # New features
fix/bug-name           # Bug fixes
docs/doc-name          # Documentation
```

### Pre-Push Checklist

```bash
# Execute quality gates
npm run lint && npm run typecheck && npm test

# Stage changes selectively (NUNCA git add -A)
git add src/components/**/*.tsx
git add src/services/**/*.ts
git add tests/**/*.test.ts

# Commit with proper message
git commit -m "feat: implement feature [Story X.Y]"

# Push (APENAS aios-devops autorizado)
git push origin feature-branch
```

---

## 🎨 UI/UX Patterns

### Component Structure

```typescript
// component-name.tsx

import { FC } from 'react'
import { cn } from '@/lib/utils'

interface ComponentNameProps {
  variant?: 'primary' | 'secondary'
  size?: 'sm' | 'md' | 'lg'
  disabled?: boolean
  children: React.ReactNode
  onAction?: () => void
}

export const ComponentName: FC<ComponentNameProps> = ({
  variant = 'primary',
  size = 'md',
  disabled = false,
  children,
  onAction
}) => {
  return (
    <div 
      className={cn(
        'base-classes',
        variant === 'primary' && 'primary-classes',
        variant === 'secondary' && 'secondary-classes',
        size === 'sm' && 'sm-classes',
        size === 'md' && 'md-classes',
        disabled && 'disabled-classes'
      )}
      onClick={!disabled ? onAction : undefined}
    >
      {children}
    </div>
  )
}
```

---

## 🔍 IDS Protocol (Obrigatório)

**Antes de criar QUALQUER arquivo novo:**

```
1. SEARCH FIRST
   - Glob: busque padrões similares
   - Grep: busque implementações parecidas  
   - Check: squads/, components/, services/

2. DECIDE
   - REUSE: Código existente resolve 100%?
   - ADAPT: Código existente resolve com modificação?
   - CREATE: Justamente necessário criar novo?

3. LOG DECISION
   Documente em comentário ou commit:
   // [IDS] REUSE: using existing PatternX from squads/Y
   // [IDS] ADAPT: modified ComponentZ to add feature W
   // [IDS] CREATE: no existing (searched: X, Y, Z)
```

---

## 🚨 Common Mistakes & Fixes

### ❌ Mistake: Imports relativos
```typescript
import { util } from '../../../utils/helper'
```

### ✅ Fix: Imports absolutos
```typescript
import { util } from '@/utils/helper'
```

---

### ❌ Mistake: Using 'any'
```typescript
function process(data: any): any {
  return data
}
```

### ✅ Fix: Proper types
```typescript
function process(data: ProcessInput): ProcessOutput {
  return {
    success: true,
    result: data.value
  }
}
```

---

### ❌ Mistake: No error handling
```typescript
const result = await api.call()
return result
```

### ✅ Fix: Proper error handling
```typescript
try {
  const result = await api.call()
  return { success: true, data: result }
} catch (error) {
  logger.error('API call failed', { error })
  throw new Error(
    `Operation failed: ${
      error instanceof Error ? error.message : 'Unknown'
    }`
  )
}
```

---

### ❌ Mistake: Criar arquivo sem IDS
```typescript
// Creating new service without checking existing ones
export class NewService {}
```

### ✅ Fix: Apply IDS Protocol
```typescript
// [IDS] SEARCH: Checked services/, found BaseService
// [IDS] DECISION: ADAPT - extending BaseService with custom logic
// [IDS] RATIONALE: Reuses auth, error handling, logging

export class NewService extends BaseService {
  // Only custom logic here
}
```

---

## 📚 Learning Resources

### Internal Documentation
- [README.md](README.md) - Project overview
- [CONTRIBUTING.md](CONTRIBUTING.md) - Como contribuir
- [docs/](docs/) - Documentação completa
- [docs/stories/](docs/stories/) - Development stories

### AIOS Architecture
- [.aios-core/](.aios-core/) - AIOS framework core
- [.aios-core/core/](.aios-core/core/) - Core modules
- [.aios-core/data/](.aios-core/data/) - Knowledge base

### Constitution & Principles
- [Constitution](constitution.md) - Princípios fundamentais
- [Architect-First](skills/architect-first.md) - Design philosophy
- [IDS Protocol]() - Search → Reuse → Adapt → Create

---

## 🆘 Troubleshooting

### Copilot não usa contextos?
```
Tente:
1. Reinicie VS Code
2. Use menção explícita: "@workspace usando contexto aios-dev"
3. Adicione comentário no código: // @context:aios-dev
```

### Quality gates falhando?
```
1. Execute localmente:
   npm run lint:fix          # Auto-fix lint issues
   npm run typecheck         # See type errors
   npm test -- --verbose     # See failing tests

2. Corrija issues
3. Re-execute gates
```

### IDS Protocol esquecido?
```
Antes de criar arquivo:
1. Pause
2. Execute search (grep, glob)
3. Encontrou similar? Reuse ou Adapt
4. Não encontrou? Documente o que buscou
5. Só então Create
```

---

## 🔗 Quick Links

| Recurso | Link |
|---------|------|
| **Configuration** | [.github/copilot/](.github/copilot/) |
| **Agents** | [agents/](.github/copilot/agents/) |
| **Skills** | [skills/](.github/copilot/skills/) |
| **Constitution** | [constitution.md](.github/copilot/constitution.md) |
| **README** | [README.md](.github/copilot/README.md) |
| **Original Claude Config** | [.claude/CLAUDE.md](.claude/CLAUDE.md) |

---

## 📖 Contribution Guidelines

Ao contribuir:

1. **Siga Constitution** - Princípios non-negotiable
2. **Use Story-Driven** - Reference story ID
3. **Apply IDS Protocol** - Search antes de criar
4. **Quality Gates** - Todos devem passar
5. **Conventional Commits** - Formato padronizado
6. **Absolute Imports** - Sempre `@/...`
7. **TypeScript Strict** - Sem `any`

---

*Synkra AIOS - GitHub Copilot Configuration v1.0*
*CLI First | Observability Second | UI Third*

# AIOS Skills - GitHub Copilot Edition

Este arquivo consolida skills especializadas do AIOS adaptadas para GitHub Copilot.

---

## Table of Contents

1. [SYNAPSE Context Engine](#synapse)
2. [MCP Builder](#mcp-builder)
3. [Skill Creator](#skill-creator)
4. [Squad Creator](#squad-creator)

---

## SYNAPSE Context Engine {#synapse}

**Ativação:** `@skill:synapse`

### Overview
SYNAPSE (Synkra Adaptive Processing & State Engine) é o context engine unificado do AIOS que injeta regras contextuais adaptativas.

### Conceitos-Chave

#### 8-Layer Processing Pipeline
```
L0: Constitution      - Princípios não-negociáveis
L1: Global Context    - Regras globais do projeto
L2: Agent Context     - Regras específicas de agente ativo
L3: Workflow Context  - Regras do workflow atual
L4: Domain Memory     - Knowledge base de domínios
L5: Session Memory    - Estado da sessão atual
L6: Task Context      - Contexto da task sendo executada
L7: Star-Commands     - Comandos especiais disponíveis
```

#### Context Brackets (Adaptive Injection)
```
FRESH      (0-50%)    - Full context injection
MODERATE   (50-75%)   - Reduced detail  
DEPLETED   (75-90%)   - Minimal context
CRITICAL   (90-100%)  - Emergency mode - essential only
```

### Quando Usar SYNAPSE

```
✅ Gerenciar domínios de conhecimento
✅ Configurar context rules específicos de projeto
✅ Criar star-commands personalizados
✅ Troubleshoot regra injection
✅ Arquitetura de contexto multi-layer
```

### Domain Structure

```markdown
# Domain: [domain-name]

## Rules
- Rule 1: Description
- Rule 2: Description

## Patterns
- Pattern 1: Example code or approach
- Pattern 2: Example code or approach

## Anti-Patterns
- Anti-pattern 1: What to avoid and why
- Anti-pattern 2: What to avoid and why

## Examples
```code
Example implementation
```
```

### Domain Manifest
```ini
# .synapse/manifest
domain-name=.synapse/domains/domain-name.md
agent-dev=.synapse/domains/agent-dev.md
workflow-story-dev=.synapse/domains/workflow-story-dev.md
```

### Star-Commands
```
*brief         - Switch to brief response mode
*dev           - Switch to developer mode  
*review        - Switch to code review mode
*synapse status - Show engine state
*synapse domains - List registered domains
*synapse debug  - Detailed debug info
```

### Integration com Copilot

```
@workspace com skill synapse:

Criar domínio customizado para:
- Feature: GraphQL API patterns
- Regras: Query optimization, N+1 prevention
- Patterns: DataLoader usage, batching
- Anti-patterns: Overfetching, underfetching

Gerar domain file e manifest entry
```

---

## MCP Builder {#mcp-builder}

**Ativação:** `@skill:mcp-builder`

### Overview
Skill para criar servidores MCP (Model Context Protocol) de alta qualidade que permitem LLMs interagir com serviços externos.

### Quando Usar
```
✅ Criar MCP servers para APIs externas
✅ Integrar serviços third-party
✅ Expor ferramentas para LLMs
✅ Build em Python (FastMCP) ou Node/TypeScript (MCP SDK)
```

### Agent-Centric Design Principles

#### 1. Build for Workflows, Not Just API Endpoints
```
❌ Wrap every API endpoint = 50 tools
✅ Consolidate into workflows = 10 tools

Example:
❌ check_availability + create_event (2 calls)
✅ schedule_event (1 call - checks + creates)
```

#### 2. Optimize for Limited Context
```
✅ Return high-signal info, not data dumps
✅ Provide "concise" vs "detailed" options
✅ Use human-readable identifiers (names > IDs)
✅ Character limits (25k tokens max)

Example:
❌ Return 500 user objects
✅ Return top 10 matching users with pagination
```

#### 3. Design Actionable Error Messages
```
❌ "Error: Invalid parameter"
✅ "Error: 'status' must be one of: active, pending, completed. 
    Try: filter='active_only' to reduce results."

Guide agents toward correct usage
```

#### 4. Natural Task Subdivisions
```
Tool names reflect human thinking:

✅ create_ticket, update_ticket, close_ticket
✅ search_users, get_user_details, update_user_profile

❌ api_post_v2, api_patch_v2_user
```

### 4-Phase Workflow

#### Phase 1: Research & Planning
```
1. Study MCP protocol docs
2. Study framework docs (Python SDK or TS SDK)
3. Exhaustively study target API
4. Create implementation plan:
   - Tool selection (prioritize high-value workflows)
   - Shared utilities design
   - Input/output design (Pydantic/Zod)
   - Error handling strategy
```

#### Phase 2: Implementation

**Python (FastMCP):**
```python
from mcp import Server
from pydantic import BaseModel, Field

class SearchInput(BaseModel):
    query: str = Field(..., description="Search query")
    limit: int = Field(10, ge=1, le=100)

server = Server("my-service")

@server.register_tool(
    name="search_items",
    description="Search for items with detailed results",
    readOnlyHint=True
)
async def search_items(input: SearchInput):
    """Search items with pagination support."""
    # Implementation
    return {
        "items": results,
        "total": total,
        "hasMore": has_more
    }
```

**TypeScript (MCP SDK):**
```typescript
import { Server } from "@modelcontextprotocol/sdk/server/index.js";
import { z } from "zod";

const SearchInputSchema = z.object({
  query: z.string().describe("Search query"),
  limit: z.number().min(1).max(100).default(10)
}).strict();

server.registerTool({
  name: "search_items",
  description: "Search for items with detailed results",
  inputSchema: zodToJsonSchema(SearchInputSchema),
  annotations: {
    readOnlyHint: true
  }
}, async ({ query, limit }) => {
  // Implementation
  return {
    content: [{
      type: "text",
      text: JSON.stringify(results)
    }]
  };
});
```

#### Phase 3: Review & Refine
```
Code Quality:
- [ ] DRY: No code duplication
- [ ] Composability: Shared logic extracted
- [ ] Consistency: Similar operations similar formats
- [ ] Error Handling: All external calls protected
- [ ] Type Safety: Full type coverage
- [ ] Documentation: Comprehensive docstrings

Testing:
⚠️ MCP = long-running process
   Don't run directly (will hang)
   Use evaluation harness or tmux
```

#### Phase 4: Evaluations
```
Create realistic test scenarios:
- Can LLM accomplish real-world tasks?
- Are tools discoverable?
- Are error messages helpful?
- Is context usage optimized?
```

### Best Practices Checklist

```
Tool Design:
- [ ] Task-oriented (workflows, not just CRUD)
- [ ] Concise vs detailed response options
- [ ] Pagination support
- [ ] Character limits enforced
- [ ] Human-readable identifiers

Input Validation:
- [ ] Pydantic (Python) or Zod (TypeScript)
- [ ] Constraints (min/max, regex)
- [ ] Clear descriptions with examples
- [ ] Diverse examples in docs

Output Format:
- [ ] Multiple formats (JSON, Markdown)
- [ ] Consistent structure across tools
- [ ] Truncated if too large
- [ ] High-signal information

Error Handling:
- [ ] Natural language errors
- [ ] Actionable suggestions
- [ ] Guide toward correct usage
- [ ] Rate limiting handled gracefully
```

### Integration com Copilot

```
@workspace com skill mcp-builder:

Criar MCP server para GitHub API

Tools prioritários:
- search_issues (query, labels, state)
- create_issue (title, body, labels)
- update_issue (issue_number, updates)
- list_pull_requests (state, filters)

Requirements:
- Pagination support
- Concise/detailed modes
- Actionable errors
- Type-safe inputs (Zod)

Language: TypeScript
```

---

## Skill Creator {#skill-creator}

**Ativação:** `@skill:skill-creator`

### Overview
Skill para criar novas skills especializadas que estendem capacidades do assistente.

### Quando Usar
```
✅ Criar nova skill especializada
✅ Atualizar skill existente
✅ Documentar domain knowledge
�✅ Estruturar workflows complexos
```

### Skill Structure

```markdown
# Skill Name

**Ativação:** `@skill:skill-name` or in chat

## Overview
[O que a skill faz, quando usar]

## Core Concepts
[Conceitos fundamentais da skill]

## Workflows
[Passo a passo de processos]

## Examples
[Exemplos práticos]

## Best Practices
[Checklist e guidelines]

## Integration
[Como usar com Copilot]

## Common Issues
[Problemas comuns e soluções]
```

### Skill Types

#### Knowledge Skill
```
Encapsula domain knowledge específico
Exemplo: "Python async patterns"

Structure:
- Concepts
- Patterns
- Anti-patterns
- Examples
- References
```

#### Workflow Skill
```
Define processo multi-step
Exemplo: "Database migration workflow"

Structure:
- Prerequisites
- Steps (sequential)
- Checkpoints
- Validation
- Rollback procedures
```

#### Tool Integration Skill
```
Como usar ferramenta específica
Exemplo: "Docker MCP usage"

Structure:
- Tool overview
- Common operations
- Configuration
- Troubleshooting
- Examples
```

### Skill Creation Workflow

```
1. IDENTIFY NEED
   - Tarefa repetitiva?
   - Knowledge domain específico?
   - Workflow complexo?

2. STRUCTURE
   - Escolha skill type
   - Define sections
   - Plan examples

3. DOCUMENT
   - Write comprehensive guide
   - Include examples
   - Add checklists

4. INTEGRATE
   - Add activation patterns
   - Test with Copilot
   - Iterate based on usage

5. MAINTAIN
   - Update as knowledge grows
   - Refine based on feedback
   - Keep examples current
```

### Integration com Copilot

```
@workspace com skill skill-creator:

Criar skill para "React Performance Optimization"

Incluir:
- Common performance issues
- Profiling techniques
- Optimization patterns (memo, callback, useMemo)
- Anti-patterns (premature optimization)
- Measurement tools (React DevTools, Profiler)

Format: Knowledge + Workflow skill
```

---

## Squad Creator {#squad-creator}

**Ativação:** `@skill:squad`

### Overview
Skill para criar squads (times de especialistas AI) para domínios específicos.

### Quando Usar
```
✅ Criar team de especialistas para domínio
✅ Orchestrar múltiplas perspectivas
✅ Clone minds de experts reais
✅ Manage existing squads
```

### Squad Architecture

```
Squad Structure:
├── Chief (Orchestrator)
│   ├── Diagnóstico Tier 0
│   ├── Routing para specialist
│   └── Coordination
├── Tier 1 Specialists (Core)
│   ├── Specialist A
│   ├── Specialist B
│   └── Specialist C
└── Tier 2 Specialists (Domain-specific)
    ├── Specialist D
    └── Specialist E
```

### Squad Types

#### Expert Squad
```
Team de experts em domínio técnico
Exemplo: "Cybersecurity Squad"

Members:
- Chief: Router e coordinator
- Penetration Tester
- Security Architect
- Compliance Specialist
- Incident Responder
```

#### Creative Squad
```
Team de creators/writers
Exemplo: "Copywriting Squad"

Members:
- Chief: Voice coordinator
- David Ogilvy (Headlines)
- Eugene Schwartz (Body copy)
- Claude Hopkins (Testing)
```

#### Process Squad
```
Team para workflows específicos
Exemplo: "Data Intelligence Squad"

Members:
- Chief: Process orchestrator
- Data Engineer
- Analytics Specialist
- BI Developer
```

### Squad Creation Template

```yaml
squad:
  name: "[Squad Name]"
  chief:
    name: "[Chief Name]"
    role: "Orchestrator"
    expertise:
      - Diagnostic
      - Routing
      - Coordination
    
  tier1:
    - name: "[Specialist 1]"
      expertise: "[Domain]"
      personality: "[Traits]"
      
    - name: "[Specialist 2]"
      expertise: "[Domain]"
      personality: "[Traits]"
  
  tier2:
    - name: "[Specialist 3]"
      expertise: "[Narrow domain]"
      personality: "[Traits]"

  workflows:
    - name: "[Workflow 1]"
      trigger: "[When to activate]"
      steps:
        - Chief: Diagnose
        - Route to Tier 1 or Tier 2
        - Execute specialized task
        - Chief: Synthesize results
```

### Integration com Copilot

```
@workspace com skill squad:

Criar squad para "API Design"

Chief: API Architect (routing, coordination)

Tier 1 (Core Design):
- REST Expert (Richardson maturity, HATEOAS)
- GraphQL Expert (schema design, resolvers)
- gRPC Expert (protobuf, streaming)

Tier 2 (Specializations):
- Authentication Expert (OAuth2, JWT)
- Documentation Expert (OpenAPI, AsyncAPI)
- Performance Expert (caching, rate limiting)

Workflows:
1. API Design Review
2. Security Audit
3. Documentation Generation
```

---

## Quick Reference

| Skill | When to Use | Key Feature |
|-------|-------------|-------------|
| **architect-first** | Architecture decisions | Map → Validate → Design → Implement |
| **synapse** | Context management | 8-layer adaptive injection |
| **mcp-builder** | External service integration | Workflow-oriented tools |
| **skill-creator** | New domain knowledge | Structured skill creation |
| **squad** | Multi-expert orchestration | Tiered specialist teams |

---

*AIOS Skills - GitHub Copilot Edition v1.0*

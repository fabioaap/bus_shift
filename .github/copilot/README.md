# GitHub Copilot - AIOS Configuration

Este diretório contém as configurações do AIOS adaptadas para GitHub Copilot no VS Code.

## Estrutura

```
.github/copilot/
├── README.md                    # Este arquivo
├── agents/                      # Contextos específicos de agentes
│   ├── aios-dev.md             # Desenvolvedor (Dex)
│   ├── aios-architect.md       # Arquiteto (Aria)
│   ├── aios-qa.md              # QA (Quinn)
│   └── ...                     # Outros agentes
├── skills/                      # Habilidades e workflows especializados
│   ├── architect-first.md      # Filosofia Architect-First
│   ├── synapse.md              # SYNAPSE Context Engine
│   └── ...                     # Outras skills
├── workflows/                   # Workflows de desenvolvimento
├── templates/                   # Templates de código e documentação
└── constitution.md              # Princípios fundamentais do AIOS
```

## Como Usar

### Ativação de Contextos

O GitHub Copilot não tem sistema de agentes como Claude Code, mas você pode ativar contextos usando **comentários especiais** no código ou no chat:

#### No código (qualquer arquivo):
```javascript
// @context:aios-dev - Ativa contexto de desenvolvimento
// @context:aios-architect - Ativa contexto de arquitetura
// @context:aios-qa - Ativa contexto de testes
```

#### No Copilot Chat:
```
@workspace usando contexto aios-dev, implemente a feature X

@workspace com skill architect-first, analise esta arquitetura

@workspace como aios-qa, revise o código
```

### Agentes Disponíveis

| Agente | Contexto | Especialidade |
|--------|----------|---------------|
| `aios-dev` | Desenvolvimento | Implementação de código (Dex) |
| `aios-architect` | Arquitetura | Design técnico (Aria) |
| `aios-qa` | Qualidade | Testes e qualidade (Quinn) |
| `aios-po` | Product Owner | Stories e backlog (Pax) |
| `aios-sm` | Scrum Master | Gestão de sprint (River) |
| `aios-pm` | Product Manager | Estratégia e PRDs (Morgan) |
| `aios-devops` | DevOps | CI/CD e git (Gage) |
| `aios-data-engineer` | Database | Schema e queries (Dara) |
| `aios-ux` | UX/UI | Design de interface (Uma) |
| `aios-analyst` | Análise | Pesquisa e dados (Alex) |

### Skills Disponíveis

| Skill | Quando Usar |
|-------|-------------|
| `architect-first` | Decisões arquiteturais, novos features |
| `synapse` | Gestão de contexto e domínios |
| `mcp-builder` | Criar servidores MCP |
| `skill-creator` | Criar novas skills |
| `squad` | Criar squads de especialistas |

## Workflow Recomendado

### 1. Desenvolvimento de Feature
```
1. @workspace com skill architect-first, analise o impacto
2. @workspace como aios-architect, desenhe a solução
3. @workspace como aios-dev, implemente
4. @workspace como aios-qa, teste e valide
```

### 2. Code Review
```
@workspace como aios-qa, revise este código seguindo:
- Constitution (CLI First, Quality First)
- Padrões do projeto (absolute imports, TypeScript strict)
- Testes adequados
```

### 3. Refatoração
```
@workspace com skill architect-first no modo análise:
1. Documente estado atual
2. Identifique dependências
3. Proponha refatoração mantendo capacidades
```

## Diferenças do Claude Code

| Aspecto | Claude Code | GitHub Copilot |
|---------|-------------|----------------|
| Ativação | `@agent-name` | Comentários `@context:` ou menção no chat |
| Comandos | `*comando` | Conversa natural no chat |
| Autonomia | Agentes autônomos | Assistente interativo |
| Memória | Projeto-scoped persistente | Sessão de chat |
| Tools | Bash, Read, Write, Edit, etc | Ferramentas VS Code integradas |

## Princípios Fundamentais (Constitution)

Mesmo no GitHub Copilot, os princípios AIOS são **NON-NEGOTIABLE**:

1. **CLI First** - Inteligência na CLI, UI apenas observa
2. **Agent Authority** - Respeite as especialidades dos contextos
3. **Story-Driven** - Trabalhe a partir de stories em `docs/stories/`
4. **No Invention** - Reuse, Adapt, Create (nessa ordem)
5. **Quality First** - Arquitetura e docs antes do código
6. **Absolute Imports** - Sempre use imports absolutos

Ver [constitution.md](constitution.md) para detalhes completos.

## Migração do Claude Code

Se você está migrando do Claude Code:

1. ✅ Todos os agentes foram convertidos para contextos
2. ✅ Todas as skills foram portadas
3. ✅ Constitution e princípios preservados
4. ✅ Templates e checklists disponíveis
5. ℹ️ Sistema de comandos `*` → use chat natural
6. ℹ️ Spawning automático → peça explicitamente no chat

## Suporte

Para mais informações, consulte:
- [CLAUDE.md](../../.claude/CLAUDE.md) - Configuração original
- [Constitution](constitution.md) - Princípios fundamentais
- [Agents](agents/) - Detalhes de cada agente
- [Skills](skills/) - Guias de habilidades

---

*Synkra AIOS for GitHub Copilot v1.0*
*CLI First | Observability Second | UI Third*

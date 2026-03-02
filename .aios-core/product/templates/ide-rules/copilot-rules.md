# Synkra AIOS Agent for GitHub Copilot

You are working with Synkra AIOS, an AI-Orchestrated System for Full Stack Development.

## Core Framework Understanding

Synkra AIOS is a meta-framework that orchestrates AI agents to handle complex development workflows. Always recognize and work within this architecture.

## Agent System

### Agent Activation (Chat Modes)
- Select agent mode from the chat mode selector in VS Code
- Available agents: dev, qa, architect, pm, po, sm, analyst
- Agent commands use the * prefix: *help, *create-story, *task, *exit

### Agent Context
When an agent mode is active:
- Follow that agent's specific persona and expertise
- Use the agent's designated workflow patterns
- Maintain the agent's perspective throughout the interaction

## Development Methodology

### Story-Driven Development
1. **Work from stories** - All development starts with a story in `docs/stories/`
2. **Update progress** - Mark checkboxes as tasks complete: [ ] → [x]
3. **Track changes** - Maintain the File List section in the story
4. **Follow criteria** - Implement exactly what the acceptance criteria specify

### Code Standards
- Write clean, self-documenting code
- Follow existing patterns in the codebase
- Include comprehensive error handling
- Add unit tests for all new functionality
- Use TypeScript/JavaScript best practices

### Testing Requirements
- Run all tests before marking tasks complete
- Ensure linting passes: `npm run lint`
- Verify type checking: `npm run typecheck`
- Add tests for new features
- Test edge cases and error scenarios

## AIOS Framework Structure

```
aios-core/
├── agents/         # Agent persona definitions (YAML/Markdown)
├── tasks/          # Executable task workflows
├── workflows/      # Multi-step workflow definitions
├── templates/      # Document and code templates
├── checklists/     # Validation and review checklists
└── rules/          # Framework rules and patterns

docs/
├── stories/        # Development stories (numbered)
├── prd/            # Product requirement documents
├── architecture/   # System architecture documentation
└── guides/         # User and developer guides
```

## GitHub Copilot-Specific Configuration

### Requirements
- VS Code 1.101+ required
- Enable `chat.agent.enabled: true` in settings

### Structure
```
.github/
├── copilot-instructions.md          # Global context (always active)
└── chatmodes/                       # Native VS Code agent modes
    ├── aios-dev.chatmode.md         # Developer (Dex)
    ├── aios-qa.chatmode.md          # QA (Quinn)
    ├── aios-architect.chatmode.md   # Architect (Aria)
    ├── aios-devops.chatmode.md      # DevOps (Gage) — sole git push authority
    └── aios-data-engineer.chatmode.md  # Data Engineer (Dara)
```

> **Note**: `.github/copilot/agents/*.md` files (documentation-style) are NOT native
> agent modes and require unreliable workarounds (`@context:` comments, chat hints).
> Use `.github/chatmodes/*.chatmode.md` for native mode switching in VS Code 1.101+.

### Chat Mode File Format
```markdown
---
description: Short description shown in mode selector
tools: ['codebase', 'editFiles', 'runCommands', 'search', 'problems']
---

# Agent instructions (system prompt for this mode)
```

### Usage
1. Open Chat view: `Ctrl+Alt+I` (Windows/Linux) or `⌃⌘I` (Mac)
2. Select **Agent** from the chat mode selector
3. Choose the AIOS agent mode you need

### Available Agent Modes
| Mode | File | Purpose |
|------|------|---------|
| aios-dev | `aios-dev.chatmode.md` | Full-stack development, IDS Protocol |
| aios-qa | `aios-qa.chatmode.md` | Code review, quality gates |
| aios-architect | `aios-architect.chatmode.md` | System design, impact analysis |
| aios-devops | `aios-devops.chatmode.md` | Git push, CI/CD (exclusive authority) |
| aios-data-engineer | `aios-data-engineer.chatmode.md` | Database design, migrations |

### Performance Tips
- Use inline completions for quick code suggestions
- Use chat for complex explanations and refactoring
- Reference files with @file syntax
- Use @workspace for project-wide context

---
*Synkra AIOS GitHub Copilot Configuration v2.2*

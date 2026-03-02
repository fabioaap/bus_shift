# GitHub Projects - Setup Guide

Guia para configurar o GitHub Projects para gerenciar o backlog do Bus Shift.

---

## 🎯 Opções de Gerenciamento

### Opção 1: GitHub Projects (Beta) - Recomendado

GitHub Projects (nova interface) oferece:
- Views customizáveis (Board, Table, Roadmap)
- Automação nativa
- Integração com Issues e PRs
- Planning tools (iteration, sprint)

### Opção 2: GitHub Projects (Classic)

Versão anterior, mais simples:
- Kanban board tradicional
- Cards manuais
- Menos automação

---

## 📋 Setup do GitHub Projects (Beta)

### 1. Criar Projeto

**Via Interface Web:**
1. Vá para `https://github.com/orgs/SUA_ORG/projects` ou seu perfil
2. Click "New project"
3. Nome: **"Bus Shift - Development"**
4. Descrição: *"Product backlog e tracking do jogo Bus Shift"*
5. Template: Escolha **"Team backlog"** ou **"Blank project"**

**Via GitHub CLI:**
```bash
# Criar projeto
gh project create \\
  --title "Bus Shift - Development" \\
  --owner "seu-usuario-ou-org"

# Listar projetos
gh project list --owner "seu-usuario-ou-org"
```

---

### 2. Configurar Views (Visualizações)

#### View 1: Backlog (Table)
Visão geral de todas as issues.

**Colunas sugeridas:**
- Title
- Status
- Epic
- Priority
- Story Points
- Assignees
- Milestone

**Filtros:**
- Ordenar por: Priority (High → Low)
- Group by: Epic

#### View 2: Sprint Board (Board)
Kanban para o sprint atual.

**Colunas:**
- 📋 Backlog
- 🏗️ To Do
- 🔨 In Progress
- 👀 In Review
- ✅ Done

**Automação:**
- Auto-move para "Done" quando issue fecha
- Auto-move para "In Progress" quando branch criado

#### View 3: Roadmap (Roadmap)
Timeline visual dos milestones.

**Configuração:**
- Date field: Milestone due date
- Group by: Milestone
- Color by: Epic

---

### 3. Criar Custom Fields

Adicionar campos customizados ao projeto:

**1. Epic** (Single select)
- 📖 Documentação
- 🎮 Mecânicas Core
- 👻 Assombrações
- 📈 Progressão
- 🎨 Arte
- 🔊 Áudio
- 📚 Narrativa
- 🧪 Testes

**2. Story Points** (Number)
- Range: 1-20

**3. Priority** (Single select) *(se não usar labels)*
- 🔴 P0 - Critical
- 🟠 P1 - High
- 🟡 P2 - Medium
- 🟢 P3 - Low

**4. Sprint** (Iteration)
- Duração: 2 semanas
- Start: 2026-03-03

---

### 4. Importar Issues

**Opção A: Criar via Script**
Use os scripts fornecidos:
- Windows: `.\\.github\\create-issues.ps1`
- Linux/Mac: `bash .github/create-issues.sh`

**Opção B: Importar Manualmente**
1. No projeto, click "Add item"
2. Selecione issues existentes ou crie novas
3. Associe ao projeto

**Opção C: Automação**
Configure workflow automático:
```yaml
# .github/workflows/auto-add-to-project.yml
name: Auto-add to project
on:
  issues:
    types: [opened]

jobs:
  add-to-project:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/add-to-project@v0.4.0
        with:
          project-url: https://github.com/users/USUARIO/projects/NUMERO
          github-token: \${{ secrets.ADD_TO_PROJECT_PAT }}
```

---

### 5. Configurar Automações

**Automações Essenciais:**

1. **Status Sync:**
   ```
   When: Issue closed
   Then: Set Status to "Done"
   ```

2. **In Progress:**
   ```
   When: Pull request linked
   Then: Set Status to "In Progress"
   ```

3. **Review:**
   ```
   When: Pull request ready for review
   Then: Set Status to "In Review"
   ```

4. **Epic Tracking:**
   ```
   When: Issue labeled "epic: X"
   Then: Set Epic field to "X"
   ```

---

## 🏃 Workflow Sugerido

### Sprint Planning

1. **Selecionar Issues:**
   - Filtrar por Priority: P0, P1
   - Ordenar por Milestone
   - Verificar Dependencies

2. **Criar Sprint:**
   - New iteration no campo "Sprint"
   - Nome: "Sprint 1 - Setup" (exemplo)
   - Duração: 2 semanas

3. **Assign Story Points:**
   - Estimar usando Fibonacci (1, 2, 3, 5, 8, 13, 20)
   - Total do sprint: 20-30 SP (ajustar por velocity real)

4. **Assign Pessoas:**
   - Distribuir issues pela equipe
   - Balancear workload

### Daily Work

1. **Mover Cards:**
   - Drag & drop no Board view
   - Atualizar status conforme progresso

2. **Atualizar Issues:**
   - Marcar checkboxes dos AC
   - Comentar progresso/blockers

3. **Code → Review → Merge:**
   - Criar PR linkado à issue (#X)
   - Review move para "In Review"
   - Merge fecha issue → "Done"

### Sprint Review

1. **Verificar "Done":**
   - Todos ACs completos?
   - QA passou?

2. **Medir Velocity:**
   - Somar Story Points concluídos
   - Ajustar próximo sprint

3. **Retrospectiva:**
   - O que funcionou?
   - O que melhorar?

---

## 📊 Métricas e Insights

### GitHub Insights (nativo)

**Acessar:** Projeto → Insights

**Métricas Disponíveis:**
- Burndown chart (story points)
- Velocity trend
- Issues por Epic
- Item age (tempo em cada status)

### Custom Dashboard (opcional)

Criar view "Metrics" com:
- Total Story Points (sum)
- Completed (count)
- In Progress (count)
- Blocked (count)

---

## 🔗 Comandos Úteis (GitHub CLI)

```bash
# Ver projeto
gh project view NUMERO --owner USUARIO

# Listar issues do projeto
gh project item-list NUMERO --owner USUARIO

# Adicionar issue ao projeto
gh project item-add NUMERO --owner USUARIO --url https://github.com/USUARIO/REPO/issues/ISSUE_NUMBER

# Fechar issue
gh issue close NUMERO

# Ver issues do milestone
gh issue list --milestone "M1: Documentação Completa"

# Criar PR linkado a issue
gh pr create --title "Fix #42" --body "Resolve #42"
```

---

## 🎨 Labels Recomendadas

**Por Tipo:**
- `documentation` - Documentação
- `feature` - Nova feature
- `bug` - Bug fix
- `art` - Assets artísticos
- `audio` - Áudio/música
- `ui-ux` - Interface
- `qa` - Testes
- `balancing` - Balanceamento

**Por Prioridade:**
- `priority: critical` (P0) - Vermelho
- `priority: high` (P1) - Laranja
- `priority: medium` (P2) - Amarelo
- `priority: low` (P3) - Verde

**Por Epic:**
- `epic: documentation`
- `epic: core-mechanics`
- `epic: ghosts`
- `epic: progression`
- `epic: art`
- `epic: audio`
- `epic: narrative`
- `epic: testing`

**Status:**
- `blocked` - Bloqueado por dependência
- `needs-discussion` - Requer discussão
- `good first issue` - Bom para iniciantes

---

## 🚀 Quick Start

**Passo a Passo Rápido:**

```bash
# 1. Autenticar
gh auth login

# 2. Navegar até o repo
cd bus_shift

# 3. Editar script com nome do seu repo
code .github/create-issues.ps1  # ou .sh

# 4. Executar script
./.github/create-issues.ps1     # Windows
# bash .github/create-issues.sh # Linux/Mac

# 5. Criar projeto via web
# Vá para github.com/usuario/bus_shift/projects

# 6. Adicionar issues ao projeto
# Use interface "Add item" ou automação
```

---

## 📚 Recursos

- [GitHub Projects Docs](https://docs.github.com/en/issues/planning-and-tracking-with-projects)
- [GitHub CLI Projects](https://cli.github.com/manual/gh_project)
- [Project Automation](https://docs.github.com/en/issues/planning-and-tracking-with-projects/automating-your-project)
- [Scrum with GitHub](https://github.com/trailheadapps/github-agile)

---

## ✅ Checklist de Setup

- [ ] GitHub CLI instalado e autenticado
- [ ] Labels criadas
- [ ] Milestones criados
- [ ] Projeto criado (Projects Beta)
- [ ] Views configuradas (Backlog, Board, Roadmap)
- [ ] Custom fields adicionados (Epic, Story Points, Sprint)
- [ ] Issues criadas via script
- [ ] Issues adicionadas ao projeto
- [ ] Automações configuradas
- [ ] Equipe tem acesso ao projeto

---

**Última atualização:** 2026-03-01
**Mantenedor:** [Nome do PM]

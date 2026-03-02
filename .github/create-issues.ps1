# Bus Shift - GitHub Issues Creation Script (PowerShell)
# Este script cria todas as issues do backlog usando GitHub CLI
# 
# Pré-requisitos:
#   1. GitHub CLI instalado: https://cli.github.com/
#   2. Autenticado: gh auth login
#   3. Execute no diretório do repositório
#
# Uso: .\\.github\\create-issues.ps1

param(
    [string]$Repo = "seu-usuario/bus_shift"  # AJUSTAR para seu repo
)

# Configurações
$LABEL_EPIC = "epic"
$LABEL_CRITICAL = "priority: critical"
$LABEL_HIGH = "priority: high"
$LABEL_MEDIUM = "priority: medium"
$LABEL_LOW = "priority: low"

Write-Host "`n=== Bus Shift - Criação de Issues ===`n" -ForegroundColor Green

# Verificar se gh CLI está instalado
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Host "Erro: GitHub CLI não encontrado. Instale em https://cli.github.com/" -ForegroundColor Red
    exit 1
}

# Verificar autenticação
$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Erro: Não autenticado. Execute: gh auth login" -ForegroundColor Red
    exit 1
}

Write-Host "Criando labels...`n" -ForegroundColor Yellow

# Criar labels customizadas
gh label create "$LABEL_EPIC" --description "Epic - Major feature" --color "7057ff" --force
gh label create "$LABEL_CRITICAL" --description "P0 - Critical priority" --color "d73a4a" --force
gh label create "$LABEL_HIGH" --description "P1 - High priority" --color "ff9800" --force
gh label create "$LABEL_MEDIUM" --description "P2 - Medium priority" --color "fbca04" --force
gh label create "$LABEL_LOW" --description "P3 - Low priority" --color "0e8a16" --force

gh label create "documentation" --description "Documentation only" --color "0075ca" --force
gh label create "feature" --description "New feature" --color "a2eeef" --force
gh label create "art" --description "Art assets" --color "f9d0c4" --force
gh label create "audio" --description "Audio/Music" --color "c5def5" --force
gh label create "ui-ux" --description "UI/UX design" --color "bfd4f2" --force
gh label create "qa" --description "Quality Assurance" --color "d4c5f9" --force
gh label create "balancing" --description "Game balancing" --color "c2e0c6" --force

Write-Host "Labels criadas!`n" -ForegroundColor Green

# Função para criar issue
function Create-Issue {
    param(
        [string]$Title,
        [string]$Body,
        [string]$Labels,
        [string]$Milestone = ""
    )
    
    Write-Host "Criando: $Title" -ForegroundColor Yellow
    
    if ($Milestone) {
        gh issue create --title $Title --body $Body --label $Labels --milestone $Milestone
    } else {
        gh issue create --title $Title --body $Body --label $Labels
    }
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✓ Issue criada`n" -ForegroundColor Green
    } else {
        Write-Host "✗ Falha ao criar issue`n" -ForegroundColor Red
    }
}

Write-Host "Criando milestones...`n" -ForegroundColor Yellow

# Criar Milestones
gh api "repos/$Repo/milestones" -X POST -f title="M1: Documentação Completa" -f description="Sprint 1-2" -f due_on="2026-03-31T00:00:00Z"
gh api "repos/$Repo/milestones" -X POST -f title="M2: Vertical Slice - Dia 1" -f description="Sprint 3-8" -f due_on="2026-05-31T00:00:00Z"
gh api "repos/$Repo/milestones" -X POST -f title="M3: MVP - 5 Dias Completos" -f description="Sprint 9-16" -f due_on="2026-08-31T00:00:00Z"
gh api "repos/$Repo/milestones" -X POST -f title="M4: Content Complete" -f description="Sprint 17-22" -f due_on="2026-10-31T00:00:00Z"
gh api "repos/$Repo/milestones" -X POST -f title="M5: Gold Master" -f description="Sprint 23-28" -f due_on="2026-12-31T00:00:00Z"

Write-Host "Milestones criadas!`n" -ForegroundColor Green

Write-Host "Criando issues do Epic 1: Documentação...`n" -ForegroundColor Yellow

# EPIC 1 - DOCUMENTAÇÃO

$issue1Body = @"
**Tipo:** Documentation
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 2 SP
**Epic:** Documentação e Design

## Descrição
Definir nomes oficiais para:
- Escola (substituir [NOME DA ESCOLA])
- 3 Crianças assombradas
- Diretor (completar nome de Siqueira)
- Motorista anterior
- Cidade/localização

## Critérios de Aceite
- [ ] Nome da escola definido e atualizado em toda documentação
- [ ] 3 crianças têm nomes e backstories básicas
- [ ] NPCs nomeados em characters.md

## Arquivo Alvo
``docs/game/narrative/characters.md``

## Estimativa
2 Story Points
"@

Create-Issue `
    -Title "#1 - Definir Nomes e Identidades" `
    -Body $issue1Body `
    -Labels "documentation,$LABEL_CRITICAL,epic: documentation" `
    -Milestone "M1: Documentação Completa"

Write-Host "`n=== Script de criação de issues (exemplo) ===" -ForegroundColor Green
Write-Host "Issues iniciais criadas. Continue adaptando para os demais épicos.`n" -ForegroundColor Yellow
Write-Host "Para ver issues: gh issue list" -ForegroundColor Yellow
Write-Host "Para criar projeto: gh project create" -ForegroundColor Yellow

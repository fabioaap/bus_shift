#!/bin/bash

# Bus Shift - GitHub Issues Creation Script
# Este script cria todas as issues do backlog usando GitHub CLI
# 
# Pré-requisitos:
#   1. GitHub CLI instalado: https://cli.github.com/
#   2. Autenticado: gh auth login
#   3. Navegue até o diretório do repositório
#
# Uso: bash .github/create-issues.sh

# Configurações
REPO="seu-usuario/bus_shift"  # AJUSTAR para seu repo
LABEL_EPIC="epic"
LABEL_CRITICAL="priority: critical"
LABEL_HIGH="priority: high"
LABEL_MEDIUM="priority: medium"
LABEL_LOW="priority: low"

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}=== Bus Shift - Criação de Issues ===${NC}\n"

# Verificar se gh CLI está instalado
if ! command -v gh &> /dev/null; then
    echo -e "${RED}Erro: GitHub CLI não encontrado. Instale em https://cli.github.com/${NC}"
    exit 1
fi

# Verificar autenticação
if ! gh auth status &> /dev/null; then
    echo -e "${RED}Erro: Não autenticado. Execute: gh auth login${NC}"
    exit 1
fi

echo -e "${YELLOW}Criando labels...${NC}\n"

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

echo -e "${GREEN}Labels criadas!${NC}\n"

# Função para criar issue
create_issue() {
    local title="$1"
    local body="$2"
    local labels="$3"
    local milestone="$4"
    
    echo -e "${YELLOW}Criando: $title${NC}"
    
    if [ -n "$milestone" ]; then
        gh issue create \
            --title "$title" \
            --body "$body" \
            --label "$labels" \
            --milestone "$milestone"
    else
        gh issue create \
            --title "$title" \
            --body "$body" \
            --label "$labels"
    fi
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ Issue criada${NC}\n"
    else
        echo -e "${RED}✗ Falha ao criar issue${NC}\n"
    fi
}

echo -e "${YELLOW}Criando milestones...${NC}\n"

# Criar Milestones
gh api repos/$REPO/milestones -X POST -f title="M1: Documentação Completa" -f description="Sprint 1-2" -f due_on="2026-03-31T00:00:00Z"
gh api repos/$REPO/milestones -X POST -f title="M2: Vertical Slice - Dia 1" -f description="Sprint 3-8" -f due_on="2026-05-31T00:00:00Z"
gh api repos/$REPO/milestones -X POST -f title="M3: MVP - 5 Dias Completos" -f description="Sprint 9-16" -f due_on="2026-08-31T00:00:00Z"
gh api repos/$REPO/milestones -X POST -f title="M4: Content Complete" -f description="Sprint 17-22" -f due_on="2026-10-31T00:00:00Z"
gh api repos/$REPO/milestones -X POST -f title="M5: Gold Master" -f description="Sprint 23-28" -f due_on="2026-12-31T00:00:00Z"

echo -e "${GREEN}Milestones criadas!${NC}\n"

echo -e "${YELLOW}Criando issues do Epic 1: Documentação...${NC}\n"

# EPIC 1 - DOCUMENTAÇÃO

create_issue \
    "#1 - Definir Nomes e Identidades" \
    "**Tipo:** Documentation
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
\`docs/game/narrative/characters.md\`

## Estimativa
2 Story Points" \
    "documentation,$LABEL_CRITICAL,epic: documentation" \
    "M1: Documentação Completa"

create_issue \
    "#2 - Completar Narrativa (Dias 2-5)" \
    "**Tipo:** Documentation
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 8 SP
**Epic:** Documentação e Design

## Descrição
Desenvolver narrativa completa dos dias 2 a 5:
- Dia 2: Revelações fragmentadas
- Dia 3: A verdade enterrada
- Dia 4: Escalada
- Dia 5: Última parada

## Critérios de Aceite
- [ ] Cada dia tem narrativa manhã + tarde
- [ ] Progressão lógica de revelações
- [ ] Eventos chave definidos por dia
- [ ] Diálogos essenciais escritos

## Arquivo Alvo
\`docs/game/narrative/story.md\`

## Estimativa
8 Story Points" \
    "documentation,$LABEL_CRITICAL,epic: documentation" \
    "M1: Documentação Completa"

create_issue \
    "#3 - Definir Backstory do Acidente Original" \
    "**Tipo:** Documentation
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 3 SP
**Epic:** Documentação e Design

## Descrição
Criar lore completo sobre o acidente:
- Ano do acidente
- Causa (mecânica/humana/sobrenatural)
- Vítimas (número, identidades)
- Motorista original (culpado? morto?)
- Por que apenas 3 crianças assombram?

## Critérios de Aceite
- [ ] Timeline definida
- [ ] Causa clara e consistente
- [ ] Motivações das assombrações justificadas
- [ ] Documentado em story.md

## Arquivo Alvo
\`docs/game/narrative/story.md\` (seção Lore Fragments)

## Estimativa
3 Story Points" \
    "documentation,$LABEL_CRITICAL,epic: documentation" \
    "M1: Documentação Completa"

create_issue \
    "#4 - Criar Style Guide Visual (Low Poly)" \
    "**Tipo:** Documentation + Art
**Prioridade:** P1 - ALTA
**Estimativa:** 5 SP
**Epic:** Documentação e Design

## Descrição
Definir estilo visual Low Poly consistente:
- Paleta de cores (Dia vs Noite)
- Contagem de polígonos (ranges)
- Referências visuais
- Iluminação e sombras
- Tratamento de texturas

## Critérios de Aceite
- [ ] Documento style-guide.md completo
- [ ] Moodboard com 10+ referências
- [ ] Paleta definida (hex codes)
- [ ] Guidelines de poly count

## Arquivo Alvo
\`docs/game/art/style-guide.md\`

## Nota
⚠️ **BLOQUEIA** toda a arte do jogo

## Estimativa
5 Story Points" \
    "documentation,art,$LABEL_HIGH,epic: documentation" \
    "M1: Documentação Completa"

create_issue \
    "#5 - Especificação Técnica Completa" \
    "**Tipo:** Documentation
**Prioridade:** P1 - ALTA
**Estimativa:** 5 SP
**Epic:** Documentação e Design

## Descrição
Criar especificação técnica detalhada:
- Engine escolhida (Unity/Unreal/Godot)
- Arquitetura de código (padrões)
- Sistema de save/load
- Performance targets (FPS, memória)
- Plataformas alvo (PC/Console/Mobile)

## Critérios de Aceite
- [ ] Engine definida com justificativa
- [ ] Architecture.md completo
- [ ] Performance targets documentados
- [ ] Tech stack definido

## Arquivo Alvo
\`docs/game/technical/architecture.md\`

## Estimativa
5 Story Points" \
    "documentation,$LABEL_HIGH,epic: documentation" \
    "M1: Documentação Completa"

echo -e "${GREEN}=== Epic 1 (Documentação) criado ===${NC}\n"
echo -e "${YELLOW}Para criar os demais épicos, continue executando blocos similares...${NC}\n"
echo -e "${YELLOW}Total de 52 issues documentadas em BACKLOG.md${NC}\n"

echo -e "${GREEN}Issues iniciais criadas com sucesso!${NC}"
echo -e "${YELLOW}Para ver as issues: gh issue list${NC}"
echo -e "${YELLOW}Para criar um projeto: gh project create${NC}"

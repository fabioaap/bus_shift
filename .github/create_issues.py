"""
Bus Shift - GitHub Issues Creator
Cria todas as 52 issues do backlog usando GitHub REST API

Requisitos:
    pip install requests

Uso:
    python .github/create_issues.py --token SEU_TOKEN --repo usuario/bus_shift
"""

import requests
import json
import time
import argparse
from typing import Dict, List

# Configuração
API_BASE = "https://api.github.com"

def create_labels(repo: str, token: str):
    """Cria labels customizadas necessárias"""
    print("📌 Criando labels...")
    
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    labels = [
        # Por Prioridade
        {"name": "priority: critical", "description": "P0 - Critical priority", "color": "d73a4a"},
        {"name": "priority: high", "description": "P1 - High priority", "color": "ff9800"},
        {"name": "priority: medium", "description": "P2 - Medium priority", "color": "fbca04"},
        {"name": "priority: low", "description": "P3 - Low priority", "color": "0e8a16"},
        
        # Por Tipo
        {"name": "documentation", "description": "Documentation only", "color": "0075ca"},
        {"name": "feature", "description": "New feature", "color": "a2eeef"},
        {"name": "art", "description": "Art assets", "color": "f9d0c4"},
        {"name": "audio", "description": "Audio/Music", "color": "c5def5"},
        {"name": "ui-ux", "description": "UI/UX design", "color": "bfd4f2"},
        {"name": "qa", "description": "Quality Assurance", "color": "d4c5f9"},
        {"name": "balancing", "description": "Game balancing", "color": "c2e0c6"},
        {"name": "animation", "description": "Animations", "color": "fef2c0"},
        {"name": "technical", "description": "Technical setup", "color": "bfdadc"},
        
        # Por Epic
        {"name": "epic: documentation", "description": "Epic 1 - Documentação", "color": "7057ff"},
        {"name": "epic: core-mechanics", "description": "Epic 2 - Mecânicas Core", "color": "0052cc"},
        {"name": "epic: ghosts", "description": "Epic 3 - Assombrações", "color": "5319e7"},
        {"name": "epic: progression", "description": "Epic 4 - Progressão", "color": "0e8a16"},
        {"name": "epic: art", "description": "Epic 5 - Arte", "color": "e99695"},
        {"name": "epic: audio", "description": "Epic 6 - Áudio", "color": "c5def5"},
        {"name": "epic: narrative", "description": "Epic 7 - Narrativa", "color": "d876e3"},
        {"name": "epic: testing", "description": "Epic 8 - Testes", "color": "fbca04"},
    ]
    
    for label in labels:
        try:
            r = requests.post(
                f"{API_BASE}/repos/{repo}/labels",
                headers=headers,
                json=label
            )
            if r.status_code == 201:
                print(f"  ✓ Label '{label['name']}' criada")
            elif r.status_code == 422:  # Já existe
                print(f"  ⊙ Label '{label['name']}' já existe")
            else:
                print(f"  ✗ Erro ao criar '{label['name']}': {r.status_code}")
        except Exception as e:
            print(f"  ✗ Erro: {e}")
        
        time.sleep(0.5)  # Rate limiting

def create_milestones(repo: str, token: str):
    """Cria milestones do projeto"""
    print("\n🎯 Criando milestones...")
    
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    milestones = [
        {
            "title": "M1: Documentação Completa",
            "description": "Sprint 1-2 - Completar toda documentação de design",
            "due_on": "2026-03-31T00:00:00Z"
        },
        {
            "title": "M2: Vertical Slice - Dia 1",
            "description": "Sprint 3-8 - Demo playável do Dia 1",
            "due_on": "2026-05-31T00:00:00Z"
        },
        {
            "title": "M3: MVP - 5 Dias Completos",
            "description": "Sprint 9-16 - Alpha testável com 5 dias",
            "due_on": "2026-08-31T00:00:00Z"
        },
        {
            "title": "M4: Content Complete",
            "description": "Sprint 17-22 - Beta build com todo conteúdo",
            "due_on": "2026-10-31T00:00:00Z"
        },
        {
            "title": "M5: Gold Master",
            "description": "Sprint 23-28 - Release build final",
            "due_on": "2026-12-31T00:00:00Z"
        }
    ]
    
    for milestone in milestones:
        try:
            r = requests.post(
                f"{API_BASE}/repos/{repo}/milestones",
                headers=headers,
                json=milestone
            )
            if r.status_code == 201:
                print(f"  ✓ Milestone '{milestone['title']}' criado")
            else:
                print(f"  ✗ Erro: {r.status_code} - {r.json().get('message', '')}")
        except Exception as e:
            print(f"  ✗ Erro: {e}")
        
        time.sleep(0.5)

def get_milestone_number(repo: str, token: str, title: str) -> int:
    """Obtém número do milestone pelo título"""
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    r = requests.get(f"{API_BASE}/repos/{repo}/milestones", headers=headers)
    if r.status_code == 200:
        for m in r.json():
            if m['title'] == title:
                return m['number']
    return None

def create_issue(repo: str, token: str, issue_data: Dict):
    """Cria uma issue individual"""
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    # Obter milestone number se especificado
    milestone_number = None
    if 'milestone' in issue_data:
        milestone_number = get_milestone_number(repo, token, issue_data['milestone'])
        if milestone_number:
            issue_data['milestone'] = milestone_number
        else:
            del issue_data['milestone']
    
    try:
        r = requests.post(
            f"{API_BASE}/repos/{repo}/issues",
            headers=headers,
            json=issue_data
        )
        if r.status_code == 201:
            issue_num = r.json()['number']
            print(f"  ✓ Issue #{issue_num}: {issue_data['title']}")
            return True
        else:
            print(f"  ✗ Erro: {r.status_code} - {r.json().get('message', '')}")
            return False
    except Exception as e:
        print(f"  ✗ Erro: {e}")
        return False

def get_issues_epic1():
    """Retorna lista de issues do Epic 1"""
    return [
        {
            "title": "#1 - Definir Nomes e Identidades",
            "body": """**Tipo:** Documentation
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
`docs/game/narrative/characters.md`

## Estimativa
2 Story Points""",
            "labels": ["documentation", "priority: critical", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#2 - Completar Narrativa (Dias 2-5)",
            "body": """**Tipo:** Documentation
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
`docs/game/narrative/story.md`

## Estimativa
8 Story Points""",
            "labels": ["documentation", "priority: critical", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#3 - Definir Backstory do Acidente Original",
            "body": """**Tipo:** Documentation
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
`docs/game/narrative/story.md` (seção Lore Fragments)

## Estimativa
3 Story Points""",
            "labels": ["documentation", "priority: critical", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#4 - Criar Style Guide Visual (Low Poly)",
            "body": """**Tipo:** Documentation + Art
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
`docs/game/art/style-guide.md`

## ⚠️ Nota
**BLOQUEIA** toda a arte do jogo

## Estimativa
5 Story Points""",
            "labels": ["documentation", "art", "priority: high", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#5 - Especificação Técnica Completa",
            "body": """**Tipo:** Documentation
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
`docs/game/technical/architecture.md`

## Estimativa
5 Story Points""",
            "labels": ["documentation", "technical", "priority: high", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#6 - Criar Asset List Completa",
            "body": """**Tipo:** Documentation
**Prioridade:** P2 - MÉDIA
**Estimativa:** 3 SP
**Epic:** Documentação e Design

## Descrição
Inventariar todos os assets necessários:
- Modelos 3D (ônibus, personagens, ambiente)
- Texturas
- Animações
- UI elements
- Partículas

## Critérios de Aceite
- [ ] Lista completa em assets.md
- [ ] Priorização (MVP vs Polish)
- [ ] Estimativas de tempo de criação

## Arquivo Alvo
`docs/game/art/assets.md`

## Estimativa
3 Story Points""",
            "labels": ["documentation", "art", "priority: medium", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        },
        {
            "title": "#7 - Plano de Testes Detalhado",
            "body": """**Tipo:** Documentation
**Prioridade:** P2 - MÉDIA
**Estimativa:** 3 SP
**Epic:** Documentação e Design

## Descrição
Criar plano de testes completo:
- Test cases por mecânica
- Matriz de compatibilidade
- Procedimentos de QA
- Ferramentas de teste

## Critérios de Aceite
- [ ] test-plan.md completo
- [ ] Test cases para cada mecânica
- [ ] Critérios de aceitação definidos

## Arquivo Alvo
`docs/game/testing/test-plan.md`

## Estimativa
3 Story Points""",
            "labels": ["documentation", "qa", "priority: medium", "epic: documentation"],
            "milestone": "M1: Documentação Completa"
        }
    ]

def main():
    parser = argparse.ArgumentParser(description='Cria issues do Bus Shift no GitHub')
    parser.add_argument('--token', required=True, help='GitHub Personal Access Token')
    parser.add_argument('--repo', required=True, help='Repositório (usuario/repo)')
    parser.add_argument('--skip-labels', action='store_true', help='Pular criação de labels')
    parser.add_argument('--skip-milestones', action='store_true', help='Pular criação de milestones')
    
    args = parser.parse_args()
    
    print("\n" + "="*60)
    print("🎮 Bus Shift - Criação de Issues no GitHub")
    print("="*60 + "\n")
    
    # Criar labels
    if not args.skip_labels:
        create_labels(args.repo, args.token)
    
    # Criar milestones
    if not args.skip_milestones:
        create_milestones(args.repo, args.token)
    
    # Criar issues do Epic 1
    print("\n📋 Criando issues do Epic 1: Documentação...")
    issues = get_issues_epic1()
    
    success_count = 0
    for issue in issues:
        if create_issue(args.repo, args.token, issue):
            success_count += 1
        time.sleep(1)  # Rate limiting
    
    print(f"\n{'='*60}")
    print(f"✅ {success_count}/{len(issues)} issues criadas com sucesso!")
    print(f"{'='*60}\n")
    
    print("📌 Próximos passos:")
    print("  1. Acesse: https://github.com/{}/issues".format(args.repo))
    print("  2. Adicione issues ao projeto GitHub Projects")
    print("  3. Continue criando issues dos demais épicos (2-8)")

if __name__ == "__main__":
    main()

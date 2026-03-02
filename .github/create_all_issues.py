# -*- coding: utf-8 -*-
"""
Bus Shift - GitHub Issues Creator (COMPLETO - 52 issues)
Cria todas as issues do backlog usando GitHub REST API

Requisitos:
    pip install requests

Uso:
    python .github/create_all_issues.py --token SEU_TOKEN --repo fabioaap/bus_shift
"""

import sys
import io
import requests
import json
import time
import argparse
from typing import Dict, List

# Fix encoding para Windows
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8', errors='replace')

# Configuração
API_BASE = "https://api.github.com"

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
            print(f"  OK Issue #{issue_num}: {issue_data['title']}")
            return True
        else:
            print(f"  ERRO: {r.status_code} - {r.json().get('message', '')}")
            return False
    except Exception as e:
        print(f"  ERRO: {e}")
        return False

def get_all_issues():
    """Retorna TODAS as 52 issues do backlog"""
    return [
        # EPIC 2: MECÂNICAS CORE (9 issues)
        {
            "title": "#9 - Implementar Controles de Direção",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Mecânicas Core

## Descrição
Implementar controles REAIS de ônibus:
- Acelerador progressivo + freio
- Volante com inércia (não arcade)
- Câmbio manual (R, N, D)
- Feedback de direção (pesado ao parar, leve em velocidade)

## Critérios de Aceite
- [ ] Controles respondem corretamente (gamepad + teclado)
- [ ] Ônibus acelera/desacelera com física realista
- [ ] Volante tem resistência simulada
- [ ] Câmbio alterável (R/N/D funcional)

## Dependências
- #8 (Setup do Projeto deve estar completo)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: critical", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#10 - Protótipo de Rota Simples",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 8 SP
**Epic:** Mecânicas Core

## Descrição
Criar rota básica com 3 pontos de ônibus:
- Ponto 1: Escola [NOME]
- Ponto 2: Bairro residencial
- Ponto 3: Centro da cidade

Rota cíclica: 1 → 2 → 3 → 1

## Critérios de Aceite
- [ ] Rota com 3 pontos marcados no mapa
- [ ] Distância aproximada de 5-7 minutos por ciclo
- [ ] Waypoints visíveis (indicadores de direção)
- [ ] Ônibus pode completar rota inteira

## Dependências
- #9 (Controles devem estar funcionais)

## Estimativa
8 Story Points""",
            "labels": ["feature", "priority: critical", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#11 - Sistema de Embarque/Desembarque",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 8 SP
**Epic:** Mecânicas Core

## Descrição
Sistema de interação em pontos:
- Parar no ponto (área de trigger)
- Abrir porta (Tecla E)
- Crianças sobem/descem (animações)
- Fechar porta (automático após 5s ou tecla E)

## Critérios de Aceite
- [ ] Ônibus detecta parada em pontos
- [ ] Porta abre/fecha com input do jogador
- [ ] NPCs entram/saem com animações
- [ ] UI indica tempo de espera

## Dependências
- #10 (Rota deve existir)

## Estimativa
8 Story Points""",
            "labels": ["feature", "priority: critical", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#12 - Sistema de Tempo e Relógio",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 5 SP
**Epic:** Mecânicas Core

## Descrição
Relógio in-game com:
- HUD mostrando hora atual
- Transição Manhã → Tarde (12:00)
- Dia avança ao completar ciclo da tarde
- Iluminação muda com período

## Critérios de Aceite
- [ ] Relógio funcional no HUD
- [ ] Transição Manhã/Tarde visível
- [ ] Sistema detecta fim do dia

## Dependências
- Nenhuma (pode ser desenvolvida em paralelo)

## Estimativa
5 Story Points""",
            "labels": ["feature", "priority: critical", "epic: core-mechanics", "ui-ux"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#13 - Implementar Mapa Interativo",
            "body": """**Tipo:** Feature + UI/UX
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Mecânicas Core

## Descrição
Mapa acessível via TAB:
- Visão top-down da rota
- Posição do ônibus em tempo real
- Pontos de parada marcados
- Waypoints até próximo ponto

## Critérios de Aceite
- [ ] Mapa abre/fecha com TAB
- [ ] Ônibus rastreado no mapa
- [ ] Pontos claramente marcados
- [ ] UI limpa e legível

## Dependências
- #10 (Rota deve existir)

## Estimativa
8 Story Points""",
            "labels": ["feature", "ui-ux", "priority: high", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#14 - Sistema de Retrovisor",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 5 SP
**Epic:** Mecânicas Core

## Descrição
Retrovisor com foco em passageiros:
- Câmera traseira no HUD
- Ativação por input (Tecla R)
- Foco na área dos assentos
- Usado para detectar assombrações

## Critérios de Aceite
- [ ] Retrovisor funcional no HUD
- [ ] Mostra interior do ônibus
- [ ] Ativável via input
- [ ] Renderiza NPCs e assombrações

## Dependências
- #11 (NPCs devem estar implementados)

## Estimativa
5 Story Points""",
            "labels": ["feature", "priority: high", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#15 - Sistema de Câmera de Segurança",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Mecânicas Core

## Descrição
Monitor de segurança funcional:
- Visão de ângulos fixos (3-4 câmeras)
- Efeito de static/ruído visual
- Ativação por input (Tecla C)
- Usado para rastrear assombrações

## Critérios de Aceite
- [ ] Monitor funcional com UI
- [ ] 3-4 ângulos de câmera
- [ ] Efeito de static implementado
- [ ] Troca de câmera suave

## Dependências
- #11 (Interior do ônibus pronto)

## Estimativa
8 Story Points""",
            "labels": ["feature", "ui-ux", "priority: high", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#16 - Sistema de Combustível",
            "body": """**Tipo:** Feature
**Prioridade:** P2 - MÉDIA
**Estimativa:** 5 SP
**Epic:** Mecânicas Core

## Descrição
Gestão de combustível:
- Medidor no HUD
- Consumo por distância
- Game Over se esvaziar durante rota
- Posto de abastecimento (entre dias)

## Critérios de Aceite
- [ ] Medidor funcional
- [ ] Consumo realista
- [ ] Game Over implementado
- [ ] Reabastecimento entre dias

## Dependências
- #9 (Controles devem existir)

## Estimativa
5 Story Points""",
            "labels": ["feature", "balancing", "priority: medium", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#17 - Implementar Sistema de Sanidade",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 10 SP
**Epic:** Mecânicas Core

## Descrição
Sistema de Sanidade (0-100%):
- Reduz ao ver assombrações
- Efeitos visuais (distorção, vinheta)
- Game Over se chegar a 0%
- Recuperação ao terminar dia

## Critérios de Aceite
- [ ] Barra de sanidade no HUD
- [ ] Redução ao interagir com assombrações
- [ ] Efeitos visuais progressivos
- [ ] Game Over implementado

## Dependências
- Nenhuma (design system)

## Estimativa
10 Story Points""",
            "labels": ["feature", "priority: high", "epic: core-mechanics"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        
        # EPIC 3: ASSOMBRAÇÕES (8 issues)
        {
            "title": "#18 - Criar Modelo 3D da Criança Fantasma #1",
            "body": """**Tipo:** Art
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 8 SP
**Epic:** Assombrações

## Descrição
Modelar primeira criança assombrada:
- Low poly (500-800 polígonos)
- Uniforme escolar genérico
- Feições neutras mas perturbadoras
- Rigging para animações

## Critérios de Aceite
- [ ] Modelo completo e rigged
- [ ] Texturas aplicadas
- [ ] Poly count dentro do limite
- [ ] Importado na engine

## Dependências
- #4 (Style Guide deve existir)
- #1 (Nome da criança deve estar definido)

## Estimativa
8 Story Points""",
            "labels": ["art", "priority: critical", "epic: ghosts"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#19 - Implementar Comportamento da Criança #1",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Assombrações

## Descrição
IA para Criança Fantasma #1:
- Aparece em assento aleatório (Dia 1)
- Olha fixo para motorista
- Desaparece se observada por 3s (retrovisor/câmera)
- Causa -15% sanidade se ignorada

## Critérios de Aceite
- [ ] Criança spawna corretamente
- [ ] IA detecta olhar do jogador
- [ ] Desaparece após observação
- [ ] Reduz sanidade se ignorada

## Dependências
- #18 (Modelo deve existir)
- #17 (Sistema de Sanidade)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: critical", "epic: ghosts"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#20 - Criar Modelo 3D da Criança Fantasma #2",
            "body": """**Tipo:** Art
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Assombrações

## Descrição
Modelar segunda criança assombrada:
- Low poly (500-800 polígonos)
- Variação visual distinta da #1
- Rigging para animações únicas

## Critérios de Aceite
- [ ] Modelo completo e rigged
- [ ] Texturas aplicadas
- [ ] Diferenciável visualmente da #1
- [ ] Importado na engine

## Dependências
- #18 (Referência do estilo)

## Estimativa
8 Story Points""",
            "labels": ["art", "priority: high", "epic: ghosts"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#21 - Implementar Comportamento da Criança #2",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Assombrações

## Descrição
IA para Criança Fantasma #2:
- Aparece em pé no corredor (Dia 2+)
- Se aproxima lentamente do motorista
- Para se observada
- Causa -20% sanidade ao tocar motorista

## Critérios de Aceite
- [ ] IA de movimento implementada
- [ ] Detecta olhar do jogador
- [ ] Para/continua corretamente
- [ ] Trigger de sanidade funciona

## Dependências
- #20 (Modelo deve existir)
- #19 (Referência de sistema)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: high", "epic: ghosts"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#22 - Criar Modelo 3D da Criança Fantasma #3",
            "body": """**Tipo:** Art
**Prioridade:** P1 - ALTA
**Estimativa:** 10 SP
**Epic:** Assombrações

## Descrição
Modelar terceira criança (mais aterrorizante):
- Low poly (800-1200 polígonos, mais detalhes)
- Design mais perturbador
- Rigging complexo para animações

## Critérios de Aceite
- [ ] Modelo completo e rigged
- [ ] Texturas com maior detalhe
- [ ] Claramente mais ameaçadora
- [ ] Importado na engine

## Dependências
- #20 (Referência dos modelos anteriores)

## Estimativa
10 Story Points""",
            "labels": ["art", "priority: high", "epic: ghosts"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#23 - Implementar Comportamento da Criança #3",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 20 SP
**Epic:** Assombrações

## Descrição
IA para Criança Fantasma #3 (BOSS dos Dias 4-5):
- Combina comportamentos das #1 e #2
- Spawna múltiplas cópias falsas
- Mais resistente a contramedidas
- Causa -30% sanidade

## Critérios de Aceite
- [ ] IA complexa implementada
- [ ] Sistema de cópias funciona
- [ ] Resistência a contramedidas
- [ ] Balanceamento ajustado

## Dependências
- #22 (Modelo deve existir)
- #19, #21 (Sistemas anteriores)

## Estimativa
20 Story Points""",
            "labels": ["feature", "priority: high", "epic: ghosts"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#24 - Sistema de Contramedidas",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Assombrações

## Descrição
3 ações para banir assombrações:
1. Olhar fixo (3s via retrovisor/câmera)
2. Abrir janela (expulsa)
3. Rádio (desorienta temporariamente)

## Critérios de Aceite
- [ ] 3 ações implementadas
- [ ] Cada ação tem feedback visual/sonoro
- [ ] Eficácia varia por criança
- [ ] Cooldowns funcionais

## Dependências
- #19 (Pelo menos 1 assombração deve existir)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: critical", "epic: ghosts"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#25 - UI de Cooldowns e Contramedidas",
            "body": """**Tipo:** UI/UX
**Prioridade:** P1 - ALTA
**Estimativa:** 5 SP
**Epic:** Assombrações

## Descrição
HUD indicando:
- Cooldown de cada contramedida
- Ícones visuais claros
- Feedback ao usar

## Critérios de Aceite
- [ ] 3 ícones no HUD
- [ ] Cooldowns visuais (progress bars)
- [ ] Feedback quando disponível/indisponível

## Dependências
- #24 (Sistema de Contramedidas)

## Estimativa
5 Story Points""",
            "labels": ["ui-ux", "priority: high", "epic: ghosts"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        
        # EPIC 4: PROGRESSÃO (6 issues)
        {
            "title": "#26 - Sistema de Tensão Progressiva",
            "body": """**Tipo:** Feature + Balancing
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Progressão

## Descrição
Aumentar dificuldade por dia:
- Dia 1: 1 assombração, fácil de banir
- Dia 3: 2 assombrações simultâneas
- Dia 5: 3 assombrações + eventos especiais

## Critérios de Aceite
- [ ] Sistema de spawn escala por dia
- [ ] Dificuldade balanceada (playtests)
- [ ] Cada dia é vencível mas desafiador

## Dependências
- #19, #21, #23 (3 assombrações implementadas)

## Estimativa
13 Story Points""",
            "labels": ["feature", "balancing", "priority: critical", "epic: progression"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#27 - Efeitos Visuais de Sanidade Baixa",
            "body": """**Tipo:** Art + Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 10 SP
**Epic:** Progressão

## Descrição
Post-processing quando sanidade < 30%:
- Vinheta vermelha
- Distorção de lente
- Flashes aleatórios
- Partículas (sombras)

## Critérios de Aceite
- [ ] Efeitos implementados
- [ ] Intensidade proporcional à sanidade
- [ ] Performance mantida (60 FPS)

## Dependências
- #17 (Sistema de Sanidade)

## Estimativa
10 Story Points""",
            "labels": ["art", "feature", "priority: high", "epic: progression"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#28 - Áudio Dinâmico de Tensão",
            "body": """**Tipo:** Audio
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Progressão

## Descrição
Trilha adaptativa que responde a:
- Presença de assombrações
- Nível de sanidade
- Progressão do dia

## Critérios de Aceite
- [ ] 3 layers de música (base, tensão, pânico)
- [ ] Transições suaves
- [ ] Sincronizada com gameplay

## Dependências
- #17 (Sistema de Sanidade)
- #19 (Assombrações)

## Estimativa
8 Story Points""",
            "labels": ["audio", "priority: high", "epic: progression"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#29 - Sistema de Dias/Períodos",
            "body": """**Tipo:** Feature
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Progressão

## Descrição
Gerenciar ciclo de 5 dias:
- Transição entre dias
- Cutscenes de transição
- Persistência de progresso
- Unlock de narrativa

## Critérios de Aceite
- [ ] 5 dias implementados
- [ ] Transições funcionais
- [ ] Progresso salvo entre dias
- [ ] Narrativa progride corretamente

## Dependências
- #12 (Sistema de Tempo)
- #3 (Narrativa completa)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: critical", "epic: progression"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#30 - Sistema de Save/Load",
            "body": """**Tipo:** Feature
**Prioridade:** P2 - MÉDIA
**Estimativa:** 10 SP
**Epic:** Progressão

## Descrição
Salvar/carregar progresso:
- Auto-save no fim de cada dia
- Save manual no menu
- Múltiplos slots de save
- Dados: dia atual, sanidade, endings desbloqueados

## Critérios de Aceite
- [ ] Auto-save funcional
- [ ] Manual save no menu
- [ ] 3 slots de save
- [ ] Load restaura estado corretamente

## Dependências
- #29 (Sistema de Dias)

## Estimativa
10 Story Points""",
            "labels": ["feature", "priority: medium", "epic: progression"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#31 - Endings Múltiplos",
            "body": """**Tipo:** Feature + Narrative
**Prioridade:** P1 - ALTA
**Estimativa:** 12 SP
**Epic:** Progressão

## Descrição
3 finais baseados em performance:
1. Good Ending: Sanidade > 50% no Dia 5
2. Neutral Ending: Sanidade 20-50%
3. Bad Ending: Sanidade < 20%

## Critérios de Aceite
- [ ] 3 cutscenes de ending criadas
- [ ] Sistema detecta condições
- [ ] Replay incentivado

## Dependências
- #29 (Sistema de Dias)
- #2 (Narrativa completa)

## Estimativa
12 Story Points""",
            "labels": ["feature", "priority: high", "epic: progression"],
            "milestone": "M4: Content Complete"
        },
        
        # EPIC 5: ARTE (7 issues)
        {
            "title": "#32 - Modelar Ônibus Escolar Completo",
            "body": """**Tipo:** Art
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 20 SP
**Epic:** Arte

## Descrição
Modelo 3D completo do ônibus:
- Exterior detalhado (2000-3000 polys)
- Interior com 20 assentos
- Cabine do motorista funcional
- Portas animáveis

## Critérios de Aceite
- [ ] Modelo exterior completo
- [ ] Interior com assentos
- [ ] Texturas aplicadas
- [ ] Rigging para portas

## Dependências
- #4 (Style Guide)

## Estimativa
20 Story Points""",
            "labels": ["art", "priority: critical", "epic: art"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#33 - Criar Ambiente da Rota (Dia 1)",
            "body": """**Tipo:** Art
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 20 SP
**Epic:** Arte

## Descrição
Cenário para rota do Dia 1:
- Ruas low poly
- Edifícios simples (200-400 polys cada)
- 3 pontos de ônibus distintos
- Iluminação básica

## Critérios de Aceite
- [ ] Rota completa modelada
- [ ] 3 pontos de ônibus únicos
- [ ] Otimizado (60 FPS)
- [ ] Coerente com style guide

## Dependências
- #10 (Rota definida)
- #4 (Style Guide)

## Estimativa
20 Story Points""",
            "labels": ["art", "priority: critical", "epic: art"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#34 - Sistema de Iluminação Dinâmica",
            "body": """**Tipo:** Art + Technical
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Arte

## Descrição
Iluminação que muda com tempo:
- Manhã: Luz suave, sombras longas
- Tarde: Luz alaranjada, sombras curtas
- Transição suave (1 min in-game)

## Critérios de Aceite
- [ ] Sistema de lighting funcional
- [ ] Transições suaves
- [ ] Performance mantida
- [ ] Atmosfera coerente

## Dependências
- #12 (Sistema de Tempo)

## Estimativa
13 Story Points""",
            "labels": ["art", "technical", "priority: high", "epic: art"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#35 - Modelar Crianças NPC (Normais)",
            "body": """**Tipo:** Art
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Arte

## Descrição
5 modelos de crianças normais:
- Variações de uniforme/roupas
- 400-600 polys cada
- Rigging básico para animações

## Critérios de Aceite
- [ ] 5 modelos distintos
- [ ] Texturas aplicadas
- [ ] Rigged e animáveis
- [ ] Diferenciáveis dos fantasmas

## Dependências
- #4 (Style Guide)

## Estimativa
13 Story Points""",
            "labels": ["art", "priority: high", "epic: art"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#36 - Texturas e Materiais Finais",
            "body": """**Tipo:** Art
**Prioridade:** P2 - MÉDIA
**Estimativa:** 20 SP
**Epic:** Arte

## Descrição
Polish de todas as texturas:
- PBR materials (albedo, normal, roughness)
- Ônibus detalhado
- Ambiente com variação
- Otimização de tamanho

## Critérios de Aceite
- [ ] Todas texturas em PBR
- [ ] Consistentes com style guide
- [ ] Otimizadas (< 2K resolution)
- [ ] Performance mantida

## Dependências
- #32, #33, #35 (Modelos completos)

## Estimativa
20 Story Points""",
            "labels": ["art", "priority: medium", "epic: art"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#37 - Criar UI Art Completo",
            "body": """**Tipo:** UI/UX + Art
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Arte

## Descrição
Assets de UI:
- Menus (Main, Pause, Settings)
- HUD (sanidade, tempo, combustível)
- Ícones de contramedidas
- Mapa estilizado

## Critérios de Aceite
- [ ] Todos elementos de UI consistentes
- [ ] Legibilidade garantida
- [ ] Estilo coerente com jogo
- [ ] Responsivo (resoluções)

## Dependências
- #4 (Style Guide)

## Estimativa
13 Story Points""",
            "labels": ["ui-ux", "art", "priority: high", "epic: art"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#38 - Animações de Personagens",
            "body": """**Tipo:** Art + Animation
**Prioridade:** P1 - ALTA
**Estimativa:** 14 SP
**Epic:** Arte

## Descrição
Animações para NPCs e fantasmas:
- Crianças: caminhar, sentar, embarcar/desembarcar
- Fantasmas: idle perturbador, movimento flutuante
- Motorista: idle, dirigir, olhar retrovisor

## Critérios de Aceite
- [ ] Todas animações criadas
- [ ] Suaves (30 FPS min)
- [ ] Loopáveis onde necessário
- [ ] Integradas na engine

## Dependências
- #18, #20, #22 (Modelos das crianças)
- #35 (NPCs normais)

## Estimativa
14 Story Points""",
            "labels": ["animation", "art", "priority: high", "epic: art"],
            "milestone": "M4: Content Complete"
        },
        
        # EPIC 6: ÁUDIO (4 issues)
        {
            "title": "#39 - Sound Effects (SFX) Pack",
            "body": """**Tipo:** Audio
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Áudio

## Descrição
Criar/licenciar SFX:
- Motor do ônibus (acelerar, freiar, idle)
- Portas abrindo/fechando
- Passos de crianças
- Contramedidas (janela, rádio)
- Ambiente (trânsito, pássaros)

## Critérios de Aceite
- [ ] 50+ SFX de alta qualidade
- [ ] Formatos otimizados (OGG/WAV)
- [ ] Implementados na engine
- [ ] Espacialização 3D funcional

## Dependências
- Nenhuma (pode começar cedo)

## Estimativa
13 Story Points""",
            "labels": ["audio", "priority: high", "epic: audio"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#40 - Trilha Sonora Original",
            "body": """**Tipo:** Audio
**Prioridade:** P1 - ALTA
**Estimativa:** 20 SP
**Epic:** Áudio

## Descrição
Música original para o jogo:
- Main Theme (menu)
- Gameplay Loop (camadas de tensão)
- Endings (3 variações)
- Cutscenes (intro, transições)

## Critérios de Aceite
- [ ] 5 faixas completas
- [ ] Estilo consistente (ambient horror)
- [ ] Loopável para gameplay
- [ ] Masterizada

## Dependências
- #28 (Sistema de Áudio Dinâmico)

## Estimativa
20 Story Points""",
            "labels": ["audio", "priority: high", "epic: audio"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#41 - Áudio Ambiental e Atmosférico",
            "body": """**Tipo:** Audio
**Prioridade:** P2 - MÉDIA
**Estimativa:** 8 SP
**Epic:** Áudio

## Descrição
Sons de ambiente:
- Trânsito distante
- Vento
- Sussurros (assombrações próximas)
- Rádio estático

## Critérios de Aceite
- [ ] 20+ sons ambientes
- [ ] Randomização para variedade
- [ ] Espacialização 3D
- [ ] Mixagem balanceada

## Dependências
- #39 (SFX base)

## Estimativa
8 Story Points""",
            "labels": ["audio", "priority: medium", "epic: audio"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#42 - Sistema de Áudio Dinâmico",
            "body": """**Tipo:** Feature + Audio
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Áudio

## Descrição
Engine de áudio adaptativo:
- Música reage a sanidade
- SFX de motor varia com velocidade
- Distorção áudio quando assombração próxima

## Critérios de Aceite
- [ ] Sistema implementado
- [ ] Transições suaves
- [ ] Performance otimizada
- [ ] Mixagem dinâmica funcional

## Dependências
- #28 (Design do sistema)
- #39 (SFX pack)

## Estimativa
8 Story Points""",
            "labels": ["feature", "audio", "priority: high", "epic: audio"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        
        # EPIC 7: NARRATIVA (5 issues)
        {
            "title": "#43 - Cutscene: Introdução Dia 1",
            "body": """**Tipo:** Narrative + Art
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Narrativa

## Descrição
Cutscene de abertura:
- Duração: 90-120 segundos
- Introduz personagem, mundo, premissa
- Estilo: In-engine cinematics
- Diálogos: narração interna do motorista

## Critérios de Aceite
- [ ] Cutscene completa e renderizada
- [ ] Áudio sincronizado
- [ ] Skipável após primeira view
- [ ] Transição suave para gameplay

## Dependências
- #1 (Personagens nomeados)
- #2 (Narrativa completa)

## Estimativa
13 Story Points""",
            "labels": ["feature", "art", "priority: critical", "epic: narrative"],
            "milestone": "M2: Vertical Slice - Dia 1"
        },
        {
            "title": "#44 - Sistema de Jumpscares",
            "body": """**Tipo:** Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Narrativa

## Descrição
Sistema de sustos contextuais:
- Gatilhos: ignorar assombração, sanidade crítica
- 5 variações de jumpscares
- Cooldown para não saturar
- Não causa game over (mas -20% sanidade)

## Critérios de Aceite
- [ ] 5 jumpscares únicos implementados
- [ ] Sincronizado com áudio
- [ ] Cooldown funcional (5 min)
- [ ] Balanceamento ajustado

## Dependências
- #19 (Assombrações)
- #17 (Sistema de Sanidade)

## Estimativa
8 Story Points""",
            "labels": ["feature", "priority: high", "epic: narrative"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#45 - Implementar 3 Endings",
            "body": """**Tipo:** Narrative + Feature
**Prioridade:** P1 - ALTA
**Estimativa:** 20 SP
**Epic:** Narrativa

## Descrição
3 cutscenes de final:
1. Good: Motorista sobrevive, entende a verdade, encontra paz
2. Neutral: Sobrevive, mas assombrado eternamente
3. Bad: Não sobrevive, torna-se parte da lenda

## Critérios de Aceite
- [ ] 3 cutscenes completas
- [ ] Diálogos escritos e gravados
- [ ] Sistema detecta ending correto
- [ ] Replay incentivado (galeria de endings)

## Dependências
- #31 (Sistema de Endings)
- #2 (Narrativa completa)

## Estimativa
20 Story Points""",
            "labels": ["feature", "priority: high", "epic: narrative"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#46 - Eventos Narrativos por Dia",
            "body": """**Tipo:** Narrative + Feature
**Prioridade:** P2 - MÉDIA
**Estimativa:** 13 SP
**Epic:** Narrativa

## Descrição
Criar eventos especiais por dia:
- Dia 2: Descoberta de jornal velho no ônibus
- Dia 3: Conversa com NPC sobre o acidente
- Dia 4: Flashback do acidente
- Dia 5: Revelação final

## Critérios de Aceite
- [ ] 4 eventos únicos implementados
- [ ] Integrados ao gameplay
- [ ] Desbloqueiam lore
- [ ] Opcionais mas recompensadores

## Dependências
- #29 (Sistema de Dias)
- #3 (Backstory do acidente)

## Estimativa
13 Story Points""",
            "labels": ["feature", "priority: medium", "epic: narrative"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#47 - Diálogos e Voiceover",
            "body": """**Tipo:** Audio + Narrative
**Prioridade:** P2 - MÉDIA
**Estimativa:** 15 SP
**Epic:** Narrativa

## Descrição
Sistema de diálogos:
- Narração interna do motorista (key moments)
- NPCs (crianças normais) comentários breves
- Voiceover para cutscenes

## Critérios de Aceite
- [ ] Script completo escrito
- [ ] Voiceover gravado (BR-PT)
- [ ] Sincronizado com cutscenes
- [ ] Mixagem balanceada

## Dependências
- #43 (Cutscenes)
- #2 (Narrativa completa)

## Estimativa
15 SP""",
            "labels": ["audio", "priority: medium", "epic: narrative"],
            "milestone": "M4: Content Complete"
        },
        
        # EPIC 8: TESTES E POLISH (6 issues)
        {
            "title": "#48 - Alpha Testing e Bug Fixing",
            "body": """**Tipo:** QA
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Testes

## Descrição
Primeira rodada de testes internos:
- Completar jogo 10x (testers)
- Documentar bugs críticos
- Fixes prioritários

## Critérios de Aceite
- [ ] 10+ playthroughs completos
- [ ] Bugs críticos (P0) resolvidos
- [ ] Game é completável sem crashes

## Dependências
- Todos features do MVP (#9-#31)

## Estimativa
13 Story Points""",
            "labels": ["qa", "priority: critical", "epic: testing"],
            "milestone": "M3: MVP - 5 Dias Completos"
        },
        {
            "title": "#49 - Balanceamento de Dificuldade",
            "body": """**Tipo:** Balancing + QA
**Prioridade:** P1 - ALTA
**Estimativa:** 10 SP
**Epic:** Testes

## Descrição
Ajustar dificuldade via playtests:
- Taxa de spawn de assombrações
- Cooldowns de contramedidas
- Depleção de sanidade
- Duração de cada dia

## Critérios de Aceite
- [ ] 20+ playtests documentados
- [ ] Dificuldade acessível mas desafiadora
- [ ] Curva de aprendizado suave
- [ ] Endings balanceados (distribuição 30/40/30)

## Dependências
- #48 (Alpha Testing)

## Estimativa
10 Story Points""",
            "labels": ["balancing", "qa", "priority: high", "epic: testing"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#50 - Performance Optimization",
            "body": """**Tipo:** Technical + QA
**Prioridade:** P1 - ALTA
**Estimativa:** 13 SP
**Epic:** Testes

## Descrição
Otimizar para target 60 FPS:
- Profiling (CPU/GPU bottlenecks)
- LOD para modelos distantes
- Occlusion culling
- Batching de draw calls

## Critérios de Aceite
- [ ] 60 FPS em hardware mid-range
- [ ] Load times < 5s
- [ ] Sem memory leaks

## Dependências
- Todos assets criados (#32-#38)

## Estimativa
13 Story Points""",
            "labels": ["technical", "qa", "priority: high", "epic: testing"],
            "milestone": "M4: Content Complete"
        },
        {
            "title": "#51 - Polish Pass Final",
            "body": """**Tipo:** QA + Art
**Prioridade:** P2 - MÉDIA
**Estimativa:** 13 SP
**Epic:** Testes

## Descrição
Refinamentos finais:
- Animações suavizadas
- Efeitos visuais polidos
- Mixagem de áudio final
- UI tweaks

## Critérios de Aceite
- [ ] Todas animações suaves
- [ ] Áudio balanceado
- [ ] UI intuitiva
- [ ] Checklist de polish 100% completa

## Dependências
- #50 (Performance OK)

## Estimativa
13 Story Points""",
            "labels": ["qa", "art", "priority: medium", "epic: testing"],
            "milestone": "M5: Gold Master"
        },
        {
            "title": "#52 - Beta Testing Externo",
            "body": """**Tipo:** QA
**Prioridade:** P1 - ALTA
**Estimativa:** 8 SP
**Epic:** Testes

## Descrição
Testes com jogadores externos:
- 50+ testers beta
- Coletar feedback estruturado
- Fixes de bugs reportados
- Ajustes finais

## Critérios de Aceite
- [ ] 50+ playthroughs beta
- [ ] Feedback analisado e agregado
- [ ] Bugs P1/P2 resolvidos
- [ ] Score médio > 7/10 em satisfação

## Dependências
- #51 (Polish Pass)

## Estimativa
8 Story Points""",
            "labels": ["qa", "priority: high", "epic: testing"],
            "milestone": "M5: Gold Master"
        },
        {
            "title": "#53 - Release Build e Deploy",
            "body": """**Tipo:** Technical + QA
**Prioridade:** P0 - CRÍTICO
**Estimativa:** 13 SP
**Epic:** Testes

## Descrição
Preparar build de release:
- Configurações finais (sem debug)
- Packager para plataformas alvo
- Asset compression
- Builds testadas em hardware limpo

## Critérios de Aceite
- [ ] Builds para PC (Windows) funcional
- [ ] Installer criado
- [ ] Readme/manual incluído
- [ ] Testado em 3+ configurações diferentes

## Dependências
- #52 (Beta Testing completo)

## Estimativa
13 Story Points""",
            "labels": ["technical", "qa", "priority: critical", "epic: testing"],
            "milestone": "M5: Gold Master"
        }
    ]

def main():
    parser = argparse.ArgumentParser(description='Cria TODAS as 52 issues do Bus Shift')
    parser.add_argument('--token', required=True, help='GitHub Personal Access Token')
    parser.add_argument('--repo', required=True, help='Repositório (usuario/repo)')
    parser.add_argument('--dry-run', action='store_true', help='Apenas mostra o que seria criado')
    
    args = parser.parse_args()
    
    print("\n" + "="*70)
    print("BUS SHIFT - Criacao Completa de 52 Issues")
    print("="*70 + "\n")
    
    issues = get_all_issues()
    
    if args.dry_run:
        print(f"[DRY RUN] Seriam criadas {len(issues)} issues:")
        for i, issue in enumerate(issues, 9):
            print(f"  {i}. {issue['title']}")
        print("\nExecute sem --dry-run para criar de verdade")
        return
    
    print(f"Criando {len(issues)} issues...")
    print("(Isso levara ~{} minutos devido ao rate limiting)\n".format(len(issues) // 60 + 1))
    
    success_count = 0
    for i, issue in enumerate(issues, 1):
        print(f"[{i}/{len(issues)}] Criando: {issue['title']}")
        if create_issue(args.repo, args.token, issue):
            success_count += 1
        time.sleep(1.2)  # Rate limiting (GitHub permite 60 req/hora)
    
    print(f"\n{'='*70}")
    print(f"COMPLETO: {success_count}/{len(issues)} issues criadas!")
    print(f"{'='*70}\n")
    
    print("Proximos passos:")
    print("  1. Acesse: https://github.com/{}/issues".format(args.repo))
    print("  2. Adicione issues ao GitHub Project")
    print("  3. Comece desenvolvimento!")

if __name__ == "__main__":
    main()

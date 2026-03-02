# ============================================================
# close-issues.ps1
# Fecha issues implementados no fabioaap/bus_shift via gh CLI
# Gerado por: Gage (aios-devops)
# ============================================================

$REPO = "fabioaap/bus_shift"

Write-Host "⚡ Gage — Iniciando fechamento de issues em $REPO`n" -ForegroundColor Cyan

# Helper: adicionar comentário + fechar
function Close-Issue {
    param([int]$Number, [string]$Comment)
    Write-Host "→ Issue #$Number : adicionando comentário..." -ForegroundColor Yellow
    gh issue comment $Number --repo $REPO --body $Comment
    Write-Host "→ Issue #$Number : fechando..." -ForegroundColor Yellow
    gh issue close $Number --repo $REPO
    Write-Host "  ✅ #$Number fechado.`n" -ForegroundColor Green
}

# Helper: apenas comentar (não fechar)
function Comment-Issue {
    param([int]$Number, [string]$Comment)
    Write-Host "→ Issue #$Number : adicionando comentário (sem fechar)..." -ForegroundColor Yellow
    gh issue comment $Number --repo $REPO --body $Comment
    Write-Host "  ⚡ #$Number comentado.`n" -ForegroundColor Green
}

# ── EPIC 1 — Documentação ──────────────────────────────────

Close-Issue -Number 2 -Comment @"
✅ Implementado

Arquivo criado: ``docs/game/narrative/characters.md``

Contém perfis completos de todos os personagens principais:
- **Ray Morgan** — motorista protagonista
- **Earl Hayes** — despachante / voz do rádio
- **Harrison Siqueira Stone** — antagonista corporativo
- **5 fantasmas nomeados** com backstory, motivações e comportamento in-game documentados
"@

Close-Issue -Number 3 -Comment @"
✅ Implementado

Arquivo criado: ``docs/game/narrative/story.md``

**Days 2–5 escritos completamente**, cobrindo:
- Arcos narrativos diários com progressão de sanidade
- Diálogos-chave por período (manhã / tarde / noite)
- Eventos de checkpoint e condições de game over por dia
"@

Close-Issue -Number 4 -Comment @"
✅ Implementado

Arquivo criado: ``docs/game/narrative/story.md`` — **Seção Lore**

Detalha as 4 camadas narrativas do universo:
- Acidente de 1998 e suas consequências
- Victor Graves — figura central do lore expandido
- Conexões entre o passado da linha de ônibus e os eventos sobrenaturais do presente
"@

Close-Issue -Number 5 -Comment @"
✅ Implementado

Arquivo criado: ``docs/game/art/style-guide.md``

**Low Poly Bible** completa com:
- 50+ códigos hex da paleta oficial por ambiente (dia / noite / sobrenatural)
- Poly budgets por categoria de asset (personagens, veículos, props, cenários)
- Diretrizes de iluminação e sombreamento flat-shading
- Referências visuais e regras de consistência para o time de arte
"@

# ── EPIC 2 — Mecânicas Core ───────────────────────────────

Close-Issue -Number 11 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/Bus/BusStopSystem.cs``

Sistema de paradas de ônibus completo:
- **Tecla T** — abre/fecha interface de embarque/desembarque
- **Auto-close em 5s** após inatividade
- Lógica de embarque e desembarque de passageiros com validação de rota
- Integração com ``PassengerManager`` e ``RouteData``
"@

Close-Issue -Number 12 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/Core/TimerSystem.cs``

Relógio in-game completo:
- Tempo diegético configurável (velocidade de escala)
- **SPACE** — ativa/desativa a UI do relógio de pulso
- Eventos temporais (``OnHourChanged``, ``OnShiftEnd``) para integração com outros sistemas
- Suporte a períodos (manhã / tarde / noite) usados pelo ``SanitySystem``
"@

Close-Issue -Number 13 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/UI/MapUI.cs``

Mapa de rota interativo:
- **TAB** — toggle para abrir/fechar o mapa
- Conversão de coordenadas world → UI para posicionamento preciso de marcadores
- Marcadores de paradas com estado (visitado / próximo / pendente)
- Indicador de posição do ônibus em tempo real
"@

Close-Issue -Number 14 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/Bus/CameraSystem.cs``

Sistema de câmera — **modo retrovisor**:
- **Hold E** — mantém pressionado para ativar visão traseira
- Efeito de **blur overlay** na transição de câmera
- Interpolação suave entre câmera frontal e retrovisor
- Integração com sistema de fantasmas para triggering de eventos visuais
"@

Close-Issue -Number 15 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/Bus/CameraSystem.cs``

Sistema de câmera — **limpeza de estática**:
- **Tap C** — limpa distorção visual por 5 segundos
- **Cooldown de 15s** entre usos
- Efeito de estática procedural vinculado ao nível de sanidade
- UI de cooldown integrada ao ``HUDController``
"@

Close-Issue -Number 17 -Comment @"
✅ Implementado

Arquivo criado: ``game/Assets/_Project/Scripts/Core/SanitySystem.cs``

Sistema de Sanidade completo:
- **Escala 0–1** (0 = colapso, 1 = saudável)
- **10 valores iniciais** configurados em ``InitialValues[day][period]`` (5 dias × 2 períodos)
- Evento ``OnGameOver`` disparado ao atingir limiar crítico
- Modificadores por evento sobrenatural, descanso e intervenções
"@

# ── GitHub #55 — Specs Engine ─────────────────────────────

Close-Issue -Number 55 -Comment @"
✅ Implementado

Arquivo criado: ``docs/game/technical/specs-engine.md``

Especificações técnicas da engine completas:
- **Unity 6 Physics** — configurações de ``WheelCollider`` com valores exatos (stiffness, damping, mass)
- **5 FSMs de fantasmas** documentadas (estados: Dormant → Patrol → React → Manifest → Retreat)
- Configurações de performance targets (draw calls, batching, LOD distances)
- Diretrizes de integração com sistemas de áudio e pós-processamento
"@

# ── #19 e #21 — Comentar APENAS (não fechar) ──────────────

$ghostComment = "⚡ Código C# implementado em ``MarcusGhost.cs`` e ``EmmaGhost.cs``. Aguardando assets 3D (#18 e #20) para completar."

Comment-Issue -Number 19 -Comment $ghostComment
Comment-Issue -Number 21 -Comment $ghostComment

# ── #23 e #24 — Fechar SE existirem ──────────────────────

$issue23 = gh issue view 23 --repo $REPO --json number 2>$null
if ($issue23) {
    Close-Issue -Number 23 -Comment @"
✅ Implementado

Arquivos criados em ``game/Assets/_Project/Scripts/Interventions/``:
- ``InterventionBase.cs`` — classe base abstrata
- ``MicrophoneSystem.cs`` — sistema de microfone sobrenatural
- ``PanelLockSystem.cs`` — sistema de travamento de painel
- ``RadioSystem.cs`` — interferências de rádio

Todas as intervenções herdam de ``InterventionBase`` com cooldown e trigger unificados.
"@
} else {
    Write-Host "  ⚠️  Issue #23 não encontrado — pulando.`n" -ForegroundColor DarkYellow
}

$issue24 = gh issue view 24 --repo $REPO --json number 2>$null
if ($issue24) {
    Close-Issue -Number 24 -Comment @"
✅ Implementado

Arquivos criados:
- ``game/Assets/_Project/Scripts/UI/CooldownIconUI.cs`` — ícones animados de cooldown para cada intervenção
- ``HUDController.cs`` atualizado com gerenciamento de todos os ícones

Integrado ao sistema de cooldowns do ``CameraSystem`` e das intervenções.
"@
} else {
    Write-Host "  ⚠️  Issue #24 não encontrado — pulando.`n" -ForegroundColor DarkYellow
}

Write-Host "`n🚀 Operação concluída — Gage, deployando com confiança." -ForegroundColor Cyan

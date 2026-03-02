# Especificações Técnicas de Engine — Bus Shift

> **Versão:** 1.0.0 | **Issue:** #55 | **Status:** Ativo  
> **Engine:** Unity 2022.3 LTS · **Pipeline:** URP · **Linguagem:** C#  
> **Documento relacionado:** [`architecture.md`](architecture.md)

---

## Sumário

1. [Física do Ônibus](#1-física-do-ônibus)
2. [AI — State Machines dos Fantasmas](#2-ai--state-machines-dos-fantasmas)
3. [Sistema de Sanidade (Tensão)](#3-sistema-de-sanidade-tensão)
4. [Sistema de Rota e Waypoints](#4-sistema-de-rota-e-waypoints)
5. [Audio Design e Implementação](#5-audio-design-e-implementação)
6. [Post-Processing e Efeitos Visuais](#6-post-processing-e-efeitos-visuais)
7. [Configurações de Qualidade e LOD](#7-configurações-de-qualidade-e-lod)

---

## 1. Física do Ônibus

### 1.1 Rigidbody

O ônibus utiliza um único `Rigidbody` na raiz do GameObject. Todos os parâmetros abaixo são configurados via código no `BusController.Awake()` ou via Inspector, com os valores sendo definidos por `BusPhysicsConfigSO`.

| Parâmetro | Valor | Justificativa |
|---|---|---|
| `mass` | 5.000 kg | Massa realista de ônibus escolar pequeno; afeta inércia e frenagem |
| `drag` | 0.05 | Resistência aerodinâmica mínima; ônibus não para sozinho rapidamente |
| `angularDrag` | 5.0 | Alto — previne rolagem excessiva em curvas; estabilidade visual |
| `centerOfMass` | (0, -0.5, 0.3) | Baixo e levemente à frente; **previne capotamento** em curvas fechadas |
| `interpolation` | `Rigidbody.Interpolation.Interpolate` | Movimento suave sem stuttering visual |
| `collisionDetection` | `Continuous` | Previne tunelamento em colisões com obstáculos |

```csharp
// BusController.cs — Inicialização do Rigidbody
private void ConfigureRigidbody()
{
    _rigidbody.mass = config.mass;                          // 5000f
    _rigidbody.drag = config.drag;                          // 0.05f
    _rigidbody.angularDrag = config.angularDrag;            // 5.0f
    _rigidbody.centerOfMass = config.centerOfMassOffset;    // Vector3(0, -0.5f, 0.3f)
    _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    _rigidbody.collisionDetection = CollisionDetectionMode.Continuous;
}
```

---

### 1.2 WheelColliders — Configuração por Roda

O ônibus possui **4 WheelColliders**: `FL` (dianteira esquerda), `FR` (dianteira direita), `RL` (traseira esquerda), `RR` (traseira direita). Configuração via `WheelColliderConfig` instanciado por roda.

#### Suspensão

| Parâmetro | Rodas Dianteiras | Rodas Traseiras | Notas |
|---|---|---|---|
| `suspensionDistance` | 0.15 m | 0.12 m | Dianteiras com maior curso — curvas e frenagem |
| `spring.spring` | 25.000 N/m | 35.000 N/m | Traseiras mais rígidas — suportam maior carga |
| `spring.damper` | 3.000 | 4.500 | Amortecimento para evitar bounce |
| `spring.targetPosition` | 0.5 | 0.5 | Posição de equilíbrio no meio do curso |

#### Atrito (WheelFrictionCurve)

```csharp
// Configuração para todas as rodas (ajustável por ScriptableObject)
WheelFrictionCurve forwardFriction = new WheelFrictionCurve
{
    extremumSlip    = 0.4f,     // slip máximo antes de perder tração
    extremumValue   = 1.0f,     // força máxima no extremo
    asymptoteSlip   = 0.8f,     // ponto de estabilização do slip
    asymptoteValue  = 0.75f,    // força residual em slip alto
    stiffness       = 1.0f      // multiplicador geral (reduzir em chuva/neve se implementado)
};

WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve
{
    extremumSlip    = 0.2f,
    extremumValue   = 1.0f,
    asymptoteSlip   = 0.5f,
    asymptoteValue  = 0.75f,
    stiffness       = 1.0f
};
```

---

### 1.3 Motor — Torque e Velocidade

```csharp
// Aplicado apenas nas rodas TRASEIRAS (tração traseira)
// RL e RR recebem motorTorque; FL e FR recebem 0

private void ApplyMotorTorque(float inputVertical)
{
    float targetTorque = inputVertical * config.maxMotorTorque;

    // Limitar velocidade máxima (~80 km/h = ~22.2 m/s)
    float currentSpeed = _rigidbody.velocity.magnitude * 3.6f; // converter para km/h
    if (currentSpeed >= config.maxSpeedKmh)
        targetTorque = 0f;

    wheelRL.motorTorque = targetTorque;
    wheelRR.motorTorque = targetTorque;
}
```

| Parâmetro | Valor | Notas |
|---|---|---|
| `maxMotorTorque` | 1.500 Nm | Aceleração realista mas não brusca |
| `maxSpeedKmh` | 80 km/h | Velocidade máxima para a rota urbana |
| `reverseSpeedKmh` | 20 km/h | Ré limitada — ônibus não corre de ré |

**Curva de torque (AnimationCurve):** Configurada no `BusPhysicsConfigSO` — pico de torque entre 0–30 km/h, decaindo linearmente até 80 km/h. Simula comportamento de motor diesel simples.

---

### 1.4 Direção — Aproximação de Ackermann

A geometria de Ackermann define que em uma curva, a roda interna gira em um ângulo maior que a externa. Implementação simplificada:

```csharp
private void ApplySteering(float inputHorizontal)
{
    float steerAngle = inputHorizontal * config.maxSteerAngle;

    // Ackermann simplificado — apenas para correção visual
    float ackermannOffset = (config.wheelBase / config.turnRadius) * Mathf.Rad2Deg;

    if (inputHorizontal > 0) // virando à direita
    {
        wheelFL.steerAngle = steerAngle - ackermannOffset * 0.5f;
        wheelFR.steerAngle = steerAngle + ackermannOffset * 0.5f;
    }
    else
    {
        wheelFL.steerAngle = steerAngle - ackermannOffset * 0.5f;
        wheelFR.steerAngle = steerAngle + ackermannOffset * 0.5f;
    }
}
```

| Parâmetro | Valor | Notas |
|---|---|---|
| `maxSteerAngle` | 30° | Ângulo máximo de esterçamento das rodas dianteiras |
| `wheelBase` | 4.5 m | Distância entre eixos dianteiro e traseiro |
| `steerSmoothing` | 3.0 | Lerp de suavização para evitar esterçamento abrupto |

---

### 1.5 Freio

```csharp
private void ApplyBrake(bool braking, bool handbrake)
{
    float brakeForce = braking ? config.brakeTorque : 0f;
    float handbrakeForce = handbrake ? config.handbrakeTorque : 0f;

    // Freio de serviço: todas as 4 rodas
    wheelFL.brakeTorque = brakeForce;
    wheelFR.brakeTorque = brakeForce;
    wheelRL.brakeTorque = brakeForce + handbrakeForce; // freio de mão: apenas traseiras
    wheelRR.brakeTorque = brakeForce + handbrakeForce;
}
```

| Parâmetro | Valor |
|---|---|
| `brakeTorque` | 3.000 Nm |
| `handbrakeTorque` | 5.000 Nm |

---

### 1.6 Sincronização Visual das Rodas

Os meshes das rodas precisam ser atualizados a partir da posição calculada pelo `WheelCollider` (a física e o visual são desacoplados no Unity):

```csharp
private void UpdateWheelVisuals(WheelCollider collider, Transform wheelMesh)
{
    collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
    wheelMesh.SetPositionAndRotation(pos, rot);
}

// Chamar em LateUpdate() para todas as 4 rodas
```

---

## 2. AI — State Machines dos Fantasmas

Todos os fantasmas utilizam o padrão `StateMachine` com `IState` descrito em `architecture.md`. Esta seção detalha os estados específicos de cada um, incluindo transições, timings e comportamentos.

### Notação do diagrama:
`[ESTADO]` → condição → `[ESTADO]` | ⚠️ = penalidade de sanidade | 💀 = Game Over

---

### 2.1 Marcus — O Invasor

**Conceito:** Move-se gradualmente do fundo para a frente do ônibus, fileira por fileira. Se atingir a primeira fileira sem ser interrompido, ocorre Game Over.

```
[IDLE] ──── timer expirou ────► [MOVING]
  ▲                                 │
  │ microfone usado com sucesso     │ chegou na fileira alvo
  │                                 ▼
[COOLDOWN] ◄───────────────── [IN_ROW]
                                    │
                          ┌─────────┴──────────┐
                    fileira != 1         fileira == 1
                          │                    │
                      [MOVING]            [ATTACKING]
                                               │
                                   ┌───────────┴───────────┐
                             microfone (Q)          não reagido
                                   │                       │
                              [COOLDOWN]⚠️            [GAME_OVER]💀
```

**Timings por dia:**

| Dia | Intervalo entre movimentos | Fileiras antes de atacar |
|---|---|---|
| 1 | 20s | 5 fileiras (inicia no fundo) |
| 2 | 16s | 5 fileiras |
| 3 | 12s | 5 fileiras |
| 4 | 9s | 5 fileiras |
| 5 | 6s | 5 fileiras |

**Modificador de tensão:** O intervalo entre movimentos é multiplicado por `(1.0 - SanitySystem.NormalizedSanity * 0.4f)` — com 80% de tensão, Marcus se move 32% mais rápido.

**Dica visual ao jogador:** Som de passos crescendo + sombra no retrovisor conforme se aproxima.

---

### 2.2 Emma — A Burladora

**Conceito:** Faz uma corrida súbita ao painel de controle do ônibus. O jogador tem uma janela crítica de **2 segundos** para usar a Trava (`SHIFT`).

```
[IDLE] ──── timer + aleatoriedade ────► [LAUGHING]
  ▲                                          │
  │ trava usada fora do rush                 │ 1.5s de aviso (risada)
  │                                          ▼
  │                                    [RUSHING_PANEL]
  │                                          │
  │                             ┌────────────┴─────────────┐
  │                       trava (SHIFT)             2s expirou
  │                       durante rush                     │
  └──────────────────[COOLDOWN]⚠️                   [GAME_OVER]💀
```

**Timings:**

| Fase | Duração | Sinal Visual/Audio |
|---|---|---|
| `LAUGHING` (aviso) | 1.5s | Risada alta + luz tremendo |
| `RUSHING_PANEL` (janela crítica) | 2.0s | Animação de corrida ao painel |
| `COOLDOWN` após defesa | 15–25s (por dia) | — |

**Timings de cooldown por dia:**

| Dia | Cooldown após defesa | Frequência de ativação |
|---|---|---|
| 1–2 | 25s | Baixa |
| 3 | 20s | Média |
| 4–5 | 15s | Alta |

---

### 2.3 Thomas — O Narrador

**Conceito:** Escalada de áudio em 3 fases. Se não interrompido, os outros passageiros entram em pânico — **efeito cascata** que dispara penalidades extras.

```
[IDLE] ──── timer ────► [WHISPERING]
  ▲                          │ 4 segundos
  │ rádio (R)                ▼
  │ em qualquer          [CRYING_KIDS]
  │ fase antes               │ 4 segundos
  │ do limite                │ ⚠️ efeito cascata: outras
  │                          │ crianças ficam agitadas
  │                          ▼
  │                    [SCREAMING] ── 2s ──► [GAME_OVER]💀
  │                          │
  └──────── rádio (R) ───────┘ ⚠️⚠️ (penalidade maior por esperar)
```

**Fases de escalada:**

| Fase | Duração | Efeito Audio | Efeito Gameplay |
|---|---|---|---|
| `WHISPERING` | 0–4s | Sussurros baixos + estática leve no rádio | Nenhum |
| `CRYING_KIDS` | 4–8s | Choro de crianças + voz de Thomas crescendo | Outras crianças ficam agitadas: +2% sanidade/s |
| `SCREAMING` | 8–10s | Grito completo | +5% sanidade imediato; cascata máxima |
| `GAME_OVER` | 10s+ | — | Fim de jogo por colapso de ordem no ônibus |

**Efeito cascata:** Durante `CRYING_KIDS` e `SCREAMING`, os outros 4 fantasmas ficam ligeiramente mais ativos (intervalo reduzido em 20%). Cria pressão composta nos dias finais.

---

### 2.4 Grace — A Observadora

**Conceito:** Aparece no retrovisor. Se o jogador não a observar (tecla `E`) por tempo suficiente, ela bloqueia a visão e pode causar uma colisão fatal.

```
[IDLE] ──── timer ────► [APPEARING_MIRROR]
  ▲                              │
  │                    jogador olha (E)
  │                    por 3+ segundos
  │                              ├─────────────────────────────────┐
  │                     Grace desaparece              jogador não olhou
  │                              │                                 │
  └──────────────────────── [IDLE_COOLDOWN]               [BLOCKING_VIEW]
                                                                   │
                                                        ┌──────────┴─────────────┐
                                                  ônibus em             ônibus colide
                                                 movimento seguro       com obstáculo
                                                        │                        │
                                                  penalidade ⚠️             [GAME_OVER]💀
                                                 (sanidade +8%)
```

**Janela de interação:**

| Dia | Tempo para reagir | Duração do bloqueio se ignorada |
|---|---|---|
| 1–2 | 5s | Não bloqueia completamente (apenas obscurece) |
| 3 | 4s | Bloqueia 40% do retrovisor |
| 4 | 3s | Bloqueia 70% do retrovisor |
| 5 | 2s | Bloqueia 100% + distorção na câmera frontal |

**Sinal ao jogador:** Reflexo estranho no retrovisor + temperatura visivelmente baixando (efeito de névoa/condensação no vidro).

---

### 2.5 Oliver — O Artista

**Conceito:** Desenha nos vidros do ônibus, causando distorção visual progressiva. Se não "apagado" dentro do tempo, as outras crianças vivas se assustam com os desenhos.

```
[IDLE] ──── timer ────► [DRAWING]
  ▲                          │ progresso visual (0→100%)
  │                          │
  │                          ├────── 50% completo ──► distorção leve ⚠️(+3% sanidade)
  │                          │
  │                          ├────── 80% completo ──► distorção severa ⚠️(+5% sanidade)
  │                          │
  │             ┌────────────┴────────────────┐
  │       dentro do prazo              100% completo
  │       (buzina dupla)                      │
  │       (sem contramedida                   ▼
  │       direta — timing)           [SCARING_KIDS]
  └──── Oliver limpa ──────────           │ crianças reagem
         [IDLE_COOLDOWN]                  ▼
                                  penalidade composta
                              ⚠️⚠️ (+10% sanidade total)
```

**Mecânica especial:** Oliver **não tem contramedida de tecla direta**. O jogador precisa buzinar duas vezes (`T` + `T` em sequência rápida — confirmar com design) ou chegar a uma parada para interrompê-lo. Isso força o jogador a gerenciar a rota como parte da defesa.

> ⚠️ **[AUTO-DECISION]** A contramedida de Oliver não está totalmente especificada no GDD. Adotei a hipótese de "dupla buzinada" com base na mecânica do `BusStopSystem`. Confirmar com o designer antes de implementar `OliverGhost.cs`.

**Progresso dos desenhos por dia:**

| Dia | Velocidade de desenho | Janela antes de `SCARING_KIDS` |
|---|---|---|
| 1 | Lenta (60s para completar) | 60s |
| 2 | Média (45s) | 45s |
| 3 | Rápida (35s) | 35s |
| 4 | Muito rápida (25s) | 25s |
| 5 | Extremamente rápida (15s) | 15s |

---

## 3. Sistema de Sanidade (Tensão)

### 3.1 Visão Geral do Cálculo

A sanidade é um `float` normalizado entre `0.0` e `100.0`. Opera em dois modos:
- **Delta Events:** Modificações pontuais por ações do jogador ou fantasmas
- **Passive Drain/Gain:** Tick contínuo (`Update()`) baseado no estado atual

```csharp
// SanitySystem.Update() — lógica de tick passivo
private void TickPassiveSanity()
{
    float delta = 0f;

    if (dayManager.CurrentPeriod == DayPeriod.Night)
        delta += config.nightPassiveIncrease * Time.deltaTime;  // +0.5%/min = +0.0083%/s

    if (routeManager.IsDelayed)
        delta += config.routeDelayIncrease * Time.deltaTime;    // +2%/min se atrasado

    AddSanity(delta, SanitySource.Passive);
}
```

---

### 3.2 Tabela de Modificadores

#### Aumentos de Tensão

| Evento | Valor | Tipo | Observações |
|---|---|---|---|
| Colisão com obstáculo | +5% | Pontual | Por colisão; sem limite |
| Colisão grave (alta velocidade) | +10% | Pontual | Velocidade > 50 km/h |
| Não reagir ao ataque de Marcus | +12% | Pontual | + Game Over se na 1ª fileira |
| Não reagir ao ataque de Emma | +15% | Pontual | + Game Over se painel travado |
| Thomas — fase Crying | +2%/s | Contínuo | Enquanto em `CRYING_KIDS` |
| Thomas — fase Screaming | +5% | Pontual | Imediato ao entrar em `SCREAMING` |
| Grace ignorada | +8% | Pontual | + risco de colisão |
| Oliver completa desenho | +10% | Pontual | Composto com susto das crianças |
| Período noturno (passivo) | +0.5%/min | Contínuo | Durante toda a fase noturna |
| Atraso na rota (> 30s) | +2%/min | Contínuo | Enquanto atrasado |
| Parada errada | +5% | Pontual | Tecla T longe de parada |
| Dia mais alto (base inicial) | Per dia | Base | Ver tabela abaixo |

#### Reduções de Tensão

| Evento | Valor | Tipo |
|---|---|---|
| Completar parada no prazo | -3% | Pontual |
| Usar contramedida com sucesso | -1% | Pontual |
| Completar período com tempo sobrando (< 80% do limite) | -5% | Bônus único |
| Tempo sobrando > 30s | -3% adicional | Bônus único |

---

### 3.3 Tensão Base por Dia e Período

| | Dia 1 | Dia 2 | Dia 3 | Dia 4 | Dia 5 |
|---|---|---|---|---|---|
| **Manhã** | 5% | 10% | 15% | 25% | 30% |
| **Noite** | 10% | 15% | 20% | 30% | 35% |

A tensão não reseta para 0 entre períodos do mesmo dia — o valor de início da noite é `max(tensãoFinalManhã * 0.7, valorBaseNoite[dia])`. Isso garante que erros na manhã têm consequência na noite, mas o jogador não começa a noite com tensão extremamente alta por erros leves.

```csharp
// DayManager.OnPeriodChanged()
float carryoverSanity = sanitySystem.CurrentSanity * config.periodCarryoverMultiplier; // 0.7
float baseSanityNight = config.initialSanityNight;
float startNightSanity = Mathf.Max(carryoverSanity, baseSanityNight);
sanitySystem.SetSanity(startNightSanity);
```

---

### 3.4 Efeitos Visuais Progressivos (Post-Processing)

Os efeitos são gerenciados pelo `TensionUI` via **URP Global Volume** com profile dinâmico:

| Sanidade | Vignette | Chromatic Aberration | Film Grain | Lens Distortion | Notas |
|---|---|---|---|---|---|
| 0–30% | 0.10 | 0.00 | 0.05 | 0.00 | Estado "normal" |
| 30–50% | 0.22 | 0.10 | 0.12 | 0.00 | Desconforto leve |
| 50–70% | 0.38 | 0.28 | 0.22 | -0.05 | Tensão evidente |
| 70–85% | 0.55 | 0.45 | 0.38 | -0.12 | Paranoia |
| 85–95% | 0.65 | 0.58 | 0.50 | -0.20 | Pré-colapso |
| 95–100% | 0.75 | 0.70 | 0.65 | -0.30 | Colapso total |

**Implementação:** Usar `Volume.profile.TryGet<T>()` para acessar e modificar cada efeito em runtime, com `Mathf.Lerp` suavizado pelo `SanitySystem.OnSanityChanged`:

```csharp
// TensionUI.cs — atualização de efeitos
private void OnSanityChanged(float newSanity)
{
    float t = newSanity / 100f;

    if (_volume.profile.TryGet<Vignette>(out var vignette))
        vignette.intensity.value = Mathf.Lerp(0.10f, 0.75f, t);

    if (_volume.profile.TryGet<ChromaticAberration>(out var chroma))
        chroma.intensity.value = Mathf.Lerp(0.00f, 0.70f, t);

    if (_volume.profile.TryGet<FilmGrain>(out var grain))
        grain.intensity.value = Mathf.Lerp(0.05f, 0.65f, t);

    if (_volume.profile.TryGet<LensDistortion>(out var lens))
        lens.intensity.value = Mathf.Lerp(0.00f, -0.30f, t);
}
```

> **Nota de performance:** Modificar parâmetros de `Volume` todo frame é barato quando o efeito já está ativo. Evitar `TryGet` todo frame — cachear as referências no `Start()`.

---

## 4. Sistema de Rota e Waypoints

### 4.1 Estrutura de Dados

```csharp
[Serializable]
public class WaypointData
{
    public Vector3 position;
    public float segmentTimeLimitSeconds;   // tempo máximo para chegar ao próximo waypoint
    public bool isCheckpoint;              // dispara avaliação de timing
}

[Serializable]
public class BusStopData
{
    public Vector3 position;
    public float stopRadius;               // raio para detecção de chegada
    public int[] childrenToBoard;          // IDs das crianças que embarcam
    public int[] childrenToAlight;         // IDs das crianças que desembarcam
    public float boardingTimeSec;          // tempo de embarque/desembarque
    public bool isMandatory;              // parada obrigatória
}
```

### 4.2 Navegação e Indicadores

O `RouteManager` calcula continuamente a direção para o próximo waypoint e fornece ao `HUDController`:

```csharp
public class RouteManager : MonoBehaviour
{
    private int _currentWaypointIndex;
    private RouteDataSO _activeRoute;

    public Vector3 DirectionToNextWaypoint => 
        (_activeRoute.waypoints[_currentWaypointIndex].position - busTransform.position).normalized;

    public float DistanceToNextWaypoint =>
        Vector3.Distance(busTransform.position, _activeRoute.waypoints[_currentWaypointIndex].position);

    public bool IsDelayed => 
        _segmentTimer > _activeRoute.waypoints[_currentWaypointIndex].segmentTimeLimitSeconds;
}
```

**Indicadores no HUD:**
- **Seta de direção:** Ícone rotacionado com base em `DirectionToNextWaypoint` relativo à câmera
- **Distância:** Numérico em metros quando < 100m do waypoint
- **Timer de segmento:** Visível apenas quando `IsDelayed == true` (não exibir constantemente — reduz pressão desnecessária)

### 4.3 Detecção de Desvio

```csharp
// RouteManager.Update()
private void CheckRouteDeviation()
{
    // Calcular distância da linha entre waypoint anterior e atual
    float deviationDistance = CalculateDeviationFromRoute();

    if (deviationDistance > _activeRoute.deviationTolerance)  // ex: 15 metros
    {
        _deviationTimer += Time.deltaTime;

        if (_deviationTimer >= config.deviationGracePeriod)    // ex: 5 segundos
        {
            sanitySystem.AddSanity(config.deviationPenalty, SanitySource.RouteDeviation);
            _deviationTimer = 0f;
        }
    }
    else
    {
        _deviationTimer = 0f;
    }
}
```

### 4.4 Penalidades de Rota

| Evento | Sanidade | Notas |
|---|---|---|
| Desvio da rota (> 15m por > 5s) | +3% a cada 5s | Acumulativo |
| Colisão durante percurso | +5% | Independente de velocidade |
| Colisão em alta velocidade (> 50 km/h) | +10% | Substitui o básico |
| Passar por parada obrigatória sem parar | +8% | Penalidade única por parada |
| Chegar na parada com > 30s de atraso | +5% | Por parada |
| Completar segmento no prazo | -3% | Por checkpoint |

---

## 5. Audio Design e Implementação

### 5.1 Estratégia de Audio

Bus Shift utiliza o **Unity Audio nativo** com `AudioMixer` para controle de grupos. FMOD é recomendado para versões futuras com maior complexidade de áudio adaptativo, mas Unity Audio nativo é suficiente para v1.0.

```
AudioMixer (Master)
├── SFX_Group
│   ├── BusEngine_Channel
│   ├── BusDoors_Channel
│   ├── Environment_Channel
│   └── Ghost_SFX_Channel
├── Voice_Group
│   ├── Ghost_Voice_Channel  (reverb/pitch processing)
│   └── Children_Voice_Channel
├── Music_Group
│   ├── Ambient_Calm_Track
│   └── Ambient_Tense_Track
└── UI_Group
    └── HUD_SFX_Channel
```

### 5.2 Audio 3D Espacial (Fantasmas)

Cada fantasma possui um `AudioSource` com **3D spatial blend = 1.0** (totalmente espacial):

```csharp
// Configuração por AudioSource de fantasma
audioSource.spatialBlend = 1.0f;                    // 100% 3D
audioSource.rolloffMode = AudioRolloffMode.Custom;  // Curva personalizada no Inspector
audioSource.minDistance = 0.5f;                     // Som em volume máximo até 0.5m
audioSource.maxDistance = 15.0f;                    // Ônibus tem ~10m — ouvido em todo o ônibus
audioSource.dopplerLevel = 0.1f;                    // Mínimo — não queremos efeito doppler em fantasma
```

**Por fantasma:**

| Fantasma | Posição 3D | Técnica de Audio |
|---|---|---|
| Marcus | Move com o fantasma (fileira a fileira) | Pan automático conforme avança |
| Emma | Painel do ônibus (frente) | Pitchshifter + reverb reverberante |
| Thomas | Fundo do ônibus + spread alto | Chorus + reverb longo |
| Grace | Posição do retrovisor | Low-pass filter quando olhado |
| Oliver | Janelas laterais | Reverb interior de ônibus |

### 5.3 Música Adaptativa

Sistema de crossfade baseado em `SanitySystem.NormalizedSanity`:

```csharp
// MusicSystem.cs
private void Update()
{
    float sanity = sanitySystem.NormalizedSanity;

    // Crossfade entre trilha calma e tensa
    float tenseVolume = Mathf.Lerp(-80f, 0f, sanity);         // dB
    float calmVolume  = Mathf.Lerp(0f, -80f, sanity);         // dB

    audioMixer.SetFloat("Music_Tense_Volume", tenseVolume);
    audioMixer.SetFloat("Music_Calm_Volume", calmVolume);
}
```

**Trilhas previstas:**
- `ambient_calm.wav` — Música suave, humming de motor, ambiente urbano diurno
- `ambient_tense.wav` — Strings dissonantes, percussão pesada, silence breaks
- `night_ambient.wav` — Versão noturna mais densa, sem melodia clara

### 5.4 SFX Principais

| SFX | Evento Trigger | Canal | Notas |
|---|---|---|---|
| Motor (loop) | Sempre que `BusController` ativo | `BusEngine_Channel` | Pitch varia com RPM |
| Portas do ônibus | `BusStopSystem.OnBoardingStart/End` | `BusDoors_Channel` | Stereo posicionado na porta |
| Passos de Marcus | `MarcusGhost` em `MOVING` | `Ghost_SFX_Channel` | 3D, posição da fileira atual |
| Risada de Emma | `EmmaGhost` em `LAUGHING` | `Voice_Group` | Pitchshift + echo |
| Sussurros de Thomas | `ThomasGhost` em `WHISPERING` | `Voice_Group` | Low-pass + reverb |
| Choro das crianças | `ThomasGhost` em `CRYING_KIDS` | `Children_Voice_Channel` | Múltiplas vozes em loop |
| Chiado TV (câmera) | `CameraSystem.OnSecurityCamActive` | `SFX_Group` | Fade out ao usar `C` |
| Colisão do ônibus | `BusController.OnCollision` | `SFX_Group` | Impacto + metal |

### 5.5 Processamento de Voz dos Fantasmas

Aplicar via **AudioMixer Effects** no `Ghost_Voice_Channel`:

```
Reverb → Pitch Shifter → Distortion (leve) → Low-Pass Filter (quando distante)
```

| Parâmetro | Marcus | Emma | Thomas | Grace | Oliver |
|---|---|---|---|---|---|
| Reverb Room | -1000 | -500 | -200 | -800 | -600 |
| Pitch Shift | -2 semitones | +3 semitones | -4 semitones | 0 | +1 semitone |
| Distortion | 0.05 | 0.02 | 0.08 | 0.00 | 0.03 |

---

## 6. Post-Processing e Efeitos Visuais

### 6.1 URP Volume Profile — Configuração Base

Dois perfis de volume:
- **`VolumeProfile_Base`** — sempre ativo, configurações padrão
- **`VolumeProfile_Sanity`** — modificado em runtime pelo `TensionUI`

### 6.2 Efeitos por Sistema

| Efeito | Sistema | Trigger | Implementação |
|---|---|---|---|
| Vignette + Chromatic Aberration + Film Grain | `TensionUI` | `SanitySystem.OnSanityChanged` | URP Volume dinâmico |
| Flash branco de susto | `JumpscareFX` | Ataque de fantasma não detectado | `Bloom` intensity spike + fade (0.2s) |
| Névoa nos vidros | `OliverGhost` | Estado `DRAWING` | Shader de condensação com `_DrawProgress` param |
| Distorção de calor | `ThomasGhost` | Estado `SCREAMING` | URP `Lens Distortion` pontual |
| TV Noise overlay | `CameraSystem` | Câmera de segurança ativa | UI Image com shader de noise animado |
| Fade in/out entre dias | `DayManager` | Transição de período/dia | `CanvasGroup.alpha` tween |

### 6.3 Shaders Personalizados (URP Shader Graph)

| Shader | Uso | Técnica |
|---|---|---|
| `GlassCondensation.shadergraph` | Vidros de Oliver | Noise texture + `_Progress` uniform; Fresnel para bordas |
| `TVNoise.shadergraph` | Câmera de segurança | Scanlines + random noise baseado em `_Time` |
| `GhostMaterial.shadergraph` | Render dos fantasmas | Transparência + dithering + emissão pulsante |

---

## 7. Configurações de Qualidade e LOD

### 7.1 Quality Settings

| Preset | Uso | Shadow Quality | MSAA | Render Scale |
|---|---|---|---|---|
| **Low** | Hardware abaixo do mínimo | Off | Off | 0.75 (720p ef.) |
| **Medium** | Hardware mínimo | Hard Shadows | 2x | 1.0 |
| **High** | Hardware recomendado | Soft Shadows | 2x | 1.0 |
| **Ultra** | Hardware acima do rec. | Soft Shadows | 4x | 1.0 |

### 7.2 LOD Groups

| Asset | LOD0 (< 20m) | LOD1 (20–50m) | LOD2 (> 50m) | Culled (> 80m) |
|---|---|---|---|---|
| Ônibus (exterior) | ~8.000 tri | ~4.000 tri | ~1.500 tri | — |
| Prédios da rua | ~3.000 tri | ~1.000 tri | ~300 tri | Sim |
| Árvores/Postes | ~500 tri | ~200 tri | ~100 tri | Sim |
| Personagens (crianças) | ~1.500 tri | ~600 tri | Sprite billboard | Sim (> 30m no ônibus) |

### 7.3 Batching e Instancing

- **Static Batching:** Todos os objetos do ambiente que não se movem (prédios, calçadas, postes)
- **GPU Instancing:** Árvores, cercas repetitivas, marcações de rua
- **Dynamic Batching:** Pequenos objetos < 300 vértices sem skinned mesh
- **Sprite Atlas:** Todos os sprites de UI em um único atlas (1024×1024 max)

---

*Documento gerado em resposta à Issue #55 — Bus Shift Engine Specifications*  
*Complementar com: `architecture.md` (Issue #6), `systems.md`, `performance.md`*

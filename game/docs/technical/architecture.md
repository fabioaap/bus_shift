# Arquitetura Técnica — Bus Shift

> **Versão:** 1.0.0 | **Issue:** #6 | **Status:** Ativo  
> **Engine:** Unity 6 (6000.3.2f1) · **Pipeline:** URP · **Linguagem:** C#  

---

## Sumário

1. [Engine e Configuração Base](#1-engine-e-configuração-base)
2. [Visão Geral da Arquitetura](#2-visão-geral-da-arquitetura)
3. [Core Systems](#3-core-systems)
4. [Bus Systems](#4-bus-systems)
5. [Ghost Systems](#5-ghost-systems)
6. [Intervention Systems](#6-intervention-systems)
7. [UI Systems](#7-ui-systems)
8. [Padrões de Design](#8-padrões-de-design)
9. [Sistema de Save/Load](#9-sistema-de-saveload)
10. [Metas de Performance](#10-metas-de-performance)
11. [Plataformas Alvo](#11-plataformas-alvo)
12. [Diagrama de Dependências](#12-diagrama-de-dependências)

---

## 1. Engine e Configuração Base

### Unity 6 (6000.3.2f1)

| Parâmetro | Valor | Justificativa |
|---|---|---|
| **Versão** | 6000.3.2f1 | Versão mais recente do Unity 6; suporte e performance modernos; render pipeline URP integrado |
| **Render Pipeline** | URP (Universal Render Pipeline) | Ideal para estética Low Poly; excelente performance em hardware mid-range; suporte nativo a post-processing (Vignette, Chromatic Aberration, Film Grain) necessários para o sistema de sanidade |
| **Input System** | New Input System (com.unity.inputsystem) | Suporte a rebinding de teclas; mais fácil de testar com InputAction mocks; substitui o Input legado |
| **Scripting Backend** | IL2CPP (build final) / Mono (desenvolvimento) | IL2CPP em builds finais para performance e segurança; Mono durante dev para iteração rápida |
| **API Compatibility** | .NET Standard 2.1 | Melhor compatibilidade com bibliotecas modernas; suporte a JSON nativo via System.Text.Json |
| **Color Space** | Linear | Necessário para shading correto com URP e efeitos de pós-processamento precisos |

### Configurações URP Recomendadas

```yaml
# UniversalRenderPipelineAsset.asset (valores recomendados)
Rendering:
  renderScale: 1.0           # 1080p nativo; reduzir para 0.75 em 720p
  msaaSampleCount: 2         # MSAA 2x (balanço qualidade/perf)
  
Quality:
  hdrEnabled: true           # Necessário para bloom/efeitos atmosféricos
  shadowDistance: 50         # Suficiente para ruas urbanas close
  
Post-processing:
  grading: LDR               # Sem necessidade de HDR tone mapping complexo
```

---

## 2. Visão Geral da Arquitetura

Bus Shift é estruturado em **4 camadas** com comunicação via Event System, evitando acoplamento direto entre domínios:

```
┌─────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                    │
│          HUDController · TensionUI · MapUI · WatchUI    │
└──────────────────────────┬──────────────────────────────┘
                           │ (UI Events / UnityEvents)
┌──────────────────────────▼──────────────────────────────┐
│                    GAMEPLAY LAYER                        │
│   GhostSystems · BusSystems · InterventionSystems       │
└──────────────────────────┬──────────────────────────────┘
                           │ (C# Events / ScriptableObject refs)
┌──────────────────────────▼──────────────────────────────┐
│                      CORE LAYER                          │
│    GameManager · DayManager · SanitySystem · Timer      │
└──────────────────────────┬──────────────────────────────┘
                           │ (PlayerPrefs / JSON)
┌──────────────────────────▼──────────────────────────────┐
│                  PERSISTENCE LAYER                       │
│              SaveSystem · SettingsManager               │
└─────────────────────────────────────────────────────────┘
```

**Princípio central:** Nenhuma camada superior referencia diretamente camadas inferiores. A comunicação flui via eventos — os `GameManager`/`DayManager` disparam eventos; os sistemas de gameplay escutam e respondem.

---

## 3. Core Systems

### 3.1 `GameManager` (Singleton)

**Responsabilidade:** Estado global do jogo, máquina de estados principal, orquestração do fluxo entre menus, dias e finais.

```csharp
// Estados possíveis
public enum GameState
{
    MainMenu,
    DayIntro,      // Cutscene/narrativa de abertura do dia
    Playing,       // Gameplay ativo
    DayOutro,      // Avaliação pós-dia
    GameOver,      // Falha
    Ending         // Final 1, 2 ou 3
}
```

**Eventos publicados:**
- `OnGameStateChanged(GameState previous, GameState current)`
- `OnGameOver(GameOverReason reason)`
- `OnEndingTriggered(EndingType type)`

**Dependências:** `DayManager`, `SanitySystem`, `SaveSystem`

---

### 3.2 `DayManager`

**Responsabilidade:** Controla os 5 dias de jogo — qual dia está ativo, transições entre manhã/noite, configuração inicial de cada dia.

| Propriedade | Tipo | Descrição |
|---|---|---|
| `currentDay` | `int` (1–5) | Dia ativo |
| `currentPeriod` | `DayPeriod` | `Morning` \| `Night` |
| `dayConfig` | `DayConfigSO` | ScriptableObject com parâmetros do dia |
| `isTransitioning` | `bool` | Guard durante fade-in/out entre períodos |

**Eventos publicados:**
- `OnDayStarted(int day, DayPeriod period)`
- `OnDayCompleted(int day, float bonusTime)`
- `OnPeriodChanged(DayPeriod newPeriod)`

**ScriptableObject `DayConfigSO`:**
```csharp
[CreateAssetMenu(menuName = "BusShift/DayConfig")]
public class DayConfigSO : ScriptableObject
{
    public int dayNumber;
    public float initialSanityMorning;   // 5, 10, 15, 25, 30%
    public float initialSanityNight;     // 10, 15, 20, 30, 35%
    public GhostConfigSO[] activeGhosts;
    public RouteDataSO morningRoute;
    public RouteDataSO nightRoute;
    public float timeLimitSeconds;
}
```

---

### 3.3 `SanitySystem`

**Responsabilidade:** Único ponto de verdade sobre o nível de tensão/sanidade do jogador (0–100%). Expõe o valor atual e dispara eventos para que sistemas de UI e pós-processamento reajam.

```csharp
public class SanitySystem : MonoBehaviour
{
    public float CurrentSanity { get; private set; }   // 0.0 – 100.0
    public float NormalizedSanity => CurrentSanity / 100f;

    // Chamados por outros sistemas — NUNCA alterar CurrentSanity diretamente
    public void AddSanity(float amount, SanitySource source);
    public void ReduceSanity(float amount, SanitySource reason);

    // Eventos
    public static event Action<float> OnSanityChanged;      // novo valor
    public static event Action OnSanityCritical;            // >= 90%
    public static event Action OnSanityMaxReached;          // 100% → GameOver
}
```

**Fontes de aumento de tensão:**

| Trigger | Incremento | Acumulável? |
|---|---|---|
| Erro de direção (colisão) | +5% | Sim |
| Não reagir ao ataque de fantasma | +10–15% | Sim |
| Atraso na rota (> 30s) | +2%/min | Sim |
| Período noturno (passivo) | +0.5%/min | Contínuo |
| Dia mais alto (bônus base) | Config no `DayConfigSO` | Por período |

**Fontes de redução de tensão:**

| Trigger | Redução |
|---|---|
| Completar parada no prazo | -3% |
| Usar contramedida com sucesso | -1% |
| Completar dia com tempo sobrando | -5% (bônus) |

---

### 3.4 `TimerSystem`

**Responsabilidade:** Gerencia o relógio in-game (não o tempo real), controla o tempo de cada segmento da rota e o tempo total do dia.

```csharp
public class TimerSystem : MonoBehaviour
{
    public TimeSpan GameTime { get; private set; }         // Horário fictício (ex: 07:00)
    public float SegmentTimeRemaining { get; private set; }

    public static event Action<TimeSpan> OnGameTimeUpdated;
    public static event Action OnSegmentTimedOut;          // Penalidade
    public static event Action<float> OnDayTimeCompleted;  // Tempo restante como bônus
}
```

---

## 4. Bus Systems

### 4.1 `BusController`

**Responsabilidade:** Física e controle do ônibus via `WheelCollider`. Processa input do `New Input System` e aplica forças físicas.

**Componentes Unity necessários:**
- `Rigidbody` (ver specs em `specs-engine.md`)
- 4× `WheelCollider` (FL, FR, RL, RR)
- `AudioSource` (motor)

**Diagrama de input:**
```
InputAction (WASD) → BusController.OnDrive()
                   → ApplyMotorTorque() / ApplySteering() / ApplyBrake()
                   → WheelColliders
                   → Rigidbody (física Unity)
```

---

### 4.2 `RouteManager`

**Responsabilidade:** Define e gerencia os waypoints da rota. Indica ao motorista o próximo destino via HUD. Detecta desvios e aplica penalidades.

```csharp
[CreateAssetMenu(menuName = "BusShift/RouteData")]
public class RouteDataSO : ScriptableObject
{
    public WaypointData[] waypoints;
    public BusStopData[] busStops;
    public float deviationTolerance;    // raio em metros antes de penalidade
}
```

---

### 4.3 `BusStopSystem`

**Responsabilidade:** Lógica de embarque e desembarque de crianças nas paradas. Ativado pela tecla `T` quando o ônibus está dentro do raio da parada.

**Estados da parada:**

```
APPROACHING → IN_RANGE → STOPPED → BOARDING/ALIGHTING → DEPARTED
```

**Eventos:**
- `OnChildrenBoarded(int count, BusStop stop)`
- `OnChildrenAlighted(int count, BusStop stop)`
- `OnWrongStop()` → penalidade de sanidade

---

### 4.4 `CameraSystem`

**Responsabilidade:** Gerencia as 3 perspectivas visuais do jogador.

| Câmera | Tecla | Comportamento |
|---|---|---|
| **Cockpit** (padrão) | — | Câmera 1ª pessoa fixa no cockpit |
| **Retrovisor** | `E` (segurar) | Suavemente rotaciona câmera para o retrovisor; DOF no fundo |
| **Câmera de Segurança** | `C` | Overlay TV com noise shader; chiado diminui por 5s; cooldown 15s |

**Implementação:** Uma única câmera principal com transições via `Cinemachine Virtual Cameras` (prioridade/blending) — evita custo de múltiplas câmeras ativas simultâneas.

---

## 5. Ghost Systems

### 5.1 `GhostBase` (abstract)

Classe base para todos os 5 fantasmas. Define o contrato da máquina de estados e os hooks de override.

```csharp
public abstract class GhostBase : MonoBehaviour
{
    [SerializeField] protected GhostConfigSO config;
    protected StateMachine stateMachine;
    protected SanitySystem sanitySystem;

    // Hooks obrigatórios para subclasses
    protected abstract void OnEnterIdle();
    protected abstract void OnEnterAttack();
    protected abstract bool IsCountermeasureActive();

    // Ciclo de vida
    public virtual void Initialize(int dayNumber) { }
    public virtual void OnCountermeasureApplied() { }

    // Evento de game over por este fantasma
    public static event Action<GhostType> OnGhostTriggeredGameOver;
}
```

**ScriptableObject `GhostConfigSO`:**
```csharp
[CreateAssetMenu(menuName = "BusShift/GhostConfig")]
public class GhostConfigSO : ScriptableObject
{
    public GhostType ghostType;
    public float baseActivityInterval;   // segundos entre ações
    public AnimationCurve activityBySanity; // mais ativo com tensão alta
    public CountermeasureType requiredCountermeasure;
    public float countermeasureWindowSeconds; // janela de reação
    public AudioClip[] warningAudios;
    public AudioClip[] attackAudios;
}
```

---

### 5.2 Fantasmas Individuais

| Classe | Fantasma | Mecânica Principal | Contramedida |
|---|---|---|---|
| `MarcusGhost` | Marcus (Invasor) | Move-se fileira por fileira; ataca ao chegar na 1ª | Microfone `Q` (3s hold) |
| `EmmaGhost` | Emma (Burladora) | Rush ao painel; 2s de janela de reação | Trava `SHIFT` |
| `ThomasGhost` | Thomas (Narrador) | Escalada de áudio em 3 fases; efeito cascata nas crianças | Rádio `R` |
| `GraceGhost` | Grace (Observadora) | Aparece no retrovisor; bloqueia visão; causa crash se ignorada | Retrovisor `E` (observar) |
| `OliverGhost` | Oliver (Artista) | Desenha nos vidros; distorção visual; assusta crianças | — (tempo limite) |

---

## 6. Intervention Systems

Todos os sistemas de intervenção seguem o mesmo contrato base:

```csharp
public abstract class InterventionBase : MonoBehaviour
{
    protected float cooldownDuration;
    protected float remainingCooldown;
    public bool IsAvailable => remainingCooldown <= 0;

    public abstract void Activate();
    protected abstract void OnActivationComplete();

    public static event Action<InterventionType, float> OnCooldownUpdated; // para HUD
}
```

### Tabela de Contramedidas

| Sistema | Tecla | Duração Ativa | Cooldown | Fantasma Alvo |
|---|---|---|---|---|
| `MicrophoneSystem` | `Q` (segurar 3s) | 3s | 20s | Marcus |
| `PanelLockSystem` | `SHIFT` | Instantâneo | 15s | Emma |
| `RadioSystem` | `R` | Enquanto ativo | 25s | Thomas |
| `HeadlightSystem` | `F` | 15s | 20s | Visibilidade noturna geral |

> **Nota de design:** `HeadlightSystem` não combate diretamente um fantasma — serve como contramedida ambiental para a fase noturna. Considerar se deve ser migrado para `BusSystems`.

---

## 7. UI Systems

### 7.1 `HUDController`

Controlador central da UI. Gerencia a visibilidade e estado de todos os overlays. **Não contém lógica de jogo** — apenas reage a eventos.

### 7.2 `TensionUI`

Subscreve `SanitySystem.OnSanityChanged` e atualiza os efeitos visuais via **URP Volume** com pós-processamento progressivo:

| Sanidade | Vignette Intensity | Chromatic Aberration | Film Grain |
|---|---|---|---|
| 0–30% | 0.1 | 0.0 | 0.05 |
| 30–60% | 0.25 | 0.15 | 0.15 |
| 60–80% | 0.45 | 0.35 | 0.30 |
| 80–100% | 0.70 | 0.65 | 0.55 |

### 7.3 `MapUI`

- Toggle via `TAB`
- Renderizado como **Render Texture** em UI Canvas (não câmera top-down em tempo real — muito caro)
- Atualiza posição do ônibus no mapa a cada 0.5s (não por frame)

### 7.4 `WatchUI`

- Toggle via `SPACE`; exibe por 5s; cooldown 20s
- Mostra: horário in-game (`TimerSystem.GameTime`) + barra de tensão (`SanitySystem.CurrentSanity`)
- Animação de "olhar para baixo" via `Cinemachine` blend

---

## 8. Padrões de Design

### 8.1 ScriptableObjects como Data Layer

Todos os dados configuráveis **não devem estar hardcoded** em MonoBehaviours. Usar ScriptableObjects garante:
- Iteração rápida de design sem recompilar código
- Testabilidade (trocar SOs em testes unitários)
- Separação clara entre dados e comportamento

**SOs previstos:**

| ScriptableObject | Conteúdo |
|---|---|
| `DayConfigSO` | Parâmetros de cada dia (tensão inicial, fantasmas ativos, rota) |
| `GhostConfigSO` | Comportamento, timings e áudios de cada fantasma |
| `RouteDataSO` | Waypoints, paradas e tolerâncias de desvio |
| `InterventionConfigSO` | Cooldowns, durações e efeitos de cada contramedida |
| `SanityConfigSO` | Tabela de modificadores de tensão por ação |

### 8.2 Event System

Comunicação entre sistemas **exclusivamente via eventos C# estáticos ou `UnityEvent`**. Regra: se sistema A precisa saber de algo que sistema B faz, B dispara um evento — A nunca chama B diretamente (exceto Core → Gameplay em inicialização).

```csharp
// Padrão preferido: eventos C# estáticos tipados
public static event Action<float> OnSanityChanged;

// Aceitável para conexões no Inspector (UI ↔ Gameplay):
public UnityEvent<float> onSanityChangedUI;
```

### 8.3 State Machine

Cada fantasma implementa uma `StateMachine` genérica com `IState` interface:

```csharp
public interface IState
{
    void Enter();
    void Update();
    void Exit();
}

public class StateMachine
{
    private IState _currentState;
    public void TransitionTo(IState newState) { _currentState.Exit(); _currentState = newState; _currentState.Enter(); }
    public void Update() => _currentState?.Update();
}
```

O `DayManager` também usa State Machine interna para controlar o fluxo `DayIntro → Playing → DayOutro`.

### 8.4 Object Pool

Usar `UnityEngine.Pool.ObjectPool<T>` (disponível nativamente no Unity 2021+) para:
- Partículas de fantasmas (aparecimento/desaparecimento)
- Efeitos de colisão do ônibus
- Áudios de ambiente (instâncias de `AudioSource` para sons 3D)

```csharp
// Exemplo de uso
private ObjectPool<ParticleSystem> _ghostParticlePool;

void Awake()
{
    _ghostParticlePool = new ObjectPool<ParticleSystem>(
        createFunc: () => Instantiate(ghostParticlePrefab),
        actionOnGet: ps => ps.gameObject.SetActive(true),
        actionOnRelease: ps => ps.gameObject.SetActive(false),
        defaultCapacity: 10
    );
}
```

---

## 9. Sistema de Save/Load

### Dados persistidos

| Dado | Onde salvar | Formato |
|---|---|---|
| Configurações (volume, resolução, keybindings) | `PlayerPrefs` | Key-value |
| Progresso (dias desbloqueados, finais vistos) | Arquivo JSON | `SaveData.json` |
| Estado mid-session | **Não persistido** | — (cada dia reinicia do zero) |

### Estrutura do SaveData

```csharp
[Serializable]
public class SaveData
{
    public int highestDayReached;         // 1–5
    public bool[] endingsUnlocked;        // [false, false, false] → 3 finais
    public DateTime lastPlayedDate;
    public int totalPlaytimeSec;
}
```

### `SaveSystem`

```csharp
public static class SaveSystem
{
    private static readonly string SavePath =
        Path.Combine(Application.persistentDataPath, "SaveData.json");

    public static void Save(SaveData data);
    public static SaveData Load();           // retorna SaveData vazio se não existir
    public static void DeleteSave();
}
```

> **Segurança:** O JSON de save não contém informações sensíveis. Para o escopo indie atual, sem necessidade de criptografia. Se cheating se tornar problema pós-launch, aplicar hash simples de integridade.

---

## 10. Metas de Performance

### Hardware Alvo

| Componente | Mínimo | Recomendado |
|---|---|---|
| GPU | GTX 1060 | GTX 1660 |
| RAM | 8 GB | 16 GB |
| CPU | i5 8ª geração | i5 10ª geração ou equivalente |
| Armazenamento | 500 MB livre | 1 GB livre |
| SO | Windows 10 64-bit | Windows 10/11 64-bit |

### Metas de Runtime

| Métrica | Meta | Método de controle |
|---|---|---|
| Frame Rate | 60 FPS locked (VSync) | `Application.targetFrameRate = 60` |
| Resolução nativa | 1080p | Escalável para 720p em hardware abaixo do mínimo |
| Uso de RAM | < 2 GB | Asset bundles + addressables para cenas grandes |
| Tempo de carregamento | < 10s entre dias | Async scene loading + loading screen |
| Poly count (cena) | < 150k triângulos | LOD Groups nos assets do ônibus e rua |
| Draw calls | < 200 | GPU Instancing + Static Batching para ambiente |
| Texture memory | < 512 MB | Texturas máx 1024×1024, compressão DXT5/BC7 |

### Estratégias de Otimização

- **Occlusion Culling** ativado para o interior do ônibus e rua
- **Light Baking** para iluminação estática (postes, escola) — somente realtime lights no ônibus
- **Audio Mixer** com DSP budget: máx 32 vozes simultâneas
- **Time.fixedDeltaTime** = 0.02s (50 Hz physics) — suficiente para WheelColliders de ônibus

---

## 11. Plataformas Alvo

| Plataforma | Status | Build Target | Notas |
|---|---|---|---|
| **Windows 10/11 (x64)** | Primário | IL2CPP | Plataforma principal; distribuição Steam |
| **macOS (Intel + Apple Silicon)** | Secundário | IL2CPP (Universal) | Universal Binary via Rosetta 2 |
| **Linux** | Fora do escopo v1.0 | — | Avaliar pós-launch com feedback da comunidade Steam |
| **Console** | Fora do escopo | — | Não previsto |

### Steam

- **Steamworks SDK** integrado via `com.unity.steamworks-steamworks` ou `Steamworks.NET`
- Achievements: mínimo 5 (um por dia completado) + 3 por final desbloqueado
- Steam Cloud: sincronizar `SaveData.json` automaticamente
- Steam Input: compatível com controles (mapeamento secundário, não foco principal)

---

## 12. Diagrama de Dependências

```
GameManager
    ├── DayManager
    │       ├── SanitySystem
    │       ├── TimerSystem
    │       └── RouteManager
    │
    ├── GhostSystems [MarcusGhost, EmmaGhost, ThomasGhost, GraceGhost, OliverGhost]
    │       └── GhostBase → SanitySystem (eventos), GhostConfigSO (dados)
    │
    ├── BusSystems
    │       ├── BusController → WheelColliders, Rigidbody
    │       ├── RouteManager → RouteDataSO
    │       ├── BusStopSystem → SanitySystem (eventos)
    │       └── CameraSystem → Cinemachine
    │
    ├── InterventionSystems [Microphone, PanelLock, Radio, Headlight]
    │       └── InterventionBase → SanitySystem (eventos), GhostBase (trigger)
    │
    └── UISystems
            ├── HUDController → (escuta eventos de todos os sistemas)
            ├── TensionUI → SanitySystem.OnSanityChanged → URP Volume
            ├── MapUI → RouteManager (posição)
            └── WatchUI → TimerSystem, SanitySystem
```

---

*Documento gerado em resposta à Issue #6 — Bus Shift Technical Documentation*  
*Próximos documentos relacionados: `specs-engine.md` (Issue #55), `systems.md`, `performance.md`*

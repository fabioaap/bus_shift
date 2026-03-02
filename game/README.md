# Bus Shift — Unity Project

> Engine: **Unity 6 (6000.3.2f1)** | Pipeline: **URP** | Language: **C#**

## Como Abrir no Unity Editor

1. Abra o **Unity Hub**
2. Clique em **Add > Add project from disk**
3. Selecione a pasta `game/` (este diretório)
4. Aguarde o Unity importar os packages (~5-10 min na primeira vez)
5. Verifique se não há erros de compilação no Console

## Estrutura de Pastas

```
Assets/_Project/
├── Scripts/
│   ├── Core/          ← GameManager, SanitySystem, DayManager
│   ├── Bus/           ← BusController, RouteManager, BusStopSystem
│   ├── Ghosts/        ← GhostBase (abstract) + 5 implementações
│   ├── Interventions/ ← MicrophoneSystem, PanelLockSystem, RadioSystem, HeadlightSystem
│   ├── UI/            ← HUDController, TensionUI, MapUI, WatchUI
│   └── Data/          ← ScriptableObjects (GhostConfigSO, DayConfigSO)
├── Prefabs/           ← Prefabs por categoria
├── Scenes/            ← MainMenu, Day01_Morning, Day01_Night, ...
├── Materials/
├── Textures/
├── Animations/
└── Audio/
    ├── SFX/
    └── Music/
```

## Packages Instalados

| Package | Versão | Uso |
|---------|--------|-----|
| Universal Render Pipeline | 17.0.3 | Renderização Low Poly + post-processing |
| Input System | 1.8.2 | Controles WASD + gamepad |
| Cinemachine | 3.1.2 | Camera 1ª pessoa + retrovisor |
| TextMeshPro | 3.0.9 | UI e HUD |
| AI Navigation | 2.0.5 | NavMesh para NPCs |
| Test Framework | 1.4.6 | Testes unitários |

## Como Buildar

```
File > Build Settings > Windows > Build
```

Pasta de output: `Builds/` (ignorada pelo git)

## Documentação do Jogo

Ver: `docs/`
- [Arquitetura Técnica](docs/technical/architecture.md)
- [Mecânicas](docs/design/mechanics.md)
- [Personagens](docs/narrative/characters.md)

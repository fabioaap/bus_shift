# Stack Técnica - Bus Shift

## Engine & Plataforma

| Aspecto | Valor |
|---------|-------|
| **Game Engine** | Unity 6000.3.2f1 |
| **Linguagem Scripting** | C# |
| **IDE Recomendada** | Visual Studio |
| **Plataforma-alvo** | Windows (Steam) |

---

## Versões & Compatibilidade

### Unity 6000.3.2f1
- **Versão LTS:** Não (6000.x é versão "superlativa" do ciclo 2024)
- **Suporte:** ✅ Ativa até junho 2026
- **Recursos:** URP (Universal Render Pipeline), DOTS, SerializationFramework v2
- **Performance:** Otimizado para indie games com low-poly aesthetic

### C# (.NET)
- **Versão:** Conforme Unity 6000.3.2 (C# 8.0+)
- **Features:** Async/await, nullable annotations, records, pattern matching

### Visual Studio
- **Versão:** Community/Professional 2022+ (recomendado)
- **Extensão:** Unity Debugger + IntelliCode
- **Alternativa:** Rider by JetBrains (premium, melhor intellisense)

---

## Dependências & Pacotes

### Built-in Packages (incluidos todo Unity 6)
- ✅ URP (Universal Render Pipeline) - Para low-poly
- ✅ Physics 3D
- ✅ Audio Engine
- ✅ Input System
- ✅ Cinemachine (câmera)

### Recomendados para Adicionar
- 📦 TextMesh Pro - UI avançada
- 📦 PostProcessing (para horror effects)
- 📦 ProGrids - Alignamento preciso
- 📦 Polybrush - Escultura de low-poly

---

## Performance Targets (Bus Shift)

| Métrica | Alvo |
|---------|------|
| **FPS** | 60+ (60fps ou higher) |
| **Resolution** | 1920x1080 (1440p suportado) |
| **Draw Calls** | < 100/frame |
| **Memory** | < 2GB (low-end) |
| **Build Size** | < 500MB |

---

## Estrutura Recomendada no Projeto

```
Assets/
├── Scenes/              # Cenas Unity .unity
├── Scripts/             # Código C#
│   ├── Core/
│   ├── Systems/
│   ├── UI/
│   └── Gameplay/
├── Prefabs/             # Componentes reutilizáveis
├── Models/              # 3D assets (FBX)
├── Materials/           # Shaders & materiais
├── Textures/            # Texturas (PNG, TGA)
├── Audio/               # Sons (WAV, OGG)
├── Editor/              # Scripts de editor customizado
└── Resources/           # Recursos carregados em runtime
```

---

## Código Padrão C# (Bus Shift)

### Convenção de Classe Principal
```csharp
using UnityEngine;

namespace BusShift.Gameplay
{
    public class DriverController : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private Transform steeringWheel;

        private void Start()
        {
            // Inicialização
        }

        private void Update()
        {
            // Input e lógica por frame
        }

        private void FixedUpdate()
        {
            // Física
        }
    }
}
```

### Padrão de Sistemas
- Usar ScriptableObject para configurações (não hard-coded)
- Usar Events/Actions para comunicação entre sistemas
- Evitar Singletons (usar Manager com DI pattern)

---

## Build & Deploy

### Para Desenvolvimento
```bash
unity --project . --executeMethod BuildManager.BuildDev
```

### Para Steam
```bash
unity --project . --executeMethod SteamBuildPipeline.Build
```

### Configuração Steam
- Steam App ID: [TBD]
- Branch: main, beta, test
- Contagem de compilações: Monitorar tempo de compilação

---

## DevOps & CI/CD

| Fase | Ferramenta | Ação |
|------|-----------|------|
| **Build** | Unity Cloud Build | Compilar binárias |
| **Test** | Unity Test Framework | Rodar testes C# |
| **Static Analysis** | SonarQube | Qualidade de código |
| **Deploy** | Steamworks | Deploy para Steam |

---

## Problemas Conhecidos & Workarounds

### Unity 6000.3.2 + Steam
- ⚠️ **Problema:** Serialization de grandes prefabs pode travar editor
  - **Workaround:** Dividir em micro-prefabs, usar addressables

- ⚠️ **Problema:** URP pode ter stuttering em algunas GPUs low-end
  - **Workaround:** Reducir sombras, ativar batching

### C# em Bus Shift
- ⚠️ **Guideline:** Evitar reflexão em runtime (performance)
  - **Padrão:** Usar static registries em editor time

---

## Referências & Documentação

- 📖 [Unity 6 Manual](https://docs.unity.com/)
- 📖 [C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- 📖 [Steam Integration Guide](https://partner.steamgames.com/)
- 📖 [URP Best Practices](https://docs.unity.com/2022.3/Documentation/Manual/urp-index.html)

---

**Status:** ✅ Stack Técnica Confirmada
**Data:** 1 de março de 2026
**Atualização anterior:** Débto técnico preenchido em `docs/game/technical/README.md`

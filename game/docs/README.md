# Bus Shift - Documentação do Jogo

Documentação completa do jogo Bus Shift.

---

## 📁 Estrutura da Documentação

```
game/docs/
├── README.md                    # Este arquivo - Índice principal
├── design/                      # Game Design Documents
│   ├── README.md               # Visão geral do design
│   ├── game-design-doc.md      # GDD completo
│   ├── mechanics.md            # Mecânicas de jogo
│   ├── progression.md          # Sistema de progressão
│   └── economy.md              # Sistema econômico
├── narrative/                   # História e narrativa
│   ├── README.md               # Visão geral narrativa
│   ├── story.md                # História principal
│   ├── characters.md           # Personagens
│   └── dialogues.md            # Diálogos
├── technical/                   # Documentação técnica
│   ├── README.md               # Visão geral técnica
│   ├── architecture.md         # Arquitetura do jogo
│   ├── systems.md              # Sistemas principais
│   └── performance.md          # Performance e otimização
├── art/                         # Arte e estilo visual
│   ├── README.md               # Visão geral de arte
│   ├── style-guide.md          # Guia de estilo visual
│   ├── assets.md               # Documentação de assets
│   └── ui-ux.md                # Design de UI/UX
├── audio/                       # Áudio e música
│   ├── README.md               # Visão geral de áudio
│   ├── music.md                # Trilha sonora
│   └── sfx.md                  # Efeitos sonoros
├── levels/                      # Design de níveis
│   ├── README.md               # Visão geral de níveis
│   └── level-design.md         # Design de níveis
└── testing/                     # Testes e QA
    ├── README.md               # Visão geral de testes
    ├── test-plan.md            # Plano de testes
    └── known-issues.md         # Issues conhecidos
```

---

## 📊 Status dos Documentos

| Documento | Status | Última Atualização |
|-----------|--------|-------------------|
| [GDD.md](GDD.md) | 🟢 Completo | Original |
| [Game Design Doc](design/game-design-doc.md) | 🟡 Template | Precisa preenchimento |
| [Mecânicas](design/mechanics.md) | 🟢 Completo | 2026-03-01 |
| [Balanceamento](design/balancing.md) | 🟢 Completo | 2026-03-01 |
| [História](narrative/story.md) | 🟡 Parcial | Dia 1 completo |
| [Finais](narrative/endings.md) | 🟢 Completo | 2026-03-01 |
| [Personagens](narrative/characters.md) | 🟡 Planejamento | Precisa definições |
| [Known Issues](testing/known-issues.md) | 🟢 Template | Pronto para uso |

**Legenda:**
- 🟢 Completo - Pronto para uso
- 🟡 Em Progresso - Parcialmente preenchido
- 🔴 Pendente - Aguardando desenvolvimento

---

## 🎮 Sobre o Jogo

**Nome:** Bus Shift
**Gênero:** Horror de Sobrevivência, Terror Psicológico, 1ª Pessoa
**Plataformas:** [A definir]
**Engine:** [A definir]
**Estilo Visual:** Low Poly

### Visão Geral
Bus Shift é um jogo de terror em primeira pessoa inspirado em Five Nights at Freddy's, onde você assume o papel de motorista de ônibus escolar assombrado. Durante 5 dias, você deve completar rotas matinais e noturnas enquanto lida com a presença de três crianças fantasmas que tentam sabotar sua jornada.

O jogo combina mecânicas de direção realista com gestão de ameaças sobrenaturais através de um sistema de cooldown de ferramentas. A tensão aumenta progressivamente a cada dia, e suas decisões determinam um dos três finais possíveis.

### Público-Alvo
- Fãs de jogos de terror (estilo FNAF)
- Jogadores que apreciam terror psicológico
- Amantes de experiências de suspense e jumpscares
- Público 16+ (classificação sugerida)

### Pilares do Game Design
1. **Tensão Crescente** - Sistema de insanidade que aumenta por erro e ao longo dos dias
2. **Decisões sob Pressão** - Gerenciamento de cooldowns de ferramentas em momentos críticos
3. **Narrativa Atmosférica** - História envolvente revelada gradualmente sobre acidente do passado
4. **Mecânicas de Direção + Horror** - Combinação única de simulação de veículo com terror sobrenatural

---

## 📖 Como Usar Esta Documentação

### Para Designers
- Consulte [`design/`](design/) para mecânicas e sistemas
- Veja [`levels/`](levels/) para design de níveis
- Confira [`narrative/`](narrative/) para história e personagens

### Para Desenvolvedores
- Consulte [`technical/`](technical/) para arquitetura
- Veja [`design/mechanics.md`](design/mechanics.md) para implementação de mecânicas

### Para Artistas
- Consulte [`art/`](art/) para guias de estilo
- Veja [`art/style-guide.md`](art/style-guide.md) para referências visuais

### Para QA/Testers
- Consulte [`testing/`](testing/) para planos de teste
- Reporte issues em [`testing/known-issues.md`](testing/known-issues.md)

---

## 🔄 Workflow de Documentação

### Atualizando Documentação
1. Identifique a seção apropriada
2. Edite o arquivo Markdown correspondente
3. Mantenha referências cruzadas atualizadas
4. Commit com mensagem clara: `docs(game): [descrição]`

### Criando Nova Documentação
1. Escolha a pasta apropriada
2. Crie arquivo Markdown descritivo
3. Adicione link neste README
4. Use templates existentes como base

---

## 📋 Status da Documentação

| Seção | Status | Última Atualização |
|-------|--------|-------------------|
| Design | 🔴 Pendente | - |
| Narrative | 🔴 Pendente | - |
| Technical | 🔴 Pendente | - |
| Art | 🔴 Pendente | - |
| Audio | 🔴 Pendente | - |
| Levels | 🔴 Pendente | - |
| Testing | 🔴 Pendente | - |

**Legenda:**
- 🟢 Completo
- 🟡 Em Progresso
- 🔴 Pendente

---

## 🚀 Quick Links

### Documentos Essenciais
- [Game Design Document](design/game-design-doc.md)
- [Arquitetura Técnica](technical/architecture.md)
- [Guia de Estilo Visual](art/style-guide.md)

### Referências Externas
- [Repositório GitHub](https://github.com/seu-usuario/bus_shift)
- [Project Board](link-para-board)
- [Wiki](link-para-wiki)

---

## 📝 Convenções

### Formatação Markdown
- Use headers hierárquicos (H1 > H2 > H3)
- Adicione tabela de conteúdo em docs longos
- Use code blocks para exemplos técnicos
- Adicione imagens na pasta `assets/` correspondente

### Nomenclatura de Arquivos
- Use kebab-case: `game-design-doc.md`
- Seja descritivo: `character-movement-system.md`
- Agrupe por funcionalidade

---

## 🤝 Contribuindo

Para contribuir com a documentação do jogo:

1. Siga os princípios da Constitution AIOS
2. Story-Driven: Referencie Story ID se aplicável
3. Use contexto apropriado do GitHub Copilot
4. Mantenha linguagem clara e objetiva
5. Adicione exemplos quando possível

---

*Bus Shift Game Documentation - v1.0*
*Última atualização: 1 de março de 2026*

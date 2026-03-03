# Bus Shift

[![Status](https://img.shields.io/badge/status-beta-orange.svg)](#)
[![Engine](https://img.shields.io/badge/engine-Unity%206-black.svg)](https://unity.com/)
[![Plataforma](https://img.shields.io/badge/plataforma-PC-blue.svg)](#)
[![Licença](https://img.shields.io/badge/licença-MIT-green.svg)](LICENSE)
[![Narrative Page](https://img.shields.io/badge/narrative%20page-online-brightgreen.svg)](https://fabioaap.github.io/bus_shift/)

> *"It's just an old bus. Old buses make noises."* — Dale Mercer, Dia 1

**Bus Shift** é um jogo de horror psicológico em primeira pessoa desenvolvido em Unity. Você é Dale Mercer, um motorista de ônibus escolar em Ravenswood, Pennsylvania. O que começa como um emprego comum revela que você não está sozinho na rota.

---

## Sinopse

**Ravenswood, Pennsylvania. Outubro de 2026.**

Em março de 2023, o Ônibus 104 caiu da Ponte Ridgemont, matando o motorista Victor Graves e cinco crianças da quinta série. Os corpos das crianças foram recuperados. O de Victor, nunca.

O ônibus foi "consertado" e voltou à rota.

Agora é você quem está no volante.

Durante 5 dias de turnos matinais e vespertinos, você vai buscar e entregar as crianças enquanto entidades inexplicáveis agem dentro do veículo. Suas ferramentas são o retrovisor, o microfone, a trava dos controles e o rádio. Sua sanidade é o recurso mais escasso.

---

## Gameplay

| Mecânica | Descrição |
|---|---|
| **Direção** | A/S/D — aceleração, freio/ré, curvas. Inércia pesada e realista |
| **Retrovisor** | Revela o que acontece nas fileiras traseiras |
| **Microfone (Q)** | Ordena que os passageiros se assentem. Recarga lenta |
| **Trava (SHIFT)** | Bloqueia controles do painel contra interferências |
| **Rádio (R)** | Abafa ruídos e dispersa algumas entidades |
| **Barra de Tensão** | Sobe a cada anomalia. Ao máximo, desencadeia o estado crítico |

Os 5 passageiros sobrenaturais — Marcus, Emma, Thomas, Grace e Oliver — têm comportamentos únicos e escalados por dia. O Dia 5 coloca todos ativos simultaneamente.

---

## Os Passageiros

| Nome | Comportamento |
|---|---|
| **Marcus Reid** (O Inquieto) | Troca de assento frequentemente. Ataque letal ao atingir a primeira fileira após 30s |
| **Emma Lynch** (A Risonha) | Risadas que antecedem sabotagens no painel de controle |
| **Thomas Sanders** (O Contador) | Sussurra no rádio, bloqueando a visibilidade e induzindo pânico |
| **Grace Harper** (A Observadora) | Olha diretamente para você no retrovisor. Avança fileiras sem ser visto se mover |
| **Oliver Crane** (O Artista) | Desenha nos bancos. Os desenhos revelam eventos futuros — e passados |

---

## Estrutura da Narrativa

O jogo se passa ao longo de **5 dias**, cada um escalando a intensidade:

| Dia | Estado de Dale | Evento Central |
|---|---|---|
| 1 | Negação | Primeiro contato com Emma no corredor |
| 2 | Curiosidade ansiosa | Encontra o jornal com a notícia do acidente de 2023 |
| 3 | Reconhecimento doloroso | Descoberta do diário de Victor Graves e os desenhos de Oliver |
| 4 | Identificação perigosa | Transmissão policial revela a causa real do acidente |
| 5 | Escolha existencial | Todos os passageiros ativos. Três finais possíveis |

**[→ Ler a narrativa completa](https://fabioaap.github.io/bus_shift/story.html)**

---

## Finais

- **Redenção** — Sobrevive intacto, descobre que é um fantasma e descansa
- **Trauma** — Foge a pé, acorda num hospital psiquiátrico
- **O Ciclo** *(true end)* — Morre no volante. A vaga é reaberta. Loop reinicia

---

## Assets

| Asset | Origem | Licença |
|---|---|---|
| Modelo do ônibus escolar | `Assets/School Bus/` | Projeto |
| Ambiente urbano low-poly | Synty PolygonCity + PolygonGeneric | Asset Store |
| Veículos extras | Vladislav Simakov — Street Vehicles Pack | Asset Store |
| Sistema de input | Unity Input System | Unity |

---

## Estrutura do Repositório

```
bus_shift/
├── game/                      # Projeto Unity
│   ├── Assets/
│   │   ├── _Project/          # Scripts, cenas e prefabs do jogo
│   │   ├── School Bus/        # Modelo 3D do ônibus escolar
│   │   ├── Synty/             # Assets de ambiente urbano
│   │   ├── Vladislav Simakov/ # Pack de veículos extras
│   │   └── Scenes/            # Cenas Unity
│   └── docs/
│       └── narrative/         # GDD e narrativa completa
├── narrative/                 # Página web interativa (GitHub Pages)
│   ├── index.html             # Mesa do investigador
│   └── story.html             # GDD e história completa
└── docs/                      # Documentação do projeto
```

---

## Como Clonar

O projeto usa **Git LFS** para os assets binários do Unity:

```bash
git lfs install
git clone https://github.com/fabioaap/bus_shift.git
cd bus_shift/game
# Abra a pasta game/ no Unity Hub
```

**Requisitos:**
- Unity 6.x
- Windows 10/11 (64-bit)
- GPU com suporte a URP
- Git LFS instalado

---

## Equipe

| Papel | Responsável |
|---|---|
| IA Builder de Product Designer | Fábio Alves |
| Programação Unity | Yan Callegaris |
| Programação Unity | Gabriel Felix |
| Game Design & Programação Unity | Luana Zaparoli |

---

## Licença

Distribuído sob a licença MIT. Veja [LICENSE](LICENSE) para detalhes.

Assets de terceiros (Synty, Vladislav Simakov) estão sujeitos às suas respectivas licenças da Unity Asset Store e não podem ser redistribuídos separadamente.

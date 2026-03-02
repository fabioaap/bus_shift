# Mapa da Vila - Referência Visual

## Descrição da Imagem

**Tipo:** Game Map / UI Element
**Função:** Mapa para navegação dentro do jogo (visualizável via TAB)

---

## Conteúdo do Mapa

### Locais Identificáveis

| Local | Símbolo | Função |
|-------|---------|--------|
| **ESCOLA** | Edifício grande | Ponto principal de partida/chegada |
| **IGREJA** | Edifício com cruz | Local secundário |
| **CASAS** | Múltiplos blocos | Residências dos alunos |
| **PONTOS DE ÔNIBUS** | Círculos/marcas | Paradas para pegar/deixar crianças |
| **SORRO/PONTO DE COLETA** | Marca especial | Garagem/estacionamento do ônibus |

### Características Visuais

- **Estilo:** Mapa "envelhecido/queimado" (estilo vintage/horror)
- **Paleta:** Tons sepia, marrom claro, sangue (manchas vermelhas)
- **Textura:** Papel enrugado, queimado nas bordas
- **Tipografia:** Manuscrita/artesanal
- **Bússola:** Norte indicado (N, S, E, W)

---

## Layout da Cidade

### Estrutura Urbana
```
[ESCOLA] ─── Ponto de Ônibus 1
   │
   ├─ [IGREJA]
   │
   ├─ [CASAS] (múltiplas)
   │
   └─ Pontos de Ônibus 2-5 distribuídos
   
[SORRO/GARAGEM] = Local de descanso do ônibus
```

---

## Uso no Jogo (TAB)

**Mecânica:** Ao pressionar TAB, jogador vê este mapa
- ✅ Mostra rota do dia (em vermelho, conforme GDD)
- ✅ Marca os pontos de parada obrigatórios
- ✅ Ajuda na navegação durante trajeto
- ⚠️ **Bloqueia parcialmente a visão da rua**

---

## Design Intencional

### Propósito Narrativo

A escolha de um **mapa "antigo/queimado"** reflete:
1. **Horror Atmosphérico** = Sensação de perigo
2. **Índício Visual** = Algo terrível aconteceu aqui (queimaduras = sangue?)
3. **Imersão** = Faz parte do mundo sinistro, não é UI moderna/clean

### Inspiração Visual

Estilo similar a:
- Documentos de suspense psicológico
- Mapas em jogos horror clássicos
- Artefatos de "cena de crime"

---

## Detalhes Importantes

### Sangue/Manchas No Mapa

As manchas vermelhas sugerem:
- Acidente anterior (crime scene)
- Presença do sobrenatural
- Aviso silencioso ao jogador

### Bússola

Indica orientação - importante para:
- Navegação
- Imersão mundo-aberto
- Realismo ambiental

---

## Tamanho da Cidade

Com base na descrição GDD:
- **Pequena o suficiente** para exploração em um dia de jogo
- **Grande o suficiente** para ter múltiplos pontos de ônibus
- **Isolada** (sem conexão clara com "fora")

---

**Status:** ✅ Documentada como Map Reference
**Data:** 1 de março de 2026

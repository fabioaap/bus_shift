# Bus Shift — Template de Auditoria de Assets da Build 0.1.0

## Identificação da sessão

| Campo | Valor |
|---|---|
| Data | |
| Responsável | |
| Branch | `agent/aiox-5-3-game-squad` |
| Commit analisado | |
| Unity | `6000.3.10f1` |
| Projeto aberto | `game/` |
| Unity MCP | disponível / indisponível / não verificado |
| Git LFS | completo / incompleto / não verificado |
| Console baseline | sem erros / com erros / não verificado |

## Regras da auditoria

1. Não modificar os assets originais de terceiros.
2. Não limpar o Console antes de registrar o baseline.
3. Não declarar um asset aprovado apenas porque o arquivo existe.
4. Separar `confirmado`, `provável` e `não verificado`.
5. Registrar origem e licença antes de integração.
6. Testar derivados em pasta própria do projeto.
7. Não comprar substitutos antes de fechar a decisão do asset existente.

## Classificações permitidas

- `REUTILIZAR`
- `ADAPTAR`
- `PRODUZIR`
- `ADQUIRIR`
- `PLACEHOLDER`
- `ADIAR`
- `BLOQUEADO`

## Severidade de problemas

- `P0`: bloqueia abertura, corrompe projeto ou impede build.
- `P1`: bloqueia a vertical slice ou uma mecânica principal.
- `P2`: degrada legibilidade, performance ou qualidade de modo relevante.
- `P3`: polish ou melhoria sem bloquear a validação.

---

# 1. Baseline técnico

## 1.1 Git LFS

- [ ] `git lfs install` executado.
- [ ] `git lfs pull` executado.
- [ ] Nenhum arquivo binário essencial permanece como pointer text.
- [ ] `git status` registrado antes da auditoria.

Evidência:

```text
Cole aqui os comandos e resultados relevantes.
```

## 1.2 Console da Unity

| Tipo | Quantidade | Resumo | Evidência |
|---|---:|---|---|
| Errors | | | |
| Warnings | | | |
| Logs relevantes | | | |

## 1.3 Render e packages

- [ ] URP carregada corretamente.
- [ ] Materiais de terceiros não aparecem rosa.
- [ ] Input System ativo.
- [ ] Cinemachine importa sem erros.
- [ ] Nenhuma incompatibilidade de package bloqueia a inspeção.

---

# 2. Auditoria do ônibus escolar

## Caminho auditado

`game/Assets/School Bus/`

## 2.1 Inventário

| Asset/prefab | Caminho | Tipo | Importa? | Material URP? | Licença localizada? | Uso potencial |
|---|---|---|---|---|---|---|
| | | | | | | |

## 2.2 Geometria e hierarquia

| Verificação | Resultado | Confiança | Evidência/observação |
|---|---|---|---|
| Exterior separado do interior | | | |
| Porta dianteira separada | | | |
| Pivot da porta utilizável | | | |
| Rodas separadas | | | |
| Volante separado | | | |
| Bancos reutilizáveis | | | |
| Corredor comporta câmera | | | |
| Frente/cockpit utilizável | | | |
| Normais corretas | | | |
| Escala coerente em metros | | | |
| Hierarquia estável | | | |

## 2.3 Materiais e texturas

| Material | Shader atual | Estado URP | Texturas | Problema | Ação |
|---|---|---|---|---|---|
| | | | | | |

## 2.4 Física e gameplay

| Verificação | Resultado | Problema | Severidade | Ação proposta |
|---|---|---|---|---|
| Collider da carroceria | | | | |
| Porta interativa | | | | |
| Rodas/sistema veicular | | | | |
| Posição da câmera | | | | |
| Campo de visão do retrovisor | | | | |
| Espaço para rádio e Panel Lock | | | | |
| Clipping interno | | | | |

## 2.5 Decisão Gate B0

Escolha uma:

- [ ] `REUTILIZAR`
- [ ] `ADAPTAR`
- [ ] `ADQUIRIR`
- [ ] `PRODUZIR`
- [ ] `BLOQUEADO`

Justificativa:

```text
Explique a decisão com base em escala, interior, pivôs, materiais, licença,
integração veicular e custo de correção.
```

Próxima ação concreta:

```text
Ex.: duplicar o prefab original para Assets/_Project/Art/Vehicles/Bus104/,
converter materiais para URP e corrigir o pivot da porta.
```

---

# 3. Auditoria Synty — cenário

## Caminho auditado

`game/Assets/Synty/`

## 3.1 Packs localizados

| Pack | Caminho | Versão | Licença localizada? | Importa sem erros? |
|---|---|---|---|---|
| PolygonCity | | | | |
| PolygonGeneric | | | | |
| Outros | | | | |

## 3.2 Módulos úteis por parada

### Parada 1 — Elm and Depot

| Asset | Caminho | Função | Reutilizar/adaptar | Observação |
|---|---|---|---|---|
| | | | | |

### Parada 2 — Birch Avenue

| Asset | Caminho | Função | Reutilizar/adaptar | Observação |
|---|---|---|---|---|
| | | | | |

### Parada 3 — Ravenswood Elementary

| Asset | Caminho | Função | Reutilizar/adaptar | Observação |
|---|---|---|---|---|
| | | | | |

## 3.3 Materiais e performance

| Verificação | Resultado | Evidência | Ação |
|---|---|---|---|
| Materiais URP | | | |
| Quantidade de materiais únicos | | | |
| LODs disponíveis | | | |
| Colliders adequados | | | |
| Escala modular consistente | | | |
| Prefabs quebrados | | | |
| Texturas ausentes | | | |

## 3.4 Decisão

| Categoria | Decisão | Justificativa |
|---|---|---|
| Estradas/calçadas | | |
| Casas | | |
| Área industrial | | |
| Escola | | |
| Vegetação | | |
| Props urbanos | | |

---

# 4. Auditoria Street Vehicles Pack

## Caminho auditado

`game/Assets/Vladislav Simakov/`

| Asset | Caminho | Importa? | Material URP? | LOD | Uso na slice | Decisão |
|---|---|---|---|---|---|---|
| | | | | | | |

Verificações:

- [ ] Veículos não competem visualmente com o Bus 104.
- [ ] Colliders simplificados.
- [ ] Nenhum veículo usa materiais excessivamente realistas fora do style guide.
- [ ] Uso restrito a composição, orientação e bloqueio visual.

---

# 5. Necessidades próprias do Bus 104

Preencher após a auditoria do ônibus existente.

| Componente | Já existe? | Qualidade | Estratégia | Dependências | Prioridade |
|---|---|---|---|---|---|
| Painel | | | | | P0 |
| Rádio | | | | | P0 |
| Panel Lock | | | | | P0 |
| Retrovisor | | | | | P0 |
| Porta dianteira | | | | | P0 |
| Cinco fileiras | | | | | P0 |
| Número 104 | | | | | P1 |
| Cartão da rota | | | | | P1 |
| Adesivo Precious Cargo | | | | | P1 |
| Banco molhado | | | | | P1 |

---

# 6. Auditoria de personagens existentes

## 6.1 Crianças vivas

| Asset | Caminho | Rig | Animações | Estilo compatível? | Licença | Decisão |
|---|---|---|---|---|---|---|
| | | | | | | |

Critérios:

- [ ] Proporção infantil apropriada.
- [ ] Um único rig pode ser compartilhado.
- [ ] Permite variações de material.
- [ ] Possui ou aceita animações de esperar, caminhar, subir, sentar e descer.
- [ ] Não é detalhado ou realista demais para o style guide.

Decisão:

- [ ] Reutilizar base existente.
- [ ] Adaptar base existente.
- [ ] Adquirir um único mesh-base.
- [ ] Produzir mesh-base simples.
- [ ] Usar placeholder até o gate visual.

## 6.2 Emma

Confirmar se já existe algum asset-base apropriado:

| Asset-base | Caminho/origem | Silhueta útil? | Rig útil? | Licença | Decisão |
|---|---|---|---|---|---|
| | | | | | |

Requisitos mínimos do placeholder autoral:

- [ ] Criança de aproximadamente dez anos.
- [ ] Silhueta distinta dos passageiros vivos.
- [ ] Starter jacket.
- [ ] Jeans acid wash.
- [ ] Tênis com velcro.
- [ ] Forma angular/incompleta.
- [ ] Mão alcança o painel.
- [ ] Material azul-cinza translúcido.
- [ ] Olhos sem pupila.
- [ ] Aparecimento e desaparecimento por corte.

Decisão:

- [ ] Adaptar base existente.
- [ ] Produzir do zero.
- [ ] Adquirir base e adaptar.
- [ ] Placeholder geométrico temporário.

## 6.3 Thomas e demais fantasmas

- [ ] Confirmado que Thomas não precisa de modelo na Build 0.1.0.
- [ ] Confirmado que Marcus, Grace e Oliver permanecem adiados.
- [ ] Nenhum tempo de produção foi alocado prematuramente a esses modelos.

---

# 7. Auditoria de shaders, VFX e iluminação

| Sistema | Estado atual | Compatibilidade URP | Custo observado | Decisão |
|---|---|---|---|---|
| Névoa | | | | |
| Material translúcido de Emma | | | | |
| Fresnel | | | | |
| Pós-processamento de tensão | | | | |
| Luz da manhã | | | | |
| Afternoon Shift | | | | |
| Faróis | | | | |
| Retrovisor | | | | |

Problemas de overdraw, sombras ou materiais:

| Problema | Evidência | Severidade | Correção proposta |
|---|---|---|---|
| | | | |

---

# 8. Registro de licenças

| Asset/pack | Origem | Autor | Licença | Evidência localizada | Redistribuição isolada? | Ação |
|---|---|---|---|---|---|---|
| School Bus | | | | | | |
| PolygonCity | Unity Asset Store | Synty Studios | Asset Store EULA | | Não | |
| PolygonGeneric | Unity Asset Store | Synty Studios | Asset Store EULA | | Não | |
| Street Vehicles Pack | Unity Asset Store | Vladislav Simakov | Asset Store EULA | | Não | |
| Outros | | | | | | |

---

# 9. Matriz final de sourcing

| Asset da slice | Estratégia final | Origem/base | Trabalho necessário | Bloqueio | Responsável |
|---|---|---|---|---|---|
| Bus 104 exterior | | | | | |
| Bus 104 interior | | | | | |
| Painel | | | | | |
| Rádio | | | | | |
| Panel Lock | | | | | |
| Retrovisor | | | | | |
| Porta | | | | | |
| Parada 1 | | | | | |
| Parada 2 | | | | | |
| Escola | | | | | |
| Criança base | | | | | |
| Emma | | | | | |
| Stone placeholder | | | | | |
| Névoa | | | | | |
| UI visual | | | | | |

---

# 10. Bugs e bloqueios encontrados

| ID | Descrição | Asset/sistema | Severidade | Reproduzível? | Próxima ação |
|---|---|---|---|---|---|
| ART-001 | | | | | |

---

# 11. Resultado da auditoria

## O que pode ser reutilizado imediatamente

```text
Liste apenas assets confirmados no Editor.
```

## O que precisa ser adaptado

```text
Liste o asset, a mudança e o motivo.
```

## O que precisa ser produzido

```text
Liste somente lacunas confirmadas.
```

## O que precisa ser adquirido

```text
Nenhuma compra deve aparecer aqui sem uma lacuna confirmada e justificativa.
```

## O que permanece placeholder

```text
Registre até qual gate o placeholder é aceitável.
```

## O que foi adiado

```text
Inclua assets fora da vertical slice.
```

## Status final

Escolha um:

- [ ] `APROVADO PARA BLOCKOUT`
- [ ] `APROVADO COM RESSALVAS`
- [ ] `BLOQUEADO POR IMPORTAÇÃO/LFS`
- [ ] `BLOQUEADO POR LICENÇA`
- [ ] `BLOQUEADO POR ASSET ESSENCIAL AUSENTE`

Resumo executivo:

```text
Descreva a menor rota segura para iniciar a produção visual da Build 0.1.0.
```

---

# 12. Evidências para anexar à issue #75

- [ ] Captura do Console baseline.
- [ ] Captura do Bus 104 no Scene View.
- [ ] Captura do cockpit/corredor.
- [ ] Captura dos módulos Synty selecionados.
- [ ] Captura dos veículos úteis.
- [ ] Captura do personagem-base, quando existir.
- [ ] Captura do teste de material de Emma.
- [ ] Lista de arquivos/prefabs auditados.
- [ ] Registro de licenças.
- [ ] Matriz final de sourcing.
- [ ] Decisão Gate B0.

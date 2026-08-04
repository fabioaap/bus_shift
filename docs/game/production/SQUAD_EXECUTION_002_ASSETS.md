# Execução 002 do Game Development Squad — Assets da Build 0.1.0

## Identificação

| Campo | Valor |
|---|---|
| Projeto | Bus Shift |
| Branch | `agent/aiox-5-3-game-squad` |
| Fase | Vertical Slice — produção e integração de assets |
| Data | 4 de agosto de 2026 |
| Issue-mãe | #75 |
| Estado | Preparação executada; inspeção binária pendente no Unity |

## Agentes invocados

### `game-chief`

1. Manteve o escopo restrito à Build 0.1.0.
2. Impediu produção antecipada dos Dias 2 a 5.
3. Decompôs o pacote visual em quatro gates executáveis.
4. Determinou que compra ou remodelagem completa só ocorre depois da auditoria dos assets existentes.

### `visual-art-director`

1. Consolidou o pipeline `reutilizar → adaptar → produzir/adquirir`.
2. Preservou o style guide low poly, URP e horror PS1/VHS.
3. Priorizou Bus 104, rota curta, passageiro-base e Emma.
4. Manteve Thomas apenas como presença sonora/ambiental na vertical slice.
5. Definiu critérios visuais e evidências para ônibus, cenário, personagens e materiais.

### `unity-gameplay-engineer`

1. Inspecionou a convenção das ferramentas existentes em `BusShift.EditorTools`.
2. Criou `Build010AssetAuditReporter.cs` e seu `.meta`.
3. Adicionou o menu `Bus Shift > Assets > Generate Build 0.1.0 Audit Report`.
4. Implementou auditoria somente leitura de:
   - raízes de assets;
   - modelos, prefabs, materiais, texturas, áudio e animações;
   - meshes e skinned meshes;
   - scripts ausentes em prefabs;
   - shaders ausentes ou de erro;
   - ponteiros Git LFS não resolvidos;
   - candidatos por nome/caminho para ônibus, cenário e personagens.
5. Configurou a geração de `docs/game/art/BUILD_0_1_0_ASSET_AUDIT_GENERATED.md`.

### `game-designer`

1. Limitou o cenário a uma rota curta e três paradas.
2. Preservou as identidades de Elm and Depot, Birch Avenue e Ravenswood Elementary.
3. Determinou que a cidade completa permanece fora da vertical slice.
4. Vinculou o blockout visual ao loop da issue #73.

### `lore-architect`

1. Preservou Dale Mercer como protagonista canônico.
2. Preservou Emma como primeira manifestação clara.
3. Impediu a criação de um modelo visível de Thomas nesta build.
4. Limitou os personagens visuais a passageiros vivos e Emma.

### `game-qa-balance`

1. Exigiu evidências de Console, capturas, caminhos e licenças.
2. Definiu zero ponteiros LFS, zero shader rosa e zero P0/P1 como gates.
3. Exigiu validação posterior em Play Mode e Development Build Windows.
4. Impediu que inventário automático fosse tratado como aprovação visual.

## Entregáveis desta rodada

1. `docs/game/art/BUILD_0_1_0_ASSET_PRODUCTION_PIPELINE.md`
2. `docs/game/art/BUILD_0_1_0_ASSET_AUDIT_TEMPLATE.md`
3. `game/Assets/_Project/Editor/Build010AssetAuditReporter.cs`
4. `game/Assets/_Project/Editor/Build010AssetAuditReporter.cs.meta`
5. Issue #77 — auditoria e adaptação do Bus 104.
6. Issue #78 — blockout da rota e das três paradas.
7. Issue #79 — passageiro-base e protótipo visual de Emma.
8. Issue #80 — materiais URP, neblina, VFX e licenças.

## Sequência de gates

### Gate B0 — Integridade e Bus 104

Responsável principal: #77.

1. Git LFS completo.
2. Projeto compila.
3. Auditoria automática executada.
4. Modelo/prefab do ônibus inspecionado.
5. Decisão `reutilizar`, `adaptar`, `substituir` ou `produzir` registrada.

Nenhuma compra ou remodelagem completa começa antes deste gate.

### Gate B1 — Kit de cenário

Responsável principal: #78.

1. Módulos Synty selecionados.
2. Rota curta definida.
3. Três paradas distinguíveis.
4. Espaço de circulação do ônibus validado.
5. Vendor assets preservados.

### Gate B2 — Personagens mínimos

Responsável principal: #79.

1. Passageiro-base decidido.
2. Variações de material definidas.
3. Emma possui silhueta, material e rig mínimo.
4. Emma é distinguível das crianças vivas.
5. Thomas continua sem modelo visível.

### Gate B3 — Integração visual URP

Responsável principal: #80.

1. Zero shader rosa.
2. Materiais derivados organizados em `_Project`.
3. Manhã e `Afternoon Shift` distinguíveis.
4. Neblina e VFX de tensão legíveis.
5. Licenças registradas.
6. Resultado validado em build Windows.

## Comando operacional no Unity

Depois de abrir a pasta `game` no Unity `6000.3.10f1` e aguardar a compilação:

`Bus Shift > Assets > Generate Build 0.1.0 Audit Report`

Saída esperada:

`docs/game/art/BUILD_0_1_0_ASSET_AUDIT_GENERATED.md`

A ferramenta deve apenas ler o AssetDatabase e escrever o relatório. Ela não deve alterar import settings, materiais, prefabs, cenas ou vendor assets.

## Prompt para Unity Essentials + MCP

```text
@Unity Essentials

Consulte:
- docs/game/art/BUILD_0_1_0_ASSET_PRODUCTION_PIPELINE.md
- docs/game/art/BUILD_0_1_0_ASSET_AUDIT_TEMPLATE.md
- docs/game/production/SQUAD_EXECUTION_002_ASSETS.md
- issue #77

Valide primeiro a conexão com o Unity MCP e o Console baseline.
Não faça alterações antes de confirmar que o projeto compila.

Em seguida:
1. confirme a branch agent/aiox-5-3-game-squad;
2. confirme que não existem ponteiros Git LFS pendentes;
3. execute Bus Shift > Assets > Generate Build 0.1.0 Audit Report;
4. leia o relatório gerado;
5. inspecione Assets/School Bus/ em modo somente leitura;
6. avalie exterior, interior, pivôs, porta, rodas, colliders, materiais e escala;
7. preencha a decisão do Gate B0;
8. registre capturas e resultado na issue #77.

Não compre assets, não edite vendor assets e não avance ao blockout da rota se houver erro de compilação, ponteiro LFS pendente ou defeito P0/P1.
```

## Limitações e estado de validação

1. O código da ferramenta foi revisado estaticamente, mas ainda não foi compilado no Unity Editor.
2. Nenhum asset binário foi aberto ou aprovado nesta conversa.
3. Nenhum modelo, material, prefab ou cena foi modificado.
4. O relatório gerado ainda não existe porque depende da execução local no Unity.
5. A vertical slice ainda não é considerada jogável.

## Próxima decisão

A próxima decisão objetiva é o Gate B0 do Bus 104. Quando a auditoria local for executada, o squad deve escolher entre:

1. adaptar o ônibus existente;
2. combinar exterior existente com interior próprio;
3. substituir por outro asset licenciado;
4. produzir um modelo próprio no Blender.

A escolha deve se basear no relatório e na inspeção real, não apenas no nome das pastas do repositório.

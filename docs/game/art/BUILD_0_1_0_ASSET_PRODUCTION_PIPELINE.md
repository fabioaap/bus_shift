# Bus Shift — Pipeline de Produção de Assets da Build 0.1.0

## Identificação

| Campo | Valor |
|---|---|
| Projeto | Bus Shift |
| Entrega | Build 0.1.0 — vertical slice do Dia 1 |
| Branch | `agent/aiox-5-3-game-squad` |
| Engine | Unity `6000.3.10f1` |
| Render pipeline | URP `17.0.3` |
| Plataforma-alvo | Windows 10/11, 64-bit |
| Estado | Planejamento executável; auditoria binária pendente na Unity |
| Issue principal | #75 |

## 1. Objetivo

Estabelecer como ônibus, cenário, personagens, props, materiais, animações e efeitos serão obtidos, adaptados, produzidos, integrados e validados para a primeira vertical slice jogável.

O objetivo não é produzir toda a arte final do jogo. A prioridade é entregar uma slice visualmente coerente, legível e executável, sem bloquear o desenvolvimento por assets finais que pertencem aos Dias 2 a 5.

## 2. Fontes de verdade

Este pipeline deve ser executado em conjunto com:

1. `docs/game/art/style-guide.md`
2. `docs/game/art/BUILD_0_1_0_VISUAL_BRIEF.md`
3. `docs/game/production/BUILD_0_1_0_VERTICAL_SLICE.md`
4. `docs/game/production/UNITY_EXECUTION_RUNBOOK_0_1_0.md`
5. `docs/game/production/SQUAD_EXECUTION_001.md`
6. `docs/game/testing/BUILD_0_1_0_TEST_MATRIX.md`

Em caso de conflito narrativo, prevalecem as decisões consolidadas em `SQUAD_EXECUTION_001.md`:

1. Dale Mercer é o protagonista canônico.
2. Emma é o primeiro contato sobrenatural claro.
3. Thomas aparece apenas como prenúncio sonoro no rádio no Dia 1.
4. Marcus, Grace, Oliver e a manifestação completa de Thomas ficam fora da Build 0.1.0.
5. A cena técnica `Day1Night` deve ser apresentada como `Afternoon Shift`.

O inventário histórico `docs/game/art/assets.md` ainda é útil para budgets, nomenclatura e referências, mas contém escopo e nomes anteriores. Nenhum item deve entrar na slice apenas porque está marcado como MVP naquele documento.

## 3. Estratégia de sourcing

Cada asset deve receber exatamente uma estratégia primária:

| Estratégia | Uso |
|---|---|
| Reutilizar | O asset já existe no projeto e atende a função com ajustes pequenos |
| Adaptar | O asset existente é a base, mas precisa de mudanças em Blender, materiais, rig, pivôs ou hierarquia |
| Produzir | Asset próprio necessário para identidade, gameplay ou legibilidade |
| Adquirir | Comprar ou licenciar somente após uma lacuna confirmada pela auditoria |
| Placeholder | Representação temporária coerente para validar mecânica ou composição |
| Adiar | Não pertence ao escopo da vertical slice |

### Regras

1. Nenhuma compra é autorizada antes da auditoria local dos assets existentes.
2. Nenhum asset final deve bloquear a fundação navegável.
3. Placeholder deve respeitar escala, silhueta, paleta e função do asset final.
4. Asset de terceiro deve possuir origem e licença registradas.
5. O projeto não deve redistribuir isoladamente assets da Unity Asset Store.
6. Mudanças em meshes de terceiros devem gerar derivados dentro de uma pasta própria do projeto, preservando o original.
7. Thomas não recebe modelo 3D na Build 0.1.0.
8. Os outros fantasmas não recebem produção final antes da aprovação da slice.

## 4. Evidência disponível no repositório

O README registra como bases existentes:

1. `game/Assets/School Bus/` — modelo de ônibus escolar.
2. `game/Assets/Synty/` — PolygonCity e PolygonGeneric.
3. `game/Assets/Vladislav Simakov/` — Street Vehicles Pack.
4. `game/Assets/_Project/` — código, cenas e prefabs próprios.

Esses caminhos são candidatos de reutilização. Eles ainda não devem ser considerados aprovados até a sessão local confirmar:

1. Git LFS completamente baixado.
2. Ausência de arquivos ponteiro no lugar dos binários.
3. Importação sem erros na Unity.
4. Materiais compatíveis com URP.
5. Escala, pivôs, colliders e hierarquia utilizáveis.
6. Licença identificável.
7. Performance adequada para a slice.

## 5. Matriz de decisão da Build 0.1.0

### 5.1 Ônibus 104

| Componente | Estratégia inicial | Entrega da slice | Finalização posterior |
|---|---|---|---|
| Carroceria exterior | Adaptar `Assets/School Bus/` | Silhueta correta, amarelo oxidado, número 104, faróis e rodas utilizáveis | Danos finais, decalques e LOD revisado |
| Interior | Adaptar ou produzir módulos | Cabine, corredor e pelo menos cinco fileiras | Interior completo e variações narrativas |
| Painel | Produzir/adaptar | Porta, rádio, Panel Lock e feedback de estado | Instrumentação detalhada e desgaste final |
| Porta dianteira | Adaptar | Pivot, collider, animação e estado legíveis | Áudio e acabamento final |
| Retrovisor | Produzir | Campo visual do corredor e suporte a Emma | Shader/reflexo final e efeitos sobrenaturais avançados |
| Rádio | Produzir/adaptar | Canal visível, canal dois e feedback de estática | Modelo e materiais finais |
| Panel Lock | Produzir | Controle identificável em até cinco segundos | Animação, VFX e acabamento final |
| Bancos | Reutilizar/instanciar | Cinco fileiras funcionais | Conjunto completo do ônibus |
| Mãos de Dale | Placeholder/adiar | Podem permanecer ausentes na fundação; usar apenas se não bloquearem câmera e interação | Modelo, rig e animações finais |

### 5.2 Cenário e rota

| Componente | Estratégia inicial | Entrega da slice | Finalização posterior |
|---|---|---|---|
| Estradas e calçadas | Reutilizar Synty | Rota curta, clara e dirigível | Variações próprias e acabamento de Ravenswood |
| Prédios residenciais | Reutilizar Synty | Silhuetas e fachadas coerentes | Texturas, placas e storytelling ambiental |
| Área industrial/Depot | Reutilizar + adaptar | Parada 1 distinguível | Props próprios e desgaste final |
| Birch Avenue | Reutilizar + compor | Parada 2 com casas antigas e árvore sem folhas | Identidade arquitetônica própria |
| Ravenswood Elementary | Adaptar ou produzir fachada modular | Parada 3 reconhecível como escola | Escola final e áreas complementares |
| Vegetação | Reutilizar/adaptar | Verde murcho e árvores de baixa densidade | Conjunto próprio por estação |
| Veículos estacionados | Reutilizar Street Vehicles Pack | Fundo e bloqueio visual controlado | Variações de material e placas locais |
| Sinalização de parada | Produzir | Três marcadores distintos e legíveis | Props finais de Ravenswood |
| Névoa | Produzir na URP | Visibilidade reduzida sem esconder a rota | Ajuste por qualidade e performance |
| Iluminação manhã | Compor na Unity | Fria, difusa e opressiva | Baked/híbrida final |
| Iluminação Afternoon Shift | Compor na Unity | Sol baixo, névoa e interior mais escuro | Polish e performance |

### 5.3 Personagens

| Personagem | Estratégia inicial | Entrega da slice | Finalização posterior |
|---|---|---|---|
| Emma | Produzir placeholder autoral | Silhueta angular, aparência sobrenatural, mão alcançando painel, aparecimento por corte | Modelo, rig, materiais e animação finais |
| Thomas | Sem modelo 3D | Rádio, estática, sussurro e reflexo impossível não identificável | Modelo apenas quando entrar no gameplay futuro |
| Crianças vivas | Reutilizar/adquirir base ou produzir mesh simples | Um mesh base com variações de material; esperar, embarcar, sentar e desembarcar | Diversidade de silhueta e animação |
| Harrison Stone | Placeholder | Silhueta ou busto apenas se a cena exigir presença física | Modelo e rig finais |
| Dale Mercer | Câmera em primeira pessoa | Corpo completo fora do escopo; mãos opcionais | Corpo, mãos e variações narrativas finais |
| Marcus, Grace e Oliver | Adiar | Nenhum modelo necessário | Produção após aprovação da vertical slice |

### 5.4 Props e feedback

| Asset | Estratégia | Critério mínimo |
|---|---|---|
| Cartão da rota | Produzir 2D/3D simples | Texto e sequência das três paradas legíveis |
| Adesivo `PRECIOUS CARGO ON BOARD` | Produzir decal | Visível sem dominar a composição |
| Banco molhado | Adaptar material | Leitura clara no `SliceEnding` |
| Botão vermelho de Emma | Produzir | Ponto de atenção inequívoco |
| Marcadores de objetivo | UI temporária | Não competir com a estrada |
| VFX de tensão | URP/post-processing | Progressão contínua, não em saltos |
| VFX de Emma | Shader + partículas limitadas | Diferenciar Emma das crianças vivas |

## 6. Pipeline técnico por asset 3D

### 6.1 Estrutura de trabalho

Os originais de terceiros permanecem intocados. Derivados próprios devem ser organizados em:

```text
game/Assets/_Project/Art/
├── Characters/
│   ├── Emma/
│   ├── Passengers/
│   └── Dale/
├── Environment/
│   ├── Depot/
│   ├── BirchAvenue/
│   ├── School/
│   └── Route/
├── Vehicles/
│   └── Bus104/
├── Props/
├── Materials/
├── Textures/
├── VFX/
└── SourceReferences/
```

Arquivos fonte editáveis de Blender não precisam ser importados diretamente pela Unity. O contrato recomendado é:

1. Fonte DCC: `.blend`, armazenada fora de `Assets/` ou em diretório de produção explicitamente ignorado pela Unity.
2. Entrega para Unity: `.fbx` com transforms aplicados.
3. Texturas: `.png`, `.tga` ou formato já aprovado pelo projeto.
4. Materiais finais: materiais URP dentro de `Assets/_Project/Art/Materials/`.
5. Prefab de integração: dentro de `Assets/_Project/Prefabs/` ou estrutura equivalente já existente.

Não introduzir glTF/GLB como contrato principal sem adicionar e validar um importer específico; o projeto atual não registra esse pacote.

### 6.2 Checklist de modelagem

1. Escala em metros e compatível com a Unity.
2. Rotação e escala aplicadas antes da exportação.
3. Forward e up consistentes com o preset de FBX do projeto.
4. Pivôs posicionados para gameplay.
5. Hierarquia com nomes estáveis.
6. Portas, rodas, volante, ponteiros, botões e outros elementos animáveis em objetos separados.
7. Sem faces internas desnecessárias, salvo quando visíveis em primeira pessoa.
8. Normais revisadas.
9. UVs sem sobreposição acidental.
10. Colisão separada da geometria visual quando necessário.
11. Materiais reutilizados; evitar um material por objeto pequeno.
12. Meshes repetidos configurados para instanciação.

### 6.3 Nomenclatura

```text
M_Bus104_Exterior
M_Bus104_Interior
M_Bus104_DoorFront
M_Emma_Placeholder
M_Child_Base
P_Bus104_Playable
P_Emma_Day1
P_Child_Variant_A
MAT_Bus104_Exterior
MAT_Ghost_Emma
T_Bus104_BaseColor
T_Bus104_Mask
A_Emma_ReachPanel
COL_Bus104_Body
```

Convenções finais devem respeitar qualquer padrão já existente detectado no onboarding local.

### 6.4 Importação na Unity

1. Criar preset de importação por categoria.
2. Confirmar escala e orientação no Inspector.
3. Desativar importação de câmeras e luzes não necessárias.
4. Configurar rig apenas em personagens animados.
5. Gerar ou importar colliders explícitos.
6. Converter materiais para URP quando necessário.
7. Criar prefab próprio sem modificar o prefab original do vendor.
8. Registrar referências no Inspector.
9. Salvar `.meta` junto ao asset.
10. Revisar diff antes do commit.

## 7. Produção do Bus 104

### Gate B0 — auditoria do modelo existente

Verificar no modelo de `Assets/School Bus/`:

1. Exterior e interior são meshes separados?
2. A porta dianteira possui geometria e pivot utilizáveis?
3. O interior possui corredor e espaço para câmera?
4. Bancos podem ser reutilizados ou instanciados?
5. Rodas e volante são separados?
6. Materiais importam corretamente na URP?
7. O modelo aceita collider sem comportamento instável?
8. A licença permite uso dentro do jogo?

Resultado obrigatório: `usar`, `adaptar profundamente` ou `substituir`.

### Gate B1 — versão navegável

1. Escala correta.
2. Câmera na posição do motorista.
3. Interior sem clipping crítico.
4. Porta funcionando.
5. Retrovisor mostrando corredor.
6. Painel com rádio e Panel Lock identificáveis.
7. Collider principal e rodas funcionais.
8. Materiais temporários dentro da paleta.

### Gate B2 — versão art-ready da slice

1. Número 104.
2. Amarelo desbotado e ferrugem controlada.
3. Exterior legível no menu.
4. Interior legível pela manhã e no Afternoon Shift.
5. Banco molhado e detalhes narrativos do final.
6. Sem material rosa ou shader quebrado.
7. Performance dentro do orçamento da cena.

## 8. Produção do cenário

### 8.1 Blockout

O blockout deve usar apenas volumes e módulos necessários para:

1. Completar a rota.
2. Identificar três paradas.
3. Testar curvas, frenagem e posição da porta.
4. Verificar campo de visão da cabine.
5. Medir densidade de névoa.
6. Validar manhã e Afternoon Shift.

### 8.2 Dressing modular

Depois da rota aprovada:

1. Substituir volumes por módulos Synty aprovados.
2. Manter a mesma malha de navegação e colliders sempre que possível.
3. Diferenciar cada parada por silhueta, não apenas por placa.
4. Usar veículos extras apenas como composição e orientação.
5. Evitar construir bairros que não aparecem na rota.

### 8.3 Identidade de Ravenswood

Peças próprias prioritárias:

1. Placas de Ravenswood.
2. Sinalização da escola.
3. Três placas de parada.
4. Adesivos e avisos do ônibus.
5. Texturas/decalques de desgaste.
6. Props narrativos vistos de perto.

## 9. Produção de Emma

### 9.1 Placeholder P0

O primeiro modelo pode ser simples, mas deve comunicar:

1. Criança de aproximadamente dez anos.
2. Silhueta diferente das crianças vivas.
3. Starter jacket, jeans acid wash e tênis com velcro como direção atual.
4. Forma angular e incompleta.
5. Uma mão capaz de alcançar o painel.
6. Material azul-cinza translúcido.
7. Olhos claros sem pupila.
8. Manifestação e desaparecimento por corte.

### 9.2 Rig mínimo

1. Root.
2. Pelvis/torso.
3. Cabeça.
4. Ombro, braço e mão usados na interação.
5. Pernas apenas se necessárias para a pose.

Não produzir rig facial complexo na Build 0.1.0. O sorriso pode ser resolvido por troca de mesh, pose, material ou poucos frames intencionalmente quebrados.

### 9.3 Animações mínimas

1. `A_Emma_Appear`.
2. `A_Emma_IdleBroken`.
3. `A_Emma_ReachPanel`.
4. `A_Emma_SuccessCut`.
5. `A_Emma_Failure`.

## 10. Crianças vivas

### Estratégia

1. Um mesh base infantil.
2. Um rig compartilhado.
3. Materiais e acessórios simples para variação.
4. Instanciação do mesmo prefab-base.
5. Animações curtas e reutilizáveis.

### Animações mínimas

1. Esperar.
2. Caminhar.
3. Subir degrau.
4. Sentar.
5. Levantar.
6. Descer.

Na primeira integração, cápsulas ou bonecos simples podem validar navegação. Antes do playtest visual, devem ser substituídos pelo mesh base coerente.

## 11. Materiais, texturas e shaders

### Budgets iniciais da slice

| Categoria | Resolução inicial |
|---|---:|
| Ônibus exterior | 2048 px, preferencialmente atlas |
| Interior/cockpit | 2048 px |
| Emma | 512–1024 px |
| Criança base | 512–1024 px |
| Props próximos | 512–1024 px |
| Prédios de fundo | 512–1024 px compartilhados |
| UI/decalques | Conforme leitura real, sem excesso |

### Regras

1. Priorizar base color, masks e emissive simples.
2. Normal map apenas quando houver ganho visível próximo à câmera.
3. Compartilhar materiais entre variações de passageiros.
4. Usar MaterialPropertyBlock ou estratégia equivalente quando apropriado, sem duplicar materiais em runtime.
5. Medir overdraw dos materiais translúcidos de Emma e da névoa.
6. Evitar transparências grandes sobrepostas dentro do ônibus.
7. Garantir fallback visual quando pós-processamento estiver reduzido.

## 12. Colliders, LOD e performance

### Ônibus

1. Collider composto simples para carroceria.
2. Colliders separados para porta e interações.
3. Evitar MeshCollider não convexo em objeto dinâmico quando não necessário.
4. Rodas configuradas conforme o sistema veicular adotado pelo projeto.

### Ambiente

1. Colliders de prédios simplificados.
2. Props pequenos sem collider quando não interativos.
3. Occlusion e batching avaliados somente após a rota funcional.
4. LOD para prédios, árvores e veículos repetidos quando necessário.

### Personagens

1. CapsuleCollider ou solução equivalente.
2. Colisões de mãos e roupas apenas se exigidas pela mecânica.
3. Emma não deve usar collider físico complexo para aparecer no painel.

### Critérios preliminares

1. Sem spikes graves ao manifestar Emma.
2. Sem queda perceptível causada pela névoa.
3. Sem dezenas de materiais únicos no ônibus.
4. Sem luz dinâmica individual por passageiro.
5. Sem assets 4K na slice sem justificativa comprovada.

## 13. Licenças e proveniência

Criar ou atualizar um registro por asset contendo:

| Campo | Exemplo |
|---|---|
| Asset | `PolygonCity` |
| Origem | Unity Asset Store |
| Autor | Synty Studios |
| Licença | Asset Store EULA |
| Pasta original | `Assets/Synty/...` |
| Derivado próprio | `Assets/_Project/Art/Environment/...` |
| Pode redistribuir isoladamente? | Não |
| Uso na slice | Paradas e fundo urbano |
| Responsável pela verificação | Visual Art Director |

Assets gerados por IA precisam registrar:

1. Ferramenta/modelo usado.
2. Prompt ou referência principal quando relevante.
3. Data de geração.
4. Licença e direitos aplicáveis à conta usada.
5. Intervenções humanas realizadas.
6. Arquivo fonte e versão aprovada.

Nenhum output de IA deve ser importado automaticamente como asset final sem revisão de topologia, direitos, coerência visual e performance.

## 14. Ordem de execução

### Sprint A — auditoria e blockout

1. Executar `git lfs pull`.
2. Abrir o projeto na Unity.
3. Auditar `School Bus`, Synty e Street Vehicles.
4. Registrar assets utilizáveis e quebrados.
5. Criar blockout da rota com três paradas.
6. Validar escala do ônibus e câmera.
7. Criar placeholders de passageiros e Emma.
8. Testar manhã e Afternoon Shift.

### Sprint B — integração funcional

1. Adaptar Bus 104.
2. Integrar porta, rádio, Panel Lock e retrovisor.
3. Substituir rota blockout por módulos aprovados.
4. Integrar passageiros base.
5. Integrar Emma placeholder.
6. Adicionar névoa, materiais e iluminação temporários.
7. Rodar testes e smoke test.

### Sprint C — art-ready da vertical slice

1. Finalizar materiais temporários do Bus 104.
2. Finalizar fachada e paradas principais.
3. Melhorar Emma sem ampliar escopo.
4. Adicionar props narrativos.
5. Revisar UI, VFX e contraste.
6. Capturar manhã e Afternoon Shift.
7. Gerar Development Build Windows.
8. Corrigir apenas problemas P0/P1 e visuais que prejudiquem compreensão.

### Sprint D — decisão pós-playtest

1. Consolidar métricas das cinco sessões.
2. Identificar assets que mais impactaram compreensão e tensão.
3. Decidir quais placeholders merecem produção final.
4. Recalcular escopo dos Dias 2 a 5.
5. Não produzir o elenco sobrenatural completo antes dessa decisão.

## 15. Responsabilidades do squad

| Agente | Responsabilidade |
|---|---|
| `game-chief` | Escopo, dependências, gates e aprovação de mudança de estratégia |
| `visual-art-director` | Direção, inventário, sourcing, briefs, consistência e aprovação visual |
| `unity-gameplay-engineer` | Import, prefabs, materiais URP, colliders, animação e performance |
| `game-designer` | Legibilidade da rota, paradas, interações e campo de visão |
| `lore-architect` | Coerência narrativa de props, Emma, escola e sinais ambientais |
| `game-qa-balance` | Critérios de aceite, regressões visuais e validação na build |

## 16. Gate de aprovação por asset

Um asset só passa para `integrado` quando:

1. Possui origem e licença identificadas.
2. Importa sem erro.
3. Escala e orientação estão corretas.
4. Pivôs de interação funcionam.
5. Materiais são compatíveis com URP.
6. Não há referências quebradas.
7. O prefab está salvo em pasta própria do projeto.
8. O asset cumpre sua função de gameplay.
9. O custo visual/performance é aceitável.
10. Há captura ou evidência no contexto da cena.

Um asset só passa para `aprovado para a Build 0.1.0` quando:

1. Funciona no executável Windows.
2. Não introduz bug P0/P1.
3. É compreendido nos playtests.
4. Respeita o style guide.
5. Não promete conteúdo fora da slice.

## 17. Evidências obrigatórias

Ao concluir cada sprint, anexar à issue #75:

1. Captura do inventário local.
2. Lista `reutilizar/adaptar/produzir/adquirir/placeholder/adiar`.
3. Capturas do Bus 104 exterior e cockpit.
4. Capturas das três paradas.
5. Captura de Emma em contexto.
6. Lista de materiais e shaders usados.
7. Registro de licenças.
8. Problemas de importação e decisões tomadas.
9. Métricas básicas de performance na cena.
10. Link para a build ou evidência do run que a produziu.

## 18. Próxima ação local

Na próxima sessão com Unity MCP:

1. Confirmar que o editor está no projeto `game` e na branch correta.
2. Verificar Console antes de limpar qualquer mensagem.
3. Confirmar Git LFS completo.
4. Inspecionar as três pastas de assets registradas no README.
5. Produzir uma tabela factual com nome, caminho, tipo, materiais, estado de importação, licença e uso possível.
6. Testar o modelo de ônibus em uma cena vazia sem salvar alterações destrutivas no original.
7. Registrar o resultado do Gate B0.
8. Somente então decidir se será necessário comprar ou modelar um ônibus substituto.

## 19. Decisão atual

A Build 0.1.0 seguirá uma estratégia híbrida:

1. Reutilização dos packs existentes para acelerar cenário e fundo urbano.
2. Adaptação do modelo de ônibus existente como primeira opção para o Bus 104.
3. Produção própria dos elementos que carregam mecânica e identidade: painel, Panel Lock, retrovisor, sinalização, Emma e props narrativos.
4. Um único mesh base para passageiros vivos.
5. Thomas apenas em áudio e feedback indireto.
6. Compra de novos assets somente após auditoria local demonstrar uma lacuna real.
7. Produção final completa condicionada aos resultados dos playtests da vertical slice.

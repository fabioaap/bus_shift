# Task: Audit Game Assets

## task_name
Auditar assets gráficos e preparar decisão de reutilização, adaptação, aquisição ou produção

## status
ready

## responsible_executor
`@visual-art-director`

## contributors

1. `@unity-gameplay-engineer`
2. `@game-designer`
3. `@lore-architect`
4. `@game-qa-balance`

## input

1. Objetivo e escopo da build.
2. Style guide e brief visual.
3. Inventário de assets do repositório.
4. Projeto Unity importado com Git LFS completo.
5. Requisitos de gameplay, narrativa, URP, performance e plataforma.
6. Licenças e restrições dos assets de terceiros.

## output

1. Relatório de auditoria baseado no AssetDatabase e inspeção do Editor.
2. Matriz `reutilizar`, `adaptar`, `adquirir`, `produzir` ou `fora do escopo`.
3. Lista de assets aprovados e caminhos exatos.
4. Lista de problemas de importação, escala, pivot, rig, materiais, colliders e licença.
5. Gates de saída para veículo, cenário, personagens e integração visual.
6. Backlog de produção com dependências e evidências.

## action_items

1. Confirmar a branch e o commit analisados.
2. Confirmar a versão da Unity e o render pipeline.
3. Confirmar que o Git LFS está completo e que não existem ponteiros pendentes.
4. Registrar o Console baseline antes de qualquer mudança.
5. Executar a ferramenta de inventário do projeto quando disponível.
6. Inspecionar modelos, prefabs, materiais, texturas, animações, áudio e VFX relevantes.
7. Verificar escala, orientação, pivôs, hierarquia, rig, colliders e referências.
8. Verificar compatibilidade dos materiais com o render pipeline ativo.
9. Separar assets first-party de vendor assets.
10. Proibir alterações diretas em vendor assets; criar derivados no diretório do projeto.
11. Registrar origem, licença e restrições de redistribuição.
12. Classificar cada necessidade da build.
13. Criar critérios de aceite e evidências para cada pacote de produção.
14. Interromper a execução diante de erro de compilação, asset corrompido, ponteiro LFS, licença desconhecida ou defeito P0/P1.

## decision_matrix

| Classificação | Uso |
|---|---|
| `reutilizar` | Asset atende sem mudança estrutural; apenas configuração first-party |
| `adaptar` | Asset atende parcialmente e exige prefab/material/rig/collider derivado |
| `adquirir` | Lacuna comprovada e compra possui licença, custo e ganho claros |
| `produzir` | Asset é central à identidade ou nenhum candidato atende |
| `fora do escopo` | Não é necessário para a build atual |

## acceptance_criteria

1. O relatório distingue inventário automático de aprovação visual.
2. Cada asset aprovado possui caminho exato e origem conhecida.
3. Nenhuma compra é recomendada sem auditar o conteúdo existente.
4. Nenhum vendor asset é modificado diretamente.
5. Todos os materiais utilizados são compatíveis com o render pipeline.
6. Assets interativos possuem plano de pivot, collider, rig e prefab.
7. Personagens possuem silhueta, escala, material e animações mínimas definidas.
8. O cenário possui kit modular e limites de escopo.
9. O backlog possui ordem de dependência e gates verificáveis.
10. Resultados bloqueados ou não verificados são explicitamente marcados.

## quality_gate

A auditoria só é aprovada quando existe evidência suficiente para decidir o que será reaproveitado, adaptado, comprado ou produzido sem depender de suposições baseadas apenas em nomes de pastas ou thumbnails.

## bus_shift_build_0_1_0

1. Executar `Bus Shift > Assets > Generate Build 0.1.0 Audit Report`.
2. Validar primeiro o Gate B0 do Bus 104 na issue #77.
3. Liberar o kit de cenário da issue #78 depois da integridade básica.
4. Liberar passageiro-base e Emma na issue #79 depois da decisão de bases existentes.
5. Concluir materiais URP, neblina, VFX e licenças na issue #80.
6. Manter Thomas sem modelo visível e os demais fantasmas fora da vertical slice.

## handoff

1. Entregar assets aprovados e backlog ao `@game-chief`.
2. Entregar integração técnica ao `@unity-gameplay-engineer`.
3. Entregar critérios visuais ao `@visual-art-director`.
4. Entregar casos de validação ao `@game-qa-balance`.

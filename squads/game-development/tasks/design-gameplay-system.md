# Task: Design Gameplay System

## task_name
Projetar uma mecânica ou sistema jogável

## status
ready

## responsible_executor
`@game-designer`

## input

1. Problema ou fantasia desejada.
2. Contexto do core loop.
3. Restrições técnicas e de produção.
4. Dependências narrativas e visuais.

## output

Especificação pronta para implementação e teste.

## action_items

1. Definir objetivo e decisão do jogador.
2. Mapear inputs, estados e transições.
3. Definir regras e exceções.
4. Descrever telegraphing, feedback e contrajogo.
5. Definir sucesso, falha e recuperação.
6. Expor variáveis de tuning.
7. Mapear integração com sistemas existentes.
8. Definir casos extremos.
9. Escrever critérios de aceite observáveis.
10. Preparar handoff para engenharia, arte, lore e QA.

## acceptance_criteria

1. A mecânica altera uma decisão real do jogador.
2. O jogador recebe sinais suficientes para aprender a regra.
3. Estados e transições são inequívocos.
4. Variáveis de balanceamento estão separadas da lógica.
5. Casos de falha são testáveis.
6. O documento não exige interpretação criativa do programador.

## quality_gate

A especificação deve permitir implementação sem inventar requisitos durante o código.

## handoff

Entregar ao `@unity-gameplay-engineer` e ao `@game-qa-balance`.

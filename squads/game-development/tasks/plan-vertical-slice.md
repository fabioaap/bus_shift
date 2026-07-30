# Task: Plan Vertical Slice

## task_name
Planejar uma vertical slice jogável

## status
ready

## responsible_executor
`@game-chief`

## input

1. Auditoria atualizada do projeto.
2. Visão do jogo.
3. Principais riscos técnicos e criativos.
4. Recursos humanos e assets disponíveis.

## output

Plano de uma build pequena, completa e demonstrável, com escopo, exclusões, responsáveis, dependências e critérios de aceite.

## action_items

1. Definir a pergunta que a build precisa responder.
2. Escolher um recorte que contenha início, loop, falha e conclusão.
3. Selecionar apenas sistemas indispensáveis.
4. Definir placeholders permitidos.
5. Mapear cenas, prefabs, dados, arte, áudio e narrativa necessários.
6. Dividir o trabalho por agente.
7. Ordenar tarefas pela cadeia crítica.
8. Definir smoke test e evidências obrigatórias.
9. Definir número e versão da build.
10. Registrar explicitamente o que fica fora.

## acceptance_criteria

1. A build pode ser iniciada sem o Unity Editor.
2. Existe um caminho completo do menu ao encerramento.
3. Toda mecânica incluída possui feedback e condição de falha.
4. O escopo cabe em um ciclo curto de produção.
5. Nenhuma dependência crítica está implícita.
6. Existe checklist de validação.

## risks

1. Transformar a vertical slice em uma versão completa prematura.
2. Incluir assets finais que não são necessários para validar o loop.
3. Ocultar problemas de integração com scripts de debug.
4. Deixar o final da slice sem uma conclusão clara.

## quality_gate

O plano deve ser pequeno o bastante para terminar e completo o bastante para provar a experiência central.

## handoff

1. Game design para especificações.
2. Engenharia para integração.
3. Lore para eventos e textos.
4. Arte para briefs e placeholders.
5. QA para plano de teste.

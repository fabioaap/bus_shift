# Bus Shift, Build 0.1.0

## Objetivo

Entregar uma vertical slice jogável do Dia 1 que valide o núcleo do Bus Shift antes da expansão para cinco dias.

A build precisa funcionar fora do Unity Editor e permitir que uma pessoa complete o fluxo principal sem intervenção técnica.

## Pergunta de produto

O loop de dirigir, observar o ônibus, identificar uma presença sobrenatural e reagir antes que a tensão alcance o limite é compreensível, tenso e repetível?

## Escopo fechado

### Cenas

1. `Bootstrap`
2. `MainMenu`
3. `Day1Morning`
4. `Day1Night`
5. `SliceEnding`

### Loop principal

1. Iniciar novo jogo.
2. Entrar no ônibus.
3. Dirigir uma rota curta.
4. Parar em três pontos.
5. Embarcar e desembarcar passageiros.
6. Perceber sinais de duas presenças sobrenaturais.
7. Usar o retrovisor e indicadores sonoros para identificar ameaça.
8. Aplicar contramedidas.
9. Controlar a tensão até concluir a rota.
10. Falhar quando a tensão atingir o limite.
11. Reiniciar a tentativa.
12. Concluir o Dia 1 e chegar ao encerramento temporário.

## Conteúdo narrativo

### Introdução

O motorista assume uma rota escolar que voltou a operar após um acidente. A escola e a empresa evitam falar sobre o ocorrido.

### Dia 1, manhã

1. Apresentação do ônibus e dos controles.
2. Passageiros normais estabelecem o comportamento esperado.
3. Pequenas inconsistências sugerem que há algo errado.
4. Uma presença aparece de modo ambíguo.

### Dia 1, noite

1. A rota ocorre em condições mais hostis.
2. Duas presenças sobrenaturais possuem sinais distintos.
3. O jogador precisa observar, interpretar e reagir.
4. A conclusão revela uma evidência do acidente anterior.

### Encerramento temporário

O motorista encontra um objeto que não deveria estar no ônibus. A tela confirma a conclusão da vertical slice e retorna ao menu.

## Fantasmas da slice

A seleção final deve priorizar duas entidades com comportamentos claramente diferentes.

### Entidade A, ameaça visual

Sinal principal: presença detectável pelo retrovisor.

Contramedida principal: desviar o foco, ajustar o espelho ou executar uma ação contextual definida pelo sistema existente.

### Entidade B, ameaça sonora ou ambiental

Sinal principal: ruído, rádio, temperatura, iluminação ou comportamento do ônibus.

Contramedida principal: interação diferente da entidade A.

## Regras de design

1. Cada ameaça precisa apresentar sinal, janela de decisão, contramedida e consequência.
2. O jogador não pode perder sem ter recebido informação interpretável.
3. A primeira ocorrência funciona como tutorial contextual.
4. A segunda ocorrência exige reconhecimento sem repetir toda a instrução.
5. A tensão deve representar risco crescente.
6. Tensão alta nunca pode ser interpretada como resultado positivo por outro sistema.
7. Falha deve permitir reinício em até dez segundos.
8. Nenhuma função fora do loop principal pode bloquear a build.

## Arquitetura mínima

### Autoridade de estado

Selecionar um único componente como fonte de verdade para:

1. Menu
2. Gameplay
3. Pausa
4. Game over
5. Conclusão da slice

O segundo gerenciador deve ser removido, convertido em adaptador ou ter responsabilidades separadas de maneira explícita.

### Progressão

A Build 0.1.0 possui apenas dois períodos:

1. Dia 1, manhã
2. Dia 1, noite

Ao terminar a noite, o fluxo carrega `SliceEnding`.

### Tensão

Usar uma escala normalizada de `0.0` a `1.0`.

1. `0.0` representa controle total.
2. Valores maiores representam risco crescente.
3. `1.0` dispara game over.
4. A conclusão registra a tensão final apenas como métrica de desempenho.
5. Qualquer lógica de ending deve respeitar esta semântica.

### Save

Para a slice, o requisito mínimo é salvar:

1. Se o jogador concluiu a manhã.
2. Qual período deve ser carregado ao continuar.
3. Configurações essenciais.

A tensão da tentativa atual pode ser reiniciada ao recarregar o período.

## Direção visual

### Meta

Garantir legibilidade, atmosfera e distinção entre estados, sem exigir arte final.

### Prioridades

1. Silhueta e interior legíveis do ônibus.
2. Três pontos de parada visualmente distintos.
3. Manhã e noite claramente diferenciadas.
4. Retrovisor com leitura imediata.
5. Entidades reconhecíveis por forma, movimento ou efeito.
6. Feedback claro para tensão, dano, contramedida e conclusão.

### Placeholders permitidos

1. Geometria simples.
2. Materiais temporários.
3. Animações básicas.
4. Áudio temporário licenciado ou gerado para protótipo.
5. UI funcional sem polish final.

## Áudio mínimo

1. Motor em idle e movimento.
2. Porta abrindo e fechando.
3. Confirmação de embarque e desembarque.
4. Ambiente de manhã.
5. Ambiente de noite.
6. Sinal sonoro da entidade B.
7. Feedback de aumento de tensão.
8. Feedback de contramedida bem sucedida.
9. Game over.
10. Conclusão da slice.

## Plano de implementação

### Pacote 1, fundação executável

Responsável: Unity Gameplay Engineer

1. Criar ou validar as cinco cenas.
2. Registrar todas no Build Settings.
3. Implementar `Bootstrap` persistente.
4. Garantir navegação do menu até o encerramento.
5. Configurar build Windows de desenvolvimento.

Critério de aceite: a build abre, inicia novo jogo, percorre as cenas e volta ao menu.

### Pacote 2, loop de rota

Responsáveis: Game Designer e Unity Gameplay Engineer

1. Rota curta com três pontos.
2. Regras de parada.
3. Embarque e desembarque.
4. Condição de conclusão de cada período.
5. Feedback de objetivo atual.

Critério de aceite: o jogador conclui manhã e noite sem usar ferramentas do Editor.

### Pacote 3, ameaças e tensão

Responsáveis: Game Designer, Unity Gameplay Engineer e Game QA and Balance

1. Selecionar duas entidades existentes.
2. Integrar sinais e contramedidas.
3. Unificar semântica da tensão.
4. Implementar game over e reinício.
5. Adicionar telemetria local de eventos essenciais.

Critério de aceite: ambas as ameaças podem ser identificadas, combatidas e testadas de forma repetível.

### Pacote 4, lore e apresentação

Responsáveis: Lore Architect e Visual Art Director

1. Escrever textos da introdução e encerramento.
2. Definir pistas ambientais.
3. Criar briefs dos assets temporários.
4. Integrar UI narrativa mínima.
5. Garantir coerência com o acidente e com os cinco fantasmas do jogo completo.

Critério de aceite: a slice possui começo, progressão e gancho final compreensíveis.

### Pacote 5, QA da slice

Responsável: Game QA and Balance

1. Smoke test da build.
2. Cinco partidas internas completas.
3. Registro de bugs P0 e P1.
4. Verificação de game over, reinício, continue e conclusão.
5. Teste em pelo menos duas configurações Windows.

Critério de aceite: nenhum bug P0, caminho principal completável e relatório de playtest versionado.

## Backlog da Build 0.1.0

| Ordem | Item | Responsável | Prioridade | Dependência |
|---:|---|---|---|---|
| 1 | Corrigir CI da PR do AIOX e do squad | Unity Gameplay Engineer | P0 | Nenhuma |
| 2 | Criar cenas mínimas e registrar Build Settings | Unity Gameplay Engineer | P0 | PR do squad utilizável |
| 3 | Definir autoridade única de estado | Unity Gameplay Engineer | P0 | Auditoria dos managers |
| 4 | Corrigir progressão e encerramento da slice | Unity Gameplay Engineer | P0 | Cenas registradas |
| 5 | Corrigir semântica de tensão e ending | Game Designer e Unity Gameplay Engineer | P0 | Autoridade de estado |
| 6 | Integrar rota curta com três paradas | Unity Gameplay Engineer | P0 | Cena Day1 criada |
| 7 | Integrar duas ameaças e contramedidas | Game Designer e Unity Gameplay Engineer | P0 | Loop de rota |
| 8 | Criar game over, reinício e continue | Unity Gameplay Engineer | P0 | Estado e tensão |
| 9 | Integrar introdução e encerramento temporário | Lore Architect | P1 | Fluxo de cenas |
| 10 | Aplicar direção visual temporária | Visual Art Director | P1 | Cenas funcionais |
| 11 | Integrar áudio mínimo | Visual Art Director e Unity Gameplay Engineer | P1 | Eventos do loop |
| 12 | Gerar build Windows e executar smoke test | Game QA and Balance | P0 | Todos os itens P0 |
| 13 | Realizar cinco playtests internos | Game QA and Balance | P1 | Build estável |
| 14 | Decidir expansão para Dias 2 a 5 | Game Chief | P0 | Relatório de playtest |

## Definition of Done

A Build 0.1.0 somente está pronta quando:

1. Existe executável Windows versionado como artefato de CI ou release interna.
2. O jogo inicia no menu.
3. Novo jogo carrega o Dia 1, manhã.
4. O jogador completa três paradas.
5. O jogo transita para o Dia 1, noite.
6. Duas ameaças funcionam com sinais e contramedidas distintas.
7. Tensão máxima dispara game over.
8. Reiniciar funciona.
9. Continue retorna ao período correto.
10. Concluir a noite carrega `SliceEnding`.
11. O encerramento retorna ao menu.
12. Não existem bugs P0 abertos.
13. Cinco partidas internas completas estão documentadas.
14. O squad registra decisão de manter, ajustar ou descartar o core loop.

## Fora do escopo

1. Dias 2 a 5.
2. Cinco fantasmas completos.
3. Arte final de todos os personagens.
4. Trilha sonora completa.
5. Voiceover final.
6. ARG e marketing transmídia.
7. Installer final.
8. Otimização sem profiling.
9. Beta externo.
10. Publicação comercial.

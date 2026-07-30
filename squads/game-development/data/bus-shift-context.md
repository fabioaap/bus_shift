# Bus Shift Context

## Produto

Bus Shift é um jogo de horror psicológico em primeira pessoa desenvolvido em Unity 6 para Windows PC.

O jogador assume o papel de Dale Mercer, motorista de um ônibus escolar ligado a um acidente ocorrido em 2023. A experiência se desenvolve por cinco dias, com turnos diurnos e noturnos, escalada de tensão e três finais.

## Pilares

1. Direção sob pressão.
2. Observação limitada pelo retrovisor e pelas câmeras.
3. Contramedidas com tempo de recarga.
4. Horror psicológico construído por som, iluminação e comportamento.
5. Narrativa ambiental revelada durante a rotina.

## Sistemas existentes

O repositório contém sistemas para direção, rota, paradas, passageiros, combustível, tensão, sanidade, dias, save, HUD, fantasmas, diálogos, cutscenes, finais, áudio dinâmico e transições de cena.

A presença de código não significa que o sistema esteja integrado ou validado dentro de uma build.

## Bloqueios conhecidos

1. Nenhuma cena registrada no Build Settings.
2. Divergência entre GameManager e GameStateManager.
3. Lógica de tensão e critérios dos finais precisam de revisão.
4. Fluxo de conclusão do Dia 5 usa estado de Game Over.
5. Conteúdo de arte, animação e áudio ainda está incompleto.
6. Não existe build de vertical slice validada.
7. Alpha, balanceamento, otimização e release ainda não foram realizados.

## Próxima meta recomendada

Produzir uma Build 0.1.0 com uma vertical slice completa do Dia 1:

1. Menu principal.
2. Dia 1 manhã.
3. Dia 1 noite.
4. Uma rota completa.
5. Embarque e desembarque.
6. Dois fantasmas integrados.
7. Tensão, observação e contramedidas.
8. Game Over e reinício.
9. Save e continue.
10. Conclusão temporária.
11. Executável Windows.

## Regra de decisão

Priorizar sempre a menor entrega que prove o loop completo do jogo. Evitar criar novos sistemas enquanto os existentes não estiverem integrados e testados.

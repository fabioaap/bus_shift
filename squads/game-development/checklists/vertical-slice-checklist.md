# Vertical Slice Checklist

## Identificação

1. [ ] Nome e versão da build definidos.
2. [ ] Commit e branch registrados.
3. [ ] Plataforma e requisitos mínimos informados.
4. [ ] Escopo e exclusões documentados.

## Fluxo completo

1. [ ] Aplicação inicia fora do Unity Editor.
2. [ ] Menu principal funciona.
3. [ ] Nova partida inicia corretamente.
4. [ ] O jogador recebe objetivo compreensível.
5. [ ] O core loop pode ser executado.
6. [ ] Existe ao menos uma ameaça funcional.
7. [ ] Existe condição de falha.
8. [ ] Game Over apresenta feedback.
9. [ ] Reinício funciona.
10. [ ] Existe conclusão clara da slice.

## Integração

1. [ ] Cenas necessárias estão no Build Settings.
2. [ ] Managers possuem fonte de verdade única.
3. [ ] Prefabs e referências estão configurados.
4. [ ] Dados de tuning não estão espalhados no código.
5. [ ] Save e load foram testados quando incluídos.
6. [ ] Não existem exceções inesperadas no log.

## Experiência

1. [ ] Inputs são informados.
2. [ ] Ações possuem feedback visual ou sonoro.
3. [ ] Ameaças possuem sinais legíveis.
4. [ ] Falhas são compreensíveis.
5. [ ] UI não bloqueia informação crítica.
6. [ ] Áudio essencial está presente, mesmo provisório.

## Qualidade

1. [ ] Smoke test concluído.
2. [ ] Caminho principal concluído ao menos três vezes.
3. [ ] Bugs P0 estão zerados.
4. [ ] Bugs P1 possuem correção ou mitigação documentada.
5. [ ] Performance foi medida na cena mais pesada.
6. [ ] Evidências foram anexadas.

## Saída

1. [ ] Build foi disponibilizada para teste.
2. [ ] Changelog foi criado.
3. [ ] Limitações conhecidas estão documentadas.
4. [ ] Próxima decisão de produto está registrada.

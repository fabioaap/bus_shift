# Unity Integration Checklist

## Compilação

1. [ ] Projeto abre na versão correta do Unity.
2. [ ] Packages resolvem sem erro.
3. [ ] Console não apresenta erros de compilação.
4. [ ] Namespaces e assemblies estão coerentes.

## Cena e prefab

1. [ ] Script está anexado ao GameObject correto.
2. [ ] Referências do Inspector estão preenchidas.
3. [ ] Prefab possui alterações aplicadas.
4. [ ] Overrides intencionais estão documentados.
5. [ ] Tags, layers e colliders estão configurados.
6. [ ] Objetos persistentes não são duplicados ao trocar de cena.

## Estado e eventos

1. [ ] Existe uma fonte de verdade por domínio.
2. [ ] Eventos possuem inscrição e remoção simétricas.
3. [ ] Estados inválidos são bloqueados.
4. [ ] Pause respeita `Time.timeScale` e tempo não escalado quando necessário.
5. [ ] Save e load restauram estado consistente.

## Dados

1. [ ] Valores de tuning estão configuráveis.
2. [ ] ScriptableObjects têm assets criados.
3. [ ] Dados obrigatórios possuem validação.
4. [ ] Valores padrão são seguros.

## Fluxo

1. [ ] Caminho feliz funciona em Play Mode.
2. [ ] Falha e recuperação funcionam.
3. [ ] Transições de cena não deixam tela bloqueada.
4. [ ] Game Over e reinício limpam estado.
5. [ ] Conclusão aciona final e créditos corretos.

## Build

1. [ ] Cenas estão registradas no Build Settings.
2. [ ] Primeira cena é a entrada correta.
3. [ ] Build Windows gera sem erro.
4. [ ] Build inicia em máquina limpa ou ambiente equivalente.
5. [ ] Logs não apresentam exceções inesperadas.

## Performance

1. [ ] Cena mais pesada foi medida.
2. [ ] Não existem alocações excessivas por frame em sistemas críticos.
3. [ ] Objetos repetidos usam pooling quando necessário.
4. [ ] Luzes, sombras e pós processamento respeitam o orçamento.
5. [ ] Otimizações são sustentadas por profiling.

## Evidência

1. [ ] Versão e commit registrados.
2. [ ] Passos de teste registrados.
3. [ ] Screenshot, vídeo, log ou resultado de teste anexado.
4. [ ] Critérios de aceite marcados.

# Migração para AIOX Core 5.3.0

## Estado atual

O Bus Shift agora possui a instalação canônica do AIOX Core 5.3.0 em `.aiox-core/`.

A estrutura anterior permanece em `.aios-core/` com o manifest 4.0.4 regenerado. Ela foi mantida temporariamente porque o repositório ainda contém scripts, workflows e referências que usam o nome legado.

## Validações executadas

1. Versão do CLI confirmada como 5.3.0.
2. Instalação oficial concluída.
3. Manifest canônico validado.
4. Configuração principal validada.
5. Reparos seguros do doctor aplicados.
6. Comandos legados sincronizados.
7. Manifest legado regenerado.
8. Correções não destrutivas de dependências aplicadas.
9. Arquivos do Game Development Squad validados.

## Pendências antes do merge

1. Confirmar o CI completo no commit mais recente.
2. Revisar o diff amplo gerado pela atualização do framework.
3. Mapear referências restantes a `.aios-core/`.
4. Definir uma PR separada para remover a árvore legada com segurança.
5. Resolver ou documentar os avisos restantes do `aiox doctor`.

## Decisão de migração

A remoção de `.aios-core/` não faz parte desta PR. Misturar atualização, criação do squad e exclusão da estrutura antiga dificultaria a revisão e poderia quebrar agentes ou automações ainda dependentes do caminho legado.

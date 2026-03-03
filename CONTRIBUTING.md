# Contribuindo com Bus Shift

Obrigado pelo interesse em contribuir com Bus Shift! Este é um projeto de jogo narrativo desenvolvido como trabalho acadêmico. Veja abaixo como colaborar.

## Como Contribuir

### Reportando bugs ou sugestões

Abra uma [issue](https://github.com/fabioaap/bus_shift/issues) descrevendo:

- O que você encontrou ou quer sugerir
- Passos para reproduzir (no caso de bug)
- Versão do Unity e sistema operacional

### Submetendo Pull Requests

1. Faça um fork do repositório
2. Crie uma branch: `git checkout -b feat/minha-contribuicao`
3. Faça suas alterações e commit: `git commit -m "feat: descrição"`
4. Envie: `git push origin feat/minha-contribuicao`
5. Abra um Pull Request explicando o que foi feito

### Convenções de Commit

Usamos [Conventional Commits](https://www.conventionalcommits.org/):

```
feat:     nova funcionalidade
fix:      correção de bug
docs:     atualização de documentação
chore:    tarefa de manutenção
refactor: refatoração sem mudança de comportamento
```

## Estrutura do Projeto

| Pasta | Conteúdo |
|---|---|
| `game/Assets/` | Assets Unity (scripts, cenas, modelos, texturas) |
| `game/Assets/Scripts/` | Scripts C# do jogo |
| `game/Assets/Scenes/` | Cenas Unity |
| `narrative/` | Página web da narrativa |
| `docs/` | Documentação do projeto |

## Padrões de Código (Unity / C#)

- Nomear scripts em `PascalCase`
- Nomear variáveis privadas com prefixo `_camelCase`
- Documentar métodos públicos com comentário XML
- Evitar lógica de gameplay em `Update()` quando possível

## Dúvidas

Entre em contato com a equipe via Issues ou diretamente com um dos membros listados no README.

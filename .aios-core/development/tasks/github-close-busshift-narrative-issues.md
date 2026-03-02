# github-close-busshift-narrative-issues

**Task**: Fechar issues de narrativa concluídas no repositório fabioaap/bus_shift
**Agent**: @devops (Gage)
**Autoridade necessária**: GitHub REST API — PATCH /repos/{owner}/{repo}/issues/{issue_number}

---

## Contexto

As seguintes issues foram implementadas pelo agente de narrativa e estão prontas para fechamento:

| Issue | Título | Entregável |
|-------|--------|------------|
| **#66** | Atualizar characters.md com Dale Mercer e Earl Pruitt | `docs/game/narrative/characters.md` ✅ |
| **#67** | Revisar endings.md com detalhes expandidos | `docs/game/narrative/endings.md` ✅ |
| **#68** | Expandir cenas de Grace e Oliver com protagonismo (Dias 3-4) | `docs/game/narrative/story.md` + `story_pt-BR.md` ✅ |

---

## Execução

### Pré-requisito
```
$env:GH_TOKEN deve estar disponível na sessão
Repositório: fabioaap/bus_shift
```

### Comandos (PowerShell)

```powershell
$token = $env:GH_TOKEN
$headers = @{
  Authorization = "Bearer $token"
  "Content-Type" = "application/json"
  "Accept" = "application/vnd.github+json"
}
$body = @{ state = "closed" } | ConvertTo-Json

# Fechar #66
Invoke-RestMethod -Uri "https://api.github.com/repos/fabioaap/bus_shift/issues/66" `
  -Method PATCH -Headers $headers -Body $body

# Fechar #67
Invoke-RestMethod -Uri "https://api.github.com/repos/fabioaap/bus_shift/issues/67" `
  -Method PATCH -Headers $headers -Body $body

# Fechar #68
Invoke-RestMethod -Uri "https://api.github.com/repos/fabioaap/bus_shift/issues/68" `
  -Method PATCH -Headers $headers -Body $body

Write-Host "Issues #66, #67, #68 fechadas com sucesso."
```

### Validação esperada
Após execução, cada issue deve retornar `"state": "closed"` na API.
O GitHub Projects (workflows ativos) moverá automaticamente para a coluna **Done**.

---

## Critérios de Aceite
- [ ] Issue #66 com estado `closed`
- [ ] Issue #67 com estado `closed`
- [ ] Issue #68 com estado `closed`
- [ ] GitHub Projects mostra as 3 na coluna Done

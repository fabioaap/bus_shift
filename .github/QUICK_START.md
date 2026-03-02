# Guia Rápido - Criar Issues do Backlog

## 🚀 Método 1: Script Python (Recomendado)

### Pré-requisitos

```powershell
# Verificar se Python está instalado
python --version

# Instalar biblioteca requests
pip install requests
```

### Passo 1: Criar GitHub Token

1. Acesse: https://github.com/settings/tokens
2. Click "Generate new token" → "Generate new token (classic)"
3. Nome: `Bus Shift Issues Creator`
4. Scopes necessários:
   - ✅ `repo` (Full control of private repositories)
   - ✅ `project` (Full control of projects)
5. Click "Generate token"
6. **COPIE O TOKEN** (só aparece uma vez!)

### Passo 2: Executar Script

```powershell
# Navegar até o diretório do projeto
cd "C:\Users\Educacross\Documents\Projetos Fábio\bus_shift"

# Executar script (SUBSTITUA os valores)
python .github\create_issues.py `
  --token SEU_TOKEN_AQUI `
  --repo seu-usuario/bus_shift
```

**Exemplo:**
```powershell
python .github\create_issues.py `
  --token ghp_abc123XYZ... `
  --repo fabricio/bus_shift
```

### O que o script faz?

1. ✅ Cria 18 labels customizadas (priorities, epics, tipos)
2. ✅ Cria 5 milestones com datas
3. ✅ Cria 7 issues do Epic 1 (Documentação)

---

## 🔧 Método 2: Manual via Interface Web

### Criar Labels

Vá para: `https://github.com/seu-usuario/bus_shift/labels`

**Priorities:**
- `priority: critical` - Cor: `#d73a4a` (vermelho)
- `priority: high` - Cor: `#ff9800` (laranja)
- `priority: medium` - Cor: `#fbca04` (amarelo)
- `priority: low` - Cor: `#0e8a16` (verde)

**Epics:**
- `epic: documentation` - Cor: `#7057ff` (roxo)
- `epic: core-mechanics` - Cor: `#0052cc` (azul)
- `epic: ghosts` - Cor: `#5319e7` (roxo escuro)
- `epic: progression` - Cor: `#0e8a16` (verde)
- `epic: art` - Cor: `#e99695` (rosa)
- `epic: audio` - Cor: `#c5def5` (azul claro)
- `epic: narrative` - Cor: `#d876e3` (roxo claro)
- `epic: testing` - Cor: `#fbca04` (amarelo)

### Criar Milestones

Vá para: `https://github.com/seu-usuario/bus_shift/milestones`

1. **M1: Documentação Completa** - Due: 31/03/2026
2. **M2: Vertical Slice - Dia 1** - Due: 31/05/2026
3. **M3: MVP - 5 Dias Completos** - Due: 31/08/2026
4. **M4: Content Complete** - Due: 31/10/2026
5. **M5: Gold Master** - Due: 31/12/2026

### Criar Issues

Vá para: `https://github.com/seu-usuario/bus_shift/issues/new`

Use os templates do `BACKLOG.md`:
- Copie título
- Copie descrição
- Adicione labels apropriadas
- Selecione milestone
- Click "Submit new issue"

Repita para as 52 issues...

---

## 📊 Adicionar Issues ao Projeto

### Opção A: Automático (via script futuro)

_(Será implementado após issues criadas)_

### Opção B: Manual

1. Vá para seu projeto: `https://github.com/users/seu-usuario/projects/NUMERO`
2. Click "Add item"
3. Selecione issues criadas
4. Arraste para colunas apropriadas

---

## 🔍 Verificar

Após executar:

```powershell
# Listar issues criadas
python -c "import requests; r=requests.get('https://api.github.com/repos/SEU_USUARIO/bus_shift/issues'); print(f'{len(r.json())} issues criadas')"
```

Ou acesse: `https://github.com/seu-usuario/bus_shift/issues`

---

## 🐛 Troubleshooting

### Erro: "Bad credentials"
- Token inválido ou expirado
- Gere novo token e tente novamente

### Erro: "Not Found"
- Nome do repositório incorreto
- Formato correto: `usuario/repositorio` (sem https://)

### Erro: "Rate limit exceeded"
- Aguarde 1 hora ou use token com rate limit maior

### Erro: "Validation Failed"
- Milestone pode já existir
- Use flag: `--skip-milestones`

---

## 📝 Próximos Passos

Após criar Epic 1:

1. ✅ Verificar issues criadas
2. ✅ Adicionar ao projeto GitHub Projects
3. ✅ Adaptar script para criar Epics 2-8
4. ✅ Criar issues dos demais épicos

---

**Dúvidas?** Consulte:
- `BACKLOG.md` - Lista completa de issues
- `GITHUB_PROJECT_SETUP.md` - Setup do projeto

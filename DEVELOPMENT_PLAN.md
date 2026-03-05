# 🎯 Plano de Desenvolvimento - Bus Shift
**Atualizado:** 5 de março de 2026

---

## ✅ RECÉM COMPLETADO

### Tooling & Infrastructure
- **#57** ✅ Unity MCP - Integrado e testado
- **#56** ⚙️ Blender MCP - Configurado (aguarda Blender aberto)

---

## 📊 STATUS GERAL

| Milestone | Issues Abertas | Issues Fechadas | Status |
|-----------|----------------|-----------------|--------|
| **M3: MVP - 5 Dias Completos** | 3 | 11 | 🟢 78% Complete |
| **M4: Content Complete** | 7 | 4 | 🟡 36% Complete |
| **M5: Gold Master** | 3 | 0 | 🔴 0% Complete |

---

## 🔴 CRÍTICO (P0) - Fazer AGORA

### #48 - Alpha Testing e Bug Fixing (13 SP)
**Milestone:** M3: MVP  
**Dependências:** Todos features MVP (#9-#31)  
**Descrição:**
- 10+ playthroughs completos
- Documentar bugs críticos
- Fixes prioritários
- Game completável sem crashes

**Critérios de Aceite:**
- [ ] 10+ playthroughs documentados
- [ ] Bugs P0 resolvidos
- [ ] Jogo completável sem crashes

**👉 PRÓXIMA TAREFA SUGERIDA:** Começar alpha testing interno para validar MVP

---

### #53 - Release Build e Deploy (13 SP)
**Milestone:** M5: Gold Master  
**Dependências:** #52 (Beta Testing)  
**Descrição:**
- Build para Windows
- Installer
- Readme/manual
- Testado em 3+ configurações

**Status:** ⏸️ Aguardando beta testing

---

## 🟠 ALTA PRIORIDADE (P1)

### #39 - Sound Effects (SFX) Pack (13 SP) 
**Milestone:** M3: MVP  
**Epic:** Áudio  
**Descrição:**
- 50+ SFX (motor, portas, passos, contramedidas, ambiente)
- Formatos OGG/WAV
- Espacialização 3D

**Critérios:**
- [ ] 50+ SFX de alta qualidade
- [ ] Implementados na engine
- [ ] Espacialização 3D funcional

**👉 PRÓXIMA:** Pode começar em paralelo com alpha testing

---

### #38 - Animações de Personagens (14 SP)
**Milestone:** M4: Content Complete  
**Dependências:** Modelos das crianças (#18, #20, #22)  
**Descrição:**
- Crianças: caminhar, sentar, embarcar/desembarcar
- Fantasmas: idle perturbador, flutuante
- Motorista: dirigir, olhar retrovisor

**Critérios:**
- [ ] Todas animações criadas (30 FPS min)
- [ ] Loopáveis
- [ ] Integradas na engine

---

### #40 - Trilha Sonora Original (20 SP)
**Milestone:** M4: Content Complete  
**Dependências:** #28 (Sistema de Áudio Dinâmico)  
**Descrição:**
- Main Theme (menu)
- Gameplay Loop (camadas de tensão)
- Endings (3 variações)
- Cutscenes

**Critérios:**
- [ ] 5 faixas completas
- [ ] Estilo ambient horror
- [ ] Loopável
- [ ] Masterizada

---

### #49 - Balanceamento de Dificuldade (10 SP)
**Milestone:** M4: Content Complete  
**Dependências:** #48 (Alpha Testing)  
**Descrição:**
- Taxa de spawn de assombrações
- Cooldowns de contramedidas
- Depleção de sanidade
- Duração de cada dia

**Critérios:**
- [ ] 20+ playtests documentados
- [ ] Dificuldade acessível mas desafiadora
- [ ] Endings balanceados (30/40/30)

---

### #50 - Performance Optimization (13 SP)
**Milestone:** M4: Content Complete  
**Descrição:**
- Profiling (CPU/GPU bottlenecks)
- LOD para modelos distantes
- Occlusion culling
- Batching de draw calls

**Critérios:**
- [ ] 60 FPS em mid-range hardware
- [ ] Load times < 5s
- [ ] Sem memory leaks

---

### #52 - Beta Testing Externo (8 SP)
**Milestone:** M5: Gold Master  
**Dependências:** #51 (Polish Pass)  
**Descrição:**
- 50+ testers beta
- Feedback estruturado
- Fixes de bugs reportados

**Critérios:**
- [ ] 50+ playthroughs beta
- [ ] Feedback analisado
- [ ] Bugs P1/P2 resolvidos
- [ ] Score médio > 7/10

---

## 🟡 MÉDIA PRIORIDADE (P2)

### #41 - Áudio Ambiental e Atmosférico (8 SP)
**Milestone:** M4: Content Complete  
**Dependências:** #39 (SFX base)  
**Descrição:**
- Trânsito distante, vento, sussurros, rádio estático
- 20+ sons ambientes
- Randomização
- Espacialização 3D

---

### #47 - Diálogos e Voiceover (15 SP)
**Milestone:** M4: Content Complete  
**Dependências:** #43 (Cutscenes), #2 (Narrativa)  
**Descrição:**
- Narração interna do motorista
- NPCs comentários
- Voiceover para cutscenes
- Script completo BR-PT

---

### #51 - Polish Pass Final (13 SP)
**Milestone:** M5: Gold Master  
**Dependências:** #50 (Performance OK)  
**Descrição:**
- Animações suavizadas
- Efeitos visuais polidos
- Mixagem de áudio final
- UI tweaks

---

## 🔵 BAIXA PRIORIDADE (P3)

### #57 - Unity MCP ✅ COMPLETO
### #56 - Blender MCP ⚙️ COMPLETO

---

## 📚 MARKETING & CONTEÚDO

### #54 - Estratégia Transmídia: Site ARG + Worldbuilding
**Estimativa:** 6+ meses de produção  
**Descrição:**
- 7 fases de implementação
- Site de investigação do acidente
- Memorial das 5 crianças
- ARG com pistas escondidas
- Redes sociais falsas
- Integração com o jogo (QR codes, códigos secretos)

**Roadmap:**
1. Documentação & Planejamento (4 semanas)
2. Conteúdo Escrito (6 semanas)
3. Assets Visuais (4 semanas)
4. Desenvolvimento Web (8 semanas)
5. Integração ARG (4 semanas)
6. Testes & Lançamento (2 semanas)
7. Marketing & Expansão (ongoing)

**👉 Lançamento sugerido:** 6 meses antes do jogo

---

## 🎯 PLANO DE AÇÃO RECOMENDADO

### Semanas 1-2: Alpha Testing & SFX
1. **Iniciar #48 - Alpha Testing**
   - Recrutar 10 testers internos
   - Criar planilha de bug tracking
   - Realizar playthroughs completos
   - Documentar todos os bugs

2. **Paralelamente: #39 - SFX Pack**
   - Licenciar/criar 50+ sound effects
   - Implementar sistema de espacialização 3D
   - Integrar na engine

### Semanas 3-4: Bug Fixing & Animações
3. **Completar #48 - Fixes de Bugs P0**
   - Resolver todos os crashes
   - Corrigir bugs que impedem completion
   - Validar estabilidade

4. **Iniciar #38 - Animações de Personagens**
   - Crianças NPCs
   - Fantasmas (idle + movimento)
   - Motorista

### Semanas 5-8: Content Complete
5. **#40 - Trilha Sonora** (paralelo com #38)
   - Compor 5 faixas
   - Integrar com sistema de áudio dinâmico

6. **#41 - Áudio Ambiental**
   - Sons atmosféricos
   - Mixagem balanceada

7. **#49 - Balanceamento**
   - 20+ playtests documentados
   - Ajustar dificuldade
   - Testar endings

8. **#50 - Performance Optimization**
   - Profiling
   - LOD, occlusion culling
   - Target: 60 FPS

### Semanas 9-10: Polish & Beta
9. **#51 - Polish Pass Final**
   - Refinamentos visuais
   - UI/UX tweaks
   - Audio final mix

10. **#52 - Beta Testing Externo**
    - Recrutar 50+ beta testers
    - Coletar feedback estruturado
    - Iterar fixes

### Semanas 11-12: Gold Master
11. **#53 - Release Build**
    - Build Windows
    - Installer
    - Manual/README
    - Testes em múltiplas configurações

### Paralelo: Marketing (6 meses antes)
12. **#54 - Site ARG + Transmedia**
    - Começar planejamento 6 meses antes do launch
    - Desenvolver conteúdo narrativo
    - Implementar website
    - Lançar ARG progressivamente

---

## 📈 MÉTRICAS DE SUCESSO

### MVP (M3)
- ✅ Jogo completável sem crashes
- ✅ 5 dias jogáveis
- ✅ Todos os sistemas core funcionais

### Content Complete (M4)
- 🎨 Todos os assets finais implementados
- 🎵 Áudio completo (SFX, música, VO)
- ⚖️ Balanceamento testado e aprovado
- 🚀 Performance: 60 FPS em mid-range

### Gold Master (M5)
- 📦 Build de release funcional
- ✅ Beta testing: score > 7/10
- 🐛 Todos bugs críticos resolvidos
- 📄 Documentação completa

---

## 🚀 PRÓXIMOS PASSOS IMEDIATOS

1. ✅ **Marcar #56 e #57 como completas** (ou in-progress)
2. 🎯 **Iniciar #48 (Alpha Testing)** - CRÍTICO
3. 🔊 **Iniciar #39 (SFX Pack)** - Pode rodar em paralelo
4. 📝 **Criar checklist de bug tracking** para alpha
5. 👥 **Recrutar 10 testers internos** para alpha phase
6. 📊 **Setup analytics** para tracking de bugs/crashes
7. 🗓️ **Definir cronograma** detalhado baseado neste plano

---

**Autor:** Claude (GitHub Copilot)  
**Data:** 5 de março de 2026  
**Status:** Draft para aprovação do time


# Build 0.1.0, matriz de ameaças

## Decisão canônica

A vertical slice do Dia 1 usará:

1. **Emma** como primeiro contato sobrenatural e ameaça principal.
2. **Thomas** como presença sonora implícita no rádio, sem manifestação completa e sem revelar sua identidade ao jogador.

Essa combinação preserva a narrativa canônica de `docs/game/narrative/story.md` e ainda testa dois canais de percepção e duas interações diferentes.

Marcus permanece reservado para o Dia 2, quando a história apresenta sua primeira aparição nas fileiras traseiras.

## Dívida de nomenclatura das cenas

O código legado usa `Day1Morning` e `Day1Night`, mas a narrativa do Dia 1 acontece de manhã e à tarde.

Para a Build 0.1.0:

1. `Day1Morning` representa a rota matinal.
2. `Day1Night` é um nome técnico temporário e representa a rota da tarde em horário de neblina e luz baixa.
3. A interface deve exibir `Afternoon Shift`, não `Night Shift`.
4. A renomeação técnica para `Day1Afternoon` será decidida depois da vertical slice, porque altera o sistema de transição de todos os cinco dias.

## Comparação dos fantasmas existentes

| Fantasma | Primeira aparição canônica | Sinal principal | Contramedida | Uso na Build 0.1.0 |
|---|---|---|---|---|
| Emma | Dia 1 | Risada e aparição ao lado do motorista | Panel Lock, com observação contextual | Ameaça principal |
| Thomas | Dia 2, com prenúncio no rádio no Dia 1 | Estática e sussurros | Rádio | Presença implícita e tutorial seguro |
| Grace | Dia 2 | Aparição no retrovisor e bloqueio visual | Condução cuidadosa e observação | Adiada |
| Marcus | Dia 2 | Avanço pelas fileiras | Observação sustentada ou microfone | Adiado |
| Oliver | Dia 2 | Desenhos e medo dos passageiros | Microfone | Adiado |

## Ameaça A, Emma

### Função narrativa

Emma é o primeiro fantasma que Dale vê com clareza. Ela transforma ruídos ambíguos em uma ameaça impossível de racionalizar.

### Função de design

Ensinar uma reação rápida no painel sem usar a janela final de dois segundos logo no primeiro encontro.

### Sequência canônica adaptada

1. Uma risada curta ocorre atrás da cabeça de Dale durante a rota matinal.
2. Nenhuma criança viva assume a risada.
3. Na rota da tarde, a risada retorna e muda de posição.
4. Emma aparece ao lado do motorista e aproxima a mão do controle da porta.
5. O Panel Lock ganha destaque contextual.
6. O jogador bloqueia o painel antes que Emma toque o botão.
7. Emma desaparece como uma mudança abrupta de canal.
8. As crianças vivas reagem ao comportamento de Dale, não à presença de Emma.

### Sinais

1. Risada em três níveis.
2. Queda de temperatura ou vinheta discreta.
3. Aparição à direita do motorista.
4. Mão aproximando se do painel.
5. Destaque visual do Panel Lock apenas no primeiro encontro.

### Contramedida

Ativar o Panel Lock antes do fim da janela.

### Parâmetros de onboarding

| Parâmetro | Valor inicial da slice |
|---|---:|
| Prenúncio antes da manifestação | 2,5 segundos |
| Janela de reação do primeiro encontro | 6 segundos |
| Janela de uma repetição opcional | 4 segundos |
| Janela final planejada para dias avançados | 2 segundos |
| Tensão ao observar cedo demais | 0,10 |
| Redução de tensão por confronto correto | 0,05 |

A janela de dois segundos permanece como dificuldade avançada, não como primeira instrução do jogo.

## Presença B, Thomas

### Função narrativa

Thomas ainda não aparece visualmente no Dia 1. Sua voz existe como algo quase inaudível na frequência proibida do rádio.

### Função de design

Ensinar que o rádio não é somente ambientação e que ameaças podem ter sinais sonoros e respostas no painel.

### Sequência

1. Ao ligar o ônibus, o rádio muda sozinho para o canal dois.
2. Estática contém fragmentos de uma voz infantil, sem legenda completa.
3. O objetivo contextual orienta Dale a retornar ao canal quatro ou ligar a transmissão normal.
4. O ruído desaparece imediatamente.
5. Na rota da tarde, o evento retorna uma vez sem instrução completa.
6. Ignorar o evento aumenta um pouco a tensão, mas não causa game over no Dia 1.
7. A manifestação completa de Thomas fica reservada para o Dia 2.

### Sinais

1. Mudança involuntária de canal.
2. Estática com padrão não aleatório.
3. Sussurro espacial vindo do fundo do ônibus.
4. Indicador visual do canal dois.
5. Pequena interferência no painel.

### Contramedida

Retornar ao canal quatro ou ativar o rádio normal, conforme a implementação final do `RadioSystem`.

### Parâmetros de onboarding

| Parâmetro | Valor inicial da slice |
|---|---:|
| Duração antes da instrução contextual | 4 segundos |
| Tensão única por ignorar o prenúncio | 0,05 |
| Game over | Desativado no Dia 1 |
| Ocorrências | Uma na manhã e uma na tarde |
| Manifestação visual | Desativada |
| Nome de Thomas na interface | Oculto |

## Ordem de apresentação

### Dia 1, manhã

1. Rádio muda para o canal dois antes da primeira saída.
2. O jogador aprende a restaurar o canal quatro.
3. Na segunda parada, a risada de Emma surge como evento sem ameaça direta.
4. Dale racionaliza o ocorrido como cansaço e ruído do ônibus.

### Dia 1, tarde

1. O rádio repete a interferência sem instrução completa.
2. A risada de Emma muda de posição e aumenta.
3. Emma manifesta se ao lado do motorista.
4. O jogador usa Panel Lock.
5. O turno termina após as crianças vivas reagirem ao comportamento de Dale.
6. `SliceEnding` apresenta o gancho para o jornal do acidente, sem entregar toda a investigação do Dia 2.

## Regras de justiça

1. O primeiro contato de Emma não pode matar o jogador antes de indicar o Panel Lock.
2. A ameaça só inicia quando o ônibus está em condição segura ou controlada.
3. O jogador recebe sinal antes da punição.
4. O rádio possui alternativa visual para pessoas sem áudio.
5. A identidade de Thomas não é revelada no Dia 1.
6. Emma não pode reaparecer durante diálogo obrigatório.
7. O tutorial contextual desaparece após a primeira resposta correta.
8. Falhar no encontro de Emma deve permitir reinício rápido da rota da tarde.

## Telemetria local

Registrar:

1. Início da interferência do rádio.
2. Tempo até o jogador restaurar o canal.
3. Início da risada de Emma.
4. Início da manifestação.
5. Tempo até ativar Panel Lock.
6. Tensão antes e depois dos eventos.
7. Causa do game over.
8. Quantidade de instruções exibidas.
9. Tempo total do turno.

## Critérios de aprovação

Emma está validada quando quatro de cinco jogadores identificam o Panel Lock e respondem corretamente após o primeiro tutorial.

O prenúncio de Thomas está validado quando quatro de cinco jogadores reconhecem que a interferência exige uma ação no rádio na segunda ocorrência.

A narrativa está validada quando jogadores compreendem que Dale viu algo impossível, mas ainda não recebem a explicação completa do acidente ou dos cinco fantasmas.

# Build 0.1.0, matriz de ameaças

## Decisão

A vertical slice do Dia 1 usará Marcus e Thomas.

A dupla foi escolhida porque testa dois canais de leitura e duas respostas diferentes sem exigir a criação de um sistema sobrenatural novo.

## Comparação dos fantasmas existentes

| Fantasma | Sinal principal | Contramedida | Risco | Adequação à primeira slice |
|---|---|---|---|---|
| Grace | Bloqueio visual | Não possui resposta direta, exige evitar colisões | Tensão por colisão | Baixa, não ensina contrajogo claro |
| Oliver | Desenhos e medo dos passageiros | Microfone | Tensão contínua, maior quando atrasado | Média, depende de passageiros e timer bem integrados |
| Marcus | Avanço pelas fileiras | Observação sustentada ou microfone na fase crítica | Game over ao alcançar o motorista | Alta, progressão espacial legível |
| Thomas | Sussurros em fases | Rádio | Tensão crescente e game over | Alta, sinal sonoro e resposta distinta |
| Emma | Aparição ao lado do motorista e risada | Panel Lock ou observação contextual | Janela de dois segundos e game over | Baixa para onboarding, punição muito rápida |

## Ameaça A, Marcus

### Função de design

Ensinar que o jogador precisa observar o interior do ônibus e acompanhar uma ameaça que se aproxima.

### Sequência

1. Marcus aparece nas fileiras traseiras.
2. O retrovisor ou a câmera revela sua posição.
3. Ele avança em intervalos definidos.
4. A observação sustentada faz Marcus recuar e reduz ligeiramente a tensão.
5. Quando chega à primeira fileira, inicia uma janela crítica.
6. O microfone neutraliza a ameaça durante a janela crítica.
7. Ignorar a ameaça até o fim da janela dispara game over.

### Sinal

1. Silhueta visível em uma fileira.
2. Mudança de posição.
3. Feedback visual ou sonoro a cada avanço.
4. Alerta crítico ao alcançar a primeira fileira.

### Contramedidas

1. Observação sustentada antes da fase crítica.
2. Microfone durante a fase crítica.

### Parâmetros iniciais da slice

| Parâmetro | Valor inicial |
|---|---:|
| Fileira de surgimento | 5 |
| Intervalo de avanço | 12 segundos |
| Tempo de observação | 3 segundos |
| Janela crítica | 8 segundos |
| Reaparecimento | Uma vez na manhã e uma vez na noite |
| Redução de tensão por observação | 0,05 |

A janela crítica deve ser maior que a configuração atual de dificuldade final para permitir aprendizagem.

## Ameaça B, Thomas

### Função de design

Ensinar que ameaças também podem ser detectadas pelo áudio e que o painel do ônibus possui ferramentas de defesa.

### Sequência

1. Sussurros suaves começam no fundo do ônibus.
2. A tensão recebe um aumento pequeno e o rádio ganha destaque contextual.
3. Os sussurros ficam mais altos.
4. A tensão passa a subir continuamente.
5. Ligar o rádio neutraliza Thomas.
6. Ignorar a última fase dispara game over.

### Sinal

1. Áudio espacial no fundo do ônibus.
2. Três níveis de intensidade.
3. Leve interferência visual ou no rádio.
4. Feedback claro quando o rádio neutraliza a presença.

### Contramedida

Ligar o rádio antes da fase final.

### Parâmetros iniciais da slice

| Parâmetro | Valor inicial |
|---|---:|
| Fase suave | 6 segundos |
| Fase intensa | 8 segundos |
| Tensão inicial | 0,05 |
| Tensão por segundo na fase intensa | 0,08 |
| Reaparecimento | Uma vez na manhã e duas vezes na noite |
| Janela total antes de game over | 14 segundos |

Os valores são hipóteses de teste e devem ser alterados após as cinco partidas internas.

## Ordem de apresentação

### Dia 1, manhã

1. Marcus aparece uma vez como tutorial contextual.
2. O jogo destaca o retrovisor sem pausar totalmente a experiência.
3. O jogador aprende observação sustentada.
4. Thomas aparece em uma versão segura, sem game over imediato.
5. O jogo destaca o rádio.

### Dia 1, noite

1. Marcus retorna sem instrução completa.
2. Thomas retorna com fases mais rápidas.
3. Uma ocorrência controlada permite sobreposição parcial.
4. A sobreposição nunca acontece antes de o jogador neutralizar cada entidade pelo menos uma vez.

## Regras de justiça

1. Nenhuma entidade surge durante uma instrução crítica de direção.
2. O primeiro encontro não pode causar game over antes de apresentar a contramedida.
3. O sinal deve começar antes da punição.
4. O feedback da contramedida correta deve ser imediato.
5. A mesma tecla não pode parecer correta para as duas ameaças no tutorial.
6. Áudio essencial precisa de alternativa visual para acessibilidade e testes sem som.
7. A sobreposição de ameaças deve ser bloqueada no onboarding.

## Telemetria local

Registrar no log ou em arquivo de sessão:

1. Momento do surgimento.
2. Momento em que o jogador observou Marcus.
3. Duração da observação.
4. Momento em que o rádio foi ligado.
5. Fase de Thomas no momento da resposta.
6. Tensão antes e depois da contramedida.
7. Causa do game over.
8. Tempo total da tentativa.

## Critérios de aprovação

Marcus está validado quando quatro de cinco jogadores reconhecem a progressão e usam ao menos uma contramedida sem instrução repetida.

Thomas está validado quando quatro de cinco jogadores associam os sussurros ao rádio após o primeiro encontro.

A dupla está validada quando a sobreposição noturna aumenta tensão sem tornar a causa da falha incompreensível.

/**
 * BUS SHIFT - NARRATIVE SYSTEM
 * Architect-First: Zero hardcoding no HTML. Lógica orientada a dados.
 */

const gameDocs = [
    {
        id: "doc-motorista",
        type: "doc-clipboard",
        title: "VAGA ABERTA",
        stamp: "CONTRATADO",
        posX: 5,
        posY: 5,
        rotation: -4,
        contentClean: `
            <p><strong>RESUMO:</strong> O ônibus 104, um veículo sucateado, precisa de um novo motorista para o trajeto escolar. Trajeto rural isolado.</p>
            <p><strong>REQUISITOS:</strong> Nervos de aço, atenção aos detalhes e capacidade extrema de ignorar o passado.</p>
            <br>
            <p><em>NOTA DO DIRETOR SIQUEIRA:</em> "O veículo possui alguns amassados e o rádio chia um pouco, mas nada com que se preocupar. Concentre-se nas crianças."</p>
        `,
        contentCorrupt: `
            <p><strong>RESUMO:</strong> A TRAGÉDIA SE REPETE. Nenhuma criança foi encontrada nos destroços. Apenas o motorista anterior.</p>
            <p><strong>REQUISITOS:</strong> Sangue fresco para o ciclo. A negação é o seu maior pecado.</p>
            <br>
            <span class="macabre-text">Você sempre volta pro volante. É o seu purgatório.</span>
        `
    },
    {
        id: "doc-mapa",
        type: "doc-folder",
        title: "ROTA E VILA",
        stamp: null,
        posX: 70,
        posY: 10,
        rotation: 3,
        contentClean: `
            <p><strong>CENÁRIO:</strong> Uma rota escolar contínua no subúrbio. Passando por residenciais, uma Igreja antiga e a Escola Municipal.</p>
            <p><strong>ATMOSFERA:</strong> Frio constante, cores frias e dessaturadas. O nevoeiro reduz a visibilidade.</p>
            <p><strong>DIREÇÃO:</strong> A (Acelerar), S (Frear/Ré), D (Curvas). O veículo tem inércia pesada.</p>
        `,
        contentCorrupt: `
            <p><strong>CENÁRIO:</strong> Não há saída da cidade. As ruas se dobram sobre si mesmas.</p>
            <p><strong>ATMOSFERA:</strong> A nevasca é feita de cinzas. A Igreja está vazia porque Deus abandonou esse asfalto.</p>
            <br>
            <span class="macabre-text">Acelerar não vai te afastar do que está no banco de trás.</span>
        `
    },
    {
        id: "doc-tensao",
        type: "doc-polaroid",
        title: "DOOM CLOCK",
        stamp: "CLASSIFIED",
        posX: 40,
        posY: 40,
        rotation: -9,
        contentClean: `
            <div style="background:#ccc; width:100%; height:100px; margin-bottom:10px; border:2px solid #555; display:flex; align-items:center; justify-content:center;">
                <p style="font-family:var(--font-typewriter); font-size:12px;">Gráfico de Sanidade: Dia 1 ao 5</p>
            </div>
            <p><strong>MECÂNICA:</strong> A Dificuldade é ditada pela Barra de Tensão. Falhas na rota ou erro nas ferramentas aumentam a tensão.</p>
            <p><strong>EFEITO:</strong> Quanto maior a tensão, mais agressivas são as anomalias no ônibus.</p>
        `,
        contentCorrupt: `
            <div style="background:#500; width:100%; height:100px; margin-bottom:10px; border:2px solid #111; display:flex; align-items:center; justify-content:center;">
                <p style="color:#fff; font-family:var(--font-handwriting); font-size:24px;">MEDO É MORTE</p>
            </div>
            <p>A insanidade assume o controle. O rádio emite sussurros que você não consegue abafar. A realidade se quebra.</p>
        `
    },
    {
        id: "doc-ferramentas",
        type: "doc-folder",
        title: "MONITORAMENTO",
        stamp: "ESSENCIAL",
        posX: 15,
        posY: 55,
        rotation: 8,
        contentClean: `
            <p>Sua sobrevivência depende das interfaces do veículo:</p>
            <ul>
                <li><strong>Retovisor:</strong> Revela o que acontece nas fileiras de trás.</li>
                <li><strong>Microfone (Q):</strong> Dá ordens para as crianças pararem. Recarga pesada.</li>
                <li><strong>Trava (SHIFT):</strong> Bloqueia controles físicos contra intervenções.</li>
                <li><strong>Rádio (R):</strong> Toca música para abafar ruídos indesejados.</li>
            </ul>
        `,
        contentCorrupt: `
            <p>Seus olhos não podem estar em todos os lugares ao mesmo tempo.</p>
            <ul>
                <li><strong>Retovisor:</strong> Mostra o que os mortos querem que você veja.</li>
                <li><strong>Microfone:</strong> Eles não escutam sua voz, só seus gritos.</li>
            </ul>
            <span class="macabre-text">Você prefere olhar pra estrada ou pro que está respirando no seu cangote?</span>
        `
    },
    {
        id: "doc-passageiros",
        type: "doc-clipboard",
        title: "OS PASSAGEIROS",
        stamp: "TOP SECRET",
        posX: 65,
        posY: 50,
        rotation: -5,
        contentClean: `
            <h3>Ameaças Comportamentais:</h3>
            <p><strong>Criança 1 (O Inquieto):</strong> Troca de lugar frequentemente. Ataque letal após 30s na primeira fileira.</p>
            <p><strong>Criança 2 (O Risonho):</strong> Emite risadas antes de apertar os botões do seu painel e sabotar o ônibus.</p>
            <p><strong>Criança 3 (O Contador):</strong> Sussurra contos no rádio. Bloqueia a visão, pânico iminente.</p>
            <p><strong>Criança 4 (O Observador):</strong> Encara você fixamente pela câmera central. O tempo de reação é mínimo.</p>
        `,
        contentCorrupt: `
            <h3>As Vítimas do Passado:</h3>
            <p>Eles não são crianças de verdade. São fragmentos de culpa do acidente fatal.</p>
            <p>Eles repetem as exatas ações que causaram a tragédia original. Só que agora, pra te matar.</p>
            <br>
            <span class="macabre-text">A Criança 5... o Artista... ele pinta de vermelho o teto... com o seu sangue.</span>
        `
    },
    {
        id: "doc-jornada",
        type: "doc-folder",
        title: "CRONOGRAMA (D1-5)",
        stamp: "ALERTA",
        posX: 35,
        posY: 10,
        rotation: 12,
        contentClean: `
            <p><strong>Dias 1 ao 3 (A Dúvida):</strong> O motorista nota falhas elétricas sutis. Aparições silenciosas. O evento começa a se formar.</p>
            <p><strong>Dia 4 (O Pesadelo):</strong> A revelação no rádio. Uma transmissão policial detalha o acidente que ocorreu exatamente há 1 ano atrás.</p>
            <p><strong>Dia 5 (Caos Total):</strong> Todas as anomalias ativas e extremamente agressivas. O clima tenta expulsar o ônibus da estrada.</p>
        `,
        contentCorrupt: `
            <p><strong>Dia 1 ao 3:</strong> A Negação. Você acha que é só o estresse de um trabalho noturno ruim.</p>
            <p><strong>Dia 4:</strong> O Choque. Você se lembra do volante molhado de sangue, dos gritos. <em>Foi culpa sua.</em></p>
            <p><strong>Dia 5:</strong> O Julgamento. O Diretor Siqueira não é humano, é o carcereiro do seu purgatório.</p>
        `
    },
    {
        id: "doc-finais",
        type: "doc-polaroid",
        title: "DESTINOS",
        stamp: null,
        posX: 80,
        posY: 80,
        rotation: -15,
        contentClean: `
            <p>As avaliações do contrato indicam 3 possíveis desfechos avaliados:</p>
            <ul>
                <li><strong>Final Bom:</strong> Desempenho excelente sem acidentes críticos.</li>
                <li><strong>Final Médio:</strong> Chega ao destino final com danos altos ao veículo.</li>
                <li><strong>Final Ruim:</strong> Falha crítica durante o trajeto. Veículo destruído.</li>
            </ul>
        `,
        contentCorrupt: `
            <p><strong>1. REDENÇÃO:</strong> Sobrevive intacto. Encontra seus próprios ossos no acidente, percebe que é um fantasma e descansa.</p>
            <p><strong>2. TRAUMA:</strong> Foge da cidade a pé. A loucura te acompanha e você acorda num hospital psiquiátrico.</p>
            <p><strong>3. O CICLO (Ruim):</strong> Você morre no volante. Sua alma é redefinida para o Dia 1. A vaga é reaberta.</p>
            <span class="macabre-text">Adivinha em qual final nós estamos presos agora? Bem vindo ao loop 48.</span>
        `
    }
];

class TensionManager {
    constructor() {
        this.docsRead = new Set();
        this.tensionLevel = 0;
        this.barUI = document.getElementById('tension-bar');
        this.valueUI = document.getElementById('tension-value');
        this.isCorrupted = false;
        this.incrementPerDoc = 100 / gameDocs.length;
    }

    registerRead(docId) {
        if (!this.docsRead.has(docId)) {
            this.docsRead.add(docId);
            this.tensionLevel += this.incrementPerDoc;
            if (this.tensionLevel > 100) this.tensionLevel = 100;
            this.updateUI();
            this.checkState();
        }
    }

    updateUI() {
        this.barUI.style.width = this.tensionLevel + '%';
        if (this.tensionLevel < 45) {
            this.valueUI.innerText = "BAIXO";
            this.barUI.style.backgroundColor = "#4caf50";
        } else if (this.tensionLevel < 80) {
            this.valueUI.innerText = "ALERTA - ATIVIDADE PARANORMAL";
            this.barUI.style.backgroundColor = "#ff9800";
        } else {
            this.valueUI.innerText = "CRÍTICO - INSANIDADE";
            this.barUI.style.backgroundColor = "#d32f2f";
        }
    }

    checkState() {
        if (this.tensionLevel > 40 && !this.isCorrupted) {
            this.isCorrupted = true;
            document.body.classList.add('corrupted');
        }
        if (this.tensionLevel >= 95) {
            document.body.classList.add('tension-critical');
        }
        if (this.isCorrupted) {
            gameDocs.forEach(doc => {
                const el = document.getElementById(doc.id);
                if (el) {
                    const contentContainer = el.querySelector('.content-container');
                    if (contentContainer) {
                        contentContainer.innerHTML = doc.contentCorrupt;
                    }
                }
            });
        }
    }
}

const tensionSystem = new TensionManager();

class DeskController {
    constructor(docsData) {
        this.desk = document.getElementById('desk');
        this.closeBtn = document.getElementById('close-btn');
        this.docsData = docsData;
        this.activeDoc = null;
        this.init();
    }

    init() {
        this.docsData.forEach((doc, index) => {
            const article = document.createElement('article');
            article.id = doc.id;
            article.className = "document " + doc.type + " idle";
            
            article.style.left = doc.posX + "vw";
            article.style.top = doc.posY + "vh";
            article.style.transform = "rotate(" + doc.rotation + "deg)";
            article.dataset.originX = doc.posX;
            article.dataset.originY = doc.posY;
            article.dataset.originRot = doc.rotation;
            article.style.zIndex = index;

            let stampHtml = doc.stamp ? '<div class="stamp">' + doc.stamp + '</div>' : '';
            
            article.innerHTML = stampHtml +
                '<h2 class="doc-title">' + doc.title + '</h2>' +
                '<div class="doc-content content-container">' +
                    doc.contentClean +
                '</div>';

            article.addEventListener('click', (e) => {
                if (this.activeDoc === article) return;
                this.focusDocument(article);
                e.stopPropagation();
            });

            this.desk.appendChild(article);
        });

        this.desk.addEventListener('click', () => {
            if (this.activeDoc) this.releaseDocument();
        });

        this.closeBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            this.releaseDocument();
        });
    }

    focusDocument(docEl) {
        if (this.activeDoc) {
            this.releaseDocument();
            setTimeout(() => this.executeFocus(docEl), 300);
        } else {
            this.executeFocus(docEl);
        }
    }

    executeFocus(docEl) {
        this.activeDoc = docEl;
        tensionSystem.registerRead(docEl.id);

        Array.from(this.desk.children).forEach(el => {
            if (el !== docEl) el.classList.add('hidden-bg');
        });

        this.closeBtn.classList.remove('hidden');

        docEl.classList.remove('idle');
        docEl.classList.add('focused');

        docEl.style.left = '50%';
        docEl.style.top = '50%';
        docEl.style.transform = "translate(-50%, -50%) rotate(0deg) scale(1.15)";
        docEl.scrollTop = 0; 
    }

    releaseDocument() {
        if (!this.activeDoc) return;

        const docEl = this.activeDoc;
        docEl.classList.remove('focused');
        docEl.classList.add('idle');
        
        docEl.style.left = docEl.dataset.originX + "vw";
        docEl.style.top = docEl.dataset.originY + "vh";
        docEl.style.transform = "rotate(" + docEl.dataset.originRot + "deg)";

        this.closeBtn.classList.add('hidden');

        Array.from(this.desk.children).forEach(el => {
            el.classList.remove('hidden-bg');
        });

        this.activeDoc = null;
    }
}

document.addEventListener("DOMContentLoaded", () => {
    new DeskController(gameDocs);
});
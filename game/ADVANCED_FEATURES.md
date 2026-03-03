# Bus Shift Narrative - Advanced Features Guide

Documento com sugestões e snippets de código para estender a página narrativa com features avançadas.

## 🎵 Audio Ambience System

### Implementação Web Audio API

```javascript
class AmbienceAudioSystem {
    constructor() {
        this.context = new (window.AudioContext || window.webkitAudioContext)();
        this.masterGain = this.context.createGain();
        this.masterGain.connect(this.context.destination);
        this.masterGain.gain.value = 0.3;
    }

    playAmbientLoop(freq, duration = 5) {
        const osc = this.context.createOscillator();
        const gain = this.context.createGain();
        
        osc.connect(gain);
        gain.connect(this.masterGain);
        
        osc.frequency.value = freq;
        osc.type = 'sine';
        
        gain.gain.setValueAtTime(0, this.context.currentTime);
        gain.gain.linearRampToValueAtTime(0.05, this.context.currentTime + 0.5);
        gain.gain.linearRampToValueAtTime(0, this.context.currentTime + duration);
        
        osc.start(this.context.currentTime);
        osc.stop(this.context.currentTime + duration);
    }

    // Chamadas por seção
    playIntroAmbience() {
        // Neblina sonora
        this.playAmbientLoop(60, 8);  // Tom grave
        setTimeout(() => this.playAmbientLoop(75, 8), 2000);
    }

    playDay1Ambience() {
        // Som de ônibus motor
        this.playAmbientLoop(100, 6);
    }

    playDay4Ambience() {
        // Som mais sinistro
        this.playAmbientLoop(85, 8);
        setTimeout(() => this.playAmbientLoop(110, 8), 3000);
    }
}
```

## 🚌 Bus Interior Environment

### Renderizar cabine do ônibus com Canvas

```javascript
class BusInteriorRenderer {
    constructor(canvasElement) {
        this.canvas = canvasElement;
        this.ctx = canvasElement.getContext('2d');
        this.seats = [];
        this.ghostSeat = null;
        this.init();
    }

    init() {
        this.generateSeats();
        this.animationFrame = requestAnimationFrame(() => this.render());
    }

    generateSeats() {
        // Gerar 12 assentos (6 fileiras, 2 cada)
        for (let i = 0; i < 12; i++) {
            this.seats.push({
                id: i + 1,
                row: Math.floor(i / 2),
                side: i % 2,
                occupied: false,
                opacity: 0.8
            });
        }
        
        // Assento 6 é o fantasma
        this.ghostSeat = {
            id: 6,
            row: 2,
            side: 1,
            isGhost: true,
            opacity: 0.3,
            distortion: 0
        };
    }

    render() {
        this.ctx.fillStyle = 'rgba(10, 10, 10, 1)';
        this.ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);

        // Desenhar linha do meio (corredor)
        this.ctx.strokeStyle = 'rgba(212, 165, 116, 0.2)';
        this.ctx.beginPath();
        this.ctx.moveTo(this.canvas.width / 2, 0);
        this.ctx.lineTo(this.canvas.width / 2, this.canvas.height);
        this.ctx.stroke();

        // Desenhar assentos normais
        this.seats.forEach(seat => this.drawSeat(seat));

        // Desenhar assento fantasma com distorção
        if (this.ghostSeat) {
            this.ghostSeat.distortion += 0.02;
            this.drawGhostSeat(this.ghostSeat);
        }

        this.animationFrame = requestAnimationFrame(() => this.render());
    }

    drawSeat(seat) {
        const x = seat.side === 0 ? 100 : this.canvas.width - 150;
        const y = 100 + seat.row * 80;
        const width = 60;
        const height = 50;

        this.ctx.fillStyle = `rgba(168, 120, 96, ${seat.opacity})`;
        this.ctx.fillRect(x, y, width, height);

        // Border
        this.ctx.strokeStyle = 'rgba(212, 165, 116, 0.4)';
        this.ctx.lineWidth = 2;
        this.ctx.strokeRect(x, y, width, height);
    }

    drawGhostSeat(seat) {
        const x = seat.side === 0 ? 100 : this.canvas.width - 150;
        const y = 100 + seat.row * 80;
        const width = 60;
        const height = 50;

        // Distorção ondulatória
        const distortion = Math.sin(seat.distortion) * 5;

        this.ctx.globalAlpha = seat.opacity;
        this.ctx.fillStyle = 'rgba(184, 134, 11, 0.4)';
        this.ctx.fillRect(x + distortion, y + distortion, width, height);

        // Contorno piscante
        this.ctx.strokeStyle = `rgba(212, 165, 116, ${0.6 + Math.sin(seat.distortion) * 0.3})`;
        this.ctx.lineWidth = 1.5;
        this.ctx.strokeRect(x + distortion, y + distortion, width, height);

        this.ctx.globalAlpha = 1;
    }

    destroy() {
        cancelAnimationFrame(this.animationFrame);
    }
}
```

**Integração no HTML:**
```html
<div class="bus-interior-section">
    <canvas id="busInterior" width="800" height="600"></canvas>
</div>

<script>
    let busRenderer;
    const busCanvas = document.getElementById('busInterior');
    if (busCanvas) {
        busRenderer = new BusInteriorRenderer(busCanvas);
    }
</script>
```

## 🖼️ Parallax Image Backgrounds

### Adicionar backgrounds para cada dia

```css
.narrative-section {
    position: relative;
    background-attachment: fixed;
    background-size: cover;
    background-position: center;
}

.intro {
    background-image: 
        linear-gradient(135deg, rgba(10, 10, 10, 0.8), rgba(26, 26, 26, 0.9)),
        url('data:image/svg+xml,...');
    background-blend-mode: multiply;
}

.day-1 {
    background-image:
        linear-gradient(135deg, rgba(10, 10, 10, 0.85), rgba(42, 42, 42, 0.85)),
        url('data:image/svg+xml,...');
}

.day-4 {
    background-color: #1a1a1a;
    box-shadow: inset 0 0 100px rgba(0, 0, 0, 0.8);
}
```

## 🎞️ Save Progress Feature

### Local Storage Para Salvar Progresso

```javascript
class ProgressSaver {
    constructor() {
        this.storageKey = 'busShift_narrative_progress';
        this.loadProgress();
    }

    saveProgress(section, timestamp = Date.now()) {
        const progress = {
            lastSection: section,
            lastViewed: timestamp,
            sectionsCompleted: this.sectionsCompleted || [],
            readingTime: this.getTotalReadingTime()
        };

        if (!progress.sectionsCompleted.includes(section)) {
            progress.sectionsCompleted.push(section);
        }

        localStorage.setItem(this.storageKey, JSON.stringify(progress));
    }

    loadProgress() {
        const saved = localStorage.getItem(this.storageKey);
        if (saved) {
            this.progress = JSON.parse(saved);
            return this.progress;
        }
        return null;
    }

    getTotalReadingTime() {
        if (!this.lastTimestamp) return 0;
        return (Date.now() - this.lastTimestamp) / 1000; // segundos
    }

    getProgressPercentage() {
        const sections = ['intro', '1', '2', '3', '4', '5'];
        if (!this.progress?.sectionsCompleted) return 0;
        return (this.progress.sectionsCompleted.length / sections.length) * 100;
    }

    // Integrar com NarrativeController
    createProgressBar() {
        const container = document.createElement('div');
        container.className = 'progress-bar-container';
        container.innerHTML = `
            <div class="progress-bar">
                <div class="progress-fill" style="width: ${this.getProgressPercentage()}%"></div>
            </div>
        `;
        return container;
    }
}
```

## 🎬 Scene Transitions

### Transições Cinematográficas Entre Seções

```javascript
class SceneTransition {
    static async fadeToBlack(duration = 500) {
        const overlay = document.createElement('div');
        overlay.className = 'fade-overlay';
        overlay.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: #000;
            opacity: 0;
            z-index: 999;
            transition: opacity ${duration}ms ease-in-out;
        `;
        document.body.appendChild(overlay);

        // Fade in
        window.requestAnimationFrame(() => {
            overlay.style.opacity = '1';
        });

        return new Promise(resolve => {
            setTimeout(() => {
                overlay.style.opacity = '0';
                setTimeout(() => {
                    overlay.remove();
                    resolve();
                }, duration);
            }, duration);
        });
    }

    static async slideFromSide(duration = 800) {
        const overlay = document.createElement('div');
        overlay.className = 'slide-overlay';
        overlay.style.cssText = `
            position: fixed;
            top: 0;
            right: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, #0a0a0a, transparent);
            z-index: 998;
            transition: right ${duration}ms cubic-bezier(0.25, 0.46, 0.45, 0.94);
        `;
        document.body.appendChild(overlay);

        window.requestAnimationFrame(() => {
            overlay.style.right = '0';
        });

        return new Promise(resolve => {
            setTimeout(() => {
                overlay.remove();
                resolve();
            }, duration);
        });
    }
}

// Uso
narrativeController.navigateToSection = async function(section) {
    await SceneTransition.fadeToBlack(300);
    // ... change section
    await SceneTransition.slideFromSide(600);
};
```

## 🎯 Interactive Choices (Branching Narrative)

### Sistema de Escolhas

```javascript
class ChoiceSystem {
    constructor() {
        this.choices = new Map();
        this.playerChoices = new Map();
    }

    registerChoice(sectionId, text, consequence) {
        if (!this.choices.has(sectionId)) {
            this.choices.set(sectionId, []);
        }
        this.choices.get(sectionId).push({ text, consequence });
    }

    renderChoiceButtons(sectionId) {
        const container = document.createElement('div');
        container.className = 'choices-container';

        const choices = this.choices.get(sectionId) || [];
        
        choices.forEach((choice, index) => {
            const button = document.createElement('button');
            button.className = 'choice-button';
            button.textContent = choice.text;
            button.style.cssText = `
                display: block;
                width: 100%;
                padding: 16px 20px;
                margin: 12px 0;
                background: rgba(212, 165, 116, 0.1);
                border: 1px solid rgba(212, 165, 116, 0.3);
                color: #e8e8e8;
                cursor: pointer;
                font-family: Georgia, serif;
                border-radius: 4px;
                transition: all 300ms;
            `;

            button.addEventListener('mouseenter', () => {
                button.style.background = 'rgba(212, 165, 116, 0.2)';
                button.style.borderColor = 'rgba(212, 165, 116, 0.6)';
            });

            button.addEventListener('mouseleave', () => {
                button.style.background = 'rgba(212, 165, 116, 0.1)';
                button.style.borderColor = 'rgba(212, 165, 116, 0.3)';
            });

            button.addEventListener('click', () => {
                this.playerChoices.set(sectionId, index);
                this.executeConsequence(choice.consequence);
            });

            container.appendChild(button);
        });

        return container;
    }

    executeConsequence(consequence) {
        if (typeof consequence === 'function') {
            consequence();
        } else if (typeof consequence === 'string') {
            // Navegar para seção resultante
            narrativeController.navigateToSection(consequence);
        }
    }
}
```

## 📊 Analytics Dashboard

### Rastrear Leitura de Usuários

```javascript
class NarrativeAnalytics {
    constructor() {
        this.sessionStart = Date.now();
        this.events = [];
        this.sectionReadTimes = {};
    }

    trackEvent(eventType, section, data = {}) {
        this.events.push({
            type: eventType,
            section,
            timestamp: Date.now(),
            data
        });
    }

    trackSectionRead(section, duration) {
        if (!this.sectionReadTimes[section]) {
            this.sectionReadTimes[section] = 0;
        }
        this.sectionReadTimes[section] += duration;
        this.trackEvent('section_read', section, { duration });
    }

    generateReport() {
        const totalTime = (Date.now() - this.sessionStart) / 1000;
        const avgTimePerSection = totalTime / Object.keys(this.sectionReadTimes).length;

        return {
            totalSessionTime: totalTime,
            sectionsRead: Object.keys(this.sectionReadTimes).length,
            avgTimePerSection,
            mostReread: this.getMostRereadSection(),
            completionPercentage: (Object.keys(this.sectionReadTimes).length / 6) * 100,
            events: this.events
        };
    }

    getMostRereadSection() {
        return Object.entries(this.sectionReadTimes).reduce((a, b) => 
            a[1] > b[1] ? a : b
        )[0];
    }

    exportAsJSON() {
        return JSON.stringify(this.generateReport(), null, 2);
    }
}
```

## 🌙 Dark/Light Mode Toggle

### Implementação de Tema

```javascript
class ThemeToggle {
    constructor() {
        this.isDarkMode = true;
        this.init();
    }

    init() {
        const toggle = document.createElement('button');
        toggle.id = 'theme-toggle';
        toggle.textContent = '☀️';
        toggle.style.cssText = `
            position: fixed;
            top: 20px;
            left: 20px;
            z-index: 101;
            background: rgba(212, 165, 116, 0.2);
            border: 1px solid rgba(212, 165, 116, 0.3);
            color: #d4a574;
            width: 40px;
            height: 40px;
            border-radius: 50%;
            cursor: pointer;
            font-size: 20px;
            transition: all 300ms;
        `;

        toggle.addEventListener('click', () => this.toggle());
        document.body.appendChild(toggle);
    }

    toggle() {
        this.isDarkMode = !this.isDarkMode;
        const root = document.documentElement;

        if (this.isDarkMode) {
            root.style.setProperty('--color-night', '#0a0a0a');
            root.style.setProperty('--color-text', '#e8e8e8');
            document.getElementById('theme-toggle').textContent = '☀️';
        } else {
            root.style.setProperty('--color-night', '#f5f5f5');
            root.style.setProperty('--color-text', '#1a1a1a');
            document.getElementById('theme-toggle').textContent = '🌙';
        }

        localStorage.setItem('busShift_theme', this.isDarkMode ? 'dark' : 'light');
    }

    loadSavedTheme() {
        const saved = localStorage.getItem('busShift_theme');
        if (saved === 'light') {
            this.isDarkMode = false;
            this.toggle();
        }
    }
}
```

## 📖 Custom Fonts & Typography

Adicione no `<head>` do HTML:

```html
<link href="https://fonts.googleapis.com/css2?family=EB+Garamond:ital@0;1&display=swap" rel="stylesheet">

<style>
    :root {
        --font-serif: 'EB Garamond', 'Georgia', serif;
    }
</style>
```

## 🔗 Share Progress Feature

```javascript
class ShareProgress {
    generateShareText() {
        const section = narrativeController.currentSection;
        const sectionNames = {
            'intro': 'Começou a explorar Ravenswood',
            '1': 'Pegou o volante do Ônibus 104',
            '2': 'Conheceu os passageiros',
            '3': 'Viu o assento 6',
            '4': 'Entendeu Victor Holder',
            '5': 'Enfrentou a escolha final'
        };

        return `🚌 Estou lendo Bus Shift - "${sectionNames[section]}". Você deveria ler também!`;
    }

    shareToTwitter() {
        const text = encodeURIComponent(this.generateShareText());
        window.open(`https://twitter.com/intent/tweet?text=${text}`);
    }

    copyToClipboard() {
        const text = this.generateShareText();
        navigator.clipboard.writeText(text);
    }
}
```

## 📱 PWA - Progressive Web App

Crie `manifest.json`:

```json
{
    "name": "Bus Shift Narrative",
    "short_name": "Bus Shift",
    "description": "Uma história atmosférica sobre um motorista de ônibus",
    "start_url": "/narrative.html",
    "display": "standalone",
    "background_color": "#0a0a0a",
    "theme_color": "#d4a574",
    "icons": [
        {
            "src": "icon-192.png",
            "sizes": "192x192",
            "type": "image/png"
        }
    ]
}
```

Adicione no `<head>`:
```html
<link rel="manifest" href="manifest.json">
<meta name="theme-color" content="#0a0a0a">
```

---

**Selecione features baseado em seu objetivo:**

- **Atmosfera ++**: Audio Ambience + Parallax Backgrounds
- **Engagement ++**: Progress Saver + Choices + Analytics
- **Visual ++**: Bus Interior Renderer + Scene Transitions
- **Social ++**: Share Progress + PWA
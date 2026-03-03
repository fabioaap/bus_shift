// ========================================
// BUS SHIFT - NARRATIVE PAGE CONTROLLER
// ========================================

class NarrativeController {
    constructor() {
        this.currentSection = 'intro';
        this.sections = ['intro', '1', '2', '3', '4', '5'];
        this.navDots = document.querySelectorAll('.nav-dot');
        this.narrativeSections = document.querySelectorAll('.narrative-section');
        this.init();
    }

    init() {
        this.setupEventListeners();
        this.setupScrollObserver();
        this.setupAmbience();
        this.activateSection('intro');
    }

    setupEventListeners() {
        // Navigation dots
        this.navDots.forEach((dot, index) => {
            dot.addEventListener('click', () => {
                const section = this.sections[index];
                this.navigateToSection(section);
            });
        });

        // CTA button
        const ctaExplore = document.querySelector('.cta-explore');
        if (ctaExplore) {
            ctaExplore.addEventListener('click', () => {
                this.navigateToSection('1');
            });
        }

        // Keyboard navigation
        document.addEventListener('keydown', (e) => {
            if (e.key === 'ArrowRight' || e.key === ' ') {
                this.nextSection();
            } else if (e.key === 'ArrowLeft') {
                this.previousSection();
            }
        });
    }

    setupScrollObserver() {
        const observerOptions = {
            threshold: 0.3,
            rootMargin: '0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const section = entry.target.getAttribute('data-section');
                    if (section) {
                        this.updateActiveSection(section);
                    }
                }
            });
        }, observerOptions);

        this.narrativeSections.forEach(section => observer.observe(section));
    }

    setupAmbience() {
        // Parallax effect on scroll
        window.addEventListener('scroll', () => {
            const scrolled = window.scrollY;
            const fog = document.querySelector('.fog');
            if (fog) {
                fog.style.transform = `translateY(${scrolled * 0.5}px)`;
            }
        });

        // Mouse mirror distortion effect
        document.addEventListener('mousemove', (e) => {
            const mirror = document.getElementById('mirror-distort');
            if (mirror) {
                const mouseX = (e.clientX / window.innerWidth) * 100;
                const mouseY = (e.clientY / window.innerHeight) * 100;
                
                // Subtle distortion based on mouse position
                const offset = (mouseX - 50) * 0.1;
                mirror.style.cx = `calc(35px + ${offset}px)`;
            }
        });

        // Audio ambience (optional - can be enabled)
        this.initOptionalAudio();
    }

    initOptionalAudio() {
        // Create audio context for ambient sounds
        const AudioContext = window.AudioContext || window.webkitAudioContext;
        if (!AudioContext) return;

        this.audioContext = new AudioContext();
    }

    playAmbientSound(frequency = 100, duration = 3) {
        if (!this.audioContext) return;

        const now = this.audioContext.currentTime;
        const oscillator = this.audioContext.createOscillator();
        const gainNode = this.audioContext.createGain();

        oscillator.connect(gainNode);
        gainNode.connect(this.audioContext.destination);

        oscillator.frequency.value = frequency;
        oscillator.type = 'sine';

        gainNode.gain.setValueAtTime(0.05, now);
        gainNode.gain.exponentialRampToValueAtTime(0.01, now + duration);

        oscillator.start(now);
        oscillator.stop(now + duration);
    }

    navigateToSection(section) {
        if (!this.sections.includes(section)) return;

        this.activateSection(section);
        
        // Scroll to section
        const sectionElement = document.querySelector(`[data-section="${section}"]`);
        if (sectionElement) {
            sectionElement.scrollIntoView({ behavior: 'smooth' });
        }
    }

    activateSection(section) {
        // Update active narrative section
        this.narrativeSections.forEach(el => {
            el.classList.remove('active');
        });

        const activeSection = document.querySelector(`[data-section="${section}"]`);
        if (activeSection) {
            activeSection.classList.add('active');
        }

        // Update navigation dots
        const sectionIndex = this.sections.indexOf(section);
        this.navDots.forEach((dot, index) => {
            dot.classList.toggle('active', index === sectionIndex);
        });

        this.currentSection = section;

        // Trigger section-specific ambience
        this.triggerSectionAmbience(section);
    }

    updateActiveSection(section) {
        if (section !== this.currentSection) {
            const sectionIndex = this.sections.indexOf(section);
            if (sectionIndex !== -1) {
                this.navDots.forEach((dot, index) => {
                    dot.classList.toggle('active', index === sectionIndex);
                });
                this.currentSection = section;
            }
        }
    }

    nextSection() {
        const currentIndex = this.sections.indexOf(this.currentSection);
        if (currentIndex < this.sections.length - 1) {
            this.navigateToSection(this.sections[currentIndex + 1]);
        }
    }

    previousSection() {
        const currentIndex = this.sections.indexOf(this.currentSection);
        if (currentIndex > 0) {
            this.navigateToSection(this.sections[currentIndex - 1]);
        }
    }

    triggerSectionAmbience(section) {
        // Different audio signatures for different sections
        const frequencies = {
            'intro': 80,
            '1': 100,
            '2': 120,
            '3': 140,
            '4': 160,
            '5': 200
        };

        const frequency = frequencies[section] || 100;
        // Uncomment to enable ambient sounds
        // this.playAmbientSound(frequency, 2);
    }
}

// ========================================
// INTERACTIVE DIALOGUE
// ========================================

class DialogueSystem {
    constructor() {
        this.dialogues = document.querySelectorAll('.character-dialogue');
        this.init();
    }

    init() {
        // Stagger dialogue appearance
        this.dialogues.forEach((dialogue, index) => {
            dialogue.style.animation = `dialogueSlideIn 600ms ease-out ${index * 100}ms forwards`;
            dialogue.style.opacity = '0';
        });
    }
}

// ========================================
// MIRROR DISTORTION EFFECT
// ========================================

class MirrorEffect {
    constructor() {
        this.mirror = document.getElementById('mirror-distort');
        this.mouseX = 0;
        this.mouseY = 0;
        this.init();
    }

    init() {
        document.addEventListener('mousemove', (e) => {
            this.mouseX = e.clientX;
            this.mouseY = e.clientY;
            this.updateMirror();
        });
    }

    updateMirror() {
        if (!this.mirror) return;

        const rect = this.mirror.getBoundingClientRect();
        const distanceX = this.mouseX - rect.left;
        const distanceY = this.mouseY - rect.top;

        const distance = Math.sqrt(
            Math.pow(distanceX - rect.width / 2, 2) +
            Math.pow(distanceY - rect.height / 2, 2)
        );

        // Distortion effect distance threshold
        const threshold = 200;
        if (distance < threshold) {
            const intensity = (1 - distance / threshold) * 0.3;
            this.mirror.style.filter = `blur(${intensity}px)`;
        } else {
            this.mirror.style.filter = 'blur(0)';
        }
    }
}

// ========================================
// PAGE STATISTICS & ANALYTICS
// ========================================

class PageAnalytics {
    constructor() {
        this.startTime = Date.now();
        this.sectionTimes = {};
        this.init();
    }

    init() {
        // Track time spent on each section
        window.addEventListener('beforeunload', () => {
            this.recordSessionData();
        });
    }

    recordSectionChange(section) {
        const now = Date.now();
        if (this.currentSection) {
            if (!this.sectionTimes[this.currentSection]) {
                this.sectionTimes[this.currentSection] = 0;
            }
            this.sectionTimes[this.currentSection] += (now - this.lastSectionChange);
        }
        this.lastSectionChange = now;
        this.currentSection = section;
    }

    recordSessionData() {
        // Optional: Send to analytics
        const sessionData = {
            duration: Date.now() - this.startTime,
            sections: this.sectionTimes,
            highestSection: this.getCurrentSection()
        };
        // console.log('Session Data:', sessionData);
    }

    getCurrentSection() {
        return this.currentSection;
    }
}

// ========================================
// RESPONSIVE BUS VISUALIZATION
// ========================================

class BusVisualization {
    constructor() {
        this.busSVG = document.querySelector('.bus-svg');
        this.init();
    }

    init() {
        if (!this.busSVG) return;

        // Responsive SVG sizing
        this.updateBusDimensions();
        window.addEventListener('resize', () => this.updateBusDimensions());

        // Hover animations
        const busReveal = document.querySelector('.bus-reveal');
        if (busReveal) {
            busReveal.addEventListener('mouseenter', () => {
                this.busSVG.style.filter = 
                    'drop-shadow(0 4px 20px rgba(212, 165, 116, 0.4)) brightness(1.1)';
            });

            busReveal.addEventListener('mouseleave', () => {
                this.busSVG.style.filter = 'drop-shadow(0 4px 8px rgba(0, 0, 0, 0.6))';
            });
        }
    }

    updateBusDimensions() {
        const container = this.busSVG?.parentElement;
        if (container) {
            const width = container.offsetWidth;
            this.busSVG.style.maxWidth = Math.min(width, 500) + 'px';
        }
    }
}

// ========================================
// NARRATIVE HINTS & EASTER EGGS
// ========================================

class EasterEggs {
    constructor() {
        this.eggSequence = [];
        this.eggKeys = ['KeyK', 'KeyA', 'KeyY', 'KeyL', 'KeyE', 'KeyE'];
        this.init();
    }

    init() {
        document.addEventListener('keydown', (e) => {
            this.handleKeyPress(e.code);
        });
    }

    handleKeyPress(key) {
        this.eggSequence.push(key);

        // Limit array size
        if (this.eggSequence.length > 10) {
            this.eggSequence.shift();
        }

        // Check for "KAYLEE" sequence
        if (this.eggSequence.join('').includes(
            this.eggKeys.join('')
        )) {
            this.triggerEasterEgg();
            this.eggSequence = [];
        }
    }

    triggerEasterEgg() {
        // Subtle visual effect for easter egg
        const container = document.querySelector('.narrative-container');
        if (container) {
            container.style.animation = 'easterEgg 3s ease-in-out';
            
            setTimeout(() => {
                container.style.animation = '';
            }, 3000);
        }

        // Could also trigger a hidden message
        console.log('✨ Você encontrou uma pista sobre Kaylee...');
    }
}

// ========================================
// INITIALIZATION
// ========================================

document.addEventListener('DOMContentLoaded', () => {
    // Initialize all controllers
    const narrativeController = new NarrativeController();
    const dialogueSystem = new DialogueSystem();
    const mirrorEffect = new MirrorEffect();
    const pageAnalytics = new PageAnalytics();
    const busVisualization = new BusVisualization();
    const easterEggs = new EasterEggs();

    // Expose controller globally for debugging
    window.narrativeController = narrativeController;

    // Optional: Initialize with smooth fade-in
    document.body.classList.add('loaded');

    // Log initialization complete
    console.log('🚌 Bus Shift Narrative loaded. Safe travels.');
});

// ========================================
// EASTER EGG ANIMATION
// ========================================

const style = document.createElement('style');
style.textContent = `
    @keyframes easterEgg {
        0%, 100% {
            filter: hue-rotate(0deg);
            transform: scale(1);
        }
        25% {
            filter: hue-rotate(-10deg);
            transform: scale(1.01);
        }
        50% {
            filter: hue-rotate(10deg);
            transform: scale(1);
        }
        75% {
            filter: hue-rotate(-10deg);
            transform: scale(1.01);
        }
    }
`;
document.head.appendChild(style);
// Horror System - Manages tension, glitches, and creepy effects

const HORROR_CONFIG = (typeof window !== 'undefined' && window.CONFIG)
    ? window.CONFIG
    : require('./config.js');

const HORROR_UTILS = (typeof window !== 'undefined' && window.Utils)
    ? window.Utils
    : require('./utils.js');

class HorrorSystem {
    constructor() {
        this.tension = 0;
        this.glitchActive = false;
        this.shadowVisible = false;
        this.whisperTriggered = false;
        
        // Effect timers
        this.glitchTimer = 0;
        this.shadowTimer = 0;
        this.whisperTimer = 0;
    }
    
    update(deltaTime) {
        // Gradually build tension
        if (this.tension < HORROR_CONFIG.horror.maxTension) {
            this.tension += HORROR_CONFIG.horror.tensionBuildRate;
        }
        
        // Random horror events based on tension
        this.checkGlitch();
        this.checkShadow();
        this.checkWhisper();
        
        // Update effect timers
        this.updateTimers(deltaTime);
    }
    
    checkGlitch() {
        if (Math.random() < HORROR_CONFIG.horror.glitchChance * this.tension) {
            this.triggerGlitch();
        }
    }
    
    checkShadow() {
        if (Math.random() < HORROR_CONFIG.horror.shadowChance * this.tension) {
            this.triggerShadow();
        }
    }
    
    checkWhisper() {
        if (Math.random() < HORROR_CONFIG.horror.whisperChance * this.tension) {
            this.triggerWhisper();
        }
    }
    
    triggerGlitch() {
        if (this.glitchActive) return;
        
        this.glitchActive = true;
        this.glitchTimer = 200; // ms
        
        // Visual distortion on canvas
        const canvas = document.getElementById('gameCanvas');
        if (canvas) {
            canvas.style.filter = `hue-rotate(${HORROR_UTILS.randomInt(0, 360)}deg) contrast(${HORROR_UTILS.random(1.2, 1.5)})`;
        }
        
        // Message glitch
        const messageEl = document.getElementById('center-message');
        if (messageEl && Math.random() < 0.5) {
            const messages = [
                "DON'T LOOK BACK",
                "KEEP DRIVING",
                "IT'S GETTING CLOSER",
                "NO ESCAPE",
                "WHY DID YOU STOP?"
            ];
            const originalText = messageEl.textContent;
            messageEl.textContent = HORROR_UTILS.choose(messages);
            
            setTimeout(() => {
                messageEl.textContent = originalText;
            }, 300);
        }
    }
    
    triggerShadow() {
        if (this.shadowVisible) return;
        
        this.shadowVisible = true;
        this.shadowTimer = 1000; // ms
        
        // Flash warning light
        const warningLights = document.querySelectorAll('.warning-light');
        warningLights.forEach(light => {
            light.classList.add('active');
        });
    }
    
    triggerWhisper() {
        if (this.whisperTriggered) return;
        
        this.whisperTriggered = true;
        this.whisperTimer = 3000; // ms
        
        // Add subtle note to document panel
        const notesContainer = document.querySelector('.document-notes');
        if (notesContainer && Math.random() < 0.3) {
            const whisper = document.createElement('p');
            whisper.className = 'note-text faded';
            whisper.textContent = HORROR_UTILS.choose([
                "I can hear it breathing...",
                "The mirrors show more than they should...",
                "Why are the passengers so quiet?",
                "This road never ends..."
            ]);
            notesContainer.appendChild(whisper);
            
            setTimeout(() => {
                whisper.remove();
            }, 3000);
        }
    }
    
    updateTimers(deltaTime) {
        // Glitch timer
        if (this.glitchActive) {
            this.glitchTimer -= deltaTime;
            if (this.glitchTimer <= 0) {
                this.glitchActive = false;
                const canvas = document.getElementById('gameCanvas');
                if (canvas) {
                    canvas.style.filter = '';
                }
            }
        }
        
        // Shadow timer
        if (this.shadowVisible) {
            this.shadowTimer -= deltaTime;
            if (this.shadowTimer <= 0) {
                this.shadowVisible = false;
                const warningLights = document.querySelectorAll('.warning-light');
                warningLights.forEach(light => {
                    light.classList.remove('active');
                });
            }
        }
        
        // Whisper timer
        if (this.whisperTriggered) {
            this.whisperTimer -= deltaTime;
            if (this.whisperTimer <= 0) {
                this.whisperTriggered = false;
            }
        }
    }
    
    getScreenShake() {
        if (this.glitchActive) {
            return HORROR_CONFIG.horror.shakeIntensity * this.tension;
        }
        return 0;
    }
    
    getDistortion() {
        if (this.glitchActive) {
            return HORROR_CONFIG.horror.distortionIntensity * this.tension;
        }
        return 0;
    }
    
    getTension() {
        return this.tension;
    }
    
    increaseTension(amount) {
        this.tension = Math.min(this.tension + amount, HORROR_CONFIG.horror.maxTension);
    }
    
    reset() {
        this.tension = 0;
        this.glitchActive = false;
        this.shadowVisible = false;
        this.whisperTriggered = false;
        this.glitchTimer = 0;
        this.shadowTimer = 0;
        this.whisperTimer = 0;
        
        // Clear any active effects
        const canvas = document.getElementById('gameCanvas');
        if (canvas) {
            canvas.style.filter = '';
        }
        
        const warningLights = document.querySelectorAll('.warning-light');
        warningLights.forEach(light => {
            light.classList.remove('active');
        });
    }
}

if (typeof window !== 'undefined') {
    window.HorrorSystem = HorrorSystem;
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = HorrorSystem;
}

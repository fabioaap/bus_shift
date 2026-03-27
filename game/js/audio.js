// Audio System - Manages game sounds (engine, ambience, effects)

const AUDIO_CONFIG = (typeof window !== 'undefined' && window.CONFIG)
    ? window.CONFIG
    : require('./config.js');

class AudioSystem {
    constructor() {
        // Audio context
        this.audioContext = null;
        this.initialized = false;
        
        // Audio nodes
        this.engineOscillator = null;
        this.engineGain = null;
        this.ambientOscillator = null;
        this.ambientGain = null;
        this.masterGain = null;
        
        // State
        this.engineRunning = false;
        this.currentSpeed = 0;
    }
    
    async init() {
        try {
            // Create audio context (requires user interaction)
            this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
            
            // Master gain (volume control)
            this.masterGain = this.audioContext.createGain();
            this.masterGain.gain.value = AUDIO_CONFIG.audio.masterVolume;
            this.masterGain.connect(this.audioContext.destination);
            
            this.initialized = true;
            console.log('Audio system initialized');
        } catch (error) {
            console.warn('Audio initialization failed:', error);
        }
    }
    
    startEngine() {
        if (!this.initialized || this.engineRunning) return;
        
        try {
            // Engine sound (low frequency oscillator)
            this.engineOscillator = this.audioContext.createOscillator();
            this.engineOscillator.type = 'sawtooth';
            this.engineOscillator.frequency.value = 40; // Low rumble
            
            this.engineGain = this.audioContext.createGain();
            this.engineGain.gain.value = 0;
            
            this.engineOscillator.connect(this.engineGain);
            this.engineGain.connect(this.masterGain);
            
            this.engineOscillator.start();
            this.engineRunning = true;
        } catch (error) {
            console.warn('Engine start failed:', error);
        }
    }
    
    stopEngine() {
        if (!this.engineRunning || !this.engineOscillator) return;
        
        try {
            this.engineOscillator.stop();
            this.engineRunning = false;
        } catch (error) {
            console.warn('Engine stop failed:', error);
        }
    }
    
    updateEngineSound(speed) {
        if (!this.initialized || !this.engineRunning) return;
        
        this.currentSpeed = speed;
        
        // Frequency increases with speed (40-200 Hz)
        const frequency = 40 + (speed / AUDIO_CONFIG.bus.maxSpeed) * 160;
        this.engineOscillator.frequency.linearRampToValueAtTime(
            frequency,
            this.audioContext.currentTime + 0.1
        );
        
        // Volume increases with speed
        const volume = (speed / AUDIO_CONFIG.bus.maxSpeed) * AUDIO_CONFIG.audio.engineVolume;
        this.engineGain.gain.linearRampToValueAtTime(
            volume,
            this.audioContext.currentTime + 0.1
        );
    }
    
    startAmbient() {
        if (!this.initialized) return;
        
        try {
            // Ambient horror sound (very low frequency)
            this.ambientOscillator = this.audioContext.createOscillator();
            this.ambientOscillator.type = 'sine';
            this.ambientOscillator.frequency.value = 20; // Sub-bass
            
            this.ambientGain = this.audioContext.createGain();
            this.ambientGain.gain.value = AUDIO_CONFIG.audio.ambientVolume;
            
            this.ambientOscillator.connect(this.ambientGain);
            this.ambientGain.connect(this.masterGain);
            
            this.ambientOscillator.start();
        } catch (error) {
            console.warn('Ambient start failed:', error);
        }
    }
    
    stopAmbient() {
        if (this.ambientOscillator) {
            try {
                this.ambientOscillator.stop();
            } catch (error) {
                console.warn('Ambient stop failed:', error);
            }
        }
    }
    
    playCollisionSound() {
        if (!this.initialized) return;
        
        try {
            // Sharp noise burst for collision
            const oscillator = this.audioContext.createOscillator();
            oscillator.type = 'square';
            oscillator.frequency.value = 100;
            
            const gain = this.audioContext.createGain();
            gain.gain.value = AUDIO_CONFIG.audio.effectsVolume;
            gain.gain.linearRampToValueAtTime(0, this.audioContext.currentTime + 0.2);
            
            oscillator.connect(gain);
            gain.connect(this.masterGain);
            
            oscillator.start();
            oscillator.stop(this.audioContext.currentTime + 0.2);
        } catch (error) {
            console.warn('Collision sound failed:', error);
        }
    }
    
    playWhisperSound() {
        if (!this.initialized) return;
        
        try {
            // Eerie whisper-like noise
            const oscillator = this.audioContext.createOscillator();
            oscillator.type = 'sine';
            oscillator.frequency.value = 400;
            oscillator.frequency.linearRampToValueAtTime(200, this.audioContext.currentTime + 1);
            
            const gain = this.audioContext.createGain();
            gain.gain.value = 0;
            gain.gain.linearRampToValueAtTime(AUDIO_CONFIG.audio.effectsVolume * 0.3, this.audioContext.currentTime + 0.5);
            gain.gain.linearRampToValueAtTime(0, this.audioContext.currentTime + 2);
            
            oscillator.connect(gain);
            gain.connect(this.masterGain);
            
            oscillator.start();
            oscillator.stop(this.audioContext.currentTime + 2);
        } catch (error) {
            console.warn('Whisper sound failed:', error);
        }
    }
    
    setMasterVolume(volume) {
        if (this.masterGain) {
            this.masterGain.gain.value = volume;
        }
    }
    
    reset() {
        this.stopEngine();
        this.stopAmbient();
    }
}

if (typeof window !== 'undefined') {
    window.AudioSystem = AudioSystem;
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = AudioSystem;
}

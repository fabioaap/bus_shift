// Keyboard Controls for Bus Shift

class Controls {
    constructor() {
        this.keys = {};
        this.previousKeys = {};
        
        // Key mappings
        this.mappings = {
            // Steering
            left: ['ArrowLeft', 'KeyA'],
            right: ['ArrowRight', 'KeyD'],
            
            // Acceleration/Braking
            accelerate: ['ArrowUp', 'KeyW'],
            brake: ['ArrowDown', 'KeyS'],
            
            // Game controls
            pause: ['Escape'],
            start: ['Space'],
            restart: ['Space']
        };
        
        // Bind event listeners
        this.bindEvents();
    }
    
    bindEvents() {
        window.addEventListener('keydown', (e) => this.onKeyDown(e));
        window.addEventListener('keyup', (e) => this.onKeyUp(e));
        
        // Prevent default behavior for game keys
        window.addEventListener('keydown', (e) => {
            const allKeys = Object.values(this.mappings).flat();
            if (allKeys.includes(e.code)) {
                e.preventDefault();
            }
        });
    }
    
    onKeyDown(e) {
        this.keys[e.code] = true;
    }
    
    onKeyUp(e) {
        this.keys[e.code] = false;
    }
    
    /**
     * Check if a control is currently pressed
     */
    isPressed(control) {
        const codes = this.mappings[control];
        if (!codes) return false;
        
        return codes.some(code => this.keys[code]);
    }
    
    /**
     * Check if a control was just pressed this frame
     */
    isJustPressed(control) {
        const codes = this.mappings[control];
        if (!codes) return false;
        
        return codes.some(code => 
            this.keys[code] && !this.previousKeys[code]
        );
    }
    
    /**
     * Get steering input (-1 to 1)
     */
    getSteeringInput() {
        let input = 0;
        if (this.isPressed('left')) input -= 1;
        if (this.isPressed('right')) input += 1;
        return input;
    }
    
    /**
     * Get acceleration input (-1 to 1)
     */
    getAccelerationInput() {
        let input = 0;
        if (this.isPressed('accelerate')) input += 1;
        if (this.isPressed('brake')) input -= 1;
        return input;
    }
    
    /**
     * Update previous keys state (call at end of frame)
     */
    update() {
        this.previousKeys = { ...this.keys };
    }
    
    /**
     * Reset all keys
     */
    reset() {
        this.keys = {};
        this.previousKeys = {};
    }
    
    /**
     * Disable controls
     */
    disable() {
        this.reset();
    }
}

if (typeof window !== 'undefined') {
    window.Controls = Controls;
}

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = Controls;
}

// Game Configuration
const CONFIG = {
    // Canvas settings
    canvas: {
        width: 1280,
        height: 720,
        backgroundColor: '#000'
    },
    
    // Road settings
    road: {
        width: 400,
        lanes: 2,
        lineWidth: 10,
        lineLength: 40,
        lineGap: 20,
        scrollSpeed: 5,
        sideMargin: 100
    },
    
    // Bus settings
    bus: {
        x: 0, // Center of road
        y: 0, // Will be calculated
        width: 80,
        height: 120,
        speed: 0,
        maxSpeed: 150,
        acceleration: 0.5,
        deceleration: 0.3,
        turnSpeed: 3,
        maxTurn: 200
    },
    
    // Environment settings
    environment: {
        fogDensity: 0.7,
        fogColor: '#2a2a2a',
        treeSpacing: 200,
        treeCount: 20,
        skyColor: '#1a1a1a',
        groundColor: '#0a0a0a'
    },
    
    // Horror mechanics
    horror: {
        tensionBuildRate: 0.001,
        maxTension: 1.0,
        glitchChance: 0.01,
        shadowChance: 0.005,
        whisperChance: 0.002,
        shakeIntensity: 2,
        distortionIntensity: 0.05
    },
    
    // Dashboard settings
    dashboard: {
        steeringWheelRadius: 60,
        steeringWheelColor: '#222',
        instrumentPanelHeight: 120,
        gaugeRadius: 40
    },
    
    // Game states
    states: {
        LOADING: 'loading',
        MENU: 'menu',
        PLAYING: 'playing',
        PAUSED: 'paused',
        GAMEOVER: 'gameover'
    },
    
    // Physics
    physics: {
        gravity: 0.5,
        friction: 0.95,
        offRoadFriction: 0.8,
        offRoadPenalty: 2
    },
    
    // Audio settings (placeholder for future)
    audio: {
        engineVolume: 0.5,
        ambientVolume: 0.3,
        effectsVolume: 0.7,
        masterVolume: 0.8
    }
};

if (typeof window !== 'undefined') {
    window.CONFIG = CONFIG;
}

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = CONFIG;
}

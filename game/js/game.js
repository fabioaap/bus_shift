// Core game loop and state manager

class Game {
    constructor() {
        this.canvas = document.getElementById('gameCanvas');
        this.ctx = this.canvas.getContext('2d');

        this.controls = new Controls();
        this.renderer = new Renderer(this.canvas, this.ctx);
        this.horror = new HorrorSystem();
        this.audio = new AudioSystem();

        this.state = CONFIG.states.LOADING;
        this.lastTime = 0;

        this.gameState = {
            speed: 0,
            fuel: 100,
            distance: 0,
            offRoadTime: 0,
            tension: 0,
            bus: {
                x: 0,
                steerAngle: 0
            }
        };

        this.ui = {
            loadingScreen: document.getElementById('loading-screen'),
            pauseScreen: document.getElementById('pause-screen'),
            gameOverScreen: document.getElementById('gameover-screen'),
            gameOverMessage: document.getElementById('gameover-message'),
            centerMessage: document.getElementById('center-message'),
            centerText: document.querySelector('#center-message .glitch-text'),
            stateMessage: document.getElementById('game-state-message'),
            speedValue: document.querySelector('#speed-gauge .gauge-value'),
            fuelFill: document.querySelector('#fuel-gauge .gauge-fill'),
            engineLight: document.getElementById('engine-light'),
            brakeLight: document.getElementById('brake-light')
        };

        this.resizeCanvas();
        this.bindEvents();
        this.syncGlitchDataText();
    }

    bindEvents() {
        window.addEventListener('resize', () => this.resizeCanvas());
    }

    resizeCanvas() {
        this.canvas.width = CONFIG.canvas.width;
        this.canvas.height = CONFIG.canvas.height;
    }

    syncGlitchDataText() {
        const glitchEls = document.querySelectorAll('.glitch-text');
        glitchEls.forEach((el) => {
            if (!el.getAttribute('data-text')) {
                el.setAttribute('data-text', el.textContent || '');
            }
        });
    }

    start() {
        this.loop = this.loop.bind(this);
        requestAnimationFrame(this.loop);
    }

    loop(timestamp) {
        const deltaTime = this.lastTime ? (timestamp - this.lastTime) / 1000 : 0;
        this.lastTime = timestamp;

        this.handleStateTransitions();

        if (this.state === CONFIG.states.PLAYING) {
            this.updatePlaying(deltaTime);
        }

        this.render();
        this.controls.update();

        requestAnimationFrame(this.loop);
    }

    handleStateTransitions() {
        if ((this.state === CONFIG.states.LOADING || this.state === CONFIG.states.MENU) && this.controls.isJustPressed('start')) {
            this.startGame();
        }

        if (this.state === CONFIG.states.PLAYING && this.controls.isJustPressed('pause')) {
            this.pauseGame();
        } else if (this.state === CONFIG.states.PAUSED && this.controls.isJustPressed('pause')) {
            this.resumeGame();
        }

        if (this.state === CONFIG.states.GAMEOVER && this.controls.isJustPressed('restart')) {
            this.restartGame();
        }
    }

    async startGame() {
        if (!this.audio.initialized) {
            await this.audio.init();
        }

        this.state = CONFIG.states.PLAYING;
        this.ui.loadingScreen.classList.add('hidden');
        this.ui.gameOverScreen.classList.add('hidden');
        this.ui.pauseScreen.classList.add('hidden');

        this.audio.startEngine();
        this.audio.startAmbient();
    }

    pauseGame() {
        this.state = CONFIG.states.PAUSED;
        this.ui.pauseScreen.classList.remove('hidden');
    }

    resumeGame() {
        this.state = CONFIG.states.PLAYING;
        this.ui.pauseScreen.classList.add('hidden');
    }

    restartGame() {
        this.gameState.speed = 0;
        this.gameState.fuel = 100;
        this.gameState.distance = 0;
        this.gameState.offRoadTime = 0;
        this.gameState.bus.x = 0;
        this.gameState.bus.steerAngle = 0;
        this.horror.reset();
        this.ui.gameOverScreen.classList.add('hidden');
        this.state = CONFIG.states.PLAYING;
    }

    updatePlaying(deltaTime) {
        const accelerationInput = this.controls.getAccelerationInput();
        const steeringInput = this.controls.getSteeringInput();

        if (accelerationInput > 0) {
            this.gameState.speed += CONFIG.bus.acceleration * 60 * deltaTime;
        } else if (accelerationInput < 0) {
            this.gameState.speed -= CONFIG.bus.deceleration * 2.2 * 60 * deltaTime;
        } else {
            this.gameState.speed -= CONFIG.bus.deceleration * 0.8 * 60 * deltaTime;
        }

        this.gameState.speed = Utils.clamp(this.gameState.speed, 0, CONFIG.bus.maxSpeed);

        const turnMultiplier = Utils.map(this.gameState.speed, 0, CONFIG.bus.maxSpeed, 0.2, 1);
        this.gameState.bus.x += steeringInput * CONFIG.bus.turnSpeed * turnMultiplier * 35 * deltaTime;
        this.gameState.bus.x = Utils.clamp(this.gameState.bus.x, -CONFIG.bus.maxTurn, CONFIG.bus.maxTurn);

        const targetSteerAngle = steeringInput * 35;
        this.gameState.bus.steerAngle = Utils.lerp(this.gameState.bus.steerAngle, targetSteerAngle, 0.18);

        this.gameState.distance += this.gameState.speed * deltaTime;
        this.gameState.fuel = Math.max(0, this.gameState.fuel - (this.gameState.speed * 0.0028 + 0.01) * deltaTime);

        this.handleRoadPenalty(deltaTime);

        this.horror.update(deltaTime * 1000);
        this.gameState.tension = this.horror.getTension();

        this.audio.updateEngineSound(this.gameState.speed);

        this.updateUI();

        if (this.gameState.fuel <= 0 && this.gameState.speed <= 1) {
            this.triggerGameOver('Você ficou sem combustível no meio da estrada.');
        }
    }

    handleRoadPenalty(deltaTime) {
        const safeLimit = CONFIG.road.width * 0.46;
        const isOffRoad = Math.abs(this.gameState.bus.x) > safeLimit;

        if (isOffRoad) {
            this.gameState.offRoadTime += deltaTime;
            this.gameState.speed = Math.max(0, this.gameState.speed - CONFIG.physics.offRoadPenalty * 40 * deltaTime);
            this.horror.increaseTension(0.004);

            if (this.gameState.offRoadTime > 2.6) {
                this.triggerGameOver('Você saiu da pista por tempo demais.');
            }
        } else {
            this.gameState.offRoadTime = Math.max(0, this.gameState.offRoadTime - deltaTime * 1.4);
        }
    }

    triggerGameOver(message) {
        this.state = CONFIG.states.GAMEOVER;
        this.ui.gameOverMessage.textContent = message;
        this.ui.gameOverScreen.classList.remove('hidden');
        this.audio.playCollisionSound();
    }

    updateUI() {
        this.ui.speedValue.textContent = `${Math.round(this.gameState.speed)} km/h`;

        const fuelPct = Utils.clamp(this.gameState.fuel, 0, 100);
        this.ui.fuelFill.style.width = `${fuelPct}%`;

        this.ui.engineLight.classList.toggle('active', this.gameState.fuel < 28 || this.gameState.tension > 0.65);
        this.ui.brakeLight.classList.toggle('active', this.gameState.offRoadTime > 0.4 || this.controls.isPressed('brake'));

        let centerMsg = 'Stay on the road...';
        if (this.gameState.offRoadTime > 1.2) {
            centerMsg = 'Volte para a pista!';
        } else if (this.gameState.tension > 0.7) {
            centerMsg = 'Algo está te seguindo...';
        }

        if (this.ui.centerText.textContent !== centerMsg) {
            this.ui.centerText.textContent = centerMsg;
            this.ui.centerText.setAttribute('data-text', centerMsg);
        }
    }

    render() {
        if (this.state === CONFIG.states.PLAYING || this.state === CONFIG.states.PAUSED || this.state === CONFIG.states.GAMEOVER) {
            const shake = this.horror.getScreenShake();
            if (shake > 0.01) {
                this.renderer.applyShake(shake);
            } else {
                this.renderer.resetShake();
            }
            this.renderer.render(this.gameState);
        }
    }
}

if (typeof window !== 'undefined') {
    window.Game = Game;
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = Game;
}

// Renderer Module - Handles all canvas drawing operations (Browser-first)

const RENDERER_CONFIG = (typeof window !== 'undefined' && window.CONFIG)
    ? window.CONFIG
    : require('./config.js');

const RENDERER_UTILS = (typeof window !== 'undefined' && window.Utils)
    ? window.Utils
    : require('./utils.js');

class Renderer {
    constructor(canvas, ctx) {
        this.canvas = canvas;
        this.ctx = ctx;
        this.roadOffset = 0;
        this.shakeX = 0;
        this.shakeY = 0;
        this.horizonY = RENDERER_CONFIG.canvas.height * 0.33;
    }

    clear() {
        this.ctx.fillStyle = RENDERER_CONFIG.canvas.backgroundColor;
        this.ctx.fillRect(0, 0, RENDERER_CONFIG.canvas.width, RENDERER_CONFIG.canvas.height);
    }

    applyShake(intensity) {
        this.shakeX = RENDERER_UTILS.random(-intensity, intensity);
        this.shakeY = RENDERER_UTILS.random(-intensity, intensity);
    }

    resetShake() {
        this.shakeX = 0;
        this.shakeY = 0;
    }

    render(gameState) {
        this.clear();

        this.ctx.save();
        this.ctx.translate(this.shakeX, this.shakeY);

        this.drawSky();
        this.drawGround();
        this.drawRoadPerspective(gameState);
        this.drawRoadsideSilhouettes();
        this.drawFog(gameState.tension || 0);
        this.drawCockpit(gameState);

        this.ctx.restore();
    }

    drawSky() {
        const gradient = this.ctx.createLinearGradient(0, 0, 0, this.horizonY);
        gradient.addColorStop(0, '#05070d');
        gradient.addColorStop(1, '#141924');
        this.ctx.fillStyle = gradient;
        this.ctx.fillRect(0, 0, CONFIG.canvas.width, this.horizonY);
    }

    drawGround() {
        const gradient = this.ctx.createLinearGradient(0, this.horizonY, 0, RENDERER_CONFIG.canvas.height);
        gradient.addColorStop(0, '#0d0d0d');
        gradient.addColorStop(1, '#050505');
        this.ctx.fillStyle = gradient;
        this.ctx.fillRect(0, this.horizonY, RENDERER_CONFIG.canvas.width, RENDERER_CONFIG.canvas.height - this.horizonY);
    }

    getRoadXAt(y, side = 'left', lateralOffset = 0) {
        const centerX = RENDERER_CONFIG.canvas.width / 2 + lateralOffset;
        const bottomWidth = RENDERER_CONFIG.road.width * 2.2;
        const topWidth = RENDERER_CONFIG.road.width * 0.28;
        const t = RENDERER_UTILS.clamp((y - this.horizonY) / (RENDERER_CONFIG.canvas.height - this.horizonY), 0, 1);
        const halfWidth = RENDERER_UTILS.lerp(topWidth / 2, bottomWidth / 2, t);
        return side === 'left' ? centerX - halfWidth : centerX + halfWidth;
    }

    drawRoadPerspective(gameState) {
        const lateralOffset = -(gameState.bus?.x || 0) * 0.35;

        const roadLeftTop = this.getRoadXAt(this.horizonY, 'left', lateralOffset);
        const roadRightTop = this.getRoadXAt(this.horizonY, 'right', lateralOffset);
        const roadLeftBottom = this.getRoadXAt(RENDERER_CONFIG.canvas.height + 40, 'left', lateralOffset);
        const roadRightBottom = this.getRoadXAt(RENDERER_CONFIG.canvas.height + 40, 'right', lateralOffset);

        this.ctx.fillStyle = '#161616';
        this.ctx.beginPath();
        this.ctx.moveTo(roadLeftTop, this.horizonY);
        this.ctx.lineTo(roadRightTop, this.horizonY);
        this.ctx.lineTo(roadRightBottom, RENDERER_CONFIG.canvas.height);
        this.ctx.lineTo(roadLeftBottom, RENDERER_CONFIG.canvas.height);
        this.ctx.closePath();
        this.ctx.fill();

        this.ctx.strokeStyle = '#5d5d5d';
        this.ctx.lineWidth = 3;
        this.ctx.beginPath();
        this.ctx.moveTo(roadLeftTop, this.horizonY);
        this.ctx.lineTo(roadLeftBottom, RENDERER_CONFIG.canvas.height);
        this.ctx.moveTo(roadRightTop, this.horizonY);
        this.ctx.lineTo(roadRightBottom, RENDERER_CONFIG.canvas.height);
        this.ctx.stroke();

        this.drawPerspectiveLaneMarkers(lateralOffset, gameState.speed || 0);
    }

    drawPerspectiveLaneMarkers(lateralOffset, speed) {
        const segmentCount = 22;
        const laneCount = RENDERER_CONFIG.road.lanes;
        this.ctx.strokeStyle = '#f4f2c6';

        for (let i = 0; i < segmentCount; i++) {
            const z = ((i + this.roadOffset) % segmentCount) / segmentCount;
            const nextZ = ((i + 0.45 + this.roadOffset) % segmentCount) / segmentCount;
            const y = RENDERER_UTILS.lerp(this.horizonY, RENDERER_CONFIG.canvas.height, z);
            const y2 = RENDERER_UTILS.lerp(this.horizonY, RENDERER_CONFIG.canvas.height, nextZ);

            if (y2 <= this.horizonY + 4) continue;

            const left1 = this.getRoadXAt(y, 'left', lateralOffset);
            const right1 = this.getRoadXAt(y, 'right', lateralOffset);
            const left2 = this.getRoadXAt(y2, 'left', lateralOffset);
            const right2 = this.getRoadXAt(y2, 'right', lateralOffset);

            const center1 = (left1 + right1) / 2;
            const center2 = (left2 + right2) / 2;
            this.ctx.lineWidth = RENDERER_UTILS.lerp(1, 6, z);

            this.ctx.beginPath();
            this.ctx.moveTo(center1, y);
            this.ctx.lineTo(center2, y2);
            this.ctx.stroke();

            for (let lane = 1; lane < laneCount; lane++) {
                const tLane = lane / laneCount;
                const x1 = RENDERER_UTILS.lerp(left1, right1, tLane);
                const x2 = RENDERER_UTILS.lerp(left2, right2, tLane);
                this.ctx.strokeStyle = 'rgba(220,220,220,0.5)';
                this.ctx.lineWidth = RENDERER_UTILS.lerp(1, 4, z) * 0.6;
                this.ctx.beginPath();
                this.ctx.moveTo(x1, y);
                this.ctx.lineTo(x2, y2);
                this.ctx.stroke();
                this.ctx.strokeStyle = '#f4f2c6';
            }
        }

        this.roadOffset += Utils.clamp(speed / 120, 0.02, 0.6);
    }

    drawRoadsideSilhouettes() {
        const count = 28;
        for (let i = 0; i < count; i++) {
            const z = i / count;
            const y = RENDERER_UTILS.lerp(this.horizonY + 10, RENDERER_CONFIG.canvas.height, z);
            const width = RENDERER_UTILS.lerp(2, 18, z);
            const height = RENDERER_UTILS.lerp(8, 120, z);

            const leftX = this.getRoadXAt(y, 'left') - RENDERER_UTILS.lerp(20, 220, z);
            const rightX = this.getRoadXAt(y, 'right') + RENDERER_UTILS.lerp(20, 220, z);

            this.ctx.fillStyle = `rgba(10, 14, 10, ${RENDERER_UTILS.lerp(0.2, 0.9, z)})`;
            this.ctx.fillRect(leftX, y - height, width, height);
            this.ctx.fillRect(rightX, y - height, width, height);
        }
    }

    drawFog(tension) {
        const fog = RENDERER_UTILS.clamp(RENDERER_CONFIG.environment.fogDensity + tension * 0.32, 0.62, 0.96);
        const gradient = this.ctx.createRadialGradient(
            RENDERER_CONFIG.canvas.width / 2,
            this.horizonY + 60,
            0,
            RENDERER_CONFIG.canvas.width / 2,
            RENDERER_CONFIG.canvas.height / 2,
            RENDERER_CONFIG.canvas.width * 0.8
        );

        gradient.addColorStop(0, 'rgba(42, 42, 42, 0.05)');
        gradient.addColorStop(1, `rgba(42, 42, 42, ${fog})`);

        this.ctx.fillStyle = gradient;
        this.ctx.fillRect(0, 0, RENDERER_CONFIG.canvas.width, RENDERER_CONFIG.canvas.height);
    }

    drawCockpit(gameState) {
        const dashboardTop = RENDERER_CONFIG.canvas.height - 195;

        const panelGradient = this.ctx.createLinearGradient(0, dashboardTop, 0, CONFIG.canvas.height);
        panelGradient.addColorStop(0, '#111');
        panelGradient.addColorStop(1, '#050505');
        this.ctx.fillStyle = panelGradient;
        this.ctx.fillRect(0, dashboardTop, RENDERER_CONFIG.canvas.width, RENDERER_CONFIG.canvas.height - dashboardTop);

        this.ctx.strokeStyle = '#2a2a2a';
        this.ctx.lineWidth = 3;
        this.ctx.beginPath();
        this.ctx.moveTo(0, dashboardTop);
        this.ctx.lineTo(RENDERER_CONFIG.canvas.width, dashboardTop);
        this.ctx.stroke();

        const clusterWidth = 360;
        const clusterHeight = 62;
        const clusterX = RENDERER_CONFIG.canvas.width / 2 - clusterWidth / 2;
        const clusterY = dashboardTop + 12;

        this.ctx.fillStyle = 'rgba(14, 14, 14, 0.9)';
        this.ctx.strokeStyle = '#3a3a3a';
        this.ctx.lineWidth = 2;
        this.ctx.beginPath();
        this.ctx.roundRect(clusterX, clusterY, clusterWidth, clusterHeight, 8);
        this.ctx.fill();
        this.ctx.stroke();

        this.ctx.fillStyle = '#89ff7a';
        this.ctx.font = 'bold 20px Courier New';
        this.ctx.textAlign = 'left';
        this.ctx.fillText(`${Math.round(gameState.speed || 0)} km/h`, clusterX + 14, clusterY + 39);

        this.ctx.fillStyle = '#9aa3ad';
        this.ctx.font = '14px Courier New';
        this.ctx.fillText('Rota 66-B', clusterX + 200, clusterY + 25);
        this.ctx.fillText('Noite / Neblina', clusterX + 200, clusterY + 45);

        const wheelX = RENDERER_CONFIG.canvas.width * 0.5;
        const wheelY = RENDERER_CONFIG.canvas.height - 74;
        const wheelRadius = 76;
        const steerAngle = RENDERER_UTILS.toRadians(gameState.bus?.steerAngle || 0);

        this.ctx.save();
        this.ctx.translate(wheelX, wheelY);
        this.ctx.rotate(steerAngle);

        this.ctx.strokeStyle = '#191919';
        this.ctx.lineWidth = 16;
        this.ctx.beginPath();
        this.ctx.arc(0, 0, wheelRadius, 0, Math.PI * 2);
        this.ctx.stroke();

        this.ctx.strokeStyle = '#4a4a4a';
        this.ctx.lineWidth = 6;
        for (let i = 0; i < 3; i++) {
            this.ctx.beginPath();
            this.ctx.moveTo(0, 0);
            this.ctx.lineTo(0, -wheelRadius + 8);
            this.ctx.stroke();
            this.ctx.rotate((Math.PI * 2) / 3);
        }

        this.ctx.fillStyle = '#1a1a1a';
        this.ctx.beginPath();
        this.ctx.arc(0, 0, 18, 0, Math.PI * 2);
        this.ctx.fill();

        this.ctx.restore();

        this.drawIndicatorLights(gameState);
    }

    drawIndicatorLights(gameState) {
        const y = RENDERER_CONFIG.canvas.height - 145;
        const startX = RENDERER_CONFIG.canvas.width / 2 - 38;
        const gap = 38;
        const speed = gameState.speed || 0;
        const fuel = gameState.fuel || 100;

        const lights = [
            { color: '#26ff00', active: speed > 1 },
            { color: '#ffe100', active: fuel < 55 },
            { color: '#ff6a00', active: gameState.offRoadTime > 0.5 }
        ];

        lights.forEach((light, index) => {
            const x = startX + index * gap;
            this.ctx.beginPath();
            this.ctx.arc(x, y, 11, 0, Math.PI * 2);
            this.ctx.fillStyle = light.active ? light.color : '#3a3a3a';
            this.ctx.fill();
            if (light.active) {
                this.ctx.shadowColor = light.color;
                this.ctx.shadowBlur = 20;
                this.ctx.beginPath();
                this.ctx.arc(x, y, 8, 0, Math.PI * 2);
                this.ctx.fillStyle = light.color;
                this.ctx.fill();
                this.ctx.shadowBlur = 0;
            }
        });
    }

    updateRoadScroll(speed) {
        this.roadOffset += speed * RENDERER_CONFIG.road.scrollSpeed * 0.001;
        if (this.roadOffset > 1000) {
            this.roadOffset = 0;
        }
    }
}

if (typeof window !== 'undefined') {
    window.Renderer = Renderer;
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = Renderer;
}

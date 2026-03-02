// Utility Functions for Bus Shift

const Utils = {
    /**
     * Generate random number between min and max
     */
    random(min, max) {
        return Math.random() * (max - min) + min;
    },
    
    /**
     * Generate random integer between min and max (inclusive)
     */
    randomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    },
    
    /**
     * Linear interpolation between two values
     */
    lerp(start, end, t) {
        return start + (end - start) * t;
    },
    
    /**
     * Clamp a value between min and max
     */
    clamp(value, min, max) {
        return Math.min(Math.max(value, min), max);
    },
    
    /**
     * Map a value from one range to another
     */
    map(value, inMin, inMax, outMin, outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    },
    
    /**
     * Calculate distance between two points
     */
    distance(x1, y1, x2, y2) {
        const dx = x2 - x1;
        const dy = y2 - y1;
        return Math.sqrt(dx * dx + dy * dy);
    },
    
    /**
     * Calculate angle between two points
     */
    angle(x1, y1, x2, y2) {
        return Math.atan2(y2 - y1, x2 - x1);
    },
    
    /**
     * Check if point is inside rectangle
     */
    pointInRect(px, py, rx, ry, rw, rh) {
        return px >= rx && px <= rx + rw && py >= ry && py <= ry + rh;
    },
    
    /**
     * Check rectangle collision
     */
    rectCollision(r1x, r1y, r1w, r1h, r2x, r2y, r2w, r2h) {
        return r1x < r2x + r2w &&
               r1x + r1w > r2x &&
               r1y < r2y + r2h &&
               r1y + r1h > r2y;
    },
    
    /**
     * Ease in-out function
     */
    easeInOut(t) {
        return t < 0.5 
            ? 2 * t * t 
            : -1 + (4 - 2 * t) * t;
    },
    
    /**
     * Ease in function
     */
    easeIn(t) {
        return t * t;
    },
    
    /**
     * Ease out function
     */
    easeOut(t) {
        return t * (2 - t);
    },
    
    /**
     * Choose random item from array
     */
    choose(array) {
        return array[Math.floor(Math.random() * array.length)];
    },
    
    /**
     * Shuffle array
     */
    shuffle(array) {
        const shuffled = [...array];
        for (let i = shuffled.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
        }
        return shuffled;
    },
    
    /**
     * Convert degrees to radians
     */
    toRadians(degrees) {
        return degrees * Math.PI / 180;
    },
    
    /**
     * Convert radians to degrees
     */
    toDegrees(radians) {
        return radians * 180 / Math.PI;
    },
    
    /**
     * Format number with leading zeros
     */
    pad(num, size) {
        let s = num + "";
        while (s.length < size) s = "0" + s;
        return s;
    },
    
    /**
     * Deep clone object
     */
    deepClone(obj) {
        return JSON.parse(JSON.stringify(obj));
    }
};

if (typeof window !== 'undefined') {
    window.Utils = Utils;
}

// Export for use in other modules
if (typeof module !== 'undefined' && module.exports) {
    module.exports = Utils;
}

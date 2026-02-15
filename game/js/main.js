// Browser bootstrap

window.addEventListener('DOMContentLoaded', () => {
    const game = new Game();
    window.busShiftGame = game;
    game.start();
});

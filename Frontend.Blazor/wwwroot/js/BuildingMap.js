window.buildingMap = (function () {
    let canvas, ctx, dotNetRef;
    let centerX, centerY;
    const GRID_SIZE = 20;
    let currentPoint = null;

    function init(dotnetReference) {
        dotNetRef = dotnetReference;
        canvas = document.getElementById('mapCanvas');
        ctx = canvas.getContext('2d');
        centerX = canvas.width / 2;
        centerY = canvas.height / 2;

        canvas.addEventListener('click', handleClick);
        draw(); // dibuja grilla al inicio
    }

    function draw() {
        const background = new Image();
        background.src = "/img/map.png"; // Ruta a tu imagen

        background.onload = function () {
            ctx.drawImage(background, 0, 0, canvas.width, canvas.height); // Dibuja la imagen de fondo

            drawGrid();

            if (currentPoint !== null) {
                drawPoint(currentPoint.x, currentPoint.y);
            }
        };
    }


    function drawGrid() {
        ctx.strokeStyle = "#ddd";
        ctx.lineWidth = 0.5;

        for (let x = -centerX; x < centerX; x += GRID_SIZE) {
            ctx.beginPath();
            const [cx1, cy1] = toCanvasCoords(x, -centerY);
            const [cx2, cy2] = toCanvasCoords(x, centerY);
            ctx.moveTo(cx1, cy1);
            ctx.lineTo(cx2, cy2);
            ctx.stroke();
        }

        for (let y = -centerY; y < centerY; y += GRID_SIZE) {
            ctx.beginPath();
            const [cx1, cy1] = toCanvasCoords(-centerX, y);
            const [cx2, cy2] = toCanvasCoords(centerX, y);
            ctx.moveTo(cx1, cy1);
            ctx.lineTo(cx2, cy2);
            ctx.stroke();
        }
    }

    function toCanvasCoords(x, y) {
        return [centerX + x, centerY - y];
    }

    function fromCanvasCoords(x, y) {
        return [x - centerX, centerY - y];
    }

    function snap(val) {
        return Math.round(val / GRID_SIZE) * GRID_SIZE;
    }

    function drawPoint(x, y) {
        const [cx, cy] = toCanvasCoords(x, y);
        const size = 10; // Tamaño del lado del cuadrado
        ctx.fillStyle = "#ff0000";
        ctx.fillRect(cx - size / 2, cy - size / 2, size, size);
    }

    function handleClick(event) {
        const rect = canvas.getBoundingClientRect();
        const mouseX = event.clientX - rect.left;
        const mouseY = event.clientY - rect.top;

        const [gx, gy] = fromCanvasCoords(mouseX, mouseY);
        const snappedX = snap(gx);
        const snappedY = snap(gy);

        // Guarda nuevo punto
        currentPoint = { x: snappedX, y: snappedY };

        // Redibuja todo (limpia canvas, grilla y nuevo punto)
        draw();

        if (dotNetRef) {
            dotNetRef.invokeMethodAsync('UpdateCoordinatesFromMap', snappedX, snappedY);
        }
    }

    function setPoint(x, y) {
        currentPoint = { x: x, y: y };
        draw();
    }

    return {
        init,
        setPoint
    };
})();

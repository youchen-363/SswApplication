var myChartTerrain;

window.addEventListener(" resize", function () {
    let canvas = document.getElementById("canvasTerrain");
    canvas.style.width = window.innerWidth;
    canvas.style.height = window.innerHeight;
})
/*
function initialiseDataSource(xrelief, zrelief) {
    if (xrelief.length != zrelief.length) {
        throw new Error("xrelief " + xrelief.length + "and zrelief " + zrelief.length + " have different size!");
    }
    let arr = [];
    for (let i = 0; i < xrelief.length; i++) {
        arr.push({ x: xrelief[i], y: zrelief[i] });
    }
    return arr;
}
*/
function maxValue(arr) {
    return arr.indexOf(Math.max(...arr));
}

function minValue(arr) {
    return arr.indexOf(Math.min(...arr));
}

function drawTerrain(xrelief, zrelief, plotted) {
    if (!plotted) {
        drawGraphTerrain(xrelief, zrelief);
    } else {
        updateGraphTerrain(xrelief, zrelief);
    } 
}

function drawGraphTerrain(xrelief, zrelief) {
    try {
        var config = {
            type: 'line',
            data: {
                labels: xrelief,
                datasets: [{
                    label: 'Relief', // Assuming 'Relief' as the label string
                    data: zrelief,
                    fill: true,
                    pointRadius: 0,
                    pointBackgroundColor: 'rgba(75, 192, 192, 0.8)',
                }],
            },
            options: {
                animation: false,
                responsive: true,
                scales: {
                    x: {
                        position: 'bottom',
                        title: {
                            display: true,
                            text: 'Distance (km)'
                        },
                        ticks: {
                            stepSize: 2
                        },
                        //min: xrelief[0],
                        //max: xrelief[xrelief.length - 1] // Corrected max value
                    },
                    y: {
                        position: 'left',
                        title: {
                            display: true,
                            text: 'Altitude (m)'
                        },
                        ticks: {
                            beginAtZero: true,
                            stepSize: 200
                        },
                        //min: zrelief[0],
                        //max: zrelief[zrelief.length - 1] // Corrected max value
                    },
                },
                plugins: {
                    title: {
                        display: false
                    }
                }
            }
        };
        var ctx = document.getElementById("canvasTerrain").getContext("2d");
        myChartTerrain = new Chart(ctx, config);
    } catch (ex) {
        console.log("Error catched by terrain: " + ex);
    }
}

function updateGraphTerrain(xrelief, zrelief) {
    myChartTerrain.data = {
        labels: xrelief,
        datasets: [{
            label: 'Relief', // Assuming 'Relief' as the label string
            data: zrelief,
            fill: true,
            pointRadius: 0,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)',
        }],
    };
    myChartTerrain.update();
}
var graphTerrain;
var graphTest;

window.addEventListener(" resize", function () {
    let canvas = document.getElementById("canvasTerrain");
    canvas.style.width = window.innerWidth;
    canvas.style.height = window.innerHeight;
})

function confirmMessage(message) {
    return confirm(message);
}

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

function drawTerrain(datastr, plotted) {
    data = JSON.parse(datastr);
    /*
    console.log(data.xVals);
    console.log(data.deltax);
    step = data.xVals.length/data.deltax;
    data.xVals = data.xVals.map(value => value/step);
    console.log('x :' + data.xVals);
    console.log('z :' + data.zVals);
    */
   data.xVals = data.xVals.map(value=>value*data.deltax / 1000);
    if (!plotted) {
        drawGraphTerrain(data.xVals, data.zVals, data.id);
    } else {
        if (data.id == "canvasTerrain") {
            updateGraphTerrain(data.xVals, data.zVals);
        } else {
            updateGraphTest(data.xVals, data.zVals);
        }
    } 
}

function drawGraphTerrain(x, z, graphId) {
    try {
        var config = Config(x, z);
        var ctx = document.getElementById(graphId).getContext("2d");
        if (graphId == "canvasTerrain") {
            graphTerrain = new Chart(ctx, config);
        } else {
            graphTest = new Chart(ctx, config);
        }
    } catch (ex) {
        console.log("Error catched by terrain: " + ex);
    }
}

function Config(x, z) {
    var config = {
        type: 'line',
        data: {
            labels: x,
            datasets: [{
                label: 'Relief', // Assuming 'Relief' as the label string
                data: z,
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
                    type: 'linear',
                    position: 'bottom',
                    title: {
                        display: true,
                        text: 'Distance (km)'
                    },
                    /*
                    min: 0, // start at zero
                    max: deltax, // end at 100
                    ticks: {
                        stepSize: 1
                    },
                    */
                },
                y: {
                    type: 'linear',
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Altitude (m)'
                    },
                    beginAtZero: true,
                    max: 1000,
                    /*
                    ticks: {
                        stepSize: 200
                    },
                    */
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
    return config;
}

function updateGraphTerrain(xrelief, zrelief) {
    graphTerrain.data = {
        labels: xrelief,
        datasets: [{
            label: 'Relief', // Assuming 'Relief' as the label string
            data: zrelief,
            fill: true,
            pointRadius: 0,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)',
        }],
    };
    /*
    graphTerrain.options = {
        animation: false,
        responsive: true,
        scales: {
            x: {
                type: 'linear',
                position: 'bottom',
                title: {
                    display: true,
                    text: 'Distance (km)'
                },
                min: 0, // start at zero
                max: deltax, // end at 100
            },
            y: {
                type: 'linear',
                position: 'left',
                title: {
                    display: true,
                    text: 'Altitude (m)'
                },
                beginAtZero: true,
                max: 1000,
            },
        },
        plugins: {
            title: {
                display: false
            }
        }
    };
    */
    graphTerrain.update();
}

function updateGraphTest(x, z) {
    graphTest.data = {
        labels: x,
        datasets: [{
            label: 'Relief', // Assuming 'Relief' as the label string
            data: z,
            fill: true,
            pointRadius: 0,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)',
        }],
    };
    /*
    graphTest.options = {
        animation: false,
        responsive: true,
        scales: {
            x: {
                type: 'linear',
                position: 'bottom',
                title: {
                    display: true,
                    text: 'Distance (km)'
                },
                min: 0, // start at zero
                max: deltax, // end at 100
            },
            y: {
                type: 'linear',
                position: 'left',
                title: {
                    display: true,
                    text: 'Altitude (m)'
                },
                beginAtZero: true,
                max: 1000,
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
    */
    graphTest.update();
}
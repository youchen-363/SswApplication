let accumulatedData = '';
let z_values = [];

function receiveChunk(chunk) {
    accumulatedData += chunk;
}

function allChunksSent() {
    z_values = JSON.parse(accumulatedData);
    accumulatedData = '';
}

function drawGraphPropa(x, y, v, plotted) {
    let x_values = JSON.parse(x);
    let y_values = JSON.parse(y);
    range = JSON.parse(v);
    var data = [
        {
            z: z_values.value,
            x: x_values.value,
            y: y_values.value,
            type: 'heatmap',
            hoverongaps: false,
            zmin: range.v_min, // minimum value
            zmax: range.v_max, // maximum value
            colorscale: 'Jet' // specify colorscale
        }
    ];

    var layout = {
        xaxis: {
            title: 'Distance (km)' // replace with your actual x axis title
        },
        yaxis: {
            title: 'Altitude (m)' // replace with your actual y axis title
        }
    };
    
    Plotly.newPlot('canvTotal', data, layout);
}
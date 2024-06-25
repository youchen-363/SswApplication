let accumulatedData = '';
let z_values = [];

function receiveChunk(chunk) {
    accumulatedData += chunk;
}

function allChunksSent() {
    // Now `accumulatedData` contains the full data
    // Parse JSON and use it as needed
    z_values = JSON.parse(accumulatedData);
    
    // Reset for next use
    accumulatedData = '';
}

function drawGraphPropa(x, y, v, plotted) {
    let x_values = JSON.parse(x);
    let y_values = JSON.parse(y);
    //z_values = JSON.parse(z);
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
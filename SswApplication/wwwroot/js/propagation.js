function drawGraphPropa(datastr, plotted) {
    data = JSON.parse(datastr);
    var data = [
        {
            z: data.z_values,
            x: data.x_values,
            y: data.y_values,
            type: 'heatmap',
            hoverongaps: false,
            zmin: data.v_min, // minimum value
            zmax: data.v_max, // maximum value
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
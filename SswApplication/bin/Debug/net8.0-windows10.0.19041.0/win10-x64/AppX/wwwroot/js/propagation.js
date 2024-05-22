
function drawTest(datastr, plotted) {
    data = JSON.parse(datastr);
    console.log(data);
    /*
    var x = Array.from({ length: 1040 }, (_, i) => i + 1);
    var y = Array.from({ length: 500 }, (_, i) => i + 1);
    var z = Array.from({ length: y.length }, () => Array.from({ length: x.length }, () => Math.floor(Math.random() * 100)));
    */
    console.log(data.x_values);
    console.log(data.y_values);
    console.log(data.z_values);
    
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
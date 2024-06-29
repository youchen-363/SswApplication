var myChartFinal;

function initialiseDataFinal(data) {
    datapoints = data.e_total.map((x, index) => ({ x: JSON.stringify(x), y: JSON.stringify(data.z_vect[index]) }));
    return datapoints;
}

function drawGraphFinal(data, datapoints) {
    try {
        var config = {
            type: 'scatter',
            data: {
                datasets: [{
                    label: "final",
                    data: datapoints,
                    fill: false,
                    pointBackgroundColor: 'rgba(75, 192, 192, 0.8)', // Customize point color
                    showLine: true, // tracer la ligne
                    pointRadius: 0
                }]
            },
            options: {
                animation: false,
                responsive: true,
                scales: {
                    x: {
                        position: 'bottom',
                        title: {
                            display: true,
                            text: 'E field (dBV/m)'
                        },
                        ticks: {
                            stepSize: 20
                        },
                        min: data.v_min,
                        max: data.v_max + 5
                    },
                    y: {
                        position: 'left',
                        title: {
                            display: true,
                            text: 'Altitude (m)'
                        },
                        ticks: {
                            beginAtZero: true,
                            stepSize: 10
                        },
                        max : data.config.Z_step.Value * data.config.N_z.Value
                    }
                },
                title: {
                    display: true,
                    text: 'Initial field'
                }
            },
            plugins: {
                tooltip: {
                    callbacks: {
                        label: (tooltipItem) => `(${tooltipItem.raw.x}, ${tooltipItem.raw.y})`
                    }
                },
                legends: true
            }
        };
        var ctx = document.getElementById('canvasFinal').getContext('2d');
        myChartFinal = new Chart(ctx, config);
    } catch (error) {
        alert('Error draw:' + error);
    }
}

function drawFinal(datastr, plotted) {
    data = JSON.parse(datastr);
    datapoints = initialiseDataFinal(data);
    if (!plotted) {
        drawGraphFinal(data, datapoints); 
    } else {
        updateGraphFinal(data, datapoints);
    }    
}

function updateGraphFinal(data, datapoints) {
    try {
        // Update dataset in the existing chart configuration
        myChartFinal.data.datasets = [{
            label: "final",
            data: datapoints,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)', // Customize point color
            pointRadius: 0,
            showLine: true,
            fill: false,
        }];

        // Update x-axis configuration
        myChartFinal.options.scales.x.type = 'linear'; // Ensure x-axis type is linear
        myChartFinal.options.scales.x.position = 'bottom'; // Set x-axis position to bottom
        myChartFinal.options.scales.x.title.text = 'E field (dBV/m)'; // Update x-axis title
        
        // SI VOUS NE VOULEZ PAS LIMITER L'INTERVALLE DE X, COMMENTEZ LES LIGNES SUIVANTES
        myChartFinal.options.scales.x.min = data.v_min; // Set minimum tick value on x-axis
        myChartFinal.options.scales.x.max = data.v_max + 5; // Set maximum tick value on x-axis

        // Update y-axis configuration
        myChartFinal.options.scales.y.type = 'linear'; // Ensure y-axis type is linear
        myChartFinal.options.scales.y.position = 'left'; // Set y-axis position to left
        myChartFinal.options.scales.y.title.text = 'Altitude (m)'; // Update y-axis title
        // SI VOUS NE VOULEZ PAS LIMITER L'INTERVALLE DE Y, COMMENTEZ LES LIGNES SUIVANTES
        myChartFinal.options.scales.y.max = data.config.Z_step.Value * data.config.N_z.Value; // Set maximum tick value on y-axis

        // Update chart title
        myChartFinal.options.title.text = 'Updated Field'; // Update chart title

        // Update the chart
        myChartFinal.update();
    } catch (error) {
        alert('Error updating graph:' + error);
    }
}
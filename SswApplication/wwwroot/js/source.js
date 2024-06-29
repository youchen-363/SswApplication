var myChartSource;

window.setCursorLoading = function () {
    document.body.style.cursor = 'wait';
}

window.resetCursor = function () {
    document.body.style.cursor = 'default';
}

function initialiseDataSource(data) {
    datapoints = data.efield_db.map((x, index) => ({ x: JSON.stringify(x), y: JSON.stringify(data.z_vect[index])}));
    return datapoints;
}

function drawGraphSource(data, datapoints) {
    try {
        var config = {
            type: 'scatter',
            data: {
                datasets: [{
                    label: "source",
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
                        // IL FAUT CHANGER A DYNAMIQUEMENT SI BESOIN  
                        min: -60,
                        max: -20
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
                        min: 0,
                        max: data.config.Z_step.Value * data.config.N_z.Value
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
        var ctx = document.getElementById('canvasSource').getContext('2d');
        myChartSource = new Chart(ctx, config);
    } catch (error) {
        alert('Error draw:' + error);
    }
}

function drawSource(datastr, plotted) {
    try {
        data = JSON.parse(datastr);
        datapoints = initialiseDataSource(data);
        if (!plotted) {
            drawGraphSource(data, datapoints);
            plotted = true;
        } else {
            updateGraphSource(data, datapoints);
        }
    } catch (error) {
        alert("in draw chart: "+error);
    }
}

function updateGraphSource(data, datapoints) {
    try {
        // Update dataset in the existing chart configuration
        myChartSource.data.datasets = [{
            label: "source",
            data: datapoints,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)', // Customize point color
            pointRadius: 0,
            showLine: true,
            fill: false,
        }];

        // Update x-axis configuration
        myChartSource.options.scales.x.type = 'linear'; // Ensure x-axis type is linear
        myChartSource.options.scales.x.position = 'bottom'; // Set x-axis position to bottom
        myChartSource.options.scales.x.title.text = 'E field (dBV/m)'; // Update x-axis title
        myChartSource.options.scales.x.min = -60; // Set minimum tick value on x-axis
        myChartSource.options.scales.x.max = -20; // Set maximum tick value on x-axis
       
        // Update y-axis configuration
        myChartSource.options.scales.y.type = 'linear'; // Ensure y-axis type is linear
        myChartSource.options.scales.y.position = 'left'; // Set y-axis position to left
        myChartSource.options.scales.y.title.text = 'Altitude (m)'; // Update y-axis title
        myChartSource.options.scales.y.max = data.config.Z_step.Value * data.config.N_z.Value; // Set maximum tick value on y-axis

        // Update chart title
        myChartSource.options.title.text = 'Updated Field'; // Update chart title

        // Update the chart
        myChartSource.update();
    } catch (error) {
        alert('Error updating graph:' + error);
    }
}

var mychart;

function initialiseData(data) {
    datapoints = data.efield_db.map((x, index) => ({ x: JSON.stringify(x), y: JSON.stringify(data.z_vect[index])}));
    filteredDataPoints = datapoints.filter(point => point.x >= data.v_min && point.x <= data.v_max);
    return datapoints;
    return filteredDataPoints;
}

function draw(data, datapoints) {
    //document.getElementById("parag").innerText = JSON.stringify(datapoints) + JSON.stringify(data);

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
                    //borderDash: [5, 5],
                }]
            },
            options: {
                responsive: true,
                scales: {
                    x: {
                        position: 'bottom',
                        title: {
                            display: true,
                            text: 'E field (dBV/m)'
                        },
                        
                        min: data.v_min,
                        max: data.v_max,
                    },
                    y: {
                        position: 'left',
                        title: {
                            display: true,
                            text: 'Altitude (m)'
                        },
                        ticks: {
                            //max: 1000, // Set maximum value for y-axis
                            beginAtZero: true,
                            
                        },
                        min: 0,
                        max: data.config.z_step.Value * data.config.N_z
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
        var ctx = document.getElementById("canvas").getContext("2d");
        mychart = new Chart(ctx, config);
    } catch (error) {
        alert('Error fetching data:' + error);
    }
}

function drawChart(datastr, plotted) {
    data = JSON.parse(datastr);
    datapoints = initialiseData(data);
    if (!plotted) {
        alert("not plotted");
        draw(data, datapoints);
        plotted = true;
    } else {
        alert("plotted");
        updateGraph(data, datapoints);
    }
}

function updateGraph(data, datapoints) {
    try {
        // Update dataset in the existing chart configuration
        mychart.data.datasets = [{
            label: "source",
            data: datapoints,
            pointBackgroundColor: 'rgba(75, 192, 192, 0.8)', // Customize point color
            pointRadius: 0,
            showLine: true,
            fill: false,
        }];

        // Update x-axis configuration
        mychart.options.scales.x.type = 'linear'; // Ensure x-axis type is linear
        mychart.options.scales.x.position = 'bottom'; // Set x-axis position to bottom
        mychart.options.scales.x.title.text = 'E field (dBV/m)'; // Update x-axis title
        mychart.options.scales.x.min = data.v_min; // Set minimum tick value on x-axis
        mychart.options.scales.x.max = data.v_max; // Set maximum tick value on x-axis

        // Update y-axis configuration
        mychart.options.scales.y.type = 'linear'; // Ensure y-axis type is linear
        mychart.options.scales.y.position = 'left'; // Set y-axis position to left
        mychart.options.scales.y.title.text = 'Altitude (m)'; // Update y-axis title
        mychart.options.scales.y.max = data.config.z_step.Value * data.config.N_z; // Set maximum tick value on y-axis

        // Update chart title
        mychart.options.title.text = 'Updated Field'; // Update chart title

        // Update the chart
        mychart.update();
    } catch (error) {
        alert('Error updating graph:' + error);
    }
}

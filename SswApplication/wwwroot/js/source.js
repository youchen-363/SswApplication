var myChartSource;

window.addEventListener("resize", function () {
    let canvas = document.getElementById("canvasSource");
    canvas.style.width = window.innerWidth;
    canvas.style.height = window.innerHeight;
})

function initialiseDataSource(data) {
    datapoints = data.efield_db.map((x, index) => ({ x: JSON.stringify(x), y: JSON.stringify(data.z_vect[index])}));
    //filteredDataPoints = datapoints.filter(point => point.x >= data.v_min-50 && point.x <= data.v_max+50);
    return datapoints;
    //console.log("in initialise data");
    //return filteredDataPoints;
}

function drawGraphSource(data, datapoints) {
    //document.getElementById("parag").innerText = JSON.stringify(datapoints) + JSON.stringify(data);
    console.log("in draw");
    console.log(data.config.Z_step.Value * data.config.N_z.Value);
    console.log("efield : \n"+ data.efield_db);
    console.log("zvect : \n"+ data.z_vect);
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
                animation: false,
                responsive: true,
                scales: {
                    x: {
                        position: 'bottom',
                        title: {
                            display: true,
                            text: 'E field (dBV/m)'
                        },
                        /*
                        ticks: {
                            stepSize: 20
                        },
                        */
                       // IL FAUT CHANGER A DYNAMIQUEMENT 
                        min: -60,
                        max: -20
                        /*
                        min: data.v_min,
                        max: data.v_max,
                        */
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
                            stepSize: 10
                            // stepSize: 200
                        },
                        //max: data.config.Z_step.Value * data.config.N_z.Value
                        //min: 0,
                        //max: data.config.Z_step.Value * data.config.N_z.Value
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
        myChartSource.options.scales.x.min = data.v_min; // Set minimum tick value on x-axis
        myChartSource.options.scales.x.max = data.v_max; // Set maximum tick value on x-axis

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

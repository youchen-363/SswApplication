function drawdraw() {
    const data = {
        datasets: [
            {
                label: 'Line Chart with Multiple Y Values',
                data: [
                    { x: 10, y: [20, 25, 30] }, // Multiple y values for x = 10
                    { x: 20, y: [35, 40] },      // Multiple y values for x = 20
                    { x: 30, y: [45] }           // Single y value for x = 30
                ],
                borderColor: 'rgba(255, 99, 132, 1)', // Line color
                backgroundColor: 'rgba(255, 99, 132, 0.2)', // Area under line color (optional)
                borderWidth: 2, // Line width
                pointRadius: 6, // Point radius
                pointHoverRadius: 8 // Point radius on hover
            }
        ]
    };
    const config = {
        type: 'line', // Use a line chart type
        data: data,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    type: 'linear',
                    position: 'bottom'
                },
                y: {
                    type: 'linear'
                }
            }
        }
    };

    // Initialize the chart
    const ctx = document.getElementById('myChart').getContext('2d');
    new Chart(ctx, config);
}
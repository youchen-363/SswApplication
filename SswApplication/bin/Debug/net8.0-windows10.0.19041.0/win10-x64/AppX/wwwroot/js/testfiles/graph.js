
var path = "../CodeSource/Source/outputs/data.csv";

// Use jQuery's AJAX function to load the CSV file
$.get(path, function(csvData) {
    // Convert CSV data into array of objects
    var data = $.csv.toObjects(csvData);

    // Log the parsed data to the console
    console.log(data);
}).fail(function() {
    console.error("Error loading CSV file");
});

/*
var data;
var datapoints;
var filteredDataPoints;

document.addEventListener('DOMContentLoaded', async function() {
    try { 
    data = await dataSource();
    datapoints = data.efield_db.map((x, index) => ({ x: JSON.stringify(x), y: JSON.stringify(data.z_vect[index]) }));
    filteredDataPoints = datapoints.filter(point => point.x >= data.v_min && point.x <= data.v_max);
    } catch (error) {
        alert('Error fetching data:', error);
    }
});
*/
/*
function dataInitialisation() {
    try { 
        data = await dataSource();
        return data;
    } catch (error) {
        alert('Error fetching data:', error);
    }
}

async function dataSource() {
    try {
        const response = await fetch("output.json");
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const jsonResponse = await response.json();
        return jsonResponse;
    } catch (error) {
        alert('Error fetching data:', error);
    }
}
*/


// e_field_db = 20 * np.log10(np.abs(e_field))
// v_max = np.max(e_field_db)
// v_min = v_max - 100
// print('Max field = ', np.round(v_max, 2), 'dBV/m')
// z_vect = np.linspace(0, ConfigSource.z_step*ConfigSource.n_z, num=ConfigSource.n_z, endpoint=False)
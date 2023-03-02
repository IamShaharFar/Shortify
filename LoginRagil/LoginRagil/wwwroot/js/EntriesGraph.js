
var month = @DateTime.Now.Month.ToString()

    function getMonthName(monthIndex) {
    var months = [
        'January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'
    ];
    return months[monthIndex - 1];
}

function backmonth(monthes) {
    monthes = parseInt(monthes)
    if (month - monthes < 1) return month - monthes + 12;
    else return month - monthes;
}

google.charts.load('current', { packages: ['corechart', 'line'] });
google.charts.setOnLoadCallback(drawLineColors);

function drawLineColors() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Month');
    data.addColumn('number', 'Entries');

    data.addRows([
        ['may', 10,], ['june', 5,],
        //[backmonth(11), 10,], [backmonth(10), 5,],
        //[backmonth(9), 12,], [backmonth(8), 5,],
        //[backmonth(7), 12,], [backmonth(6), 5,],
        //[backmonth(5), 12,], [backmonth(4), 5,],
        //[backmonth(3), 5,], [backmonth(2), 5,],
        //[backmonth(1), 5,], [month, 5,]
    ]);

    var options = {
        hAxis: {
            title: 'Month'
        },
        vAxis: {
            title: 'Entries'
        },
        colors: ['#a52714', '#097138']
    };

    var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
    chart.draw(data, options);
}
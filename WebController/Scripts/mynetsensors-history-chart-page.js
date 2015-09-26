﻿/*  MyNetSensors 
    Copyright (C) 2015 Derwish <derwish.pro@gmail.com>
    License: http://www.gnu.org/licenses/gpl-3.0.txt  
*/

var groups = new vis.DataSet();
groups.add({ id: 0 });

var DELAY = 1000; // delay in ms to add new data points

var update = document.getElementById('update');
var charttype = document.getElementById('charttype');

// create a graph2d with an (currently empty) dataset
var container = document.getElementById('visualization');
var dataset = new vis.DataSet();

var options = {
    start: vis.moment().add(-30, 'seconds'), // changed so its faster
    end: vis.moment(),
    drawPoints: { style: 'circle' },
    shaded: { orientation: 'bottom' }
};




var graph2d = new vis.Graph2d(container, dataset, groups, options);


function renderStep() {
    var now = vis.moment();
    var range = graph2d.getWindow();
    var interval = range.end - range.start;
    switch (update.value) {
        case 'continuous':
            graph2d.setWindow(now - interval, now, { animation: false });
            requestAnimationFrame(renderStep);
            break;
        case 'discrete':
            graph2d.setWindow(now - interval, now, { animation: false });
            setTimeout(renderStep, DELAY);
            break;
        case 'none':
            setTimeout(renderStep, DELAY);
            break;
        default: // 'static'
            // move the window 90% to the left when now is larger than the end of the window
            if (now > range.end) {
                graph2d.setWindow(now - 0.1 * interval, now + 0.9 * interval);
            }
            setTimeout(renderStep, DELAY);
            break;
    }
}

renderStep();


$(document).ready(function() {
    $.ajax({
        url: "../../GetSensorDataJsonByDbId/" + dbId, //get dbId from viewbag before
        dataType: "json",
        success: function(data) {
            if ("chartData" in data) {
                // console.log(data);
                addChartData(data.chartData);
                $('#infoPanel').hide();
                $('#chartPanel').fadeIn(1000);
            } else {
                $('#infoPanel').html("There are no entries in history");
            }
        },
        error: function() {
            $('#infoPanel').html("<p class='text-danger'>Failed to get data from server!</p>");
        }
    });

    updateCharType();

});


function addChartData(chartData) {
    var start = vis.moment(chartData[0].x).add(-1, 'seconds');
    var end = vis.moment(chartData[chartData.length - 1].x).add(1, 'seconds');

    var options = {
        start: start,
        end: end
    };

    /* groups.add({
         id: 1,
         //  className: 'vis-graph-group0',
         // options: {excludeFromLegend: true}
    });*/

    graph2d.setOptions(options);

    dataset.add(chartData);
    graph2d.fit();

}

function updateCharType() {
    switch (charttype.value) {
        case 'bars':
            options = {
                style: 'bar',
                drawPoints: false,
                barChart: { width: 50, align: 'right', sideBySide: false }
            };
            break;
        case 'splines':
            options = {
                style: 'line',
                drawPoints: { style: 'circle', size: 6 },
                shaded: {enabled: false },
                interpolation:{enabled: true}
            };
            break;
        case 'shadedsplines':
            options = {
                style: 'line',
                drawPoints: { style: 'circle' , size: 6},
                shaded: {enabled: true , orientation: 'bottom' },
                interpolation:{enabled: true}
            };
            break;
        case 'lines':
            options = {
                style: 'line',
                drawPoints: { style: 'square' , size: 6},
                shaded: {enabled: false  },
                interpolation:{enabled: false}
            };
            break;
        case 'shadedlines':
            options = {
                style: 'line',
                drawPoints: { style: 'square', size: 6},
                shaded: {enabled: true , orientation: 'bottom' },
                interpolation:{enabled: false}
            };
            break;
        case 'dots':
            options = {
                style:'points',
                drawPoints: {style: 'circle' ,size: 10}
            };
            break;
        default:
            break;
    }



    //setOptions cause a bug when switching to dots!!!
    //graph2d.setOptions(options);
    //thats why we need redraw:
    redrawChart(options);

            
}

function redrawChart(options) {
    var window = graph2d.getWindow();
    options.start = window.start;
    options.end = window.end;
    graph2d.destroy();
    graph2d = new vis.Graph2d(container, dataset, groups, options);
}
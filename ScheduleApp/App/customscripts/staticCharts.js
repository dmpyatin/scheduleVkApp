


function openThreadSelectorCounterChart(threadCounter) {

    if (threadCounter != undefined) {

        var ctx = document.getElementById("threadSelectorChart").getContext("2d");

        var data = {
            labels: ["Лекции", "Практики", "Лабораторные", "Другое", "Всего"],
            datasets: [
                {
                    label: "Всего",
                    fillColor: "rgba(220,220,220,0.5)",
                    strokeColor: "rgba(220,220,220,0.8)",
                    highlightFill: "rgba(220,220,220,0.75)",
                    highlightStroke: "rgba(220,220,220,1)",
                    data: [threadCounter.lecturesCount,
                           threadCounter.practiceCount,
                           threadCounter.labsCount,
                           threadCounter.othersCount,
                           threadCounter.totalCount]
                },
                {
                    label: "Запланировано",
                    fillColor: "rgba(151,187,205,0.5)",
                    strokeColor: "rgba(151,187,205,0.8)",
                    highlightFill: "rgba(151,187,205,0.75)",
                    highlightStroke: "rgba(151,187,205,1)",
                    data: [threadCounter.plannedLecturesCount,
                           threadCounter.plannedPracticeCount,
                           threadCounter.plannedLabsCount,
                           threadCounter.plannedOthersCount,
                           threadCounter.plannedTotalCount]
                }
            ]
        };

        if (myBarChart != undefined)
            myBarChart.destroy();

        var myBarChart = new Chart(ctx).Bar(data, {
            scaleBeginAtZero: true,
            scaleShowGridLines: true,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            barShowStroke: true,
            barStrokeWidth: 2,
            barValueSpacing: 5,
            barDatasetSpacing: 1,
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

        });
    }

    $('#threadSelectorDiagramModal').modal('show');
}


function openQuartersSelectorCounterChart(quartersCounter) {

    if (quartersCounter != undefined) {

        var ctx = document.getElementById("quartersSelectorChart").getContext("2d");

        var data = {
            labels: ["Аудитории", "Кабинеты", "Лаборатории", "Диспл. каб.", "Линг. каб.","Залы","Другое","Все"],
            datasets: [
                {
                    label: "Всего",
                    fillColor: "rgba(220,220,220,0.5)",
                    strokeColor: "rgba(220,220,220,0.8)",
                    highlightFill: "rgba(220,220,220,0.75)",
                    highlightStroke: "rgba(220,220,220,1)",
                    data: [quartersCounter.allAuditoriumsCount,
                           quartersCounter.allCabinetsCount,
                           quartersCounter.allLaboratoriesCount,
                           quartersCounter.allDisplayRoomsCount,
                           quartersCounter.allLingafonRoomsCount,
                           quartersCounter.allHallsCount,
                           quartersCounter.allOthersCount,
                           quartersCounter.allAllCount]
                },
                {
                    label: "Занято",
                    fillColor: "rgba(151,187,205,0.5)",
                    strokeColor: "rgba(151,187,205,0.8)",
                    highlightFill: "rgba(151,187,205,0.75)",
                    highlightStroke: "rgba(151,187,205,1)",
                    data: [quartersCounter.busyAuditoriumsCount,
                           quartersCounter.busyCabinetsCount,
                           quartersCounter.busyLaboratoriesCount,
                           quartersCounter.busyDisplayRoomsCount,
                           quartersCounter.busyLingafonRoomsCount,
                           quartersCounter.busyHallsCount,
                           quartersCounter.busyOthersCount,
                           quartersCounter.busyAllCount]
                }
            ]
        };

        if(myBarChart != undefined)
             myBarChart.destroy();

        var myBarChart = new Chart(ctx).Bar(data, {
            scaleBeginAtZero: true,
            scaleShowGridLines: true,
            scaleGridLineColor: "rgba(0,0,0,.05)",
            scaleGridLineWidth: 1,
            barShowStroke: true,
            barStrokeWidth: 2,
            barValueSpacing: 5,
            barDatasetSpacing: 1,
            legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].lineColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>"

        });
    }

    $('#quartersSelectorDiagramModal').modal('show');
}
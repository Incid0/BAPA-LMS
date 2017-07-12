$(document).ready(function () {
    $('#calendar').fullCalendar({
        // put your options and callbacks here
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,listWeek'
        },
        hiddenDays: [0, 6],
        businessHours: {
            dow: [1, 2, 3, 4, 5],
            start: '08:00',
            end: '19:00'
        },
        weekNumberTitle: 'v. ',
        monthNames: ['Januari', 'Februari', 'Mars', 'April', 'Maj', 'Juni', 'Juli', 'Augusti', 'September', 'October', 'November', 'December'], // Custom names to get Pascal casing on months
        views: {
            month: {
                titleFormat: 'MMMM YYYY', // Gives the whole name of the month and the full number for the year
            },
            agendaWeek: {
                columnFormat: 'ddd DD/MM',
                titleFormat: 'MMMM YYYY', // Gives the whole name of the month and the full number for the year
                slotLabelFormat: 'HH:mm' //Gives the hours a 00:00 format in agendaWeek
            },
            listWeek: {
                titleFormat: '[V.] WW, MMMM YYYY' // Gives the whole name of the month and the full number for the year
            }
        },
        height: 'auto', // Maximize height so everything is shown, no scrollbar
        allDaySlot: false,
        firstDay: 1, // Monday
        minTime: '08:00:00', // Show schedule from only 8-19
        maxTime: '19:00:00',
        weekNumbers: true,

        defaultTimedEventDuration: '01:00:00',
        defaultView: 'agendaWeek',
        events: '/activities/GetStudentActivities',
        // Custom renderer to add icon functionality on events
        eventRender: function (event, element) {
            if (event.icon) {
                var icons = event.icon.split(' ');
                for (i = 0; i < icons.length; i++) {
                    element.find(".fc-title").prepend("<i class='glyphicon glyphicon-" + icons[i] + "'></i>");
                }
            }
        }
    });

});
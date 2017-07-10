$(document).ready(function () {

    // page is now ready, initialize the calendar...

    $('#calendar').fullCalendar({
        // put your options and callbacks here
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek'
        },
        businessHours: {
            dow: [1, 2, 3, 4, 5],
            start: '08:00',
            end: '18:00',
        },
        firstDay: 1, // Monday
        minTime: '08:00:00',
        maxTime: '18:00:00',
        weekNumbers: true,
        defaultView: 'agendaWeek',
        events: [
            {
                title: 'All Day Event',
                start: '2017-07-10'
            },
            {
                title: 'Long Event',
                start: '2017-07-11',
                end: '2017-07-11'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2017-07-13T16:00:00'
            },
            {
                id: 999,
                title: 'Repeating Event',
                start: '2017-07-12T16:00:00'
            },
            {
                title: 'Meeting',
                start: '2017-07-10T10:30:00',
                end: '2017-07-10T12:30:00'
            },
            {
                title: 'Lunch',
                start: '2017-07-14T12:00:00'
            },
            {
                title: 'Birthday Party',
                start: '2017-07-16T09:00:00'
            },
            {
                title: 'Click for Google',
                url: 'http://google.com/',
                start: '2017-07-10'
            }
        ]
    })

});
$(document).ready(function () {

    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'agendaDay',
        editable: true,
        allDaySlot: false,
        selectable: true,
        slotMinutes: 15

    });
});
var student = (function ($) {
    // Full Calendar initialization
    function InitCalendar() {
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
            buttonText: {
                list: "Veckoschema"
            },
            views: {
                month: {
                    titleFormat: 'MMMM YYYY', // Gives the whole name of the month and the full number for the year
                    timeFormat: 'HH:mm' //Gives the hours a 00:00 format in month
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
            timezone: 'local',
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
            },
            eventAfterAllRender: student.calUpdate
        });
    }

    return {
        init: function () {
            console.log('init started');

            InitCalendar();
            // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
            $('body').on('click', '.modal-close-btn', function () {
                $('#modalContainer').modal('hide');
            });
            //clear modal cache, so that new content can be loaded and clear old content so it won't show before new
            $('#modalContainer').on('hidden.bs.modal', function () {
                $(this).removeData('bs.modal').children('.modal-content').html('');
            });
        },
        calUpdate: function () {
            $('#calendar a.modal-link').attr('data-toggle', 'modal').attr('data-target', '#modalContainer');
            $('#calendar tr.fc-list-item').each(function (idx, elem) {
                var a = $(elem).find('a');
                $(elem).attr('href', a.attr('href'));
                a.replaceWith(a.text());
            }).attr('data-toggle', 'modal').attr('data-target', '#modalContainer').on('click', function (event) {
                event.preventDefault();
            });
        }
    };
})(jQuery);

$(function () {
    student.init();
});
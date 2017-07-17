var teacher = (function ($) {

    return {
        init: function () {
            console.log('init started');

            // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
            $('body').on('click', '.modal-close-btn', function () {
                $('#modalContainer').modal('hide');
            });
            //clear modal cache, so that new content can be loaded and clear old content so it won't show before new
            $('#modalContainer').on('hidden.bs.modal', function () {
                $(this).removeData('bs.modal').children('.modal-content').html('');
            });

            // Initializing DatePicker
            $('.input-group.date').datepicker({
                weekStart: 1,
                language: "sv",
                calendarWeeks: true
            });
            // Initializing TimePicker
            $('.timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });
        }
    };
})(jQuery);

$(function () {
    teacher.init();
});
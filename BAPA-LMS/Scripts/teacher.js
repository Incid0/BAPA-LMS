var teacher = (function ($) {
    function treeNodeSelect(event, node) {
    };

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

            // Setting MaxLength automatically according to MVC StringLength
            $('input[data-val-length-max]').each(function (idx, element) {
                element.setAttribute('maxlength', element.getAttribute('data-val-length-max'));
            });

            // Initializing DatePicker
            $.fn.datepicker.weekStart = 1;
            $.fn.datepicker.language = "sv";
            $.fn.datepicker.calendarWeeks = true;
            $.fn.datepicker.todayHighlight = true;

            // Initializing TimePicker
            $('.timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });

            // Initializing TreeView
            var tree = $('#tree');
            if (tree.length) {
                $.getJSON('/Courses/GetTree/' + tree.data('id'), function (result) {
                    tree.treeview({ data: [result] });
                    tree.on('nodeSelected', treeNodeSelect);
                });
            }
        }
    };
})(jQuery);

$(function () {
    teacher.init();
});
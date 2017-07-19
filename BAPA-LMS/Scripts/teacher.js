var teacher = (function ($) {
    var lastModule = 0;
    function treeNodeSelect(event, node) {
        var index = node.id, type = index[0], id = index.substr(1);
        // Limit activity creation
        $('#btnAddActivity').prop('disabled', (type === 'c'));

        if (type === 'a') type = 'activities'
        else if (type === 'm') {
            type = 'modules';
            lastModule = id;
        }
        else if (type === 'c') type = 'courses'
        else type = '';
        if (type !== '') {
            $.ajax({
                type: 'GET',
                url: '/' + type + '/details/' + id,
                success: changeEditor
            });
        }
    };

    function changeEditor(view) {
        $('#editarea').html(view);
    }

    function addModule() {
        $.get('/modules/create/' + lastModule, changeEditor);
    }

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
            $.fn.datepicker.defaults.weekStart = 1;
            $.fn.datepicker.defaults.language = "sv";
            $.fn.datepicker.defaults.calendarWeeks = true;
            $.fn.datepicker.defaults.todayHighlight = true;

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
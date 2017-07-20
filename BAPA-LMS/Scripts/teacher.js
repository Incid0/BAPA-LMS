var teacher = (function ($) {
    var lastModule = 0;
    var alertbox;

    function localAlert(message, style) {
        var parent = alertbox.parent();
        if (message) alertbox.text(message);
        if (!style) style = 'success';
        parent.removeClass().addClass('alert alert-' + style);
        parent.show();
        setTimeout(function () {
            parent.fadeOut(500);
        }, 2000);
    }

    function treeNodeSelect(event, node) {
        var index = node.id, ctrl = index[0], id = index.substr(1);
        // Limit activity creation to modules and activities
        $('#btnAddActivity').prop('disabled', (ctrl === 'c'));

        if (ctrl === 'a') ctrl = 'activities'
        else if (ctrl === 'm') {
            ctrl = 'modules';
            lastModule = id;
        }
        else if (ctrl === 'c') ctrl = 'courses'
        else ctrl = '';
        if (ctrl !== '') changeEditor('edit', ctrl, id);
    };

    function changeEditor(action, ctrl, id) {
        var url = '/' + ctrl + '/' + action + '/' + (id ? id : '');
        $.get(url,
            function (view) {
                $('#editarea').html(view);
            });
    }

    function addModule() {
        changeEditor('create', 'modules', lastModule);
    }

    return {
        init: function () {
            console.log('init started');

            // Show message left after page load
            alertbox = $('#alertbox');
            if (alertbox.length && alertbox.text()) {
                localAlert();
            }

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
        },
        showAlert: function (message) {
            localAlert(message)
        },
        initForm: function () {
            $.validator.unobtrusive.parse('#formEdit');
            $('#btnSave').click(function () {
                if (!$('#formEdit').valid()) {
                    return false;
                }
            });
            $('#formEdit .timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });
            var input = $('#formResult'), result = input.val(), data = result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
                if (data[0] === 'success') {
                    // refresh tree
                    //$('#modalContainer').modal('hide');
                }
            }
        }

    };
})(jQuery);

$(function () {
    teacher.init();
});
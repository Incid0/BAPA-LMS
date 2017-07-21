var teacher = (function ($) {
    var alertbox;
    var tree;
    var skipEditor = false;

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

    function findNode(id) {
        return tree.treeview('getEnabled').find(function (node) {
            return node.id === id;
        });
    }

    function loadTree(id) {
        $.getJSON('/Courses/GetTree/' + tree.data('id'), function (result) {
            tree.treeview({
                data: [result],
                levels: 3
            });
            tree.on('nodeSelected', treeNodeSelect);
            if (id) {
                var node = findNode(id);
                if (node) {
                    tree.treeview('selectNode', [node]);
                    tree.treeview('revealNode', [node, { silent: true }]);
                    $('html, body').scrollTop(tree.find('.node-selected').offset().top - $('.navbar').height());
                }
            }
        });
    }

    function treeNodeSelect(event, node) {
        var index = node.id, ctrl = index[0], id = index.substr(1);
        var btnA = $('#btnActivity'), btnD = $('#btnDel');
        // Limit activity creation to modules and activities
        btnA.prop('disabled', (ctrl === 'c'));
        // Limit deletion to modules and activities
        btnD.prop('disabled', (ctrl === 'c'));

        if (ctrl === 'a') {
            ctrl = 'activities';
            btnA.data('parent', tree.treeview('getParent', node).id.substr(1));
            btnD.attr('href', '/activities/delete/' + id);
        }
        else if (ctrl === 'm') {
            ctrl = 'modules';
            btnA.data('parent', id);
            btnD.attr('href', '/modules/delete/' + id);
        }
        else if (ctrl === 'c') {
            ctrl = 'courses'
        }
        else ctrl = '';
        if (!skipEditor && ctrl !== '') changeEditor('edit', ctrl, id);
        skipEditor = false;
    };

    function changeEditor(action, ctrl, id) {
        var url = '/' + ctrl + '/' + action + '/' + (id ? id : '');
        $.get(url, function (view) {
            $('#editarea').html(view);
        });
    }

    function createAction() {
        var btn = $(this);
        changeEditor('create', btn.data('controller'), btn.data('parent'));
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

            // Initializing DatePicker
            $.fn.datepicker.defaults.weekStart = 1;
            $.fn.datepicker.defaults.language = "sv";
            $.fn.datepicker.defaults.calendarWeeks = true;
            $.fn.datepicker.defaults.todayHighlight = true;

            // Initializing TimePicker
            $('.timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });

            // Initializing TreeView
            tree = $('#tree');
            if (tree.length) {
                loadTree();
            }

            // Connecting buttons
            $('button.add').on('click', createAction);
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
            // Setting MaxLength automatically according to MVC StringLength
            $('input[data-val-length-max]').each(function (idx, element) {
                element.setAttribute('maxlength', element.getAttribute('data-val-length-max'));
            });
            var input = $('#formResult'), result = input.val(), data = result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
                if (data[0] === 'success') {
                    skipEditor = true;
                    loadTree(data[2]);
                }
            }
            input.remove();
        },
        initPopup: function () {
            var input = $('#popupResult'), result = input.val(), data = result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
                if (data[0] === 'success') {
                    $('#modalContainer').modal('hide');
                    $('#editarea').html('');
                    $('#btnDel').prop('disabled', true);
                    loadTree();
                }
            }
        }
    };
})(jQuery);

$(function () {
    teacher.init();
});
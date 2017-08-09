var teacher = (function ($) {
    var alertbox;
    var tree;
    var skipEditor = false;
    var filterhandler;

    // Show temporary message at the top of page
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

    // Returns a node from TreeView if id matches
    function findNode(id) {
        return tree.treeview('getEnabled').find(function (node) {
            return node.id === id;
        });
    }

    function loadTree(id) {
        $.getJSON('/Courses/GetTree/' + tree.data('id'), function (result) {
            tree.treeview({
                data: [result],
                showTags: true
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
            // Initializing tooltips
            tree.find('[data-toggle="tooltip"]').tooltip();

            // Testing edit click
            tree.on('click', '.editnode', function () {
                var nodeid = tree.treeview('getNode', $(this).parents('li').data('nodeid')).id;
                if (nodeid) changeEditor('edit', 'courses', nodeid.substr(1));
                //console.log();
                return false;
            });
        });
    }

    function treeNodeSelect(event, node) {
        var index = node.id, ctrl = index[0], action = 'edit', id = index.substr(1);
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
            action = 'studentlist';
            ctrl = 'teacher';
        }
        else ctrl = '';
        if (!skipEditor && ctrl !== '') changeEditor(action, ctrl, id);
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
        if (!btn.data('parent')) {
            switchToEditor(0, '<Ny kurs>');
            tree.empty();
        }
        changeEditor('create', btn.data('controller'), btn.data('parent'));
    }

    function updateCourseList() {
        var filter = $(this);
        if (filterhandler) clearTimeout(filterhandler);
        filterhandler = setTimeout(function () {
            filter.parents('form').submit();
            filterhandler = null;
        }, 300);
    }

    function switchToEditor(id, name) {
        $('#courses').slideUp();
        $('#courseeditor').slideDown();
        $('#coursename').text(name);
        if (tree.length) {
            tree.data('id', id);
            $('#editarea').empty();
            if (id !== 0) loadTree();
        }
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

            // Initializing tooltips
            $('[data-toggle="tooltip"]').tooltip(); 

            // Initializing DatePicker
            $.fn.datepicker.defaults.weekStart = 1;
            $.fn.datepicker.defaults.language = "sv";
            $.fn.datepicker.defaults.calendarWeeks = true;
            $.fn.datepicker.defaults.todayHighlight = true;
            $.fn.datepicker.defaults.autoclose = true;

            // Initializing TimePicker
            $('.timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });

            // Initializing TreeView
            tree = $('#tree');

            // Connecting filterinput
            $('#coursefilter').on('input', updateCourseList);
            $('#StartRange, #EndRange').on('change', updateCourseList);

            // Connecting add buttons
            $('button.add').on('click', createAction);

            // Clicking course
            $('#courseList').on('click', 'tr[data-id]', function () {              
                var row = $(this);
                switchToEditor(row.data('id'), row.find('td:first').text());
            });

            // Return to courselist
            $('#btnReturn').on('click', function () {
                $('#courseeditor').slideUp();
                $('#courses').slideDown();
                $('#coursefilter').trigger('input');
            });
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
            var input = $('#formResult'), result = input.val(), data = result && result.split('|');
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
            var input = $('#popupResult'), result = input.val(), data = result && result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
                if (data[0] === 'success') {
                    $('#modalContainer').modal('hide');
                    $('#editarea').empty();
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
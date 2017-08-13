var teacher = (function ($) {
    var alertbox;
    var tree;
    var treeview;
    var skipEditor = false;
    var filterhandler;
    var formrefresh = false;

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

    function loadTree(id) {
        $.getJSON('/Courses/GetTree/' + tree.data('id'), function (result) {
            tree.treeview({
                data: [result],
                preventUnselect: true,
                showTags: true
            });
            tree.on('rendered', function () {
                treeview = tree.treeview(true);

                tree.on('nodeSelected', treeNodeSelect);
                var node = id ? treeview.findNodes(id, 'id')[0] : treeview.getNodes()[0];
                if (node) {
                    treeview.selectNode(node);
                    treeview.revealNode(node, { silent: true });
                    $('html, body').scrollTop(node.$el.offset().top - $('.navbar').height());
                }
                // Initializing tooltips
                tree.find('[data-toggle="tooltip"]').tooltip();

                // Testing edit click
                tree.on('click', '.editnode', function () {
                    var node = treeview.findNodes('^' + $(this).parents('li').data('nodeid') + '$', 'nodeId'), nodeid = node.length && node[0].id;
                    if (nodeid) changeEditor('edit', 'courses', nodeid.substr(1));
                    return false;
                });
            });
        });
    }

    function treeNodeSelect(event, node) {
        var index = node.id, ctrl = index[0], action = 'edit', id = index.substr(1);
        var btnA = $('#btnActivity'), btnD = $('#btnDel');

        $('.toggle-action').prop('disabled', false);
        // Limit activity creation to modules and activities
        btnA.prop('disabled', (ctrl === 'c'));

        if (ctrl === 'a') {
            ctrl = 'activities';
            btnA.data('parent', treeview.getParents(node)[0].id.substr(1));
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
            btnD.attr('href', '/courses/delete/' + id);
            $('#btnModule').data('parent', id)
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
        var btn = $(this), ctrl = btn.data('controller'), action = 'create', id = btn.data('parent');
        if (ctrl === 'courses') {
            switchToEditor(0, '<Ny kurs>');
            $('.toggle-action').prop('disabled', true);
            tree.empty();
        } else if (ctrl === 'teacher') {
            action = 'register';
            node = treeview.getNodes()[0];
            id = node && node.id.substr(1);
        }
        changeEditor(action, ctrl, id);
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

    function switchToList() {
        $('#courseeditor').slideUp();
        $('#courses').slideDown();
        $('#coursefilter').trigger('input');
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

            // Connecting TreeView
            tree = $('#tree');

            // Connecting filterinput
            $('#coursefilter').on('input', updateCourseList);
            $('#StartRange, #EndRange').on('change', updateCourseList);

            // Connecting add buttons
            $('.create-action').on('click', createAction);

            // Clicking course
            $('#courseList').on('click', 'tr[data-id]', function () {              
                var row = $(this);
                switchToEditor(row.data('id'), row.find('td:first').text());
            });

            // Return to courselist
            $('#btnReturn').on('click', switchToList);
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
                    var id = data[2];
                    if (id && id[0] === 'c') {
                        tree.data('id', id.substr(1));
                        $('#coursename').text(data[3]);
                    }
                    loadTree(id);
                }
            }
            input.remove();
        },
        initUpload: function () {
            $.validator.unobtrusive.parse('#formUpload');
            $('#btnUpload').click(function () {
                if (!$('#formUpload').valid()) {
                    return false;
                }
            });
            // FileUpload
            $('#formUpload').ajaxForm({
                data: {
                    refresh: function () { return formrefresh; }
                },
                target: $('#uploadWindow'),
                success: function () { formrefresh = false; }
            });
            $('#inputFile').change(function () {
                if ($(this).val()) {
                    $('#btnUpload').attr('disabled', false);
                }
            });
            $(":file").filestyle({ buttonText: "Välj fil" });
            $('#formUpload .timepicker').timepicker({ 'timeFormat': 'H:i', 'scrollDefault': 'now' });
            // Setting MaxLength automatically according to MVC StringLength
            $('input[data-val-length-max]').each(function (idx, element) {
                element.setAttribute('maxlength', element.getAttribute('data-val-length-max'));
            });
            var input = $('#uploadResult'), result = input.val(), data = result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
            }
            input.remove();
        },
        initPopup: function () {
            var input = $('#popupResult'), result = input.val(), data = result && result.split('|');
            if (result) {
                localAlert(data[1], data[0]);
                if (data[0] === 'success') {
                    $('#modalContainer').modal('hide');
                    if (!data[2]) { // Treeview or other view item deleted
                        var node = treeview.getSelected(), parent = treeview.getParents(node)[0];
                        treeview.removeNode(node, { silent: true });
                        if (parent) {
                            treeview.selectNode(parent);
                        } else {
                            $('#editarea').empty();
                            switchToList();
                        }
                    } else {
                        if (data[2] === 'user') { // Deleted user, refresh node to show updated classlist
                            treeNodeSelect(null, treeview.getNodes()[0]);
                        } else {
                            formrefresh = true; // Deleted file, trigger form to reload filelist
                            $('#formUpload').submit();
                        }
                    }
                }
            }
        },
        initStudents: function () {
            // Initializing tooltips
            $('#tabedit [data-toggle="tooltip"]').tooltip();
            // Clicking user edit
            $('#tabedit .edit-student').on('click', function () {
                changeEditor('editstudent', 'teacher', $(this).data('id'));
            });
        }
    };
})(jQuery);

$(function () {
    teacher.init();
});
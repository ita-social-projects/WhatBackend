// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ConfigureExporting() {

    let selectFormat = $('#selectFormat');

    selectFormat.on('change', () => {
        $('#exportForm').attr('action', '/Export/studentsClassbooks/' + selectFormat.val());
    });

    $('#exportForm').submit(function () {

        $('#exportCourseId').val($('#courseSelect').val());
        $('#exportStudentGroupId').val($('#groupSelect').val());
        $('#exportStart').val($('#startDate').val());
        $('#exportFinish').val($('#finishDate').val());

        return true;
    });

}

function ConfigureExportingStudent() {

    let selectFormat = $('#selectFormat');

    selectFormat.on('change', () => {
        $('#exportForm').attr('action', '/Export/studentClassbook/' + selectFormat.val());
    });

    $('#exportForm').submit(function () {

        $('#exportStudentId').val($('#exportStudentId').val());
        $('#exportStart').val($('#exportStart').val());
        $('#exportFinish').val($('#exportFinish').val());

        return true;
    });

}
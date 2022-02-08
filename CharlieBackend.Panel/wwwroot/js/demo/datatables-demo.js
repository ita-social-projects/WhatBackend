// Call the dataTables jQuery plugin
$(document).ready(function() {
    $('#dataTable').DataTable();
    $('#groupsDataTable').DataTable({
        "order": [[2, "desc"]]
    });
});

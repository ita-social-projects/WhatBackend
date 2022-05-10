// Call the dataTables jQuery plugin
$(document).ready(function() {
    $('#dataTable').DataTable();
    $('#groupsDataTable').DataTable({
        "order": [[2, "desc"]]
    });

    var counter = document.getElementById('dataTable_length');
    if (counter != null) {
        counter.style.float = 'left';
    }
    var counter = document.getElementById('groupsDataTable_length');
    if (counter != null) {
        counter.style.float = 'left';
    }
    
    var content = document.getElementById('content');
    if (content != null) {
        content.style.minWidth = '900px';
    }
});

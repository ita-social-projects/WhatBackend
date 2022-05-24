$(document).on("click", ".btn-event", function (e) {
    e.preventDefault();
    var _self = $(this);
    var seGroup = _self.attr("seGroup");
    var seMentor = _self.attr("seMentor");
    var seTheme = _self.attr("seTheme");
    var seTime = _self.attr("seTime");
    var seDescription = _self.attr("seDescription");
    var seLink = _self.attr("seLink");
    var seEventOccurrenceId = _self.attr("seEventOccurrenceId");
    var seSingleEventId = _self.attr("seSingleEventId");
    $("#seGroup").val(seGroup);
    $("#seMentor").val(seMentor);
    $("#seTheme").val(seTheme);
    $("#seTime").val(seTime);
    $("#seEventOccurrenceId").attr('href', "/EventOccurrence/PrepareEventOccurrenceForUpdate/" + seEventOccurrenceId);
    $("#seSingleEventId").attr('href', "/EventOccurrence/PrepareSingleEventForUpdate/" + seSingleEventId);
    $("#deleteEventOccurrence").attr('href', "/EventOccurrence/DeleteEventOccurrence/" + seEventOccurrenceId);
    $("#seDescription").val(seDescription);
    $("#seLink").val(seLink);
    $("#seLink1").attr('href', seLink);
});

function ChangeWeekFormat() {
    var x = document.getElementById("weekFormat").value;

    if (x == 1) {
        $(".weekend").removeClass("col-calendar-full-week").addClass("d-none");
        $(".col-calendar-full-week").removeClass("col-calendar-full-week").addClass("col-calendar-working-week");
    }
    else if (x == 2) {
        $(".col-calendar-working-week").removeClass("col-calendar-working-week").addClass("col-calendar-full-week");
        $(".weekend").removeClass("d-none").addClass("col-calendar-full-week");
    }
}
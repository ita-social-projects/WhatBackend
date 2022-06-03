$(document).on("click", ".btn-event", function (e) {
    e.preventDefault();
    var _self = $(this);
    var seGroup = _self.attr("seGroup");
    var seMentor = _self.attr("seMentor");
    var seTheme = _self.attr("seTheme");
    var seTime = _self.attr("seTime");
    var seEventOccurrenceId = _self.attr("seEventOccurrenceId");
    var seSingleEventId = _self.attr("seSingleEventId");
    var seColor = _self.css("background-color");

    if (seEventOccurrenceId == 0) {
        document.getElementById("seEventOccurrenceId").hidden = true;
        document.getElementById("deleteEventOccurrence").hidden = true;
    }
    else {
        document.getElementById("seEventOccurrenceId").hidden = false;
        document.getElementById("deleteEventOccurrence").hidden = false;
    }

    $("#seGroup").val(seGroup);
    $("#seMentor").val(seMentor);
    $("#seTheme").val(seTheme);
    $("#seTime").val(seTime);
    $('#seColor').css("background-color", seColor);
    $("#seEventOccurrenceId").attr('href', "/EventOccurrence/PrepareEventOccurrenceForUpdate/" + seEventOccurrenceId);
    $("#seSingleEventId").attr('href', "/EventOccurrence/PrepareSingleEventForUpdate/" + seSingleEventId);
    $("#deleteEventOccurrence").attr('href', "/EventOccurrence/DeleteEventOccurrence/" + seEventOccurrenceId);
    $("#deleteSingleEvent").attr('href', "/EventOccurrence/DeleteSingleEvent/" + seSingleEventId);
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

$(".btn-event").mouseover(function () {
    $(this).css("background-color", $(this).css("color"));
    $(this).css("color", "White");
});

$(".btn-event").mouseleave(function () {
    $(this).css("background-color", "White");
    $(this).css("color", $(this).css("border-color"));
});

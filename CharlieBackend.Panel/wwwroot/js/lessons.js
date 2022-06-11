function SetActivityToMarkField(iterator) {
    var elementMarkInput = document.getElementById('LessonVisits_' + iterator + '__StudentMark');
    var elementCommentInput = document.getElementById('LessonVisits_' + iterator + '__Comment');

    var isChecked = document.getElementById('LessonVisits_' + iterator + '__Presence').checked;
    if (isChecked) {
        elementMarkInput.removeAttribute('disabled');
        elementCommentInput.removeAttribute('disabled');
    }
    else {
        elementMarkInput.setAttribute('disabled', 'disabled');
        elementMarkInput.value = "";
        elementCommentInput.setAttribute('disabled', 'disabled');
        elementCommentInput.value = "";
    }
}

function SetActivityToAllElements() {
    var rowCount = document.getElementsByClassName("row").length;
    for (var i = 0; i < rowCount; i++) {
        SetActivityToMarkField(i);
    }
}

$(document).ready(SetActivityToAllElements());
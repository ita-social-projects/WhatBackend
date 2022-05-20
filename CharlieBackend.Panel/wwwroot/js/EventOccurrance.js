
function PassColor(initColor) {
    $("#picker1").colorPick({
        'initialColor': initColor,
        'palette': ["#1abc9c", "#16a085", "#2ecc71", "#27ae60", "#3498db", "#2980b9", "#9b59b6", "#8e44ad", "#34495e", "#2c3e50", "#f1c40f", "#f39c12", "#e67e22", "#d35400", "#e74c3c", "#c0392b", "#ecf0f1"],
        'onColorSelected': function () {
            this.element.css({ 'backgroundColor': this.color, 'color': this.color });
            $("#inputColor").val(this.color);
        }
    });
}

function UpdateForm(value) {
    if (value == 0) {
        document.getElementById("dates").hidden = true;
        document.getElementById("days-of-week").hidden = true;
        document.getElementById("pattern-index").hidden = true;
    }
    else if (value == 1) {
        document.getElementById("dates").hidden = true;
        document.getElementById("days-of-week").hidden = false;
        document.getElementById("pattern-index").hidden = true;
    }
    else if (value == 2) {
        document.getElementById("dates").hidden = false;
        document.getElementById("days-of-week").hidden = true;
        document.getElementById("pattern-index").hidden = true;
    }
    else if (value == 3) {
        document.getElementById("dates").hidden = true;
        document.getElementById("days-of-week").hidden = false;
        document.getElementById("pattern-index").hidden = false;
    }
};
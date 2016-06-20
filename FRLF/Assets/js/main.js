var currentGrid = 0;

// Length of codes
var codeLen = 4;

// Clicked codes
var codes = [];

const LOGIN = "login";
const GRID1 = "grid1";
const GRID2 = "grid2";
const GRID3 = "grid3";

// This javascript method calls C# by setting the browser
// to a URL with a custom scheme that is registered in C#.
// All values are sent to C# as part of the querystring
function InvokeCSharpWithFormValues(elm) {
    var qs = "";
    var elms = elm.form.elements;

    for (var i = 0; i < elms.length; i++) {
        qs += "&" + elms[i].id + "=" + elms[i].value;
    }

    if (elms.length > 0)
        qs = qs.substring(1);

    location.href = "hybrid:" + elm.id + "?" + qs;
}

function InvokeGridValidation() {
    var qs = "";
    for (var i = 0; i < codes.length; i++) {
        qs += "&" + i + "=" + codes[i];
    }
    qs += "&grid=" + currentGrid;

    if (codes.length > 0)
        qs = qs.substring(1);

    location.href = "hybrid:" + "ValidateGrid" + "?" + qs;
}

function onCodeSubmit() {
    // Submit value to server for validation
    $("#CodeEntry").prop('disabled', true);
    $("#submitCode").prop('disabled', true);
    $("#error-code").hide();
    
    var loading = $("#loading-code");
    loading.show();
    loading.addClass('animated infinite flash');

    InvokeCSharpWithFormValues($("#CodeEntry")[0]);
}

function invalidCodeEntry() {
    $("#CodeEntry").prop('disabled', false);
    $("#submitCode").prop('disabled', false);
    $("#loading-code").hide();
    var error = $("#error-code");
    error.show();
    error.addClass('animated shake');
}

function cellSelected(id) {
    $("#invalid-grid").hide();
    $("#valid-grid").hide();

    var cell = $("#" + id);
    var selectedClass = "selected";
    if (!cell.hasClass(selectedClass))
        cell.addClass(selectedClass);
    else
        return;

    codes.push(id);

    if (codeLen < codes.length) {
        for (var x = 0; x < codes.length; x++) {
            var cell = $("#" + codes[x]);
            cell.removeClass(selectedClass);
        }

        // Submit codes
        InvokeGridValidation();

        // Reset codes
        codes = [];
    }
}

function parseGridResult(isValid) {
    var validGrid = $("#valid-grid");
    var invalidGrid = $("#invalid-grid");
    if (isValid === "true") {
        validGrid.show();
        validGrid.addClass('animated tada');

        invalidGrid.hide();

        removeGridEvents();
    } else {
        validGrid.hide();
        invalidGrid.show();
        invalidGrid.addClass('animated shake');
    }
}

function showView(view) {
    $("#error-code").hide();
    $("#invalid-grid").hide();
    $("#valid-grid").hide();
    $("#loading-code").hide();

    switch (view) {
        case LOGIN:
            $("#" + LOGIN).show();
            $("#" + GRID1).hide();
            $("#" + GRID2).hide();
            $("#" + GRID3).hide();
            break;
        case GRID1:
            $("#" + LOGIN).hide();
            $("#" + GRID1).show();
            $("#" + GRID2).hide();
            $("#" + GRID3).hide();
            currentGrid = 1;
            addGridEvents(currentGrid);
            break;
        case GRID2:
            $("#" + LOGIN).hide();
            $("#" + GRID1).hide();
            $("#" + GRID2).show();
            $("#" + GRID3).hide();
            currentGrid = 2;
            addGridEvents(currentGrid);
            break;
        case GRID3:
            $("#" + LOGIN).hide();
            $("#" + GRID1).hide();
            $("#" + GRID2).hide();
            $("#" + GRID3).show();
            currentGrid = 3;
            addGridEvents(currentGrid);
            break;
    }
}

function addGridEvents() {
    var grid = $("#grid" + currentGrid);

    var rows = grid.children();
    for (var i = 0; i < rows.length; i++) {
        var row = $(rows[i]).children();
        for (var r = 0; r < row.length; r++) {
            $(row[r]).click(function (e) {
                cellSelected(e.currentTarget.id);
            });
        }
    }
}

function removeGridEvents() {
    var grid = $("#grid" + currentGrid);

    var rows = grid.children();
    for (var i = 0; i < rows.length; i++) {
        var row = $(rows[i]).children();
        for (var r = 0; r < row.length; r++) {
            $(row[r]).off();
        }
    }
}
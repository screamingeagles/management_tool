/*!
 * project file v1.0.7 (http://getbootstrap.com)
 * Copyright 2011-2016 Twitter, Inc.
 * Licensed under the Arsalan Open Genreal Public Liscense
 */
function newOnClick() {
    // onclick="window.location.href='(at)Url.Content("~/Project/Create")'"

    var selDivisionId = $('#DivisionId').find(":selected").val();
    var selAreaId = $('#AreaId').find(":selected").val();
    var selSubAreaId = $('#SubAreaId').find(":selected").val();

    window.location.href = "/Project/Create?div=" + selDivisionId + "&area=" + selAreaId + "&sa=" + selSubAreaId;
}

function FillProjectsList(obj) {
    var selDiv = $('#DivisionId').find(":selected").val();
    var selArea = $('#AreaId').find(":selected").val();
    var selSArea = $('#SubAreaId').find(":selected").val();

    //alert(selSArea);

    if ((selDiv == "") || (selArea == "")) {
        return;
    }

    call_toggle();

    var postedContent = { dv: selDiv, area: selArea, sarea: selSArea }

    $.ajax({
        type: "POST",
        url: "/Project/GetProjectsListBySubArea",
        data: postedContent,
        success: function (message) {
            var res = message.data;
            if (res.length > 0) {
                var innerhtml = "";
                $("#tblList > tbody").empty();
                for (var i = 0; i < message.data.length; i++) {
                    innerhtml += "<tr>" +
                                    "<td>" + (i + 1) + "</td>" +
                                    "<td>" + message.data[i].pl.DivisionName + "</td>" +
                                    "<td>" + message.data[i].pl.AreaName + "</td>" +
                                    "<td>" + message.data[i].pl.SubAreaName + "</td>" +
                                    "<td>" + message.data[i].pl.ProjectName + "</td>" +
                                    "<td>" + ((message.data[i].pl.IsActive == true) ? "<i class='fa fa-check-square text-navy'></i>" : "<i class='fa fa-square text-navy'></i>") + "</td>" +
                                    "<td>" + message.data[i].pl.GeneratedByName + "</td>" +
                                    "<td>" + parseJsonDate(message.data[i].pl.GeneratedDate) + "</td>" +
                                    "<td>" +
                                        "<a href='/Project/Edit/" + message.data[i].pl.ProjectId + "'>Edit    </a> |" +
                                        "<a href='/Project/Details" + message.data[i].pl.ProjectId + "'>Details </a> |" +
                                        "<a href='/Project/Delete" + message.data[i].pl.ProjectId + "'>Delete  </a>" +
                                    "</td>" +
                                 "</tr>";

                } // end of for loop
                $("#tblList > tbody").append(innerhtml);
            } // end of if for length
            else {
            }
        },
        error: function (xerr) {
            /* $('#txteng').val("err : " +xerr);*/
        }
    });

    setTimeout(call_toggle, 500);

}

function FillSubArea(obj) {
    var sel = obj.options[obj.selectedIndex].value;
    var selText = obj.options[obj.selectedIndex].text
    if (sel == "") { return; }

    $("#tblList > tbody").empty();

    var postedContent = { SelectedArea: sel }

    $.ajax({
        type: "POST",
        url: "/Project/GetSubAreaByArea",
        data: postedContent,
        success: function (message) {
            $("#SubAreaId").empty()
            var res = message.data;

            if (res.length > 0) {
                $('#SubAreaId').append($('<option>', { value: "", text: "Select Sub Area", selected: true }));
                for (var i = 0; i < message.data.length; i++) {
                    $('#SubAreaId').append($('<option>', { value: message.data[i].SubAreaId, text: message.data[i].SubAreaName }));
                } // end of for loop
            } // end of if for length
            else {
                $('#SubAreaId').append($('<option>', { value: "", text: "Select Sub Area", selected: true }));
                $('#SubAreaId').append($('<option>', { value: '0', text: "Select to Load Projects For " + selText }));
            }
        },
        error: function (xerr) {
            /* $('#txteng').val("err : " +xerr);*/
        }
    });
}

function FillArea(obj) {
    var sel = obj.options[obj.selectedIndex].value;
    if (sel == "") { return; }

    call_toggle();

    var postedContent = { SelectedDivision: sel }

    $("#tblList > tbody").empty();

    $.ajax({
        type: "POST",
        url: "/Project/GetSubAreaByDivision",
        data: postedContent,
        success: function (message) {
            var res = message.data;
            $("#AreaId").prop('disabled', false);
            $("#AreaId").empty()
            if (res.length > 0) {
                for (var i = 0; i < message.data.length; i++) {
                    $('#AreaId').append($('<option>', { value: message.data[i].AreaId, text: message.data[i].AreaName }));
                } // end of for loop
            } // end of if for length
        },
        error: function (xerr) {
            /* $('#txteng').val("err : " +xerr);*/
        }
    });

    setTimeout(call_toggle, 500);
}

function call_toggle() {
    $('#ibox1').toggleClass('sk-loading');
}

function parseJsonDate(jsonDateString) {
    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    jsonDateString = jsonDateString.replace('/Date(', '');
    jsonDateString = jsonDateString.replace(')/', '');
    var date = new Date(parseInt(jsonDateString));
    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    return day + "-" + monthNames[monthIndex] + "-" + year;
}
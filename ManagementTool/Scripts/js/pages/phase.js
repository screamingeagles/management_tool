/*!
 * phase view file v1.0.7 (http://getbootstrap.com)
 * Copyright 2011-2016 rethink technology, Inc.
 * Licensed under the Arsalan Open Genreal Public Liscense
 */

$(document).ready(function () {

    GetCompanyByLocation = function (param) {
        if (param.value == "") { return; }

        call_toggle();

        $("#CompanyId").empty();
        $("#tblList > tbody").empty();

        var postedContent = { LocId: param.value }

        $.ajax({
            type: "POST",
            url: "/Phase/GetCompanyByLocation",
            data: postedContent,
            success: function (message) {

                if (message.data.length > 0) {
                    $('#CompanyId').append($('<option>', { value: '0', text: 'Select Company', selected: true }));
                    for (var i = 0; i < message.data.length; i++) {
                        $('#CompanyId').append($('<option>', { value: message.data[i].CompanyId, text: message.data[i].CompanyName }));
                    } // end of for loop
                }
                // end of if for length
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
            }
        });

        setTimeout(call_toggle, 500);

    }

    FillPhaseByCompany = function (param) {
        if (param.value == "") { return; }

        call_toggle();

        $("#tblList > tbody").empty();

        var postedContent = { CompanyId: param.value }

        $.ajax({
            type: "POST",
            url: "/Phase/GetPhaseByCompany",
            data: postedContent,
            success: function (message) {
                filltable(message.data);
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
            }
        });

        setTimeout(call_toggle, 500);

    }


    GetAreaByDivision = function (param) {
        if (param.value == "") { return; }

        call_toggle();

        $("#AreaId").empty();
        $("#tblList > tbody").empty();

        var postedContent = { DivId: param.value }

        $.ajax({
            type: "POST",
            url: "/Phase/GetAreaByDivision",
            data: postedContent,
            success: function (message) {

                if (message.data.length > 0) {
                    $('#AreaId').append($('<option>', { value: '0', text: 'Select Area', selected: true }));
                    for (var i = 0; i < message.data.length; i++) {
                        $('#AreaId').append($('<option>', { value: message.data[i].AreaId, text: message.data[i].AreaName }));
                    } // end of for loop
                }
                // end of if for length
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
            }
        });

        setTimeout(call_toggle, 500);

    }

    GetSubAreaByArea = function (param) {
        if (param.value == "") { return; }

        call_toggle();

        $("#SubAreaId").empty();
        $("#tblList > tbody").empty();

        var postedContent = { SelectedArea: param.value }

        $.ajax({
            type: "POST",
            url: "/Project/GetSubAreaByArea",
            data: postedContent,
            success: function (message) {
                if (message.data.length > 0) {
                    $('#SubAreaId').append($('<option>', { value: '0', text: 'Select Sub Area', selected: true }));
                    for (var i = 0; i < message.data.length; i++) {
                        $('#SubAreaId').append($('<option>', { value: message.data[i].SubAreaId, text: message.data[i].SubAreaName }));
                    } // end of for loop
                }
                else {
                    $('#SubAreaId').append($('<option>', { value: '', text: 'Select Sub Area', selected: true }));
                    $('#SubAreaId').append($('<option>', { value: '0', text: 'No Sub Area', selected: true }));
                }
                // end of if for length
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
            }
        });

        setTimeout(call_toggle, 500);

    }

    GetPhaseListBySubArea = function (param) {
        var AreaId = $('#AreaId').find(":selected").val();

        var sarea = (param == null) ? 0 : param.value;
        if (AreaId == "") { return; }

        call_toggle();

        $("#ProjectId").empty();
        $("#tblList > tbody").empty();

        var postedContent = { Area: AreaId, SubArea: sarea }

        //alert(AreaId +  " -- " + postedContent.SubArea );
        $.ajax({
            type: "POST",
            url: "/Phase/GetPhaseBySubArea",
            data: postedContent,
            success: function (message) {

                if (message.data.length > 0) {
                    $('#ProjectId').append($('<option>', { value: '0', text: 'Select Project', selected: true }));
                    for (var i = 0; i < message.data.length; i++) {
                        $('#ProjectId').append($('<option>', { value: message.data[i].ProjectId, text: message.data[i].ProjectName }));
                    } // end of for loop
                }
                // end of if for length


                filltable(message.list);

                setTimeout(call_toggle, 500);
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
                setTimeout(call_toggle, 500);
            }
        });



    }

    GetPhaseListByProject = function (param) {
        if (param.value == "") { return; }

        call_toggle();

        $("#tblList > tbody").empty();

        var postedContent = { ProjectId: param.value }

        $.ajax({
            type: "POST",
            url: "/Phase/GetPhaseByProject",
            data: postedContent,
            success: function (message) {
                filltable(message.data);
            },
            error: function (xerr) {
                /* $('#txteng').val("err : " +xerr);*/
            }
        });

        setTimeout(call_toggle, 500);

    }
});

function filltable(obj) {
    var innerhtml = "";

    for (var i = 0; i < obj.length; i++) {
        var res = obj[i];
        innerhtml += "<tr>" +
                        "<td>" + (i + 1) + "</td>" +
                        "<td>" + res.ProjectName + "</td>" +
                        "<td>" + res.PhaseName + "</td>" +
                        "<td>" + parseJsonDate(res.StartDate) + "</td>" +
                        "<td>" + parseJsonDate(res.EndDate) + "</td>" +
                        "<td>" + ((res.IsActive == true) ? "<i class='fa fa-check-square text-navy'></i>" : "<i class='fa fa-square text-navy'></i>") + "</td>" +
                        "<td>" + res.UserName + "</td>" +
                        "<td>" + parseJsonDate(res.GeneratedDate) + "</td>" +
                        "<td>" +
                            "<a href='/Phase/Edit/" + res.PhaseId + "'>Edit    </a> |" +
                            "<a href='/Phase/Details/" + res.PhaseId + "'>Details </a> |" +
                            "<a href='/Phase/Delete/" + res.PhaseId + "'>Delete  </a>" +
                        "</td>" +
                     "</tr>";

    }
    // end of for loop

    $("#tblList > tbody").append(innerhtml);
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


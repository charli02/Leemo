$(document).ready(function () {
    var checkSession = $('#loginPage').val();
    if (checkSession == 'Login') {
        window.location.href = '/Account/Login';
    }
    fnGetCompanyLocations();
    fnActiveMenu();
});

$(document).on('hidden.bs.modal', function () {
    if ($('.modal.show').length) {
        $('body').addClass('modal-open');
    }
});

'use strict';

//custom-toast 
var _msgSave = "Your record has been successfully saved.",
    _msgUpdate = "Your record has been successfully updated.",
    _msgExists = "A similar record already exists. Let's make sure there are no duplicates.",
    _msgUserExists = "Email already registered.",
    _msgDelete = "Your record has been successfully deleted.",
    _msgInUse = "Record(s) you're trying to delete are being used.",
    _msgError = "Sorry, we messed up something. Please contact support or try again later.";
    _msgImage = "Image has been successfully Uploaded";
_msgErrorImage = "Please select Image to upload";
_msgExtImage = "Only JPG, JPEG, PNG images are allowed.";
_msgSizeImage = "File size limit exceeded (7MB)!";
imgBtnText = "Select As Profile Photo";
_updateBtnText = "Update";
_saveBtnText = "SAVE";
_CountryCode = "Please Select Country Code";
_ErrorPassword = "Your Old Password is Incorrect";
_SavePassword = "Password has been updated";
_selectRepotingUsers = "Please select Role Repoting To";
_ValidDateError = "Please Select A Valid Date";
_SelectImage = " Please select image";
function Alert(message, messageType) {
    handleToasters(message, messageType);  
}

var handleToasters = function (message, messageType) {
    switch (messageType) {
        case 'Success':
            handleToasterType('.success-toast', message);
            break;
        case 'Error':
            handleToasterType('.danger-toast', message);
            break;
        case 'Info':
            handleToasterType('.info-toast', message);
            break;
        default:
            handleToasterType('.warning-toast', message);
            break;
    }
};

var handleToasterType = function (type, message) {
    $('.toast').css("display", "none");
    $(type).toast("show");
    $(type).children('p').text(message);
    $(type).removeAttr('style');
    $(type).removeClass('fade');
    $(type).css('z-index','9999');
    $(type).stop().animate({ opacity: '100' }).fadeOut(10000);
};

function fnButtonLoader(e) {
    var $btn = $(e);




    $('*').off('click', fnRemoveBtnDisable);
   //$('*').unbind('click');
   // $(".close").bind('click',);
    //$(".ml-2 mb-1 close").on('click', fnRemoveBtnDisable);
   

    $btn.html('<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>').prop('disabled', true);

        //Show a transparent div
    //$("<div class='loader align-items-center justify-content-center' style='display: flex;'></div >")
    //        .css("opacity", "0.5")
    //                .appendTo(document.body);
}

function fnRemoveBtnDisable(e) {
    var $btn = $(e);

    $btn.html(imgBtnText).prop('disabled', false);
}
function fnSendRemoveBtnDisable(e) {
    var $btn = $(e);
    $btn.html("Send").prop('disabled', false);
}
function fnRemoveBtnDisableData(e) {
    var $btn = $(e);
    $btn.html(_updateBtnText).prop('disabled', false);
}

function fnRemoveSaveBtnDisable(e) {
    var $btn = $(e);
    $btn.html(_saveBtnText).prop('disabled', false);
}



function fnAccessDenied() {
      $('#access_denied-modal').modal('show');
}

function fnAddNewLocation() {
    var get = $.get("/Location/CreateLocation");
    get.success(function (result) {
        if (forceLogOut(result)) {
            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }
            $("#newLocation").html(result);
            $("#newLocation").modal("show");
        }
        })
}

function fnGetCompanyLocations() {

}

// Save Location Information
function saveLocation(e) {
    event.preventDefault();
    $('#txtLocationName').val($('#txtLocationName').val().trim());
    if (checkLocationName())
        return false;
    var Data = $('#createLocationForm'),
        Url = '/Location/CreateLocation';
    if (Data.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                $('#newLocation').modal("hide");
                Alert(_msgSave, 'Success');
                GetLocationList();
                fnCountLocationByUser();
            }
        })
    }
}

function scrollToTop() {
    $(window).scrollTop(0);
}

function closetoast(e) {
    $(e).closest(".toast").toast("hide");
}

function forceLogOut(data) {
    if ($.isNumeric(data)) {
        if (parseInt(data) == -2) {
            $('.modal').modal('hide');
            $('#access_denied-modal').attr('onclick', 'window.location.reload()');
            $("#access_denied-modal").modal("show");
            return false;
        }
    }

    var isHtml = /<(?=.*? .*?\/ ?>|br|hr|input|!--|wbr)[a-z]+.*?>|<([a-z]+).*?<\/\1>/i.test(data);
    if (isHtml) {
        //var isforceLogOut = $(data).find("#loginPage").val();
        var isforceLogOut = $(data).find("#SessionOut").val();
        if (isforceLogOut != undefined && isforceLogOut == 'Login') {
            $('.modal').modal('hide');
            $("#sessionLogOutModal").modal("show");
            return false;
        }
    }
    return true;
}



function setMonth() {
    $("#alertdob").addClass("hide");
    $("#selectDOBMonth").find('option').remove().end().append("<option value='' selected>Month</option>")
    $("#selectDOBDay").find('option').remove().end().append("<option value='' selected>Day</option>");
    
    if ($("#selectDOBYear").val() != "") {
        for (var i = 0; i < 12; i++) {
            $("#selectDOBMonth").append("<option value = " + (i + 1) + ">" + (i + 1) + "</option>");
        }

        $('.nice-select2').niceSelect('destroy');
        $('.nice-select2').niceSelect();
        $('.nice-select2 ul').addClass('demo-y');
        $(".demo-y").mCustomScrollbar({
            theme: "minimal-dark"
        });
        $('#createUserForm,#editUserForm').find('.nice-select2 .current').addClass('wraptext');
    }
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}
function getDaysInMonth() {
    $("#alertdob").addClass("hide");
    $('#selectDOBDay').empty();
    var SelectedMonth = parseInt($("#selectDOBMonth").val());
    var year = $("#selectDOBYear").val();
    if (SelectedMonth == "" || year == "") {
        Alert(_ValidDateError, "Error")
        return false;
    }
    else {
        var daysInSelectedMonth = daysInMonth(SelectedMonth, year);
        $('#selectDOBDay').append("<option value='' selected> Day </option>");
        for (var i = 1; i <= daysInSelectedMonth; i++) {
            $('#selectDOBDay').append($("<option value=" + i + "></option>").attr("value", i).text(i));
        }
        $('.nice-select2').niceSelect('destroy');
        $('.nice-select2').niceSelect();
        $('.nice-select2 ul').addClass('demo-y');
        $(".demo-y").mCustomScrollbar({
            theme: "minimal-dark"
        });
        $('#createUserForm,#editUserForm').find('.nice-select2 .current').addClass('wraptext');

    }

    // $("#dayCol").css("display", "block");
}

//Delete Profile Success 
function DeleteProfile() {
    debugger;
    var infoID = $("#deleteProfileID").val();
    var tab = infoID.split("_")[0];
    infoID = infoID.split("_")[1];

    if (tab == "Profile") {
        var get = $.get("/SecurityControls/DeleteProfile?id=" + infoID);
        get.success(function (result) {
            if (forceLogOut(result)) {
                if (result.returnValue == 1) {
                    $("#deleteModal").modal("hide");
                    SecurityProfile();
                    Alert(_msgDelete, 'Success');
                  
                    $('#selectUserProfile').parent().find('.nice-select').removeClass('disabled').find("ul li:first").trigger("click");
                   // $("#user-info").find("#UserRole").html("");
                   // $("#user-info").find("info-area mt-5 > #UserRole").html("");
                  
                }
                else if (result.returnValue == -1) {
                    Alert(result.errorMessage, 'Warning')
                }
                else {
                    Alert(_msgError, "Error");
                }
                scrollToTop();
            }
            //if (forceLogOut(result)) {
            //    $("#deleteModal").modal("hide");
            //    SecurityProfile();
            //    Alert(_msgDelete, 'Success');
            //}
        })
    }
    else if (tab == "BillingAddress"){
        var get = $.get("/Billing/DeleteBillingAddress?id=" + infoID);
        get.success(function (result) {
            debugger;
            if (forceLogOut(result)) { 
                if (result == 0) {
                    $("#deleteModal").modal("hide");
                    GetBillingList();
                    Alert(_msgDelete, 'Success');
                }
                else if (result.returnValue == -1) {
                    Alert(result.errorMessage, 'Warning')
                }
                else {
                    Alert(_msgError, "Error");
                }
                scrollToTop();
            }
        })
    }
    else {
        var get = $.get("/SecurityControls/DeleteDesignations?id=" + infoID);
        get.success(function (result) {
            if (forceLogOut(result)) {
                if (result.returnValue == 1) {
                    $("#deleteModal").modal("hide");
                    ShowDesignation();
                    Alert(_msgDelete, 'Success');
                }
                else if (result.returnValue == -1) {
                    Alert(result.errorMessage, 'Warning')
                }
                else {
                    Alert(_msgError, "Error");
                }
                scrollToTop();
            }
        })
    }
}

function fnActiveMenu(){
    $('.listing ul li a.active').parent().addClass("active");
    $('.listing ul li a.active').children().last().css("color", "white");
}
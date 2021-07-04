'use strict';

//custom-toast 
var

    _msgSave = "Your record has been successfully saved.",
    _msgUpdate = "Your record has been successfully updated.",
    _msgExists = "A similar record already exists. Let's make sure there are no duplicates.",
    _msgDelete = "Your record has been successfully deleted.",
    _msgInUse = "Record(s) you're trying to delete are being used.",
    _msgError = "Sorry, we messed up something. Please contact support or try again later.",
    _msgSelectDomain = "This Domain name is not available.",
    _msgSelectDomainOption = "You Can Try following Suggestion.",
    _msgNotAvailable = 'This Domain name is not available.',
    _msgAvailable = ' This Domain name is  available.',
    _msgDomainValidation = '*Start with alphabets, Enter numbers and alphabets only.',
    _finishSignUproster = 'Verify your email to finish signing up with Uproster',
    _msgUserExists = "Email already exists.",
    _msgCompanyNameExists = "Company already exists.",
    _msgRequired = "*Required",
    _SubmitBtnText="Submit";

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
    $(type).toast("show");
    $(type).children('p').text(message);
    $(type).removeAttr('style');
    $(type).css('z-index', '9999');
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
    $btn.html(_SubmitBtnText).prop('disabled', false);
}

function fnRemoveSaveBtnDisable(e) {
    var $btn = $(e);
    $btn.html(_saveBtnText).prop('disabled', false);
}

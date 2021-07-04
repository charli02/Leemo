'use strict';



// Create/insert CompanySetup
function CreateProductLead(e) {

    $('#finishSignUproster').text(null);

    var Data = $('#CompanySetup'),
        Url = '/CompanySetup/Index';
    event.preventDefault();
    if (Data.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {

            fnRemoveBtnDisableData(e);
            if (parseInt(result.success) == 4) {
                /* Alert(_msgExists);*/
                $("#finishSignUproster").text(result.message);
                return false;
            }

            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }

            if (parseInt(result.success) > 0) {
                if (parseInt(result.success) == 1) {
                    /* Alert(_msgSave, 'Success');*/
                    $('#finishSignUproster').text(_finishSignUproster);
                    return false;
                }
            }
            else
                Alert(_msgError, 'Error');


        })


    }
}

// Update ProductLead
function UpdateProductLead(e) {

    var Data = $('#WebDomain'),

        Url = '/CompanyRegistration/EditProductLead';

    event.preventDefault();
    if (Data.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {

            fnRemoveBtnDisableData(e);
            if (parseInt(result.success) == 4) {
                /* Alert(_msgExists);*/
                // $("#finishSignUproster").text(result.message);
                return false;
            }

            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }

            if (parseInt(result.success) > 0) {
                if (parseInt(result.success) == 1) {
                    /* Alert(_msgSave, 'Success');*/
                    $('#finishSignUproster').text(_finishSignUproster);

                }
            }
            else
                Alert(_msgError, 'Error');

        })

    }
}
 
function checkDomain() {


    $("#DomainSuggestion").addClass("d-none");
    $("#DomainSuggestion").html("");
    $("#existMsg").text("");

    $("#existMsg").addClass("text-danger");

    var domainName = $("#ValidateDomainName").val().trim();

    var isValidDomainResponse = isValidDomain(domainName);

    if (isValidDomainResponse == false) {
        $("#existMsg").text(_msgDomainValidation);
        return false;
    }

    var DomainMaxLength = $("#DomainMaxLength").val();
    var DomainMinLength = $("#DomainMinLength").val();

    if (!(domainName.length >= parseInt(DomainMinLength) && domainName.length <= parseInt(DomainMaxLength))) {

        $("#existMsg").text("DomainName should be  between " + DomainMinLength + " and " + DomainMaxLength + " characters");

        return false;
    }

    if (domainName != "") {
        //var get = $.get("/CompanySetup/GetAvailableDomain/domainName/" + domainName);
        var get = $.get("/CompanyRegistration/GetAvailableDomain?domainName=" + domainName);
        get.done(function (result) {

            if (parseInt(result.success) == 4) {

                $("#DomainSuggestion").removeClass("d-none");
                $("#DomainSuggestion").html("");
                $("#ValidateDomainName").val(domainName);
                $("#existMsg").text(_msgNotAvailable);
                $("#existMsg").removeClass("hidden");



                if (result.message.length > 0) {
                    $('#DomainSuggestion').append('<p>' + _msgSelectDomainOption + '</p>   <ul id="DomainSuggestionul" >');

                    $.each(result.message, function (i, item) {

                        /*<li>https://<span>item</span>.uproster.io</li>*/
                        $('#DomainSuggestionul').append('<li onclick="SugeestedSelectDomain(\'' + item + '\')"> <span>' + item + '</span> </li>');
                    });
                    $('#DomainSuggestion').append('</ul>');

                    $("#DomainSuggestion").removeClass("d-none");
                    $("#DomainSuggestion").addClass("suggestion-box");


                }

                return false;
                /* $('#hfUserExist').val(1);*/
            }
            if (parseInt(result.success) == 6) {
                $("#DomainSuggestion").addClass("d-none");
                $("#DomainSuggestion").html("");
                $("#ValidateDomainName").val(domainName);
                $('#DomainName').val(domainName)
                $("#existMsg").text(_msgAvailable);
                /* $("#existMsg").addClass("hidden");*/
                $("#existMsg").addClass("text-success");
                $("#existMsg").removeClass("text-danger");
                return true;
                /*$('#hfUserExist').val(0);*/
            }

            $("#existMsg").text(_msgDomainValidation);
            $("#existMsg").addClass("text-danger");
        });

       
    }
    else {
        $("#existMsg").text("Not Available");
        $("#existMsg").removeClass("hidden");
    }
    // alert(getEmail);
}
//Validate DomainName
function isValidDomain(str) {
    if (str.match(/^[a-zA-Z][a-zA-Z0-9]*$/)) {
        return true;
    } else {
        return false;
    }
}


function SugeestedSelectDomain(Selected) {

    $("#ValidateDomainName").removeAttr("value");
    $("#ValidateDomainName").val(Selected);
    $("#ValidateDomainName").attr("value", Selected);
    checkDomain();
}


function PasswordGenrate() {
    var Data = $('#WebDomain');
    var ValidateDomainName = $('#ValidateDomainName').val();
    var DomainName = $('#DomainName').val();

    if (DomainName == "" || ValidateDomainName == "") {

        if (ValidateDomainName == "") {
            $('#existMsg').text(_msgRequired);
        }
        else {
            $('#existMsg').text(_msgNotAvailable);
        }


        var get = $.get("/CompanyRegistration/Address", Data.serialize());
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").removeClass('js-active');
            $("#OrderInfo").removeClass('js-active');
            $("#Address").removeClass('js-active');
            return false;
        });

        return false;
    }

    
   
    event.preventDefault();
    if (Data.valid()) {
        var get = $.get("/CompanyRegistration/PasswordGenrate", Data.serialize());
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").addClass('js-active');
            $("#OrderInfo").addClass('js-active');
            $("#Address").addClass('js-active');
            $("#btncheckDomain").addClass('d-none');

            return false;
        });
    }
    else {
       
        return false;
    }
}
function VerifyAddress() {
    var Id = $('#Id').val();
    //$(".uproster-domain").removeClass('d-none');
    $('#ValidateDomainName').prop('readonly', false);
    event.preventDefault();
   
    var get = $.get("/CompanyRegistration/Address?Id=" + Id);
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").removeClass('js-active');
            $("#OrderInfo").removeClass('js-active');
            $("#Address").removeClass('js-active');
            var DomainName = $('#DomainName').val();
            $('#ValidateDomainName').val(DomainName);
            $("#btncheckDomain").removeClass('d-none');
            return false;
        });
    
}

function OrderInfo() {
    $('existMsg').text('');
    var Data = $('#WebDomain');
    var ValidateDomainName = $('#ValidateDomainName').val();
    var DomainName = $('#DomainName').val();
 
    if (DomainName == "" || ValidateDomainName == "") {

        if (ValidateDomainName == "") {
            $('#existMsg').text(_msgRequired);
        }
        else {
            $('#existMsg').text(_msgNotAvailable);
        }


        var get = $.get("/CompanyRegistration/Address", Data.serialize());
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").removeClass('js-active');
            $("#OrderInfo").removeClass('js-active');
            $("#Address").removeClass('js-active');
            return false;
        });
        
        return false;
    }

    event.preventDefault();
    if (Data.valid()) {
        var post = $.post("/CompanyRegistration/OrderInfo", Data.serialize());
        post.done(function (result) {
            $("#VerifyWebDomain").html(result);
           // $(".uproster-domain").addClass('d-none');


            $("#Comments").removeClass('js-active');
            $("#btncheckDomain").addClass('d-none');
            $('#ValidateDomainName').prop('readonly', true);
            $("#OrderInfo").removeClass('js-active');
            $("#Address").addClass('js-active');
            return false;
        });
    }
    else {
      
        return false;
    }
}
function orderComments() {
    var Data = $('#WebDomain');
    var ValidateDomainName = $('#ValidateDomainName').val();
    var DomainName = $('#DomainName').val();

    if (DomainName == "" || ValidateDomainName == "") {

        if (ValidateDomainName == "") {
            $('#existMsg').text(_msgRequired);
        }
        else {
            $('#existMsg').text(_msgNotAvailable);
        }
        

        var get = $.get("/CompanyRegistration/Address", Data.serialize());
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").removeClass('js-active');
            $("#OrderInfo").removeClass('js-active');
            $("#Address").removeClass('js-active');
            return false;
        });

        return false;
    }

    $('#u_password').val(null);
    $('#u_confirm_password').val(null);

  
    event.preventDefault();

    if (Data.valid()) {
        var get = $.get("/CompanyRegistration/Comments", Data.serialize());
        get.done(function (result) {
            $("#VerifyWebDomain").html(result);
            $("#Comments").removeClass('js-active');
            $('#ValidateDomainName').prop('readonly', true);
            $("#OrderInfo").addClass('js-active');
            $("#btncheckDomain").addClass('d-none');
            $("#Address").addClass('js-active');
            return false;
        });

    }
    else {
       
        return false;

    }

}


function ValidatePassword() {

    $('#passwordValidate').text("");
   var upassword= $('#u_password').val();
    var Confirmpassword = $('#u_confirm_password').val();

    if (upassword == "" || upassword== null){

        $('#passwordValidate').text("Please enter value");
        $('#passwordValidate').removeClass("text-danger");
        $('#passwordValidate').addClass("text-success");
        return false;
    }

    if (upassword.toLowerCase().trim() == Confirmpassword.toLowerCase().trim()) {

        $('#passwordValidate').text("Password Match");
        $('#passwordValidate').removeClass("text-danger");
        $('#passwordValidate').addClass("text-success");
        return false;
    }

    $('#passwordValidate').text("Not match");
    $('#passwordValidate').removeClass("text-success");
    $('#passwordValidate').addClass("text-danger");
    return false;


    //var Data = $('#WebDomain');
    //event.preventDefault();

    //if (Data.valid()) {
    //    var get = $.get("/CompanyRegistration/Comments", Data.serialize());
    //    get.done(function (result) {
    //        $("#VerifyWebDomain").html(result);
    //        $("#Comments").removeClass('js-active');
    //        $("#OrderInfo").addClass('js-active');
    //        $("#Address").addClass('js-active');
    //        return false;
    //    });

    //}
    //else {

    //    return false;

    //}

}






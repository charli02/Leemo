$(document).ready(function () {
    var accessDenied = $('#UserAccessDenied').val();
    if (accessDenied != '-1') {
        init();
    }
})

function init() {
    fnTriggerUserList();
    $('#roles').removeAttr('multiple');
    $('#selectUserProfile').removeAttr('multiple');
    $.get('/User/UserList', { QuerySearch: null, GetActiveGroups: 1 }, function (data) {
        if (forceLogOut(data)) {
            $("#_UserList").html(data);
            fnTriggerUserList();
        }
    })
}

var fnTriggerUserList = function () {
    $('#_UserList li').find('input').first().trigger('click');
}

//GET USER DETAILS BY ID
function UserDetails(id) {
    var get = $.get("/User/UserDetails?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }
            $("#user-info").html(result);
        }
    })
}

//User search button Click Event
$('input#QuerySearch').keyup(function (event) {
    var x = event || window.event;
    var ival = this.value;
    if (ival.includes('#') || ival.includes('+') || x.keyCode == 35) {
        var ilen = ival.length;
        ival = ival.slice(0, ilen - 1);
        $('#QuerySearch').val(ival);
        event.preventDefault();
        return false;
    }
    $("#searchImg").hide();
    $("#closeImg").show();
    var inputValue = this.value;
    if (inputValue) {
        $('.search-users').addClass('clear-toggle')
    } else {
        $('.search-users').removeClass('clear-toggle')
    }
    UserList();
});

function fnOnClickUserCancelBtn() {

    if ($('#QuerySearch').val() != "") {
        $('#QuerySearch').val("");
        $("#closeImg").hide();
        $("#searchImg").show();
        UserList();
    }
}

//FOR USER LIST
function UserList() {
    if ($("#QuerySearch").val() == "") {
        $("#closeImg").hide();
        $("#searchImg").show();
    }
    $("#_UserList").html('');
    ShowPartialLoader();
    var _GetActiveUsrs = $('#GetActiveUsrs').val();
    var _QuerySearch = $('#QuerySearch').val();
    var get = $.get("/User/UserList?QuerySearch=" + _QuerySearch + "&GetActiveUsrs=" + _GetActiveUsrs);
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();
            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }
            $("#_UserList").html(result);
            var getPartialUserInfo = $("#_UserList").text().trim();
            if (getPartialUserInfo == "Record not found.") {
                $("#user-info").html("<p class= 'text-center mt-5 textPara'> No Record Exist</p >");
                fnUpdateActiveFilterUsersCounts();
            }
            else {
                fnUpdateActiveFilterUsersCounts();
                fnTriggerUserList();
            }
        }

    })
}

function fnUpdateActiveFilterUsersCounts() {

    var allUsers = $("#hfAllUserCount").val();
    var activeUsers = $("#hfUserActiveCount").val();
    var inActiveUsers = $("#hfUserInActiveCount").val();


    $('li[data-value="0"]').html("All Users(" + allUsers + ")");
    $('li[data-value="1"]').html("Active Users(" + activeUsers + ")");
    $('li[data-value="2"]').html("InActive Users(" + inActiveUsers + ")");
    $('span[class="current"]').html($('li[data-value="' + $('#GetActiveUsrs').val() + '"]').html().trim());
}


//FOR CREATE USER
function CreateUser() {
    $.ajax(
        {
            url: "/User/CreateUser/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    $("#newUser").html(result);
                    $('#createUserForm').find('.nice-select .current').addClass('wraptext');
                    $('#createUserForm').find('.nice-select2 .current').addClass('wraptext');
                    $("#newUser").modal("show");

                }
            }
        });
}

//FOR EDIT USER MODAL
function EditUser(id) {
    var returnPageType = $('#returnFrom').val();
    $.ajax(
        {
            url: "/User/EditUser/",
            method: "GET",
            data: { id: id, returnPageType: returnPageType },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    $("#userEditModal").html(result);
                    $("#userEditModal").modal("show");
                    var input = document.querySelector("#userProfile_Mobile");
                    var instance = window.intlTelInput(input, {
                        utilsScript: "../lib/intlTelInput/js/utils.js",
                        preferredCountries: [''],
                        formatOnDisplay: false
                    });
                    var CountryCode = $('#CountryCode').val().trim().toLowerCase();




                    if (CountryCode == "") {
                        instance.setCountry("in");

                    } else {
                        instance.setCountry(CountryCode);
                    }

                    $('.adj-width').find('.nice-select .current').addClass('wraptext');
                    $('.adj-width').find('.nice-select2 .current').addClass('wraptext');
                }
            }
        });
}

function saveUser(e) {
    event.preventDefault();
    var validReportingToUser = checkValidReportingToUser();

    if ($("#selectUserRole").val() == null || $("#selectUserRole").val() == "") {
        $("#DesignationData").css({ "display": "block" });

    }
    else {
        $("#DesignationData").css({ "display": "none" });
    }

    if ($("#roles").val() == null || $("#roles").val() == "") {
        $("#UserProfile").css({ "display": "block" });

    }
    else {
        $("#UserProfile").css({ "display": "none" });
    }

    var Data = $('#createUserForm'),
        Url = '/User/CreateUser';
    if (Data.valid() && validReportingToUser) {
        if ($("#selectUserRole").val() == null || $("#selectUserRole").val() == "-- select an option --") {
            $("#DesignationData").css({ "display": "block" });
            return false;
        }

        if ($("#roles").val() == null || $("#roles").val() == "-- select an option --") {
            $("#UserProfile").css({ "display": "block" });
            return false;
        }
        //var selectRepotingUsers = $('#selectRepotingUsers').val();
        //if (selectRepotingUsers == "") {
        //    Alert(_selectRepotingUsers, 'Error');
        //    return false;
        //}

        fnButtonLoader(e);
        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                fnRemoveBtnDisableData(e);
                if (parseInt(result) == 4) {
                    //Alert(_msgUserExists, 'Warning');
                    $("#existMsg").text(_msgUserExists);
                    return false;
                }

                if (parseInt(result) <= 0) {
                    if (parseInt(result) < 0) {
                        Alert(_msgError, "Error");
                        return false;
                    }
                }
                $('#newUser').modal("hide");

                if (parseInt(result) > 0) {
                    if (parseInt(result) == 1) {
                        Alert(_msgSave, 'Success');
                        UserList();
                        $('#_UserList').closest('li').find('input').trigger();
                    }
                }
                else
                    Alert(_msgError, 'Error');

            }
        })
    }
}


function resetPassword() {
    var formdata = $('#resetPassForm');
    if (formdata.valid()) {
        $.ajax({
            url: '/Account/ResetPassword',
            method: 'POST',
            data: formdata.serialize(),
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    if (result.success == true) {
                        window.location.href = "/Account/Login";
                    }
                    else
                        Alert(result.message, 'Error');
                }
            }
        });
    }
}

function changePassword() {
    var formdata = $('#changePassForm');
    var firstLogin = $('#FirstLogin').val();
    var URl = '/Account/ChangePassword';
    if (firstLogin == "FirstLogin") {
        URL = '/Account/ResetPassword';
    }
    if (formdata.valid()) {
        $.ajax({
            url: URL,
            method: 'POST',
            data: formdata.serialize(),
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");

                        return false;
                    }
                    if (result.success == true) {
                        window.location.href = "/Account/Login";
                    }
                    else
                        Alert(result.message, 'Error');
                }
            }
        });
    }
}

function forgotPassword(e) {
    var formdata = $('#forgotPassForm');
    if (formdata.valid()) {
        fnButtonLoader(e);
        $.ajax({
            url: '/Account/ForgotPassword',
            method: 'POST',
            data: formdata.serialize(),
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        fnSendRemoveBtnDisable(e);
                        return false;
                    }
                    if (result.success == true) {
                        window.location.href = "/Account/Login";
                    }
                    else {
                        fnSendRemoveBtnDisable(e)
                        Alert(result.message, 'Error');
                    }
                }
            }
        });
    }
}

//function checkAlreadyExistsUser() {
//    var getEmail = $("#userEmail").val();
//    var isValidEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
//    if (isValidEmail.test(getEmail)) {
//        $.ajax(
//            {
//                url: "/User/GetUserEmail/",
//                method: "GET",
//                data: { "Email": getEmail },
//                success: function (result) {
//                    if (parseInt(result) == 1) {
//                        // Already exists in same location
//                        $("#userEmail").val("");
//                        $("#existMsg").text(_msgExists);
//                        $("#existMsg").removeClass("hidden");
//                    }
//                    else if (parseInt(result) == 2) {
//                        //Already exists in other location and same company
//                        var $modalUserLocationMapping = $("#modalUserLocationMapping");
//                        $('#lblUserAlreadyExists').html('<b>' + getEmail + '</b> user already exists in [location] location.<br/> Do you want to add it to ' + $('#hfCurrentCompanyLocationName').val() + ' location also ?');
//                        $modalUserLocationMapping.modal('show');
//                        $('#newUser').modal('hide');
//                    }
//                    else if (parseInt(result) == 0) {
//                        //Not exists (New user)
//                        $("#userEmail").val(getEmail);
//                        $("#existMsg").text("");
//                        $("#existMsg").addClass("hidden");
//                    }
//                    else {
//                        return true;
//                    }
//                }
//            });
//    }
//}

//Add Existing User Functionality - Commented
function fnHandleActiveExistingUser(username) {
    var Url = '/User/GetCompanyUsersExceptCurrentCompanyLocation?username=' + username;
    var getting = $.get(Url);
    getting.done(function (result) {
        if (forceLogOut(result)) {
            $('#existingUserList').html('');
            var innerHtml = '';
            jQuery.each(result, function (i, val) {
                innerHtml += '<option data-companyLocationId=' + val.companyLocationId + ' data-userId=' + val.id + ' value=' + val.email + '></option>';

            });
            $('#existingUserList').html(innerHtml);

        }
    })
}

//function fnActiveExistingUser(e) {
//    BlockUI();
//    if ($(e).is(':checked')) {
//        $('#divEditableUserDropdown').removeClass('hide');
//        fnHandleActiveExistingUser();

//    }
//    else {
//        CreateUser();
//    }

//    UnBlockUI();

//}

function BlockUI() {
    $('#newUser').block({ message: null });
}

function UnBlockUI() {
    setTimeout(function () { $('#newUser').unblock(); }, 1000);
}


//DATALIST ONCHANGE LISTNER - Commented
var timer;
$("[name='UserName']").on('change', function (e) {
    timer = setTimeout(function () {
        checkEmail();
        var isUserExists = $('#hfUserExist').val();
        if (isUserExists == 0)
            fnGetExistingUserData();
    }, 1);
});
$("[name='UserName']").on('blur', function (e) {
    clearTimeout(timer);
});

function fnGetExistingUserData() {
    var ddl = $('[name="UserName"]').val();
    var userid = $('#existingUserList option[value="' + ddl + '"]').attr('data-userId');
    var companylocationid = $('#existingUserList option[value="' + ddl + '"]').attr('data-companyLocationId');
    BindExistingUserRecord(userid, companylocationid);
}

function BindExistingUserRecord(userid, locationid) {
    $.ajax(
        {
            url: "/User/GetExistingUserData",
            method: "GET",
            data: { UserId: userid, CompanyLocationId: locationid },
            success: function (result) {
                if (forceLogOut(result)) {
                    $("#newUser").html(result);
                    $('#createUserForm').find('.nice-select .current').addClass('wraptext');
                    $('#createUserForm').find('.nice-select2 .current').addClass('wraptext');
                    $("#warningMessage").removeClass("hide");
                    fnHandleActiveExistingUser();
                    fnDisabledUserFields();
                }
            }
        });
}

function fnDisabledUserFields() {
    $("#IsExistingUser").prop('checked', true);
    $("#userProfile_FirstName").attr("readonly", "readonly").addClass("not-allowed");
    $("#userProfile_LastName").attr("readonly", "readonly").addClass("not-allowed");
    //  $("#userEmail").attr("readonly", "readonly").removeAttr("onblur").addClass("not-allowed");
    $('#selectUserRole').parent().find('.nice-select').addClass('disabled').addClass('readOnlyColor');
    $('#selectRepotingUsers').parent().find('.nice-select2').addClass('disabled').addClass('readOnlyColor');
    $("#userProfile_Description").attr("readonly", "readonly").addClass("not-allowed");
}


function getprofiles() {
    $("#UserProfile").css({ "display": "none" });
}

//function getReportingToUserId() {
//    $("#ReportingUser").css({ "display": "none" });
//}

$("#userEmail").keyup(function () {
    checkEmail();
    var username = $("#userEmail").val();
    if (username != null && username.length >= 3)
        fnHandleActiveExistingUser(username);
    else {
        $('#existingUserList').html("");
        //var isUserExists = $('#hfUserExist').val();
        //var oldEmail = $('#hfUserExist').attr("data-val");
        //if (isUserExists == 0 && oldEmail != username) {
        //        CreateUser();
        //}
        return;
    }
});
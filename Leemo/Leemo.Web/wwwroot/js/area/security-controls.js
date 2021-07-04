$(document).ready(function () {
    init();
})

function init() {
    InitAccordionToggle();
    fnExpandAccordian();
    $('#_ProductList').css('display', 'block');
    fnTriggerProduct();
    var designation_tab = $('#designation-tab').attr('class');
    if (designation_tab.includes('active')) {
        $('#_ProductList').css('display', 'none');
    }
}

//----------------------------Tab Role--------------------------//

//GEt UserProfile
function SecurityProfile() {
    $.ajax(
        {
            url: "/SecurityControls/SecurityProfile/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#SecurityProfileResult").html(result);
                    $('#profiles .profile-permissions').css("display", "none");
                    $('#profiles .general-setting').css("display", "block");
                }
            }
        });
}

function GetCreateProfile() {
    $.ajax(
        {
            url: '/SecurityControls/CreateNewProfile',
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#newProfile").html(result);
                    $("#newProfile").modal("show");
                }
            }
        });
}

//Create new Profile
function CreateProfile(e) {
    event.preventDefault();
    if (!checkRoleName()) {
        var formData = $('#CreateProfile');
        var Url = '/SecurityControls/CreateNewProfile';
        if (formData.valid()) {
            fnButtonLoader(e);
            var posting = $.post(Url, formData.serialize());
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    fnRemoveSaveBtnDisable(e);
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    Alert(_msgUpdate, 'Success');
                    SecurityProfile();
                    $("#newProfile").modal("hide");
                }
            });
        }
    }
    


   
}

//PROFILE PERMISSION GET METHOD
function ProfilePermission(id, name) {
    var selectedProductId = $('#_ProductList li a.nav-link.active').attr('data-id');
    var get = $.get("/SecurityControls/ProfilePermissions?id=" + id + "&auth_role=" + name + "&ProductId=" + selectedProductId);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#profiles .profile-permissions").html(result);
            $('#profiles .general-setting').css("display", "none");
            $("#profiles .profile-permissions").css("display", "block");
        }
    })
}

//PROFILE USERS GET METHOD
function ProfileUsers(id) {
    $("#profileUserList").html("");
    $("#QuerySearch").val("");
    var get = $.get("/SecurityControls/ProfileUsers?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#profileUserList").html(result);
            //ADDING PROFILE ID WITH USER DATA FOR SEARCHING PURPOSES
            $("#viewuserProfile #searchProfileID").attr('value', id);
            $("#cancelImg").hide();
            $("#viewuserProfile #QuerySearch").val("");
            $("#viewuserProfile").modal("show");
        }
    })
}

//BACK TO PERMISSION PAGE
function backToPermission() {
    SecurityProfile();
};

//SEARCH PROFILE USERS BY THE DATA
function SearchProfileUsers(a, b) {
    if ($("#QuerySearch").val() == "") {
        $("#cancelImg").hide();
        $("#searchImg").show();
    } else {
        $("#searchImg").hide();
        $("#cancelImg").show();

    }
    $("#profileUserList").html("");
    ShowPartialLoader();
    _searchProfileID = a;
    _QuerySearch = b;
    var get = $.get("/SecurityControls/ProfileUsers?id=" + _searchProfileID + "&QuerySearch=" + _QuerySearch);
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();
            $("#viewuserProfile .partialLoader").addClass("hide");
            $("#profileUserList").html(result);
        }
    })
}

// Delete Profile Infomation Conformation On Security Controles
function DeleteProfileInfo(id) {
    $("#deleteProfileID").val("Profile_" + id);
    if (id != null) {
        $("#deleteModal h3").html("Are you sure you want to delete this Role ?");
        $("#deleteModal").modal("show");
    }
}



function ShowProfileTab() {
    $('#_ProductList').css('display', 'block');
    $("#designation").css("display", "none");
    $("#profiles").show();
    $('#profiles .general-setting').css("display", "block");
}

//---------------Feature level permission start--------------
function BlockUI() {
    $('#profilePermissions').block({ message: null });
}

function UnBlockUI() {
    setTimeout(function () { $('#profilePermissions').unblock(); }, 1000);
}

function fnSaveFeaturePermission(Data) {
    var Url = '/SecurityControls/UpdateFeaturePermission';
    var posting = $.post(Url, Data);
    posting.done(function (result) {
        if (forceLogOut(result)) {
            //UnBlockUI();
            var activeCode = '';
            var featureId = '';

            if (parseInt(result.responseType) > 0) {
                $.each(result.rowData, function (key, val) {
                    featureId = val.featureId;
                    var feature_code_id = val.codeId + "_" + val.featureId;
                    if (val.isDeleted) {
                        $("#" + feature_code_id).prop('checked', false);
                    }
                    else {
                        $("#" + feature_code_id).prop('checked', true);
                        activeCode += $("#" + feature_code_id).next().html() + ', ';
                        $('#' + val.featureId + " li:nth-last-child(2) span").text(activeCode.slice(0, -2));
                    }
                });
                if (!activeCode.includes(",")) {
                    $('#' + featureId + " li:nth-last-child(2) span").text('');
                    $('#' + featureId + " li:nth-last-child(3) input").prop('checked', false);
                }
                else
                    $('#' + featureId + " li:nth-last-child(3) input").prop('checked', true);

                $('#savePermissions, #discardPermissions, #warningMessage').removeClass('hide');
            }
            else
                Alert(_msgError, 'Error');
        }
    });
}

function fnOnChangeFeaturePermission(c, f, r, e) {
    //BlockUI();
    $('#roleName').val();
    var isDeleted = true;
    var isViewPermission = true;
    var codeValueArray = [];

    if ($(e).is(':checked')) {
        isDeleted = false;
        $.each($(e).parent().parent().find('.drop-check > label'), function (index, value) {
            if ($(this).text().toLowerCase() == "view") {
                if ($('#' + $(this).attr('for')).is(':checked') == false) {
                    isViewPermission = false;
                }
            }
            else {
                codeValueArray.push($(this).text());
            }
        });
    }
    if ($(e).is(':checked') == false) {
        var x = $(e).siblings();
        var IsCodeChecked = false;
        $.each($(e).parent().parent().find('.drop-check > label'), function (index, value) {
            if ($('#' + $(this).attr('for')).is(':checked') == true) {
                IsCodeChecked = true;
            }
        });
        if (x.text().toLowerCase() == "view") {
            var _f = '_' + f;
            var _e = e.id;
            if (IsCodeChecked) {
                var $confirm = $("#modalConfirmYesNo");
                $('#btnCancel').removeClass('hide');
                $('#btnOk').attr('onclick', 'fnOnChangeBulkFeaturePermission("' + f + '", "' + r + '", "' + _f + '")');
                $('#btnCancel').attr('onclick', 'fnHandleCancelConfirmationModal("' + _e + '")');

                $confirm.modal('show');
                //UnBlockUI();
                return;
            }
            else {
                var Data = { FeatureId: f, CodeId: c, RoleId: r, IsDeleted: isDeleted };
                fnSaveFeaturePermission(Data);
            }
        }
    }

    if (isViewPermission == false) {
        $('#' + c + "_" + f).prop('checked', false);
        $('#lblViewPermissionMsg').text(codeValueArray.toString() + ' are not allowed without View Permission.')
        var $confirm = $("#modalConfirmYesNo");
        $('#btnCancel').addClass('hide');
        $confirm.modal('show');
        $('#btnOk').attr('onclick', 'fnHandlePermissionPropagation("' + f + '")');
       // UnBlockUI();
    }
    else {
        var Data = { FeatureId: f, CodeId: c, RoleId: r, IsDeleted: isDeleted };
        fnSaveFeaturePermission(Data);
    }
}

function fnHandleCancelConfirmationModal(e) {
    $('#' + e).prop('checked', true);
    $("#modalConfirmYesNo").modal('hide');
}

function fnHandlePermissionPropagation(f) {
    $("#modalConfirmYesNo").modal('hide');
}

function fnOnChangeBulkFeaturePermission(f, r, e) {
    //BlockUI();
    var roleName = $('#roleName').val();
    var isDeleted = true;
    if ($(e).is(':checked')) {
        isDeleted = false;
    }
    var Data = { FeatureId: f, RoleId: r, IsDeleted: isDeleted },
        Url = '/SecurityControls/BulkUpdateFeaturePermissions';
    var posting = $.post(Url, Data);
    posting.done(function (result) {
        if (forceLogOut(result)) {
            //UnBlockUI();
            var activeCode = '';
            if (parseInt(result.responseType) > 0) {
                $.each(result.rowData, function (key, val) {
                    var feature_code_id = val.codeId + "_" + val.featureId;
                    if (val.isDeleted) {
                        $("#" + feature_code_id).prop('checked', false);
                        $('#' + val.featureId + " li:nth-last-child(2) span").text('');
                    }
                    else {
                        $("#" + feature_code_id).prop('checked', true);
                        activeCode += $("#" + feature_code_id).next().html() + ', ';
                        $('#' + val.featureId + " li:nth-last-child(2) span").text(activeCode.slice(0, -2));
                    }
                });
                if (activeCode == '') {
                    $('#_' + f).prop('checked', false);
                }
                $('#savePermissions, #discardPermissions, #warningMessage').removeClass('hide');
            }
            else
                Alert(_msgError, 'Error');
        }
    });
}
//---------------Feature level permission end--------------

function UpdateProfile(id,e) {
    //if ($("#hdnProfileNameold").val() != $("#txtProName").val())
    //{
    //    checkRoleName();
    //}

    if (!checkRoleName()) {
        var objProfile = {};
        objProfile.Name = $("#txtProName").val();
        objProfile.Description = $("#txtProDesc").val();
        var formData = $('#updateProfile');
        var Url = '/SecurityControls/UpdateProfile?id=' + id;
        if (formData.valid()) {
            fnButtonLoader(e);
            var posting = $.post(Url, objProfile);
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    fnRemoveBtnDisableData(e);
                    if (result == '0') {
                        Alert(_msgUpdate, 'Success');
                        SecurityProfile();
                        $("#newProfile").modal("hide");
                    }
                }
            });
        }
    }

       
  //  }
}

//Rename Profile  
function RenameProfile(id) {
    var get = $.get("/SecurityControls/EditProfile?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#newProfile").html(result);
            $("#newProfile").modal("show");
        }
    })
}

function setSecirtyUpdatePopup() {
    $($('#SecurityProfileResult table tbody').find(' tr:nth-last-child(-n+3)')).find('.dropdown-menu');//.addClass('btm-opn')
}

function UpdateAuthRoleFeatureMappingChanges(id,e) {
//    BlockUI();
    var roleName = $('#roleName').val();
    Url = '/SecurityControls/UpdateAuthRoleFeatureMappingChanges?roleId=' + id;
    fnButtonLoader(e);
    var posting = $.get(Url);
    posting.done(function (result) {
        if (forceLogOut(result)) {
            setTimeout(function () { fnRemoveSaveBtnDisable(e); }, 1000);
            //UnBlockUI();
            if (parseInt(result) > 0) {
                ProfilePermission(id, roleName);
                Alert(_msgUpdate, 'Success');
            }
            else
                Alert(_msgError, 'Error');
        }
    });
}

function DiscardAuthRoleFeatureMappingChanges(id) {
    var roleName = $('#roleName').val();
    ProfilePermission(id, roleName);
    Alert('Changes Reverted.', 'Success');
}


//----------------------------Tab Designation--------------------------//

function newDesignationPopUp(ParentDesignationId) {
    event.stopPropagation();

  
        $.ajax(
            {
                url: "/SecurityControls/CreateDesignation/",
                method: "GET",
                success: function (result) {
                    if (forceLogOut(result)) {
                        if (result == "400") {
                            Alert(_msgError, "Error");
                            return false;
                        }
                        $("#newDesignation").html(result);
                        $("#ParentDesignationId").val('');
                        $("#newDesignation").modal("show");
                        $("#ParentDesignationId").val(ParentDesignationId);
                    }
                }
            });
    
   
}

function UpdateDesignation() {
    event.stopPropagation();
    $.ajax(
        {
            url: "/SecurityControls/EditDesignation/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#updateDesignation").html(result);
                    $("#updateDesignation").modal("show");
                }
            }
        });
}


function SaveDesignation() {
    if (!checkDesignationName()) {
        var Data = $("#DesignationAddForm");
        if (Data.valid()) {
            $.ajax(
                {
                    url: "/SecurityControls/CreateDesignation",
                    method: "POST",
                    data: Data.serialize(),
                    success: function (result) {
                        if (forceLogOut(result)) {
                            if (parseInt(result) == 1) {
                                $("#newDesignation").modal("hide");
                                Alert(_msgSave, "Success");
                                ShowDesignation();
                            }
                            else {
                                if (result.responseType == 4) {
                                    $('#newDesignation').find('#txtDegName').val("");
                                    $('#newDesignation').find('#existDegMsg').text(_msgExists);
                                    $('#newDesignation').find('#existDegMsg').removeClass("hidden");

                                } else {
                                    Alert(_msgError, "Error");
                                    return false;
                                }
                            }
                        }
                    }
                });

        }

    }


    
}
function ShowDesignation() {
    $.ajax(
        {
            url: "/SecurityControls/getDesignation/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#designation").html(result);
                    $("#profiles").css("display", "none");
                    $("#designation").show();
                }
            }
        });
}

function designationExpandCall() {
    if ($(this).hasClass("expand-role")) {
        $(".toggle_container").slideDown("slow");
    }
    else {
        $(".toggle_container").slideUp("slow");
    }
}

function resetDesignations() {
    var divs = $(".my-role");
    var designationIds = "";
    $(divs).each(function (i, item) {
        designationIds += $(item).attr('desig') + ',';
    });
    $.ajax(
        {
            url: "/SecurityControls/UpdateDesignationTree/",
            method: "GET",
            data: { designationIds: designationIds },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#updateDesignation").modal("hide");
                    ShowDesignation();
                }
            }
        });
}

function EditDesignation(id) {
    event.stopPropagation();
    $("#updateDesignation").modal("hide");
    var get = $.get("/SecurityControls/getDesignationByID?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#newProfile").html(result);
            $("#newProfile").modal("show");
        }
    })
}

function UpdateDesignationInfo() {
    if ($('#hdnDesignationNameold').val().toLowerCase() != $("#txtDegName").val().toLowerCase()) {
        checkDesignationName();
    }
        var Data = $("#DesignationUpdateForm");
        if (Data.valid()) {
            var Url = '/SecurityControls/UpdateDesignationInfo';
            var posting = $.post(Url, Data.serialize());
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    $("#newProfile").modal("hide");
                    ShowDesignation();
                    Alert(_msgSave, "Success");
                    scrollToTop();
                }
            })
        }
}

$('input#QueryDeginationSearch').keyup(function (event) {
    var x = event || window.event;
    var ival = this.value;
    if (ival.includes('#') || ival.includes('+') || x.keyCode == 35) {
        var ilen = ival.length;
        ival = ival.slice(0, ilen - 1);
        $('#QueryDeginationSearch').val(ival);
        event.preventDefault();
        return false;
    }
    $("#searchImg").hide();
    $("#cancelImg").show();
    var inputValue = this.value;
    if (inputValue) {
        $('.user-search').addClass('clear-toggle')
    } else {
        $('.user-search').removeClass('clear-toggle')
    }
    SearchDesignationUsers($('#searchDesignationID').val(), $('#QueryDeginationSearch').val());
});

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
    $("#cancelImg").show();
    var inputValue = this.value;
    if (inputValue) {
        $('.user-search').addClass('clear-toggle')
    } else {
        $('.user-search').removeClass('clear-toggle')
    }
    SearchProfileUsers($('#searchProfileID').val(), $('#QuerySearch').val());
});

function fnOnClickCancelBtn() {
    if ($('#QuerySearch').val() != "") {
        $('#QuerySearch').val("");
        $("#cancelImg").hide();
        $("#searchImg").show();
        var id = $("#searchProfileID").val();
        SearchProfileUsers(id, "");
    }
    else {
        $('#QueryDeginationSearch').val("");
        $(".cancelImg").hide();
        $(".searchImg").show();
        var id = $("#searchDesignationID").val();
        SearchDesignationUsers(id, "");
    }
}

function fnSelectDesignation(e) {
    event.stopPropagation();
    var IsSelected = $('#hfIsSelected').val();
    if (parseInt(IsSelected) == 0) {
        $('.accordion-toggle').addClass('marked');
        $(e).parent().parent().removeClass('marked');
        $(e).parent().parent().addClass('selected');
        $('#info-msg-designation').removeClass('hide');
        $('#hfIsSelected').val(1);
        $('#hfChildNode').val($(e).closest('div[class^="accordion-toggle"]').attr('data-id'));
        $(e).addClass('hide');
    }
    if (parseInt(IsSelected) == 1) {
        $('#hfParentNode').val($(e).closest('div[class^="accordion-toggle"]').attr('data-id'));
        if ($('#hfChildNode').val() != '' && $('#hfParentNode').val() != '') {
            fnUpdateDesignationHierarchy($('#hfChildNode').val(), $('#hfParentNode').val());
        }
    }
    fnHandleSiblings($(e).closest('div[class^="accordion-toggle"]').attr('data-id'));
}

function handleClearSelectionDesignation() {
    $('.accordion-toggle').removeClass('marked');
    $('.accordion-toggle').removeClass('selected');
    $('#info-msg-designation').addClass('hide');
    $('#hfIsSelected').val(0);
    $('#hfChildNode').val('');
    $('#hfParentNode').val('');
    $('.accordion-toggle').on("click");
    $('.btnSwap').removeClass('hide');
}

function InitAccordionToggle() {
    $('.accordion-toggle').click(function () {
        $(this).next('.accordion-inner').slideToggle();
        $(this).toggleClass('open');
        $(this).next('.accordion-inner').parent('li').toggleClass('active');
        $(this).parent('ul').removeClass('last');
    });
    $('.collapse-role').click(function () {
        $('.accordion-inner').slideUp();
        $('.accordion-inner').parent('li').removeClass('active');
        $('.accordion-toggle').removeClass('open');
    });
    $('.expand-role').click(function () {
        $('.accordion-inner').slideDown();
        $('.accordion-inner').parent('li').addClass('active');
        $('.accordion-toggle').addClass('open');
    });
}

function fnUpdateDesignationHierarchy(childId, parentId) {
    var Data = { DesignationId: childId, ParentDesignationId: parentId },
        Url = "/SecurityControls/UpdateDesignationHierarchy/";
    var posting = $.post(Url, Data);
    posting.done(function (result) {
        if (forceLogOut(result)) {
            if (parseInt(result) > 0) {
                Alert(_msgUpdate, "Success");
                ShowDesignation();
            }
            else {
                Alert(_msgError, "Error");
                handleClearSelectionDesignation();
            }
            scrollToTop();
        }
    })
}

function DeleteDesignation(id) {
    event.stopPropagation();
    $("#deleteProfileID").val("Designation_" + id);
    if (id != null) {
       // $("#headingMessage").html("Are you sure you want to delete this Designation?");
        $("#deleteModal h3").html("Are you sure you want to delete this Designation?");
        $("#deleteModal").modal("show");
    }
}

function fnOnHoverShow(e) {
    $(e).addClass("onhoverborder");
}

function fnOnHoverHide(e) {
    $(e).removeClass("onhoverborder");
}

function fnExpandAccordian() {
    $('.accordion-inner').slideDown();
    $('.accordion-inner').parent('li').addClass('active');
    $('.accordion-toggle').addClass('open');
}

function fnHandleSiblings(itemId) {
    $($('div[data-id=' + itemId + ']').siblings()).find('.accordion-toggle > .role-edit-options > .btnSwap').addClass('hide');
    $($('div[data-id=' + itemId + ']').siblings()).parent().find('.accordion-toggle').removeClass('marked');
}
function checkDesignationName() {
    var isDesignationValid = false;
    var designationName = $("#txtDegName").val();
    //var oldDesignationName = $("#txtDegName").attr("data-name");
    if ($(".btnDesignation").text() == "Update") {
        var oldDesignationName = $("#hdnDesignationNameold").val().trim().toLowerCase();
    } else {
        var oldDesignationName = $("#hdnDesignationNameold").val();
    }

   

    if (oldDesignationName != undefined && oldDesignationName === designationName.trim().toLowerCase()) {
        return false;
    }
    $.ajax(
        {
            url: "/SecurityControls/GetDesignationName/",
            method: "GET",
            async: false,
            data: { "designationName": designationName },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    if (result == "Success") {
                        $("#txtDegName").val(designationName);
                        $("#existDegMsg").text("");
                        $("#existDegMsg").addClass("hidden");
                        isDesignationValid = true;
                        return isDesignationValid;
                    }
                    else {
                        $("#txtDegName").val("");
                        $("#existDegMsg").text(result);
                        $("#existDegMsg").removeClass("hidden");
                        isDesignationValid = false;
                        return isDesignationValid;
                    }

                }
            }
        });
}


//Designation USERS GET METHOD
function DesignationUsers(id) {
    event.stopPropagation();
    $("#QueryDeginationSearch").val("");
    var get = $.get("/SecurityControls/DesignationUsers?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#designationUserList").html(result);
            $("#viewuserDesignation #searchDesignationID").attr('value', id);
            $("#viewuserDesignation #QuerySearch").val("");
            $("#viewuserDesignation").modal("show");
            $(".cancelImg").hide();
        }
    })
}

//SEARCH PROFILE USERS BY THE DATA
function SearchDesignationUsers(a, b) {
    if ($("#QueryDeginationSearch").val() == "") {
        $(".cancelImg").hide();
        $(".searchImg").show();
    } else {
        $(".searchImg").hide();
        $(".cancelImg").show();
    }
    $("#designationUserList").html("");
    ShowPartialLoader();
    _searchDesignationID = a;
    _QuerySearch = b;
    var get = $.get("/SecurityControls/DesignationUsers?id=" + _searchDesignationID + "&QuerySearch=" + _QuerySearch);
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();
            $("#viewuserDesignation .partialLoader").addClass("hide");
            $("#designationUserList").html(result);
        }
    })
}


function fnTriggerProduct() {
    $('#_ProductList li').first().find('a').trigger('click');
}


//----------------------------Tab Data Sharing--------------------------//



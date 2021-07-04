$(document).ready(function () {
    var accessDenied = $('#GroupAccessDenied').val();
    if (accessDenied != '-1')
    {
    init();
    }


})
function init() {
    fnTriggerGroupList();
    fnGroupSourceChange();
    $.get('/Group/GroupList', { QuerySearch: null, GetActiveGroups: 1 }, function (data) {      
            if (forceLogOut(data)) {
                $("#_GroupList").html(data);
                if ($("#_GroupList").text().trim() == "Record not found.") {
                    $("#group-info").html("<p class='text-center mt-5 textPara'>No Record Exist</p>");
                }
                fnTriggerGroupList();
            }
        });
}

//Local Variables
var selectedArray = [];

function getSelectedItems() {
    var items = [];
    $('.selectedItems').each(function () {
        items.push($(this).attr('data-Id'));
    });
    return items;
}
function getSelectedUsers() {
    var items = [];
    $('.selectedItems[data-source="Users"]').each(function () {
        items.push($(this).attr('data-Id'));
    });
    return items;
}
function getSelectedRoles() {
    var items = [];
    $('.selectedItems[data-source="Designation"]').each(function () {
        items.push($(this).attr('data-Id'));
    });
    return items;
}
function getSelectedGroups() {
    var items = [];
    $('.selectedItems[data-source="Groups"]').each(function () {
        items.push($(this).attr('data-Id'));
    });
    return items;
}
function GetGroupSourceData(sourceType, gsource) {
    $('#Available').empty();
    var btnValue = $("#submitGroup").text();
    var hdngrpId = $("#hdnGroupId").val();
    var clsName = "";
    if (btnValue == "Update") {
        clsName = " editList";
    }
    var get = $.get("/Group/GetGroupSourceData?source=" + sourceType);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $('#Available').append('<ul class="List' + gsource + ' availList demo-y"></ul>');
            $.each(result, function (i, item) {
                var name = (gsource == "Users") ? item.userName : item.name;
                $('.List' + gsource).append('<li class="pointer word_break' + clsName + '" onclick="onclickList(this)" data-Id="' + item.id + '">' + name + '</li>');
            });
            GetSelectData();
            $('.ListGroups li[data-id="' + hdngrpId + '"]').remove();
        }
    });
}
function GetSelectData() {
    $('.editList,.ListUsers li,.ListDesignation li,.ListRoles li').each(function () {
        var allSelected = getSelectedItems();
        var thisSelected = $(this).attr('data-Id');
        for (var i = 0; i <= allSelected.length; i++) {
            if (thisSelected == allSelected[i]) {
                $(this).addClass("activeItem");
            }
        }
    });
}
//GET USER DETAILS BY ID
function GroupDetails(id) {

    $.ajax(
        {
            url: "/Group/GroupDetails/",
            method: "GET",
            data: { id: id },
            success: function (result) {
                if (forceLogOut(result)) {
                    $("#group-info").html(result);
                }
            }
        });
}
// get group details for the filter used in the group tab
function GroupList() {
    if ($("#GroupQuerySearch").val() == "") {
        $("#closeImg").hide();
        $("#searchImg").show();
    }
    $("#_GroupList").html('');
    ShowPartialLoader();
    var _GetActiveGroups = $('#getActiveGroups').val();
    var _QuerySearch = $('#GroupQuerySearch').val();
    var get = $.get("/Group/GroupList?QuerySearch=" + _QuerySearch + "&GetActiveGroups=" + _GetActiveGroups);
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();
            if (result == "400") {
                Alert(_msgError, "Error");
                return false;
            }
            $("#_GroupList").html(result);
            var partialTabInfo = $("#_GroupList").text().trim();
            if (partialTabInfo == "Record not found.") {
                $("#group-info").html("<p class='text-center mt-5 textPara'>No Record Exist</p>");
                fnUpdateActiveFilterGrpCounts();
            }
            else {
                fnUpdateActiveFilterGrpCounts();
                fnTriggerGroupList();
            }
        }
    })
}
//To Get Group Details
function Groupsinfo() {
    $.ajax(
        {
            url: "/Group/GroupInformation/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    $("#_GroupList").html(result);
                    fnTriggerGroupList();
                }
            }
        });
}
//FOR EDIT Group MODAL
function EditGroup(id) {

    $.ajax(
        {
            url: "/Group/EditGroup/",
            method: "GET",
            data: { id: id },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#newGroup").html(result);
                    $("#newGroup").modal("show");
                }
            }
            ,
            error: function (e) {
            }
        });
}

function saveGroup() {
    if (checkGroupName())
        return false;

    var btnValue = $("#submitGroup").text();
   // var hdngrpId = $("#hdnGroupId").val();
    var getImageName = $("#hdnImageName").val();
    event.preventDefault();
    var formData = $('#GroupForm');
    if (btnValue != "Update") {
        if ($("#GroupSource").val() == null || $("#GroupSource").val() == "Select Group Source") {
            $("#groupSourcemsg").css({ "display": "block" });
            return false;
        } 
        else {
            $("#groupSourcemsg").css({ "display": "none" });
        }
        if (formData.valid()) {
            $.ajax({
                url: "/Group/CreateGroup",
                method: "POST",
                data: { Name: $('#groupName').val(), Description: $('#Description').val(), UserIds: getSelectedUsers(), RoleIds: getSelectedRoles(), GroupMappingIds: getSelectedGroups() },
                success: function (result) {
                    if (forceLogOut(result)) {
                        if (result == "400") {
                            Alert(_msgError, "Error");

                            return false;
                        }

                        $('#newGroup').modal("hide");
                        if (parseInt(result) >= 0) {
                            if (parseInt(result) == 0) {
                                Alert(_msgSave, 'Success');
                                $('.modal-backdrop').remove();
                                GroupList();
                                $('#_GroupList').closest('li').find('input').trigger();

                            }
                            if (parseInt(result) == 4) {
                                Alert(_msgExists, 'Warning');
                            }
                        }
                        else
                            Alert(_msgError, 'Error');

                    }
                }
            });
        }
    }
    else {
        event.preventDefault();

        $('#GroupSource').rules('add', {
            required: false   // overwrite an existing rule
        });

        var Data = new FormData();
        Data = { Id: $('#hdnGroupId').val(), Name: $('#groupName').val(), Description: $('#Description').val(), IsActive: $("#IsActive").is(':checked'), UserIds: getSelectedUsers(), RoleIds: getSelectedRoles(), GroupMappingIds: getSelectedGroups(), ImageName: getImageName };
        var Url = '/Group/UpdateGroup';

        //Return Page Type
        var returnPageType = $('#hfReturngrpPageType').val();

        if (formData.valid()) {
            var posting = $.post(Url, Data);
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $('[aria-label="Close"]').trigger('click');
                    if (parseInt(result) >= 0) {
                        Alert(_msgUpdate, 'Success');
                        //Check Page Type
                        if (returnPageType == 'groupdtail') {
                            GroupList();
                        }
                        else {
                            UserList();
                        }

                        if (parseInt(result) == 4) {
                            Alert(_msgExists, 'Warning');
                        }
                    }
                    else
                        Alert(_msgError, 'Error');
                }
                })
        }
    }






}
function deleteListItem(e) {
    var listItem = $(e).parent().attr("data-id");
    $(".selectedList").find("li[data-id='" + listItem + "']").remove();
    $("#Available").find("li[data-id='" + listItem + "']").removeClass('activeItem');
}

var fnTriggerGroupList = function () {

    $('#_GroupList li').find('input').first().trigger('click');
}
var fnCloseGroupModal = function () {
    $('#groupprofileModal [aria-label="Close"]').trigger('click');
}
function onclickList(e) {
    var gsourcetype = $('#GroupSource').val();
    var dataKeyword = '';
    var $list = $(e);
    var selectedItems = getSelectedItems();
    var ids = $list.attr('data-Id');
    var name = $list.text();
    for (var i = 0; i <= selectedItems.length; i++) {
        if (ids == selectedItems[i]) {
            Alert('You have already selected this option previously - please choose another.', 'Warning');
            return false;
        }
    }
    $list.addClass('activeItem');
    if (gsourcetype == "Users") {
        dataKeyword = 'U: ';
    }
    if (gsourcetype == "Designation") {
        dataKeyword = 'D: ';
    }
    if (gsourcetype == "Groups") {
        dataKeyword = 'G: ';
    }
    $('.selectedList ul').append('<li class="selectedItems word_break" data-source="' + gsourcetype + '" data-Id="' + ids + '"onmouseover="fnOnHoverShow_Cancel(this)" onmouseout = "fnOnHoverHide_Cancel(this)"' + '">' + dataKeyword + name + '<span class="float-right hide pointer h5" onclick="deleteListItem(this)">x</span></li>');
    $('.selectedList').addClass('demo-y');
    $(".demo-y").mCustomScrollbar({
        theme: "minimal-dark"
    });
}
function fnOnHoverShow_Cancel(e) {
    $(e).find("span").removeClass("hide");
    $(e).addClass("onhoverGroupUsers");
}
function fnOnHoverHide_Cancel(e) {
    $(e).find("span").addClass("hide");
    $(e).removeClass("onhoverGroupUsers");
}
function fnUpdateActiveFilterGrpCounts() {
    var allGroups = $("#hfAllGrpCount").val();
    var activeGroups = $("#hfGrpActiveCount").val();
    var inActiveGroups = $("#hfGrpInActiveCount").val();
    $('li[data-value="0"]').html("All Groups(" + allGroups + ")");
    $('li[data-value="1"]').html("Active Groups(" + activeGroups + ")");
    $('li[data-value="2"]').html("InActive Groups(" + inActiveGroups + ")");
    $('span[class="current"]').html($('li[data-value="' + $('#getActiveGroups').val() + '"]').html().trim());

}
$('input#GroupQuerySearch').keyup(function (event) {
    var x = event || window.event;
    var ival = this.value;
    if (ival.includes('#') || ival.includes('+') || x.keyCode == 35) {
        var ilen = ival.length;
        ival = ival.slice(0, ilen - 1);
        $('#GroupQuerySearch').val(ival);
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
    GroupList();
});
function fnOnClickCancelBtn() {

    if ($('#GroupQuerySearch').val() != "") {
        $('#GroupQuerySearch').val("");
        $("#closeImg").hide();
        $("#searchImg").show();
        GroupList();
    }
}
function fnGroupSourceChange() {
    $("#groupSourcemsg").css({ "display": "none" });
    var gsource = $('#GroupSource').val();
    if (gsource == 'Users') {
        GetGroupSourceData(1, gsource);
    }
    if (gsource == 'Designation') {
        GetGroupSourceData(2, gsource);
    }
    if (gsource == 'Groups') {
        GetGroupSourceData(3, gsource);
    }

}
function onChangeList() {
    var gsourcetype = $('#GroupSource').val();
    var gsource = $('.List' + gsourcetype + ' :selected').text();
    var selected = $(".List" + gsourcetype + " :selected").map((_, e) => e.text).get();

    for (var z = 0; z < selected.length; z++) {
        if ($.inArray(selected[z], selectedArray) >= 0) {
            Alert('You have already selected this option previously - please choose another.', 'Warning');
        }
        else {
            selectedArray.push(selected[z]);
            if (gsourcetype == "Users") {
                $("#initialBlock").val($("#initialBlock").val() + "U: " + selected[z] + "\n");
            }
            if (gsourcetype == "Designation") {
                $("#initialBlock").val($("#initialBlock").val() + "D: " + selected[z] + "\n");
            }
            if (gsourcetype == "Groups") {
                $("#initialBlock").val($("#initialBlock").val() + "G: " + selected[z] + "\n");
            }
        }
    }
    var gsourcearray = [gsource];
}

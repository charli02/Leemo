$(document).ready(function () {
    var accessDenied = $('#LocationAccessDenied').val();
    if (accessDenied != '-1') {
        init();
    }
})
$(document).on('hidden.bs.modal', function () {
    if ($('.modal.show').length) {
        $('body').addClass('modal-open');
    }
});
function init() {
    fnTriggerLocationList();
    $.get('/Location/LocationList', { QuerySearch: null, GetLocationGroups: 1 }, function (data) {
        if (forceLogOut(data)) {
            $("#_LocationList").html(data);
            fnTriggerLocationList();
        }
    })
}

var fnTriggerLocationList = function () {
    $('#_LocationList li').find('input').first().trigger('click');
}

//FOR CREATE USER
function CreateLocation() {
    $.ajax(
        {
            url: "/Location/CreateLocation/",
            method: "GET",
            success: function (result) {
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
            }
        });
}

// Search Filter On Location
$('input#LocationQuerySearch').keyup(function (event) {
    var x = event || window.event;
    var ival = this.value;
    if (ival.includes('#') || ival.includes('+') || x.keyCode == 35) {
        var ilen = ival.length;
        ival = ival.slice(0, ilen - 1);
        $('#LocationQuerySearch').val(ival);
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
    GetLocationList();
});
//Location search button Click Event
function fnOnClickLocationCancelBtn() {

    if ($('#LocationQuerySearch').val() != "") {
        $('#LocationQuerySearch').val("");
        $("#closeImg").hide();
        $("#searchImg").show();
        GetLocationList();
    }
}

// Save Location Information
//function saveLocation(e) {
//   event.preventDefault();
//    var Data = $('#createLocationForm'),
//        Url = '/Location/CreateLocation';
//    if (Data.valid()) {
//        fnButtonLoader(e);
//        var posting = $.post(Url, Data.serialize());
//        posting.done(function (result) {
//            $('#newLocation').modal("hide");
//            Alert(_msgSave, 'Success');
//            GetLocationList();
//        })
//    }
//}


// Update Location
function updateLocation(id) {
    event.preventDefault();
    $('#txtLocationName').val($('#txtLocationName').val().trim());
    if (!checkLocationName()) {
        var Data = $('#updateLocationForm'),
            Url = '/Location/UpdateLocation?id=' + id;
        if (Data.valid()) {
            //fnButtonLoader(e);
            var posting = $.post(Url, Data.serialize());
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    if (result == 0) {
                        $('#newLocation').modal("hide");
                        Alert(_msgSave, 'Success');
                        $('#confirmModal').modal("hide");
                        if ($('#isConfirmed').val() == "1") {
                            window.location.href = '/Account/Logout';
                            return false;
                        }
                        GetLocationList();
                        fnCountLocationByUser();
                    }
                    else
                        Alert(_msgError, 'Error');
                }
            })
        }

    }
   

}

//Get All Locations
function GetLocationList() {
    if ($("#LocationQuerySearch").val() == "") {
        $("#closeImg").hide();
        $("#searchImg").show();
    }
    $("#_LocationList").html('');
        ShowPartialLoader();   
    var _GetActiveLocations = $('#GetActiveLocations').val();
    var _QuerySearch = $('#LocationQuerySearch').val();
    var get = $.get("/Location/LocationList?QuerySearch=" + _QuerySearch + "&GetActiveLocations=" + _GetActiveLocations);
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();

            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }
            $("#_LocationList").html(result);

            var getLocationPartialInfo = $("#_LocationList").text().trim();
            if (getLocationPartialInfo == "Record not found.") {
                $("#location-info").html("<p class= 'text-center mt-5 textPara'> No Record Exist</p>");
                fnUpdateActiveFilterCounts();
            }
            else {
                fnUpdateActiveFilterCounts();
                fnTriggerLocationList();

            }
        }
    })
}




function fnUpdateActiveFilterCounts() {
    
    var allLocations = $("#hfAllLocationsCount").val();
    var activeLocations=$("#hfActiveCount").val();
    var inActiveLocations = $("#hfInActiveCount").val();


    $('li[data-value="0"]').html("All Locations(" + allLocations + ")");
    $('li[data-value="1"]').html("Active Locations(" + activeLocations + ")");
    $('li[data-value="2"]').html("InActive Locations(" + inActiveLocations + ")");
    $('span[class="current"]').html($('li[data-value="' + $('#GetActiveLocations').val()+'"]').html().trim());
}
function LocationDetails(id) {
    var get = $.get("/Location/GetLocationById?id="+id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#location-info").html(result);
        } 
    })
}

function EditLocation(id) {
    var get = $.get('/Location/EditLocation?id=' + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            $("#newLocation").html(result);
            $("#newLocation").modal("show");
        }
    })
}

function fnisheadAlert() {
    Alert('Location of Head Office cannot be In-Active!');
}

function CheckCurrentLocation(id) {
    if (checkLocationName()) {
        return false;
    }
    var Data = $('#updateLocationForm');    
            $('#isConfirmed').val("0");
            var currentLocationId = $('#currentLocation').val();
    if (id == currentLocationId) {
        if (Data.valid()) {
            $("#newLocation").modal("hide");
            $('#isConfirmed').val("1");
            $('#confirmModal').modal("show");            
            $('#proceedButton').attr("onclick", "updateLocation('" + id + "')");
        }
    }
    else
    {
        updateLocation(id);
    }
}

function checkLocationName() {
    var isCheckLocation = false;
    var locationName = $("#txtLocationName").val();
    if ($(".btnLocation").text() == "Update") {
        var oldLocationName = $("#hdnLocationName").val().trim().toLowerCase();
    }
    else {
        var oldLocationName = $("#hdnLocationName").val();
    }
    if (oldLocationName != undefined && oldLocationName === locationName.trim().toLowerCase()) {
        return;
    }
    //if (locationName == "") {
    //    return isCheckLocation;
    //}

    $.ajax(
        {
            url: "/Location/GetLocationName/",
            method: "GET",
            async: false,
            data: { "locationName": locationName },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "Success") {
                        $("#txtLocationName").val(locationName);
                        $("#existLocationMsg").text("");
                        $("#existLocationMsg").addClass("hidden");
                        isCheckLocation = true;
                        return isCheckLocation;
                    }
                    else {
                        $("#txtLocationName").val("");
                        $("#existLocationMsg").text(result);
                        $("#existLocationMsg").removeClass("hidden");
                        isCheckLocation = false;
                        return isCheckLocation;
                    }
                }
            }
        });
}

function fnCountLocationByUser() {
    var get = $.get("/Location/CountCompanyLocationByUser");
    get.success(function (result) {
        if (forceLogOut(result)) {
            if (result == true) {
                $(".layoutSwitchLocation").removeClass("hide");
            }
            else {
                $(".layoutSwitchLocation").addClass("hide");
            }
        }
    })
}

function setAlertofActivation() {
    debugger;
    var value1 = $('#inputLocation_IsActive').prop('checked');
    if (value1 == true) {
        $("#warningDeActivateMessage").show();
    }
    else {
        $("#warningDeActivateMessage").hide();
    }

}

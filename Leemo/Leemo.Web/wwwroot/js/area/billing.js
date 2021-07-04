$(document).ready(function () {
    GetBillingList();
   // init();
})
//function init() {

//    //fnTriggerGroupList();
//    //fnGroupSourceChange();
//    $.get('/Billing/GetBillingAddresses', function (data) {
//       // if (forceLogOut(data)) {
//        $("#SecurityProfileResult").html(data);
//           // fnTriggerGroupList();
//        //}
//    })

//}

//function showBillingTab() {
//    $.ajax({
//        url: '/Billing/GetBillingAddresses',
//    });
//}


function CreateBillingAddress() {
    $.ajax(
        {
            url: "/Billing/CreateBillingAddress/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    $("#newBillingAddress").html(result);
                    $("#newBillingAddress").modal("show");
                }
            }
        });
}

function EditBillingAddress(id) {
    if (id != null) {
        $.ajax(
            {
                url: "/Billing/EditBillingAddress/",
                method: "GET",
                data: {
                    Id: id
                },
                success: function (result) {
                    if (forceLogOut(result)) {
                        if (parseInt(result) <= 0) {
                            if (parseInt(result) < 0) {
                                Alert(_msgError, "Error");
                                return false;
                            }
                        }
                        $("#newBillingAddress").html(result);
                        $("#newBillingAddress").modal("show");
                    }

                }
            });
    }
   
}

function DeleteBillingAddress(id) {
        $("#deleteProfileID").val("BillingAddress_" + id);
        if (id != null) {
            $("#deleteModal h3").html("Are you sure you want to delete this BillingAddress ?");
            $("#deleteModal").modal("show");
        }
}

function SaveBillingAddress(e) {
    var Data = $('#createBillingAddressForm'),
        Url = '/Billing/CreateBillingAddress';
    if (Data.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                $('#newBillingAddress').modal("hide");
                Alert(_msgSave, 'Success');
                GetBillingList();
           }
        })
    }
}

function GetBillingList() {
    var get = $.get("/Billing/GetBillingAddresses");
    get.success(function (result) {
        if (forceLogOut(result)) {
            HidePartialLoader();

            if (parseInt(result) <= 0) {
                if (parseInt(result) < 0) {
                    Alert(_msgError, "Error");
                    return false;
                }
            }
            $("#SecurityProfileResult").html(result);
        }
    })
}

function UpdateBillingAddress(id) {
    if (id != null) {
        var Data = $('#editBillingAddressForm'),
            Url = '/Billing/EditBillingAddress?Id=' + id;
        if (Data.valid()) {
            var posting = $.post(Url, Data.serialize());
            posting.done(function (result) {
                if (forceLogOut(result)) {
                    $('#newBillingAddress').modal("hide");
                    Alert(_msgSave, 'Success');
                    GetBillingList();
                }
            })
        }
    }
}
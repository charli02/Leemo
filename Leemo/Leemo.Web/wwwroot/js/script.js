//$(".demo-y").mCustomScrollbar();
$(document).ready(function () {
    $('.full-menu').click(function (event) {
        $('.main-row').toggleClass('full-nav');
    });
    (function () {
        'use strict';
        window.addEventListener('load', function () {
            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            var forms = document.getElementsByClassName('needs-validation');
            // Loop over them and prevent submission
            var validation = Array.prototype.filter.call(forms, function (form) {
                form.addEventListener('submit', function (event) {
                    if (form.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    form.classList.add('was-validated');
                }, false);
            });
        }, false);
    })();
    /////////////////////////////////
    $(window).load(function () {
        $(".loader").fadeOut("slow");;
    });

    //$('.toast').toast('show')

    // scrollbar js
    $(window).on("load", function () {
        //$.mCustomScrollbar.defaults.theme="dark-2";
        $(".demo-y").mCustomScrollbar({
            theme: "minimal-dark"
        });
    });

    $("#content-6").mCustomScrollbar({
        axis: "x",
        theme: "minimal-dark",
        advanced: { autoExpandHorizontalScroll: true }
    });

    // custom select js
    $('.niceSelect').niceSelect();

    // datepicker js
    $('#datepicker').datepicker({
        uiLibrary: 'bootstrap4',
        format: 'dd mmm yyyy'
    });

    // timepicker js
    $('#timepicker').timepicker();

    //////////////////////////////////
    $('.install-plugin-action').fadeOut();
    $('#installed-tab').click(function () {
        $('body').find('.install-plugin-action').show();
    });
    $('#all-plugin-tab').click(function () {
        $('body').find('.install-plugin-action').hide();
    });

    ///////////////////////////////////
    $('.goto-permission').click(function () {
        $('.profile-permissions').fadeIn();
    });
    $('.back-to-profile').click(function () {
        $('.profile-permissions').fadeOut();
    });
    /////////////////////////////////////
    //$('.accordion-toggle').click(function () {
    //    $(this).next('.accordion-inner').slideToggle();
    //    $(this).toggleClass('open');
    //});
    //$('.accordion-toggle').click(function () {
    //    $(this).next('.accordion-inner').parent('li').toggleClass('active');
    //});
    //$('.accordion-toggle').click(function () {
    //    $(this).parent('ul').removeClass('last');
    //});


    ////////////////////////////////////////////
    //$('.collapse-role').click(function () {
    //    $('.accordion-inner').slideUp();
    //    $('.accordion-inner').parent('li').removeClass('active');
    //    $('.accordion-toggle').removeClass('open');
    //});
    ////////////////////////////////////////////
    //$('.expand-role').click(function () {
    //    $('.accordion-inner').slideDown();
    //    $('.accordion-inner').parent('li').addClass('active');
    //    $('.accordion-toggle').addClass('open');
    //});
    /////////////////////////////////////////////
    $('.site-tgl').click(function () {
        $(this).toggleClass('activeTgl');
        $('body').addClass('show-nav');
        $('.tgl-overlay').fadeIn();
    });
    $('.close-tgl, .tgl-overlay, .menu-list .listing ul li a').click(function () {
        $('body').removeClass('show-nav');
        $('.site-tgl').removeClass('activeTgl');
        $('.tgl-overlay').fadeOut();
    });
    /////////////////////////////////////////////
    $('.inner-search-icon').click(function () {
        $('body').addClass('searching')
        $(this).parents('.header-right').find('.form-control').focus();
    });
    $('.search-cls-icon').click(function () {
        $('body').removeClass('searching')
    });
    ////////////////////////////////////////////
    var dragSrcEl = null;

    function handleDragStart(e) {

        // Target (this) element is the source node.
        dragSrcEl = this;

        e.dataTransfer.effectAllowed = 'move';
        e.dataTransfer.setData('text/html', this.outerHTML);

        this.classList.add('dragElem');
    }
    function handleDragOver(e) {
        if (e.preventDefault) {
            e.preventDefault(); // Necessary. Allows us to drop.
        }
        this.classList.add('over');

        e.dataTransfer.dropEffect = 'move';  // See the section on the DataTransfer object.

        return false;
    }

    function handleDragEnter(e) {
        // this / e.target is the current hover target.
    }

    function handleDragLeave(e) {
        this.classList.remove('over');  // this / e.target is previous target element.
    }

    function handleDrop(e) {

        // this/e.target is current target element.

        if (e.stopPropagation) {
            e.stopPropagation(); // Stops some browsers from redirecting.
        }

        // Don't do anything if dropping the same column we're dragging.
        if (dragSrcEl != this) {
            // Set the source column's HTML to the HTML of the column we dropped on.
            //alert(this.outerHTML);
            //dragSrcEl.innerHTML = this.innerHTML;
            //this.innerHTML = e.dataTransfer.getData('text/html');
            this.parentNode.removeChild(dragSrcEl);
            var dropHTML = e.dataTransfer.getData('text/html');
            this.insertAdjacentHTML('beforebegin', dropHTML);
            var dropElem = this.previousSibling;
            addDnDHandlers(dropElem);

        }
        this.classList.remove('over');
        return false;
    }

    function handleDragEnd(e) {
        // this/e.target is the source node.
        this.classList.remove('over');

        /*[].forEach.call(cols, function (col) {
          col.classList.remove('over');
        });*/
    }

    function addDnDHandlers(elem) {

        elem.addEventListener('dragstart', handleDragStart, false);
        elem.addEventListener('dragenter', handleDragEnter, false)
        elem.addEventListener('dragover', handleDragOver, false);
        elem.addEventListener('dragleave', handleDragLeave, false);
        elem.addEventListener('drop', handleDrop, false);
        elem.addEventListener('dragend', handleDragEnd, false);

    }

    var cols = document.querySelectorAll('.role-list-content');
    [].forEach.call(cols, addDnDHandlers);

    ///////////////////////////////////////////////
    // see http://www.webdesignerdepot.com/2012/01/password-strength-verification-with-jquery/ for original

    var password = [];
    var password2 = [];


    $(document).ready(function () {
        password = $('#password');
        password2 = $('#password-verify');

        var ruleValidator = function () {
            var pswd = $(this).val();
            //gets what is being input in the field and sets as variable

            if (pswd.length < 8) {
                $('#length').removeClass('valid').addClass('invalid');
            } else {
                $('#length').removeClass('invalid').addClass('valid');
            }

            // letter
            if (pswd.match(/[a-z]/)) {
                $('#letter').removeClass('invalid').addClass('valid');
            } else {
                $('#letter').removeClass('valid').addClass('invalid');
            }

            // Capital
            if (pswd.match(/[A-Z]/)) {
                $('#capital').removeClass('invalid').addClass('valid');
            } else {
                $('#capital').removeClass('valid').addClass('invalid');

            }

            // number
            if (pswd.match(/\d/)) {
                $('#number').removeClass('invalid').addClass('valid');
            } else {
                $('#number').removeClass('valid').addClass('invalid');
            }

            // character
            if (pswd.match(/[$&+,:;=?@#|'<>.^*()%!-]/)) {
                $('#special').removeClass('invalid').addClass('valid');
            } else {
                $('#special').removeClass('valid').addClass('invalid');
            }
        }

        password.focus(function () {
            // (triggers whenever the password field is selected by the user)
            $('.pswd_info').slideDown();
        });

        password.keyup(ruleValidator);
    });

    /////////////////// TOOL TIP  /////////////////////////
    tippy('[data-tippy-content]');
});

//$(document).ajaxStart(function () {
//    // Show partial loader
//    $(".partialLoader").fadeIn("slow");
//});
//$(document).ajaxComplete(function () {
//    // Hide partial loader
//    $(".partialLoader").fadeOut("slow");
//});
$('.inputImageFile').on('change', function () {
    readURL(this);
    var iname = this.files[0].name;

    var ext = $($('#EditImageModalTarget').val()).find("#myfile").val().split('.').pop().toLowerCase();
    //$('#myfile').val().split('.').pop().toLowerCase();
    if ($.inArray(ext, ['png', 'jpg', 'jpeg']) == -1) {
        Alert(_msgExtImage, "Error");
        return false;
    }

    var MaxImageSize = $($('#EditImageModalTarget').val()).find('#hfMaxImageSize').val();

    var file_size = $($('#EditImageModalTarget').val()).find('#myfile')[0].files[0].size;
    if (file_size > MaxImageSize) {
        Alert(_msgSizeImage, "Error")
        return false;
    }


    $($('#EditImageModalTarget').val()).find('#selectedImgName').text(iname);

});

//});
//FOR EDIT PERSONAL INFORMATION MODAL
function EditPersonalDetails(id) {
    var returnPageType = $('#ResultUser_returnFrom').val();
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
                    $("#personalDetailEditModal").html(result);
                    $('#InputUser_returnFrom').val('PersonalSettings');
                    $("#personalDetailEditModal").modal("show");
                    var input = document.querySelector("#userProfile_Mobile");
                    var instance = window.intlTelInput(input, {
                        utilsScript: "~/lib/intlTelInput/js/utils.js",
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

//Creating New Group Pop-up
function NewGroupPopup() {
    //
    $.ajax(
        {
            url: "/Group/CreateGroup/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "400") {
                        Alert(_msgError, "Error");
                        return false;
                    }
                    $("#newGroup").html(result);
                    $("#groupSourcemsg").css({ "display": "none" });
                    $("#newGroup").modal("show");
                }
            }
        });
}

function checkEmail() {
    var getEmail = $("#userEmail").val();
    var isValidEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
    if (isValidEmail.test(getEmail)) {
        $.ajax(
            {
                url: "/User/GetUserEmail/",
                method: "GET",
                async: false,
                data: { "Email": getEmail },
                success: function (result) {
                    if (forceLogOut(result)) {
                        if (parseInt(result) == 1) {
                            //$("#userEmail").val("");
                            $("#existMsg").text(_msgUserExists);
                            $("#existMsg").removeClass("hidden");
                            $('#hfUserExist').val(1);
                        }
                        if (parseInt(result) == 0) {
                            $("#userEmail").val(getEmail);
                            $("#existMsg").text("");
                            $("#existMsg").addClass("hidden");
                            $('#hfUserExist').val(0);
                        }
                        //alert(result);
                        //$("#newGroup").html(result);
                        //$("#newGroup").modal("show");
                    }
                }
            });
    }
    else {
        $("#existMsg").text("The Email/Username field is not a valid e-mail address.");
        $("#existMsg").removeClass("hidden");
    }
    // alert(getEmail);
}

function checkGroupName() {
    var isCheckGroup = false;
    var groupName = $("#groupName").val();
    if ($(".btnGroup").text() == "Update") {
        var oldgroupName = $("#hdnGroupNameOld").val().trim().toLowerCase();
    }
    else {
        var oldgroupName = $("#hdnGroupNameOld").val();
    }



    //var oldgroupName = $("#groupName").attr("data-name");
    if (oldgroupName != undefined && oldgroupName === groupName.trim().toLowerCase()) {
        return isCheckGroup;
    }
    if (groupName == "") {
        return isCheckGroup;
    }

    $.ajax(
        {
            url: "/Group/GetGroupName/",
            method: "GET",
            async: false,
            data: { "groupName": groupName },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (result == "Success") {
                        $("#groupName").val(groupName);
                        $("#existgrpMsg").text("");
                        $("#existgrpMsg").addClass("hidden");
                        isCheckGroup = true;
                        return isCheckGroup;
                    }
                    else {
                        $("#groupName").val("");
                        $("#existgrpMsg").text(result);
                        $("#existgrpMsg").removeClass("hidden");
                        return isCheckGroup;
                    }

                }
            }
        });

    // return isCheckGroup;
    // alert(groupName);

}


function updateUser(e) {
    event.preventDefault();
    var validDOB = checkValidDOB();
    var validReportingToUser = checkValidReportingToUser();
   

    var Data = $('#editUserForm'),
        Url = '/User/EditUser';
    var _userId = $('#InputUser_Id').val();
    //get Country value
    var Country = $('.iti__selected-flag').attr('aria-activedescendant');
    //get Country Codenumber 
    var CountryCode = $('.iti__selected-flag').attr('title').toLowerCase();
    if ($("#selectUserProfile").val() == " -- select an option -- ") {
        $("#UserProfile").removeClass("hide");
       // $("#securityRole").vali
        return false;
    }
    //if (CountryCode == "unknown") {
    //    Alert(_CountryCode, 'Error');
    //    return false;
    //}
    //set countryCode
    Country = Country.trim().split('-');
    var CountryData = Country[2];
    $('#CountryCode').val(CountryData);


    //set Country codeNumber 
    CountryCode = CountryCode.trim().split('+');
    var CountryCodeNunber = CountryCode[1].trim();
    $('#CountryCodeNumber').val(CountryCodeNunber);
    //Return Page Type
    var returnPageType = $('#InputUser_returnFrom').val();

    if (Data.valid() && validDOB && validReportingToUser) {


        fnButtonLoader(e);


        var posting = $.post(Url, Data.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                fnRemoveBtnDisableData(e);
                if (parseInt(result) <= 0) {
                    if (parseInt(result) < 0) {
                        Alert(_msgError, "Error");
                        return false;
                    }
                }
            if (parseInt(result)== 3) {
                Alert(_ValidDateError, 'Error');
                return false;
            }
        

                $('#personalDetailEditModal').modal("hide");
                $('#userEditModal').modal("hide");
                $('.modal-backdrop').remove();
                if (parseInt(result) > 0) {
                    if (parseInt(result) == 2) {
                        Alert(_msgUpdate, 'Success');
                        //Check Page Type 
                        if (returnPageType == 'PersonalSettings') {
                            $('.name').text($('#InputUser_userProfile_FirstName').val() + " " + $('#InputUser_userProfile_LastName').val());
                            $('.name').attr('title', $('#InputUser_userProfile_FirstName').val() + " " + $('#InputUser_userProfile_LastName').val());
                            PersonalSettings();
                        }
                        else {
                            if (_userId == $('#LoggedInUserId').val()) {
                                $('.name').text($('#InputUser_userProfile_FirstName').val() + " " + $('#InputUser_userProfile_LastName').val());
                                $('.name').attr('title', $('#InputUser_userProfile_FirstName').val() + " " + $('#InputUser_userProfile_LastName').val());
                            }
                            UserList();
                        }



                    }
                    if (parseInt(result) == 4) {
                        Alert(_msgUserExists, 'Warning');
                    }
                }
                else
                    Alert(_msgError, 'Error');
            }
        })
    }
   
}

//CHECK FOR VALID DATE OF BIRTH
function checkValidDOB() {
    if (!($("#selectDOBDay").val() == null || $("#selectDOBDay").val() == "" || $("#selectDOBDay").val() == "-Day-") || !($("#selectDOBMonth").val() == null || $("#selectDOBMonth").val() == "" || $("#selectDOBMonth").val() == "-Month-") || !($("#selectDOBYear").val() == null || $("#selectDOBYear").val() == "" || $("#selectDOBYear").val() == "-Year-")) {
        if (!($("#selectDOBDay").val() == null || $("#selectDOBDay").val() == "" || $("#selectDOBDay").val() == "-Day-") && !($("#selectDOBMonth").val() == null || $("#selectDOBMonth").val() == "" || $("#selectDOBMonth").val() == "-Month-") && !($("#selectDOBYear").val() == null || $("#selectDOBYear").val() == "" || $("#selectDOBYear").val() == "-Year-")) {
            $("#alertdob").addClass("hide");
            var s = $("#selectDOBDay").val() + "/" + $("#selectDOBMonth").val() + "/" + $("#selectDOBYear").val();
            var bits = s.split('/');
            var d = new Date(bits[2] + '/' + bits[1] + '/' + bits[0]);
            return !!(d && (d.getMonth() + 1) == bits[1] && d.getDate() == Number(bits[0]));
            return true;
        }
        else {
            Alert(_ValidDateError, "Error");
            return false;
        }

    }
    else {
         // $("#alertdob").addClass("hide");
        $("#alertdob").removeClass("hide");
        //return true;
           return false;
    }
    $("#alertdob").removeClass("hide");
    return false;
}

//CHECK FOR VALID REPORTING TO
function checkValidReportingToUser() {
    if (!($("#selectUserRole").val() == null || $("#selectUserRole").val() == "")) {
        if ($("#selectRepotingUsers").val() == null || $("#selectRepotingUsers").val() == "" ) {
            $("#ReportingUser").removeClass("hide");
            return false;
        }
        else {
            $("#ReportingUser").addClass("hide");
            return true;
        }
    }
}

//To Get Personal Setting
function PersonalSettings() {

    $('#setting-tab').addClass('active');
    $('#PersonalChangePassword').removeClass('active');
    $.ajax(
        {
            url: "/User/DetailsPersonal/",
            method: "GET",
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    $("#UserDataPersonal").html(result);

                }
            }
        });
}

function editCompanyPopup(id) {
    var get = $.get("/Company/CompanyEditPopup?id=" + id);
    get.success(function (result) {
        if (forceLogOut(result)) {
            if (result == "400") {
                Alert(_msgError, "Error");
                return false;
            }
            $("#organization-modal").html(result);
            $("#organization-modal").modal("show");
            var input = document.querySelector("#userCompany_Mobile");
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
        }
    })
}

function updateCompany(e) {
    event.preventDefault();

    //get Country value
    var Country = $('.iti__selected-flag').attr('aria-activedescendant');
    //get Country Codenumber 
    var CountryCode = $('.iti__selected-flag').attr('title').toLowerCase();

    if (CountryCode == "unknown") {
        Alert(_CountryCode, 'Error');
        return false;
    }

    //set countryCode
    Country = Country.trim().split('-');
    var CountryData = Country[2];
    $('#CountryCode').val(CountryData);


    //set Country codeNumber 
    CountryCode = CountryCode.trim().split('+');
    var CountryCodeNunber = CountryCode[1].trim();
    $('#CountryCodeNumber').val(CountryCodeNunber);

    var formData = $('#updateCompany');
    var Url = '/Company/EditCompany';
    if (formData.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, formData.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                fnRemoveBtnDisableData(e);
                if (parseInt(result) <= 0) {
                    if (parseInt(result) < 0) {
                        Alert(_msgError, "Error");
                        return false;
                    }
                }
                Alert(_msgUpdate, 'Success');
                $("#organization-modal").modal("hide");
                getCompanyDetails();
            }
            });
    }
}

function getCompanyDetails() {
    var get = $.get("/Company/CompanyDetails");
    get.success(function (result) {
        if (forceLogOut(result)) {
            $('#renderCompanyDetails').html(result);
        }
    })
}



//FOR UPLOADING PERSONAL DETAIL PROFILE IMAGE
function uploadimage(e) {

    var ext = $($('#EditImageModalTarget').val()).find("#myfile-01").val().split('.').pop().toLowerCase();
    //$('#myfile').val().split('.').pop().toLowerCase();
    if (ext != "") {
        if ($.inArray(ext, ['png', 'jpg', 'jpeg']) == -1) {
            Alert(_msgExtImage, "Error");
            return false;
        }
    }
    else {
        Alert(_SelectImage, "Error");
        return false;
    }
    
   

    var MaxImageSize = $($('#EditImageModalTarget').val()).find('#hfMaxImageSize').val();

    var file_size = $($('#EditImageModalTarget').val()).find('#myfile-01')[0].files[0].size;
    if (file_size > MaxImageSize) {
        Alert(_msgSizeImage, "Error")
        return false;
    }

    //var dataform = new FormData(document.getElementById('uploadImageForm'));
    var dataform = new FormData($($('#EditImageModalTarget').val()).find('form')[0]);
    //var files = $('[type="file"]').get(0).files;
    var files = $($($('#EditImageModalTarget').val())[0]).find('[type="file"]').get(0).files;
    if (files.length == 0) {
        Alert(_msgErrorImage, 'Error');
        return false;
    }
    if (files.length > 0) {
        dataform.append("files[0].name", files[0]);
    }
    var target = dataform.get('EditImageModalTarget');
    var _url = '', _imgPath = '', _userId = '';
    switch (target) {
        case '#uploadPersonalImageModal':
            // Upload Profile Image Action
            _url = "/User/UploadProfileImage/";
            _imgPath = '/data/ProfileImages/';
            _userId = dataform.get('Id');
            break;
        case '#uploadUserImageModal':
            // Upload User Image Action
            _url = "/User/UploadProfileImage/";
            _imgPath = '/data/ProfileImages/';
            _userId = dataform.get('Id');
            break;
        case '#uploadCompanyImageModal':
            // Upload Company Image Action
            _url = "/Company/UploadCompanyImage/";
            _imgPath = '/data/CompanyImages/';
            break;
        case '#uploadGroupImageModal':
            // Upload Group Image Action
            _url = "/Group/UploadGroupImage/";
            _imgPath = '/data/GroupImages/';
            break;
        default:
            return;
    }

    fnButtonLoader(e);
    $.ajax(
        {
            url: _url,
            method: "POST",
            contentType: false,
            processData: false,
            data: dataform,
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    if (result == "0") {
                        Alert(_msgExtImage, "Error");
                        return false;
                    }
                    if (result == "1") {
                        Alert(_msgSizeImage, "Error")
                        return false;
                    }
                    Alert(_msgImage, "Success")
                    $(target).modal("hide");
                    $('.modal-backdrop').remove();
                    if (target == '#uploadUserImageModal') {
                        $('#changedUserImage').attr('src', _imgPath + result);
                        $('#displayImage').attr('src', _imgPath + result);
                        if (_userId == $('#LoggedInUserId').val()) {
                            $('#loggedUserImage').attr('src', _imgPath + result);
                        }
                        UserList();
                    }
                    if (target == '#uploadCompanyImageModal') {
                        $('#changedCompanyImage').attr('src', _imgPath + result);
                        $('#displayImage').attr('src', _imgPath + result);
                    }
                    if (target == '#uploadGroupImageModal') {
                        $('#changedGroupImage').attr('src', _imgPath + result);
                        $('#displayImage').attr('src', _imgPath + result);
                        GroupList();
                    }
                    if (target == '#uploadPersonalImageModal') {

                        $('#changedPersonalImage').attr('src', _imgPath + result);
                        $('#displayImage').attr('src', _imgPath + result);
                        $('#loggedUserImage').attr('src', _imgPath + result);
                    }
                    fnRemoveBtnDisable(e);
                }
            }
        });
}


function readURL(input) {

    //if (input.files && input.files[0]) {
    //    $('#selectedImage').attr('src', '');
    //    var reader = new FileReader();
    //    reader.onload = function (e) {
    //        $('#selectedImage')
    //            .attr('src', e.target.result)
    //    };
    //    reader.readAsDataURL(input.files[0]);
    //}


    if (input.files && input.files[0]) {
        //$('#selectedImage').attr('src', '');
        $($($('#EditImageModalTarget').val()).find('#selectedImage')[0]).attr('src', '');
        var reader = new FileReader();
        reader.onload = function (e) {
            //$('#selectedImage')
            //    .attr('src', e.target.result)
            $($($('#EditImageModalTarget').val()).find('#selectedImage')[0]).attr('src', e.target.result);
            $($('#EditImageModalTarget').val()).find('#selectedImgName').text(input.files[0].name);
        };
        reader.readAsDataURL(input.files[0]);

        var ext = $($('#EditImageModalTarget').val()).find("#myfile-01").val().split('.').pop().toLowerCase();
        //$('#myfile').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['png', 'jpg', 'jpeg']) == -1) {
            Alert(_msgExtImage, "Error");
            return false;
        }

        var MaxImageSize = $($('#EditImageModalTarget').val()).find('#hfMaxImageSize').val();

        var file_size = $($('#EditImageModalTarget').val()).find('#myfile-01')[0].files[0].size;
        if (file_size > MaxImageSize) {
            Alert(_msgSizeImage, "Error")
            return false;
        }
    }

}

function fnDisplayImage(e) {
    $('#' + e).modal("show");
    //$('#EditImageModalTarget').val('#' + e);
}

function fnUploadImage(e) {


    $('#' + e).modal("show");
    if (e == "uploadGroupImageModal" || e == "displayGroupImageModal") {
        $("#updateImageHeading").html("Select Group Photo");
        $("#DragImageDetail").html("Drag a Group photo here <span>to upload your image</span>");
        $("#btnImageSave").text("Select As Group Photo");
    }
    if (e == "uploadUserImageModal" || e == "displayUserImageModal") {
        $("#updateImageHeading").html("Select User Photo");
        $("#DragImageDetail").html("Drag a User photo here <span>to upload your image</span>");
        $("#btnImageSave").text("Select As User Photo");
    }

    //  $('#EditImageModalTarget').val('#' + e);
}

var fnCloseModal = function (e) {
    $(e).modal("hide");
    var modelName = e.substring(1, e.length);
    fnUploadImage(modelName);
}

var fnChangeUserImage = function () {
    $('#profileModal img').attr('src', $('#userImage img').attr('src'));
}

var fnChangeGroupImage = function () {
    $('#groupprofileModal img').attr('src', $('#groupImage img').attr('src'));
}

function getSelectedRoleId() {
    $("#DesignationData").css({ "display": "none" });

    $("#selectRepotingUsers").empty();
    var selectedRoleId = $("#selectUserRole").val().toUpperCase();
    var UserId = $("#InputUser_Id").val();
    $.ajax(
        {
            url: "/User/getParentRoleByRoleId/",
            method: "GET",
            data: { "DesignationId": selectedRoleId, "UserId": UserId },
            success: function (result) {
                if (forceLogOut(result)) {
                    if (parseInt(result) <= 0) {
                        if (parseInt(result) < 0) {
                            Alert(_msgError, "Error");
                            return false;
                        }
                    }
                    if (result.length > 0) {
                        $("#selectRepotingUsers").append("<option disabled selected value='' > -- select an option -- </option>");
                        for (var i = 0; i < result.length; i++) {
                            $("#selectRepotingUsers").append("<option value='" + result[i].userId + "'>" + result[i].firstName +
                                " " + result[i].lastName + " (" + result[i].userName + ")</option>");

                        }
                    }
                    else {
                        $("#selectRepotingUsers").append("<option value='" + '00000000-0000-0000-0000-000000000000' + "'>-- select an option --</option>");
                        //$("#selectRepotingUsers").append("<option>-- select an option --</option>");
                    }

                    checkValidReportingToUser();

                    $('.nice-select2').niceSelect('destroy');
                    $('.nice-select2').niceSelect();
                    $('.nice-select2 ul').addClass('demo-y');
                    $(".demo-y").mCustomScrollbar({
                        theme: "minimal-dark"
                    });
                    $('#createUserForm,#editUserForm').find('.nice-select2 .current').addClass('wraptext');
                }
            }
        });
    // }
}

//To Get Personal Setting change password
function PersonalChangePassword() {
    $('#PersonalChangePassword').addClass('active');
    $('#setting-tab').removeClass('active');
    $.ajax(
        {
            url: "/User/PersonalChangePassword/",
            method: "GET",
            success: function (result) {
                if (parseInt(result) <= 0) {
                    if (parseInt(result) < 0) {
                        Alert(_msgError, "Error");
                        return false;
                    }
                }
                $("#UserDataPersonal").html(result);
            }
        });
}
//To Get Personal Setting change password
function UpdateChangePassword(e) {


    event.preventDefault();

    var formData = $('#ChangePassword');
    var Url = '/User/PersonalChangePassword';
    if (formData.valid()) {
        fnButtonLoader(e);
        var posting = $.post(Url, formData.serialize());
        posting.done(function (result) {
            if (forceLogOut(result)) {
                fnRemoveBtnDisableData(e);

                if (parseInt(result) <= 0) {
                    if (parseInt(result) < 0) {
                        Alert(_msgError, "Error");
                        fnRemoveBtnDisableData(e);
                        return false;
                    }

                }
                if (parseInt(result) == 5) {
                    Alert(_ErrorPassword, "Error");
                    fnRemoveBtnDisableData(e);
                    return false;
                }

                Alert(_SavePassword, "Success");
                $("#UserDataPersonal").html(result);
            }
        });
    }
}
////////////////////////////////////////////
$('body').on("click", ".viewuser-dropdown", function (e) {
    $(this).is(".show") && e.stopPropagation();
});




function checkRoleName() {
    var roleName = $("#txtProName").val();
    // var oldRoleName = $("#txtProName").attr("data-name");

    if ($(".btnProfile").text() == "Update") {
        var oldRoleName = $("#hdnProfileNameold").val().trim().toLowerCase();
    }
    else {
        var oldRoleName = $("#hdnProfileNameold").val();
    }


    if (oldRoleName != undefined && oldRoleName === roleName.trim().toLowerCase()) {
        return;
    }
    var isRoleValid = false;
    $.ajax(
        {
            url: "/SecurityControls/GetProfileName/",
            method: "GET",
            async: false,
            data: { "roleName": roleName },
            success: function (result) {
                if (forceLogOut(result)) {
                    //if (parseInt(result) <= 0) {
                    //    if (parseInt(result) < 0) {
                    //        Alert(_msgError, "Error");
                    //        return false;
                    //    }
                    //}
                    if (result == "Success") {
                        $("#txtProName").val(roleName);
                        $("#existroleMsg").text("");
                        $("#existroleMsg").addClass("hidden");
                        isRoleValid = true;
                        return isRoleValid;
                    }
                    else {
                        $("#txtProName").val("");
                        $("#existroleMsg").text(result);
                        $("#existroleMsg").removeClass("hidden");
                        isRoleValid = false;
                        return isRoleValid;
                    }

                }
            }
        });
}

function ShowPartialLoader() {
    // Show partial loader
    //$(".partialLoader").fadeIn("fast");
    $(".partialLoader").removeClass("hide");
}
function HidePartialLoader() {
    // Hide partial loader
    //$(".partialLoader").fadeOut("fast");
    $(".partialLoader").addClass("hide");
}


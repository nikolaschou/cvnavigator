var scrollPosition = 0;
var pageNames = ["searchPage", "findPage", "readPage", "meetPage"];
var numberOfPages;
var currentResource;
function scroll(goRight) {
    if (scrollPosition >= numberOfPages && goRight) return;
    if (scrollPosition <= 0 && !goRight) return;
    scrollPosition = scrollPosition + (goRight ? 1 : -1);
    $.mobile.changePage("#" + pageNames[scrollPosition]);
    //if (goRight) $('#Span' + (scrollPosition + 1)).addClass('selected');
    //else $('#Span' + (scrollPosition + 2)).removeClass('selected');
}

$(document).ready(

    function () {
        //Determine the number of pages
        numberOfPages = $('#outerDiv div.main').length;

        $('#resourceList li a').live('click',
            function () {
                var resourceId = $(this).closest('li').attr('resourceId');
                GetResource(resourceId, populateCvBody);
            }
        );

     


        if ($('#findPage').is(':visible')) populateList();

        $.mobile.defaultPageTransition = "flip";

        //Copy in footer on all pages
        var commonFooter = $("#common-footer");
        $('div[data-role=content]').each(function () {
            $(this).append(commonFooter.clone());
        });
        /*$(".waitingGifDiv").ajaxStart(function () {
            $(this).show();
        });
       $(".waitingGifDiv").ajaxStop(function () {
            $(this).hide();
        });
        $(".waitingGifDiv").click(function () {
            $(this).hide();
        });*/
    }
);

    $(document).bind("mobileinit", function () {
        $.mobile.defaultPageTransition = "slide";
    });

    function populateCvBody(data) {
        var r = data.d;
        E.cvHeadline.text((r.ProfileTitle ? r.ProfileTitle + " - " : "") + r.Initials + "");
        E.cvResume.text(r.ProfileResume);
        E.cvSince.text(r.ProfessionalSince);
        E.cvCountry.text(r.CountryName);
        E.cvLanguage.text(r.LanguageName);
        E.cvAvailableBy.text(r.AvailableBy);
        addToList(E.cvExpertSkills, r.ExpertSkills);
        addToList(E.cvCustomers, r.Customers);
        addToList(E.cvOtherSkills, r.OtherSkills);
    }

    function addToList(listRef, arr) {
        listRef.html("");
        if (arr == null || arr.length == 0) {
            listRef.append('<li>Not registered</li>');
        } else {
            for (var i = 0; i < arr.length; i++) {
                listRef.append('<li>' + arr[i] + '</li>');
            }
        }
    }



    function GetResources() {
        callAspnet("GetResources", { skillName: $('#searchSkillText').val(), availableByStr: "" }, function (data) { populateList(data.d); })
    }

    function GetResource(id, handler) {
        callAspnet("GetResource", { resourceId: id }, function (data) { currentResource = data.d; handler(data); });
    }   
    
    function BookMeeting(id, email) {
        callAspnet("BookMeeting", { resourceId: id, contactEmail: email }, function () { E.requestMessage.text("Thank you for your interest in our consultants. We will get back to you as soon as possible") });
    }

    function requestMeeting() {
        scroll(true);
        E.requestResource.val(currentResource.Initials);
    }

    function doBookMeeting() {
        var resourceId = currentResource.ResourceId;
        var email =E.requestEmail.val();
        if (email == "") alert('Please enter an email address');
        else BookMeeting(resourceId, email);
    }

    function populateList(resources) {
        if (!resources && !localStorage.currentResultList) return;
        if (!resources) resources = $.evalJSON(localStorage.currentResultList);
        else localStorage.currentResultList = $.toJSON(resources);

        $('#resourceList').html("");
        for (var i = 0; i < resources.length; i++) {
            var r = resources[i];
            $('#resourceList').append("<li resourceId='" + r.ResourceId + "'><a href='#readPage'>" + cap(E.searchSkillText.val()) + " " + r.SkillLevelName + " - <b>" + r.Initials + "</li>");
        }
        $('#resourceList').listview('refresh');
    }

    function cap(str) {
        return str[0].toUpperCase() + str.substring(1);
    }

//jQeruy 語法
$("#popupPanel").on({
    popupbeforeposition: function () {
        var h = $(window).height();
        $("#popupPanel").css("height", h);
    }
});
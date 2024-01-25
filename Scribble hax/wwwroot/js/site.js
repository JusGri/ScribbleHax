// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).keydown(function(){
    $(".board-tile").each(function () {
        var currentValue = $(this).data("value");
        if (currentValue != "") {
            if(currentValue.length > 1){
                $(this).addClass(currentValue.toLowerCase());
            }
            else{
                $(this).addClass("occupied");
            }
        }
    });
});
// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$('#SpecialCharacter').keypress(function (e) {
    var regex = new RegExp("^[ a-zA-Z0-9]+$");
    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
    if (regex.test(str)) {
        return true;
    }
    e.preventDefault();
    return false;
});

let today = new Date().toISOString().split('T')[0];
$('#RemoveFutureDate').attr('max', today);

$(".error-message").hide();
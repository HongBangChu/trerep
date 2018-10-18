// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

var bootpagOpt = {
    page: 1,
    maxVisible: 10,
    leaps: true,
    firstLastUse: true,
    first: '←',
    last: '→',
    wrapClass: 'pagination',
    activeClass: 'active',
    disabledClass: 'disabled',
    nextClass: 'next',
    prevClass: 'prev',
    lastClass: 'last',
    firstClass: 'first'
};

$(function () {
    $('a.nav-link').parent().removeClass('active');
    $('a.nav-link[href="' + window.location.pathname + '"]').parent().addClass('active');
    // dropdown link
    $('a.dropdown-item[href="' + window.location.pathname + '"]').parent().parent().addClass('active');
});
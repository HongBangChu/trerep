﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
var selectionActive = function (instance, firstColumn, lastColumn) {
    // because sheet focus on to A1 before delete then blur after delete
    if ($(firstColumn).prop('id') != $(lastColumn).prop('id'))
        $(instance).data('cif', $(firstColumn).text());
}

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
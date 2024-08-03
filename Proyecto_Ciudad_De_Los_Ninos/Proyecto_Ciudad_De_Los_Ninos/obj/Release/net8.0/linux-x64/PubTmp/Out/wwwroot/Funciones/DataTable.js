﻿$(document).ready(function () {
    inicializarTabla('#tableShow');
});

function inicializarTabla(selector) {
    new DataTable(selector, {
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json",
            "paginate": {
                "first": "",
                "last": ""
            }
        }
    });
}
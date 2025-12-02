// Users Page JavaScript
document.addEventListener("DOMContentLoaded", () => {
    const $ = window.$;
    const bootstrap = window.bootstrap;

    // Solo inicializa si hay filas con datos
    if ($(".my-table-custom tbody tr").length > 0 && !$(".my-table-custom tbody tr td").attr("colspan")) {
        $(".my-table-custom").DataTable({
            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json",
            },
            pageLength: 10,
            order: [[0, "asc"]],
        });
    }
});
// Users Page JavaScript
document.addEventListener("DOMContentLoaded", () => {
    const $ = window.$;
    const bootstrap = window.bootstrap;

    // Solo inicializa si hay filas con datos
    if ($("#usersTable tbody tr").length > 0 && !$("#usersTable tbody tr td").attr("colspan")) {
        $("#usersTable").DataTable({
            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json",
            },
            pageLength: 10,
            order: [[0, "asc"]],
            columnDefs: [{ orderable: false, targets: 6 }],
        });
    }
});
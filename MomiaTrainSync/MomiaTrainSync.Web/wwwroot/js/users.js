// Users Page JavaScript
document.addEventListener("DOMContentLoaded", () => {
    // Import jQuery and Bootstrap
    const $ = window.$;
    const bootstrap = window.bootstrap;

    // ✅ Inicializa DataTable
    const usersTable = $("#usersTable").DataTable({
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json",
        },
        pageLength: 10,
        order: [[0, "asc"]],
        columnDefs: [{ orderable: false, targets: 5 }],
    });

    // ✅ Formulario para agregar usuario
    const addUserForm = document.getElementById("addAthleteForm");
    if (addUserForm) {
        const modal = document.getElementById("addAthleteModal");
        const saveButton = modal.querySelector(".btn-primary");

        saveButton.addEventListener("click", () => {
            const name = document.getElementById("athleteName").value;
            const email = document.getElementById("athleteEmail").value;
            const coachSelect = document.getElementById("athleteCoach");
            const coach = coachSelect.options[coachSelect.selectedIndex].text;
            const sport = document.getElementById("athleteSport").value;

            if (name && email && coach && sport) {
                const newId = String(usersTable.rows().count() + 1).padStart(3, "0");

                usersTable.row
                    .add([
                        newId,
                        `<div class="d-flex align-items-center">
                            <img src="/placeholder.svg?height=32&width=32" alt="Usuario" class="rounded-circle me-2" width="32" height="32">
                            <span>${name}</span>
                        </div>`,
                        email,
                        coach,
                        `<span class="badge bg-success">Activo</span>`,
                        `<div class="btn-group" role="group">
                            <button class="btn btn-sm btn-outline-primary" title="Ver perfil">
                                <i class="bi bi-eye"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-secondary" title="Editar">
                                <i class="bi bi-pencil"></i>
                            </button>
                            <button class="btn btn-sm btn-outline-danger" title="Eliminar">
                                <i class="bi bi-trash"></i>
                            </button>
                        </div>`,
                    ])
                    .draw();

                addUserForm.reset();
                bootstrap.Modal.getInstance(modal).hide();

                console.log("[v1] Nuevo usuario agregado:", { name, email, coach, sport });
            }
        });
    }

    // ✅ Eliminar usuario
    $("#athletesTable").on("click", ".btn-outline-danger", function () {
        if (confirm("¿Estás seguro de que deseas eliminar este usuario?")) {
            usersTable.row($(this).parents("tr")).remove().draw();
        }
    });
});

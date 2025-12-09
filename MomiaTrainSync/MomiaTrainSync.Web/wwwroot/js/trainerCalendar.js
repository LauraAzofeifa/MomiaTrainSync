document.addEventListener("DOMContentLoaded", async () => {

    const calendarEl = document.getElementById("calendar");
    const FullCalendar = window.FullCalendar;
    const bootstrap = window.bootstrap;

    // ─────────────────────────────────────────────
    // Fetch
    // ─────────────────────────────────────────────


    const fetchEventos = async () => {
        const r = await fetch(`/Calendar/Get?esEntrenador=true`);
        return r.json();
    };

    const fetchTiposSesion = async () => {
        const r = await fetch('/Calendar/TipoSesiones');
        return r.json();
    };

    const tiposSesionResult = await fetchTiposSesion();
    const tiposSesion = tiposSesionResult.datos || tiposSesionResult || [];

    // ─────────────────────────────────────────────
    // Crear color map dinámico
    //─────────────────────────────────────────────

    const generarColor = (id) => {
        // genera colores distintivos de forma determinística
        const colors = ["#0ea5e9", "#22c55e", "#facc15", "#38bdf8", "#ef4444", "#8b5cf6", "#f97316"];
        return colors[id % colors.length];
    };

    const colorMap = {};
    tiposSesion.forEach(t => {
        const key = t.nombre.toLowerCase();
        colorMap[key] = generarColor(t.idTipoSesion);
    });

    // ─────────────────────────────────────────────
    // Renderizar filtros dinámicos
    //─────────────────────────────────────────────

    const contenedorFiltros = document.getElementById("contenedorFiltrosTipo");
    contenedorFiltros.innerHTML = "";

    tiposSesion.forEach(t => {
        const tipo = t.nombre.toLowerCase();
        const color = colorMap[tipo];

        contenedorFiltros.insertAdjacentHTML("beforeend", `
            <div class="form-check mb-2">
                <input class="form-check-input filtro-tipo" 
                       type="checkbox" 
                       data-tipo="${tipo}" 
                       id="filter-${tipo}" 
                       checked>

                <label class="form-check-label" for="filter-${tipo}">
                    <span class="badge me-2" style="background:${color}"></span>
                    ${t.nombre}
                </label>
            </div>
        `);
    });

    // ─────────────────────────────────────────────
    // Cargar eventos del día (versión final dinámica)
    // ─────────────────────────────────────────────

    async function cargarEventosHoy() {
        try {
            const data = await fetchEventos();
            const contenedor = document.getElementById("eventosHoyContainer");

            contenedor.innerHTML = "";

            // Obtener fecha de hoy en formato YYYY-MM-DD
            const hoy = new Date();
            const yyyy = hoy.getFullYear();
            const mm = String(hoy.getMonth() + 1).padStart(2, "0");
            const dd = String(hoy.getDate()).padStart(2, "0");
            const hoyStr = `${yyyy}-${mm}-${dd}`;

            // Filtrar por coincidencia exacta
            const eventosHoy = data.filter(e => e.fechaProgramada === hoyStr);

            if (eventosHoy.length === 0) {
                contenedor.innerHTML = `<p class="text-muted">No hay eventos programados para hoy.</p>`;
                return;
            }

            eventosHoy.forEach(e => {

                // Como DateOnly NO trae hora, mostramos "Sin hora" o lo que prefieras
                const hora = "Sin hora";

                // Normalizar tipo
                const tipo = (e.tipoSesionNombre || "default").toLowerCase();
                const color = colorMap[tipo] || "#6366f1";

                // Badge dinámico
                const badge = `
                <span class="badge" style="background:${color}">
                    ${e.tipoSesionNombre}
                </span>
            `;

                contenedor.insertAdjacentHTML(
                    "beforeend",
                    `
                <div class="event-item mb-3">
                    <div class="d-flex align-items-start">
                        <div class="event-time">
                            <small class="text-muted">${hora}</small>
                        </div>
                        <div class="ms-3">
                            <h6 class="mb-1">${e.nombreEntrenamiento}</h6>
                            <small class="text-muted">${e.nombreEntrenador}</small>
                            <div class="mt-1">${badge}</div>
                        </div>
                    </div>
                </div>
            `
                );
            });

        } catch (err) {
            console.error("Error cargando eventos de hoy:", err);
        }
    }


    cargarEventosHoy();

    // ─────────────────────────────────────────────
    // Calendar
    // ─────────────────────────────────────────────

    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            //right: "dayGridMonth,timeGridWeek,timeGridDay",
            right: "dayGridMonth,timeGridWeek",
        },
        locale: "es",
        buttonText: { today: "Hoy", month: "Mes", week: "Semana"},

        events: async (info, success, fail) => {
            try {
                const data = await fetchEventos();

                success(
                    data.map(e => {
                        const tipo = (e.tipoSesionNombre || "default").toLowerCase();
                        return {
                            id: e.idEntrenamiento,
                            title: `${e.nombreEntrenamiento} - ${e.nombreRutina}`,
                            start: e.fechaProgramada,
                            backgroundColor: colorMap[tipo] || "#6366f1",
                            borderColor: colorMap[tipo] || "#6366f1",
                            extendedProps: {
                                tipoSesion: tipo,
                                descripcionRutina: e.descripcionRutina,
                                entrenador: e.nombreEntrenador,
                                atleta: e.nombreAtleta
                            }
                        };
                    })
                );

            } catch (err) {
                fail(err);
            }
        },

        eventClick: function (info) {
            // Rellenar el contenido del modal
            const modalTitle = document.getElementById("modalTitle");
            const modalBody = document.getElementById("modalBody");

            modalTitle.textContent = info.event.title;
            modalBody.innerHTML = `
            <p><strong>Entrenador:</strong> ${info.event.extendedProps.entrenador}</p>
            <p><strong>Atleta:</strong> ${info.event.extendedProps.atleta}</p>
            <p><strong>Tipo de sesión:</strong> ${info.event.extendedProps.tipoSesion}</p>
            <p><strong>Descripción:</strong> ${info.event.extendedProps.descripcionRutina}</p>
            <p><strong>Fecha:</strong> ${info.event.start.toLocaleDateString()}</p>
        `;

            // Abrir el modal con Bootstrap
            const myModal = new bootstrap.Modal(document.getElementById("eventModal"));
            myModal.show();
        }
    });

    calendar.render();


    // ─────────────────────────────────────────────
    // Filtros
    // ─────────────────────────────────────────────

    function aplicarFiltros() {
        const activos = [...document.querySelectorAll(".filtro-tipo")]
            .filter(cb => cb.checked)
            .map(cb => cb.dataset.tipo);

        calendar.getEvents().forEach(ev => {
            ev.setProp("display", activos.includes(ev.extendedProps.tipoSesion) ? "auto" : "none");
        });
    }

    document.querySelectorAll(".filtro-tipo")
        .forEach(cb => cb.addEventListener("change", aplicarFiltros));

});

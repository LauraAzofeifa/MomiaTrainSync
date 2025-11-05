// Schedule Page JavaScript
document.addEventListener("DOMContentLoaded", () => {
    const calendarEl = document.getElementById("calendar")

    // Declare FullCalendar and bootstrap variables
    const FullCalendar = window.FullCalendar
    const bootstrap = window.bootstrap

    // Initialize FullCalendar
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: "dayGridMonth",
        headerToolbar: {
            left: "prev,next today",
            center: "title",
            right: "dayGridMonth,timeGridWeek,timeGridDay",
        },
        locale: "es",
        buttonText: {
            today: "Hoy",
            month: "Mes",
            week: "Semana",
            day: "Día",
        },
        events: [
            {
                title: "Entrenamiento de Fuerza - María González",
                start: new Date().toISOString().split("T")[0] + "T09:00:00",
                end: new Date().toISOString().split("T")[0] + "T10:30:00",
                backgroundColor: "#2563eb",
                borderColor: "#2563eb",
            },
            {
                title: "Sesión de Velocidad - Juan Martínez",
                start: new Date().toISOString().split("T")[0] + "T14:00:00",
                end: new Date().toISOString().split("T")[0] + "T15:30:00",
                backgroundColor: "#10b981",
                borderColor: "#10b981",
            },
            {
                title: "Técnica de Carrera - Ana Torres",
                start: new Date().toISOString().split("T")[0] + "T16:00:00",
                end: new Date().toISOString().split("T")[0] + "T17:00:00",
                backgroundColor: "#f59e0b",
                borderColor: "#f59e0b",
            },
            {
                title: "Recuperación - Laura Fernández",
                start: new Date(Date.now() + 86400000).toISOString().split("T")[0] + "T10:00:00",
                end: new Date(Date.now() + 86400000).toISOString().split("T")[0] + "T11:00:00",
                backgroundColor: "#06b6d4",
                borderColor: "#06b6d4",
            },
        ],
        eventClick: (info) => {
            alert("Evento: " + info.event.title)
        },
        dateClick: (info) => {
            document.getElementById("eventDate").value = info.dateStr
            const modal = new bootstrap.Modal(document.getElementById("addEventModal"))
            modal.show()
        },
    })

    calendar.render()

    // Add Event Form Handler
    const addEventForm = document.getElementById("addEventForm")
    if (addEventForm) {
        const modal = document.getElementById("addEventModal")
        const saveButton = modal.querySelector(".btn-primary")

        saveButton.addEventListener("click", () => {
            const title = document.getElementById("eventTitle").value
            const athlete =
                document.getElementById("eventAthlete").options[document.getElementById("eventAthlete").selectedIndex].text
            const type = document.getElementById("eventType").value
            const date = document.getElementById("eventDate").value
            const time = document.getElementById("eventTime").value
            const duration = document.getElementById("eventDuration").value

            if (title && athlete && type && date && time) {
                // Calculate end time
                const startDateTime = new Date(date + "T" + time)
                const endDateTime = new Date(startDateTime.getTime() + duration * 60000)

                // Determine color based on type
                let color = "#2563eb"
                if (type === "cardio") color = "#10b981"
                else if (type === "technique") color = "#f59e0b"
                else if (type === "recovery") color = "#06b6d4"

                // Add event to calendar
                calendar.addEvent({
                    title: `${title} - ${athlete}`,
                    start: startDateTime,
                    end: endDateTime,
                    backgroundColor: color,
                    borderColor: color,
                })

                // Reset form and close modal
                addEventForm.reset()
                const bsModal = bootstrap.Modal.getInstance(modal)
                bsModal.hide()

                console.log("[v0] New event added:", { title, athlete, type, date, time, duration })
            }
        })
    }

    // Filter checkboxes
    const filterCheckboxes = document.querySelectorAll('[id^="filter"]')
    filterCheckboxes.forEach((checkbox) => {
        checkbox.addEventListener("change", function () {
            console.log("[v0] Filter changed:", this.id, this.checked)
            // In a real app, this would filter the calendar events
        })
    })
})

// Profile Page JavaScript
document.addEventListener("DOMContentLoaded", () => {

    const editProfileBtn = document.getElementById("editProfileBtn")
    const cancelEditBtn = document.getElementById("cancelEditBtn")
    const saveButtons = document.getElementById("saveButtons")
    const profileForm = document.getElementById("profileForm")

    // 🔹 SOLO CAMPOS EDITABLES
    const editableInputs =
        profileForm.querySelectorAll(".editable-field")

    // 🔹 Guardar valores originales
    const originalValues = {}

    editableInputs.forEach(input => {
        originalValues[input.name] = input.value
    })

    // =========================
    // Evento Editar
    // =========================
    if (editProfileBtn) {

        editProfileBtn.addEventListener("click", () => {

            editableInputs.forEach(input => {
                input.disabled = false
            })

            saveButtons.classList.remove("d-none")
            editProfileBtn.classList.add("d-none")

        })

    }

    // =========================
    // Evento Cancelar
    // =========================
    if (cancelEditBtn) {

        cancelEditBtn.addEventListener("click", () => {

            editableInputs.forEach(input => {

                // Restaurar valores originales
                input.value =
                    originalValues[input.name]

                input.disabled = true

            })

            saveButtons.classList.add("d-none")
            editProfileBtn.classList.remove("d-none")

        })

    }

    // =========================
    // Two Factor Authentication
    // =========================
    const twoFactorSwitch =
        document.getElementById("twoFactorSwitch")

    if (twoFactorSwitch) {

        twoFactorSwitch.addEventListener("change", (e) => {

            if (e.target.checked) {

                console.log("Two-factor authentication enabled")
                alert("Autenticación de dos factores activada")

            }
            else {

                console.log("Two-factor authentication disabled")
                alert("Autenticación de dos factores desactivada")

            }

        })

    }

    // =========================
    // Preferences Save
    // =========================
    const languageSelect =
        document.getElementById("language")

    const timezoneSelect =
        document.getElementById("timezone")

    const emailNotifications =
        document.getElementById("emailNotifications")

    const pushNotifications =
        document.getElementById("pushNotifications")

    const preferencesBtn =
        document.querySelector(".card:last-child .btn-primary")

    if (preferencesBtn) {

        preferencesBtn.addEventListener("click", () => {

            const preferences = {

                language: languageSelect?.value,
                timezone: timezoneSelect?.value,
                emailNotifications: emailNotifications?.checked,
                pushNotifications: pushNotifications?.checked,

            }

            console.log("Preferences saved:", preferences)

            localStorage.setItem(
                "userPreferences",
                JSON.stringify(preferences)
            )

            alert("Preferencias guardadas correctamente")

        })

    }

})
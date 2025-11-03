// Profile Page JavaScript
document.addEventListener("DOMContentLoaded", () => {
    // Load user data from localStorage
    const userData = JSON.parse(localStorage.getItem("user") || "{}")

    // Edit Profile Button
    const editProfileBtn = document.getElementById("editProfileBtn")
    const cancelEditBtn = document.getElementById("cancelEditBtn")
    const saveButtons = document.getElementById("saveButtons")
    const profileForm = document.getElementById("profileForm")
    const formInputs = profileForm.querySelectorAll("input, textarea")

    // Evento Editar
    editProfileBtn.addEventListener("click", () => {
        formInputs.forEach((input) => {
            if (input.id !== "role") {
                input.disabled = false
            }
        })
        saveButtons.classList.remove("d-none")
        editProfileBtn.classList.add("d-none")
    })

    // EventoForm
    cancelEditBtn.addEventListener("click", () => {
        formInputs.forEach((input) => {
            input.disabled = true
        })
        saveButtons.classList.add("d-none")
        editProfileBtn.classList.remove("d-none")
        profileForm.reset()
    })

    // Profile Form Submit
    //profileForm.addEventListener("submit", (e) => {
    //    e.preventDefault()

    //    const updatedData = {
    //        firstName: document.getElementById("firstName").value,
    //        lastName: document.getElementById("lastName").value,
    //        email: document.getElementById("email").value,
    //        phone: document.getElementById("phone").value,
    //        birthdate: document.getElementById("birthdate").value,
    //        bio: document.getElementById("bio").value,
    //    }

    //    console.log("[v0] Profile updated:", updatedData)

    //    // Update localStorage
    //    const currentUser = JSON.parse(localStorage.getItem("user") || "{}")
    //    localStorage.setItem("user", JSON.stringify({ ...currentUser, ...updatedData }))

    //    // Disable inputs and hide save buttons
    //    formInputs.forEach((input) => {
    //        input.disabled = true
    //    })
    //    saveButtons.classList.add("d-none")
    //    editProfileBtn.classList.remove("d-none")

    //    // Show success message
    //    alert("Perfil actualizado correctamente")
    //})

    // Change Password Form
    const changePasswordForm = document.getElementById("changePasswordForm")
    changePasswordForm.addEventListener("submit", (e) => {
        e.preventDefault()

        const currentPassword = document.getElementById("currentPassword").value
        const newPassword = document.getElementById("newPassword").value
        const confirmNewPassword = document.getElementById("confirmNewPassword").value

        if (newPassword !== confirmNewPassword) {
            alert("Las contraseñas no coinciden")
            return
        }

        if (newPassword.length < 8) {
            alert("La contraseña debe tener al menos 8 caracteres")
            return
        }

        console.log("[v0] Password change requested")

        // Close modal
        const modal = window.bootstrap.Modal.getInstance(document.getElementById("changePasswordModal"))
        modal.hide()

        // Reset form
        changePasswordForm.reset()

        // Show success message
        alert("Contraseña cambiada correctamente")
    })

    // Two Factor Authentication Toggle
    const twoFactorSwitch = document.getElementById("twoFactorSwitch")
    twoFactorSwitch.addEventListener("change", (e) => {
        if (e.target.checked) {
            console.log("[v0] Two-factor authentication enabled")
            alert("Autenticación de dos factores activada")
        } else {
            console.log("[v0] Two-factor authentication disabled")
            alert("Autenticación de dos factores desactivada")
        }
    })

    // Preferences Save
    const languageSelect = document.getElementById("language")
    const timezoneSelect = document.getElementById("timezone")
    const emailNotifications = document.getElementById("emailNotifications")
    const pushNotifications = document.getElementById("pushNotifications")

    document.querySelector(".card:last-child .btn-primary").addEventListener("click", () => {
        const preferences = {
            language: languageSelect.value,
            timezone: timezoneSelect.value,
            emailNotifications: emailNotifications.checked,
            pushNotifications: pushNotifications.checked,
        }

        console.log("[v0] Preferences saved:", preferences)

        // Save to localStorage
        localStorage.setItem("userPreferences", JSON.stringify(preferences))

        alert("Preferencias guardadas correctamente")
    })
})

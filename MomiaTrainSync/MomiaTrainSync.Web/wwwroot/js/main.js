// Main JavaScript for Dashboard
document.addEventListener("DOMContentLoaded", () => {
    // Check if user is logged in
    //const user = JSON.parse(localStorage.getItem("user"))
    //if (!user && !window.location.pathname.includes("login") && !window.location.pathname.includes("register")) {
    //    window.location.href = "login.html"
    //    return
    //}

    // Sidebar Toggle
    const sidebarCollapse = document.getElementById("sidebarCollapse")
    const sidebar = document.getElementById("sidebar")
    const content = document.getElementById("content")

    if (sidebarCollapse) {
        sidebarCollapse.addEventListener("click", () => {
            sidebar.classList.toggle("active")
            content.classList.toggle("active")
        })
    }

    // Handle submenu toggles
    //const submenuToggles = document.querySelectorAll('[data-bs-toggle="collapse"]')
    //const bootstrap = window.bootstrap // Declare the bootstrap variable
    //submenuToggles.forEach((toggle) => {
    //    toggle.addEventListener("click", function (e) {
    //        e.preventDefault()
    //        const target = document.querySelector(this.getAttribute("href"))
    //        if (target) {
    //            const bsCollapse = new bootstrap.Collapse(target, {
    //                toggle: true,
    //            })
    //        }
    //    })
    //})
})

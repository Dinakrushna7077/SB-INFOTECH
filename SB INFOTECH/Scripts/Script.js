const menubtn = document.getElementById("btn");
menubtn.addEventListener("click", () => {
    const dropdown = document.getElementById("drdn");
    if (dropdown.classList.contains("d-flex")) {
        dropdown.classList.add("d-none");
        dropdown.classList.remove("d-flex");
    }
    else {
        dropdown.classList.remove("d-none");
        dropdown.classList.add("d-flex", "dropdown");
    }
})
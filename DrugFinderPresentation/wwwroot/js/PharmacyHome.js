document.getElementById("distanceFilter").addEventListener("change", function () {
    const value = this.value;
    const items = document.querySelectorAll(".request-item");

    items.forEach(item => {
        const distance = parseFloat(item.dataset.distance);

        if (value === "all" || distance <= parseFloat(value)) {
            item.style.display = "block";
        } else {
            item.style.display = "none";
        }
    });
});
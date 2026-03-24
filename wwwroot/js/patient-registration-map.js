// patient-registration-map.js

// Check if geolocation is available
if ('geolocation' in navigator) {
    navigator.geolocation.getCurrentPosition(success, error);
} else {
    alert('Geolocation is not supported by this browser.');
}

function success(position) {
    const lat = position.coords.latitude;
    const lng = position.coords.longitude;

    // Initialize the map (using Leaflet for this example)
    const map = L.map('map').setView([lat, lng], 13);

    // Load the tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap'
    }).addTo(map);

    // Marker for user's location
    const marker = L.marker([lat, lng]).addTo(map);
    marker.bindPopup('<b>You are here!</b>').openPopup();
}

function error() {
    alert('Unable to retrieve your location.');
}
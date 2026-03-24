// Leaflet initialization
var map = L.map('map').setView([51.505, -0.09], 13);

// Adding tile layer
L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
}).addTo(map);

// Geolocation detection
if (navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function(position) {
        var lat = position.coords.latitude;
        var lng = position.coords.longitude;
        var marker = L.marker([lat, lng]).addTo(map);
        map.setView([lat, lng], 13);
        document.getElementById('latitude').value = lat;
        document.getElementById('longitude').value = lng;
    });
}

// Creating a Control for Geocoding
var geocoder = new L.Control.Geocoder.Nominatim();
var control = L.Control.geocoder({
    placeholder: 'Enter a location',
    onFind: function(results) {
        if (results.length) {
            var latLng = results[0].center;
            L.marker(latLng).addTo(map);
            map.setView(latLng, 13);
        }
    }
}).addTo(map);

// Adding click handler to place markers
map.on('click', function(e) {
    var lat = e.latlng.lat;
    var lng = e.latlng.lng;
    L.marker([lat, lng]).addTo(map);
    document.getElementById('latitude').value = lat;
    document.getElementById('longitude').value = lng;
});

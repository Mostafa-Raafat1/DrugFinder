<<<<<<< HEAD
/* DrugFinder MVC – site.js */

$(function () {

  /* ── Auto-dismiss flash alerts ─────────────── */
=======
<<<<<<< Updated upstream
﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
=======
/* DrugFinder MVC – site.js
   Global helpers only. Loading-state logic lives in each
   page-specific js/pages/*.js file so validation failures
   never leave the submit button permanently disabled.      */

$(function () {

  /* ── Auto-dismiss flash alerts ─────────────────────────── */
>>>>>>> origin/Mostafa
  setTimeout(function () {
    $('.alert-banner').fadeOut(600);
  }, 5000);

<<<<<<< HEAD
  /* ── Loading state on submit buttons ────────── */
  $('form').on('submit', function () {
    var btn = $(this).find('.df-btn-submit');
    if (btn.length) {
      btn.prop('disabled', true)
         .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Please wait…');
    }
  });

  /* ── Add form-control class to df-inputs on
        validation error so Bootstrap shows red ─── */
=======
  /* ── Map server-side invalid inputs to is-invalid class ── */
>>>>>>> origin/Mostafa
  $('.df-input').each(function () {
    if ($(this).hasClass('input-validation-error')) {
      $(this).addClass('is-invalid');
    }
  });

<<<<<<< HEAD
  /* ── Geolocation helper (used in register forms) */
=======
  /* ── Geolocation helper (shared by register forms) ─────── */
>>>>>>> origin/Mostafa
  window.detectLocation = function (latId, lngId, btnId) {
    var btn = document.getElementById(btnId);
    if (!btn) return;
    btn.addEventListener('click', function () {
      if (!navigator.geolocation) {
        alert('Geolocation is not supported by your browser.');
        return;
      }
      btn.disabled = true;
      btn.textContent = 'Detecting…';
      navigator.geolocation.getCurrentPosition(
        function (pos) {
          document.getElementById(latId).value = pos.coords.latitude.toFixed(6);
          document.getElementById(lngId).value = pos.coords.longitude.toFixed(6);
          btn.textContent = '✓ Location Detected';
          btn.style.borderColor = '#22c55e';
          btn.style.color = '#22c55e';
        },
        function () {
          alert('Unable to retrieve your location. Please enter coordinates manually.');
          btn.disabled = false;
          btn.textContent = 'Use My Location';
        }
      );
    });
  };
<<<<<<< HEAD
});
=======

});
>>>>>>> Stashed changes
>>>>>>> origin/Mostafa

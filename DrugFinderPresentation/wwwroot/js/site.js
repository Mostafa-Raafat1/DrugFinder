<<<<<<< Updated upstream
<<<<<<< Updated upstream
﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
=======
=======
>>>>>>> Stashed changes
/* DrugFinder MVC – site.js
   Global helpers only. Loading-state logic lives in each
   page-specific js/pages/*.js file so validation failures
   never leave the submit button permanently disabled.      */

$(function () {

  /* ── Auto-dismiss flash alerts ─────────────────────────── */
  setTimeout(function () {
    $('.alert-banner').fadeOut(600);
  }, 5000);

  /* ── Map server-side invalid inputs to is-invalid class ── */
  $('.df-input').each(function () {
    if ($(this).hasClass('input-validation-error')) {
      $(this).addClass('is-invalid');
    }
  });

  /* ── Geolocation helper (shared by register forms) ─────── */
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

});
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======

/* ── Pharmacy: poll for unread notifications every 60 s ──── *
   Shows a pulsing red dot on the bell icon in the navbar
   without requiring a full page reload.                      */
(function () {
  var dot = document.getElementById('notifNavDot');
  if (!dot) return; // only runs for pharmacy users

  function checkUnread() {
    $.ajax({
      url: '/Pharmacy/Notifications',
      type: 'GET',
      success: function (html) {
        // Count how many unread cards exist in the fetched HTML
        var parsed    = $(html);
        var unreadCount = parsed.find('.notif-card[data-read="false"]').length;
        if (unreadCount > 0) {
          $(dot).show();
          $('#notifNavLink').attr('title', unreadCount + ' unread notification(s)');
        } else {
          $(dot).hide();
          $('#notifNavLink').removeAttr('title');
        }
      }
    });
  }

  // Run once immediately, then every 60 seconds
  checkUnread();
  setInterval(checkUnread, 60000);
})();
>>>>>>> Stashed changes

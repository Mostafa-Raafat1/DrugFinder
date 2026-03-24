/**
 * pharmacy-register.js — DrugFinder Pharmacy Registration Page
 * Handles: GPS geolocation, licence number formatting,
 *          password match indicator, validation feedback,
 *          submit loading state
 */

$(function () {

  /* ── Card entrance ──────────────────────────────────────── */
  $('.df-form-card').css({ opacity: 0, transform: 'translateY(28px)', transition: 'opacity .4s ease, transform .4s ease' });
  setTimeout(function () {
    $('.df-form-card').css({ opacity: 1, transform: 'translateY(0)' });
  }, 60);

  /* ── Validation summary ─────────────────────────────────── */
  var summary = $('#valSummary');
  if (summary.find('li').length) summary.fadeIn(300);

  $('[data-valmsg-for]').each(function () {
    if ($(this).text().trim()) {
      $('[name="' + $(this).attr('data-valmsg-for') + '"]').addClass('is-invalid');
    }
  });

  /* ── Real-time border feedback ──────────────────────────── */
  $('.df-input').on('input', function () {
    if ($(this).val().trim()) $(this).removeClass('is-invalid').css('border-color', 'var(--teal)');
  }).on('blur', function () {
    if (!$(this).hasClass('is-invalid')) $(this).css('border-color', '');
  });

  /* ── Licence number: uppercase + trim on blur ───────────── */
  $('[name="LiscenceNumber"]').on('blur', function () {
    $(this).val($(this).val().trim().toUpperCase());
  });

  /* ── Password strength meter ────────────────────────────── */
  var strengthBar  = $('<div>').css({ height: '4px', borderRadius: '4px', marginTop: '6px', background: '#e5e7eb', overflow: 'hidden' });
  var strengthFill = $('<div>').css({ height: '100%', width: '0%', transition: 'width .3s ease, background .3s ease', borderRadius: '4px' });
  strengthBar.append(strengthFill);

  var pwdField = $('[name="Password"]');
  pwdField.closest('.col-sm-6').append(strengthBar);

  var strengthLabel = $('<small>').css({ fontSize: '.78rem', color: 'var(--gray-400)', marginTop: '3px', display: 'block' });
  pwdField.closest('.col-sm-6').append(strengthLabel);

  pwdField.on('input', function () {
    var val   = $(this).val();
    var score = 0;
    if (val.length >= 6)  score++;
    if (val.length >= 10) score++;
    if (/[A-Z]/.test(val)) score++;
    if (/[0-9]/.test(val)) score++;
    if (/[^A-Za-z0-9]/.test(val)) score++;

    var pct   = (score / 5) * 100;
    var color = score <= 1 ? '#f45b69' : score <= 3 ? '#f4c842' : '#22c55e';
    var label = score <= 1 ? 'Weak' : score <= 3 ? 'Moderate' : 'Strong';

    strengthFill.css({ width: pct + '%', background: color });
    strengthLabel.text(val.length ? 'Password strength: ' + label : '').css('color', color);
  });

  /* ── Password match indicator ───────────────────────────── */
  $('[name="ConfirmPassword"]').on('input', function () {
    var match = $(this).val() === pwdField.val();
    $(this).css('border-color', $(this).val() ? (match ? 'var(--success)' : 'var(--coral)') : '');
  });

  /* ── GPS Geolocation ────────────────────────────────────── */
  var gpsBtn = $('#gpsBtn');

  gpsBtn.on('click', function () {
    if (!navigator.geolocation) {
      showGpsError('Geolocation is not supported by your browser.');
      return;
    }

    gpsBtn.prop('disabled', true)
          .html('<span class="spinner-border spinner-border-sm me-1" role="status"></span> Detecting pharmacy location…');

    navigator.geolocation.getCurrentPosition(
      function (pos) {
        var lat = pos.coords.latitude.toFixed(6);
        var lng = pos.coords.longitude.toFixed(6);

        $('#lat').val(lat).css('border-color', 'var(--success)').trigger('input');
        $('#lng').val(lng).css('border-color', 'var(--success)').trigger('input');

        gpsBtn.prop('disabled', false)
              .html('<i class="fa-solid fa-circle-check me-1" style="color:var(--success);"></i> Location set: ' + lat + ', ' + lng)
              .css({ borderColor: 'var(--success)', color: 'var(--success)' });
      },
      function (err) {
        var msgs = {
          1: 'Access denied. Please allow location or enter coordinates manually.',
          2: 'Location unavailable. Please enter coordinates manually.',
          3: 'Request timed out. Please try again.'
        };
        showGpsError(msgs[err.code] || 'Unable to retrieve location.');
      },
      { timeout: 10000, maximumAge: 0 }
    );
  });

  function showGpsError(msg) {
    gpsBtn.prop('disabled', false)
          .html('<i class="fa-solid fa-triangle-exclamation me-1" style="color:var(--coral);"></i> ' + msg)
          .css({ borderColor: 'var(--coral)', color: 'var(--coral)' });
    setTimeout(function () {
      gpsBtn.html('<i class="fa-solid fa-location-crosshairs me-1"></i> Use My Location')
            .css({ borderColor: 'var(--teal)', color: 'var(--teal)' });
    }, 4000);
  }

  /* ── Coordinate range validation on blur ────────────────── */
  $('#lat').on('blur', function () {
    var v = parseFloat($(this).val());
    if ($(this).val() && (isNaN(v) || v < -90 || v > 90)) {
      $(this).addClass('is-invalid');
      addCoordError($(this), 'Latitude must be between -90 and 90');
    }
  });

  $('#lng').on('blur', function () {
    var v = parseFloat($(this).val());
    if ($(this).val() && (isNaN(v) || v < -180 || v > 180)) {
      $(this).addClass('is-invalid');
      addCoordError($(this), 'Longitude must be between -180 and 180');
    }
  });

  function addCoordError(input, msg) {
    if (!input.next('.coord-err').length) {
      $('<small class="coord-err field-validation-error">' + msg + '</small>').insertAfter(input);
    }
  }

  $('#lat, #lng').on('input', function () {
    $(this).removeClass('is-invalid').css('border-color', 'var(--teal)');
    $(this).next('.coord-err').remove();
  });

  /* ── Pharmacy name – title-case helper on blur ──────────── */
  $('[name="Name"]').on('blur', function () {
    var val = $(this).val().trim();
    if (val) {
      $(this).val(val.replace(/\b\w/g, function (c) { return c.toUpperCase(); }));
    }
  });

  /* ── Submit loading state ───────────────────────────────── */
  $('form').on('submit', function () {
    var btn = $(this).find('.df-btn-submit');
    btn.prop('disabled', true)
       .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Registering Pharmacy…');
  });

});

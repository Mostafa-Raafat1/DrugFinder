/**
 * patient-register.js — DrugFinder Patient Registration Page
<<<<<<< Updated upstream
 * Handles: GPS geolocation, coordinate validation,
 *          password strength meter, validation feedback,
<<<<<<< HEAD
 *          form submit loading state
=======
 *          submit loading state (only fires when form is valid)
<<<<<<< HEAD
>>>>>>> origin/Mostafa
=======
=======
>>>>>>> Stashed changes
>>>>>>> origin/Mostafa
 */

$(function () {

  /* ── Card entrance ──────────────────────────────────────── */
  $('.df-form-card').css({ opacity: 0, transform: 'translateY(28px)', transition: 'opacity .4s ease, transform .4s ease' });
  setTimeout(function () { $('.df-form-card').css({ opacity: 1, transform: 'translateY(0)' }); }, 60);

<<<<<<< HEAD
<<<<<<< HEAD
  /* ── Validation summary ─────────────────────────────────── */
=======
  /* ── Validation summary (server-side errors on page load) ─ */
>>>>>>> origin/Mostafa
=======
<<<<<<< Updated upstream
  /* ── Validation summary (server-side errors on page load) ─ */
=======
  /* ── Validation summary (server-side errors) ────────────── */
>>>>>>> Stashed changes
>>>>>>> origin/Mostafa
  var summary = $('#valSummary');
  if (summary.find('li').length) summary.fadeIn(300);

  /* ── Highlight server-side invalid fields ───────────────── */
  $('[data-valmsg-for]').each(function () {
    if ($(this).text().trim()) {
      $('[name="' + $(this).attr('data-valmsg-for') + '"]').addClass('is-invalid');
    }
  });

  /* ── Real-time border feedback for all inputs ───────────── */
  $('.df-input').on('input change', function () {
    if ($(this).val() !== '' && $(this).val() !== null) {
      $(this).removeClass('is-invalid').css('border-color', 'var(--teal)');
    }
  }).on('blur', function () {
    if (!$(this).hasClass('is-invalid') && !$(this).hasClass('is-valid')) {
      $(this).css('border-color', '');
    }
  });

  /* ── Fix: jQuery Validate ignores type=number empty values  ─
        Force it to treat an empty number input as invalid      */
  $.validator.methods.number = (function (original) {
    return function (value, element) {
      if ($(element).attr('type') === 'number' && value === '') return false;
      return original.call(this, value, element);
    };
  })($.validator.methods.number);

  /* ── Also make required work for number inputs explicitly ── */
  $.validator.methods.required = (function (original) {
    return function (value, element) {
      if ($(element).attr('type') === 'number') {
        return value !== '' && value !== null && value !== undefined;
      }
      return original.call(this, value, element);
    };
  })($.validator.methods.required);

  /* ── Password strength meter ────────────────────────────── */
<<<<<<< HEAD
  var strengthBar = $('<div>')
    .attr('id', 'strengthBar')
    .css({
      height: '4px', borderRadius: '4px', marginTop: '6px',
      background: '#e5e7eb', overflow: 'hidden'
    });
=======
  var strengthBar  = $('<div>').css({ height: '4px', borderRadius: '4px', marginTop: '6px', background: '#e5e7eb', overflow: 'hidden' });
>>>>>>> origin/Mostafa
  var strengthFill = $('<div>').css({ height: '100%', width: '0%', transition: 'width .3s ease, background .3s ease', borderRadius: '4px' });
  strengthBar.append(strengthFill);

  var pwdInput = $('[name="Password"]');
  pwdInput.closest('.col-sm-6').append(strengthBar);

  var strengthLabel = $('<small>').css({ fontSize: '.78rem', color: 'var(--gray-400)', marginTop: '3px', display: 'block' });
  pwdInput.closest('.col-sm-6').append(strengthLabel);

  pwdInput.on('input', function () {
    var val = $(this).val(), score = 0;
    if (val.length >= 6)          score++;
    if (val.length >= 10)         score++;
    if (/[A-Z]/.test(val))        score++;
    if (/[0-9]/.test(val))        score++;
    if (/[^A-Za-z0-9]/.test(val)) score++;

    var pct   = (score / 5) * 100;
    var color = score <= 1 ? '#f45b69' : score <= 3 ? '#f4c842' : '#22c55e';
    var label = score <= 1 ? 'Weak'    : score <= 3 ? 'Moderate' : 'Strong';

    strengthFill.css({ width: pct + '%', background: color });
    strengthLabel.text(val.length ? 'Password strength: ' + label : '').css('color', color);
  });

  /* ── Password match indicator ───────────────────────────── */
  $('[name="ConfirmPassword"]').on('input', function () {
    var match = $(this).val() === pwdInput.val();
    $(this).css('border-color', $(this).val() ? (match ? 'var(--success)' : 'var(--coral)') : '');
  });

  /* ── GPS Geolocation ────────────────────────────────────── */
  var gpsBtn = $('#gpsBtn');

  gpsBtn.on('click', function () {
    if (!navigator.geolocation) { showGpsError('Geolocation is not supported by your browser.'); return; }

    gpsBtn.prop('disabled', true)
          .html('<span class="spinner-border spinner-border-sm me-1" role="status"></span> Detecting location…');

    navigator.geolocation.getCurrentPosition(
      function (pos) {
        var lat = pos.coords.latitude.toFixed(6);
        var lng = pos.coords.longitude.toFixed(6);

        $('#lat').val(lat).trigger('input').css('border-color', 'var(--success)').removeClass('is-invalid');
        $('#lng').val(lng).trigger('input').css('border-color', 'var(--success)').removeClass('is-invalid');

        // Clear any validation error messages for these fields
        $('[data-valmsg-for="Latitude"], [data-valmsg-for="Longitude"]').text('');

        gpsBtn.prop('disabled', false)
              .html('<i class="fa-solid fa-circle-check me-1" style="color:var(--success);"></i> Location Detected: ' + lat + ', ' + lng)
              .css({ borderColor: 'var(--success)', color: 'var(--success)' });
      },
      function (err) {
        var msgs = { 1: 'Location access denied. Please enter coordinates manually.',
                     2: 'Location unavailable. Please enter coordinates manually.',
                     3: 'Location request timed out.' };
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

  /* ── Show is-invalid on lat/lng when they lose focus empty ─ */
  $('#lat, #lng').on('blur', function () {
    if ($(this).val() === '' || $(this).val() === null) {
      $(this).addClass('is-invalid').css('border-color', 'var(--coral)');
    }
<<<<<<< Updated upstream
  });

  $('#lng').on('blur', function () {
    var v = parseFloat($(this).val());
    if ($(this).val() && (isNaN(v) || v < -180 || v > 180)) {
      $(this).addClass('is-invalid');
      showFieldError($(this), 'Longitude must be between -180 and 180');
    }
  });

  function showFieldError(input, msg) {
<<<<<<< HEAD
    var existingErr = input.next('.inline-err');
    if (!existingErr.length) {
=======
    if (!input.next('.inline-err').length) {
>>>>>>> origin/Mostafa
      $('<small class="inline-err field-validation-error">' + msg + '</small>').insertAfter(input);
    }
  }

<<<<<<< HEAD
  /* Clear inline coord errors on input */
=======
>>>>>>> origin/Mostafa
  $('#lat, #lng').on('input', function () {
=======
  }).on('input', function () {
>>>>>>> Stashed changes
    $(this).removeClass('is-invalid').css('border-color', 'var(--teal)');
    $(this).next('.inline-err').remove();
  });

<<<<<<< HEAD
<<<<<<< HEAD
  /* ── Submit loading state ───────────────────────────────── */
  $('#regForm').on('submit', function () {
    var btn = $(this).find('.df-btn-submit');
    btn.prop('disabled', true)
       .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Creating Account…');
  });

=======
=======
<<<<<<< Updated upstream
>>>>>>> origin/Mostafa
  /* ── Submit: only disable button when form is actually valid */
  var submitBtn = $('#regForm .df-btn-submit');
  var originalLabel = submitBtn.html();

  $('#regForm').on('submit', function (e) {
    var form = $(this);

    /* Ask jQuery Validate if the form is valid before we do anything */
=======
  /* ── Submit: only disable button when the form is valid ──── */
  var submitBtn  = $('#regForm .df-btn-submit');
  var origLabel  = submitBtn.html();

  $('#regForm').on('submit', function () {
    var form      = $(this);
>>>>>>> Stashed changes
    var validator = form.data('validator');
    if (validator && !form.valid()) return; // let unobtrusive show errors

    submitBtn.prop('disabled', true)
             .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Creating Account…');
  });

  // Re-enable if server returned errors
  if (summary.find('li').length) {
    submitBtn.prop('disabled', false).html(origLabel);
  }

>>>>>>> origin/Mostafa
});

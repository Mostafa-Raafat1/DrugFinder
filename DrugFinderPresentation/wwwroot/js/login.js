/**
 * login.js — DrugFinder Account / Login Page
 * Handles: show/hide password, validation feedback,
<<<<<<< HEAD
 *          form card entrance animation, submit loading state
=======
 *          form card entrance animation,
 *          submit loading state (only fires when form is valid)
>>>>>>> origin/Mostafa
 */

$(function () {

  /* ── Card entrance animation ────────────────────────────── */
  $('.df-form-card')
<<<<<<< HEAD
    .css({ opacity: 0, transform: 'translateY(32px)' })
    .animate({ opacity: 1 }, {
      duration: 420,
      step: function (now) {
        var offset = 32 * (1 - now);
        $(this).css('transform', 'translateY(' + offset + 'px)');
      }
    });
=======
    .css({ opacity: 0, transform: 'translateY(32px)', transition: 'opacity .42s ease, transform .42s ease' });
  setTimeout(function () {
    $('.df-form-card').css({ opacity: 1, transform: 'translateY(0)' });
  }, 60);
>>>>>>> origin/Mostafa

  /* ── Show / hide password toggle ───────────────────────── */
  $('#togglePwd').on('click', function () {
    var input = $('#pwdInput');
    var icon  = $('#pwdIcon');
    var isPassword = input.attr('type') === 'password';

    input.attr('type', isPassword ? 'text' : 'password');
    icon.toggleClass('fa-eye', !isPassword)
        .toggleClass('fa-eye-slash', isPassword);

<<<<<<< HEAD
    /* Briefly highlight the field */
=======
>>>>>>> origin/Mostafa
    input.css({ borderColor: 'var(--teal)', boxShadow: '0 0 0 3px rgba(15,163,177,.15)' });
    setTimeout(function () { input.css({ borderColor: '', boxShadow: '' }); }, 600);
  });

  /* ── Show validation summary if server returned errors ──── */
  var summary = $('.validation-summary-errors');
  if (summary.find('li').length) {
    summary.show().css({ opacity: 0 }).animate({ opacity: 1 }, 300);
  }

  /* ── Highlight inputs that failed server-side validation ── */
  $('[data-valmsg-for]').each(function () {
    if ($(this).text().trim()) {
<<<<<<< HEAD
      var fieldName = $(this).attr('data-valmsg-for');
      $('[name="' + fieldName + '"]').addClass('is-invalid');
=======
      $('[name="' + $(this).attr('data-valmsg-for') + '"]').addClass('is-invalid');
>>>>>>> origin/Mostafa
    }
  });

  /* ── Real-time border feedback while typing ─────────────── */
  $('.df-input').on('input', function () {
    if ($(this).val().trim()) {
      $(this).removeClass('is-invalid').css({ borderColor: 'var(--teal)' });
    }
  }).on('blur', function () {
    if (!$(this).hasClass('is-invalid')) {
      $(this).css({ borderColor: '' });
    }
  });

<<<<<<< HEAD
  /* ── Submit loading state ───────────────────────────────── */
  $('#loginForm').on('submit', function () {
    var isValid = true;

    /* Basic client-side checks before letting form go */
    $(this).find('[required]').each(function () {
      if (!$(this).val().trim()) { isValid = false; }
    });

    if (!isValid) return; /* Let unobtrusive validation handle it */

    var btn = $(this).find('.df-btn-submit');
    btn.prop('disabled', true)
       .html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Signing in…');
  });

=======
  /* ── Submit: only disable button when form is actually valid */
  var submitBtn = $('#loginForm .df-btn-submit');
  var originalLabel = submitBtn.html();

  $('#loginForm').on('submit', function () {
    var form = $(this);

    /* Ask jQuery Validate if the form is valid */
    var validator = form.data('validator');
    if (validator && !form.valid()) {
      /* Client-side validation failed — do not disable the button */
      return;
    }

    /* Valid — show loading state */
    submitBtn.prop('disabled', true)
             .html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Signing in…');
  });

  /* Re-enable if server returned errors (bad credentials etc.) */
  if (summary.find('li').length) {
    submitBtn.prop('disabled', false).html(originalLabel);
  }

>>>>>>> origin/Mostafa
  /* ── Auto-focus first empty field ──────────────────────── */
  $('.df-input').each(function () {
    if (!$(this).val()) { $(this).focus(); return false; }
  });

});

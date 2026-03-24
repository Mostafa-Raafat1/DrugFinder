/**
 * login.js — DrugFinder Account / Login Page
 * Handles: show/hide password, validation feedback,
 *          form card entrance animation, submit loading state
 */

$(function () {

  /* ── Card entrance animation ────────────────────────────── */
  $('.df-form-card')
    .css({ opacity: 0, transform: 'translateY(32px)' })
    .animate({ opacity: 1 }, {
      duration: 420,
      step: function (now) {
        var offset = 32 * (1 - now);
        $(this).css('transform', 'translateY(' + offset + 'px)');
      }
    });

  /* ── Show / hide password toggle ───────────────────────── */
  $('#togglePwd').on('click', function () {
    var input = $('#pwdInput');
    var icon  = $('#pwdIcon');
    var isPassword = input.attr('type') === 'password';

    input.attr('type', isPassword ? 'text' : 'password');
    icon.toggleClass('fa-eye', !isPassword)
        .toggleClass('fa-eye-slash', isPassword);

    /* Briefly highlight the field */
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
      var fieldName = $(this).attr('data-valmsg-for');
      $('[name="' + fieldName + '"]').addClass('is-invalid');
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

  /* ── Auto-focus first empty field ──────────────────────── */
  $('.df-input').each(function () {
    if (!$(this).val()) { $(this).focus(); return false; }
  });

});

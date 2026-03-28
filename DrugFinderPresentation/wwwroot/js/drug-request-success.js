/**
 * drug-request-success.js — DrugFinder Request Success Page
 * Handles: icon pop animation, confetti burst,
 *          auto-redirect countdown (optional)
 */

$(function () {

  /* ── Success icon pop ───────────────────────────────────── */
  var iconWrap = $('.success-icon-wrap');
  iconWrap.css({ transform: 'scale(0)', opacity: 0 });
  setTimeout(function () {
    iconWrap.css({ transition: 'transform .5s cubic-bezier(.175,.885,.32,1.275), opacity .3s ease', transform: 'scale(1)', opacity: 1 });
  }, 80);

  /* ── Page content stagger ───────────────────────────────── */
  var items = ['h1', 'p', '.row.g-3', '.d-flex.gap-3'];
  items.forEach(function (sel, i) {
    var el = $(sel).first();
    el.css({ opacity: 0, transform: 'translateY(20px)', transition: 'none' });
    setTimeout(function () {
      el.css({ transition: 'opacity .45s ease, transform .45s ease', opacity: 1, transform: 'translateY(0)' });
    }, 350 + i * 100);
  });

  /* ── Stat cards entrance ────────────────────────────────── */
  $('.row.g-3 .col-4').each(function (i) {
    var el = $(this);
    el.css({ opacity: 0, transform: 'scale(.85)', transition: 'none' });
    setTimeout(function () {
      el.css({ transition: 'opacity .4s ease ' + (i * 0.12) + 's, transform .4s cubic-bezier(.175,.885,.32,1.275) ' + (i * 0.12) + 's', opacity: 1, transform: 'scale(1)' });
    }, 550);
  });

  /* ── Simple confetti burst ──────────────────────────────── */
  function launchConfetti() {
    var colors = ['#0fa3b1', '#22c55e', '#f4c842', '#f45b69', '#6366f1'];
    var container = $('<div>').css({ position: 'fixed', inset: 0, pointerEvents: 'none', zIndex: 9999 });
    $('body').append(container);

    for (var i = 0; i < 60; i++) {
      (function (i) {
        var piece = $('<div>').css({
          position:     'absolute',
          width:        Math.random() * 8 + 5 + 'px',
          height:       Math.random() * 8 + 5 + 'px',
          borderRadius: Math.random() > 0.5 ? '50%' : '2px',
          background:   colors[Math.floor(Math.random() * colors.length)],
          left:         Math.random() * 100 + 'vw',
          top:          '-10px',
          opacity:      1
        });
        container.append(piece);

        var duration = Math.random() * 1800 + 1200;
        var endTop   = Math.random() * 60 + 40 + 'vh';
        var endLeft  = (parseFloat(piece.css('left')) + (Math.random() * 120 - 60)) + 'px';

        piece.animate(
          { top: endTop, left: endLeft, opacity: 0 },
          { duration: duration, easing: 'linear', complete: function () { $(this).remove(); } }
        );
      })(i);
    }

    setTimeout(function () { container.remove(); }, 3200);
  }

  setTimeout(launchConfetti, 400);

  /* ── Button hover effects ───────────────────────────────── */
  $('a').on('mouseenter', function () {
    $(this).css('transform', 'translateY(-2px)');
  }).on('mouseleave', function () {
    $(this).css('transform', '');
  });

});

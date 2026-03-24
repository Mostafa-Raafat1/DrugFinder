/**
 * home.js — DrugFinder Home / Landing Page
 * Handles: hero entrance animation, step-card scroll reveal,
 *          CTA card hover lifts, smooth anchor scrolling
 */

$(function () {

  /* ── Hero entrance stagger ──────────────────────────────── */
  var heroItems = [
    '.df-hero-badge',
    '.df-hero h1',
    '.df-hero p',
    '.df-hero .d-flex.flex-wrap',
    '.df-hero .d-flex.gap-4'
  ];

  heroItems.forEach(function (sel, i) {
    var el = $(sel);
    el.css({ opacity: 0, transform: 'translateY(28px)', transition: 'none' });
    setTimeout(function () {
      el.css({ transition: 'opacity .55s ease, transform .55s ease', opacity: 1, transform: 'translateY(0)' });
    }, 120 + i * 110);
  });

  /* Hero visual circle – fade in */
  $('.hero-circle').css({ opacity: 0, transform: 'scale(.88)' });
  setTimeout(function () {
    $('.hero-circle').css({ transition: 'opacity .7s ease, transform .7s ease', opacity: 1, transform: 'scale(1)' });
  }, 300);

  /* ── Intersection Observer – scroll reveal for step cards ─ */
  if ('IntersectionObserver' in window) {
    var revealObs = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          $(entry.target).addClass('revealed');
          revealObs.unobserve(entry.target);
        }
      });
    }, { threshold: 0.15 });

    /* Prepare step cards */
    $('.step-card').each(function (i) {
      $(this).css({
        opacity: 0,
        transform: 'translateY(32px)',
        transition: 'opacity .5s ease ' + (i * 0.13) + 's, transform .5s ease ' + (i * 0.13) + 's'
      });
      revealObs.observe(this);
    });

    /* Prepare CTA cards */
    $('.cta-card').each(function (i) {
      $(this).css({
        opacity: 0,
        transform: 'translateY(24px)',
        transition: 'opacity .5s ease ' + (i * 0.15) + 's, transform .5s ease ' + (i * 0.15) + 's'
      });
      revealObs.observe(this);
    });

    /* When revealed, restore styles */
    $(document).on('animationend webkitAnimationEnd', '.step-card, .cta-card', function () {
      $(this).css({ opacity: '', transform: '' });
    });

    /* Polyfill: set revealed immediately */
    $('<style>.revealed { opacity: 1 !important; transform: translateY(0) !important; }</style>').appendTo('head');
  }

  /* ── Smooth scroll for any in-page anchor links ─────────── */
  $('a[href^="#"]').on('click', function (e) {
    var target = $($(this).attr('href'));
    if (target.length) {
      e.preventDefault();
      $('html, body').animate({ scrollTop: target.offset().top - 80 }, 500);
    }
  });

  /* ── Step number counter animation ─────────────────────── */
  /* Adds a subtle "pop" when a step scrolls into view */
  if ('IntersectionObserver' in window) {
    var numObs = new IntersectionObserver(function (entries) {
      entries.forEach(function (entry) {
        if (entry.isIntersecting) {
          var num = $(entry.target).find('.step-number');
          num.css({ transform: 'scale(1.25)', transition: 'transform .25s ease' });
          setTimeout(function () { num.css({ transform: 'scale(1)' }); }, 280);
          numObs.unobserve(entry.target);
        }
      });
    }, { threshold: 0.6 });

    $('.step-card').each(function () { numObs.observe(this); });
  }

  /* ── CTA card button ripple effect ─────────────────────── */
  $(document).on('click', '.btn-teal, .btn-outline-white', function (e) {
    var btn = $(this);
    var ripple = $('<span>')
      .css({
        position: 'absolute',
        borderRadius: '50%',
        background: 'rgba(255,255,255,.35)',
        width: '8px', height: '8px',
        left: e.offsetX - 4, top: e.offsetY - 4,
        transform: 'scale(0)',
        transition: 'transform .5s ease, opacity .5s ease',
        opacity: 1,
        pointerEvents: 'none'
      });

    if (btn.css('position') === 'static') btn.css('position', 'relative');
    btn.append(ripple);
    setTimeout(function () { ripple.css({ transform: 'scale(28)', opacity: 0 }); }, 10);
    setTimeout(function () { ripple.remove(); }, 560);
  });

});

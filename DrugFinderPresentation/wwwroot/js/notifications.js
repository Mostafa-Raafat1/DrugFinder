/**
 * notifications.js — DrugFinder Pharmacy Notifications Page
 * Handles: filter tabs (all / unread / read),
 *          mark-as-read (client-side toggle),
 *          mark-all-read,
 *          auto-refresh every 60 s,
 *          page entrance animation
 */

$(function () {

  /* ── Entrance animation ─────────────────────────────────── */
  $('.notif-card').each(function (i) {
    $(this).css({ opacity: 0, transform: 'translateY(14px)', transition: 'none' });
    setTimeout((function (el) {
      return function () {
        el.css({ transition: 'opacity .35s ease, transform .35s ease', opacity: 1, transform: 'translateY(0)' });
      };
    })($(this)), 60 + i * 55);
  });

  /* ── State helpers ──────────────────────────────────────── */
  function getCards()     { return $('#notifList .notif-card'); }
  function getUnread()    { return getCards().filter('[data-read="false"]'); }
  function updateBadge()  {
    var n = getUnread().length;
    if (n > 0) {
      $('#unreadBadge').text(n + ' unread').show();
      $('#markAllReadBtn').prop('disabled', false);
    } else {
      $('#unreadBadge').fadeOut(200);
      $('#markAllReadBtn').prop('disabled', true);
    }
  }

  /* ── Filter tabs ────────────────────────────────────────── */
  var currentFilter = 'all';

  function applyFilter(filter) {
    currentFilter = filter;
    var cards = getCards();
    var visible = 0;

    cards.each(function () {
      var isRead   = $(this).attr('data-read') === 'true';
      var show     = filter === 'all'
                  || (filter === 'unread' && !isRead)
                  || (filter === 'read'   && isRead);
      $(this).toggle(show);
      if (show) visible++;
    });

    $('#filterEmpty').toggle(visible === 0 && cards.length > 0);
  }

  $('.notif-filter-btn').on('click', function () {
    $('.notif-filter-btn').removeClass('active');
    $(this).addClass('active');
    applyFilter($(this).data('filter'));
  });

  /* ── Mark single notification as read ──────────────────── */
  $(document).on('click', '.btn-mark-read', function () {
    var btn  = $(this);
    var card = btn.closest('.notif-card');
    var id   = card.data('id');

    /* Optimistic UI — update immediately */
    card.attr('data-read', 'true')
        .removeClass('notif-unread');

    card.find('.notif-dot')
        .removeClass('dot-active');

    btn.replaceWith(
      '<span class="notif-read-label">' +
      '<i class="fa-solid fa-circle-check me-1"></i>Read' +
      '</span>'
    );

    updateBadge();

    /* Re-apply current filter (may hide this card if filter = unread) */
    if (currentFilter !== 'all') {
      setTimeout(function () {
        card.slideUp(250, function () {
          applyFilter(currentFilter);
        });
      }, 400);
    }
  });

  /* ── Mark ALL as read ───────────────────────────────────── */
  $('#markAllReadBtn').on('click', function () {
    getUnread().each(function () {
      var card = $(this);
      card.attr('data-read', 'true').removeClass('notif-unread');
      card.find('.notif-dot').removeClass('dot-active');
      card.find('.btn-mark-read').replaceWith(
        '<span class="notif-read-label">' +
        '<i class="fa-solid fa-circle-check me-1"></i>Read' +
        '</span>'
      );
    });
    updateBadge();

    if (currentFilter !== 'all') applyFilter(currentFilter);

    /* Visual confirmation */
    var btn = $(this);
    var orig = btn.html();
    btn.html('<i class="fa-solid fa-check me-1"></i>Done!').css('color', 'var(--success)');
    setTimeout(function () { btn.html(orig).css('color', ''); }, 1800);
  });

  /* ── Refresh button — full page reload ──────────────────── */
  $('#refreshBtn').on('click', function () {
    var btn = $(this);
    btn.prop('disabled', true)
       .html('<span class="spinner-border spinner-border-sm me-1" role="status"></span>Refreshing…');
    location.reload();
  });

  /* ── Auto-refresh every 60 seconds ─────────────────────── */
  var autoRefreshTimer = setInterval(function () {
    /* Silent refresh: reload only if tab is visible */
    if (!document.hidden) {
      location.reload();
    }
  }, 60000);

  /* Show a small countdown hint */
  var secondsLeft = 60;
  var countdownEl = $('<span>')
    .attr('id', 'refreshCountdown')
    .css({ fontSize: '.75rem', color: 'var(--gray-400)', marginLeft: '.5rem' })
    .text('Auto-refresh in 60s');
  $('#refreshBtn').after(countdownEl);

  var countdownTimer = setInterval(function () {
    secondsLeft--;
    if (secondsLeft <= 0) {
      clearInterval(countdownTimer);
      countdownEl.text('Refreshing…');
    } else {
      countdownEl.text('Auto-refresh in ' + secondsLeft + 's');
    }
  }, 1000);

  /* Stop timers if user navigates away */
  $(window).on('beforeunload', function () {
    clearInterval(autoRefreshTimer);
    clearInterval(countdownTimer);
  });

  /* ── Card hover: show full message on long text ─────────── */
  getCards().each(function () {
    var msgEl = $(this).find('.notif-message');
    var full  = msgEl.text();
    if (full.length > 180) {
      var short = full.substring(0, 180) + '…';
      msgEl.text(short);

      var toggle = $('<button>')
        .addClass('btn-mark-read ms-2')
        .css({ borderColor: 'var(--gray-400)', color: 'var(--gray-400)', fontSize: '.75rem' })
        .text('Show more');

      var expanded = false;
      toggle.on('click', function () {
        expanded = !expanded;
        msgEl.text(expanded ? full : short);
        toggle.text(expanded ? 'Show less' : 'Show more');
      });

      $(this).find('.notif-card-footer').prepend(toggle);
    }
  });

  /* ── Initial badge sync ─────────────────────────────────── */
  updateBadge();

});

/**
 * drug-request-create.js — DrugFinder Drug Request Builder
 * Handles: dynamic add/remove drug rows (re-indexed),
 *          live summary counter, form validation,
 *          drug name autocomplete suggestions,
 *          submit confirmation + loading state
 */

$(function () {

  /* ── Card entrance ──────────────────────────────────────── */
  $('.df-form-card').css({ opacity: 0, transform: 'translateY(28px)', transition: 'opacity .4s ease, transform .4s ease' });
  setTimeout(function () { $('.df-form-card').css({ opacity: 1, transform: 'translateY(0)' }); }, 60);

  /* ── Validation summary ─────────────────────────────────── */
  var summary = $('#valSummary');
  if (summary.find('li').length) summary.fadeIn(300);

  /* ── Common drug name suggestions ───────────────────────── */
  var drugSuggestions = [
    'Paracetamol', 'Ibuprofen', 'Amoxicillin', 'Omeprazole', 'Metformin',
    'Aspirin', 'Atorvastatin', 'Amlodipine', 'Lisinopril', 'Cetirizine',
    'Azithromycin', 'Diclofenac', 'Pantoprazole', 'Salbutamol', 'Metronidazole',
    'Ciprofloxacin', 'Doxycycline', 'Loratadine', 'Ranitidine', 'Clopidogrel',
    'Insulin', 'Prednisone', 'Fluconazole', 'Tramadol', 'Codeine'
  ];

  /* ── Attach autocomplete to a drug name input ───────────── */
  function attachAutocomplete(input) {
    input = $(input);
    var list = $('<datalist>').attr('id', 'drugList-' + Date.now());
    drugSuggestions.forEach(function (d) {
      list.append($('<option>').val(d));
    });
    $('body').append(list);
    input.attr('list', list.attr('id'));
  }

  /* ── Attach real-time border feedback to all inputs ─────── */
  function attachInputFeedback(scope) {
    $(scope).find('.df-input').on('input change', function () {
      if ($(this).val()) $(this).css('border-color', 'var(--teal)').removeClass('is-invalid');
    }).on('blur', function () {
      if (!$(this).hasClass('is-invalid')) $(this).css('border-color', '');
    });
  }

  /* ── Update summary counter & show/hide ─────────────────── */
  function updateSummary() {
    var count = $('#drugsContainer .drug-item-card').length;
    $('#drugCount').text(count);
    count > 0 ? $('#requestSummary').slideDown(200) : $('#requestSummary').slideUp(200);
  }

  /* ── Re-index all drug cards after add / remove ─────────── */
  function reindex() {
    $('#drugsContainer .drug-item-card').each(function (i) {
      $(this).attr('data-index', i);
      $(this).find('.item-num').text(i + 1);
      $(this).find('[name]').each(function () {
        var newName = $(this).attr('name').replace(/Drugs\[\d+\]/, 'Drugs[' + i + ']');
        $(this).attr('name', newName);
      });
    });
    updateSummary();
  }

  /* ── Build a new drug card HTML ─────────────────────────── */
  function buildCard(index) {
    return $('<div class="drug-item-card" style="animation:slideUp .3s ease;">')
      .attr('data-index', index)
      .append(
        $('<div class="drug-item-header">')
          .append($('<span class="drug-item-num">Drug #<span class="item-num">' + (index + 1) + '</span></span>'))
          .append($('<button type="button" class="btn-remove-drug" title="Remove this drug"><i class="fa-solid fa-trash-can"></i> Remove</button>'))
      )
      .append(
        $('<div class="row g-3">')
          .append(
            $('<div class="col-sm-6">')
              .append('<label class="df-label">Drug Name <span style="color:var(--coral);">*</span></label>')
              .append($('<input class="df-input" type="text" placeholder="e.g. Paracetamol" required>').attr('name', 'Drugs[' + index + '].DrugName'))
          )
          .append(
            $('<div class="col-sm-6">')
              .append('<label class="df-label">Strength</label>')
              .append($('<input class="df-input" type="text" placeholder="e.g. 500mg">').attr('name', 'Drugs[' + index + '].Strength'))
          )
          .append(
            $('<div class="col-sm-6">')
              .append('<label class="df-label">Form <span style="color:var(--coral);">*</span></label>')
              .append(
                $('<select class="df-input" required>')
                  .attr('name', 'Drugs[' + index + '].Form')
                  .append('<option value="">Select form…</option>')
                  .append('<option value="1">Tablet</option>')
                  .append('<option value="2">Capsule</option>')
                  .append('<option value="3">Syrup</option>')
                  .append('<option value="4">Injection</option>')
                  .append('<option value="5">Cream</option>')
                  .append('<option value="6">Drops</option>')
              )
          )
          .append(
            $('<div class="col-sm-6">')
              .append('<label class="df-label">Quantity <span style="color:var(--coral);">*</span></label>')
              .append($('<input class="df-input" type="number" min="1" placeholder="e.g. 2" required>').attr('name', 'Drugs[' + index + '].Quantity'))
          )
      );
  }

  /* ── Add drug button ────────────────────────────────────── */
  $('#addDrugBtn').on('click', function () {
    var newIndex = $('#drugsContainer .drug-item-card').length;
    var card = buildCard(newIndex);
    $('#drugsContainer').append(card);

    /* attach autocomplete and feedback to new card */
    attachAutocomplete(card.find('[name$=".DrugName"]'));
    attachInputFeedback(card);

    reindex();

    /* scroll to and focus new card */
    $('html, body').animate({ scrollTop: card.offset().top - 120 }, 300);
    card.find('.df-input').first().focus();
  });

  /* ── Remove drug (delegated) ────────────────────────────── */
  $(document).on('click', '.btn-remove-drug', function () {
    var cards = $('#drugsContainer .drug-item-card');
    if (cards.length <= 1) {
      /* Shake the last card instead of removing it */
      var card = $(this).closest('.drug-item-card');
      card.css({ borderColor: 'var(--coral)', transition: 'border-color .1s' });
      var shakes = 0;
      var shake  = setInterval(function () {
        card.css('transform', shakes % 2 === 0 ? 'translateX(5px)' : 'translateX(-5px)');
        shakes++;
        if (shakes > 5) { clearInterval(shake); card.css({ transform: '', borderColor: '' }); }
      }, 60);
      return;
    }
    $(this).closest('.drug-item-card').slideUp(200, function () {
      $(this).remove();
      reindex();
    });
  });

  /* ── Attach autocomplete + feedback to existing cards ───── */
  $('#drugsContainer .drug-item-card').each(function () {
    attachAutocomplete($(this).find('[name$=".DrugName"]'));
    attachInputFeedback(this);
  });

  /* ── Quantity: prevent 0 or negative ────────────────────── */
  $(document).on('change blur', '[name$=".Quantity"]', function () {
    var v = parseInt($(this).val(), 10);
    if (isNaN(v) || v < 1) {
      $(this).val(1);
    }
  });

  /* ── Submit: validate all required fields + confirm ─────── */
  $('#drugForm').on('submit', function (e) {
    var allValid = true;

    $('#drugsContainer .drug-item-card').each(function (i) {
      var drugName = $(this).find('[name$=".DrugName"]');
      var form     = $(this).find('[name$=".Form"]');
      var quantity = $(this).find('[name$=".Quantity"]');

      [drugName, form, quantity].forEach(function (field) {
        if (!field.val() || !field.val().trim()) {
          field.addClass('is-invalid').css('border-color', 'var(--coral)');
          allValid = false;
        }
      });
    });

    if (!allValid) {
      e.preventDefault();
      summary.html('<ul><li>Please fill in all required fields (Drug Name, Form, Quantity) for each drug.</li></ul>').show();
      $('html, body').animate({ scrollTop: summary.offset().top - 100 }, 300);
      return;
    }

    /* Loading state */
    var btn = $(this).find('#submitBtn');
    btn.prop('disabled', true)
       .html('<span class="spinner-border spinner-border-sm me-2" role="status"></span>Submitting Request…');
  });

  /* ── Initialize summary ─────────────────────────────────── */
  updateSummary();

});

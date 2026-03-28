// Track state per drug index
var itemCount = @Model.Items.Count;
var state = Array.from({ length: itemCount }, () => ({ avail: false, price: 0 }));

function setAvailability(btn, idx, isAvailable) {
    var card = btn.closest('.respond-drug-card');
    var allBtns = card.querySelectorAll('.respond-toggle-btn');

    allBtns[0].classList.remove('respond-toggle-yes');
    allBtns[1].classList.remove('respond-toggle-no');

    if (isAvailable) {
        allBtns[0].classList.add('respond-toggle-yes');
    } else {
        allBtns[1].classList.add('respond-toggle-no');
    }

    // Update hidden field
    document.getElementById('avail-hidden-' + idx).value = isAvailable;
    state[idx].avail = isAvailable;

    // Enable / disable price input
    var priceInput = document.getElementById('price-input-' + idx);
    priceInput.disabled = !isAvailable;
    if (!isAvailable) {
        priceInput.value = '';
        state[idx].price = 0;
    }

    updateSummary();
}

function updateSummary() {
    var avail = 0, unavail = 0, total = 0;

    for (var i = 0; i < itemCount; i++) {
        var isAvail = document.getElementById('avail-hidden-' + i).value === 'true';
        if (isAvail) {
            avail++;
            var val = parseFloat(document.getElementById('price-input-' + i).value) || 0;
            total += val;
        } else {
            unavail++;
        }
    }

    document.getElementById('js-avail-count').textContent = avail;
    document.getElementById('js-unavail-count').textContent = unavail;
    document.getElementById('js-total-price').textContent = 'EGP ' + total.toFixed(2);
}
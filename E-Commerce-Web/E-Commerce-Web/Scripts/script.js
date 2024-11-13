// Script for navigation bar
const bar = document.getElementById('bar');
const close = document.getElementById('close');
const nav = document.getElementById('navbar');

if(bar){
    bar.addEventListener('click', () => {
        nav.classList.add('active')
    })
}

if(close){
    close.addEventListener('click', () => {
        nav.classList.remove('active')
    })
}

function openPaymentModal() {
    document.getElementById("payment-modal").style.display = "flex";
}

function closePaymentModal() {
    document.getElementById("payment-modal").style.display = "none";
}

function updatePaymentMethod(title, imgSrc) {
    selectedPaymentMethod = title;
    document.getElementById('paymentMethod').value = selectedPaymentMethod;
    document.getElementById("payment-title").textContent = title;
    document.getElementById("payment-image").src = "/Content/img/pay/" + imgSrc;
    closePaymentModal();
}

window.onclick = function (event) {
    const payModal = document.getElementById("payment-modal");
    const emptyModal = document.getElementById("emptyCartModal");
    const deliveryModal = document.getElementById("deliveryModal");
    const emptyPaymentModal = document.getElementById("emptyPaymentModal");

    if (event.target === payModal || event.target === emptyModal || event.target === deliveryModal || event.target === emptyPaymentModal) {

        event.target.style.display = "none";
    }
}

function proceedToCheckout(count) {
    var cartItemsExist = count > 0 ? "true" : "false";

    if (cartItemsExist === "false") {
        document.getElementById('emptyCartModal').style.display = 'block';
    }

    return cartItemsExist === "true";
}

function closeModal() {
    var cart = document.getElementById('emptyCartModal');
    if (cart != null) {
        cart.style.display = 'none';
    }

    var delivery = document.getElementById('deliveryModal');
    if (delivery != null) {
        delivery.style.display = 'none';
    }

    var emptyPayment = document.getElementById('emptyPaymentModal');
    if (emptyPayment != null) {
        emptyPayment.style.display = 'none';
    }
}
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
    document.getElementById("payment-title").textContent = title;
    document.getElementById("payment-image").src = "/Content/img/pay/" + imgSrc;
    closePaymentModal();
}

window.onclick = function (event) {
    const modal = document.getElementById("payment-modal");
    if (event.target === modal) {
        modal.style.display = "none";
    }
}
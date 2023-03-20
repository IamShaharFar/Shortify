const form = document.querySelector('form');
const registerBtn = document.getElementById('registerBtn');

form.addEventListener('submit', function (event) {
    if (form.checkValidity()) {
        registerBtn.classList.add('animate__shakeX');

        registerBtn.addEventListener('animationend', function () {
            registerBtn.classList.remove('animate__shakeX');
        }, { once: true });
    }
});
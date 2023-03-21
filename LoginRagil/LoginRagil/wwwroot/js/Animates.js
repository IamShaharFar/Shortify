const form = document.querySelector('form');
const registerBtn = document.getElementById('registerBtn');

form.addEventListener('submit', function (event) {
    let isvalid = await isValidUser(getuser());
    if (!isvalid) {
        registerBtn.classList.add('animate__shakeX');

        registerBtn.addEventListener('animationend', function () {
            registerBtn.classList.remove('animate__shakeX');
        }, { once: true });
    }
});

async function getuser() {
    const nameInput = document.querySelector('#nameInput').value;
    const emailInput = document.querySelector('#emailInput').value;
    const passwordInput = document.querySelector('#passwordInput').value;
    const confirmPasswordInput = document.querySelector('#confirmPasswordInput').value;
    const agreeServicesCheckbox = document.querySelector('#AgreeServices').value;

    const user = {
        Name: nameInput,
        Email: emailInput,
        Password: passwordInput,
        ConfirmPassword: confirmPasswordInput,
        AgreeServices: agreeServicesCheckbox
    };
}

function isValidUser(model) {
    if (!model) {
        return false;
    }
    // Check if all required fields are present
    if (!model.Name || !model.Email || !model.Password || !model.ConfirmPassword || model.AgreeServices === false) {
        return false;
    }

    // Check if the Name field is between 2 and 20 characters
    if (model.Name.length < 2 || model.Name.length > 20) {
        return false;
    }

    // Check if the Email field is a valid email address
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(model.Email)) {
        return false;
    }

    // Check if the Password field is between 4 and 20 characters
    if (model.Password.length < 4 || model.Password.length > 20) {
        return false;
    }

    // Check if the ConfirmPassword field matches the Password field
    if (model.ConfirmPassword !== model.Password) {
        return false;
    }

    // Check if the AgreeServices field is true
    if (!model.AgreeServices) {
        return false;
    }

    // If all checks pass, the user is considered valid
    return true;
}
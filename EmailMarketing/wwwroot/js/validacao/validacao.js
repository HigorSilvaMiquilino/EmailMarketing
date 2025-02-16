export function showError(elementId, message, campo) {
    const errorElement = document.getElementById(elementId);
    errorElement.textContent = message;
    errorElement.style.display = 'block';
    errorElement.style.color = 'red';
    document.getElementById(campo).style.border = '1px solid red';
}

export function clearErrors() {
    const errorMessages = document.querySelectorAll('.error-message');
    errorMessages.forEach(function (error) {
        error.textContent = '';
        error.style.display = 'none';
    });
}

export function validateEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
    return regex.test(email);
}

export function inputErro(selectorInput,selectorErroMensagem) {
    const inputs = document.querySelectorAll(selectorInput);
    inputs.forEach(input => {
        input.addEventListener('focus', function (e) {
            e.target.style.border = '1px solid #007bff';
            const errorElement = input.parentElement.querySelector(selectorErroMensagem);
            errorElement.style.display = 'none';
        });
        input.addEventListener('blur', function (e) {
            e.target.style.border = '1px solid #ced4da';
        });
    });
}

export function inputEmailValidation(selector) {

    const emailInput = document.getElementById(selector);
    emailInput.addEventListener('input', function (e) {
        const regex = /^[a-z0-9@.]+$/;
        if (!regex.test(e.target.value)) {
            e.target.value = e.target.value.replace(/[^a-z0-9@.]/g, '');
        }
    });

    emailInput.addEventListener('keydown', function (e) {
        if (e.key === ' ') {
            e.preventDefault();
        }
    })
}



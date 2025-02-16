import { showError, clearErrors, validateEmail, inputErro, inputEmailValidation } from './validacao.js';

document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.recovery-form');

    form.setAttribute('novalidate', true);

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        clearErrors();

        const email = document.getElementById('email');

        let isValid = true;

        if (email.value.trim() === '') {
            showError('email-error', 'O e-mail é obrigatório.', email.id);
            isValid = false;
        } else if (!validateEmail(email.value.trim())) {
            showError('email-error', 'Por favor, insira um e-mail válido.', email.id);
            isValid = false;
        }

        if (isValid) {
            form.submit();
        }
    });

    inputErro('.form-input input', '.error-message');

    inputEmailValidation('email')
});
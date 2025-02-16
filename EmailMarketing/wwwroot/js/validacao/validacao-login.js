import { showError, clearErrors, validateEmail, inputErro, inputEmailValidation } from './validacao.js';
import { enviarLogin } from '../enviar/enviar-login.js';

document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.login-form');

    form.setAttribute('novalidate', true);

    form.addEventListener('submit', function (e) {
        e.preventDefault();

        clearErrors();

        const email = document.getElementById('email');
        const password = document.getElementById('password');

        let isValid = true;

        if (email.value.trim() === '') {
            showError('email-error', 'O e-mail é obrigatório.', email.id);
            isValid = false;
        } else if (!validateEmail(email.value.trim())) {
            showError('email-error', 'Por favor, insira um e-mail válido.', email.id);
            isValid = false;
        }

        if (password.value.trim() === '') {
            showError('password-error', 'A senha é obrigatória.', password.id);
            isValid = false;
        } 



        if (isValid) {
            try {
                enviarLogin();
            } catch (error) {
                console.error("Error sending login request:", error);
            }
        }
    });

    inputErro('.form-input input', '.error-message');

    inputEmailValidation('email')
});
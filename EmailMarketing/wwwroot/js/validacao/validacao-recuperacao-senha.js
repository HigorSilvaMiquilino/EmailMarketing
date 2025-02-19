import { showError, clearErrors, validateEmail, inputErro, inputEmailValidation } from './validacao.js';
import { enviarRecuperacaoSenha } from '../enviar/enviar-recuperacao-senha.js'
import { CsrfToken } from '../enviar/CsrfToken.js'

document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('.recuperacao-form');

    form.setAttribute('novalidate', true);

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        clearErrors();

        const password = document.getElementById('password');
        const confirmPassword = document.getElementById('confirm-password');

        let isValid = true;

        if (password.value.trim() === '') {
            showError('password-error', 'A senha é obrigatória.', password.id);
            isValid = false;
        } else if (!validatePassword(password.value.trim())) {
            showError('password-error', 'A senha deve ter pelo menos 6 caracteres, uma letra maiúscula e um caractere especial.', password.id);
            isValid = false;
        }

        if (confirmPassword.value.trim() === '') {
            showError('confirm-password-error', 'Confirme sua senha.', confirmPassword.id);
            isValid = false;
        } else if (confirmPassword.value.trim() !== password.value.trim()) {
            showError('confirm-password-error', 'As senhas não coincidem.', confirmPassword.id);
            isValid = false;
        }



        if (isValid) {
            await CsrfToken();
            enviarRecuperacaoSenha();
        }
    });

    function validatePassword(password) {
        const minLength = 6;
        const hasUpperCase = /[A-Z]/.test(password);
        const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);

        return password.length >= minLength && hasUpperCase && hasSpecialChar;
    }

    inputErro('.form-input input', '.error-message');

});
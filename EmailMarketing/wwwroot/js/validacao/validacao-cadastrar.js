import { showError, clearErrors, validateEmail, inputErro, inputEmailValidation } from './validacao.js';
import { enviarCadastro } from '../enviar/enviar-cadastrar.js'
import { CsrfToken } from '../enviar/CsrfToken.js'

document.addEventListener('DOMContentLoaded',  function () {
    const form = document.querySelector('.signup-form');

    form.setAttribute('novalidate', true);

    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        clearErrors();

        const name = document.getElementById('name');
        const email = document.getElementById('email');
        const password = document.getElementById('password');
        const confirmPassword = document.getElementById('confirm-password');

        let isValid = true;

        if (name.value.trim() === '') {
            showError('name-error', 'O nome completo é obrigatório.', name.id);
            isValid = false;
        } else if (!validateName(name.value.trim())) {
            showError('name-error', 'nome não está completo', name.id);
            isValid = false;
        }

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
        } else if (!validatePassword(password.value.trim())) {
            showError('password-error', 'A senha deve ter pelo menos 6 caracteres, uma letra maiúscula e um caractere especial.');
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
            enviarCadastro();
        }
    });

    function validateName(name) {
        const nameParts = name.split(/\s+/);

        if (nameParts.length < 2) {
            return false;
        }

        for (let part of nameParts) {
            if (part.length < 2) {
                return false;
            }
        }

        return true;
    }

    const nameInput = document.getElementById('name');

    nameInput.addEventListener('keydown', function (e) {
        if (e.key >= '0' && e.key <= '9') {
            e.preventDefault();
        }
        if (!/^[a-zA-Z\s]$/.test(e.key) && e.key !== 'Backspace' && e.key !== 'Delete') {
            e.preventDefault();
        }
    });

    nameInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\d/g, '');

        e.target.value = e.target.value.replace(/[^a-zA-Z\s]/g, '');

        e.target.value = e.target.value.trimStart();
    });

    function validatePassword(password) {
        const minLength = 6;
        const hasUpperCase = /[A-Z]/.test(password);
        const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);

        return password.length >= minLength && hasUpperCase && hasSpecialChar;
    }

    inputErro('.form-input input', '.error-message');

    inputEmailValidation('email')
});
import { showError, clearErrors, validateEmail, inputErro, inputEmailValidation } from './validacao.js';
import { enviarEmail } from '../enviar/enviar-esqueciminha-senha.js'; 

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
            try {
                verificarEmailExistente(email);
            } catch (error) {
                console.error("Erro ao enviar a requisição de e-mail:", error);
            }
        }
        
    });

    inputErro('.form-input input', '.error-message');

    inputEmailValidation('email')
});

async function verificarEmailExistente(email) {
    try {
        const response = await fetch(`/api/Recuperacao/email?email=${encodeURIComponent(email.value.trim())}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 404) {
                const errorData = await response.json();
                throw new Error(errorData.message || "E-mail não encontrado.");
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || "Erro na requisição.");
            }
        }

        const data = await response.json();

        if (data.success) {
            console.log(data);
            showEmailSuccess(data.message);
            enviarEmail();
        } else {
            const successMessagem = document.getElementById('email-error');
            if (successMessagem) {
                successMessagem.style.display = 'none';
            }
            showError('email-error', 'E-mail não encontrado.', email.id);

        }
    } catch (error) {
        console.error("Erro ao verificar e-mail:", error, email.id);
        showError('email-error', error.message || "Erro ao verificar e-mail. Tente novamente.",email.id);
    }
}

function showEmailSuccess(message) {
    const successMessage = document.getElementById('email-error');
    if (successMessage) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = '#155724';
        successMessage.style.backgroundColor = '#d4edda';
        successMessage.style.padding = '0.75rem';
        successMessage.style.borderRadius = '8px';
        successMessage.style.border = '1px solid #c3e6cb';
    }
}
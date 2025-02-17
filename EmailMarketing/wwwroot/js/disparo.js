document.addEventListener('DOMContentLoaded', function () {
    carregarPromocoes();
});

async function carregarPromocoes() {
    const token = localStorage.getItem('token');

    try {
        const response = await fetch('https://localhost:7103/api/Promocoes', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Erro ao carregar promoções.');
        }

        const data = await response.json();
        const promocoes = data.data; 
        const selectPromocao = document.getElementById('promocao');

        selectPromocao.innerHTML = '<option value="">Selecione uma promoção</option>';

        promocoes.forEach(promocao => {
            const option = document.createElement('option');
            option.value = promocao.id;
            option.textContent = promocao.nome;
            selectPromocao.appendChild(option);
        });
    } catch (error) {
        console.error('Erro:', error);
        showDisparoError('Erro ao carregar promoções. Tente novamente.');
    }
}

document.getElementById('disparoForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const assunto = document.getElementById('assunto').value;
    const corpoEmail = document.getElementById('corpoEmail').value;
    const promocaoId = document.getElementById('promocao').value;
    const imagemPromocao = document.getElementById('imagemPromocao').files[0];


    if (!assunto || !corpoEmail || !promocaoId || !imagemPromocao) {
        showDisparoError('Preencha todos os campos.');
        return;
    }

    const token = localStorage.getItem('token');

    const formData = new FormData();
    formData.append('assunto', assunto);
    formData.append('corpoEmail', corpoEmail);
    formData.append('promocaoId', promocaoId);
    if (imagemPromocao) {
        formData.append('imagemPromocao', imagemPromocao);
    }

    try {
        const response = await fetch('https://localhost:7103/api/Disparo', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
            body: formData
        });

        if (!response.ok) {
            throw new Error('Erro ao disparar e-mails.');
        }

        const data = await response.json();
        showDisparoSuccess(data.message || 'E-mails disparados com sucesso!');
    } catch (error) {
        console.error('Erro:', error);
        showDisparoError(error.message || 'Erro ao disparar e-mails. Tente novamente.');
    }
});

function showDisparoError(message) {
    const errorMessage = document.getElementById('disparoErrorMessage');
    if (errorMessage) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
        errorMessage.style.color = '#dc3545';
    }
}

function showDisparoSuccess(message) {
    const successMessage = document.getElementById('disparoStatus');
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
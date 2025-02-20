document.addEventListener('DOMContentLoaded', function () {
    carregarArquivos();
});

async function carregarArquivos() {
    const token = localStorage.getItem('token');

    try {
        const response = await fetch('https://localhost:7103/api/Clientes/ArquivoNome', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });

        if (!response.ok) {
            throw new Error('Erro ao carregar arquivos.');
        }

        const arquivos = await response.json();
        console.log(arquivos);  
        const selectArquivos = document.getElementById('arquivo');

        selectArquivos.innerHTML = '<option value="">Selecione um arquivo</option>';

        arquivos.cliente.forEach(arquivo => {
            const option = document.createElement('option');
            option.value = arquivo;
            option.textContent = arquivo;
            selectArquivos.appendChild(option);
        });

        showDisparoSuccess(arquivos.message);
    } catch (error) {
        console.error('Erro:', error);
        showDisparoError('Erro ao carregar arquivos. Tente novamente.');
    }
}

document.getElementById('arquivoForm').addEventListener('submit', async function (e) {
    e.preventDefault();

    const token = localStorage.getItem('token');

    const arquivoId = document.getElementById('arquivo').value;

    if (!arquivoId) {
        showDisparoError('Escolha um arquivo.');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7103/api/Clientes/PorArquivoNome?arquivoNome=${arquivoId}`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
            },
        });

        if (!response.ok) {
            throw new Error('Erro ao buscar os clientes.');
        }

        const data = await response.json();
        console.log(data);  
        showDisparoSuccess(data.message || 'Clientes buscados com sucesso!');
        preencherTabelaClientes(data.cliente);
    } catch (error) {
        console.error('Erro:', error);
        showDisparoError(error.message || 'Erro ao buscar os clientes. Tente novamente.');
    }
});

function showDisparoError(message) {
    const errorMessage = document.getElementById('buscaErrorMessage');
    if (errorMessage) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
        errorMessage.style.color = '#dc3545';
    }
}

function showDisparoSuccess(message) {
    const successMessage = document.getElementById('buscaStatus');
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

function preencherTabelaClientes(clientes) {
    const tbody = document.querySelector('.clientes-table tbody');
    tbody.innerHTML = '';

    clientes.forEach(cliente => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${cliente.id}</td>
            <td>${cliente.nome}</td>
            <td>${cliente.email}</td>
            <td>${cliente.promocaoId}</td>
        `;
        tbody.appendChild(row);
    });
}






document.getElementById('uploadForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];

    if (!file) {
        showError('Por favor, selecione um arquivo.');
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    const token = localStorage.getItem('token');

    fetch('https://localhost:7103/api/Upload', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: formData,
    })
        .then(response => {
            if (!response.ok) {
                // Handle HTTP errors (e.g., 401 Unauthorized, 400 Bad Request, etc.)
                if (response.status === 401) {
                    throw new Error('Você não está autorizado. Faça login novamente.');
                } else if (response.status === 400) {
                    return response.json().then(data => {
                        throw new Error(data.message || 'Erro ao processar o arquivo.');
                    });
                } else {
                    throw new Error(`Erro na requisição: ${response.statusText}`);
                }
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                console.log(data.success);
                console.log(data.message);
                showSuccess(data.message); // Display success message
            } else {
                showError(data.message || 'Erro ao processar o arquivo.');
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            showError(error.message || 'Erro ao enviar o arquivo. Tente novamente.');
        });
});

function showError(message) {
    const errorMessage = document.getElementById('errorMessage');
    if (errorMessage) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
        errorMessage.style.color = 'red';
    }
}

function showSuccess(message) {
    const successMessage = document.getElementById('successMessage');
    if (successMessage) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = 'green';
    }
}
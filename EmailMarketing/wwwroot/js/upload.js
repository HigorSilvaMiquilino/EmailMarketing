document.getElementById('uploadForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];

    if (!file) {
        showError('Por favor, selecione um arquivo.');
        return;
    }

    const extensoesPermitidas = ['.csv', '.xlsx', '.sql'];
    const fileExtension = file.name.substring(file.name.lastIndexOf('.')).toLowerCase();

    if (!extensoesPermitidas.includes(fileExtension)) {
        showError('Formato de arquivo inválido. Por favor, selecione um arquivo CSV, xlsx ou SQL.');
        return;
    }

    const formData = new FormData();
    formData.append('file', file);

    const token = localStorage.getItem('token');

    let url;

    if (fileExtension === '.csv' || fileExtension === '.xlsx') {
        url = 'https://localhost:7103/api/Upload/UploadCsv';
    } else if (fileExtension === '.sql') {
        url = 'https://localhost:7103/api/Upload/UploadSql';
    }

     

    fetch(url, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: formData,
    })
        .then(response => {
            if (!response.ok) {
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
                showSuccess(data.message); 
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
    const errorMessage = document.getElementById('fileErrorMessage');
    if (errorMessage) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
        errorMessage.style.color = 'red';
    }
}

function showSuccess(message) {
    const errorMessage = document.getElementById('fileErrorMessage');
    if (errorMessage) {
        errorMessage.style.display = 'none';
    }

    const successMessage = document.getElementById('uploadStatus');
    if (successMessage) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = 'green';
    }
}


document.getElementById('promocaoForm').addEventListener('submit', function (e) {
    e.preventDefault();

    const nomePromocao = document.getElementById('nomePromocao');
    const descricaoPromocao = document.getElementById('descricaoPromocao');
    const dataInicio = document.getElementById('dataInicio');
    const dataTermino = document.getElementById('dataTermino');

    let isValid = true;

    if (!validateNamePromocao(nomePromocao.value)) {
        showPromocaoError('O nome da promoção é obrigatório.', 'nomeErrorMessage' , nomePromocao.id);
        isValid = false;
    }

    if (!descricaoPromocao.value) {
        showPromocaoError('A descrição da promoção é obrigatória.', 'textareaErrorMessage', descricaoPromocao.id);
        isValid = false;
    }

    if (!dataInicio.value) {
        showPromocaoError('A data de início é obrigatória.', 'dataInicioErrorMessage', dataInicio.id);
        isValid = false;
    }

    if (!dataTermino.value) {
        showPromocaoError('A data de término é obrigatória.', 'dataTerminoErrorMessage', dataTermino.id);
        isValid = false;
    }

    const dataInicioObj = new Date(dataInicio.value);
    const dataTerminoObj = new Date(dataTermino.value);

    if (isNaN(dataInicioObj) || isNaN(dataTerminoObj)) {
        showPromocaoError('As datas inseridas são inválidas.', 'dataTerminoErrorMessage', 'promocaoStatus');
        isValid = false;
    }

    if (dataTerminoObj <= dataInicioObj) {
        showPromocaoError('A data de término deve ser posterior à data de início.', 'dataTerminoErrorMessage', 'promocaoStatus');
        isValid = false;
    }

    if (!isValid) {
        return;
    }

    const promocaoData = {
        Nome: nomePromocao.value,
        Descricao: descricaoPromocao.value,
        DataInicio: dataInicio.value,
        DataTermino: dataTermino.value
    };

    const token = localStorage.getItem('token');

    fetch('https://localhost:7103/api/Promocoes', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(promocaoData)
    })
        .then(response => {
            if (!response.ok) {
                if (response.status === 401) {
                    throw new Error('Você não está autorizado. Faça login novamente.');
                } else if (response.status === 400) {
                    return response.json().then(data => {
                        throw new Error(data.message || 'Erro ao cadastrar a promoção.');
                    });
                } else {
                    throw new Error(`Erro na requisição: ${response.statusText}`);
                }
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                showPromocaoSuccess(data.message); 
            } else {
                showPromocaoError(data.message || 'Erro ao cadastrar a promoção.');
            }
        })
        .catch(error => {
            console.error('Erro:', error);
            showPromocaoError(error.message || 'Erro ao cadastrar a promoção. Tente novamente.');
        });
});

function showPromocaoError(message, messageDiv, campo) {
    const errorMessage = document.getElementById(messageDiv);
    if (errorMessage) {
        errorMessage.textContent = message;
        errorMessage.style.display = 'block';
        errorMessage.style.color = 'red';
    }
    const errorInput = document.getElementById(campo);
    if (errorInput) {
        errorInput.style.borderColor = "red";
    }
}


function showPromocaoSuccess(message) {
    const successMessage = document.getElementById('promocaoStatus');
    if (successMessage) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = 'green';
    }
}

function validateNamePromocao(name) {
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

document.addEventListener('DOMContentLoaded', function () {
    const inputs = document.querySelectorAll('.campoEntrada');
    inputs.forEach(input => {
        input.addEventListener('focus', function (e) {
            e.target.style.border = '1px solid #007bff';
            const errorElement = input.parentElement.querySelector('.error-message');
            if (errorElement) {
                errorElement.style.display = 'none';
            }
        });

        input.addEventListener('blur', function (e) {
            e.target.style.border = '1px solid #ced4da';
        });

        input.addEventListener('input', function (e) {
            e.target.value = e.target.value.trimStart();
    
        });
    });
});

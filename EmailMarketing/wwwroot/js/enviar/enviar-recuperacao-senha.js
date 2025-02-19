export function enviarRecuperacaoSenha() {
    const formElement = document.querySelector('.recuperacao-form');

    const formData = new FormData(formElement);
    const email = localStorage.getItem('email');

    const cliente = {
        Email: email,
        Senha: formData.get('password'),
    }

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch("https://localhost:7103/api/Recuperacao/recuperar", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token

        },
        body: JSON.stringify(cliente),
        credentials: 'include'
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error("Erro na requisição");
            }
            return response.json();
        })
        .then((data) => {
            console.log(data);
            if (data.success) {
                console.log(data);
                showEmailSuccess(data.message);
            } else {
                console.log("Erro: " + JSON.stringify(data.errors));
            }
        })
        .catch((error) => {
            console.error("Error:", error);
        });
}

function showEmailSuccess(message) {
    const successMessage = document.getElementById('form-links');
    if (successMessage) {

        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = '#155724';
        successMessage.style.backgroundColor = '#d4edda';
        successMessage.style.padding = '0.75rem';
        successMessage.style.borderRadius = '8px';
        successMessage.style.border = '1px solid #c3e6cb';
        successMessage.style.marginTop = '20px'; 
    }
}
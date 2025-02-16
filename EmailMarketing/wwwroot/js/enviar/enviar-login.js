export function enviarLogin() {
    const formElement = document.querySelector('.login-form');
    const formData = new FormData(formElement);

    const cliente = {
        Email: formData.get('email'),
        Senha: formData.get('password'),
    };

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch("https://localhost:7103/api/Auth/login", {
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
                return response.json().then(errorData => {
                    throw new Error(errorData.message || "Erro na requisição");
                });
            }
            return response.json();
        })
        .then((data) => {
            if (data.token) {
                localStorage.setItem('token', data.token);
                window.location.href = "/html/carregar.html";
            } else {
                showError('password-error', data.message || "Erro ao realizar o login.");
            }
        })
        .catch((error) => {
            console.error("Error:", error);
            showError('password-error', error.message || "Erro ao realizar o login. Tente novamente.");
        });
}

function showError(elementId, message) {
    const errorElement = document.getElementById(elementId);
    errorElement.textContent = message;
    errorElement.style.display = 'block';
}

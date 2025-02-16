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
                throw new Error("Erro na requisição");
            }
            return response.json();
        })
        .then((data) => {
            if (data.token) {
                localStorage.setItem('token', data.token); // Armazena o token
                window.location.href = "/html/carregar.html"; // Redireciona para a página protegida
            }
        })
        .catch((error) => {
            console.error("Error:", error);
        });
}
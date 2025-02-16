export function enviarCadastro() {
    const formElement = document.querySelector('.signup-form');

    const formData = new FormData(formElement);

    const cliente = {
        Nome: formData.get('name'),
        Email: formData.get('email'),
        Senha: formData.get('password'),
    }

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch("https://localhost:7103/api/Home/Registrar", {
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
            if (data.success) {
                    localStorage.setItem('token', data.token);
                    window.location.href = "/html/carregar.html"; 
            } else {
                console.log("Erro: " + JSON.stringify(data.errors));
            }
        })
        .catch((error) => {
            console.error("Error:", error);
        });
}


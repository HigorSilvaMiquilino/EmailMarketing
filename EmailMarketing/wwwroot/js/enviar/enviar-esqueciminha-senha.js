 

export function enviarEmail() {
    const formElement = document.querySelector('.recovery-form');
    const formData = new FormData(formElement);

    const email = formData.get('email').trim();
    localStorage.setItem('email', email);

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    fetch(`/api/Recuperacao/disparo?email=${encodeURIComponent(email.trim())}`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
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
                Console.log(data);
                
            } else {
                showError('email-error', data.message || "Erro ao buscar o e-mail.");
            }
        })
        .catch((error) => {
            console.error("Error:", error);
            showError('email-error', error.message || "Erro ao buscar o e-mail. Tente novamente.");
        });
}

function showError(elementId, message) {
    const errorElement = document.getElementById(elementId);
    errorElement.textContent = message;
    errorElement.style.display = 'block';
}

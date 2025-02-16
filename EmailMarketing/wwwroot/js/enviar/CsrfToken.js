export async function CsrfToken() {
    try {
        const response = await fetch("https://localhost:7103/api/Token/GetCsrfToken", {
            method: "GET",
            credentials: 'include'
        });
        const data = await response.json();
        document.querySelector('input[name="__RequestVerificationToken"]').value = data.token;
    } catch (error) {
        console.error("Erro ao obter o token CSRF:", error);
    }
}
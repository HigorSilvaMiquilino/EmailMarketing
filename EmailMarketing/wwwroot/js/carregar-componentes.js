import { renderHeader } from "./componentes/header.js";
import { renderFooter } from "./componentes/footer.js";

document.addEventListener('DOMContentLoaded', function () {
    const header = renderHeader();
    document.body.prepend(header); 

    const footer = renderFooter();
    document.body.append(footer); 

    const comeceAgoraBtn = document.getElementById('comeceAgora');
    if (comeceAgoraBtn) {
        const token = localStorage.getItem('token');
        if (token) {
            comeceAgoraBtn.href = "/html/carregar.html";
            comeceAgoraBtn.textContent = "Acessar Campanhas";
        } else {
            comeceAgoraBtn.href = "/html/login.html";
            comeceAgoraBtn.textContent = "Comece Agora"; 
        }
    }
});
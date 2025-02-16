export function renderHeader() {
    const header = document.createElement('header');
    const token = localStorage.getItem('token');

    const menuItems = token ? `
        <li><a href="/html/disparo.html">Disparo</a></li>
        <li><a href="/html/carregar.html">Carregar</a></li>
        <li><a href="/html/estatistica.html">Estatísticas</a></li>
        <li><a href="/html/logout.html" class="btn-login">Logout</a></li>
    ` : `
        <li><a href="/html/login.html" class="btn-login">Login</a></li>
    `;

    header.innerHTML = `
        <nav class="navbar">
            <div class="container">
                <a href="#" class="logo">
                    <img src="/assets/imagens/logo.png" alt="Fenix System">
                </a>
                <button class="navbar-toggle" aria-label="Abrir menu">
                    <span></span>
                    <span></span>
                    <span></span>
                </button>
                <ul class="navbar-menu">
                    <li><a href="/html/index.html" class="active">Início</a></li>
                    <li><a href="/html/index.html">Funcionalidades</a></li>
                    ${menuItems}
                </ul>
            </div>
        </nav>
    `;
    return header;
}
document.addEventListener('DOMContentLoaded', function () {

    function carregarComponentes(seletor, arquivo) {

        fetch(arquivo)
            .then(response => response.text())
            .then(html => {
               document.querySelector(seletor).innerHTML = html;
            })
            .catch(error => console.error(`erro ao carregar ${arquivo}: `, error));
    }

    carregarComponentes("#header", "/html/componentes/header.html");
    carregarComponentes("#footer", "/html/componentes/footer.html");
});
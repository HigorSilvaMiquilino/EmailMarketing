document.addEventListener('DOMContentLoaded', function () {
    const labels = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul'];
    const emailsEnviados = [120, 150, 180, 200, 220, 250, 300];
    const emailsComErro = [5, 10, 8, 12, 15, 20, 18];
    const emailsAbertos = [100, 130, 160, 180, 200, 220, 250];
    const cliques = [80, 100, 120, 150, 180, 200, 220];

    const commonOptions = {
        responsive: true, 
        maintainAspectRatio: false, 
        scales: {
            y: {
                beginAtZero: true
            }
        }
    };

    const emailsEnviadosChart = new Chart(document.getElementById('emailsEnviadosChart'), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'E-mails Enviados',
                data: emailsEnviados,
                backgroundColor: 'rgba(54, 162, 235, 0.6)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: commonOptions
    });

    const emailsComErroChart = new Chart(document.getElementById('emailsComErroChart'), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'E-mails com Erro',
                data: emailsComErro,
                backgroundColor: 'rgba(255, 99, 132, 0.6)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        },
        options: commonOptions
    });

    const emailsAbertosChart = new Chart(document.getElementById('emailsAbertosChart'), {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'E-mails Abertos',
                data: emailsAbertos,
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 2,
                fill: false
            }]
        },
        options: commonOptions
    });

    const cliquesChart = new Chart(document.getElementById('cliquesChart'), {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Cliques nos Botões',
                data: cliques,
                borderColor: 'rgba(153, 102, 255, 1)',
                borderWidth: 2,
                fill: false
            }]
        },
        options: commonOptions
    });
});
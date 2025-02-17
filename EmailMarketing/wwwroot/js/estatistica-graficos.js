document.addEventListener('DOMContentLoaded', async function () {
    async function fetchData(url) {
        const token = localStorage.getItem('token');
        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        if (!response.ok) {
            throw new Error('Erro ao buscar dados');
        }
        return await response.json();
    }

    function getMonthName(month) {
        const months = ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"];
        return months[month - 1];
    }

    try {
        const logsEnvio = await fetchData('https://localhost:7103/api/Logs/envio');
        const logsAbertura = await fetchData('https://localhost:7103/api/Logs/abertura');
        const logsClique = await fetchData('https://localhost:7103/api/Logs/clique');
        const logsErro = await fetchData('https://localhost:7103/api/Logs/erro');

        console.log('Logs Envio:', logsEnvio);

        const labels = Array.from({ length: 12 }, (_, i) => getMonthName(i + 1));
        const dataEnvio = Array(12).fill(0);
        const dataAbertura = Array(12).fill(0);
        const dataClique = Array(12).fill(0);
        const dataErro = Array(12).fill(0);

        console.log('1°  Data Envio:', dataEnvio);

        logsEnvio.forEach(log => {
            const mesIndex = parseInt(log.mes) - 1;
            if (!isNaN(mesIndex) && mesIndex >= 0 && mesIndex < 12) {
                dataEnvio[mesIndex] = parseInt(log.total);
            }
        });

        logsAbertura.forEach(log => {
            const mesIndex = parseInt(log.mes) - 1;
            if (!isNaN(mesIndex) && mesIndex >= 0 && mesIndex < 12) {
                dataAbertura[mesIndex] = parseInt(log.total);
            }
        });

        logsClique.forEach(log => {
            const mesIndex = parseInt(log.mes) - 1;
            if (!isNaN(mesIndex) && mesIndex >= 0 && mesIndex < 12) {
                dataClique[mesIndex] = parseInt(log.total);
            }
        });

        logsErro.forEach(log => {
            const mesIndex = parseInt(log.mes) - 1;
            if (!isNaN(mesIndex) && mesIndex >= 0 && mesIndex < 12) {
                dataErro[mesIndex] = parseInt(log.total);
            }
        });

        console.log('2° Data Envio:', dataEnvio);

        const commonOptions = {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        };

        new Chart(document.getElementById('emailsEnviadosChart'), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'E-mails Enviados',
                    data: dataEnvio,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: commonOptions
        });

        new Chart(document.getElementById('emailsComErroChart'), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'E-mails com Erro',
                    data: dataErro,
                    backgroundColor: 'rgba(255, 99, 132, 0.6)',
                    borderColor: 'rgba(255, 99, 132, 1)',
                    borderWidth: 1
                }]
            },
            options: commonOptions
        });

        new Chart(document.getElementById('emailsAbertosChart'), {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'E-mails Abertos',
                    data: dataAbertura,
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 2,
                    fill: false
                }]
            },
            options: commonOptions
        });

        new Chart(document.getElementById('cliquesChart'), {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Cliques nos Botões',
                    data: dataClique,
                    borderColor: 'rgba(153, 102, 255, 1)',
                    borderWidth: 2,
                    fill: false
                }]
            },
            options: commonOptions
        });
    } catch (error) {
        console.error('Erro ao carregar dados:', error);
    }
});
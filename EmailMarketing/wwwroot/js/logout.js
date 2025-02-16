document.addEventListener('DOMContentLoaded', function () {
    const countdownElement = document.getElementById('countdown');
    let countdown = 5;

    localStorage.removeItem('token');

    const interval = setInterval(() => {
        countdown--;
        countdownElement.textContent = countdown;

        if (countdown === 0) {
            clearInterval(interval);
            window.location.href = "/html/index.html";
        }
    }, 1000);
});
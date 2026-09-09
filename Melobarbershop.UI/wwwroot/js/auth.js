document.querySelectorAll(".auth-eye").forEach(button => {
    button.addEventListener("click", () => {
        const input = document.getElementById(button.dataset.target);
        if (input) input.type = input.type === "password" ? "text" : "password";
    });
});

const loginForm = document.getElementById("loginForm");
if (loginForm) {
    loginForm.addEventListener("submit", event => {
        event.preventDefault();
        const message = document.getElementById("authMessage");
        if (message) message.textContent = "Login demonstrativo realizado!";
    });
}

const forgotPassword = document.getElementById("forgotPassword");
if (forgotPassword) {
    forgotPassword.addEventListener("click", event => {
        event.preventDefault();
        const message = document.getElementById("authMessage");
        if (message) message.textContent = "A recuperação de senha estará disponível na versão conectada ao backend.";
    });
}

document.querySelectorAll(".auth-eye").forEach(button => {
    button.addEventListener("click", () => {
        const input = document.getElementById(button.dataset.target);
        if (input) input.type = input.type === "password" ? "text" : "password";
    });
});

const loginForm = document.getElementById("loginForm");
if (loginForm) {
    loginForm.addEventListener("submit", async event => {
        event.preventDefault();

        const emailInput = document.getElementById("email");
        const passwordInput = document.getElementById("password");
        const message = document.getElementById("authMessage");
        const submitBtn = loginForm.querySelector("button[type='submit']");

        const email = emailInput ? emailInput.value.trim() : "";
        const senha = passwordInput ? passwordInput.value : "";

        if (!email || !senha) {
            if (message) {
                message.textContent = "Por favor, preencha o e-mail e a senha.";
                message.className = "auth-message error";
            }
            return;
        }

        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.textContent = "Entrando...";
        }

        if (message) {
            message.textContent = "";
            message.className = "auth-message";
        }

        try {
            const response = await fetch("/Auth/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },
                body: JSON.stringify({ email, senha })
            });

            const data = await response.json().catch(() => null);

            if (response.ok && data && data.sucesso) {
                if (data.token) {
                    try {
                        localStorage.setItem("melo_token", data.token);
                        localStorage.setItem("melo_user", JSON.stringify({
                            nome: data.nome,
                            roles: data.roles
                        }));
                    } catch (e) {
                        // Não interrompe caso o navegador bloqueie storage
                    }
                }

                if (message) {
                    message.textContent = data.mensagem || "Login realizado com sucesso! Redirecionando...";
                    message.className = "auth-message success";
                }

                const urlParams = new URLSearchParams(window.location.search);
                const returnUrl = urlParams.get("returnUrl");
                const destination = (returnUrl && returnUrl.startsWith("/")) ? returnUrl : (data.redirectUrl || "/Home/Index");
                setTimeout(() => {
                    window.location.href = destination;
                }, 600);
            } else {
                let errorMsg = "E-mail ou senha incorretos.";
                if (data && data.mensagem) {
                    errorMsg = data.mensagem;
                } else if (response.status === 401) {
                    errorMsg = "Email ou senha inválidos.";
                } else if (response.status === 403) {
                    errorMsg = "Acesso não autorizado.";
                } else if (response.status >= 500) {
                    errorMsg = "Serviço indisponível ou erro no servidor. Tente novamente mais tarde.";
                }

                if (message) {
                    message.textContent = errorMsg;
                    message.className = "auth-message error";
                }
            }
        } catch (err) {
            if (message) {
                message.textContent = "Falha de conexão com o servidor. Verifique sua internet.";
                message.className = "auth-message error";
            }
        } finally {
            if (submitBtn) {
                submitBtn.disabled = false;
                submitBtn.textContent = "Entrar";
            }
        }
    });
}

const forgotPassword = document.getElementById("forgotPassword");
if (forgotPassword) {
    forgotPassword.addEventListener("click", event => {
        event.preventDefault();
        const message = document.getElementById("authMessage");
        if (message) {
            message.textContent = "A recuperação de senha estará disponível em breve.";
            message.className = "auth-message";
        }
    });
}


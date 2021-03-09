$(document).ready(function () {
    let signupRef = document.getElementById("signupRef");
    signupRef.href = "signup.html"
});

function clearErrors() {
    let elements = document.getElementsByClassName("errorMessage");
    while (elements.length > 0) elements[0].remove();
}

$("#login").click(function () {
    login();
});

function login() {
    let email = document.getElementById("email");
    let password = document.getElementById("password");

    let loginModel = {email: email.value, password: password.value};
    $.ajax({
        method: "POST",
        url: "https://localhost:44339/api/Authentication/token/",
        data: JSON.stringify(loginModel),
        contentType: "application/json",
        success: function (token) {
            sessionStorage.setItem("token", token);
            sessionStorage.setItem("decryptedToken", JSON.stringify(jwt_decode(token)));
            window.location.href = "sending.html";
        },
        error: function (response) {
            clearErrors();
            let errors = response.responseText;
            if (errors === undefined) {return}
            if (errors.includes("Too many incorrect attempts. Please wait 5 minutes and try again."))
            {
                $("<span class='alert-danger errorMessage'>U heeft te vaak incorrect aangemeld. Probeer binnen 5 minuten opnieuw.</span>").insertAfter(password);
            }
            if (errors.includes("The Password field is required.")) {
                $("<span class='alert-danger errorMessage'>Vul een wachtwoord in!</span>").insertAfter(password);
            }
            if (errors.includes("Unauthorized"))
            {
                $("<span class='alert-danger errorMessage'>Deze combinatie is incorrect! Controleer uw e-mail en wachtwoord.</span>").insertAfter(password);
            }
        }
    });
}
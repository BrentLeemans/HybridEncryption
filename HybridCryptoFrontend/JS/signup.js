$(document).ready(function () {
    let loginRef = document.getElementById("loginRef");
    loginRef.href = "login.html"
});

function clearErrors() {
    let elements = document.getElementsByClassName("errorMessage");
    while (elements.length > 0) elements[0].remove();
}

function handleValidation()
{
    clearErrors();
    let nickname = document.getElementById("username");
    let email = document.getElementById("email");
    let password = document.getElementById("password");
    let re_password = document.getElementById("re_password");

    if (nickname.value.length === 0)
    {
        $("<span class='alert-danger errorMessage'>Fill in your username!</span>").insertAfter(nickname);
    }

    if (email.value.length === 0)
    {
        $("<span class='alert-danger errorMessage'>Fill in your email!</span>").insertAfter(email);
    }else {
        if (!validator.isEmail(email.value))
        {
            $("<span class='alert-danger errorMessage'>This email is not valid!</span>").insertAfter(email);
        }
    }

    if (password.value.length === 0)
    {
        $("<span class='alert-danger errorMessage'>Fill in your password!</span>").insertAfter(password);
    } else {
        if (password.value.length < 6) {
            $("<span class='alert-danger errorMessage'>The password must contain at least 6 characters!</span>").insertAfter(password);
        } else {
            if (!/[a-z]/.test(password.value)) {
                $("<span class='alert-danger errorMessage'>The password must contain at least 1 lower case character!</span>").insertAfter(password);
                $("<span class='alert-danger errorMessage'>The password must contain at least 1 lower case character!</span>").insertAfter(re_password);
            }
            else {
                if (!/\d/.test(password.value)){
                    $("<span class='alert-danger errorMessage'>The password must contain at least 1 digit!</span>").insertAfter(password);
                }
            }
        }
    }

    if (re_password.value.length === 0)
    {
        $("<span class='alert-danger errorMessage'>Repeat your password!</span>").insertAfter(re_password);
    }else {
        if (password.value !== re_password.value) {
            $("<span class='alert-danger errorMessage'>The passwords don't match!</span>").insertAfter(re_password);
        }
    }

    let elements = document.getElementsByClassName("errorMessage");
    if (elements.length === 0) {
        let registerModel = {nickname: nickname.value, email:email.value, password:password.value};

        register(registerModel);
    }
}

$("#submit").click(function () {
    handleValidation();
});

function register(data) {
    $.ajax({
        method: "POST",
        url: "https://localhost:44339/api/Authentication/register/",
        data: JSON.stringify(data),
        contentType: "application/json",
        dataType: "json",
        success: function () {
            console.log("Successfully submitted.");
            window.location.href = "login.html"
        },
        error: function (response) {
            let errors = response.responseText;
            console.log(errors);
            if (errors.includes("DuplicateEmail")) {
                $("<span class='alert-danger errorMessage'>Dit e-mailadres is al in gebruik!</span>").insertAfter(document.getElementById("email"));
            }
        }
    });
}
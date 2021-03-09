let receiverId;
let decryptedToken;
let senderId;
let fileInBytes = null;

$(document).ready(function () {
    getAllReceivers();
    senderId = JSON.parse(sessionStorage.getItem("decryptedToken"))["UserId"];
});

$("#send").click(function () {
    fillMessage();
});

$("#logout").click(function () {
    sessionStorage.clear();
    window.location.href = "login.html"
});

$("#receiver").change(function () {
    receiverId = $("#receiver").val();
});

function emptyFields() {
    $("#customFile").val("");
    $("#customFileLabel").text("Choose file");
    $("#message")[0].value = "";
    fileInBytes = null;
}

function fillMessage() {
    let text = document.getElementById("message").value;
    let file = document.getElementById("customFile");
    file = file.files;
    if (file.length === 0) {
        file = null;
        sendMessage(text, null);
    } else {
        file = file[0];
        if (file.size === 0) {
            emptyFields();
            alert("The file is empty! Fill in something.");
        } else {
            let fileReader = new FileReader();
            fileReader.onload = function (e) {
                fileInBytes = Object.values(new Int8Array(e.target.result));
                sendMessage(text, fileInBytes);
            };
            fileReader.readAsArrayBuffer(file);
        }
    }
}

function sendMessage(text, fileInBytes) {
    let data = {senderId: senderId, receiverId: receiverId, text: text, file: fileInBytes};
    console.log(JSON.stringify(data));
    $.ajax({
        method: "POST",
        url: "https://localhost:44339/api/Messages",
        data: JSON.stringify(data),
        processData: false,
        contentType: "application/json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem("token"));
        },
        success: function () {
            console.log("Successfully submitted.");
            emptyFields();
        },
        error: function (response) {
            let errors = response.responseText;
            if (errors.includes("The ReceiverId field is required."))
            {
                alert("Select a receiver!");
            }
            console.error(response.responseText);
        }
    });
}

function getAllReceivers() {
    $.ajax({
        method: "GET",
        type: "jsonp",
        url: "https://localhost:44339/api/Users/",
        success: function (data) {
            createOptions(data);
        },
        error: (function (data) {
            alert("Error " + data.status);
        })
    });
}

function createOptions(users) {
    let select = document.getElementById("receiver");
    users.forEach(u => {
        if (u.id !== senderId) {
            let option = document.createElement("option");
            option.text = u.nickname + " - " + u.email;
            option.value = u.id;
            select.add(option);
        }
    });
    select.selectedIndex = -1;
}

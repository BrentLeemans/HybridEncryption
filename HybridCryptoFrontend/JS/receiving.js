let receiverId;
let messageCounter = 0;
$(document).ready(function () {
    getAllSenders();
    receiverId = JSON.parse(sessionStorage.getItem("decryptedToken"))["UserId"];
    getAllMessages();
});

$("#logout").click(function () {
    sessionStorage.clear();
    window.location.href = "login.html"
});

function createMessages(messages) {
    messages.slice().reverse().forEach(m =>
    {
        let fileButton = `<a class="btn-lg hasfile btn-primary active mt-1" href="data:text/plain;charset=utf-8,${encodeURIComponent(atob(m.file))}" download="secretFile">Download</a>\n`;
        //let fileButton = `<button download class=\"btn-lg hasfile btn-primary active mt-1\"><i class=\"fa fa-download\"></i>Download File</button>\n`;
        if (m.file === "") fileButton = `<button class=\"btn btn-secondary mt-1 btn-lg\" disabled>No file available</button>\n`;

        --messageCounter;
        let message =
            `<div class=\"container mb-5 d-inline-block\">\n` +
                `<div class=\"row\">\n` +
                    `<div class=\"col text-left\">\n` +
                        `<h3>Message ${messageCounter}</h3>` +
                    `</div>\n` +
                    `<div class=\"col text-right\">` +
                        `<h3>${dateFormat(m.date, "dd-mm | hh:MM")}</h3>\n` +
                    `</div>\n` +
                `</div>\n` +

                `<div class=\"row\">\n` +
                    `<textarea id=\"message\" class=\"form-control rounded-0\" rows=\"10\" disabled>${m.text}</textarea>\n` +
                `</div>\n` +
                `<div class=\"row float-right\">\n` +
                     fileButton +
                `</div>\n`;
            `</div>`;
        $(message).appendTo($("#allMessages"));
    });
}

$("#send").click(function () {
    sendMessage();
});

$("#sender").change(function () {
    $("#allMessages").empty();
    senderId = $("#sender").val();
    getAllMessages(senderId);
});

function getAllMessages(senderId) {
    $.ajax({
        method: "GET",
        type: "jsonp",
        url: "https://localhost:44339/api/Messages/" + "df31643e-955e-4bc0-9f77-5ae273748e67",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", 'Bearer ' + sessionStorage.getItem("token"));
        },
        success: function (data) {
            messageCounter = data.length +1;
            console.log(data);
            createMessages(data);
        },
        error: (function (data) {
            alert("Error " + data.status);
        })
    });
}

function getAllSenders() {
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
    let select = document.getElementById("sender");
    users.forEach(u =>
    {
        if (u.id !== receiverId)
        {
            let option = document.createElement("option");
            option.text = u.nickname + " - " + u.email;
            option.value = u.id;
            select.add(option);
        }
    });
    select.selectedIndex = -1;
}

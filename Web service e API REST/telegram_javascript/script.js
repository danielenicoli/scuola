function inviaMessaggio() {
    var esito = document.getElementById("esito");
    var messaggio = document.getElementById("messaggio").value;
    var botToken = "";
    var chatId = "";
    var endpoint = "https://api.telegram.org/bot" + botToken + "/sendMessage";

    fetch(endpoint, {
        method: "POST",
        body: JSON.stringify({
            chat_id: chatId,
            text: messaggio
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    })
        .then(response => {
            if (response.status == 200) {
                esito.innerHTML = "Messaggio inviato con successo";
            }
        })
        .catch(error => {
            alert("Errore: " + error);
        });
}
function getMeteo() {
    var paese = document.getElementById("paese").value;
    var endpoint = "https://api.openweathermap.org/data/2.5/weather?q=" + paese + ",it&units=metric&APPID=XXXXXXXXXXXX";

    fetch(endpoint)
        .then(risposta => {
            if(risposta.status == 200) {
                return risposta.json();
            }
        })
        .then(body => {
            console.log(body);
            document.getElementById("condizione").innerHTML = "Condizioni attuali a " + paese;
            document.getElementById("temperatura").innerHTML = body.main.temp + "°C";
            document.getElementById("umidita").innerHTML = body.main.humidity + "%";
        })
        .catch(errore => {
            alert("Errore: " + errore);
        })
}
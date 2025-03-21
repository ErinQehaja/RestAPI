async function addNumbersWithAPI() {
    const number1 = parseInt(document.getElementById("number1").value);
    const number2 = parseInt(document.getElementById("number2").value);
    const result = document.getElementById("result");

    if (isNaN(number1) || isNaN(number2)) {
        result.innerText = "Ungültige Eingabe!";
        return;
    }

    try {
        const response = await fetch("https://localhost:7112/WeatherForecast/add", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Number1: number1, Number2: number2 })
        });
        result.innerText = await response.json();
    } catch {
        result.innerText = "Fehler!";
    }
}
async function addNumbersWithAPI() {
    const input1 = parseInt(document.getElementById("input1").value);
    const input2 = parseInt(document.getElementById("input2").value);
    
    if (isNaN(input1) || isNaN(input2)) {
        document.getElementById("result").innerText = "Bitte gültige Zahlen eingeben!";
        return;
    }
    
    const data = {
        Input1: input1,
        Input2: input2
    };
    
    try {
        const response = await fetch("https://localhost:7112/WeatherForecast/add", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(data)
        });
        
        if (!response.ok) {
            throw new Error("Fehler bei der API-Anfrage");
        }
        
        const result = await response.json();
        document.getElementById("result").innerText = result;
    } catch (error) {
        document.getElementById("result").innerText = "Fehler: " + error.message;
    }
}
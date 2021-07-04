// WebSocket = undefined;
//EventSource = undefined;
//, signalR.HttpTransportType.LongPolling

let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/ProductSetupHub")
        .build();

    connection.on("ReceiveOrderUpdate", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
    }
    );

    connection.on("NewOrder", function (order) {
        var statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "Someone ordered an " + order.product;
    }
    );

    connection.on("finished", function () {
        connection.stop();
    }
    );

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const product = document.getElementById("product").value;
    const size = document.getElementById("size").value;

    fetch("/Home/OrderCoffee",
        {
            method: "POST",
            body: JSON.stringify({ product, size }),
            headers: {
                'content-type': 'application/json'
            }
        })
        .then(response => response.text())
        //.then(id => connection.invoke("GetUpdateForOrder", id));
        .then(connection.invoke("GetUpdateForOrder", 1));
});

//document.getElementById("submit").addEventListener("click", function (event) {

//    event.preventDefault();

//    const product = document.getElementById("product").value;
//    const size = document.getElementById("size").value;

//    connection.invoke("GetUpdateForOrder", 1).catch(function (err) {
//        return console.error(err.toString());
//    });
    
//});
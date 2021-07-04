// WebSocket = undefined;
//EventSource = undefined;
//, signalR.HttpTransportType.LongPolling

let connection = null;

setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/ProductSetupHub")
        .build();

    connection.on("Progress", (update) => {
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = update;
        $("#progressLine").width(update + "%");
        if (update == 20) {
            $("#divMilestone20").addClass("completed colored");
            $("#lblMilestone20").addClass("colored");
        }
        else if (update == 40) {
            $("#divMilestone40").addClass("completed colored");
            $("#lblMilestone40").addClass("colored");
        }
        else if (update == 60) {
            $("#divMilestone60").addClass("completed colored");
            $("#lblMilestone60").addClass("colored");
        }
        else if (update == 80) {
            $("#divMilestone80").addClass("completed colored");
            $("#lblMilestone80").addClass("colored");
        }
        else if (update == 100) {
            $("#divMilestone100").addClass("completed colored");
            $("#lblMilestone100").addClass("colored");
        }
    });

    connection.on("Finished", function () {    
        const statusDiv = document.getElementById("status");
        statusDiv.innerHTML = "finished";
        $("#divMilestone100").addClass("completed colored");
        $("#lblMilestone100").addClass("colored");
        $("#progressLine").width("100%");
        connection.stop();
    }
    );

    connection.start()
        .catch(err => console.error(err.toString()));
};

setupConnection();

document.getElementById("btnProgress").addEventListener("click", e => {
    e.preventDefault();
    //const product = document.getElementById("product").value;
    //const size = document.getElementById("size").value;

    $("#btnSiteLink").hide();

    connection.invoke('getConnectionId')
        .then(function (result) {
            // Send the connectionId to controller
            $("#txtConnectionId").val(result);
            //fetch("/Home/Index",
            //    {
            //        method: "POST",
            //        body: JSON.stringify({ connectionId: result }),
            //        headers: {
            //            'content-type': 'application/json'
            //        }
            //    })

            $.ajax('/Home/Index', {
                type: 'POST',  // http method
                data: { connectionId: result },  // data to submit
                success: function (data, status, xhr) {
                    //$('p').append('status: ' + status + ', data: ' + data);
                    $("#btnSiteLink").attr("href", data);
                    $("#btnSiteLink").show();
                },
                error: function (jqXhr, textStatus, errorMessage) {
                    $("#btnSiteLink").hide();
                    //$('p').append('Error' + errorMessage);
                }
            });

        });

    //connection.invoke("GetConnectionID", (result) => {
    //        $("#txtConnectionId").val(result);
    //    });

    //alert($("#txtConnectionId").val());

    
    //    .then(response => response.text())
    //    //.then(id => connection.invoke("GetUpdateForOrder", id));
    //    .then(connection.invoke("GetUpdateForOrder", 1));
});

//document.getElementById("btnStartSetup").addEventListener("click", e => {
//    e.preventDefault();
//    //const product = document.getElementById("product").value;
//    //const size = document.getElementById("size").value;

//    connection.invoke()

//    txtConnectionId


//    //connection.invoke("ShowProgress").catch(function (err) {
//    //    return console.error(err.toString());
//    //});

//    //fetch("/Home/OrderCoffee",
//    //    {
//    //        method: "POST",
//    //        body: JSON.stringify({ product, size }),
//    //        headers: {
//    //            'content-type': 'application/json'
//    //        }
//    //    })
//    //    .then(response => response.text())
//    //    //.then(id => connection.invoke("GetUpdateForOrder", id));
//    //    .then(connection.invoke("GetUpdateForOrder", 1));
//});

//document.getElementById("submit").addEventListener("click", function (event) {

//    event.preventDefault();

//    const product = document.getElementById("product").value;
//    const size = document.getElementById("size").value;

//    connection.invoke("GetUpdateForOrder", 1).catch(function (err) {
//        return console.error(err.toString());
//    });
    
//});
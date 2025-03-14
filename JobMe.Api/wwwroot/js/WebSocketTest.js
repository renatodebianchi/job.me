console.log("WebSocketTest.js loaded");

window.addEventListener('load', () => {
    const connectButton = document.createElement('button');
    connectButton.id = 'connectButton';
    connectButton.textContent = 'Connect';
    document.body.appendChild(connectButton);

    const reconnectButton = document.createElement('button');
    reconnectButton.id = 'reconnectButton';
    reconnectButton.textContent = 'Reconnect';
    document.body.appendChild(reconnectButton);

    const disconnectButton = document.createElement('button');
    disconnectButton.id = 'disconnectButton';
    disconnectButton.textContent = 'Disconnect';
    document.body.appendChild(disconnectButton);

    const sendMessageButton = document.createElement('button');
    sendMessageButton.id = 'sendMessageButton';
    sendMessageButton.textContent = 'Send Message';
    document.body.appendChild(sendMessageButton);

    const messagesDiv = document.createElement('div');
    messagesDiv.id = 'messages';
    document.body.appendChild(messagesDiv);

    let connection;
    let reconnectTimeout;

    const createConnection = () => {
        connection = new signalR.HubConnectionBuilder()
            .withUrl("/Hub", {
                transport: signalR.HttpTransportType.WebSockets,
                accessTokenFactory: () => "your-access-token",
                headers: { // Add custom headers
                    "Custom-Header": "value"
                }
            })
            .configureLogging(signalR.LogLevel.Information) // Optional: set log level
            .withHubProtocol(new signalR.JsonHubProtocol()) // Optional: set the protocol
            .withAutomaticReconnect([0, 2000, 10000, 30000]) // Retry connection after 0, 2, 10 and 30 seconds
            .build();

        connection.on("clientFunctionCallbackName", (message) => {
            const messageElement = document.createElement('div');
            messageElement.textContent = message;
            messagesDiv.appendChild(messageElement);
        });

        connection.onclose(async () => {
            console.log("Connection closed.");
            reconnectTimeout = setTimeout(startConnection, 5000); // Retry connection after 5 seconds
        });
    };

    const startConnection = async () => {
        try {
            await connection.start();
            console.log("Connected to WebSocketHub");
            if (reconnectTimeout) {
                clearTimeout(reconnectTimeout);
                reconnectTimeout = null;
            }
        } catch (err) {
            console.error("Error connecting to WebSocketHub:", err);
            reconnectTimeout = setTimeout(startConnection, 5000); // Retry connection after 5 seconds
        }
    };

    connectButton.addEventListener('click', async () => {
        createConnection();
        await startConnection();
    });

    reconnectButton.addEventListener('click', async () => {
        if (connection) {
            await connection.stop();
            await startConnection();
        }
    });

    disconnectButton.addEventListener('click', async () => {
        if (connection) {
            await connection.stop();
            if (reconnectTimeout) {
                clearTimeout(reconnectTimeout);
                reconnectTimeout = null;
            }
        }
    });

    sendMessageButton.addEventListener('click', async () => {
        try {
            await connection.invoke("SendMessageAsync", "GeneralGroup", "clientFunctionCallbackName", "Hello from client");
        } catch (err) {
            console.error("Error sending message:", err);
        }
    });
});

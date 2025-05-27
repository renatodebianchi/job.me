// Usage: Add this file to your project and include it in your HTML file.
// This file creates a WebSocket connection to a SignalR hub and sends messages to the hub.
// It also listens for messages from the hub and updates the UI with the received messages.
// The hub must have the following methods:
// - SendMessageAsync(groupName, clientFunctionCallbackName, message)
// - AtackCharacterAsync(clientFunctionCallbackName, character)
// The hub must also have the following events:
// - clientFunctionCallbackName(message)
// - charAtacked(character)
// The hub must also have the following groups:
// - GeneralGroup

const charList = [];

let characterCount = 0;

const createNewElement = (tagName, id, className, textContent) => {
    const element = document.createElement(tagName);
    element.id = id;
    element.className = className;
    element.textContent = textContent;

    if (tagName === 'button') {
        element.style.margin = '10px';
        element.style.padding = '5px 10px';
        element.style.border = '1px solid #ccc';
        element.style.borderRadius = '5px';
        element.style.backgroundColor = '#f1f1f1';
        element.style.color = '#333';
        element.style.cursor = 'pointer';
    }

    return element;
};

function updateCharacterList() {
    const tableContainer = document.getElementById('character-table');
    tableContainer.innerHTML = '';
    charList.forEach(character => {
        updateCharacter(character);
    });
}

function updateCharacter(character) {
    try {
        const newColumn = document.createElement('td');
        newColumn.style.minWidth = '240px'; // Make the card wider
        newColumn.style.width = '240px';
        newColumn.style.padding = '10px';
        newColumn.style.margin = '10px';

        // Remove the table and use a humanoid avatar with CSS
        const tableContainer = document.getElementById('character-table');
        const avatarWrapper = document.createElement('div');
        avatarWrapper.className = 'avatar-wrapper';
        avatarWrapper.style.display = 'flex';
        avatarWrapper.style.flexDirection = 'column';
        avatarWrapper.style.alignItems = 'center';
        avatarWrapper.style.margin = '20px';
        avatarWrapper.style.position = 'relative';
        avatarWrapper.style.width = '220px'; // Increased width for more space
        avatarWrapper.style.height = '260px';

        // Status border
        avatarWrapper.style.border = '6px solid ' + (character.Status == 0 ? 'green' : 'red');
        avatarWrapper.style.borderRadius = '20px';
        avatarWrapper.style.boxShadow = '0 0 16px rgba(0,0,0,0.2)';

        // Simpler humanoid avatar: head and half-circle body
        const humanoid = document.createElement('div');
        humanoid.className = 'humanoid-avatar';
        humanoid.style.position = 'relative';
        humanoid.style.width = '80px';
        humanoid.style.height = '120px';
        humanoid.style.margin = '20px auto 0 auto'; // Center the humanoid horizontally
        humanoid.style.left = 'unset'; // Remove any previous left positioning
        humanoid.style.right = 'unset';
        humanoid.style.display = 'block';

        // Head
        const head = document.createElement('div');
        head.style.width = '60px';
        head.style.height = '60px';
        head.style.background = 'linear-gradient(135deg, #ffe0b2 80%, #f5c16c 100%)';
        head.style.borderRadius = '50%';
        head.style.position = 'absolute';
        head.style.left = '10px';
        head.style.top = '0';
        head.style.border = '2.5px solid #bfa06a';
        head.style.boxShadow = '0 2px 8px rgba(0,0,0,0.10)';

        // Face details
        const leftEye = document.createElement('div');
        leftEye.style.width = '7px';
        leftEye.style.height = '7px';
        leftEye.style.background = '#333';
        leftEye.style.borderRadius = '50%';
        leftEye.style.position = 'absolute';
        leftEye.style.left = '20px';
        leftEye.style.top = '22px';
        head.appendChild(leftEye);

        const rightEye = document.createElement('div');
        rightEye.style.width = '7px';
        rightEye.style.height = '7px';
        rightEye.style.background = '#333';
        rightEye.style.borderRadius = '50%';
        rightEye.style.position = 'absolute';
        rightEye.style.left = '33px';
        rightEye.style.top = '22px';
        head.appendChild(rightEye);

        const mouth = document.createElement('div');
        mouth.style.width = '18px';
        mouth.style.height = '8px';
        mouth.style.borderBottom = '2px solid #a67c52';
        mouth.style.position = 'absolute';
        mouth.style.left = '21px';
        mouth.style.top = '38px';
        mouth.style.borderRadius = '0 0 10px 10px';
        head.appendChild(mouth);

        humanoid.appendChild(head);

        // Half-circle body
        const body = document.createElement('div');
        body.style.width = '70px';
        body.style.height = '40px';
        body.style.background = 'linear-gradient(135deg, #90caf9 80%, #42a5f5 100%)';
        body.style.position = 'absolute';
        body.style.left = '5px';
        body.style.top = '60px';
        body.style.borderTopLeftRadius = '40px';
        body.style.borderTopRightRadius = '40px';
        body.style.borderBottomLeftRadius = '40px';
        body.style.borderBottomRightRadius = '40px';
        body.style.borderBottom = 'none';
        body.style.border = '2px solid #1976d2';
        body.style.borderTop = 'none';
        body.style.boxShadow = '0 2px 8px rgba(0,0,0,0.10)';
        humanoid.appendChild(body);

        // Avatar image (if exists)
        if (character.AvatarPath) {
            const avatarImage = document.createElement('img');
            avatarImage.src = character.AvatarPath;
            avatarImage.alt = 'Avatar';
            avatarImage.style.width = '60px';
            avatarImage.style.height = '60px';
            avatarImage.style.borderRadius = '50%';
            avatarImage.style.position = 'absolute';
            avatarImage.style.left = '10px';
            avatarImage.style.top = '0';
            avatarImage.style.border = '2px solid #bfa06a';
            avatarImage.style.objectFit = 'cover';
            avatarImage.style.boxShadow = '0 2px 8px rgba(0,0,0,0.15)';
            humanoid.appendChild(avatarImage);
            fileInput.style.display = 'none';
        }

        avatarWrapper.appendChild(humanoid);

        // Character name and stats
        const nameDiv = document.createElement('div');
        nameDiv.textContent = character.Name;
        nameDiv.style.fontWeight = 'bold';
        nameDiv.style.marginTop = '10px';
        avatarWrapper.appendChild(nameDiv);

        // Stats with icons and tooltips
        const statsDiv = document.createElement('div');
        statsDiv.style.fontSize = '15px';
        statsDiv.style.marginTop = '5px';
        statsDiv.style.display = 'flex';
        statsDiv.style.gap = '10px';
        statsDiv.style.justifyContent = 'center';
        // Optionally, center the stats row
        statsDiv.style.maxWidth = '200px';
        statsDiv.style.flexWrap = 'wrap';

        // Helper to create icon span with tooltip
        function statIcon(icon, label, value, extra) {
            const span = document.createElement('span');
            span.title = label;
            span.style.display = 'inline-flex';
            span.style.alignItems = 'center';
            span.style.gap = '2px';
            span.innerHTML = `<span style="font-size:17px;">${icon}</span> <span style="font-size:13px;">${value}${extra||''}</span>`;
            return span;
        }

        // Unicode icons for stats
        statsDiv.appendChild(statIcon('⭐', 'Level', character.Level));
        statsDiv.appendChild(statIcon('❤️', 'Health', Math.floor(character.Health), `/${Math.floor(character.MaxHealth)}`));
        statsDiv.appendChild(statIcon('⚔️', 'Physical Attack', Math.floor(character.PhysicalAtack)));
        statsDiv.appendChild(statIcon('🛡️', 'Physical Defense', Math.floor(character.PhysicalDefense)));
        statsDiv.appendChild(statIcon('💨', 'Speed', Math.floor(character.Speed)));
        avatarWrapper.appendChild(statsDiv);

        newColumn.appendChild(avatarWrapper);
        tableContainer.appendChild(newColumn);
    } catch (err) {
        console.error("Error creating a new character:", err);
    }
}

window.addEventListener('load', () => {
    let connection;
    let reconnectTimeout;

    const connectButton = document.getElementById('connect-button');
    const reconnectButton = document.getElementById('reconnect-button');
    const disconnectButton = document.getElementById('disconnect-button');
    const characterAtackButton = document.getElementById('character-atack-button');
    const sendMessageButton = document.getElementById('send-message-button');
    const messages = document.getElementById('messages');
    const getCharacterButton = document.getElementById('get-character-button');

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
            messages.appendChild(messageElement);
        });

        connection.on("charAtacked", (result) => {
            charList[0] = result.atacker;
            charList[1] = result.defender;
            updateCharacterList();
        });

        connection.on("charIncluded", (character) => {
            charList.push(character);
            updateCharacterList();
            //updateCharacter(character);
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

    getCharacterButton.addEventListener('click', async () => {
        try {
            if (charList.length == 2) {
                await connection.invoke("SendMessageAsync", "GeneralGroup", "clientFunctionCallbackName", "Max 2 characters");
                return;
            }

            const character = {
                id: ++characterCount,
                name: faker.name.findName(),
            };
            await connection.invoke("CreateNewCharacterAsync", "GeneralGroup", "charIncluded", character);
        } catch (err) {
            console.error("Error sending message:", err);
        }
    });

    characterAtackButton.addEventListener('click', async () => {
        try {
            if (charList.length < 2) {
                await connection.invoke("SendMessageAsync", "GeneralGroup", "clientFunctionCallbackName", "Requires 2 characters");
                return;
            }

            await connection.invoke("CharacterAtackAsync", "GeneralGroup", "charAtacked", charList[0].Id, charList[1].Id);
        } catch (err) {
            console.error("Error sending message:", err);
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

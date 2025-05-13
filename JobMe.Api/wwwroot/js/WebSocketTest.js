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
        newColumn.style.minWidth = '250px';
        newColumn.style.padding = '10px';
        newColumn.style.margin = '10px';


        const charTable = document.createElement('table');
        const tableContainer = document.getElementById('character-table');
        charTable.style.backgroundColor = character.Status == 0 ? "green" : "red";

        newColumn.appendChild(charTable);
        tableContainer.appendChild(newColumn);

        const tableTrInicio = document.createElement('tr');
        const tableTdInicio = document.createElement('td');
        tableTdInicio.colSpan = 2;
        tableTdInicio.style.textAlign = 'center';
        const tableHrInicio = document.createElement('hr');
        tableTdInicio.appendChild(tableHrInicio);
        tableTrInicio.appendChild(tableTdInicio);
        charTable.appendChild(tableTrInicio);

        const fields = [
            { label: 'Id', value: character.Id },
            { label: 'Name', value: character.Name },
            { label: 'Health', value: Math.floor(character.Health) },
            { label: 'Level', value: character.Level },
            { label: 'Physical Atack', value: Math.floor(character.PhysicalAtack) },
            { label: 'Physical Defense', value: Math.floor(character.PhysicalDefense) },
            { label: 'Speed', value: Math.floor(character.Speed) }
        ];

        fields.forEach(field => {
            const row = document.createElement('tr');
            const labelCell = document.createElement('td');
            labelCell.textContent = field.label;
            const valueCell = document.createElement('td');
            valueCell.style.marginRight = '10px';
            valueCell.style.paddingRight = '10px';
            const input = createNewElement('input', `character-${field.label.toLowerCase()}-${character.Id}`, 'form-control', field.value);
            input.value = field.value;
            valueCell.appendChild(input);
            row.appendChild(labelCell);
            row.appendChild(valueCell);
            charTable.appendChild(row);
        });
        
        // Add file upload section
        const fileUploadRow = document.createElement('tr');
        const fileUploadLabelCell = document.createElement('td');
        fileUploadLabelCell.textContent = 'Avatar';
        const fileUploadValueCell = document.createElement('td');
        const fileInput = createNewElement('input', `character-avatar-${character.Id}`, 'form-control', '');
        fileInput.type = 'file';
        fileInput.addEventListener('change', async (event) => {
            const file = event.target.files[0];
            if (file) {
                const formData = new FormData();
                formData.append('avatar', file);
                formData.append('characterId', character.Id);

                try {
                    const response = await fetch('/character/upload-avatar', {
                        method: 'POST',
                        body: formData
                    });

                    if (response.ok) {
                        const result = await response.json();
                        console.log('Avatar uploaded successfully');

                        // Update the character's avatarPath in charList
                        const charIndex = charList.findIndex(c => c.Id === character.Id);
                        if (charIndex !== -1) {
                            charList[charIndex].AvatarPath = result.avatarPath;
                        }

                        // Display the uploaded image
                        const avatarImage = document.createElement('img');
                        avatarImage.src = result.avatarPath;
                        avatarImage.alt = 'Avatar';
                        avatarImage.style.width = '100px';
                        avatarImage.style.height = '100px';
                        avatarImage.style.borderRadius = '50%';
                        fileUploadValueCell.appendChild(avatarImage);

                        event.target.style.display = 'none'; // Hide the file input after upload
                    } else {
                        console.error('Failed to upload avatar');
                    }
                } catch (error) {
                    console.error('Error uploading avatar:', error);
                }
            }
        });

        if (character.AvatarPath) {
            const avatarImage = document.createElement('img');
            avatarImage.src = character.AvatarPath;
            avatarImage.alt = 'Avatar';
            avatarImage.style.width = '100px';
            avatarImage.style.height = '100px';
            avatarImage.style.borderRadius = '50%';
            fileUploadValueCell.appendChild(avatarImage);

        } else {
            fileUploadValueCell.appendChild(fileInput);
        }
        
        fileUploadRow.appendChild(fileUploadLabelCell);
        fileUploadRow.appendChild(fileUploadValueCell);
        charTable.appendChild(fileUploadRow);

        const tableTrFinal = document.createElement('tr');
        const tableTdFinal = document.createElement('td');
        tableTdFinal.colSpan = 2;
        tableTdFinal.style.textAlign = 'center';
        const tableHr = document.createElement('hr');
        tableTdFinal.appendChild(tableHr);
        tableTrFinal.appendChild(tableTdFinal);
        charTable.appendChild(tableTrFinal);
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

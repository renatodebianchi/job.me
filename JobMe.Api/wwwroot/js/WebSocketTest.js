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

console.log("WebSocketTest.js loaded");

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

function updateCharacter(character) {
    const characterElements = {
        Name: document.getElementById('character-name-' + character.id),
        Health: document.getElementById('character-health-' + character.id),
        Level: document.getElementById('character-level-' + character.id),
        PhysicalAtack: document.getElementById('character-physicalAtack-' + character.id),
        PhysicalDefense: document.getElementById('character-physicalDefense-' + character.id),
        Speed: document.getElementById('character-speed-' + character.id)
    };

    for (const key in characterElements) {
        if (characterElements.hasOwnProperty(key)) {
            console.log(key, character[key]);
            characterElements[key].value = character[key];
        }
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

        connection.on("charAtacked", (character) => {
            console.log(character);
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
            const character = {
                id: ++characterCount,
                name: 'New Character',
                health: Math.round(Math.random() * 100, 2),
                level: Math.round(Math.random() * 100, 2),
                physicalAtack: Math.round(Math.random() * 100, 2),
                physicalDefense: Math.round(Math.random() * 100, 2),
                speed: Math.round(Math.random() * 100, 2)
            };
            const newColumn = document.createElement('td');
            newColumn.style.minWidth = '250px';
            newColumn.style.padding = '10px';
            newColumn.style.margin = '10px';

            const charTable = document.createElement('table');
            const charTr1 = document.createElement('tr');
            const charTr2 = document.createElement('tr');
            const charTr3 = document.createElement('tr');
            const charTr4 = document.createElement('tr');
            const charTr5 = document.createElement('tr');
            const charTr6 = document.createElement('tr');
            const charTr7 = document.createElement('tr');
            
            const charTd11 = document.createElement('td'); charTd11.textContent = 'Id';
            const charTd12 = document.createElement('td');
            const charTd21 = document.createElement('td'); charTd21.textContent = 'Name';
            const charTd22 = document.createElement('td');
            const charTd31 = document.createElement('td'); charTd31.textContent = 'Health';
            const charTd32 = document.createElement('td');
            const charTd41 = document.createElement('td'); charTd41.textContent = 'Level';
            const charTd42 = document.createElement('td');
            const charTd51 = document.createElement('td'); charTd51.textContent = 'Physical Atack';
            const charTd52 = document.createElement('td');
            const charTd61 = document.createElement('td'); charTd61.textContent = 'Physical Defense';
            const charTd62 = document.createElement('td');
            const charTd71 = document.createElement('td'); charTd71.textContent = 'Speed';
            const charTd72 = document.createElement('td');

            const charInput1 = createNewElement('input', `character-id-${character.id}` , 'form-control', character.id);
            const charInput2 = createNewElement('input', `character-name-${character.id}` , 'form-control', character.name);
            const charInput3 = createNewElement('input', `character-health-${character.id}` , 'form-control', character.health);
            const charInput4 = createNewElement('input', `character-level-${character.id}` , 'form-control', character.level);
            const charInput5 = createNewElement('input', `character-physicalAtack-${character.id}` , 'form-control', character.physicalAtack);
            const charInput6 = createNewElement('input', `character-physicalDefense-${character.id}` , 'form-control', character.physicalDefense);
            const charInput7 = createNewElement('input', `character-speed-${character.id}` , 'form-control', character.speed);
            
            charInput1.value = character.id;
            charInput2.value = character.name;
            charInput3.value = character.health;
            charInput4.value = character.level;
            charInput5.value = character.physicalAtack;
            charInput6.value = character.physicalDefense;
            charInput7.value = character.speed;

            charTd12.appendChild(charInput1);
            charTd22.appendChild(charInput2);
            charTd32.appendChild(charInput3);
            charTd42.appendChild(charInput4);
            charTd52.appendChild(charInput5);
            charTd62.appendChild(charInput6);
            charTd72.appendChild(charInput7);

            charTr1.appendChild(charTd11);
            charTr1.appendChild(charTd12);
            charTr2.appendChild(charTd21);
            charTr2.appendChild(charTd22);
            charTr3.appendChild(charTd31);
            charTr3.appendChild(charTd32);
            charTr4.appendChild(charTd41);
            charTr4.appendChild(charTd42);
            charTr5.appendChild(charTd51);
            charTr5.appendChild(charTd52);
            charTr6.appendChild(charTd61);
            charTr6.appendChild(charTd62);
            charTr7.appendChild(charTd71);
            charTr7.appendChild(charTd72);

            charTable.appendChild(charTr1);
            charTable.appendChild(charTr2);
            charTable.appendChild(charTr3);
            charTable.appendChild(charTr4);
            charTable.appendChild(charTr5);
            charTable.appendChild(charTr6);
            charTable.appendChild(charTr7);

            const tableTrFinal = document.createElement('tr');
            const tableTdFinal = document.createElement('td');
            tableTdFinal.colSpan = 2;
            tableTdFinal.style.textAlign = 'center';
            const tableHr = document.createElement('hr');
            tableTdFinal.appendChild(tableHr);
            tableTrFinal.appendChild(tableTdFinal);
            charTable.appendChild(tableTrFinal);
            
            const tableContainer = document.getElementById('character-table');
            newColumn.appendChild(charTable);
            tableContainer.appendChild(newColumn);
        } catch (err) {
            console.error("Error creating a new character:", err);
        }
    });

    characterAtackButton.addEventListener('click', async () => {
        try {
            const character = {
                name: 'Character 1',
                health: 100,
                level: 1,
                physicalAtack: 10,
                physicalDefense: 5,
                speed: 5
            };
            await connection.invoke("AtackCharacterAsync", "charAtacked", character);
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

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

window.addEventListener('load', () => {

    const div1 = createNewElement('div', 'div1', 'swagger-ui opblock-tag-section is-open', '');
    const header1 = createNewElement('h3', 'header1', 'opblock-tag no-desc', '');
    const span1 = createNewElement('span', '', '', 'WebSocket Test');
    header1.appendChild(span1);
    div1.appendChild(header1);

    const button = createNewElement('button', 'call-websocket-button', 'btn execute opblock-control__btn', 'Go to WebSocket Test Page');
    div1.appendChild(button);

    document.body.appendChild(div1);

    // Add an event listener to the button to navigate to /WebSocket.html when clicked
    button.addEventListener('click', () => {
        window.location.href = '/WebSocket.html';
    });

});

const rootElement = document.getElementById('csx-app');

elements = {
    '00000000-0000-0000-0000-000000000000': rootElement
};

// create an html element
window.CreateElement = (rawTag, rawId) => {
    const tag = BINDING.conv_string(rawTag);
    const id = BINDING.conv_string(rawId);

    elements[id] = document.createElement(tag);
    elements[id].setAttribute('class', 'csx-view');
}

window.RemoveElement = (rawId) => {
    const id = BINDING.conv_string(rawId);
    elements[id].remove();
}

window.DestroyElement = (rawId) => {
    const id = BINDING.conv_string(rawId);
    elements[id] = undefined;
}

window.SetElementAttribute = (rawId, rawName, rawValue) => {
    const id = BINDING.conv_string(rawId);
    const name = BINDING.conv_string(rawName);
    const value = BINDING.conv_string(rawValue);

    elements[id].setAttribute(name, value);
}

window.AttachElement = (rawParentId, rawId) => {
    const parentId = BINDING.conv_string(rawParentId);
    const id = BINDING.conv_string(rawId);

    if (parentId === '00000000-0000-0000-0000-000000000000') {
        while (rootElement.firstChild) {
            rootElement.removeChild(rootElement.firstChild);
        }
    }

    elements[parentId].appendChild(elements[id]);
}

window.SetElementText = (rawId, rawText) => {
    const id = BINDING.conv_string(rawId);
    const text = BINDING.conv_string(rawText);

    elements[id].textContent = text;
}



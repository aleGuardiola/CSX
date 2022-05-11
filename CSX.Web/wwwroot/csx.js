
const rootElement = document.getElementById('csx-app');

elements = {
    0: rootElement
};

// create an html element
window.CreateElement = (rawTag, rawId) => {
    const tag = BINDING.conv_string(rawTag);
    const id = parseInt(BINDING.conv_string(rawId));

    const element = document.createElement(tag);

    // event listeners
    element.addEventListener('click', ev => {
        var payload = {
            X: ev.clientX,
            Y: ev.clientY
        }
        SendEventToDotNet(id, 'click', payload);
    });
    element.addEventListener('mouseover', ev => {
        var payload = {
            X: ev.clientX,
            Y: ev.clientY
        }
        SendEventToDotNet(id, 'mouseover', payload);
    });
    element.addEventListener('mouseout', ev => {
        var payload = {
            X: ev.clientX,
            Y: ev.clientY
        }
        SendEventToDotNet(id, 'mouseout', payload);
    });

    let isCoolingScrollEventDown = false;
    element.addEventListener('scroll', function (ev) {
        if (isCoolingScrollEventDown) {
            return;
        }

        isCoolingScrollEventDown = true;
        //SendEventToDotNet(id, 'scroll', payload);
        //setTimeout(() => isCoolingScrollEventDown = false, 100);
        setTimeout(() => {
            var payload = {
                X: this.scrollLeft,
                Y: this.scrollTop
            }

            SendEventToDotNet(id, 'scroll', payload);
            isCoolingScrollEventDown = false;
        }, 25);
    });

    element.addEventListener('input', function (ev) {
        var payload = {
            Text: this.value
        }
        SendEventToDotNet(id, 'textChanged', payload);
    });



    elements[id] = element;
}

window.RemoveElement = (rawId) => {
    const id = parseInt(BINDING.conv_string(rawId));
    elements[id].remove();
}

window.SetChildren = (rawId, rawChildrenJson) => {
    const id = parseInt(BINDING.conv_string(rawId));
    const childrenJson = BINDING.conv_string(rawChildrenJson);
    const childrenIds = JSON.parse(childrenJson);

    const parent = elements[id];

    const children = childrenIds.map(x => elements[x]);
    for (let i = 0; i < children.length; i++) {
        const child = children[i];
        const currentChildren = [...parent.childNodes];

        const nextCurrentChild = currentChildren[i + 1];
        const currentChild = currentChildren[i];

        if (currentChild === child) {
            continue;
        }

        if (i == 0) {
            parent.prepend(child);
            continue;
        }

        if (!nextCurrentChild) {
            parent.appendChild(child);
            continue;
        }

        parent.insertBefore(child, currentChildren[i + 1]);       

    }


}

window.DestroyElement = (rawId) => {
    const id = parseInt(BINDING.conv_string(rawId));
    elements[id] = undefined;
}

window.SetElementAttribute = (rawId, rawName, rawValue) => {
    const id = parseInt(BINDING.conv_string(rawId));
    const name = BINDING.conv_string(rawName);
    const value = BINDING.conv_string(rawValue);

    elements[id].setAttribute(name, value);
}

window.SetElementAttributes = (rawId, rawAttrs) => {
    const id = parseInt(BINDING.conv_string(rawId));
    const attrsStr = BINDING.conv_string(rawAttrs);

    attrsStr.split(',').forEach(x => {
        const split = x.split(':');
        element.setAttribute(split[0], split[1]);
    })

    //const attributes = JSON.parse(attributesJson);

    //var element = elements[id];

    //attributes.forEach(attr => {
    //    element.setAttribute(attr.Key, attr.Value);
    //});

    
}

window.AttachElement = (rawParentId, rawId) => {
    const parentId = parseInt(BINDING.conv_string(rawParentId));
    const id = parseInt(BINDING.conv_string(rawId));

    if (parentId === 0) {
        while (rootElement.firstChild) {
            rootElement.removeChild(rootElement.firstChild);
        }
    }

    elements[parentId].appendChild(elements[id]);
}

window.SetElementText = (rawId, rawText) => {
    const id = parseInt(BINDING.conv_string(rawId));
    const text = BINDING.conv_string(rawText);    
    elements[id].textContent = text;
}

function SendEventToDotNet(id, eventName, payload) {
    DotNet.invokeMethodAsync('CSX.Web', 'OnEvent', { ElementId: id, EventName: eventName, Payload: payload });
}


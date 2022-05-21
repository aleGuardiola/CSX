var CSX;
(function (CSX) {
    const rootElement = document.getElementById('csx-app');
    CSX.elements = {
        0: rootElement
    };
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function module(name) {
        return window[name];
    }
    CSX.module = module;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function AttachElement(parentId, id) {
        if (parentId === 0) {
            const rootElement = CSX.elements[0];
            while (rootElement.firstChild) {
                rootElement.removeChild(rootElement.firstChild);
            }
        }
        CSX.elements[parentId].appendChild(CSX.elements[id]);
    }
    CSX.AttachElement = AttachElement;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    let NativeEvent;
    (function (NativeEvent) {
        NativeEvent[NativeEvent["Click"] = 0] = "Click";
        NativeEvent[NativeEvent["MouseOver"] = 1] = "MouseOver";
        NativeEvent[NativeEvent["MouseOut"] = 2] = "MouseOut";
        NativeEvent[NativeEvent["Scroll"] = 3] = "Scroll";
        NativeEvent[NativeEvent["TextChanged"] = 4] = "TextChanged";
    })(NativeEvent || (NativeEvent = {}));
    function CreateElement(tag, id) {
        const element = document.createElement(tag);
        element.addEventListener('click', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            };
            SendEventToDotNet(id, NativeEvent.Click, payload);
        });
        element.addEventListener('mouseover', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            };
            SendEventToDotNet(id, NativeEvent.MouseOver, payload);
        });
        element.addEventListener('mouseout', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            };
            SendEventToDotNet(id, NativeEvent.MouseOut, payload);
        });
        let isCoolingScrollEventDown = false;
        element.addEventListener('scroll', function (ev) {
            if (isCoolingScrollEventDown) {
                return;
            }
            isCoolingScrollEventDown = true;
            setTimeout(() => {
                var payload = {
                    X: this.scrollLeft,
                    Y: this.scrollTop
                };
                SendEventToDotNet(id, NativeEvent.Scroll, payload);
                isCoolingScrollEventDown = false;
            }, 25);
        });
        element.addEventListener('input', function (ev) {
            var payload = {
                Text: this.value
            };
            SendEventToDotNet(id, NativeEvent.TextChanged, payload);
        });
        CSX.elements[id] = element;
    }
    CSX.CreateElement = CreateElement;
    function SendEventToDotNet(id, eventType, payload) {
    }
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function DestroyElement(id) {
        CSX.elements[id] = undefined;
    }
    CSX.DestroyElement = DestroyElement;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function SetElementAttribute(id, name, value) {
        CSX.elements[id].setAttribute(name, value);
    }
    CSX.SetElementAttribute = SetElementAttribute;
    function SetElementAttributes(id, attrsStr) {
        const element = CSX.elements[id];
        attrsStr.split(',').forEach(x => {
            const split = x.split(':');
            element.setAttribute(split[0], split[1]);
        });
    }
    CSX.SetElementAttributes = SetElementAttributes;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function RemoveElement(id) {
        CSX.elements[id].remove();
    }
    CSX.RemoveElement = RemoveElement;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function SetChildren(id, childrenStr) {
        const childrenIds = childrenStr.split(',');
        const parent = CSX.elements[id];
        const children = childrenIds.map(x => CSX.elements[x]);
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
    CSX.SetChildren = SetChildren;
})(CSX || (CSX = {}));
var CSX;
(function (CSX) {
    function SetElementText(id, text) {
        CSX.elements[id].textContent = text;
    }
    CSX.SetElementText = SetElementText;
})(CSX || (CSX = {}));

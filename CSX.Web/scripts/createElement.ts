
module CSX {

    enum NativeEvent {
        Click = 0,
        MouseOver,
        MouseOut,
        Scroll,
        TextChanged
    }

    // create an html element
    export function CreateElement(tag: string, id: number) {
        
        const element = document.createElement(tag);

        // event listeners
        element.addEventListener('click', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            }
            SendEventToDotNet(id, NativeEvent.Click, payload);
        });
        element.addEventListener('mouseover', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            }
            SendEventToDotNet(id, NativeEvent.MouseOver, payload);
        });
        element.addEventListener('mouseout', ev => {
            var payload = {
                X: ev.clientX,
                Y: ev.clientY
            }
            SendEventToDotNet(id, NativeEvent.MouseOut, payload);
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

                SendEventToDotNet(id, NativeEvent.Scroll, payload);
                isCoolingScrollEventDown = false;
            }, 25);
        });

        element.addEventListener('input', function (ev) {
            var payload = {
                Text: (this as HTMLInputElement).value
            }
            SendEventToDotNet(id, NativeEvent.TextChanged, payload);
        });

        elements[id] = element;
    }


    function SendEventToDotNet(id: number, eventType: NativeEvent, payload) {
        // DotNet.invokeMethodAsync('CSX.Web', 'OnEvent', { ElementId: id, EventType: eventType.valueOf(), Payload: payload });
    }

}
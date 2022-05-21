
module CSX {

    export function SetElementAttribute(id: number, name: string, value: string) {
        elements[id].setAttribute(name, value);
    }

    export function SetElementAttributes(id: number, attrsStr: string) {        
        const element = elements[id];

        attrsStr.split(',').forEach(x => {
            const split = x.split(':');
            element.setAttribute(split[0], split[1]);
        });
    }
}
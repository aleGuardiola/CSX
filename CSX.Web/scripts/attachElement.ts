
module CSX {    

    export function AttachElement(parentId, id){        
        if (parentId === 0) {
            const rootElement = elements[0];
            while (rootElement.firstChild) {
                rootElement.removeChild(rootElement.firstChild);
            }
        }

        elements[parentId].appendChild(elements[id]);
    }

}
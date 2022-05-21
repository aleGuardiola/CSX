
module CSX {

    export function SetChildren(id: number, childrenStr: string) {        
        const childrenIds = childrenStr.split(',');

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

}

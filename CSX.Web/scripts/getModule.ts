

module CSX {
    export function module(name: string) {        
        return (window as any)[name];
    }
}
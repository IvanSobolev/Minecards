const state = {
    rarity: 'Wood',
    skinFile: null,
    thumbnailFile: null,
    packLogoUrl: '/pack-logo-placeholder.png'
};

const subscribers = new Set();

const notify = () => {
    for (const subscriber of subscribers) {
        subscriber();
    }
};

const handler = {
    set(target, property, value) {
        target[property] = value;
        
        notify();
        
        return true;
    }
};

export const cardStore = new Proxy(state, handler);

export function subscribe(callback) {
    subscribers.add(callback);
    return () => subscribers.delete(callback);
}
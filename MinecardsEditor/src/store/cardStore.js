const state = {
    rarity: 'Wood',
    skinFile: null,
    thumbnailFile: null,
    
    packs: [],
    selectedPackId: null,
    packLogoUrl: null, 

    currentView: 'editor',
};

const subscribers = new Set();

const notify = () => {
    for (const subscriber of subscribers) {
        subscriber();
    }
};

export const cardStore = {
    getState: () => ({ ...state }),
    
    setRarity: (rarity) => { state.rarity = rarity; notify(); },
    setSkinFile: (file) => { state.skinFile = file; notify(); },
    setThumbnailFile: (file) => { state.thumbnailFile = file; notify(); },
    
    setPacks: (packs) => {
        state.packs = packs;
        if (packs.length > 0) {
            cardStore.setSelectedPackId(packs[0].id); 
        }
        notify();
    },

    addPack: (newPack) => {
        state.packs.push(newPack);
        if (state.packs.length === 1) {
            cardStore.setSelectedPackId(newPack.id);
        }
        notify();
    },

    setSelectedPackId: (id) => {
        const numericId = id ? parseInt(id, 10) : null;
        state.selectedPackId = numericId;

        const selectedPack = state.packs.find(p => p.id === numericId);

        if (selectedPack) {
            state.packLogoUrl = selectedPack.urlImage;
        } else {
            state.packLogoUrl = '/pack-logo-placeholder.png';
        }
        
        notify();
    },
    
    setView: (view) => {
        state.currentView = view;
        notify();
    },

    subscribe: (callback) => {
        subscribers.add(callback);
        return () => subscribers.delete(callback);
    }
};
import { Card } from 'minecards-renderer';
import 'minecards-renderer/style.css';
import { cardStore } from '../store/cardStore';

const container = document.createElement('div');
container.innerHTML = `
    <h2>Предпросмотр</h2>
    <div id="card-container" style="width: 360px; height: 560px; background: #23272a; border-radius: 18px; display: flex; justify-content: center; align-items: center; color: #777;">
        Заполните данные для предпросмотра
    </div>
`;

const cardContainer = container.querySelector('#card-container');
let cardInstance = null;

let cachedSkinUrl = null;
let cachedThumbnailUrl = null;
let lastSkinFile = null;
let lastThumbnailFile = null;
let lastRarity = null;

function updatePreview() {
    const state = cardStore.getState();

    let needsFullRecreation = false;

    if (state.skinFile !== lastSkinFile) {
        if (cachedSkinUrl) URL.revokeObjectURL(cachedSkinUrl); 
        cachedSkinUrl = state.skinFile ? URL.createObjectURL(state.skinFile) : null;
        lastSkinFile = state.skinFile;
        needsFullRecreation = true;
    }

    if (state.thumbnailFile !== lastThumbnailFile) {
        if (cachedThumbnailUrl) URL.revokeObjectURL(cachedThumbnailUrl);
        cachedThumbnailUrl = state.thumbnailFile ? URL.createObjectURL(state.thumbnailFile) : null;
        lastThumbnailFile = state.thumbnailFile;
        needsFullRecreation = true;
    }
    
    if (state.rarity !== lastRarity) {
        lastRarity = state.rarity;
        needsFullRecreation = true;
    }

    if (!needsFullRecreation && cardInstance) {
        return;
    }

    if (cardInstance) {
        cardInstance.destroy();
        cardInstance = null;
    }
    
    if (cachedSkinUrl && cachedThumbnailUrl) {
        const cardData = {
            rarity: state.rarity,
            backgroundImage: cachedThumbnailUrl,
            skinImage: cachedSkinUrl,
            packLogoImage: state.packLogoUrl,
        };
        cardInstance = new Card(cardContainer, cardData);
    } else {
        cardContainer.innerHTML = 'Заполните данные для предпросмотра';
    }
}

export function CardPreview() {
    cardStore.subscribe(updatePreview);
    updatePreview(); 
    return container;
}
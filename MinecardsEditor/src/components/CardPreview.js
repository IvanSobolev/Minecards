import { Card } from 'minecards-renderer';
import 'minecards-renderer/style.css';
import { cardStore, subscribe } from '../store/cardStore'; 

const container = document.createElement('div');
container.innerHTML = `
    <h2>Предпросмотр</h2>
    <div id="card-container" style="width: 360px; height: 560px; background: #23272a; border-radius: 18px; display: flex; justify-content: center; align-items: center; color: #777;">
        Заполните данные для предпросмотра
    </div>
`;

const cardContainer = container.querySelector('#card-container');
let cardInstance = null;

function updatePreview() {
    if (cardInstance) {
        cardInstance.destroy();
        cardInstance = null;
    }
    
    if (cardStore.skinFile && cardStore.thumbnailFile) {
        const cardData = {
            rarity: cardStore.rarity,
            backgroundImage: URL.createObjectURL(cardStore.thumbnailFile),
            skinImage: URL.createObjectURL(cardStore.skinFile),
            packLogoImage: cardStore.packLogoUrl,
        };
        cardInstance = new Card(cardContainer, cardData);
    } else {
        cardContainer.innerHTML = 'Заполните данные для предпросмотра';
    }
}

export function CardPreview() {
    subscribe(updatePreview);
    updatePreview();
    return container;
}
import { cardStore } from '../store/cardStore';
import { fetchSkinByUsername } from '../api/minecraftApi';
import { fetchThumbnailByUrl } from '../api/youtubeApi';
import { createCardBase } from '../api/cardBasesApi'; 
import { urlToFile } from '../services/fileConverter';

const container = document.createElement('div');
container.className = 'panel';
container.innerHTML = `
    <h2>Редактор карт</h2>
    <div class="form-group">
        <label for="card-name-input">Название карты</label>
        <input type="text" id="card-name-input" placeholder="Имя ютубера или название">
    </div>
    <div class="form-group">
        <label for="creator-input">Создатель (Creator)</label>
        <input type="text" id="creator-input" placeholder="Ник создателя (необязательно)">
    </div>
    <div class="form-group">
        <label for="pack-select">Пак</label>
        <select id="pack-select" disabled><option>Загрузка...</option></select>
    </div>
    <div class="form-group">
        <label for="rarity-select">Редкость карты</label>
        <select id="rarity-select">
            <option value="Wood">Wood</option><option value="Iron">Iron</option><option value="Gold">Gold</option><option value="Diamond">Diamond</option><option value="Netherite">Netherite</option><option value="Signed">Signed</option>
        </select>
    </div>
    <div class="form-group">
        <label>Скин игрока</label>
        <div class="input-group">
            <input type="text" id="nickname-input" placeholder="Ник Minecraft">
            <button id="fetch-skin-btn">Найти</button>
        </div>
        <input type="file" id="skin-file-input" accept="image/png">
        <div class="status-message" id="skin-status"></div>
    </div>
    <div class="form-group">
        <label>Превью видео</label>
        <div class="input-group">
            <input type="text" id="youtube-url-input" placeholder="Ссылка на YouTube">
            <button id="fetch-thumbnail-btn">Найти</button>
        </div>
        <input type="file" id="thumbnail-file-input" accept="image/*">
        <div class="status-message" id="thumbnail-status"></div>
    </div>
    <button id="save-card-btn" disabled>Сохранить карту</button>
    <div class="status-message" id="save-status"></div>
`;

function setStatus(element, message, type = 'info') {
    element.textContent = message;
    element.className = `status-message ${type}`;
}

export function EditorForm() {
    const cardNameInput = container.querySelector('#card-name-input');
    const creatorInput = container.querySelector('#creator-input');
    const packSelect = container.querySelector('#pack-select');
    const raritySelect = container.querySelector('#rarity-select');
    const saveBtn = container.querySelector('#save-card-btn');
    
    cardStore.subscribe(() => {
        const state = cardStore.getState();
        saveBtn.disabled = !state.skinFile || !state.thumbnailFile || !state.selectedPackId;
        if (packSelect.disabled && state.packs.length > 0) {
            packSelect.innerHTML = '';
            state.packs.forEach(pack => {
                packSelect.innerHTML += `<option value="${pack.id}">${pack.name}</option>`;
            });
            packSelect.value = state.selectedPackId;
            packSelect.disabled = false;
        } else if (state.packs.length === 0) {
            packSelect.innerHTML = '<option>Сначала создайте пак</option>';
        }
    });
    
    saveBtn.disabled = true;

    packSelect.addEventListener('change', () => cardStore.setSelectedPackId(packSelect.value));
    raritySelect.addEventListener('change', () => cardStore.setRarity(raritySelect.value));
    
    const skinStatus = container.querySelector('#skin-status');
    container.querySelector('#fetch-skin-btn').addEventListener('click', async () => {
        const nickname = container.querySelector('#nickname-input').value.trim();
        if (!nickname) return setStatus(skinStatus, 'Введите ник', 'error');
        setStatus(skinStatus, 'Загрузка...', 'info');
        try {
            const url = await fetchSkinByUsername(nickname);
            const file = await urlToFile(url, `${nickname}.png`, 'image/png');
            cardStore.setSkinFile(file);
            setStatus(skinStatus, 'Скин получен!', 'success');
        } catch (e) {
            setStatus(skinStatus, e.message, 'error');
        }
    });
    container.querySelector('#skin-file-input').addEventListener('change', (e) => {
        if(e.target.files[0]) cardStore.setSkinFile(e.target.files[0]);
    });

    const thumbStatus = container.querySelector('#thumbnail-status');
    container.querySelector('#fetch-thumbnail-btn').addEventListener('click', async () => {
        const url = container.querySelector('#youtube-url-input').value.trim();
        if (!url) return setStatus(thumbStatus, 'Введите URL', 'error');
        setStatus(thumbStatus, 'Загрузка...', 'info');
        try {
            const thumbUrl = await fetchThumbnailByUrl(url);
            const file = await urlToFile(thumbUrl, 'thumbnail.jpg', 'image/jpeg');
            cardStore.setThumbnailFile(file);
            setStatus(thumbStatus, 'Превью получено!', 'success');
        } catch(e) {
            setStatus(thumbStatus, e.message, 'error');
        }
    });

    container.querySelector('#thumbnail-file-input').addEventListener('change', (e) => {
        if(e.target.files[0]) cardStore.setThumbnailFile(e.target.files[0]);
    });

    const saveStatus = container.querySelector('#save-status');
    saveBtn.addEventListener('click', async () => {
        const state = cardStore.getState();
        const name = cardNameInput.value.trim();
        const creator = creatorInput.value.trim();

        if (!name || !state.selectedPackId || !state.skinFile || !state.thumbnailFile) {
            return setStatus(saveStatus, 'Заполните все обязательные поля!', 'error');
        }

        setStatus(saveStatus, 'Сохранение...', 'info');
        
        const formData = new FormData();
        formData.append('Name', name);
        formData.append('Creator', creator);
        formData.append('BaseRarityLevel', state.rarity);
        formData.append('PackId', state.selectedPackId);
        formData.append('SkinFile', state.skinFile);
        formData.append('BackgroundImageFile', state.thumbnailFile);
        
        try {
            await createCardBase(formData);
            setStatus(saveStatus, 'Карта успешно создана!', 'success');
        } catch (e) {
            setStatus(saveStatus, e.message, 'error');
        }
    });

    return container;
}
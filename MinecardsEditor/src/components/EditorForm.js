import { cardStore, subscribe } from '../store/cardStore';
import { fetchSkinByUsername } from '../api/minecraftApi';
import { fetchThumbnailByUrl } from '../api/youtubeApi';
import { urlToFile } from '../services/fileConverter';

const container = document.createElement('div');
container.innerHTML = `
    <div class="form-group">
        <label for="rarity-select">Редкость карты</label>
        <select id="rarity-select">
            <option value="Wood">Wood</option><option value="Iron">Iron</option><option value="Gold">Gold</option><option value="Diamond">Diamond</option><option value="Netherite">Netherite</option><option value="Signed">Signed</option>
        </select>
    </div>
    <div class="form-group">
        <label>Скин игрока</label>
        <div class="input-group">
            <input type="text" id="nickname-input" placeholder="Введите ник Minecraft">
            <button id="fetch-skin-btn">Найти</button>
        </div>
        <div class="status-message" id="skin-status"></div>
        <input type="file" id="skin-file-input" accept="image/png">
    </div>
    <div class="form-group">
        <label>Превью видео</label>
        <div class="input-group">
            <input type="text" id="youtube-url-input" placeholder="Ссылка на YouTube видео">
            <button id="fetch-thumbnail-btn">Найти</button>
        </div>
        <div class="status-message" id="thumbnail-status"></div>
        <input type="file" id="thumbnail-file-input" accept="image/jpeg, image/png, image/webp">
    </div>
    <button id="save-card-btn">Сохранить карту</button>
    <div class="status-message" id="save-status"></div>
`;

function setStatus(element, message, type = 'info') {
    element.textContent = message;
    element.className = `status-message ${type}`;
}

export function EditorForm() {
    const raritySelect = container.querySelector('#rarity-select');
    const saveBtn = container.querySelector('#save-card-btn');
    
    subscribe(() => {
        saveBtn.disabled = !cardStore.skinFile || !cardStore.thumbnailFile;
    });
    saveBtn.disabled = true;

    raritySelect.addEventListener('change', () => cardStore.rarity = raritySelect.value);
    
    const skinStatus = container.querySelector('#skin-status');
    container.querySelector('#fetch-skin-btn').addEventListener('click', async () => {
        const nickname = container.querySelector('#nickname-input').value.trim();
        if (!nickname) return setStatus(skinStatus, 'Введите ник', 'error');
        setStatus(skinStatus, 'Загрузка...', 'info');
        try {
            const url = await fetchSkinByUsername(nickname);
            const file = await urlToFile(url, `${nickname}.png`, 'image/png');
            cardStore.skinFile = file; 
            setStatus(skinStatus, 'Скин получен!', 'success');
        } catch (e) {
            setStatus(skinStatus, e.message, 'error');
        }
    });
    container.querySelector('#skin-file-input').addEventListener('change', (e) => {
        if(e.target.files[0]) cardStore.skinFile = e.target.files[0]; // Простое присваивание!
    });

    const thumbStatus = container.querySelector('#thumbnail-status');
    container.querySelector('#fetch-thumbnail-btn').addEventListener('click', async () => {
        const url = container.querySelector('#youtube-url-input').value.trim();
        if (!url) return setStatus(thumbStatus, 'Введите URL', 'error');
        setStatus(thumbStatus, 'Загрузка...', 'info');
        try {
            const thumbUrl = await fetchThumbnailByUrl(url);
            const file = await urlToFile(thumbUrl, 'thumbnail.jpg', 'image/jpeg');
            cardStore.thumbnailFile = file; 
            setStatus(thumbStatus, 'Превью получено!', 'success');
        } catch(e) {
            setStatus(thumbStatus, e.message, 'error');
        }
    });
    container.querySelector('#thumbnail-file-input').addEventListener('change', (e) => {
        if(e.target.files[0]) cardStore.thumbnailFile = e.target.files[0]; // Простое присваивание!
    });

    const saveStatus = container.querySelector('#save-status');
    saveBtn.addEventListener('click', async () => {
        setStatus(saveStatus, 'Сохранение...', 'info');
        const formData = new FormData();
        formData.append('rarity', cardStore.rarity);
        formData.append('skinFile', cardStore.skinFile);
        formData.append('backgroundImageFile', cardStore.thumbnailFile);

        console.log("Отправка данных на несуществующий API /api/cards:");
        for(let [key, value] of formData.entries()) console.log(key, value);
        
        await new Promise(r => setTimeout(r, 1000));
        setStatus(saveStatus, 'Карта успешно сохранена!', 'success');
    });

    return container;
}
import { cardStore } from '../store/cardStore';
import { createPack } from '../api/packsApi';

const container = document.createElement('div');
container.className = 'panel';
container.innerHTML = `
    <h2>Управление паками</h2>
    <div class="form-group">
        <label for="pack-name-input">Название нового пака</label>
        <input type="text" id="pack-name-input" placeholder="Введите название">
    </div>
    <div class="form-group">
        <label for="pack-image-input">Изображение пака</label>
        <input type="file" id="pack-image-input" accept="image/png, image/jpeg">
    </div>
    <button id="create-pack-btn">Создать пак</button>
    <div class="status-message" id="pack-status"></div>
    <hr style="margin: 2rem 0; border-color: var(--border-color);">
    <h3>Существующие паки</h3>
    <ul id="packs-list" style="list-style: none; padding: 0;"></ul>
`;

function setStatus(element, message, type = 'info') {
    element.textContent = message;
    element.className = `status-message ${type}`;
}

export function PackManager() {
    const packNameInput = container.querySelector('#pack-name-input');
    const packImageInput = container.querySelector('#pack-image-input');
    const createPackBtn = container.querySelector('#create-pack-btn');
    const packStatus = container.querySelector('#pack-status');
    const packsList = container.querySelector('#packs-list');
    
    cardStore.subscribe(() => {
        const { packs } = cardStore.getState();
        packsList.innerHTML = '';
        if (packs.length === 0) {
            packsList.innerHTML = '<li>Паков пока нет.</li>';
        } else {
            packs.forEach(pack => {
                const li = document.createElement('li');
                li.innerHTML = `<img src="${pack.urlImage}" width="32" height="32" style="vertical-align: middle; margin-right: 10px;"> ${pack.name}`;
                packsList.append(li);
            });
        }
    });

    createPackBtn.addEventListener('click', async () => {
        const name = packNameInput.value.trim();
        const file = packImageInput.files[0];

        if (!name || !file) {
            return setStatus(packStatus, 'Заполните название и выберите файл.', 'error');
        }

        setStatus(packStatus, 'Создание пака...', 'info');
        const formData = new FormData();
        formData.append('Name', name);
        formData.append('ImageFile', file);
        
        try {
            const newPack = await createPack(formData);
            cardStore.addPack(newPack);
            setStatus(packStatus, `Пак "${newPack.name}" успешно создан!`, 'success');
            packNameInput.value = '';
            packImageInput.value = '';
        } catch (e) {
            setStatus(packStatus, e.message, 'error');
        }
    });

    return container;
}
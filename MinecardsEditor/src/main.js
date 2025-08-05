import './style.css';
import { EditorForm } from './components/EditorForm';
import { CardPreview } from './components/CardPreview';
import { PackManager } from './components/PackManager';
import { cardStore } from './store/cardStore';
import { getAllPacks } from './api/packsApi';

document.addEventListener('DOMContentLoaded', () => {

    const mainContent = document.getElementById('main-content');
    const navEditorBtn = document.getElementById('nav-editor-btn');
    const navPacksBtn = document.getElementById('nav-packs-btn');
    
    const editorView = document.createElement('div');
    editorView.className = 'app-layout'; 
    editorView.append(EditorForm(), CardPreview());

    const packsView = document.createElement('div');
    packsView.className = 'app-layout';
    packsView.append(PackManager());

    function renderView() {
        const { currentView } = cardStore.getState();
        
        mainContent.innerHTML = '';
        
        if (currentView === 'editor') {
            mainContent.append(editorView);
            navEditorBtn.classList.add('active');
            navPacksBtn.classList.remove('active');
        } else if (currentView === 'packs') {
            mainContent.append(packsView);
            navPacksBtn.classList.add('active');
            navEditorBtn.classList.remove('active');
        }
    }

    navEditorBtn.addEventListener('click', () => cardStore.setView('editor'));
    navPacksBtn.addEventListener('click', () => cardStore.setView('packs'));

    async function initializeApp() {
        cardStore.subscribe(renderView);
        
        renderView();

        try {
            const packs = await getAllPacks();
            cardStore.setPacks(packs);
        } catch (error) {
            console.error("Не удалось загрузить паки:", error);
        }
    }
    
    initializeApp();

});
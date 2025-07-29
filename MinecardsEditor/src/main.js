import './style.css';
import { EditorForm } from './components/EditorForm';
import { CardPreview } from './components/CardPreview';

document.querySelector('#editor-container').append(EditorForm());
document.querySelector('#preview-container').append(CardPreview());
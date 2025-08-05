export async function getAllPacks() {
    const response = await fetch('/api/packs');
    if (!response.ok) throw new Error('Не удалось загрузить паки');
    return await response.json();
}

export async function createPack(formData) {
    const response = await fetch('/api/packs', {
        method: 'POST',
        body: formData
    });
    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Не удалось создать пак');
    }
    return await response.json();
}
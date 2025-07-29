export async function fetchSkinByUsername(username) {
    const response = await fetch(`/api/minecraft/skin/${username}`);
    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Не удалось получить скин');
    }
    const data = await response.json();
    return data.url;
}
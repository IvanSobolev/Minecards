export async function fetchThumbnailByUrl(url) {
    const response = await fetch(`/api/youtube/thumbnail?url=${encodeURIComponent(url)}`);
    if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || 'Не удалось получить превью');
    }
    const data = await response.json();
    return data.url;
}
export async function createCardBase(formData) {
    const response = await fetch('/api/cardbases', {
        method: 'POST',
        body: formData,
    });
    
    if (!response.ok) {
        let errorMessage = 'Не удалось создать карту';
        try {
            const errorData = await response.json();
            errorMessage = errorData.message || errorMessage;
        } catch (e) {
            
        }
        throw new Error(errorMessage);
    }
    
    return await response.json();
}
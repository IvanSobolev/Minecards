import { defineConfig } from 'vite';

export default defineConfig({
  server: {
    proxy: {
      // Проксируем все запросы, начинающиеся с /api
      '/api': {
        // Укажите адрес вашего бэкенд-сервера
        target: 'http://localhost:8080', // Пример для ASP.NET Core на порту 5001
        changeOrigin: true, // Необходимо для виртуальных хостов
        secure: false,      // Отключаем проверку SSL-сертификата, если бэкенд на https с самоподписанным сертом
      },
    },
  },
});
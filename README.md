# EventsWebApp
## Настройка проекта

### Шаг 1. Клонирование репозитория

Склонируйте проект на локальную машину:

```bash
git clone https://github.com/d1amond09/EventsWebApp.git
```

### Шаг 2. Настройка базы данных

1. Откройте файл `appsettings.json` и настройте строку подключения к вашей базе данных:

```json
"ConnectionStrings": {
    "DefaultConnection": "YourPostgreSQLConnectionString"
}
```

2. В консоли Package Manager Console выполните миграции для создания базы данных:

```bash
Update-Database
```

### Шаг 3. Запуск приложения

1. Убедитесь, что проект **EventsWebApp.API** выбран в качестве стартового.
2. Запустите приложение с помощью http/https или Container (Dockerfile)

### Шаг 4. Тестирование API

Для работы с API используйте **Postman** или другой клиент для отправки HTTP-запросов.

1. **Получение JWT-токена**:

   - Отправьте POST-запрос с запущенным проектом на `https://localhost:{ваш порт}/api/auth` с телом:
     ```json
     {
      "FirstName":"Ivan",
      "LastName":"Ivanov",
      "UserName":"ivan",
      "Password":"YourPassword!1234",
      "ConfirmPassword":"MrD1amond!09",
      "Email":"your@gmail.com",
      "BirthDate":"2004-10-31T00:00:00Z",
      "Roles":["Administrator"]
     }
     ```
   - Отправьте POST-запрос с запущенным проектом на `https://localhost:{ваши порт}/api/auth/login` с телом:
     ```json
     {
       "email": "your@gmail.com",
       "password": "YourPassword!1234"
     }
     ```
   - Ответ будет содержать JWT-токен.

2. **Использование токена**:

   - В Authorization выберете в Auth Type **Bearer Token**, вставьте ваш JWT-токен

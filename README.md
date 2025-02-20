# EventsWebApp 
[![build and test docker](https://github.com/d1amond09/EventsWebApp/actions/workflows/ci.yml/badge.svg)](https://github.com/d1amond09/EventsWebApp/actions/workflows/ci.yml)
## Настройка проекта

### Шаг 1. Клонирование репозитория

Склонируйте проект на локальную машину:

```bash
git clone https://github.com/d1amond09/EventsWebApp.git
```

### Шаг 2. Настройка конфигураций

1. Откройте файл `appsettings.json` и настройте строку подключения к вашей базе данных и `validAudience`:

```json
"ConnectionStrings": {
    "DefaultConnection": "YourPostgreSQLConnectionString"
},
"JwtSettings": {
    "validAudience": "http://localhost:{ваш host-порт}"
},
``` 

2. В консоли Package Manager Console выполните миграции для создания базы данных:

```bash
Update-Database
```

### Шаг 3. Запуск приложения

1. Убедитесь, что проект **EventsWebApp.API** выбран в качестве стартового.
2. Запустите приложение с помощью http/https или Container (Dockerfile)

3. Если приложение не запускается через Docker, перейдите в корневую папку решения (`EventsWebApp`).
```bash
cd path/to/folder/EventsWebApp
```
4. Откройте командную строку (cmd) и выполните следующие команды:
```docker
docker build -t event-web-app-api -f ./EventsWebApp.API/Dockerfile .
docker run -p {ваш host-порт}:{docker порт} -e JWT_SECRET_KEY=$'YOUR_JWT_SECRET_KEY_EVENT_WEB_APP_API' event-web-app-api
```

### Шаг 4. Тестирование API

Для работы с API используйте **Postman** или другой клиент для отправки HTTP-запросов.

1. **Получение JWT-токена**:

   - Отправьте POST-запрос с запущенным проектом на `http://localhost:{ваш порт}/api/auth` с телом:
     ```json
     {
      "FirstName":"Ivan",
      "LastName":"Ivanov",
      "UserName":"ivan",
      "Password":"YourPassword!1234",
      "ConfirmPassword":"YourPassword!1234",
      "Email":"your@gmail.com",
      "BirthDate":"2004-10-31T00:00:00Z",
      "Roles":["Administrator"]
     }
     ```
   - Перейдите в элекронную почту указанную в поле `Email` и подтвердите регистрацию, нажав на ссылку.
   - Отправьте POST-запрос с запущенным проектом на `http://localhost:{ваши порт}/api/auth/login` с телом:
     ```json
     {
       "email": "your@gmail.com",
       "password": "YourPassword!1234"
     }
     ```
   - Ответ будет содержать JWT-токен (access и refresh).

3. **Использование токена**:

   - В Authorization выберете в Auth Type **Bearer Token**, вставьте ваш JWT-токен

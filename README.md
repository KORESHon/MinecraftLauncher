# Minecraft Launcher

Простой лаунчер для Minecraft сервера.

## Функции
- Автоматическое обновление клиента
- Загрузка модов
- Запуск игры одной кнопкой

## Технологии
- C# .NET 8
- Windows Forms
- Single-file executable

## Установка
Скачай последний релиз из [Releases](../../releases)


# Техническое задание: Minecraft Launcher для сервера

## Описание проекта
Создать простой лаунчер для Minecraft сервера в виде одного .exe файла. Лаунчер должен автоматически загружать нужную версию игры и модов, проверять обновления и запускать клиент.

## Технические требования

### Платформа и технологии
- **Язык**: C# (.NET 6 или выше)
- **UI Framework**: WinForms (простота реализации)
- **Сборка**: PublishSingleFile = true (один .exe файл)
- **Целевая ОС**: Windows 10/11

### Архитектура приложения
```
MinecraftLauncher/
├── MainForm.cs (главное окно)
├── GameUpdater.cs (проверка и загрузка обновлений)
├── MinecraftStarter.cs (запуск игры)
├── ConfigManager.cs (настройки лаунчера)
├── Models/
│   ├── ServerInfo.cs
│   └── GameProfile.cs
└── Resources/ (иконки, изображения)
```

## Функциональные требования

### 1. Главное окно (MainForm)
- **Размер**: 400x300 пикселей, не изменяемый
- **Элементы интерфейса**:
  - Label с названием сервера
  - PictureBox для логотипа (опционально)
  - ProgressBar для отображения прогресса загрузки
  - Label для статуса ("Проверка версии...", "Загрузка модов...", "Готово к игре")
  - Button "Играть" (при обновлении меняется на "Обновление...")
  - Label с версией лаунчера (внизу)

### 2. Проверка версии (GameUpdater)
- При запуске лаунчера отправлять GET запрос на `http://yourserver.com/api/version`
- Ожидаемый JSON ответ:
```json
{
  "minecraft_version": "1.20.1",
  "mods_hash": "abc123def456",
  "client_files": [
    {
      "name": "minecraft-1.20.1.jar",
      "url": "http://yourserver.com/files/minecraft-1.20.1.jar",
      "hash": "sha256_hash"
    },
    {
      "name": "forge-installer.jar", 
      "url": "http://yourserver.com/files/forge-installer.jar",
      "hash": "sha256_hash"
    }
  ],
  "mods": [
    {
      "name": "mod1.jar",
      "url": "http://yourserver.com/mods/mod1.jar", 
      "hash": "sha256_hash"
    }
  ]
}
```

### 3. Структура файлов клиента
```
%APPDATA%/.minecraft_custom/
├── versions/
│   └── server-1.20.1/
├── mods/
├── launcher_profiles.json
└── launcher_version.txt
```

### 4. Логика обновления
1. Проверить существование локальных файлов
2. Сравнить SHA256 хеши локальных файлов с серверными
3. Если файл отсутствует или хеш не совпадает - загрузить
4. Показывать прогресс загрузки в ProgressBar
5. После успешного обновления - активировать кнопку "Играть"

### 5. Запуск Minecraft (MinecraftStarter)
- Параметры JVM:
```bash
java -Xmx4G -Xms2G 
-Djava.library.path="%APPDATA%/.minecraft_custom/versions/server-1.20.1/natives" 
-cp "%APPDATA%/.minecraft_custom/libraries/*;%APPDATA%/.minecraft_custom/versions/server-1.20.1/server-1.20.1.jar" 
net.minecraft.client.main.Main 
--username "Player" 
--version "server-1.20.1" 
--gameDir "%APPDATA%/.minecraft_custom" 
--assetsDir "%APPDATA%/.minecraft_custom/assets" 
--assetIndex "1.20" 
--accessToken "dummy"
```

### 6. Обработка ошибок
- Отсутствие интернет соединения
- Недоступность сервера обновлений
- Ошибки при загрузке файлов
- Недостаток места на диске
- Отсутствие Java

## Нефункциональные требования

### Производительность
- Время запуска лаунчера: не более 3 секунд
- Скорость загрузки: показывать прогресс, поддержка resume
- Потребление памяти: не более 100MB

### Безопасность
- Проверка цифровых подписей загружаемых файлов (SHA256)
- Валидация URL для предотвращения path traversal

### Пользовательский интерфейс
- Минималистичный дизайн
- Отзывчивый интерфейс (async операции)
- Информативные сообщения об ошибках

## Конфигурация

### Файл config.json (в папке с .exe)
```json
{
  "server_name": "Мой Minecraft Сервер",
  "update_server_url": "http://yourserver.com/api",
  "minecraft_directory": "%APPDATA%/.minecraft_custom",
  "java_path": "java",
  "jvm_args": "-Xmx4G -Xms2G"
}
```

## Примеры кода для реализации

### Базовая структура MainForm
```csharp
public partial class MainForm : Form
{
    private Button playButton;
    private ProgressBar progressBar;
    private Label statusLabel;
    private GameUpdater updater;
    
    // async void playButton_Click - проверка обновлений и запуск
    // async Task CheckForUpdates() - основная логика
    // void UpdateProgress(int percentage, string status) - обновление UI
}
```

### HTTP клиент для загрузки
```csharp
public class GameUpdater
{
    private readonly HttpClient httpClient;
    
    // async Task<ServerInfo> GetServerVersion()
    // async Task DownloadFile(string url, string localPath, IProgress<int> progress)
    // string CalculateFileHash(string filePath)
    // async Task<bool> ValidateLocalFiles(ServerInfo serverInfo)
}
```

## Дополнительные требования

### Логирование
- Создавать файл launcher.log с информацией о:
  - Времени запуска
  - Результатах проверки версий
  - Ошибках загрузки
  - Параметрах запуска игры

### Автообновление лаунчера
- Проверка версии самого лаунчера при запуске
- Возможность скачать новую версию (в будущем)

## Результат
В итоге должен получиться один файл `MinecraftLauncher.exe` размером около 20-30MB, который можно просто скопировать на любой компьютер с Windows и запустить.

## Приоритеты разработки
1. **Высокий**: Базовый UI, проверка версий, загрузка файлов
2. **Средний**: Обработка ошибок, логирование, конфигурация
3. **Низкий**: Автообновление лаунчера, дополнительные настройки

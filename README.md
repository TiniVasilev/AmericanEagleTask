# AmericanEagleTask
Entry task for American Eagle
Development Assignment 
Goal: Develop a simple website for listing information regarding Asteroids. Users should be able to export the data in .xlsx. In addition, the website should have a section where visitors can browse the Astronomy Picture of the Day with support for past periods. 

Remark: The solution must be built with ASP.NET Core. It should utilize the NASA REST API - https://api.nasa.gov. 

Deliverable: Link to public BitBucket/GitHub repository. 

# AsteroidsApp

AsteroidsApp е ASP.NET Core MVC уеб приложение, което показва информация за астероиди и Astronomy Picture of the Day (APOD) чрез NASA API. Архитектурата е Clean Architecture (Domain, Application, Infrastructure, Web).

## Основни функционалности
- Извличане и показване на астероиди за избрана дата (NASA NeoWs API)
- Astronomy Picture of the Day (APOD)
- Експорт на астероиди към Excel (ClosedXML)
- Кеширане на резултатите от NASA API (In-Memory)
- Обработка на грешки и user-friendly съобщения
- Unit тестове (xUnit, Moq)

## Архитектура
- **Domain** – ентитети и интерфейси
- **Application** – DTOs, use cases, интерфейси
- **Infrastructure** – имплементации на услуги (NasaApiService, ExcelExportService)
- **Web** – MVC контролери, Razor изгледи

## Стартиране
1. Инсталирай зависимостите:
   ```sh
   dotnet restore
   ```
2. Билдни проекта:
   ```sh
   dotnet build
   ```
3. Стартирай уеб приложението:
   ```sh
   dotnet run --project AsteroidsApp.Web
   ```
4. Отвори браузър на http://localhost:5215 (или порта, изписан в терминала)

## Конфигурация на NASA API ключ
- Файл: `AsteroidsApp.Web/appsettings.json`
- Ключът е в секция:
  ```json
  "NasaApi": {
    "ApiKey": "DEMO_KEY"
  }
  ```
- Можеш да смениш `DEMO_KEY` с твой собствен ключ от https://api.nasa.gov

## Кеширане
- Вградено In-Memory кеширане (IMemoryCache) в `NasaApiService`
- Резултатите за всяка дата се пазят 10 минути

## Експорт към Excel
- Бутона "Export to Excel" експортира астероидите за избраната дата
- Използва ClosedXML (няма нужда от Excel на машината)

## Unit тестове
- Проект: `AsteroidsApp.Tests`
- Стартирай с:
  ```sh
  dotnet test
  ```

## Препоръчителни подобрения
- Добави твой NASA API ключ за повече заявки
- Добави интеграционни тестове
- Добави кеширане на APOD
- UI/UX подобрения (Bootstrap, loading indicators и др.)
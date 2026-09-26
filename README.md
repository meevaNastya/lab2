# Отчет по лабораторной работе: Разработка микросервисной архитектуры с GraphQL 

Микросервисное приложение Car Rental для управления арендой автомобилей.
## Демонстрация:https://drive.google.com/drive/folders/1-72ke8lE2BFdXNjwXfGMLkEpNVi57Vhn
**Архитектура:**

Frontend (HTML + CSS + JavaScript) -> API Gateway (GraphQL Gateway) -> Микросервисы (Users, Cars, Rentals) -> Базы данных (PostgreSQL)

**Компоненты:**

GraphQL API Gateway (Hot Chocolate) -> маршрутизирует запросы на микросервисы
Микросервис "Пользователи" -> управляет пользователями и их ролями (PostgreSQL)
Микросервис "Автомобили" -> управляет автомобилями и их характеристиками (PostgreSQL)
Микросервис "Аренда" -> управляет заявками на аренду и договорами (PostgreSQL)


Для взаимодействия frontend с микросервисами используется **CarRental.Gateway**, предоставляющий единый GraphQL API.

```text
Frontend :8080
      |
      | GraphQL
      v
CarRental.Gateway :5004
      |
      +----------------+----------------+
      |                |                |
      v                v                v
Cars :5001        Users :5002      Rentals :5003
      |                |                |
      v                v                v
 PostgreSQL        PostgreSQL        PostgreSQL
```

Каждый микросервис работает со своей базой данных:

```text
car_rental_cars
car_rental_users
car_rental_rentals
```

## Технологии

* ASP.NET Core
* C#
* GraphQL
* Hot Chocolate
* Entity Framework Core
* PostgreSQL
* HTML
* CSS
* JavaScript

## Реализованный функционал

### Автомобили

Для автомобилей реализованы CRUD-операции:

* создание автомобиля
* получение списка автомобилей
* изменение стоимости аренды
* удаление автомобиля

### Пользователи

Для пользователей реализованы:

* создание пользователя
* получение списка пользователей
* изменение пароля
* удаление пользователя

### Аренда

Реализован сценарий аренды автомобиля:

1. создание заявки на аренду
2. одобрение заявки
3. создание договора
4. просмотр заявок и договоров
5. удаление заявки и связанного договора

Все действия frontend выполняет через **GraphQL Gateway**.

## Структура проекта

```text
car-rental-graphql/
│
├── CarRental.Cars/
├── CarRental.Users/
├── CarRental.Rentals/
├── CarRental.Gateway/
├── CarRental.Frontend/
│   └── wwwroot/
│       ├── index.html
│       ├── style.css
│       └── app.js
│
└── CarRentalGraphQL.slnx
```

## Запуск

Для работы приложения необходимо запустить PostgreSQL.

Каждый сервис запускается в отдельном терминале из корневой директории проекта.

### Cars

```bash
dotnet run --project CarRental.Cars --urls http://localhost:5001
```

### Users

```bash
dotnet run --project CarRental.Users --urls http://localhost:5002
```

### Rentals

```bash
dotnet run --project CarRental.Rentals --urls http://localhost:5003
```

### GraphQL Gateway

```bash
dotnet run --project CarRental.Gateway --urls http://localhost:5004
```

GraphQL endpoint:

```text
http://localhost:5004/graphql
```

### Frontend

```bash
python3 -m http.server 8080 --directory CarRental.Frontend/wwwroot
```

После запуска frontend доступен по адресу:

```text
http://localhost:8080
```


## Схема взаимодействия

Например, при создании автомобиля запрос проходит следующий путь:

```text
Frontend
   ↓
GraphQL Mutation
   ↓
CarRental.Gateway
   ↓
CarRental.Cars
   ↓
PostgreSQL
```

После сохранения данных frontend повторно получает информацию через GraphQL и отображает обновлённый список автомобилей.

Аналогично Gateway взаимодействует с сервисами Users и Rentals.

## Бизнес-сценарий аренды

```text
Пользователь
     ↓
Выбор автомобиля
     ↓
Создание заявки
     ↓
Одобрение заявки
     ↓
Создание договора
```

Данные сохраняются в PostgreSQL, поэтому они не теряются после перезапуска микросервисов.

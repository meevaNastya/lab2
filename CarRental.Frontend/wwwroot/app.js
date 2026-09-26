const GRAPHQL_URL = "http://localhost:5004/graphql";

async function graphql(query) {
    const response = await fetch(GRAPHQL_URL, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ query })
    });

    const result = await response.json();

    if (result.errors) {
        console.error(result.errors);
        throw new Error(result.errors[0].message);
    }

    return result.data;
}

async function loadData() {
    try {
        const data = await graphql(`
            {
                cars {
                    id
                    brand
                    model
                    vin
                    category
                    pricePerDay
                }

                users {
                    id
                    username
                    birthDate
                    driverLicenseIssueDate
                    roles
                }

                rentals {
                    id
                    clientId
                    carId
                    startDate
                    endDate
                    status
                }

                rentalAgreements {
                    id
                    rentalRequestId
                    totalPrice
                    createdAt
                }
            }
        `);

        renderCars(data.cars);
        renderUsers(data.users);
        renderRentals(data.rentals, data.users, data.cars);
        renderAgreements(data.rentalAgreements);

    } catch (error) {
        console.error(error);

        document.getElementById("cars").innerHTML =
            "<p class='empty'>Не удалось подключиться к GraphQL Gateway.</p>";
    }
}

function renderCars(cars) {
    const container = document.getElementById("cars");

    if (!cars.length) {
        container.innerHTML =
            "<p class='empty'>Автомобилей пока нет.</p>";
        return;
    }

    container.innerHTML = cars.map(car => `
        <div class="card">
            <h3>${car.brand} ${car.model}</h3>
            <p>VIN: ${car.vin}</p>
            <p>Категория: ${carCategory(car.category)}</p>
            <p>Цена: ${car.pricePerDay} ₽ / день</p>

            <div class="actions">
                <button onclick="changeCarPrice(${car.id})">
                    Изменить цену
                </button>

                <button
                    class="delete"
                    onclick="deleteCar(${car.id})">
                    Удалить
                </button>
            </div>
        </div>
    `).join("");
}

function renderUsers(users) {
    const container = document.getElementById("users");

    if (!users.length) {
        container.innerHTML =
            "<p class='empty'>Пользователей пока нет.</p>";
        return;
    }

    container.innerHTML = users.map(user => `
        <div class="card">
            <h3>${user.username}</h3>
            <p>ID: ${user.id}</p>
            <p>Дата рождения: ${user.birthDate}</p>
            <p>Роли: ${user.roles.map(userRole).join(", ")}</p>

            <div class="actions">
                <button onclick="changeUserPassword(${user.id})">
                    Изменить пароль
                </button>

                <button
                    class="delete"
                    onclick="deleteUser(${user.id})">
                    Удалить
                </button>
            </div>
        </div>
    `).join("");
}

function renderRentals(rentals, users, cars) {
    const container = document.getElementById("rentals");

    if (!rentals.length) {
        container.innerHTML =
            "<p class='empty'>Аренд пока нет.</p>";
        return;
    }

    container.innerHTML = rentals.map(rental => {
        const user = users.find(u => u.id === rental.clientId);
        const car = cars.find(c => c.id === rental.carId);

        const clientName = user
            ? user.username
            : `ID ${rental.clientId}`;

        const carName = car
            ? `${car.brand} ${car.model}`
            : `ID ${rental.carId}`;

        return `
        <div class="card">
            <h3>Аренда #${rental.id}</h3>
            <p>Клиент: ${clientName}</p>
            <p>Автомобиль: ${carName}</p>
            <p>${rental.startDate} — ${rental.endDate}</p>
            <p>Статус: ${rentalStatus(rental.status)}</p>

            <div class="actions">
                ${rental.status === 1 ? `
                    <button onclick="approveRental(${rental.id})">
                        Одобрить
                    </button>
                ` : ""}

                ${rental.status === 2 ? `
                    <button onclick="createAgreement(${rental.id})">
                        Создать договор
                    </button>
                ` : ""}

                <button
                    class="delete"
                    onclick="deleteRental(${rental.id})">
                    Удалить
                </button>
            </div>
        </div>
    `;
    }).join("");
}

function renderAgreements(agreements) {
    const container = document.getElementById("agreements");

    if (!agreements.length) {
        container.innerHTML =
            "<p class='empty'>Договоров пока нет.</p>";
        return;
    }

    container.innerHTML = agreements.map(agreement => `
        <div class="card">
            <h3>Договор #${agreement.id}</h3>
            <p>Аренда: #${agreement.rentalRequestId}</p>
            <p>Стоимость: ${agreement.totalPrice} ₽</p>
            <p>
                Создан:
                ${new Date(agreement.createdAt).toLocaleString()}
            </p>
        </div>
    `).join("");
}

function userRole(role) {
    const roles = {
        1: "Клиент",
        2: "Менеджер",
        3: "Администратор"
    };

    return roles[role] ?? "Неизвестно";
}

function carCategory(category) {
    const categories = {
        1: "Economy",
        2: "Comfort",
        3: "Premium",
        4: "Sport"
    };

    return categories[category] ?? "Неизвестно";
}

function rentalStatus(status) {
    const statuses = {
        1: "Создана",
        2: "Одобрена",
        3: "Отклонена",
        4: "Отменена"
    };

    return statuses[status] ?? "Неизвестно";
}

function showCarForm() {
    document
        .getElementById("car-form")
        .classList.toggle("hidden");
}

async function createCar() {
    const brand =
        document.getElementById("car-brand").value;

    const model =
        document.getElementById("car-model").value;

    const vin =
        document.getElementById("car-vin").value;

    const category =
        Number(document.getElementById("car-category").value);

    const price =
        Number(document.getElementById("car-price").value);

    await graphql(`
        mutation {
            createCar(
                brand: "${brand}"
                model: "${model}"
                vin: "${vin}"
                category: ${category}
                pricePerDay: ${price}
            ) {
                id
                brand
                model
            }
        }
    `);

    document
        .getElementById("car-form")
        .classList.add("hidden");

    await loadData();
}

async function changeCarPrice(id) {
    const price = prompt("Введите новую цену:");

    if (!price) {
        return;
    }

    await graphql(`
        mutation {
            changeCarPrice(
                id: ${id}
                pricePerDay: ${Number(price)}
            ) {
                id
                pricePerDay
            }
        }
    `);

    await loadData();
}

async function deleteCar(id) {
    if (!confirm("Удалить автомобиль?")) {
        return;
    }

    await graphql(`
        mutation {
            deleteCar(id: ${id})
        }
    `);

    await loadData();
}

loadData();

function showUserForm() {
    document
        .getElementById("user-form")
        .classList.toggle("hidden");
}

async function createUser() {
    const username =
        document.getElementById("user-username").value;

    const password =
        document.getElementById("user-password").value;

    const birthDate =
        document.getElementById("user-birthdate").value;

    const licenseDate =
        document.getElementById("user-license-date").value;

    const role =
        Number(document.getElementById("user-role").value);

    if (!username || !password || !birthDate || !licenseDate) {
        alert("Заполните все поля");
        return;
    }

    await graphql(`
        mutation {
            createUser(
                username: "${username}"
                passwordHash: "${password}"
                birthDate: "${birthDate}"
                driverLicenseIssueDate: "${licenseDate}"
                role: ${role}
            ) {
                id
                username
                birthDate
                roles
            }
        }
    `);

    document
        .getElementById("user-form")
        .classList.add("hidden");

    await loadData();
}

async function changeUserPassword(id) {
    const password = prompt("Введите новый пароль:");

    if (!password) {
        return;
    }

    await graphql(`
        mutation {
            changeUserPassword(
                id: ${id}
                passwordHash: "${password}"
            ) {
                id
                username
            }
        }
    `);

    alert("Пароль изменён");
    await loadData();
}

async function deleteUser(id) {
    if (!confirm("Удалить пользователя?")) {
        return;
    }

    await graphql(`
        mutation {
            deleteUser(id: ${id})
        }
    `);

    await loadData();
}

function showRentalForm() {
    document
        .getElementById("rental-form")
        .classList.toggle("hidden");
}

async function createRental() {
    const clientId =
        Number(document.getElementById("rental-client").value);

    const carId =
        Number(document.getElementById("rental-car").value);

    const startDate =
        document.getElementById("rental-start").value;

    const endDate =
        document.getElementById("rental-end").value;

    if (!clientId || !carId || !startDate || !endDate) {
        alert("Заполните все поля");
        return;
    }

    await graphql(`
        mutation {
            createRental(
                clientId: ${clientId}
                carId: ${carId}
                startDate: "${startDate}"
                endDate: "${endDate}"
            ) {
                id
                clientId
                carId
                startDate
                endDate
                status
            }
        }
    `);

    document
        .getElementById("rental-form")
        .classList.add("hidden");

    await loadData();
}

async function approveRental(id) {
    await graphql(`
        mutation {
            approveRental(id: ${id}) {
                id
                status
            }
        }
    `);

    await loadData();
}

async function createAgreement(id) {
    const totalPrice = prompt("Введите итоговую стоимость аренды:");

    if (!totalPrice) {
        return;
    }

    await graphql(`
        mutation {
            createRentalAgreement(
                id: ${id}
                totalPrice: ${Number(totalPrice)}
            ) {
                id
                rentalRequestId
                totalPrice
                createdAt
            }
        }
    `);

    await loadData();
}

async function deleteRental(id) {
    if (!confirm("Удалить аренду и связанный с ней договор?")) {
        return;
    }

    await graphql(`
        mutation {
            deleteRental(id: ${id})
        }
    `);

    await loadData();
}

const devicesContainer = document.getElementById("devices-container");
const refreshButton = document.getElementById("refresh-button");
const lastUpdated = document.getElementById("last-updated");
const errorMessage = document.getElementById("error-message");

let isLoading = false;

// Format device uptime

function formatUptime(seconds) {
    if (seconds == null) return "N/A";

    const total = Number(seconds);

    if (!Number.isFinite(total)) return "N/A";

    const hours = Math.floor(total / 3600);
    const minutes = Math.floor((total % 3600) / 60);
    const remainingSeconds = Math.floor(total % 60);

    return `${hours}h ${minutes}m ${remainingSeconds}s`;
}

// Format date

function formatDate(date) {
    if (!date) return "Never";

    return new Date(date).toLocaleString();
}

// Create information row

function createInfoRow(label, value) {
    const row = document.createElement("div");
    row.className = "info-row";

    const labelElement = document.createElement("span");
    labelElement.className = "info-label";
    labelElement.textContent = label;

    const valueElement = document.createElement("span");
    valueElement.className = "info-value";
    valueElement.textContent = value;

    row.append(labelElement, valueElement);

    return row;
}

// Create device card

function createDeviceCard(device) {
    const card = document.createElement("article");
    card.className = "device-card";

    const header = document.createElement("div");
    header.className = "device-header";

    const name = document.createElement("h3");
    name.textContent = device.name;

    const status = document.createElement("span");

    status.className = device.isOnline
        ? "status-badge status-online"
        : "status-badge status-offline";

    status.textContent = device.isOnline
        ? "ONLINE"
        : "OFFLINE";

    header.append(name, status);

    const info = document.createElement("div");
    info.className = "device-info";

    const metrics = device.info?.metrics ?? {};

    info.append(
        createInfoRow("API URL", device.url),
        createInfoRow("IP Address", device.info?.address ?? "N/A"),
        createInfoRow(
            "Temperature",
            metrics.Temperature != null
                ? `${metrics.Temperature} °C`
                : "N/A"
        ),
        createInfoRow(
            "Uptime",
            formatUptime(metrics.UptimeSeconds)
        ),
        createInfoRow(
            "Last Seen",
            formatDate(device.lastSeen)
        )
    );

    card.append(header, info);

    return card;
}

// Update dashboard summary

function updateSummary(devices) {
    const total = devices.length;
    const online = devices.filter(device => device.isOnline).length;

    document.getElementById("total-devices").textContent = total;
    document.getElementById("online-devices").textContent = online;
    document.getElementById("offline-devices").textContent = total - online;
}

// Display devices

function renderDevices(devices) {
    devicesContainer.replaceChildren();

    if (devices.length === 0) {
        devicesContainer.textContent = "No devices found.";
        return;
    }

    devices.forEach(device => {
        devicesContainer.appendChild(createDeviceCard(device));
    });
}

// Fetch data from backend

async function loadDashboard() {
    if (isLoading) return;

    isLoading = true;
    refreshButton.disabled = true;
    errorMessage.textContent = "";

    try {
        const response = await fetch("/api/devices");

        if (!response.ok) {
            throw new Error("Failed to fetch devices.");
        }

        const devices = await response.json();

        updateSummary(devices);
        renderDevices(devices);

        lastUpdated.textContent =
            `Last updated: ${new Date().toLocaleString()}`;

    } catch (error) {
        errorMessage.textContent =
            "Unable to connect to the Smart Home Hub.";

        console.error(error);

    } finally {
        isLoading = false;
        refreshButton.disabled = false;
    }
}

// Manual refresh

refreshButton.addEventListener("click", loadDashboard);

// Initial load

loadDashboard();

// Automatic refresh every 5 seconds

setInterval(loadDashboard, 5000);
// References to dashboard HTML elements.
const devicesContainer = document.getElementById("devices-container");
const refreshButton = document.getElementById("refresh-button");
const lastUpdated = document.getElementById("last-updated");
const errorMessage = document.getElementById("error-message");

// Prevents overlapping dashboard refresh requests.
let isLoading = false;

/**
 * Converts device uptime from seconds into a readable
 * hours, minutes, and seconds format.
 */
function formatUptime(seconds) {
    if (seconds == null) return "N/A";

    const total = Number(seconds);

    if (!Number.isFinite(total)) return "N/A";

    const hours = Math.floor(total / 3600);
    const minutes = Math.floor((total % 3600) / 60);
    const remainingSeconds = Math.floor(total % 60);

    return `${hours}h ${minutes}m ${remainingSeconds}s`;
}

/**
 * Formats a timestamp using the user's local date and time format.
 */
function formatDate(date) {
    if (!date) return "Never";

    return new Date(date).toLocaleString();
}

/**
 * Creates an information row containing a label and value.
 */
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

/**
 * Creates a device card displaying its connection status,
 * network information, and available metrics.
 */
function createDeviceCard(device) {
    const card = document.createElement("article");
    card.className = "device-card";

    // Creates the device card header.
    const header = document.createElement("div");
    header.className = "device-header";

    const name = document.createElement("h3");
    name.textContent = device.name;

    const status = document.createElement("span");

    // Sets the status badge according to device availability.
    status.className = device.isOnline
        ? "status-badge status-online"
        : "status-badge status-offline";

    status.textContent = device.isOnline
        ? "ONLINE"
        : "OFFLINE";

    header.append(name, status);

    // Creates the device information container.
    const info = document.createElement("div");
    info.className = "device-info";

    // Retrieves available device metrics.
    const metrics = device.info?.metrics ?? {};

    // Displays device properties and measurements.
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

/**
 * Calculates and updates total, online, and offline
 * device statistics on the dashboard.
 */
function updateSummary(devices) {
    const total = devices.length;
    const online = devices.filter(device => device.isOnline).length;

    document.getElementById("total-devices").textContent = total;
    document.getElementById("online-devices").textContent = online;
    document.getElementById("offline-devices").textContent = total - online;
}

/**
 * Renders all monitored devices in the dashboard grid.
 */
function renderDevices(devices) {
    // Removes previously displayed device cards.
    devicesContainer.replaceChildren();

    // Displays a message when no devices are available.
    if (devices.length === 0) {
        devicesContainer.textContent = "No devices found.";
        return;
    }

    // Creates and displays a card for each device.
    devices.forEach(device => {
        devicesContainer.appendChild(createDeviceCard(device));
    });
}

/**
 * Retrieves current device data from the backend REST API
 * and updates the dashboard.
 */
async function loadDashboard() {
    // Prevents multiple simultaneous refresh requests.
    if (isLoading) return;

    isLoading = true;
    refreshButton.disabled = true;
    errorMessage.textContent = "";

    try {
        // Sends a request to the device monitoring API.
        const response = await fetch("/api/devices");

        if (!response.ok) {
            throw new Error("Failed to fetch devices.");
        }

        // Converts the JSON response into JavaScript data.
        const devices = await response.json();

        // Updates dashboard statistics and device cards.
        updateSummary(devices);
        renderDevices(devices);

        // Displays the time of the last successful update.
        lastUpdated.textContent =
            `Last updated: ${new Date().toLocaleString()}`;

    } catch (error) {
        // Displays an error message if API communication fails.
        errorMessage.textContent =
            "Unable to connect to the Smart Home Hub.";

        console.error(error);

    } finally {
        // Restores the refresh button after the request completes.
        isLoading = false;
        refreshButton.disabled = false;
    }
}

// Enables manual dashboard refresh.
refreshButton.addEventListener("click", loadDashboard);

// Loads dashboard data when the page is opened.
loadDashboard();

// Automatically refreshes dashboard data every 5 seconds.
setInterval(loadDashboard, 5000);
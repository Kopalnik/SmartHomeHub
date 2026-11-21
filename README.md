# Smart Home Hub

## Project Overview

Smart Home Hub is a web-based device monitoring application
developed as part of DataArt IT Camp 2026.

The application communicates with virtual smart home devices
and provides a dashboard for monitoring their availability
and key metrics.

## Features

- Real-time device monitoring
- Online and offline device detection
- Total, online and offline device counters
- Temperature monitoring
- Device uptime monitoring
- IP address and API URL display
- Last seen timestamp
- Automatic refresh every 5 seconds
- Manual refresh
- Responsive dark-themed dashboard
- Error handling

## Technologies

### Backend

- C#
- .NET 11
- ASP.NET Core Web API
- HttpClient
- REST API

### Frontend

- HTML5
- CSS3
- JavaScript

## Prerequisites

- .NET 11 SDK
- Git
- HomeManagementDeviceApi emulator

## Getting Started

### 1. Clone the device emulator

```bash
git clone https://github.com/VladislavAntonyuk/HomeManagementDeviceApi.git
cd HomeManagementDeviceApi
```

### 2. Configure HTTPS development certificates

```bash
dotnet dev-certs https --trust
```

On Linux, if required:

```bash
export SSL_CERT_DIR="$HOME/.aspnet/dev-certs/trust:/usr/lib/ssl/certs"
```

### 3. Start Device1

```bash
dotnet run --launch-profile Device1
```

### 4. Start Device2 in a separate terminal

```bash
dotnet run --launch-profile Device2
```

### 5. Start Smart Home Hub

Open a separate terminal in the SmartHomeHub directory:

```bash
dotnet restore
dotnet build
dotnet run --no-launch-profile --urls http://localhost:5000
```

### 6. Open the dashboard

Open the following address in your browser:

http://localhost:5000

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/devices | Returns device information and status |
| GET | /api/devices/summary | Returns device statistics |

## Testing

The following scenarios were tested manually:

- Both devices online
- One device offline
- Device reconnection
- Automatic dashboard refresh
- Device statistics updates

## Credits

The device emulator was provided as part of DataArt
IT Camp 2026 workshop materials.

## Project Status

Device monitoring dashboard implemented and tested.
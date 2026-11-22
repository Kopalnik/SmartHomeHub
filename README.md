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
- Node.js and npm (required for MCP Inspector)

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

Open a separate terminal and clone the Smart Home Hub repository:

```bash
git clone https://github.com/Kopalnik/SmartHomeHub.git
cd SmartHomeHub
dotnet restore
dotnet build
dotnet run --no-launch-profile --urls http://localhost:5002
```

### 6. Open the dashboard

Open the following address in your browser:

http://localhost:5002

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

## Workflow Automation Engine

The application includes a background workflow engine
that automatically executes device actions based on
defined triggers and conditions.

### Supported Features

- Creating and deleting workflows
- Enabling and disabling workflows at creation
- Automatic execution every 10 seconds
- Condition-based execution
- Device availability verification
- Execution logging and error handling

### Supported Triggers

- `interval` - periodic workflow execution

### Supported Conditions

- `always` - executes without checking device availability
- `online` - executes only when the selected device is online

### Supported Actions

- `INFO` - retrieves device information and metrics

### Workflow API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/workflows | Returns all workflows |
| POST | /api/workflows | Creates a new workflow |
| DELETE | /api/workflows/{id} | Deletes a workflow |

### Example Workflow

```json
{
  "name": "Monitor Online Device1",
  "isEnabled": true,
  "trigger": "interval",
  "deviceName": "Device1",
  "condition": "online",
  "action": "INFO"
}
```

## AI Integration (MCP)

The application includes a Model Context Protocol (MCP)
server that enables AI clients to interact with
smart home devices.

### Supported Features

- Discovering available smart home devices
- Retrieving device information and status
- Accessing device configuration
- Executing supported device actions
- Integration with MCP-compatible clients

### Available MCP Tools

| Tool | Description |
|------|-------------|
| get_devices | Returns all devices and their current status |
| get_device_actions | Returns available actions for a selected device |
| execute_device_action | Executes the supported INFO action |

### Current Limitations

- Device interaction is currently limited to the INFO action.
- Other device commands are not enabled.

### Connecting with MCP Inspector

Make sure the Smart Home Hub application is running.

Start MCP Inspector in a separate terminal:

```bash
npx @modelcontextprotocol/inspector
```

Configure the connection:

- Transport Type: Streamable HTTP
- URL: http://localhost:5002/mcp

Click Connect, open the Tools tab and click List Tools.

The available tools can then be selected and executed
directly from MCP Inspector.

### Testing

The MCP server was tested using MCP Inspector
to verify communication between the client
and the Smart Home Hub.

## Credits

The device emulator was provided as part of DataArt
IT Camp 2026 workshop materials.

## Project Status

- [x] Main Task - Device Monitoring Dashboard
- [x] Optional Challenge 1 - AI Integration (MCP)
- [x] Optional Challenge 2 - Workflow Automation Engine

All three components have been implemented and
manually tested in a local development environment.
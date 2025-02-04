# HumanChat – Azure Functions API

**HumanChat** is a multi-tenant chat system built with **Azure Functions** and organized in a **vertical slice** architecture. 

Each slice (Tenants, Organizations, Agents, Chats, Messages) has separate folders for **commands** (create/update/delete) and **queries** (get/list).

## Table of Contents

1. [Overview](#overview)
2. [Project Structure](#project-structure)
3. [Endpoints](#endpoints)
4. [Vertical Slice Architecture](#vertical-slice-architecture)
5. [Prerequisites](#prerequisites)
6. [Running Locally](#running-locally)
7. [Testing via Postman](#testing-via-postman)
8. [License](#license)

---

## Overview

This repository implements:

- **Multi-tenancy**: A single system can host multiple *Tenants*.
- **Organizations**: Subdivisions under a Tenant.
- **Agents**: Users who handle chat conversations for each organization.
- **Chats**: Live chat sessions with a status (open/closed).
- **Messages**: Individual messages within a chat (sender, content, timestamp).

All endpoints are exposed via **Azure Functions**’ HTTP triggers, each with a consistent route format:


Each **feature** (Tenants, Organizations, Agents, Chats, Messages) has subfolders for **Commands** and **Queries**. For example, you’ll see files like:

- **CreateTenantFunction.cs**, **UpdateTenantFunction.cs** in `Features/Tenants/Commands`.
- **GetTenantByIdFunction.cs**, **ListTenantsFunction.cs** in `Features/Tenants/Queries`.
- Similar pattern for **Organizations**, **Agents**, **Chats**, **Messages**.

---

## Project Structure

The project is organized into the following folders:

```
HumanChat.Application
├─ Program.cs               
├─ Features
│   ├─ Tenants
│   │   ├─ Commands
│   │   │   └─ CreateTenant, UpdateTenant, etc.
│   │   └─ Queries
│   ├─ Organizations
│   ├─ Agents
│   ├─ Chats
│   └─ Messages
├─ Entities
├─ Infrastructure
│   ├─ Persistence
│   ├─ Interceptors
│   └─ Migrations


```

---


## Endpoints

Below is a high-level summary of the HTTP routes.

### Tenants

| Operation       | Method | Route                           | Description                     |
|-----------------|--------|---------------------------------|---------------------------------|
| Create Tenant   | POST   | `/api/tenants`                  | Creates a new Tenant            |
| Get Tenant      | GET    | `/api/tenants/{tenantId}`       | Retrieves an existing Tenant    |
| Update Tenant   | PATCH  | `/api/tenants/{tenantId}`       | Partially updates a Tenant      |

### Organizations

| Operation          | Method | Route                                                           | Description                           |
|--------------------|--------|-----------------------------------------------------------------|---------------------------------------|
| Create Organization| POST   | `/api/tenants/{tenantId}/organizations`                        | Creates an Organization under a Tenant|
| List Organizations | GET    | `/api/tenants/{tenantId}/organizations`                        | Lists Organizations for a Tenant      |
| Get Organization   | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}`        | Retrieves an Organization             |
| Update Organization| PATCH  | `/api/tenants/{tenantId}/organizations/{organizationId}`        | Partially updates an Organization     |

### Agents

| Operation      | Method | Route                                                                 | Description                                     |
|----------------|--------|-----------------------------------------------------------------------|-------------------------------------------------|
| Create Agent   | POST   | `/api/tenants/{tenantId}/organizations/{organizationId}/agents`       | Creates an Agent in an Organization            |
| List Agents    | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/agents`       | Lists Agents in an Organization                |
| Get Agent      | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/agents/{agentId}` | Retrieves an Agent                              |
| Update Agent   | PATCH  | `/api/tenants/{tenantId}/organizations/{organizationId}/agents/{agentId}` | Partially updates an Agent (e.g., status)       |

### Chats

| Operation      | Method | Route                                                                                           | Description                                       |
|----------------|--------|------------------------------------------------------------------------------------------------|---------------------------------------------------|
| Create (Start) | POST   | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/start`                           | Creates (starts) a new Chat (status=“open”)       |
| List Chats     | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/chats`                                 | Lists all Chats in an Organization               |
| Get Chat       | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}`                        | Retrieves a specific Chat                         |
| Close Chat     | POST   | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}/close`                  | Closes a Chat (status=“closed”)                  |

### Messages

| Operation         | Method | Route                                                                                                         | Description                                |
|-------------------|--------|---------------------------------------------------------------------------------------------------------------|--------------------------------------------|
| List Messages     | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}/messages`                             | Lists messages in a Chat                   |
| Get Message by ID | GET    | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}/messages/{messageId}`                 | Retrieves a single Message in a Chat       |
| Send Message      | POST   | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}/messages`                             | Creates (sends) a new Message in a Chat    |
| Update Message    | PATCH  | `/api/tenants/{tenantId}/organizations/{organizationId}/chats/{chatId}/messages/{messageId}`                 | Partially updates a Message (e.g., content)|

---

## Vertical Slice Architecture

1. **Commands**: Create, update, or delete operations. Each command folder contains:
    - A **request** record (e.g. `CreateTenantRequest`, `UpdateOrganizationRequest`).
    - A **function** class (e.g. `CreateTenantFunction`), which does the logic.

2. **Queries**: Read/get operations. Each query folder contains:
    - A **request** record if needed (e.g. `ListAgentsRequest`, `GetChatByIdRequest`).
    - A **function** class (e.g. `ListAgentsFunction`).

3. This approach groups **function code + request models** by feature. It avoids mixing all requests and models in one global folder.

---

## Prerequisites

- **.NET 7 or later**
- **Azure Functions Core Tools** if you want to run locally.
- **EF Core** + your **HumanChatDbContext**.

---

## Running Locally

1. Clone the repository.
2. `dotnet restore`
3. `dotnet build`
4. Run:
   ```bash
   func start
   

---

## Testing via Postman

You can test the API endpoints using Postman. Here’s a sample collection:

[![Run in Postman](https://run.pstmn.io/button.svg)](TBD)

---

## License


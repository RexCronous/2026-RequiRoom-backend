# RequiRoom - Backend
> Campus Room Reservation System built with ASP.NET Core Web API

## Description
RequiRoom is a backend system designed to manage campus room reservations in a centralized and structured manner.

The system replaces manual booking methods (chat, spreadsheets, paper-based records) with a secure, role-based web API that supports room management, reservation approval workflow, and status tracking.

This project is developed as part of **PBL 2026 Track RPL** using industry-standard Git workflow and professional development practices.

## Features

> **User Management**
> - Role-based System:
>  -- Admin
>  -- Reservator
>  - Secure password hashing
>  - User registration and management

>  **Room Management**
>  - Create room **( admin )**
>  - Update room
>  - Delete room
>  - List all rooms
>  - Availability tracking

> **Reservation System**
> - Reservator:
> -- Apply for reservation
> -- View on reservations
> - Admin:
> -- View all reservations
> -- Approve reservations
> -- Reject reservations

> **Reservation Workflow**
> - Reservation Status
> -- `pending`
> -- `Approved`
> -- `Rejected`
> -- `Cancelled`
> - Approval stores:
> -- Approval ID
> -- Approval timestamp
> Security
> - Role-based authorization
> Password hashing
> Input validation via DTO + DataAnnotations

## Tech Stack
- ASP.NET Core Web API
- SQL Server

## Author
> RexCronous

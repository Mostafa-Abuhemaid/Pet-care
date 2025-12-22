# 🐾 PetPaw – ASP.NET Core Web API

**PetPaw** is a comprehensive backend system built with **ASP.NET Core Web API**, designed to handle multiple real-world domains related to pet care services.
The project focuses on **clean API design, scalability, performance, and real production practices**, rather than simple CRUD operations.

---

## 🚀 Project Overview

PetPaw provides a complete backend solution for a pet-care platform, covering:

- User authentication & account management
- Pet profiles & breeding services
- Vet clinics & appointment booking
- Boarding services
- E-commerce features (products, cart, orders, payments)
- AI-powered smart notifications

The system is built with a **layered architecture**, applying widely-used backend patterns and best practices.

---

## 🧠 Key Concepts & Principles Applied

- RESTful API design
- Separation of concerns
- Async & background processing
- Secure authentication & authorization
- Performance optimization
- Realistic data handling

---

## 🔧 Technologies & Tools Used

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **JWT Bearer Authentication**
- **OpenAPI / Swagger**
- **Gemini AI Integration**
- **Background Jobs**
- **SMTP Email Service**
- **Caching**
- **Pagination & Filtering**
- **Multipart File Uploads**

---

## 🧩 Design Patterns & Libraries

- **Repository Pattern** – clean separation of data access logic
- **Result Pattern** – unified handling of success & failure responses
- **FluentValidation** – strong request validation
- **Mapster** – efficient DTO ↔ Entity mapping
- **Options Pattern** – centralized configuration management

---

## 🤖 AI & Background Processing

- Integrated **Gemini AI** to generate smart daily notifications for users
- Used **Background Jobs** to:

  - Run AI generation asynchronously
  - Avoid blocking API requests
  - Improve performance and reliability

---

## 📊 Data Collection & Initialization

- **Web Scraping**

  - Used to collect realistic data from external sources
  - Helped simulate real production-like data

- **Data Seeding**

  - Pre-populated the database with meaningful initial data
  - Improved development, testing, and demo experience

---

## 📦 Main API Modules

### 🔐 Account & Authentication

- Login & Signup
- JWT-based authentication
- Forgot password with OTP verification
- User profile, history, and favorites

### 👤 User Management (Admin)

- Lock / Unlock user accounts
- Delete users by email
- Retrieve all users

### 🐾 Pets & Breeding

- Create, update, delete pets
- Pet medical details & images
- Breeding availability services

### 🏥 Vet Clinics

- Vet profiles & services
- Appointment booking system
- Available slots
- Booking confirmation
- Reviews & booking file downloads

### 🏠 Boarding Services

- Boarding places management
- Reviews & services
- Advanced filtering (location, price, availability)

### 🛒 Products & E-Commerce

- Product listing & search
- Categories
- Best sellers & special offers
- Shopping cart
- Promo codes

### 💳 Orders & Payments

- Order confirmation
- Payment intent generation (Stripe-like flow)

### 🤖 Smart Notifications

- AI-generated daily notifications
- User-specific notification retrieval

---

## 📑 API Documentation

The API is fully documented using **Swagger / OpenAPI**, making it easy to explore, test, and understand all endpoints.

---

## 🧪 Additional Features

- Consistent API responses
- Strong validation & error handling
- Secure file uploads
- Performance-focused querying

---

## 🤝 Acknowledgment

Special thanks to **Eng. Mostafa Abuhemaid**
GitHub: **[@Mostafa-Abuhemaid](https://github.com/Mostafa-Abuhemaid)**

His guidance and support were very valuable throughout the development of this project.

---

## 🔗 Repository

👉 **GitHub Repo:**
[https://github.com/Ahmed40988/Pet-care](https://github.com/Ahmed40988/Pet-care)

---

## 📌 Final Notes

This project represents a strong hands-on experience with **real backend challenges**, including API design, AI integration, async processing, and scalable system structure.

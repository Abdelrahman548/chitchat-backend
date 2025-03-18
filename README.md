### 🚀 ChitChat - Chatting Application  

#### ⚡ Features & Technologies Used:  

- **🔒 Authentication & Security**  
  - 🔑 OTP for email verification & password reset  
  - 🛡️ JWT Authentication  
  - 🔐 Password hashing using `BCrypt.Net-Next`  

- **📧 Email Services**  
  - ✉️ Sending emails using `MailKit` & Google SMTP  

- **💬 Real-Time Communication**  
  - ⚡ Implemented **SignalR** for real-time chat  

- **🏗️ Architecture & Design Patterns**  
  - **🗂️ Layered Architecture**  
    - 📁 Data Layer  
    - 📦 Repository Layer  
    - ⚙️ Service Layer  
    - 🌐 Web API Layer  
  - 📌 Repository Pattern  
  - 🔄 Unit of Work Pattern  

- **💾 Database**  
  - 🍃 MongoDB  

- **⚙️ Environment & Configuration**  
  - 🔑 Storing sensitive information in `.env` file using `DotNetEnv`  

- **🛠️ Utilities & Enhancements**  
  - 🔄 `AutoMapper` for mapping between Entities & DTOs  
  - ☁️ `CloudinaryDotNet` for cloud storage (videos & photos)  
  - 🧠 Memory cache for rate limiting middleware  
  - 🔑 Static API Key Middleware  
  - ⚠️ Custom Exception Handling Middleware  
  - 📡 Unified API responses for easier integration  
# 🛒 E-Commerce Website

![GitHub repo size](https://img.shields.io/github/repo-size/maigiaminh/E-Commerce-Web?color=blue&style=flat-square)
![GitHub last commit](https://img.shields.io/github/last-commit/maigiaminh/E-Commerce-Web?color=green&style=flat-square)
![GitHub stars](https://img.shields.io/github/stars/maigiaminh/E-Commerce-Web?style=social)

📌 **A full-featured online shopping platform with secure payment integration, order tracking, and an intuitive admin dashboard.**  
💰 Built with **ASP.NET Core, Entity Framework, SQL Server, and VNPay/Momo API** for a seamless shopping experience.

---

## 📖 Table of Contents
- [🌟 Features](#-features)
- [📸 Screenshots](#-screenshots)
- [🎥 Video Demo](#-video-demo)
- [⚙️ Installation](#️-installation)
- [🚀 Usage](#-usage)
- [🛠 Technologies](#-technologies)
- [🙌 Contributing](#-contributing)
- [📄 License](#-license)
- [📩 Contact](#-contact)

---

## 🌟 Features
✅ **User authentication (Register, Login, Role-based Access)**  
✅ **Product catalog with categories and search functionality**  
✅ **Shopping cart and checkout system**  
✅ **Secure payment gateway integration (VNPay/Momo API)**  
✅ **Admin dashboard for managing products, users, and orders**  
✅ **Order tracking and invoice generation**  
✅ **Wishlist and product recommendations**  
✅ **Responsive design for both desktop and mobile**  

---

## 📸 Screenshots
### 🏠 Homepage
![Homepage Screenshot](https://raw.githubusercontent.com/maigiaminh/E-Commerce-Web/main/assets/images/ecommerce-homepage.png)



### 🛍️ Product Page
![Product Page Screenshot](https://raw.githubusercontent.com/maigiaminh/E-Commerce-Web/main/assets/images/ecommerce-product.png)



### 🛒 Shopping Cart
![Shopping Cart Screenshot](https://raw.githubusercontent.com/maigiaminh/E-Commerce-Web/main/assets/images/ecommerce-cart.png)



### 🧾 Invoice
![Invoice Screenshot](https://raw.githubusercontent.com/maigiaminh/E-Commerce-Web/main/assets/images/ecommerce-invoice.png)



---

## 🎥 Video Demo
[![Watch the video](https://img.youtube.com/vi/8yPZAbMdChk/maxresdefault.jpg)](https://www.youtube.com/watch?v=8yPZAbMdChk)



---

## ⚙️ Installation

### **🔧 Prerequisites**
- Install **.NET 6.0 SDK**, **SQL Server**, **Visual Studio**  
- Clone this repository:
```sh
git clone https://github.com/maigiaminh/E-Commerce-Web.git
cd E-Commerce-Web
```

### **📦 Setup Database**
1. Open SQL Server and create a new database called `ECommerceDB`
2. Open the project and modify `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
3. Run migration:
```sh
dotnet ef database update
```

### **💻 Run the Application**
```sh
dotnet run
```
App will run on **http://localhost:61377**

---

## 🚀 Usage
1️⃣ **Users can register/login**  
2️⃣ **Browse products and add to cart**  
3️⃣ **Checkout securely via VNPay/Momo API**  
4️⃣ **Admin can manage products, users, and orders**  

---

## 🛠 Technologies
- **Frontend:** Razor Pages, Bootstrap, JavaScript
- **Backend:** ASP.NET Core, C#, Entity Framework
- **Database:** SQL Server
- **Payment Integration:** VNPay API
- **Authentication:** ASP.NET Identity
- **Deployment:** Azure, AWS, or Heroku

---

## 🙌 Contributing
🛠 Want to contribute? **Pull requests are welcome!**
1. **Fork this repo**  
2. **Create a new branch** (`git checkout -b feature-branch`)
3. **Commit your changes** (`git commit -m "Add new feature"`)
4. **Push to the branch** (`git push origin feature-branch`)
5. **Submit a Pull Request**

---

## 📄 License
📜 This project is licensed under the **MIT License**.

---

## 📩 Contact
📧 **Email:** minh.mgia@gmail.com  
🔗 **GitHub:** [maigiaminh](https://github.com/maigiaminh)  
🌍 **Portfolio:** [https://maigiaminh.me](https://maigiaminh.me)


# SupportFlow

SupportFlow, modern mikroservis mimarisi üzerine inşa edilmiş bir müşteri destek ve bilet (ticket) yönetim sistemidir. .NET 9.0 ve en yeni teknolojiler kullanılarak geliştirilmiştir.

## 🚀 Proje Hakkında

SupportFlow, müşterilerin destek taleplerini oluşturabildiği, şirketlerin bu talepleri yönetebildiği ve gerçek zamanlı bildirimlerin gönderildiği ölçeklenebilir bir platformdur. Proje, bağımsız servislerin bir araya gelerek oluşturduğu esnek bir yapıya sahiptir.

## 🛠️ Teknoloji Yığını

-   **Backend:** .NET 9.0 (ASP.NET Core Web API)
-   **API Gateway:** YARP (Yet Another Reverse Proxy)
-   **Database:** MS SQL Server (Entity Framework Core)
-   **Messaging:** MassTransit & RabbitMQ (Olay tabanlı iletişim için)
-   **Inter-service Communication:** Refit (Tip güvenli HTTP istemcisi)
-   **Authentication:** JWT (JSON Web Token) & ASP.NET Core Identity
-   **Architecture:** Mikroservis Mimarisi, N-Layered Architecture

## 📂 Servis Yapısı

Proje aşağıdaki temel mikroservislerden oluşmaktadır:

1.  **Auth Service (`SupportFlow.Auth`):** Kullanıcı kaydı, girişi ve yetkilendirme (JWT) süreçlerini yönetir.
2.  **Customer Service (`SupportFlow.Customer`):** Şirket ve müşteri bilgilerinin yönetimini sağlar.
3.  **Ticket Service (`SupportFlow.Ticket`):** Destek taleplerinin (ticket) oluşturulması, güncellenmesi ve yorum süreçlerini yönetir.
4.  **Notification Service (`SupportFlow.Notification`):** Sistemdeki olayları (bilet oluşturulması vb.) dinler ve ilgili birimlere bildirimler gönderir.
5.  **Yarp Gateway (`SupportFlow.YarpGateway`):** Dış dünyadan gelen tüm istekleri karşılayan ve ilgili mikroservislere yönlendiren giriş kapısıdır.
6.  **Messaging Shared (`SupportFlow.Messaging`):** Servisler arası asenkron iletişimde kullanılan ortak olay (event) tanımlarını içerir.

## 🏗️ Mimari ve İletişim

-   **Gateway Yönlendirmesi:** Tüm API istekleri `/api/{service-name}/...` yolu üzerinden Gateway'e gelir ve Gateway bu istekleri ilgili servise yönlendirir.
-   **Asenkron İletişim:** Bir bilet oluşturulduğunda veya güncellendiğinde, `Ticket Service` üzerinden RabbitMQ'ya bir event fırlatılır. `Notification Service` bu eventleri dinleyerek gerekli aksiyonları alır.
-   **Senkron İletişim:** Servisler arası doğrudan veri ihtiyacı olduğunda Refit kullanılarak HTTP üzerinden haberleşme sağlanır.

## ⚙️ Kurulum ve Çalıştırma

### Gereksinimler
-   .NET 9.0 SDK
-   MS SQL Server
-   RabbitMQ (Docker üzerinden çalıştırılabilir)

### Çalıştırma Adımları
1.  **Veritabanı:** Her servisin (Auth, Customer, Ticket) `appsettings.json` dosyasındaki ConnectionString bilgilerini kendi SQL Server ayarlarınıza göre güncelleyin.
2.  **Migration:** Her veri erişim katmanında (DataAccess) `Update-Database` komutunu çalıştırarak veritabanı tablolarını oluşturun.
3.  **RabbitMQ:** RabbitMQ'nun yerel makinenizde (veya belirtilen hostta) çalıştığından emin olun.
4.  **Çoklu Başlatma:** Projeyi Visual Studio üzerinden "Multiple Startup Projects" seçeneğiyle tüm API servislerini ve Gateway'i başlatacak şekilde yapılandırın.

## 🛣️ API Rotaları
-   **Gateway URL:** `https://localhost:7001` (Tüm istekler bu adres üzerinden yönlendirilir)
-   **Auth Service:** `/api/auth`
-   **Customer Service:** `/api/customer`
-   **Ticket Service:** `/api/ticket`

---

## **API Referansı**

Tüm API istekleri için temel URL: `https://localhost:7001`

### **1. Kimlik Doğrulama (Auth Service)**

#### **Kullanıcı Kaydı**
- **Metot:** `POST`
- **Yol:** `/api/auth/register`
- **Açıklama:** Yeni bir kullanıcı hesabı oluşturur ve oturum açma bilgilerini (JWT) döner.
- **Girdi (Body):**
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!",
    "fullName": "Ahmet Yılmaz",
    "companyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  }
  ```
- **Başarılı Yanıt (200 OK):**
  ```json
  {
    "accessToken": "eyJhbGci...",
    "refreshToken": "abcdef...",
    "expiration": "2026-03-14T12:00:00Z"
  }
  ```
- **Hata Yanıtı (400 Bad Request):** `{"message": "Kullanıcı adı zaten alınmış."}`

#### **Kullanıcı Girişi**
- **Metot:** `POST`
- **Yol:** `/api/auth/login`
- **Açıklama:** Email ve şifre ile giriş yapar, JWT token döner.
- **Girdi (Body):**
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!"
  }
  ```
- **Başarılı Yanıt (200 OK):** (Kayıt ile aynı şema)

#### **Kullanıcı Detaylarını Getir**
- **Metot:** `GET`
- **Yol:** `/api/auth/users/{id}`
- **Açıklama:** Belirtilen ID'ye sahip kullanıcının temel bilgilerini getirir.
- **Başarılı Yanıt (200 OK):**
  ```json
  {
    "id": "3fa85f64...",
    "fullName": "Ahmet Yılmaz",
    "email": "user@example.com"
  }
  ```

---

### **2. Şirket Yönetimi (Customer Service)**

#### **Tüm Şirketleri Listele**
- **Metot:** `GET`
- **Yol:** `/api/customer/companies`
- **Açıklama:** Sistemdeki tüm aktif şirketleri listeler.
- **Başarılı Yanıt (200 OK):**
  ```json
  [
    {
      "id": "3fa85f64...",
      "name": "ABC Yazılım",
      "email": "contact@abc.com",
      "isActive": true
    }
  ]
  ```

#### **Yeni Şirket Oluştur**
- **Metot:** `POST`
- **Yol:** `/api/customer/companies`
- **Açıklama:** Yeni bir müşteri şirketi sisteme kaydeder.
- **Girdi (Body):**
  ```json
  {
    "name": "Yeni Şirket A.Ş.",
    "taxNumber": "1234567890",
    "email": "info@yenisirket.com",
    "address": "İstanbul, Türkiye"
  }
  ```
- **Başarılı Yanıt (200 OK):**
  ```json
  {
    "id": "new-guid-value",
    "message": "Şirket başarıyla oluşturuldu."
  }
  ```

---

### **3. Destek Talepleri (Ticket Service)**
*Bu bölümdeki tüm istekler için `Authorization: Bearer {token}` başlığı zorunludur.*

#### **Yeni Destek Talebi Oluştur**
- **Metot:** `POST`
- **Yol:** `/api/ticket/Tickets`
- **Açıklama:** Oturum açan kullanıcının şirketi adına yeni bir destek bileti oluşturur.
- **Girdi (Body):**
  ```json
  {
    "title": "Bağlantı Sorunu",
    "description": "Sisteme giriş yaparken hata alıyorum.",
    "priority": 1
  }
  ```
  *(Priority: 0: Low, 1: Medium, 2: High, 3: Urgent)*
- **Başarılı Yanıt (200 OK):**
  ```json
  {
    "id": "ticket-guid",
    "message": "Destek Talebiniz Başarıyla Alındı"
  }
  ```

#### **Şirketimin Taleplerini Listele**
- **Metot:** `GET`
- **Yol:** `/api/ticket/Tickets`
- **Açıklama:** Kullanıcının bağlı olduğu şirkete ait tüm biletleri getirir.
- **Başarılı Yanıt (200 OK):**
  ```json
  [
    {
      "id": "guid",
      "title": "Bağlantı Sorunu",
      "status": 0,
      "priority": 1,
      "createdAt": "2026-03-14T10:00:00Z"
    }
  ]
  ```
  *(Status: 0: Open, 1: InProgress, 2: Resolved, 3: Closed)*

#### **Bilet Durumunu Güncelle**
- **Metot:** `PATCH`
- **Yol:** `/api/ticket/Tickets/{id}/status`
- **Açıklama:** Bir biletin durumunu (Açık, İşlemde, Çözüldü vb.) günceller.
- **Girdi (Body):**
  ```json
  {
    "status": 1,
    "assignedStaffId": "staff-guid-optional"
  }
  ```

#### **Yorum Ekle**
- **Metot:** `POST`
- **Yol:** `/api/ticket/Tickets/{id}/comments`
- **Açıklama:** Mevcut bir destek talebine yeni bir yorum ekler.
- **Girdi (Body):** `{"message": "Sorun çözüldü, teşekkürler."}`

#### **Boştaki Bilet Havuzu**
- **Metot:** `GET`
- **Yol:** `/api/ticket/Tickets/pool`
- **Açıklama:** Henüz kimseye atanmamış (Open durumundaki) tüm biletleri listeler (Destek Personeli için).

#### **Bileti Üzerine Al**
- **Metot:** `PATCH`
- **Yol:** `/api/ticket/Tickets/{id}/assign`
- **Açıklama:** Havuzdaki bir bileti oturum açan personelin üzerine atar.

---

## **Hata Durum Kodları**
- **200 OK:** İstek başarıyla tamamlandı.
- **400 Bad Request:** Geçersiz girdi veya iş mantığı hatası (Hata mesajı gövdede yer alır).
- **401 Unauthorized:** Geçersiz veya eksik JWT token.
- **404 Not Found:** İstenen kaynak (kullanıcı, şirket, bilet) bulunamadı.
- **500 Internal Server Error:** Beklenmedik sunucu hatası.

```
# SupportFlow API Documentation

## Genel Açıklama
SupportFlow, müşteri desteği süreçlerini yönetmek için bir API sunar. Bu API, kullanıcıların destek talepleri oluşturması, şirketlerin taleplerini yönetmesi ve destek personellerinin biletleri işlemek için kullanılabilir.

## API Endpoint'leri
Aşağıda, API'de bulunan temel endpoint'lerin listesi bulunmaktadır. Her endpoint'in detaylı açıklaması için ilgili bölümlere bakınız.

### **1. Kullanıcı Yönetimi (Auth Service)**
- `/api/auth/register` - Kullanıcı kaydı
- `/api/auth/login` - Kullanıcı girişi
- `/api/auth/users/{id}` - Kullanıcı detayları

### **2. Şirket Yönetimi (Customer Service)**
- `/api/customer/companies` - Tüm şirketleri listele
- `/api/customer/companies` - Yeni şirket oluştur

### **3. Destek Talepleri (Ticket Service)**
- `/api/ticket/Tickets` - Yeni destek talebi oluştur
- `/api/ticket/Tickets` - Şirketimin taleplerini listele
- `/api/ticket/Tickets/{id}/status` - Bilet durumu güncelle
- `/api/ticket/Tickets/{id}/comments` - Yorum ekle
- `/api/ticket/Tickets/pool` - Boştaki bilet havuzu
- `/api/ticket/Tickets/{id}/assign` - Bileti üzerine al

## Hata Durum Kodları
- **200 OK:** İstek başarıyla tamamlandı.
- **400 Bad Request:** Geçersiz girdi veya iş mantığı hatası (Hata mesajı gövdede yer alır).
- **401 Unauthorized:** Geçersiz veya eksik JWT token.
- **404 Not Found:** İstenen kaynak (kullanıcı, şirket, bilet) bulunamadı.
- **500 Internal Server Error:** Beklenmedik sunucu hatası.

```
# SupportFlow API Documentation

## Genel Açıklama
SupportFlow, müşteri desteği süreçlerini yönetmek için bir API sunar. Bu API, kullanıcıların destek talepleri oluşturması, şirketlerin taleplerini yönetmesi ve destek personellerinin biletleri işlemek için kullanılabilir.

## API Endpoint'leri
Aşağıda, API'de bulunan temel endpoint'lerin listesi bulunmaktadır. Her endpoint'in detaylı açıklaması için ilgili bölümlere bakınız.

### **1. Kullanıcı Yönetimi (Auth Service)**
- `/api/auth/register` - Kullanıcı kaydı
- `/api/auth/login` - Kullanıcı girişi
- `/api/auth/users/{id}` - Kullanıcı detayları

### **2. Şirket Yönetimi (Customer Service)**
- `/api/customer/companies` - Tüm şirketleri listele
- `/api/customer/companies` - Yeni şirket oluştur

### **3. Destek Talepleri (Ticket Service)**
- `/api/ticket/Tickets` - Yeni destek talebi oluştur
# SupportFlow
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

-   **Auth:** `https://localhost:6001/api/auth` (Gateway üzerinden: `https://localhost:5001/api/auth`)
-   **Customer:** `https://localhost:6101/api/customer` (Gateway üzerinden: `https://localhost:5001/api/customer`)
-   **Ticket:** `https://localhost:6201/api/ticket` (Gateway üzerinden: `https://localhost:5001/api/ticket`)

```
# SupportFlow
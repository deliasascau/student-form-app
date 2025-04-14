# Student Form Application

## Descriere
Această aplicație full stack permite completarea unui formular de student, stocarea datelor în SQL Server și generarea unui PDF cu datele introduse.

## Tehnologii utilizate
- **Backend:** ASP.NET Core Web API (.NET 7)
- **Frontend:** React
- **Bază de date:** SQL Server
- **PDF Generator:** DinkToPdf
- **Containerizare:** Docker & docker-compose
- (Opțional) Keycloak pentru autentificare

## Cum se rulează local
1. Asigură-te că Docker Desktop este pornit.
2. Din directorul proiectului, rulează:
   ```bash
   docker-compose up --build

# TimeTrackingSolution

## Wprowadzenie

Przykładowy system do ewidencji czasu pracy składający się z dwóch głównych części:

- **TimeTrackingAPI** – serwer REST zbudowany w oparciu o ASP.NET Core 6 i Entity Framework Core,
- **TimeTrackingMobile** – mobilna aplikacja Xamarin.Forms (Android, iOS oraz UWP).

Celem projektu jest ułatwienie rejestracji przepracowanych godzin oraz zarządzanie danymi takimi jak projekty, zadania czy pracownicy.

Poniższy opis zawiera skrócony przewodnik technologiczny oraz instrukcję uruchomienia obu aplikacji.

## Technologie

- **ASP.NET Core 6** – tworzenie usług HTTP
- **Entity Framework Core** – mapowanie obiektowo-relacyjne i migracje bazy
- **SQL Server** – przechowywanie danych
- **Xamarin.Forms** – warstwa prezentacji dla urządzeń mobilnych

Kod podzielony jest na dwa niezależne projekty współdzielone w ramach rozwiązania Visual Studio.

## Funkcjonalności

- zarządzanie pracownikami i działami firmy,
- ewidencja projektów, zadań oraz przypisanych tagów,
- rejestrowanie czasu pracy wraz z wygodnym timerem,
- prezentacja i edycja danych w aplikacji mobilnej.

## Wymagania

- [.NET 6 SDK](https://dotnet.microsoft.com/download) wraz z narzędziami do obsługi platform mobilnych (np. Visual Studio 2022 z modułem Xamarin)
- Dostęp do serwera **SQL Server** – baza może zostać odtworzona z pliku `TimeTrackingDB.bak`
- Opcjonalnie **Docker** do uruchamiania API w kontenerze

## Konfiguracja bazy danych

1. Utwórz bazę danych z wykorzystaniem pliku `TimeTrackingDB.bak` lub uruchom migracje z katalogu `TimeTrackingAPI/Migrations`.
2. W pliku `TimeTrackingAPI/appsettings.json` zaktualizuj ciąg połączenia `TimeTrackingDBContext` tak, aby wskazywał na Twoją instancję SQL Server.

Przykładowa konfiguracja:
```json
"ConnectionStrings": {
  "TimeTrackingDBContext": "Server=localhost;Database=TimeTrackingDB;Trusted_Connection=True;"
}
```

## Uruchomienie API

1. Przygotuj środowisko:
   ```bash
   dotnet restore TimeTrackingSolution.sln
   ```
2. Zbuduj projekt API i uruchom go:
   ```bash
   dotnet run --project TimeTrackingAPI/TimeTrackingAPI.csproj
   ```
   Domyślnie serwer nasłuchuje pod adresem `http://localhost:5215`. Dokumentacja Swagger dostępna będzie pod `/swagger`.

### Docker (opcjonalnie)
API można uruchomić w kontenerze Docker:
```bash
docker build -t timetrackingapi -f TimeTrackingAPI/Dockerfile .
docker run -p 5215:80 timetrackingapi
```

## Uruchomienie aplikacji mobilnej

1. Otwórz rozwiązanie `TimeTrackingSolution.sln` w Visual Studio.
2. W projektach z katalogu `TimeTrackingMobile` ustaw projekt właściwy dla wybranej platformy jako startowy (Android, iOS lub UWP).
3. Przed uruchomieniem zaktualizuj w plikach `Services/*Service.cs` stałe `BaseUrl`, aby wskazywały na adres działającego API (np. `http://localhost:5215`).
4. Uruchom aplikację na emulatorze lub podłączonym urządzeniu.

### Zrzuty ekranu

![Zrzut ekranu 2025-07-21 195232](https://github.com/user-attachments/assets/1e4edde3-0aeb-4f28-a2ba-7adcad2e7c11)

![Zrzut ekranu 2025-07-21 222602](https://github.com/user-attachments/assets/78985c01-7975-4099-84f9-0c12352c3916)

![Zrzut ekranu 2025-07-21 222627](https://github.com/user-attachments/assets/01dc37b5-7d8f-4f33-8ff3-e53f6459f9c7)

![Zrzut ekranu 2025-07-21 222709](https://github.com/user-attachments/assets/1a0f5000-f87a-4e5f-a5e5-d90447df385a)

![Zrzut ekranu 2025-07-21 222745](https://github.com/user-attachments/assets/10a2b92c-1841-4617-ab78-395c7205ce25)

![Zrzut ekranu 2025-07-21 222818](https://github.com/user-attachments/assets/28afa4f0-2a76-4a6e-8c53-834f7165e1e3)

![Zrzut ekranu 2025-07-21 222835](https://github.com/user-attachments/assets/eb8f75be-b59c-4fcc-993c-8c2bfbd57fcc)

![Zrzut ekranu 2025-07-21 222854](https://github.com/user-attachments/assets/1d9afe27-71fd-45a7-86f0-37e2fbb10738)

![Zrzut ekranu 2025-07-21 222924](https://github.com/user-attachments/assets/d2664398-e9ad-411e-bac6-b009b3faf67c)

## Struktura projektu

```
TimeTrackingAPI/      # Serwer REST z kontrolerami i modelem EF Core
TimeTrackingMobile/   # Aplikacja mobilna (Xamarin.Forms)
TimeTrackingDB.bak    # Kopia zapasowa bazy danych
TimeTrackingSolution.sln  # Plik rozwiązania Visual Studio
```

**Główne katalogi API**

- `Controllers` – implementacja końcówek REST dla wszystkich encji (CRUD)
- `Models` – klasy encji wygenerowane przez EF Core wraz z kontekstem `TimeTrackingDBContext`
- `Migrations` – pliki migracji bazy danych

**Główne katalogi aplikacji mobilnej**

- `Views` – definicje stron w XAML (np. DepartmentsPage, TimerPage)
- `ViewModels` – logika prezentacji dla poszczególnych widoków
- `Services` – klasy korzystające z `HttpClient` do komunikacji z API
- `Models` – obiekty DTO wykorzystywane w aplikacji

## Kontakt

Projekt udostępniony jest w celach edukacyjnych. Wszelkie sugestie i poprawki są mile widziane.

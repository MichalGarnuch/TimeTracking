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

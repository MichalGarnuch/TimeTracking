# TimeTracking — przewodnik i audyt edukacyjny

**Stan przeglądu:** 29 października 2026 r.  
**Gałąź bazowa:** `master`  
**Analizowany commit:** `05e593407956f6cbe4b7e7796cd2b01a5762a44d`  
**Mapa wszystkich plików śledzonych przez Git:** [1](AUDYT_MAPA_01.md)

> To jest przegląd reprezentatywnych części projektu z inwentaryzacją wszystkich śledzonych plików, a nie pełna, ręczna weryfikacja każdego wiersza ani certyfikat bezpieczeństwa. Mapa jest automatyczna; wymienienie pliku nie oznacza, że został przetestowany lub dokładnie przeczytany. Wyniki odnoszą się do powyższego commita. Inne gałęzie mogą zawierać inne wersje.

## Co robi projekt

System ewidencji czasu pracy z API ASP.NET oraz klientem Xamarin.Forms.

## Jak czytać kod i jak przepływają dane

[`TimeTrackingAPI/Program.cs`](TimeTrackingAPI/Program.cs) konfiguruje serwer i EF Core; [`TimeTrackingAPI/Controllers/TimeRecordController.cs`](TimeTrackingAPI/Controllers/TimeRecordController.cs#L14) realizuje operacje na zapisach czasu, a [`TimeTrackingMobile/`](TimeTrackingMobile) zawiera aplikację mobilną.

Podczas czytania zacznij od punktu wejścia, przejdź do funkcji odpowiadającej za żądanie lub działanie użytkownika, a potem do operacji na danych i wyniku. Mapa plików podaje ścieżki i automatycznie rozpoznane nazwy wybranych deklaracji.

## Co jest wartościowe w obecnym rozwiązaniu

Podział na API, modele danych i klienta mobilnego pokazuje niezależne role serwera i urządzenia.

## Ustalenia, luki i proponowany rozwój

- **Wysoki priorytet — dostęp do ewidencji:** w przejrzanym `Program.cs` nie ma konfiguracji uwierzytelniania, a `TimeRecordController` nie deklaruje ograniczenia `[Authorize]` i udostępnia również zapis/usuwanie. Jeśli API jest osiągalne, należy chronić je przed udostępnieniem prawdziwych danych i wdrożyć kontrolę użytkownika/roli.

- **Do rozwoju:** kontroler przyjmuje encję bezpośrednio i oznacza ją jako zmodyfikowaną. DTO, walidacja oraz testy uprawnień ograniczą ryzyko nadpisania niewłaściwych pól.

- **Do sprawdzenia:** repozytorium zawiera kopię bazy `.bak`; należy sprawdzić, czy nie ma w niej danych osobowych i czy powinna znajdować się w publicznym repozytorium. Nie analizowano zawartości backupu.

## Sprawdzenia i ich granice

Nie uruchomiono .NET/Xamarin ani SQL Server. Ustalenia dotyczą kodu, a nie potwierdzonego stanu jakiegokolwiek wdrożenia.

W kopii roboczej dostępne pliki Pythona sprawdzono dodatkowo parserem składni; sprawdzono też składnię odczytanych plików `package.json` i `composer.json`. Nie zastępuje to uruchomienia programu. Kod źródłowy tej gałęzi jest taki sam jak w commicie bazowym; dodane są wyłącznie dokumenty audytu i mapy.

## Jak zaplanować kolejne prace

Najpierw zajmij się punktami oznaczonymi jako krytyczne lub wysokie, następnie ustal, które uwagi wymagają testu na pełnym środowisku. Dla każdej zmiany przygotuj osobną gałąź, konkretny przypadek odtwarzający problem i test rezultatu. Nie traktuj propozycji jako potwierdzenia, że poprawka została już wdrożona.

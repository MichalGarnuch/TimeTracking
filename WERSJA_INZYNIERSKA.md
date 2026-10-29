# TimeTracking: jawne uwierzytelnienie OIDC i polityka zapisu

Branch: `wersja-inzynierska-2026`. Stan bazowy: `master` / `05e593407956f6cbe4b7e7796cd2b01a5762a44d`. Data wykonania: 29.10.2026.

To autorski wariant rozwojowy konkretnego fragmentu projektu. Branch zachowuje pełne drzewo kodu z wersji bazowej i zawiera opisane poniżej zmiany. [Wcześniejszy audyt](https://github.com/MichalGarnuch/TimeTracking/tree/audyt-edukacyjny-2026) jest na osobnym branchu. Nie wykonywano merge ani wdrożenia.

## Problem i granica zaufania

Projekt (ewidencja czasu pracy) wywoływał `UseAuthorization`, ale bez kompletnego mechanizmu rozpoznawania użytkownika i wymaganej polityki nie stanowiło to ochrony API. Dodałem `TimeTrackingAPI/Security/AuthenticationSetup.cs`, konfigurację usług przed `Build` i `UseAuthentication` przed `UseAuthorization`.

Wybrałem walidację tokenów Bearer wystawionych przez zewnętrzny serwer OIDC. Aplikacja nie generuje własnego stałego tokena ani hasła. Wymaga poprawnego `Authentication:Authority` z HTTPS oraz `Authentication:Audience`. Gdy wartości nie są ustawione, start jest zatrzymywany. To celowa, niezgodna ze starym anonimowym dostępem zmiana kontraktu.

## Jak działa polityka

Middleware sprawdza token zgodnie z konfiguracją wystawcy; nie akceptujemy samego odkodowania JWT jako dowodu tożsamości. Wyłączone mapowanie nazw claimów pozwala jawnie wskazać `Authentication:RoleClaimType` (domyślnie `roles`). Polityka `ApiWrite` wymaga uwierzytelnionej tożsamości i roli `ApiWriter`; pozostałe endpointy bez własnej polityki podlegają fallback wymagającemu użytkownika. Dopuszczalna różnica zegarów wynosi 30 sekund.

Polityka `ApiWrite` chroni operacje zapisu w kontrolerach działów, pracowników, projektów, typów projektów, tagów, zadań, powiązań tagów i wpisów czasu. Usunąłem sztywne wiązanie `http://0.0.0.0:5215`; adres jest konfigurowany przez hosta, np. `ASPNETCORE_URLS`, i konfigurację HTTPS. Uwierzytelnienie nie ogranicza jeszcze wpisów do konkretnego pracownika. Zalogowany czytelnik może nadal mieć zbyt szeroki wgląd w dane, więc kolejnym krokiem musi być autoryzacja rekordów.

## Konfiguracja przed uruchomieniem

Ustaw w konfiguracji środowiska `Authentication__Authority`, `Authentication__Audience` oraz w razie potrzeby `Authentication__RoleClaimType`. Wystawca musi wydawać token dla tego audience, z właściwą rolą w wybranym claimie. Klienci wysyłają go w `Authorization: Bearer ...`. Tokenów, haseł i connection stringów nie należy zapisywać w repozytorium. Nazwa roli musi pochodzić z zaufanego wystawcy; klient nie nadaje jej sam sobie.

Połączenie klient–API musi mieć TLS, również gdy OIDC discovery jest HTTPS. W środowisku z reverse proxy skonfiguruj zaufane proxy i protokół żądania; samo `UseHttpsRedirection` nie tworzy certyfikatu ani portu HTTPS. Dotychczasowe connection stringi bazy pozostają wymagane.

## Jak czytać testy i co dalej

`engineering-tests/Authentication.Tests.csproj` dołącza rzeczywisty plik konfiguracji z aplikacji. Test sprawdza odmowę dla anonimowego użytkownika i czytelnika, zgodę dla `ApiWriter` oraz odmowę konfiguracji bez audience lub z HTTP authority. Uruchomienie docelowe: `dotnet run --project engineering-tests/Authentication.Tests.csproj`.

Następna warstwa to test HTTP z podpisanymi tokenami: zły issuer, audience, podpis, wygasły token, brak roli i poprawny odczyt. Potem polityki zasobów, limity zapytań, współbieżność zapisu i pełny przepływ klienta. Autoryzacja endpointu jest pierwszą granicą, a nie dowodem poprawności domeny ani izolacji organizacji.

## Weryfikacja wykonana w tej sesji

| Sprawdzenie | Wynik i znaczenie |
|---|---|
| Testy kontraktu autoryzacji (Roslyn/.NET 8) | PASS: 5 sprawdzeń na rzeczywistej klasie AuthenticationSetup |
| Pełny build solucji / docelowego TFM | NIEWYKONANY: MSBuild nie uruchamia się poprawnie w tym środowisku |
| Test API + OIDC + SQL Server | NIEWYKONANY: wymaga skonfigurowanych usług |

### Granice weryfikacji

MSBuild w środowisku roboczym przerwał inicjalizację procesu (odczyt informacji procesu), dlatego nie wykonano pełnego restore/build całej solucji ani testów HTTP z prawdziwym IdP i SQL Server. Kontrakt konfiguracji skompilowano i uruchomiono bezpośrednio na .NET/ASP.NET 8.0.33 z JwtBearer 8; nie jest to kompilacja docelowego net6.0/JwtBearer 6. Projekt aplikacji pozostaje na .NET 6 — przed produkcją wymaga migracji do wspieranej wersji. Klient mobilny i przepływ pobrania tokena nie zostały przebudowane.

## Mapa zmian

- [`TimeTrackingAPI/Security/AuthenticationSetup.cs`](TimeTrackingAPI/Security/AuthenticationSetup.cs)
- [`engineering-tests/Authentication.Tests.csproj`](engineering-tests/Authentication.Tests.csproj)
- [`engineering-tests/Program.cs`](engineering-tests/Program.cs)
- [`TimeTrackingAPI/Controllers/DepartmentController.cs`](TimeTrackingAPI/Controllers/DepartmentController.cs)
- [`TimeTrackingAPI/Controllers/EmployeeController.cs`](TimeTrackingAPI/Controllers/EmployeeController.cs)
- [`TimeTrackingAPI/Controllers/ProjectController.cs`](TimeTrackingAPI/Controllers/ProjectController.cs)
- [`TimeTrackingAPI/Controllers/ProjectTypeController.cs`](TimeTrackingAPI/Controllers/ProjectTypeController.cs)
- [`TimeTrackingAPI/Controllers/TagController.cs`](TimeTrackingAPI/Controllers/TagController.cs)
- [`TimeTrackingAPI/Controllers/TaskController.cs`](TimeTrackingAPI/Controllers/TaskController.cs)
- [`TimeTrackingAPI/Controllers/TaskTagController.cs`](TimeTrackingAPI/Controllers/TaskTagController.cs)
- [`TimeTrackingAPI/Controllers/TimeRecordController.cs`](TimeTrackingAPI/Controllers/TimeRecordController.cs)
- [`TimeTrackingAPI/Program.cs`](TimeTrackingAPI/Program.cs)
- [`TimeTrackingAPI/TimeTrackingAPI.csproj`](TimeTrackingAPI/TimeTrackingAPI.csproj)

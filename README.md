# Project Manager Backend

## Információk a projectről:

A projekt BME VIK AUT témalaboratóriumra készült teendőket kezelő háromrétegű architektúrára épülő alkalmazás backendje. A frontend itt érhető el: https://github.com/Balint-Jeszenszky/project-manager-frontend

### Felhasznált technológiák:
- Asp .net core 3.1
- Entity Framework Core 3.1

### Szükséges szoftverek:
- MSSQL server
- Visual studio 2019

## Beüzemelés:
- Nyissuk meg VS-ban a projektet
- Startup.cs-ben módosítsuk a connection stringet az adatbázisunkhoz
- launchSettings.json fájlban módosíthatjuk az alkalmazés url-jét
- Hozzuk léter az adatbázist a projekt alapján
    - Tools > NuGet Package Manager > Package Manager Console ablakot megnyitva adjuk ki az update-database parancsot, ilyenkor a Migrations mappa alapján létre lesz hozva az adatbázis
    - Vagy ugyan itt add-migration {név} majd update-database parancscsal létrehozhatjuk a migrations mappában léfő fájlok nélkül is
- A buildet állítsuk release-re majd Build > Build Solution opciókkal buildeljük a projektet
- bin\Release\netcoreapp3.1\project-manager-backend programot futtatva kész a backend szerverünk

## Felépítés:

### Data:
Itt található az datbázis ORM leképzés

### Models:
Entitások objektumokként itt találhatóak

### Controllers:
A kontrollerek http://localhost:5000 címen érhetők el alapértelmezetten.
#### UserController:
Felhasználók kezelését teszi lehetővé api végpontokon keresztül.
- GET /api/User/:userID
    - Meghatározott felhasználó adatait kérhetjük le
- POST /api/User/register
    - Új felhasználó regisztrálása
    - JSON objektumként kapja meg a nevet, felhasználónevet, jelszót, emailt
- POST /api/User/login
    - Felhasználónevet illetve jelszót kap JSON-ként, ezt validálja
- PUT /api/User/:userID
    - Felhasználó adatai módosíthatók rajta keresztül
- DELETE /api/User/:userID
    - Felhasználó törlése

#### ProjectController:
Projektek kezelését teszi lehetővé api végpontokon keresztül.
- GET /api/Project/projects/:userID
    - Meghatározott felhasználó projektjei kérhetjük le
- GET /api/Project/project/:projectID
    - Projekt lekérése ID alaján
- POST /api/Project
    - Új projekt létrehozása
- PUT /api/Project/:projectID
    - Projekt adatai módosíthatók rajta keresztül
- DELETE /api/Project/:projectID
    - Projekt törlése

#### TaskGroupController:
Feladatcsoportok kezelését teszi lehetővé api végpontokon keresztül.
- GET /api/TaskGroup/group/:groupID
    - Feladatcsoport lekérése ID alapján
- GET /api/TaskGroup/groups/:projectID
    - Projekthez tartozó feladatcsoportok lekérése
- POST /api/TaskGroup
    - Új feladatcsoport létrehozása
- PUT /api/TaskGroup/:groupID
    - Feladatcsoport adatai módosíthatók rajta keresztül
- DELETE /api/TaskGroup/:groupID
    - Feladatcsoport törlése

#### TaskController:
Feladatok kezelését teszi lehetővé api végpontokon keresztül.
- GET api/Tasks/task/:taskID
    - Feladat lekérése ID alapján
- GET api/Tasks/group/:groupID
    - Egy feladatcsoportba tartozó feladatok lekérése
- POST api/Tasks
    - Új feladat létrehozása
- PUT api/Tasks/:taskId
    - Feladat módosítása
- DELETE api/Tasks/:taskId
    - Feladat törlése
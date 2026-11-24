# ProyectoTecWeb — Circle Rush

## Descripción del proyecto

Circle Rush es un juego web de 30 segundos en el que aparecen círculos en pantalla y el jugador debe hacer clic en la mayor cantidad posible para conseguir puntos.

Los resultados se almacenan en PostgreSQL y pueden consultarse mediante tablas de clasificación por país.

El proyecto está desarrollado con ASP.NET Core .NET 10, Entity Framework Core, PostgreSQL, Docker y autenticación mediante proveedores externos.

Para la Tarea 6, cada integrante implementó cuatro tablas, dos métodos de inicio de sesión y diferentes operaciones protegidas de la API.

**Repositorio principal:** https://github.com/NardoLj/ProyectoWebTec

## 1. Integrantes y distribución de proveedores

| Integrante | País | Login fácil | Login medio | Estado |
|---|---|---|---|---|
| Leonardo Julio (líder) | Bolivia 🇧🇴 | Discord | Google | Integrado |
| Jheferson Velasco | Perú 🇵🇪 | Dropbox | Spotify | Tablas y autenticación integradas |
| Adalid Callizaya | Argentina 🇦🇷 | Twitch | Slack | Completado e integrado |

Cada integrante tiene dos proveedores diferentes y ninguno se repite entre los miembros del equipo.

### Tablas asignadas

**Bolivia — Leonardo Julio**

- `leaderboard_bolivia`
- `jugadores_bolivia`
- `partidas_bolivia`
- `logros_bolivia`

**Perú — Jheferson Velasco**

- `leaderboard_peru`
- `jugadores_peru`
- `partidas_peru`
- `logros_peru`

**Argentina — Adalid Calizaya**

- `leaderboard_argentina`
- `jugadores_argentina`
- `partidas_argentina`
- `logros_argentina`

Las cuatro tablas de Argentina están integradas, junto con sus controladores y operaciones CRUD.

Las tablas, controladores y proveedores de Perú están plenamente integrados y funcionales.

Todas las tablas de Bolivia están integradas con controladores CRUD, soft delete y reporte.

## 2. Tecnologías utilizadas

| Tecnología | Función |
|---|---|
| C# | Programación del backend |
| ASP.NET Core .NET 10 | Desarrollo de la Web API |
| Entity Framework Core | Acceso y operaciones sobre la base de datos |
| PostgreSQL 16 | Almacenamiento de información |
| Docker | Ejecución mediante contenedores |
| Docker Compose | Configuración de servicios |
| Swagger | Pruebas de los endpoints |
| Git y GitHub | Control de versiones y Pull Requests |
| HTML, CSS y JavaScript | Interfaz del juego Circle Rush |
| OAuth / OpenID Connect | Autenticación con proveedores externos |

## 3. Métodos de inicio de sesión

Se implementaron seis proveedores de autenticación, distribuidos entre los tres integrantes.

| País | Proveedor | Categoría | Ruta de inicio |
|---|---|---|---|
| Bolivia | Discord | Fácil | `/login/discord` |
| Bolivia | Google | Medio | `/login/google` |
| Perú | Dropbox | Fácil | `/login/dropbox` |
| Perú | Spotify | Medio | `/login/spotify` |
| Argentina | Twitch | Fácil | `/login/twitch` |
| Argentina | Slack | Medio | `/login/slack` |

### Funcionamiento de la autenticación

El usuario selecciona un botón de inicio de sesión y la aplicación lo redirige al proveedor correspondiente.

Después de iniciar sesión y autorizar el acceso, el proveedor devuelve al usuario a la aplicación.

La API valida la autenticación y crea una cookie de sesión.

Finalmente, la aplicación muestra los datos de identidad del usuario autenticado.

También permite cerrar la sesión.

### Endpoints de autenticación

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/auth/providers` | Muestra los proveedores configurados |
| GET | `/api/auth/me` | Consulta la identidad del usuario autenticado |
| POST | `/logout` | Cierra la sesión |

Sin una sesión válida, los endpoints protegidos devuelven HTTP 401.

### URLs de retorno

| Proveedor | Callback |
|---|---|
| Discord | `/signin-discord` |
| Google | `/signin-google` |
| Dropbox | `/signin-dropbox` |
| Spotify | `/signin-spotify` |
| Twitch | `/signin-twitch` |
| Slack | `/signin-slack` |

Las URLs deben registrarse correctamente en los portales de cada proveedor.

Para las pruebas locales de Argentina se utilizaron:

```text
https://localhost:8443/signin-twitch
https://localhost:8443/signin-slack
```

## 4. Implementación de Argentina

**Responsable:** Adalid Calizaya.

**País asignado:** Argentina.

**Proveedores implementados:** Twitch y Slack.

**Repositorio personal:** https://github.com/Adalid-web/ProyectoWebTec

### 4.1. Cuatro tablas de Argentina

Se implementaron cuatro tablas relacionadas con jugadores, partidas, logros y clasificación.

Las tablas se administran mediante Entity Framework Core y PostgreSQL.

### 4.2. Autenticación Twitch y Slack

Se implementaron dos botones funcionales de inicio de sesión:

- Continuar con Twitch.
- Continuar con Slack.

Los dos proveedores permiten iniciar sesión, regresar a Circle Rush, mostrar la identidad del usuario y cerrar sesión.

Las credenciales se configuran mediante variables de entorno y no se almacenan directamente en el código.

### 4.3. Controladores protegidos

Se implementaron cuatro controladores:

- `JugadoresArgentinaController`
- `PartidasArgentinaController`
- `LogrosArgentinaController`
- `LeaderboardArgentinaController`

Todos utilizan el atributo `[Authorize]` para exigir una sesión autenticada.

Cada controlador permite:

| Método | Función |
|---|---|
| GET | Consultar una lista |
| GET por ID | Consultar un registro |
| POST | Crear un registro |
| PUT | Actualizar un registro |
| DELETE | Realizar un borrado lógico |

### 4.4. Endpoints de Argentina

**Jugadores**

```text
GET     /api/argentina/jugadores
GET     /api/argentina/jugadores/{id}
POST    /api/argentina/jugadores
PUT     /api/argentina/jugadores/{id}
DELETE  /api/argentina/jugadores/{id}
```

**Partidas**

```text
GET     /api/argentina/partidas
GET     /api/argentina/partidas/{id}
POST    /api/argentina/partidas
PUT     /api/argentina/partidas/{id}
DELETE  /api/argentina/partidas/{id}
```

**Logros**

```text
GET     /api/argentina/logros
GET     /api/argentina/logros/{id}
POST    /api/argentina/logros
PUT     /api/argentina/logros/{id}
DELETE  /api/argentina/logros/{id}
```

**Leaderboard**

```text
GET     /api/leaderboard/argentina
GET     /api/leaderboard/argentina/{id}
POST    /api/leaderboard/argentina
PUT     /api/leaderboard/argentina/{id}
DELETE  /api/leaderboard/argentina/{id}
```

## 5. Soft delete

El sistema utiliza borrado lógico para evitar la eliminación física de registros.

En las cuatro tablas de Argentina se agregaron las columnas:

```text
is_deleted
deleted_at
```

**`is_deleted`:** indica si el registro fue eliminado.

**`deleted_at`:** almacena la fecha y hora del borrado lógico.

### Funcionamiento

Cuando se ejecuta DELETE:

1. La API busca el registro.
2. Comprueba que todavía esté activo.
3. Cambia `is_deleted` a `true`.
4. Registra la fecha en `deleted_at`.
5. Guarda los cambios.
6. Devuelve HTTP 204.

El registro permanece físicamente en PostgreSQL.

Los métodos GET excluyen los registros marcados como eliminados.

Si se intenta consultar, actualizar o volver a eliminar un registro inactivo, la API devuelve HTTP 404.

### Resultados de las pruebas

| Operación | Resultado |
|---|---|
| POST de un registro válido | HTTP 201 |
| PUT de un registro activo | HTTP 204 |
| DELETE de un registro activo | HTTP 204 |
| GET de un registro eliminado | HTTP 404 |
| PUT de un registro eliminado | HTTP 404 |
| DELETE repetido | HTTP 404 |
| Consulta SQL del registro eliminado | El registro sigue existiendo |

Las pruebas se realizaron utilizando Swagger y PostgreSQL.

### Verificación en PostgreSQL

```sql
SELECT id, nombre, is_deleted, deleted_at
FROM jugadores_argentina
ORDER BY id DESC;
```

En las pruebas se comprobó que las cuatro tablas conservaban sus registros eliminados con `is_deleted = true`.

### Migración de Argentina

Se generó y aplicó la siguiente migración:

```text
20261123060805_AddArgentinaSoftDelete
```

Esta migración agregó las columnas de borrado lógico a las cuatro tablas de Argentina.

## 6. Pull Requests y trabajo colaborativo

El desarrollo se realizó mediante forks, ramas individuales y Pull Requests.

Los integrantes no necesitan convertirse en colaboradores directos del repositorio principal para enviar sus aportes.

### Pull Requests de Argentina

| PR | Implementación | Resultado |
|---|---|---|
| [#1](https://github.com/NardoLj/ProyectoWebTec/pull/1) | Cuatro tablas de Argentina | Integrada |
| [#12](https://github.com/NardoLj/ProyectoWebTec/pull/12) | Autenticación Twitch y Slack | Aprobada e integrada |
| [#13](https://github.com/NardoLj/ProyectoWebTec/pull/13) | Cuatro controladores protegidos | Aprobada e integrada |
| [#14](https://github.com/NardoLj/ProyectoWebTec/pull/14) | POST duplicado intencional | Rechazada y cerrada sin merge |
| [#16](https://github.com/NardoLj/ProyectoWebTec/pull/16) | PUT y soft delete | Aprobada e integrada con Squash and merge |

### Pull Requests de Perú

- [PR #10 — Tablas de Perú](https://github.com/NardoLj/ProyectoWebTec/pull/10).
- [PR #11 — Autenticación Dropbox y Spotify](https://github.com/NardoLj/ProyectoWebTec/pull/11).

### PR rechazada — Argentina

Para practicar la revisión de aportes incorrectos, se creó una rama con un método POST duplicado.

La duplicación provocaba un conflicto de rutas porque dos métodos utilizaban la misma ruta HTTP.

El líder revisó el código, explicó el problema y solicitó cambios.

La Pull Request #14 fue cerrada sin integrarse a `main`.

### PR con Squash and merge — Argentina

La implementación de PUT y soft delete se desarrolló mediante dos commits separados.

**Primer commit:**

```text
4c3b069 feat(argentina): agregar PUT a cuatro controladores
```

**Segundo commit:**

```text
5dd0e6e feat(argentina): implementar soft delete con migracion y filtros
```

La PR #16 recibió la aprobación del líder y se integró mediante **Squash and merge**.

**Commit final integrado en `main`:**

```text
54472b3 Feat/argentina put softdelete (#16)
```

De esta manera, los dos commits originales quedaron registrados en la PR y sus cambios se incorporaron a `main` mediante un único commit.

## 7. Configuración del proyecto

### Requisitos

Para ejecutar el proyecto se necesita:

- Docker y Docker Compose.
- Credenciales de los proveedores OAuth que se utilizarán.
- Un archivo `.env` configurado.
- Para HTTPS local, un certificado válido para `localhost`.

Si se ejecuta sin Docker, también se necesita el SDK de .NET 10 y PostgreSQL.

### Preparar variables de entorno

Desde la carpeta principal del proyecto:

```bash
cp .env.example .env
```

En Windows PowerShell:

```powershell
Copy-Item .env.example .env
```

Después se deben configurar las variables necesarias.

### Variables para los proveedores

**Bolivia**

```text
DISCORD_CLIENT_ID
DISCORD_CLIENT_SECRET

GOOGLE_CLIENT_ID
GOOGLE_CLIENT_SECRET
```

**Perú**

```text
DROPBOX_CLIENT_ID
DROPBOX_CLIENT_SECRET

SPOTIFY_CLIENT_ID
SPOTIFY_CLIENT_SECRET
```

**Argentina**

```text
TWITCH_CLIENT_ID
TWITCH_CLIENT_SECRET

SLACK_CLIENT_ID
SLACK_CLIENT_SECRET
```

También se deben configurar las variables de PostgreSQL:

```text
POSTGRES_DB
POSTGRES_USER
POSTGRES_PASSWORD
POSTGRES_HOST
```

**Importante:** nunca subir el archivo `.env` con contraseñas reales a GitHub.

### Consideración para Perú

Los proveedores Dropbox y Spotify están implementados en el backend, pero el archivo `contructor.yml` de la versión revisada no inyecta automáticamente sus cuatro variables OAuth.

Para utilizarlos mediante Docker Compose es necesario agregar esas variables al servicio API o configurar un archivo Compose adicional.

## 8. Ejecutar mediante Docker

### Construcción del proyecto

Desde la raíz del repositorio:

```bash
docker compose --env-file .env \
  -f contructor.yml \
  up -d --build
```

Comprobar los servicios:

```bash
docker compose --env-file .env \
  -f contructor.yml \
  ps
```

### Ejecutar con HTTPS local

Para utilizar Twitch y Slack con el entorno HTTPS configurado:

```bash
docker compose --env-file .env \
  -f contructor.yml \
  -f oauth.local.yml \
  up -d --build
```

Verificar los contenedores:

```bash
docker compose --env-file .env \
  -f contructor.yml \
  -f oauth.local.yml \
  ps
```

En el entorno utilizado para las pruebas de Argentina se configuraron estas direcciones:

**Aplicación:**

https://localhost:8443/

**Swagger:**

https://localhost:8443/swagger

La ejecución HTTPS requiere configurar el certificado local y sus variables correspondientes.

El certificado privado no se incluye en el repositorio.

### Detener los contenedores

```bash
docker compose --env-file .env \
  -f contructor.yml \
  -f oauth.local.yml \
  down
```

No utilizar `down -v` si se desea conservar el volumen de PostgreSQL.

## 9. Ejecutar sin Docker

También es posible ejecutar la API con .NET 10, siempre que PostgreSQL y las variables de entorno estén configurados.

```bash
dotnet run --project ProyectoTecWeb/ProyectoTecWeb.csproj
```

Para generar una migración:

```bash
dotnet ef migrations add NombreDeLaMigracion \
  --project ProyectoTecWeb/ProyectoTecWeb.csproj \
  --startup-project ProyectoTecWeb/ProyectoTecWeb.csproj
```

La aplicación utiliza Entity Framework Core para administrar las migraciones de la base de datos.

Para los proveedores que requieren HTTPS, es necesario configurar el servidor con una URL segura y registrar correctamente los callbacks.

## 10. Pruebas de autenticación y API

Las pruebas se realizaron mediante Swagger y sesiones reales.

### Sin autenticación

Se verificó que los controladores protegidos rechazaran solicitudes sin una sesión válida.

**Resultado esperado:**

```text
HTTP 401 Unauthorized
```

### Con autenticación

Después de iniciar sesión con Twitch o Slack, los endpoints protegidos permitieron consultar y crear registros.

**Resultados comprobados:**

```text
GET    → HTTP 200
POST   → HTTP 201
PUT    → HTTP 204
DELETE → HTTP 204
```

### Registros eliminados

Después del borrado lógico:

```text
GET    → HTTP 404
PUT    → HTTP 404
DELETE → HTTP 404
```

En PostgreSQL se comprobó que los registros permanecían almacenados con `is_deleted = true`.

## 11. Docker Hub y Compose de Release (Puntos 6 y 7)

### Repositorio Docker Hub
- **Usuario:** `nardolj`
- **Imagen:** `proyectowebtec`
- **Versión de entrega:** `2.0.0`
- **Referencia completa:** `nardolj/proyectowebtec:2.0.0`
- **URL pública:** https://hub.docker.com/r/nardolj/proyectowebtec

### Construcción y publicación
```bash
docker build -f ProyectoTecWeb/Dockerfile -t nardolj/proyectowebtec:2.0.0 ProyectoTecWeb
docker push nardolj/proyectowebtec:2.0.0
```

### Compose de Release con Cuatro Instancias (`compose.release.yml`)
El archivo `compose.release.yml` descarga la imagen `nardolj/proyectowebtec:2.0.0` desde Docker Hub y levanta **cuatro instancias del backend** junto con PostgreSQL 16 sin compilar código fuente:

| Instancia | Puerto Externo | Puerto Interno | Función |
|---|---|---|---|
| `api1` | `5001` | `8080` | Instancia 1 del backend |
| `api2` | `5002` | `8080` | Instancia 2 del backend |
| `api3` | `5003` | `8080` | Instancia 3 del backend |
| `api4` | `5004` | `8080` | Instancia 4 del backend |
| `postgres` | `5432` (red interna) | `5432` | Base de datos compartida |

**Compatibilidad de sesión entre instancias:** Las cuatro instancias comparten el volumen `authkeys` montado en `/auth-keys`, permitiendo que ASP.NET Core DataProtection reconozca la misma cookie de sesión en cualquiera de los puertos.

**Ejecución del Release:**
```bash
docker compose -f compose.release.yml up -d
docker compose -f compose.release.yml ps
```

**Verificación en los cuatro puertos:**
```powershell
foreach ($p in 5001..5004) {
    Write-Host "Puerto $p health:" (Invoke-RestMethod -Uri "http://localhost:$p/health")
    Write-Host "Puerto $p DB:" (Invoke-RestMethod -Uri "http://localhost:$p/health/db").database
}
```

---

## 12. Relación de Puntos de la Consigna y Pull Requests (Punto 8)

Cada punto del Proyecto Final se desarrolló mediante ramas y Pull Requests respetando la revisión del equipo:

| Punto | Descripción | Responsable | Rama / PR | Estado |
|---|---|---|---|---|
| **1** | Frontend con Login (6 botones), Identidad y Logout verificable (HTTP 401 tras salir) | Adalid Callizaya | PR #19 (`feat-final-frontend-login-logout`) | Integrada |
| **2** | Reporte autorizado de Argentina (`GET /api/reportes/argentina/resumen`, Twitch/Slack) | Adalid Callizaya | PR #18 (`feat-final-reporte-argentina`) | Integrada |
| **2** | Reportes autorizados de Bolivia (Google/Discord) y Perú (Dropbox/Spotify) | Leonardo Julio y Jheferson Velasco | PR #24 (`feat/reportes-bolivia-peru`) | Integrada |
| **3** | Dockerfiles por integrante (`Dockerfile.AdalidCallizaya`, `Dockerfile.LeonardoJulio`, `Dockerfile.JhefersonVelasco`) | Adalid Callizaya / Leonardo Julio | PR #20 y PR #25 (`feat/dockerfiles-integrantes`) | Integrada / Lista |
| **4** | Docker Compose de desarrollo del backend con PostgreSQL y `.env` | Adalid Callizaya / Leonardo Julio | PR #21 (`feat-final-compose-dev`) | Integrada |
| **5** | Incorporación del Frontend Nginx al mismo Compose de desarrollo (`compose.dev.yml`) | Adalid Callizaya | PR #22 (`feat-final-frontend-compose`) | Integrada |
| **6** | Publicación de la imagen final `nardolj/proyectowebtec:2.0.0` en Docker Hub | Leonardo Julio | Rama `release/dockerhub-2.0.0` | Publicada |
| **7** | Docker Compose de release con cuatro instancias (`compose.release.yml`, puertos 5001-5004, keys compartidas) | Leonardo Julio | Rama `feat/compose-release-4instancias` | Listo |
| **8** | Flujo de revisión cruzada por Pull Requests en GitHub y evidencias | Equipo completo | Flujo de forks y PRs revisadas | Cumplido |

### Detalle de los reportes por integrante (Punto 2)

| Integrante | País | Endpoint | Tablas utilizadas | Política de autorización | Proveedores permitidos |
|---|---|---|---|---|---|
| **Leonardo Julio** | Bolivia | `GET /api/reportes/bolivia/resumen` | `jugadores_bolivia`, `partidas_bolivia`, `logros_bolivia`, `leaderboard_bolivia` | `ReporteBolivia` | Google, Discord |
| **Jheferson Velasco** | Perú | `GET /api/reportes/peru/resumen` | `jugadores_peru`, `partidas_peru`, `logros_peru`, `leaderboard_peru` | `ReportePeru` | Dropbox, Spotify |
| **Adalid Callizaya** | Argentina | `GET /api/reportes/argentina/resumen` | `jugadores_argentina`, `partidas_argentina`, `logros_argentina`, `leaderboard_argentina` | `ReporteArgentina` | Twitch, Slack |

*Sin sesión: HTTP 401. Con sesión de proveedor no autorizado: HTTP 403. Con sesión autorizada: HTTP 200 con resumen estadístico.*

---

## 13. Evidencias del Proyecto Final

Para el documento PDF / Word de entrega se requieren evidencias de:
1. **Frontend:** Seis botones OAuth, estado cargado/deshabilitado, identidad del usuario tras login y botón «Salir».
2. **Logout:** Ejecución de `POST /logout` y confirmación de `HTTP 401 Unauthorized` al intentar acceder a `/api/auth/me` y a endpoints protegidos.
3. **Reportes autorizados:** Respuestas 200 (autorizado), 401 (sin sesión) y 403 (proveedor distinto) para Bolivia, Perú y Argentina.
4. **Dockerfiles:** Compilación exitosa de cada uno de los 3 Dockerfiles de los integrantes y del Dockerfile principal.
5. **Compose dev:** Ejecución de `docker compose -f compose.dev.yml up -d` y `ps` mostrando `frontend`, `api` y `postgres` saludables.
6. **Docker Hub:** Imagen `nardolj/proyectowebtec:2.0.0` visible en Docker Hub y comprobación de `docker pull`.
7. **Compose release (4 instancias):** `docker compose -f compose.release.yml ps` con 4 APIs (5001-5004) y respuestas 200 en `/health/db` en los cuatro puertos.
8. **GitHub PRs:** Capturas del listado de Pull Requests cerradas/integradas con sus revisiones cruzadas.

## 14. Entrega final

La entrega del proyecto debe incluir:

- Código fuente integrado.
- Archivos Docker y Docker Compose.
- Migraciones de Entity Framework Core.
- Plantilla `.env.example`.
- README actualizado.
- Informe individual de cada integrante.
- Capturas de funcionamiento.
- Enlaces a las Pull Requests.

El ZIP de entrega no debe incluir contraseñas, tokens, cookies de sesión ni certificados privados.

Tampoco es necesario incluir las carpetas `bin`, `obj` o `.git`.

Cada persona que ejecute el proyecto en otro equipo debe configurar sus propias credenciales y variables de entorno.

## 14. Seguridad

- Las contraseñas no deben almacenarse directamente en el código.
- Las credenciales OAuth deben configurarse mediante variables de entorno.
- El archivo `.env` debe mantenerse privado.
- Los controladores protegidos requieren autenticación.
- Las sesiones pueden cerrarse desde la aplicación.
- Los secretos y tokens deben ocultarse de las capturas.
- Los integrantes trabajan mediante forks y Pull Requests.
- El borrado lógico permite conservar el historial de registros.

## 15. Uso de inteligencia artificial

Se utilizó inteligencia artificial como apoyo para la organización del proyecto, la documentación, la revisión del código y la solución de problemas técnicos.

Los cambios se compilaron y verificaron mediante pruebas de funcionamiento antes de integrarse.

---

**ProyectoTecWeb — Circle Rush**

**Tarea 6: Autenticación, controladores protegidos, Pull Requests y soft delete.**

**Equipo:** Bolivia, Perú y Argentina.

**Tecnologías:** .NET 10, PostgreSQL, Docker, OAuth y GitHub.

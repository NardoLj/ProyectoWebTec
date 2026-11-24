# Proyecto Final · Puntos 6 y 7 — Publicación en Docker Hub y Compose de Release con 4 Instancias

**Equipo:** Leonardo Julio (Bolivia), Jheferson Velasco (Perú), Adalid Callizaya (Argentina).

---

## Punto 6: Publicar la imagen en Docker Hub (5 pts)

### 1. Referencia de la imagen final
- **Usuario Docker Hub:** `nardolj`
- **Nombre de la imagen:** `proyectowebtec`
- **Versión/Etiqueta:** `2.0.0`
- **Referencia completa:** `nardolj/proyectowebtec:2.0.0`
- **Enlace al repositorio:** [https://hub.docker.com/r/nardolj/proyectowebtec](https://hub.docker.com/r/nardolj/proyectowebtec)

### 2. Dockerfile utilizado
El equipo definió utilizar el Dockerfile unificado:
```text
ProyectoTecWeb/Dockerfile
```
Este Dockerfile compila el proyecto .NET 10 en modo Release (`dotnet publish`), utiliza la imagen base `mcr.microsoft.com/dotnet/aspnet:10.0`, crea el directorio `/auth-keys` con permisos para `$APP_UID` y expone el puerto interno 8080.

### 3. Comandos para construir y publicar

```bash
# 1. Construir la imagen con la etiqueta 2.0.0
docker build -f ProyectoTecWeb/Dockerfile -t nardolj/proyectowebtec:2.0.0 ProyectoTecWeb

# 2. Iniciar sesión en Docker Hub
docker login -u nardolj

# 3. Publicar la imagen
docker push nardolj/proyectowebtec:2.0.0

# 4. Comprobar que se puede descargar públicamente
docker pull nardolj/proyectowebtec:2.0.0
```

---

## Punto 7: Compose de release con cuatro instancias (20 pts)

### 1. Archivo Compose
- **Archivo:** `compose.release.yml`
- **Levanta:** PostgreSQL 16 y 4 instancias del backend usando exclusivamente la imagen publicada `nardolj/proyectowebtec:2.0.0` (sin compilar código fuente).

### 2. Puertos asignados a las cuatro instancias

| Servicio | Puerto Externo (Host) | Puerto Interno (Contenedor) | Propósito |
|---|---|---|---|
| `api1` | `5001` | `8080` | Instancia 1 de la API |
| `api2` | `5002` | `8080` | Instancia 2 de la API |
| `api3` | `5003` | `8080` | Instancia 3 de la API |
| `api4` | `5004` | `8080` | Instancia 4 de la API |
| `postgres` | `5432` (interno) | `5432` | Base de datos compartida |

### 3. Compatibilidad de sesiones (DataProtection compartido)
Para que las cookies de sesión generadas en cualquiera de las cuatro instancias sean reconocidas por las demás:
- Todas las instancias montan el volumen compartido `authkeys` en `/auth-keys`.
- La aplicación configura `AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(keyDirectory))` cuando `AUTH_KEYS_PATH` está definida.
- Las cuatro instancias comparten el mismo keyring XML en `/auth-keys`.

### 4. Conexión a la base de datos y persistencia
- Todas las instancias se conectan a `postgres:5432` usando la misma red interna `appnet`.
- PostgreSQL almacena los datos en el volumen persistente `pgdata`.
- Cada instancia espera que PostgreSQL esté en estado saludable (`condition: service_healthy`) antes de iniciar.
- Las migraciones de EF Core se ejecutan de manera automática y segura al arrancar.

### 5. Comandos de ejecución

```bash
# Iniciar las 4 instancias y la base de datos
docker compose -f compose.release.yml up -d

# Verificar estado de los 5 contenedores
docker compose -f compose.release.yml ps

# Detener los contenedores conservando los datos
docker compose -f compose.release.yml down
```

### 6. Pruebas de verificación en cada puerto

```powershell
# En PowerShell:
foreach ($port in 5001..5004) {
    $h = Invoke-RestMethod -Uri "http://localhost:$port/health"
    $db = (Invoke-RestMethod -Uri "http://localhost:$port/health/db").database
    $prov = (Invoke-RestMethod -Uri "http://localhost:$port/api/auth/providers").Count
    $repBolivia = (try { Invoke-RestMethod -Uri "http://localhost:$port/api/reportes/bolivia/resumen" } catch { $_.Exception.Response.StatusCode.value__ })
    $repPeru = (try { Invoke-RestMethod -Uri "http://localhost:$port/api/reportes/peru/resumen" } catch { $_.Exception.Response.StatusCode.value__ })
    $repArg = (try { Invoke-RestMethod -Uri "http://localhost:$port/api/reportes/argentina/resumen" } catch { $_.Exception.Response.StatusCode.value__ })
    Write-Output "Puerto $port -> Health: $h | DB: $db | Proveedores: $prov | Reportes (401 sin auth): Bol=$repBolivia, Per=$repPeru, Arg=$repArg"
}
```

**Resultado esperado:**
- Todos responden `Health: Healthy`.
- Todos responden `DB: connected`.
- Todos retornan `HTTP 401 Unauthorized` al intentar consultar los reportes protegidos sin sesión.
- Una cookie de sesión iniciada en la instancia 5001 es válida y aceptada en 5002, 5003 y 5004.

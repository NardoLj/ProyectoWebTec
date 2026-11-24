# Proyecto Final - Punto 3: Dockerfile por integrante

Cada integrante mantiene un Dockerfile propio y el equipo identifica el Dockerfile que construye la imagen publicada.

| Integrante | País | Dockerfile | Estado |
|---|---|---|---|
| Leonardo Julio | Bolivia | `ProyectoTecWeb/Dockerfile.LeonardoJulio` | Probado localmente |
| Jheferson Velasco | Perú | `ProyectoTecWeb/Dockerfile.JhefersonVelasco` | Probado localmente |
| Adalid Callizaya | Argentina | `ProyectoTecWeb/Dockerfile.AdalidCallizaya` | Probado localmente |

## Imagen final

La imagen que se publica en Docker Hub se construye con:

```text
ProyectoTecWeb/Dockerfile
```

Ese Dockerfile usa `dotnet publish`, ejecuta la aplicación con el usuario `APP_UID` y crea `/auth-keys` con permisos de escritura para que las cuatro instancias del servidor compartan las claves de sesión.

## Construcción y prueba de cada Dockerfile

Desde la carpeta `ProyectoTecWeb`:

```powershell
docker build -f Dockerfile.LeonardoJulio -t proyectowebtec:leonardo .
docker build -f Dockerfile.JhefersonVelasco -t proyectowebtec:jhefer .
docker build -f Dockerfile.AdalidCallizaya -t proyectowebtec:adalid .

docker run --rm -p 5101:8080 -e PORT=8080 proyectowebtec:leonardo
docker run --rm -p 5102:8080 -e PORT=8080 proyectowebtec:jhefer
docker run --rm -p 5103:8080 -e PORT=8080 proyectowebtec:adalid
```

Cada contenedor debe responder `GET /health` con HTTP 200. Para una prueba completa se le pasa la cadena de conexión de PostgreSQL y las credenciales OAuth mediante variables de entorno; el archivo `.env` no se copia dentro de la imagen.

## Imagen publicada

```text
nardolj/proyectowebtec:2.0.0
```

La verificación de `docker pull` y la ejecución de las cuatro instancias se documentan en `PROYECTO_FINAL_PUNTO6_7_RELEASE.md`.

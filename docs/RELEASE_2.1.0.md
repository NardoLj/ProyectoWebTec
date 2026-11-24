# Proyecto Final - Circle Rush 2.1.0

## Autor del aporte

Adalid Aebi Callizaya Huarahuara - Argentina.

## Imagen publicada

- Imagen: adalidweb/circle-rush-api:2.1.0
- Commit de construcción: ed5a194
- Docker Hub: https://hub.docker.com/r/adalidweb/circle-rush-api/tags
- Pull Request de autenticación: https://github.com/NardoLj/ProyectoWebTec/pull/31
- SHA256: 6782badc2f0d9fd8b6c4fd11c2aa05114ab6282af3dee9cc8db46b2ca3789543

## Mejoras incorporadas

- Inicio de sesión mediante TikTok OAuth 2.0.
- Single Sign-Out con PostgreSQL.
- Registro y revocación de sesiones.
- Cuatro instancias con claves de autenticación compartidas.
- Conservación de las políticas de autorización de los reportes.

## Ejecución del release

Configurar .env a partir de .env.example.
Completar las credenciales de PostgreSQL y de los proveedores OAuth.
No publicar credenciales reales.

    docker compose --env-file .env -f compose.release.yml pull
    docker compose --env-file .env -f compose.release.yml up -d --no-build
    docker compose --env-file .env -f compose.release.yml ps

## Verificación automática

    bash scripts/verificar-release-4instancias.sh

## Resultados obtenidos

| Puerto | Health | PostgreSQL | Sin sesión | Reporte con Twitch | Después del logout |
|--------|--------|------------|------------|--------------------|-------------------|
| 5001   | 200    | 200        | 401        | 200                | 401               |
| 5002   | 200    | 200        | 401        | 200                | 401               |
| 5003   | 200    | 200        | 401        | 200                | 401               |
| 5004   | 200    | 200        | 401        | 200                | 401               |

## Autenticación compartida

Las cuatro instancias utilizan la misma imagen Docker,
la misma base de datos PostgreSQL y el volumen de claves
proyectotecweb-release_authkeys.

Se comprobó el acceso autorizado al reporte de Argentina
desde las cuatro instancias utilizando una sesión de Twitch
y un proxy Nginx temporal.

El cierre global desde API 2 respondió HTTP 200.
PostgreSQL registró dos sesiones de Twitch revocadas.
En la prueba posterior al cierre, las cuatro instancias
rechazaron las solicitudes de identidad y reporte con HTTP 401.

TikTok fue probado mediante OAuth real en desarrollo.
En el release se verificó su configuración en las cuatro instancias.

El cierre global afecta a las sesiones de Circle Rush,
no a las sesiones externas de las redes sociales.

## Seguridad

No se incluyen archivos .env reales, tokens, certificados
privados ni claves de autenticación.

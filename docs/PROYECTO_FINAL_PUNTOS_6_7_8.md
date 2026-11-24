# Proyecto Final - Tecnologia Web I (Puntos 6, 7 y 8)

## Punto 6: Trabajo colaborativo y PRs
- Rama: feat/dockerfile-jheferson-velasco (commit d8a3a65)
- Integrado en upstream/main: PR #29 (afda031)

## Punto 7: 4 instancias (compose.release.yml)
- Servicios: postgres (healthy), api1..api4 en puertos 5001..5004
- Imagen: nardolj/proyectowebtec:1.0.0
- Red: proyectotecweb-release_appnet
- Volumenes: proyectotecweb-release_pgdata y proyectotecweb-release_authkeys
- Verificacion: GET / -> 200 en 5001-5004; /api/reportes/peru/resumen -> 404 en tag 1.0.0

## Punto 8: Documentacion y evidencias
- docs/outputs/14-release-4-instancias-http.txt
- docs/outputs/15-release-4-instancias-ps.txt
- Comandos: docker compose -f compose.release.yml up -d | ps | down

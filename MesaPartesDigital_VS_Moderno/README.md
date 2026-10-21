# MesaPartesDigital

Prototipo de mesa de partes digital para el curso Fundamentos de Programación (CIIN1205P).

Permite registrar, validar, buscar, ordenar, cambiar estado, **derivar entre áreas** y guardar expedientes en archivos de texto.

## Cómo ejecutar

1. Abrir `MesaPartesDigital.slnx` en Visual Studio 2022 17.10+ (o usar `MesaPartesDigital.sln` si tu VS es más antiguo).
2. Presionar F5 o Ctrl+F5 para compilar y ejecutar.
3. Los archivos `expedientes.txt` y `derivaciones.txt` se copian automáticamente a `bin/Debug/` al compilar, y ahí mismo se actualizan al guardar.

## Estructura

- `Program.cs` - Menú y ciclo principal
- `Expedientes.cs` - Arreglos paralelos, registro, búsqueda, ordenamiento, cambio de estado y derivación
- `Validaciones.cs` - Validaciones de los datos
- `Archivos.cs` - Guardado, carga, derivaciones e historial

## Opciones del menú

1. Registrar expediente
2. Buscar por código
3. Buscar por DNI o RUC
4. Ordenar y listar (por código, fecha, solicitante, documento, tipo, estado o área)
5. Guardar datos en archivo
6. Cambiar estado de un expediente
7. Derivar expediente a otra área
8. Ver historial de derivaciones
9. Salir

## Estados del expediente

- INGRESADO (por defecto al registrar)
- EN TRAMITE
- ATENDIDO
- OBSERVADO
- ARCHIVADO

## Áreas por las que puede pasar un expediente (I.E. pública)

1. SECRETARIA (donde se recepciona, asignada por defecto)
2. DIRECCION
3. SUBDIRECCION ACADEMICA
4. SUBDIRECCION ADMINISTRATIVA
5. TUTORIA
6. OFICINA DE MATRICULA
7. ARCHIVO
8. UGEL/EXTERNO

## Tipos de trámite predefinidos

1. SOLICITUD DE VACANTE
2. RATIFICACION DE MATRICULA
3. TRASLADO DE MATRICULA
4. CERTIFICADO DE ESTUDIOS
5. CONSTANCIA DE NOTAS
6. OFICIO EXTERNO
7. OTRO

## Formato del archivo expedientes.txt

Cada línea tiene los 10 campos separados por `|`:

```
EXP-2026-002|20/08/2026|70123456|QUISPE ROJAS, ANA|SOLICITUD DE VACANTE|1RO-2026|Archivador 1º 2026|987654321|ATENDIDO|DIRECCION
```

## Formato del archivo derivaciones.txt

Cada línea registra una derivación (ruta del expediente entre áreas):

```
EXP-2026-002|20/08/2026|SECRETARIA|DIRECCION
EXP-2026-002|21/08/2026|DIRECCION|OFICINA DE MATRICULA
```

## Integrantes

Grupo 8 - Fundamentos de Programación

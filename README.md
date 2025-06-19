# Proyecto SYSACAD - Importador de Alumnos (CSV)

## Descripción

Este proyecto en C# forma parte de un Trabajo Práctico cuyo objetivo es continuar con el desarrollo del sistema **SYSACAD**. Este tiene la capacidad de trabajar con grandes volúmenes de datos, por lo tanto es necesario importar datos provenientes de un sistema heredado, almacenados en archivos CSV, y persistir dichos datos en la base de datos **DEV_SYSACAD**.

## Funcionalidades

- Importación de datos desde archivos CSV.
- Procesamiento y validación de registros únicos por identificador.
- Persistencia de cada registro en su tabla correspondiente dentro de la base de datos.

## Archivo CSV a importar

La aplicación procesa el siguiente archivo: `alumnos.csv`


## Requisitos

### Para ejecutar la aplicación:
- Sistema operativo compatible (Windows o Linux)
- Permisos de ejecución para el archivo (`AlumnosCSVImporter` o `AlumnosCSVImporter.exe`)
- Acceso a una instancia de **PostgreSQL** con la base de datos `DEV_SYSACAD`


### Para compilar o desarrollar:
- [.NET SDK](https://dotnet.microsoft.com/download)
- Visual Studio o cualquier editor compatible con C# (por ejemplo, Visual Studio Code con la extensión de C#)

## Ejecución

#### Windows 
Ejecutar el archivo `AlumnosCSVImporter.exe`

#### Linux
1. Asegúrate de que el archivo tenga permisos de ejecución. Si no los tiene, asígnalos con:

   ```bash
   chmod +x AlumnosCSVImporter
    ```
2. Ejecuta el archivo directamente desde la terminal:
   ```bash
   ./AlumnosCSVImporter
    ```
## Desarrollador

Esta aplicación fue desarrollada por:

- Ignacio Bianchi – Desarrollo completo (backend, lógica de negocio, base de datos, documentación)
string? path;
bool existe = false;

do
{
    Console.Write("Ingrese el path a localizar: ");
    path = Console.ReadLine();

    if (Directory.Exists(path))
    {
        existe = true;
    } else
    {
        Console.WriteLine("El path ingresado es incorrecto o no se encuentra. Ingrese nuevamente.");
    }

} while (!existe);

Console.WriteLine("\nLocalizacion exitosa!");
Console.WriteLine("\n--- CONTENIDO ---");

// --- ENLISTO CARPETAS ---
string[] carpetas = Directory.GetDirectories(path);

// caso arreglo vacio
if (carpetas.Length == 0)
{
    Console.WriteLine("No hay subcarpetas en esta ruta.");
} else
{
    foreach (string sub_carpeta in carpetas)
    {
        Console.WriteLine($"> {Path.GetFileName(sub_carpeta)}");
    }
}

// --- ENLISTO ARCHIVOS + tamaño en kb ---
string[] archivos = Directory.GetFiles(path);

if (archivos.Length == 0)
{
    Console.WriteLine("No hay archivos en esta ruta.");
} else
{
    foreach (string archivo in archivos)
    {
        // FileInfo funciona por INSTANCIAS (no es una clase estatica), debo crear primero un objeto del mismo para despues acceder a sus propiedades
        FileInfo info = new FileInfo(archivo);

        string nombreArchivo = info.Name; 
        // me devuelve originalmente en bytes, por lo que lo dividimos para convertirlo a kb
        double tamanioKB = info.Length / 1024.0;

        Console.WriteLine($"> {nombreArchivo} - {tamanioKB} KB");

    }
}

// --- CREAMOS Y CARGAMOS LOS DATOS EN EL ARCHIVO 'reporte_archivos.csv' ---
string rutaArchivoCsv = Path.Combine(path, "reporte_archivos.csv");

using (StreamWriter agregar = new StreamWriter(rutaArchivoCsv))
{
    agregar.WriteLine("Nombre del Archivo,Tamanio (KB),Fecha de Ultima Modificacion");

    foreach (string archivo in archivos)
    {
        FileInfo info = new FileInfo(archivo);
        double tamanioKB = info.Length / 1024.0;
        
        agregar.WriteLine($"{info.Name},{tamanioKB:F2},{info.LastWriteTime}");
    }
}

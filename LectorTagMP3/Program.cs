using System.Text;

string? pathCancion;
bool existe = false;

do
{
    Console.Write("Ingrese el path de la cancion a localizar: ");
    // archivo con formato: test.mp3 | sin formato: little-freak.mp3
    pathCancion = Console.ReadLine();

    if (File.Exists(pathCancion))
    {
        existe = true;
    } else
    {
        Console.WriteLine("El path ingresado es incorrecto o no se encuentra. Ingrese nuevamente.");
    }

} while (!existe);

Console.WriteLine("\nLocalizacion exitosa!");
// como ya verificamos la existencia del archivo, usamos fileStream para conectarnos al mismo a traves de una instancia y extraemos + traducimos sus ultimos 128 bytes, donde sabemos por su formato (ID3v1) que se encuentran sus metadatos - comprobando esto buscando los primeros 3 de esos 128 bytes la palabra 'TAG' -, precisamente para luego mostrarlos

// creamos arreglo para guardar los ultimos 128 bytes
byte[] buffer = new byte[128];

// instanciamos 
FileStream archivo = new FileStream(pathCancion,FileMode.Open);

// paramos el puntero en la posicion -128 desde el final 
archivo.Seek(-128,SeekOrigin.End);

// leemos 128 lugares desde donde estamos paradas
int cantLeida = archivo.Read(buffer,0,128);

// si se leyó bien la cantidad de bytes, corroboramos el resto de condiciones
if (cantLeida == 128)
{
    // sacamos los primeros 3 bytes y comprobamos que sea = 'TAG'
    string? cabecera = Encoding.Latin1.GetString(buffer,0,3);

    if (cabecera == "TAG")
    {
        Console.WriteLine("Metadatos Encontrados! \n");
        Console.WriteLine($"TITULO: {Encoding.Latin1.GetString(buffer,3,30).Trim('\0', ' ')}\n");
        Console.WriteLine($"NOMBRE ARTISTA: {Encoding.Latin1.GetString(buffer,33,30).Trim('\0', ' ')}\n");
        Console.WriteLine($"ALBUM: {Encoding.Latin1.GetString(buffer,63,30).Trim('\0', ' ')}\n");
        Console.WriteLine($"ANIO: {Encoding.Latin1.GetString(buffer,93,4).Trim('\0', ' ')}\n");
    } else
    {
        Console.WriteLine("El archivo no tiene el formato ID3v1\n");
    }
}

archivo.Close();

// // ------- EXTRA: ARCHIVO CON FORMATO INTERNO ID3v1 PARA PRUEBA (realizado con ia) ---------
// // Generamos un archivo falso llamado "test.mp3" para probar el lector
// string rutaPrueba = "test.mp3";

// using (FileStream fs = new FileStream(rutaPrueba, FileMode.Create, FileAccess.Write))
// {
//     // Escribimos 1000 bytes de "ruido" simulando el audio de la canción
//     byte[] audioFalso = new byte[1000];
//     fs.Write(audioFalso, 0, audioFalso.Length);

//     // Ahora preparamos los 128 bytes finales de la etiqueta ID3v1
//     byte[] tagFalso = new byte[128];

//     // Ponemos la cabecera "TAG" en los primeros 3 bytes (ASCII: T=84, A=65, G=71)
//     tagFalso[0] = 84; 
//     tagFalso[1] = 65; 
//     tagFalso[2] = 71;

//     // Ponemos un título de prueba a partir del byte 3 (por ejemplo, "Hola")
//     byte[] bytesTitulo = Encoding.Latin1.GetBytes("Little-Freak");
//     Array.Copy(bytesTitulo, 0, tagFalso, 3, bytesTitulo.Length);

//     // artista de prueba a partir del byte 33
//     byte[] bytesArtista = Encoding.Latin1.GetBytes("Harry Styles");
//     Array.Copy(bytesArtista, 0,tagFalso, 33,bytesArtista.Length);

//     // album de prueba a partir del byte 63
//     byte[] bytesAlbum = Encoding.Latin1.GetBytes("Harry's House");
//     Array.Copy(bytesAlbum, 0,tagFalso, 63,bytesAlbum.Length);

//     // año de prueba a partir del byte 63
//     byte[] bytesAnio = Encoding.Latin1.GetBytes("2022");
//     Array.Copy(bytesAnio, 0,tagFalso, 93,bytesAnio.Length);

//     // Guardamos esos 128 bytes al final del archivo
//     fs.Write(tagFalso, 0, tagFalso.Length);
// }

// Console.WriteLine("Archivo 'test.mp3' de prueba generado con éxito.");
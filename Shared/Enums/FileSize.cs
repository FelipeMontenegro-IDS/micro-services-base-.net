
namespace Shared.Enums;

/// <summary>
/// Representa tamaños de archivo en megabytes (MB) y gigabytes (GB).
/// </summary>
/// <remarks>
/// Este enum define una lista de tamaños de archivo comunes que pueden ser utilizados
/// para especificar capacidades de almacenamiento o tamaños de archivos en una aplicación.
/// Los tamaños de archivo son importantes para gestionar el uso del espacio de almacenamiento
/// y para realizar conversiones entre diferentes unidades de medida.
/// </remarks>
public enum FileSize
{
     /// <summary>
     /// Representa 1 megabyte.
     /// </summary>
     Mb1, 
     
     /// <summary>
     /// Representa 2 megabytes.
     /// </summary>
     Mb2, 
     
     /// <summary>
     /// Representa 5 megabytes.
     /// </summary>
     Mb5,
     
     /// <summary>
     /// Representa 10 megabytes.
     /// </summary>
     Mb10,
     
     /// <summary>
     /// Representa 50 megabytes.
     /// </summary>
     Mb50,
     
     /// <summary>
     /// Representa 100 megabytes.
     /// </summary>
     Mb100,
     
     /// <summary>
     /// Representa 200 megabytes.
     /// </summary>
     Mb200, 
     
     /// <summary>
     /// Representa 500 megabytes.
     /// </summary>
     Mb500, 
     
     /// <summary>
     /// Representa 1 gigabyte.
     /// </summary>
     Gb1, 
     
     /// <summary>
     /// Representa 2 gigabytes.
     /// </summary>
     Gb2, 
     
     /// <summary>
     /// Representa 3 gigabytes.
     /// </summary>
     Gb3
}
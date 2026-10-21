using System;
using System.IO;
using System.Text;

namespace MesaPartesDigital
{
    class Archivos
    {
        const string NOMBRE_ARCHIVO = "expedientes.txt";
        const string NOMBRE_DERIVACIONES = "derivaciones.txt";

        // Ruta completa al archivo de expedientes, junto al ejecutable
        // Esto evita el problema de que el archivo se guarde en otro directorio
        static string RutaArchivo()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NOMBRE_ARCHIVO);
        }

        // Ruta del archivo de derivaciones (historial de rutas)
        static string RutaDerivaciones()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NOMBRE_DERIVACIONES);
        }

        // Escribe una línea por expediente con los 10 campos separados por '|'
        public static void GuardarExpedientes()
        {
            if (Expedientes.contador == 0)
            {
                Console.WriteLine("No hay expedientes para guardar.");
                return;
            }

            // Arreglo con las líneas a escribir
            string[] lineas = new string[Expedientes.contador];
            for (int i = 0; i < Expedientes.contador; i++)
            {
                lineas[i] = Expedientes.codigos[i] + "|" +
                            Expedientes.fechas[i] + "|" +
                            Expedientes.documentos[i] + "|" +
                            Expedientes.solicitantes[i] + "|" +
                            Expedientes.tiposTramite[i] + "|" +
                            Expedientes.periodos[i] + "|" +
                            Expedientes.ubicaciones[i] + "|" +
                            Expedientes.celulares[i] + "|" +
                            Expedientes.estados[i] + "|" +
                            Expedientes.areaActual[i];
            }

            // Usamos UTF-8 para que las tildes y la ñ se guarden bien
            File.WriteAllLines(RutaArchivo(), lineas, Encoding.UTF8);
            Console.WriteLine("Datos guardados en: " + RutaArchivo());
        }

        // Lee el archivo y llena los arreglos. Omite las líneas incompletas
        public static void CargarExpedientes()
        {
            string ruta = RutaArchivo();
            if (!File.Exists(ruta))
            {
                Console.WriteLine("Archivo no encontrado: se inicia sin expedientes.");
                Console.WriteLine("(Se creará en: " + ruta + ")");
                return;
            }

            // Leemos con UTF-8 para preservar las tildes
            string[] lineas = File.ReadAllLines(ruta, Encoding.UTF8);
            int omitidas = 0;
            Expedientes.contador = 0;

            foreach (string linea in lineas)
            {
                // Saltamos las líneas vacías
                if (linea.Trim().Length == 0) continue;

                string[] partes = linea.Split('|');
                // Aceptamos 10 campos (formato nuevo con área), 9 (con estado) u 8 (formato viejo)
                if ((partes.Length == 10 || partes.Length == 9 || partes.Length == 8)
                    && Expedientes.contador < Expedientes.CAPACIDAD)
                {
                    Expedientes.codigos[Expedientes.contador] = partes[0];
                    Expedientes.fechas[Expedientes.contador] = partes[1];
                    Expedientes.documentos[Expedientes.contador] = partes[2];
                    Expedientes.solicitantes[Expedientes.contador] = partes[3];
                    Expedientes.tiposTramite[Expedientes.contador] = partes[4];
                    Expedientes.periodos[Expedientes.contador] = partes[5];
                    Expedientes.ubicaciones[Expedientes.contador] = partes[6];
                    Expedientes.celulares[Expedientes.contador] = partes[7];

                    // Estado: 9 o 10 campos lo tienen; 8 no, asignamos INGRESADO
                    if (partes.Length >= 9)
                        Expedientes.estados[Expedientes.contador] = partes[8];
                    else
                        Expedientes.estados[Expedientes.contador] = "INGRESADO";

                    // Área: solo el formato nuevo (10 campos) lo tiene; resto: SECRETARIA
                    if (partes.Length == 10)
                        Expedientes.areaActual[Expedientes.contador] = partes[9];
                    else
                        Expedientes.areaActual[Expedientes.contador] = "SECRETARIA";

                    Expedientes.contador++;
                }
                else
                {
                    omitidas++;
                }
            }

            Console.WriteLine(Expedientes.contador + " expedientes cargados.");
            if (omitidas > 0)
            {
                Console.WriteLine(omitidas + " líneas omitidas.");
            }
        }

        // Agrega una derivación al final del archivo derivaciones.txt
        // Formato de línea: codigo|fecha|areaOrigen|areaDestino
        public static void RegistrarDerivacion(string codigo, string fecha, string areaOrigen, string areaDestino)
        {
            string ruta = RutaDerivaciones();
            // Si el archivo no existe, File.AppendAllText lo crea
            string linea = codigo + "|" + fecha + "|" + areaOrigen + "|" + areaDestino;
            File.AppendAllText(ruta, linea + Environment.NewLine, Encoding.UTF8);
        }

        // Muestra todas las derivaciones de un expediente específico
        public static void MostrarHistorial(string codigoBuscado)
        {
            string ruta = RutaDerivaciones();
            if (!File.Exists(ruta))
            {
                Console.WriteLine("No hay derivaciones registradas todavía.");
                return;
            }

            string[] lineas = File.ReadAllLines(ruta, Encoding.UTF8);
            int encontradas = 0;

            Console.WriteLine("Fecha       | De → Hacia");
            Console.WriteLine("-------------------------------");
            foreach (string linea in lineas)
            {
                if (linea.Trim().Length == 0) continue;
                string[] partes = linea.Split('|');
                if (partes.Length == 4 && partes[0] == codigoBuscado)
                {
                    Console.WriteLine(partes[1] + " | " + partes[2] + " → " + partes[3]);
                    encontradas++;
                }
            }

            if (encontradas == 0)
            {
                Console.WriteLine("(El expediente no ha sido derivado todavía.)");
            }
            else
            {
                Console.WriteLine("Total: " + encontradas + " derivación(es).");
            }
        }
    }
}

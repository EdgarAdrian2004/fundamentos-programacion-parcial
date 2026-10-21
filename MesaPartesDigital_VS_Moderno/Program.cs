using System;
// using System.Text; // descomentar si se necesitan tildes en consola

namespace MesaPartesDigital
{
    class Program
    {
        static void Main(string[] args)
        {
            // Si hay problemas con tildes en la consola, descomentar estas dos líneas:

            // Cargamos los expedientes guardados al iniciar el programa
            Archivos.CargarExpedientes();
            Console.WriteLine("\nPresione Enter para continuar...");
            Console.ReadLine();

            int opcion = 0;
            // Ciclo principal: se repite hasta elegir Salir (9)
            while (opcion != 9)
            {
                Console.Clear();
                MostrarMenu();
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        Expedientes.RegistrarExpediente();
                        break;
                    case 2:
                        Expedientes.BuscarExpediente();
                        break;
                    case 3:
                        Expedientes.BuscarPorDocumento();
                        break;
                    case 4:
                        MenuOrdenar();
                        break;
                    case 5:
                        Archivos.GuardarExpedientes();
                        break;
                    case 6:
                        Expedientes.CambiarEstado();
                        break;
                    case 7:
                        Expedientes.DerivarExpediente();
                        break;
                    case 8:
                        Expedientes.VerHistorial();
                        break;
                    case 9:
                        // Antes de salir, guardamos para no perder lo registrado
                        Archivos.GuardarExpedientes();
                        Console.WriteLine("Hasta pronto.");
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                // Pausa antes de volver al menú (excepto al salir)
                if (opcion != 9)
                {
                    Console.WriteLine("\nPresione Enter para volver al menú...");
                    Console.ReadLine();
                }
            }
        }

        // Muestra las opciones del menú principal
        static void MostrarMenu()
        {
            Console.WriteLine("===== MESA DE PARTES DIGITAL =====");
            Console.WriteLine("1. Registrar expediente");
            Console.WriteLine("2. Buscar por código");
            Console.WriteLine("3. Buscar por DNI o RUC");
            Console.WriteLine("4. Ordenar y listar expedientes");
            Console.WriteLine("5. Guardar datos en archivo");
            Console.WriteLine("6. Cambiar estado de un expediente");
            Console.WriteLine("7. Derivar expediente a otra área");
            Console.WriteLine("8. Ver historial de derivaciones");
            Console.WriteLine("9. Salir");
            Console.Write("Ingrese una opción: ");
        }

        // Submenú para elegir el criterio de ordenamiento
        static void MenuOrdenar()
        {
            Console.WriteLine("\n----- ORDENAR EXPEDIENTES -----");
            Console.WriteLine("1. Por código");
            Console.WriteLine("2. Por fecha");
            Console.WriteLine("3. Por solicitante (apellidos)");
            Console.WriteLine("4. Por documento (DNI/RUC)");
            Console.WriteLine("5. Por tipo de trámite");
            Console.WriteLine("6. Por estado");
            Console.WriteLine("7. Por área actual");
            Console.Write("Elija el criterio: ");

            string texto = Console.ReadLine();
            int criterio = 0;
            if (texto != null && texto.Length == 1 && texto[0] >= '1' && texto[0] <= '7')
            {
                criterio = texto[0] - '0';
            }

            if (criterio == 0)
            {
                Console.WriteLine("Criterio no válido.");
                return;
            }

            Expedientes.OrdenarExpedientes(criterio);
            Expedientes.ListarExpedientes();
        }

        // Lee la opción del menú. Devuelve 0 si no es válida
        static int LeerOpcion()
        {
            string texto = Console.ReadLine();
            if (texto == null) return 0;
            // Validamos que sea un solo dígito entre 1 y 9
            if (texto.Length == 1 && texto[0] >= '1' && texto[0] <= '9')
            {
                return texto[0] - '0';
            }
            return 0;
        }
    }
}

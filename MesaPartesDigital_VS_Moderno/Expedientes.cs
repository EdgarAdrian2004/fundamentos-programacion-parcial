using System;

namespace MesaPartesDigital
{
    class Expedientes
    {
        public const int CAPACIDAD = 100;
        public static int contador = 0;

        // Arreglos paralelos: la posición i en todos representa el mismo expediente
        public static string[] codigos = new string[CAPACIDAD];
        public static string[] fechas = new string[CAPACIDAD];
        public static string[] documentos = new string[CAPACIDAD];
        public static string[] solicitantes = new string[CAPACIDAD];
        public static string[] tiposTramite = new string[CAPACIDAD];
        public static string[] periodos = new string[CAPACIDAD];
        public static string[] ubicaciones = new string[CAPACIDAD];
        public static string[] celulares = new string[CAPACIDAD];
        // Arreglo para el estado del expediente
        public static string[] estados = new string[CAPACIDAD];
        // Arreglo para el área donde se encuentra actualmente el expediente
        public static string[] areaActual = new string[CAPACIDAD];

        // Estados válidos para el expediente
        static string[] estadosValidos = { "INGRESADO", "EN TRAMITE", "ATENDIDO", "OBSERVADO", "ARCHIVADO" };

        // Áreas por las que puede pasar un expediente en la I.E.
        static string[] areasValidas = {
            "SECRETARIA",
            "DIRECCION",
            "SUBDIRECCION ACADEMICA",
            "SUBDIRECCION ADMINISTRATIVA",
            "TUTORIA",
            "OFICINA DE MATRICULA",
            "ARCHIVO",
            "UGEL/EXTERNO"
        };

        // Tipos de trámite que maneja la institución educativa
        static string[] tiposTramiteValidos = {
            "SOLICITUD DE VACANTE",
            "RATIFICACION DE MATRICULA",
            "TRASLADO DE MATRICULA",
            "CERTIFICADO DE ESTUDIOS",
            "CONSTANCIA DE NOTAS",
            "OFICIO EXTERNO",
            "OTRO"
        };

        // Helper para leer un campo. Devuelve null si el usuario escribe C para cancelar
        static string LeerCampo(string prompt)
        {
            Console.Write(prompt + " (o 'C' para cancelar): ");
            string entrada = Console.ReadLine();
            if (entrada == null) return null;
            entrada = entrada.Trim();
            if (entrada.ToUpper() == "C") return null;
            return entrada;
        }

        // Muestra la lista de tipos de trámite y pide al usuario que elija uno
        // Devuelve el tipo elegido, o null si cancela
        static string LeerTipoTramite()
        {
            while (true)
            {
                Console.WriteLine("Tipos de trámite disponibles:");
                for (int i = 0; i < tiposTramiteValidos.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + tiposTramiteValidos[i]);
                }
                Console.Write("Elija una opción (1-" + tiposTramiteValidos.Length + ", o 'C' para cancelar): ");
                string texto = Console.ReadLine();
                if (texto == null) return null;
                texto = texto.Trim();
                if (texto.ToUpper() == "C") return null;

                int opcion;
                // TryParse evita que el programa falle si el usuario escribe letras
                if (int.TryParse(texto, out opcion) && opcion >= 1 && opcion <= tiposTramiteValidos.Length)
                {
                    return tiposTramiteValidos[opcion - 1];
                }
                Console.WriteLine("Opción no válida. Intente de nuevo.");
            }
        }

        // Registra un nuevo expediente. Pide y valida cada dato, permite reintentar
        public static void RegistrarExpediente()
        {
            if (contador == CAPACIDAD)
            {
                Console.WriteLine("No hay espacio para más expedientes.");
                return;
            }

            string codigo, fecha, documento, solicitante, tipoTramite, periodo, ubicacion, celular;

            // === Código ===
            while (true)
            {
                codigo = LeerCampo("Código (ej. EXP-2026-001)");
                if (codigo == null) { Console.WriteLine("Registro cancelado."); return; }
                codigo = codigo.ToUpper();
                if (!Validaciones.ValidarCodigo(codigo))
                {
                    Console.WriteLine("Código con formato inválido. Intente de nuevo.");
                    continue;
                }
                if (BuscarIndice(codigo) != -1)
                {
                    Console.WriteLine("El código ya existe. Intente de nuevo.");
                    continue;
                }
                break;
            }

            // === Fecha ===
            while (true)
            {
                fecha = LeerCampo("Fecha (ej. 20/08/2026)");
                if (fecha == null) { Console.WriteLine("Registro cancelado."); return; }
                if (Validaciones.ValidarFecha(fecha)) break;
                Console.WriteLine("Fecha inválida. Intente de nuevo.");
            }

            // === Documento (DNI o RUC) ===
            while (true)
            {
                documento = LeerCampo("DNI (8 dígitos) o RUC (11 dígitos) (ej. 70123456)");
                if (documento == null) { Console.WriteLine("Registro cancelado."); return; }
                if (Validaciones.ValidarDocumento(documento)) break;
                Console.WriteLine("DNI o RUC inválido. Intente de nuevo.");
            }

            // === Solicitante ===
            while (true)
            {
                solicitante = LeerCampo("Solicitante (ej. Quispe Rojas, Ana)");
                if (solicitante == null) { Console.WriteLine("Registro cancelado."); return; }
                solicitante = solicitante.ToUpper();
                if (Validaciones.ValidarTextoObligatorio(solicitante)) break;
                Console.WriteLine("No puede estar vacío ni contener el carácter |.");
            }

            // === Tipo de trámite (elegir de la lista) ===
            tipoTramite = LeerTipoTramite();
            if (tipoTramite == null) { Console.WriteLine("Registro cancelado."); return; }

            // === Periodo ===
            while (true)
            {
                periodo = LeerCampo("Periodo (ej. 1RO-2026)");
                if (periodo == null) { Console.WriteLine("Registro cancelado."); return; }
                if (Validaciones.ValidarTextoObligatorio(periodo)) break;
                Console.WriteLine("No puede estar vacío ni contener el carácter |.");
            }

            // === Ubicación ===
            while (true)
            {
                ubicacion = LeerCampo("Ubicación física (ej. Archivador 1º 2026)");
                if (ubicacion == null) { Console.WriteLine("Registro cancelado."); return; }
                if (Validaciones.ValidarTextoObligatorio(ubicacion)) break;
                Console.WriteLine("No puede estar vacío ni contener el carácter |.");
            }

            // === Celular ===
            while (true)
            {
                celular = LeerCampo("Celular (9 dígitos, ej. 987654321)");
                if (celular == null) { Console.WriteLine("Registro cancelado."); return; }
                if (Validaciones.ValidarCelular(celular)) break;
                Console.WriteLine("Celular inválido. Intente de nuevo.");
            }

            // El estado se asigna como INGRESADO por defecto al registrar
            string estado = "INGRESADO";
            // El área inicial siempre es SECRETARIA (donde se recepciona)
            string area = "SECRETARIA";

            // Guardamos en la posición libre y aumentamos el contador
            codigos[contador] = codigo;
            fechas[contador] = fecha;
            documentos[contador] = documento;
            solicitantes[contador] = solicitante;
            tiposTramite[contador] = tipoTramite;
            periodos[contador] = periodo;
            ubicaciones[contador] = ubicacion;
            celulares[contador] = celular;
            estados[contador] = estado;
            areaActual[contador] = area;
            contador++;

            Console.WriteLine("Expediente registrado con estado INGRESADO y área SECRETARIA.");
        }

        // Búsqueda secuencial por código. Devuelve la posición o -1 si no existe
        public static int BuscarIndice(string codigoBuscado)
        {
            codigoBuscado = codigoBuscado.Trim().ToUpper();
            for (int i = 0; i < contador; i++)
            {
                if (codigos[i] == codigoBuscado)
                {
                    return i;
                }
            }
            return -1;
        }

        // Búsqueda por DNI o RUC
        public static int BuscarIndicePorDocumento(string documentoBuscado)
        {
            documentoBuscado = documentoBuscado.Trim();
            for (int i = 0; i < contador; i++)
            {
                if (documentos[i] == documentoBuscado)
                {
                    return i;
                }
            }
            return -1;
        }

        // Pide un código y muestra el expediente. Permite cancelar y reintentar
        public static void BuscarExpediente()
        {
            while (true)
            {
                string codigo = LeerCampo("Ingrese el código (ej. EXP-2026-001)");
                if (codigo == null) { Console.WriteLine("Búsqueda cancelada."); return; }

                int indice = BuscarIndice(codigo);
                if (indice == -1)
                {
                    Console.Write("No se encontró el expediente. ¿Desea intentar con otro código? (S/N): ");
                    string r = Console.ReadLine();
                    if (r == null || r.Trim().ToUpper() != "S") return;
                    continue;
                }
                MostrarExpediente(indice);
                return;
            }
        }

        // Pide un DNI o RUC y muestra el expediente. Permite cancelar y reintentar
        public static void BuscarPorDocumento()
        {
            while (true)
            {
                string doc = LeerCampo("Ingrese el DNI o RUC (ej. 70123456)");
                if (doc == null) { Console.WriteLine("Búsqueda cancelada."); return; }

                int indice = BuscarIndicePorDocumento(doc);
                if (indice == -1)
                {
                    Console.Write("No se encontró el expediente. ¿Desea intentar con otro documento? (S/N): ");
                    string r = Console.ReadLine();
                    if (r == null || r.Trim().ToUpper() != "S") return;
                    continue;
                }
                MostrarExpediente(indice);
                return;
            }
        }

        // Muestra los 10 datos del expediente en la posición i
        public static void MostrarExpediente(int i)
        {
            Console.WriteLine("Código: " + codigos[i]);
            Console.WriteLine("Fecha: " + fechas[i]);
            Console.WriteLine("Documento: " + documentos[i]);
            Console.WriteLine("Solicitante: " + solicitantes[i]);
            Console.WriteLine("Trámite: " + tiposTramite[i]);
            Console.WriteLine("Periodo: " + periodos[i]);
            Console.WriteLine("Ubicación: " + ubicaciones[i]);
            Console.WriteLine("Celular: " + celulares[i]);
            Console.WriteLine("Estado: " + estados[i]);
            Console.WriteLine("Área actual: " + areaActual[i]);
        }

        // Ordena los expedientes según el criterio elegido
        // criterio: 1=código, 2=fecha, 3=solicitante, 4=documento, 5=tipo, 6=estado, 7=área
        public static void OrdenarExpedientes(int criterio)
        {
            if (contador == 0)
            {
                Console.WriteLine("No hay expedientes para ordenar.");
                return;
            }

            int n = contador;
            bool huboCambio = true;
            while (huboCambio)
            {
                huboCambio = false;
                for (int j = 0; j < n - 1; j++)
                {
                    bool debeIntercambiar = false;
                    // Elegimos el arreglo a comparar según el criterio
                    switch (criterio)
                    {
                        case 1:
                            debeIntercambiar = string.Compare(codigos[j], codigos[j + 1]) > 0;
                            break;
                        case 2:
                            debeIntercambiar = string.Compare(fechas[j], fechas[j + 1]) > 0;
                            break;
                        case 3:
                            debeIntercambiar = string.Compare(solicitantes[j], solicitantes[j + 1]) > 0;
                            break;
                        case 4:
                            debeIntercambiar = string.Compare(documentos[j], documentos[j + 1]) > 0;
                            break;
                        case 5:
                            debeIntercambiar = string.Compare(tiposTramite[j], tiposTramite[j + 1]) > 0;
                            break;
                        case 6:
                            debeIntercambiar = string.Compare(estados[j], estados[j + 1]) > 0;
                            break;
                        case 7:
                            debeIntercambiar = string.Compare(areaActual[j], areaActual[j + 1]) > 0;
                            break;
                    }
                    if (debeIntercambiar)
                    {
                        IntercambiarPosiciones(j, j + 1);
                        huboCambio = true;
                    }
                }
                n--;
            }
            Console.WriteLine("Expedientes ordenados.");
        }

        // Intercambia dos posiciones en los 10 arreglos a la vez
        // Para que los datos de un expediente no se separen
        static void IntercambiarPosiciones(int a, int b)
        {
            string tmp;
            tmp = codigos[a]; codigos[a] = codigos[b]; codigos[b] = tmp;
            tmp = fechas[a]; fechas[a] = fechas[b]; fechas[b] = tmp;
            tmp = documentos[a]; documentos[a] = documentos[b]; documentos[b] = tmp;
            tmp = solicitantes[a]; solicitantes[a] = solicitantes[b]; solicitantes[b] = tmp;
            tmp = tiposTramite[a]; tiposTramite[a] = tiposTramite[b]; tiposTramite[b] = tmp;
            tmp = periodos[a]; periodos[a] = periodos[b]; periodos[b] = tmp;
            tmp = ubicaciones[a]; ubicaciones[a] = ubicaciones[b]; ubicaciones[b] = tmp;
            tmp = celulares[a]; celulares[a] = celulares[b]; celulares[b] = tmp;
            tmp = estados[a]; estados[a] = estados[b]; estados[b] = tmp;
            tmp = areaActual[a]; areaActual[a] = areaActual[b]; areaActual[b] = tmp;
        }

        // Lista todos los expedientes registrados
        public static void ListarExpedientes()
        {
            if (contador == 0)
            {
                Console.WriteLine("No hay expedientes registrados.");
                return;
            }

            for (int i = 0; i < contador; i++)
            {
                MostrarExpediente(i);
                Console.WriteLine("----------------------");
            }
        }

        // Cambia el estado de un expediente buscando por código
        // Permite cancelar y reintentar en cada paso
        public static void CambiarEstado()
        {
            int indice;
            // Pedimos el código hasta encontrarlo o cancelar
            while (true)
            {
                string codigo = LeerCampo("Ingrese el código del expediente (ej. EXP-2026-001)");
                if (codigo == null) { Console.WriteLine("Operación cancelada."); return; }

                indice = BuscarIndice(codigo);
                if (indice == -1)
                {
                    Console.Write("No se encontró el expediente. ¿Desea intentar con otro código? (S/N): ");
                    string r = Console.ReadLine();
                    if (r == null || r.Trim().ToUpper() != "S") return;
                    continue;
                }
                break;
            }

            // Mostramos estado actual y opciones
            Console.WriteLine("Estado actual: " + estados[indice]);
            Console.WriteLine("Estados disponibles:");
            for (int i = 0; i < estadosValidos.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + estadosValidos[i]);
            }

            // Pedimos la opción de estado hasta que sea válida
            int opcionEstado = 0;
            while (true)
            {
                Console.Write("Elija el nuevo estado (1-5, o 'C' para cancelar): ");
                string texto = Console.ReadLine();
                if (texto == null) return;
                texto = texto.Trim();
                if (texto.ToUpper() == "C")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                if (texto.Length == 1 && texto[0] >= '1' && texto[0] <= '5')
                {
                    opcionEstado = texto[0] - '0';
                    break;
                }
                Console.WriteLine("Opción no válida. Intente de nuevo.");
            }

            estados[indice] = estadosValidos[opcionEstado - 1];
            Console.WriteLine("Estado actualizado a: " + estados[indice]);
        }

        // Deriva un expediente a otra área de la I.E.
        // Pide código, muestra área actual, permite elegir nueva área y registra la derivación
        public static void DerivarExpediente()
        {
            int indice;
            // Pedimos el código hasta encontrarlo o cancelar
            while (true)
            {
                string codigo = LeerCampo("Ingrese el código del expediente (ej. EXP-2026-001)");
                if (codigo == null) { Console.WriteLine("Operación cancelada."); return; }

                indice = BuscarIndice(codigo);
                if (indice == -1)
                {
                    Console.Write("No se encontró el expediente. ¿Desea intentar con otro código? (S/N): ");
                    string r = Console.ReadLine();
                    if (r == null || r.Trim().ToUpper() != "S") return;
                    continue;
                }
                break;
            }

            string areaOrigen = areaActual[indice];
            Console.WriteLine("Área actual: " + areaOrigen);
            Console.WriteLine("Áreas disponibles (excepto la actual):");
            int contadorOpciones = 0;
            // Mostramos todas las áreas menos la actual
            for (int i = 0; i < areasValidas.Length; i++)
            {
                if (areasValidas[i] != areaOrigen)
                {
                    contadorOpciones++;
                    Console.WriteLine(contadorOpciones + ". " + areasValidas[i]);
                }
            }

            // Pedimos la nueva área hasta que sea válida
            int opcionArea = 0;
            while (true)
            {
                Console.Write("Elija el área destino (1-" + contadorOpciones + ", o 'C' para cancelar): ");
                string texto = Console.ReadLine();
                if (texto == null) return;
                texto = texto.Trim();
                if (texto.ToUpper() == "C")
                {
                    Console.WriteLine("Operación cancelada.");
                    return;
                }
                int.TryParse(texto, out opcionArea);
                if (opcionArea >= 1 && opcionArea <= contadorOpciones)
                {
                    break;
                }
                Console.WriteLine("Opción no válida. Intente de nuevo.");
            }

            // Buscamos el nombre del área elegida (la que NO es la actual)
            string areaDestino = "";
            int contadorRecorrido = 0;
            for (int i = 0; i < areasValidas.Length; i++)
            {
                if (areasValidas[i] != areaOrigen)
                {
                    contadorRecorrido++;
                    if (contadorRecorrido == opcionArea)
                    {
                        areaDestino = areasValidas[i];
                        break;
                    }
                }
            }

            // Pedimos la fecha de la derivación
            string fechaDerivacion;
            while (true)
            {
                fechaDerivacion = LeerCampo("Fecha de derivación (ej. 25/08/2026)");
                if (fechaDerivacion == null) { Console.WriteLine("Operación cancelada."); return; }
                if (Validaciones.ValidarFecha(fechaDerivacion)) break;
                Console.WriteLine("Fecha inválida. Intente de nuevo.");
            }

            // Actualizamos el área en memoria y registramos la derivación en el archivo
            string areaOrigenVieja = areaActual[indice];
            areaActual[indice] = areaDestino;

            // Registramos la derivación en derivaciones.txt
            Archivos.RegistrarDerivacion(codigos[indice], fechaDerivacion, areaOrigenVieja, areaDestino);

            Console.WriteLine("Expediente derivado de " + areaOrigenVieja + " a " + areaDestino + ".");
        }

        // Muestra el historial de derivaciones de un expediente
        public static void VerHistorial()
        {
            string codigo = LeerCampo("Ingrese el código del expediente (ej. EXP-2026-001)");
            if (codigo == null) { Console.WriteLine("Operación cancelada."); return; }

            // Buscamos el expediente para mostrar datos básicos
            int indice = BuscarIndice(codigo);
            if (indice != -1)
            {
                Console.WriteLine("Expediente: " + codigos[indice] + " - " + solicitantes[indice]);
                Console.WriteLine("Área actual: " + areaActual[indice]);
                Console.WriteLine("----- Historial de derivaciones -----");
            }
            else
            {
                Console.WriteLine("(El expediente no está cargado en memoria, pero se buscará en el historial.)");
                Console.WriteLine("----- Historial de derivaciones -----");
            }

            // Leemos el archivo de derivaciones y mostramos las que coinciden con el código
            Archivos.MostrarHistorial(codigo.Trim().ToUpper());
        }
    }
}

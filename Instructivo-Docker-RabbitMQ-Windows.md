# 🐳 Instructivo: Instalación de Docker + RabbitMQ en Windows

## 🧰 Requisitos previos

Antes de comenzar, asegurarse de tener:

* Una PC con Windows 10 o superior
* Acceso a internet
* Permisos de administrador

## 🔹 Paso 1: Instalar Docker Desktop

1. Ir al sitio oficial de Docker: 👉 https://www.docker.com/products/docker-desktop
2. Descargar Docker Desktop para Windows.
3. Ejecutar el instalador y seguir los pasos. Asegurarse de habilitar la opción "Use WSL 2 instead of Hyper-V" si está disponible.
4. Reiniciar la computadora si el instalador lo solicita.
5. Verificar que Docker esté instalado correctamente: Abrir una terminal (PowerShell o CMD) y ejecutar:

```
docker --version
```

Si ves algo como `Docker version 24.0.5, build abc123`, ¡todo está listo!

## 🔹 Paso 2: Descargar la imagen de RabbitMQ

RabbitMQ es un sistema de mensajería que permite que distintas aplicaciones se comuniquen entre sí.

En la terminal, ejecutar:

```bash
docker pull rabbitmq:3-management
```

Esto descargará la imagen oficial de RabbitMQ con la interfaz de administración web incluida.

## 🔹 Paso 3: Ejecutar el contenedor de RabbitMQ

Ejecutar el siguiente comando en la terminal:

```bash
docker run -d -p 15672:15672 -p 5672:5672 --name rabbit-test rabbitmq:3-management
```

* `-d`: ejecuta el contenedor en segundo plano
* `-p`: expone los puertos necesarios
  * `15672`: acceso al panel web
  * `5672`: puerto de comunicación para aplicaciones
* `--name rabbit-test`: nombre del contenedor

## 🔹 Paso 4: Acceder al panel de administración

1. Abrir el navegador y visitar: 👉 http://localhost:15672/#/
2. Iniciar sesión con las credenciales por defecto:
   * Usuario: `guest`
   * Contraseña: `guest`

### ✅ Verificación final

Una vez dentro del panel, deberías ver el dashboard de RabbitMQ con estadísticas, colas, exchanges y más. ¡Ya estás listo para comenzar a trabajar con mensajería!

## 🔹 Paso 5: Probar la demo (`Demo_RabbitMQ`)

Con el contenedor de RabbitMQ corriendo, se puede usar la demo incluida en [`Demo_RabbitMQ/`](./Demo_RabbitMQ) para corroborar el funcionamiento de punta a punta. Contiene dos aplicaciones de consola en .NET 6:

* **`GeneradorTrafico`**: pide una "patente" por consola y la publica (`BasicPublish`) en la cola `GeneradorTrafico` del exchange por defecto.
* **`ServiceBus`**: se suscribe (`BasicConsume`) a la misma cola `GeneradorTrafico` y muestra por consola cada mensaje recibido.

Ambas se conectan a `localhost` (host y puertos por defecto de AMQP, `5672`), que es exactamente lo que expone el contenedor `rabbit-test` del Paso 3.

Para probarla (en dos terminales distintas, con el contenedor de RabbitMQ ya corriendo):

```bash
# Terminal 1 - consumidor
cd Demo_RabbitMQ/ServiceBus/ServiceBus
dotnet run

# Terminal 2 - productor
cd Demo_RabbitMQ/GeneradorTrafico/GeneradorTrafico
dotnet run
```

Al ingresar una patente en `GeneradorTrafico` (por ejemplo `AB123CD`), debería aparecer inmediatamente en la consola de `ServiceBus` como `[X] Recibido AB123CD`. También puede verse la cola `GeneradorTrafico` (y el mensaje pasando por ella) desde el panel web en http://localhost:15672/#/queues.

### 📦 Paquetes NuGet utilizados

Ambos proyectos (`GeneradorTrafico` y `ServiceBus`) usan el mismo y único paquete NuGet de terceros para hablar con RabbitMQ:

| Paquete | Versión | Uso |
|---|---|---|
| [`RabbitMQ.Client`](https://www.nuget.org/packages/RabbitMQ.Client) | `6.4.0` | Cliente oficial de AMQP 0-9-1 para .NET. Provee `ConnectionFactory`, `IModel` (canal), `QueueDeclare`, `BasicPublish`, `BasicConsume` y `EventingBasicConsumer` (namespace `RabbitMQ.Client.Events`), que son las clases que usan `GeneradorTrafico/Program.cs` y `ServiceBus/Program.cs`. |

Se instala/restaura con:

```bash
dotnet add package RabbitMQ.Client --version 6.4.0
```

o simplemente con `dotnet restore` una vez clonado el repositorio, ya que la referencia ya está declarada en ambos `.csproj`.

Como dependencias transitivas (se resuelven solas al restaurar, no requieren instalación manual), `RabbitMQ.Client 6.4.0` trae:

* `System.Memory` `4.5.4`
* `System.Threading.Channels` `4.7.1`

### 🧪 Verificación realizada

Se corroboró el funcionamiento de la demo contra un broker RabbitMQ real (protocolo AMQP en el puerto `5672`, equivalente al expuesto por el contenedor Docker del Paso 3):

* `dotnet restore` resolvió correctamente `RabbitMQ.Client 6.4.0` (y sus dependencias transitivas) para ambos proyectos.
* `dotnet build` compiló sin errores `GeneradorTrafico` y `ServiceBus`.
* De punta a punta: al publicar una patente desde `GeneradorTrafico`, el mensaje fue recibido correctamente por el consumidor sobre la cola `GeneradorTrafico`, confirmando que el flujo productor → RabbitMQ → consumidor funciona tal como está codeado.
* El panel de administración (puerto `15672`) respondió correctamente con las credenciales por defecto `guest`/`guest`.

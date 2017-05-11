# SignalR.RequestResponse

This framework is a "Proof of Concept"-implementation of the... well, I call it "Request & Response"-pattern.

Basically a client sends a server a **Request**.
The server will process the **Request** inside a **RequestHandler**.
Once the server is done, it will send back a **Response** to the client.

This framework, like the name is already saying, is using the SignalR-framework to establish the communication between client and server. It extents the already given functionality of the framework.

Feel free to take a look at the example projects to get started.

*Since I am not very experienced and this is my first public project, feel free to give me some feedback.
I would love to improve my skills in terms of code architecture, patterns etc.*

## Get started

I've included a very small example for both the client and server part, which should get you started. You can find all the example files in the *Examples* dir. Alternatively, you can just continue reading these instructions:

## Setting up the server

Basically, you have to initialize the server first:

```
string serverUrl = "http://*:15117/";
RequestResponseServer requestResponseServer = new RequestResponseServer();
requestResponseServer.Init(new SimpleRequestHandlerFactory());
requestResponseServer.Run(serverUrl);
```

The **RequestHandlerFactory** manages which handlers can process which request.
Here is an example implementation:

```
internal class SimpleRequestHandlerFactory : IRequestHandlerFactory
{
    private readonly IRequestHandler m_testRequestHandler = new TestRequestHandler();
    
    public IRequestHandler GetHandlerFor(string typeName)
    {
        if (m_testRequestHandler.IsResponsibleFor(typeName))
        {
            return m_testRequestHandler;
        }
        return null;
    }

    public IResponselessRequestHandler GetResponselessHandlerFor(string typeName)
    {
        return null;
    }
}
```

Feel free to use dependency injection to build up the factory, it's all up to you.

## Setting up the client:

To open up a connection, there's a static **Connection**-class, you can use.

```
var connection = new SignalRConnection();
var connectionOptions = new ConnectionOptions("http://127.0.0.1:15117/signalr");
await connection.Connect(connectionOptions);
```

The connection should be established afterwards.
Additionally, to send requests to the server and receive responses, you will have to use an instance of the class **RequestReceiver**.

```
IRequestReceiver requestReceiver = new RequestReceiver(connection);
```

The created **IRequestReceiver**-instance should be a reused as long as the client is connected to the server.

In the current state of this library, the RequestReceiver **HAS** to subscribe to the static connection class.
In later stages I will try to figure out a better approach (or maybe you have a better idea?).

## Sending requests / processing requests / sending back responses

To send a request, just use the RequestReceiver instance you have created above.
The response will be returned asynchroniously when the server has processed the request.

```
var response = await requestReceiver.ReadAsync<TestResponse>(new TestRequest { Test = "Hello Server!" });
```

The request handler could look like this:

```
public class TestRequestHandler : BaseRequestHandler<TestRequest>
{
    protected override async Task<BaseResponse> ExecuteAsync(TestRequest request)
    { 
        return new TestResponse { Text = "Hello client!" };
    }
}
```

## Logging

Both the server and the client support basic logging.

### Server logging

To enable server logging, you have to initialize the server with another parameter, which is the logger.

```
requestResponseServer.Init(new SimpleRequestHandlerFactory(), myLogger);
```

*myLogger* has to be an implementation of the provided ```IServerLogger``` interface.

### Client logging

To enable client logging, you have to register a logger in the ```ConnectionOptions``` class.

```
var connectionOptions = new ConnectionOptions("http://127.0.0.1:15117/signalr", myLogger)
```

*myLogger* has to be an implementation of the provided ```IClientLogger``` interface.

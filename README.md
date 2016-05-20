#SignalR.RequestResponse

This repository is a "Proof of Concept"-implementation of the "Request-Response-RequestHandlern" pattern.

Basically a client sends a server a Request.
The server will process the Request inside a RequestHandler.
Once the server is done, it will send back a specialization of the BaseResponse-class.

This repository, like the name is already saying, is using the SignalR-framework to establish the communication between client and server. It extents the already given functionality of the framework.

Feel free to take a look at the example projects to get started.

*Since I am not very experienced and this is my first public project, feel free to give me some feedback.
I would love to improve my skills in terms of code architecture, patterns etc.*

##Get started

I've included a very small example for both client and server part, which should get you started. You can find all the example files in the *Examples* dir. Alternatively just read the following instructions:

##Setting up the server

Basically you have to start the server first:

```
string serverUrl = "http://*:15117/";
RequestResponseServer requestResponseServer = new RequestResponseServer();
requestResponseServer.Init(new SimpleRequestHandlerFactory());
requestResponseServer.Run(serverUrl);
```

The RequestHandlerFactory manages, which handler can process which request.
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

##Setting up the client:

To open up a connection, there's a static Connection-class, you can use.

```
var connectionOptions = new ConnectionOptions("http://127.0.0.1:15117/signalr");
await Connection.Connect(connectionOptions);
```

The connection should be established afterwards.
To send requests to the server and receive responses, you will have to use a new class instance of the RequestReceiver.

```
IRequestReceiver requestReceiver = new RequestReceiver();
Connection.ResponseReceived += requestReceiver.OnResponseReceived;
```

In the current state of this library, the RequestReceiver **HAS** to subscribe to the static connection class.
In later stages I will try to figure out a better approach (or maybe you have a better idea?).

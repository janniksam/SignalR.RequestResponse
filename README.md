SignalR.RequestResponse

This repository is a Proof Of Concept-implementation of the "RequestHandlern" pattern.

Basically your client sends your server a Request (which derives from BaseRequest).
The server will process the Request in a implementation of a RequestHandler.
When the server is done, it will send back a specialization of the BaseResponse-class.

This repository, like the name is already saying, is using the SignalR-communication-library to establish the communication between client and server.

Feel free to take a look at the example projects to get started.

Since I am not very experienced, feel free to give me some feedback. I really would love to improve my skills in terms of code architecture, patterns etc.

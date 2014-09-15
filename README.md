ProtoBuf.Wcf
============

What does it do?
----------------

Its a library which you can plug into your solution (no code changes required) and expose an endpoint which uses protoBuf serialization without the drawbacks that come with it. It makes the wcf anywhere between 2-20 times faster depending upon the size of the contract. Check the advantages list in the below section.

What is this Proto-Buf thingie?
-------------------------------

Proto-Buf or protocol buffers is a serialization technique developed by some guy (or team) in google, Marc Gravell (check credits section) ported google's logic into .Net and even gave a WCF serializer to use.<br/>

ProtoBuf serialization is ultra fast, ultra compact, takes less CPU, the works, however there are some limitations with that serialization which this project aims to resolve. The problems are listed below:

1) Each member to be serialized needs to be numbered, if you use inheritance there needs to be inheritance numbers as well, this causes a lot of maintainance overhead if your contract is large in size and is prone to problems.<br/>
2) The serialized result can be deserialized into the same object due to the above numbering problem, therefore it works best in an assembly sharing model when using it for WCF serializations.<br/>
3) The above point also means that if you change the datacontract at the server side, the same needs to flow into the client side before it can work, this makes it impractical to use this for anything other than small internally used services.<br/>
4) The serialization is intrusive, i.e. code (attributes) needs to be written in the code for it to work with WCF, this adds a lot of dependency on proto-buf throughout the application.<br/>

What is it?
-----------

Its a custom binding, a custom channel for WCF which basically uses proto-buf serialization to transfer data from server to client and viceversa. It solves the problem which typically arises out of using protobuf serialization in services. It improves over the standard implementation in the following ways:<br/>
1) There is no need for numbering (and maintaining) those numbers for each and every data member in a contract, not to mention the inheritance numbers.<br/>
2) This is a plug n play solution, all you have to do is paste the dll, add a couple of references in the host wcf project, do some changes in the web.config and you are ready to go, no need to code anything to make this work!<br/>
3) It also solves the problem where if the data contract changes on server side, the serialization fails on the client side, this was a major deterrent in using protoBuf as all the clients would need to update their proxy before it can be successfully consumed. This makes the management easy and makes it possible for you to give out the channel to customers as long as they use this library on client side as well.<br/>
4) The solution works seamlessly with other endpoints, i.e. you can have a basic http endpoint, a webhttp endpoint and the protoBuf endpoint all in the same service without any issues.<br/>
5) The larger your datacontract the bigger the benefit will be (checkout the wiki page for some numbers).

What is it not?
---------------

1) Its not intra-operable, works only from .net to .net although there is nothing that ties it down to .Net, a library will need to be created for other languages.<br/>

How does it work?
-----------------

1) The channel works by taking the server as the source of truth for serialization, the client when started for the first time sends a metadata request to the server (automatically) which the server responds with the metadata (also automatic) which tells the client how to serialize and deserialize its data. This meta data is cached for later use by both the server and the client between application restarts.<br/>
2) The first request is a bit slow as the additional meta data request is made for each operation contract. The rest are mind blowing faster.<br/>

Credits
-------

The .proto serialization format is a credit to Google's ingenuity.

The specific protobuf serialization .NET implementation is designed and written by Marc Gravell, developer for Stack Exchange / Stack Overflow.

https://code.google.com/p/protobuf-net/

The protoBuf implementation is under open source license of "Apache License 2.0" You can read the terms here: http://www.apache.org/licenses/LICENSE-2.0

LZ4 compression library is taken from https://lz4net.codeplex.com/ and is developed by Krashan, its original implementation is written by Yann Collet in C. The LZ4 library for .Net is protected by New BSD License (BSD) and its terms can be read here: https://lz4net.codeplex.com/license


This solution is based from the library in the link given above and is designed and created by Sumit Maingi, a developer with Tavisca Solutions.

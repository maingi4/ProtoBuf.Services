ProtoBuf.Wcf
============

What does it do?
----------------

Its a library which you can plug into your solution (no code changes required) and expose an endpoint which uses protoBuf serialization without the drawbacks that come with it. Check the advantages list in the below section.


What is it?
-----------

Its a custom binding, a custom channel for WCF which basically uses proto-buf serialization to transfer data from server to client and viceversa. It solves the problem which typically arises out of using protobuf serialization in services. It improves over the standard implementation in the following ways:<br/>
1) There is no need for numbering (and maintaining) those numbers for each and every data member in a contract, not to mention the inheritance numbers.<br/>
2) This is a plug n play solution, all you have to do is paste the dll, add a couple of references in the host wcf project, do some changes in the web.config and you are ready to code, no need to code anything to make this work!<br/>
3) It also solves the problem where if the data contract changes on server side, the serialization fails on the client side, this was a major deterrent in using protoBuf as all the clients would need to update their proxy before it can be successfully consumed. This makes the management easy and makes it possible for you to give out the channel to customers as long as they use this library on client side as well.<br/>
4) The solution works seamlessly with other endpoints, i.e. you can have a basic http endpoint, a webhttp endpoint and the protoBuf endpoint all in the same service without any issues.<br/>
5) The larger your datacontract the bigger the benefit will be (checkout the wiki page for some numbers).

What is it not?
---------------

1) Its not intra-operable, works only from .net to .net although there is nothing that ties it down to .Net, a library will need to be created for other languages.<br/>
2) It currently does not support serialization between lists and arrays which the other contract does, which means if you have a list or an array at the server side, you will need to make sure that its the same on the client side, I will attempt to solve this problem later.<br/>

How does it work?
-----------------

1) The channel works by taking the server as the source of truth for serialization, the client when started for the first time sends a metadata request to the server (automatically) which the server responds with the metadata (also automatic) which tells the client how to serialize and deserialize its data. This meta data is cached for later use by both the server and the client between application restarts.<br/>
2) The first request is a bit slow as the additional meta data request is made for each operation contract. The rest are mind blowing faster.<br/>

Credits
-------

The .proto serialization format is a credit to Google's ingenuity.

This specific .NET implementation is designed and written by Marc Gravell, developer for Stack Exchange / Stack Overflow.

https://code.google.com/p/protobuf-net/

This solution is based from the library in the link given above and is designed and created by Sumit Maingi, a developer with Tavisca Solutions.

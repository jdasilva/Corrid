# Corrid
Corrid is a .NET library to facilitate creating high-performance correlation ids for cross-process request tracing.

## Notes
Requires Visual Studio 2019 to build.

*This is experimental*

The goal of this project is to create a simple and easy to integrate solution for adding correlation IDs to any project. This will consist of a relatively small core library that does base work of managing id creation and lifetime.  This core will
be expanded by multiple integration packages to allow drop-in usage with a particular stack. 

This is based on the common concept of a correlation ID, but viewed through my own lens of what I think should be represented.  It is driven by my own experiences and ideas on what I would like to have available to me when examining the behavior of a 
distributed (or not) system.

Each unique ID represents the execution scope of a particular action which is initiated by some event.  A common example would be a HTTP request.  A unique ID is associated with the lifetime of the request.  Any action may initiate one or more child actions which will each be assigned a unique ID.  Child actions can be related to the parent either by association of IDs at the action boundry or adopting an ID that is a composite of the parent and child.

Example execution scopes:
- HTTP request
- Process execution
- Queue message
- User action (UI event)
- Timer event

Considering support for:
https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.DiagnosticSource/src/HierarchicalRequestId.md

Inspired by this article about the ASP.NET Core Trace ID generator.  I had done some previous work like this with Guids and the thought of being able to put this everywhere AND have it be super-efficient was very exciting to me.
http://www.nimaara.com/2018/10/10/generating-ids-in-csharp/
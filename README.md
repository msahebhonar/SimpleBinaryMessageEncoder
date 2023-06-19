# Simple Binary Message Encoder API
The Simple Binary Message Encoder API is a RESTful web API built using ASP.NET Core 6.0 and C#. It serves as the endpoint for the Simple Binary Message Encoder service, offering methods for encoding messages with headers and payload, as well as decoding messages to extract headers and payload. The API provides an interface for interacting with the encoding scheme, with JSON as the data interchange format for requests and responses. 

# Simple Binary Message Encoding Scheme
The Simple Binary Message Encoding Scheme (SBMES) is a method for encoding and decoding messages using binary. The binary message structure consists of a variable number of headers and a binary payload. Headers are represented as name-value pairs. The headers are encoded sequentially, with each header consisting of the length of the name, the name itself, the length of the value, and the value. The payload is appended to the headers, forming the complete binary message. 

The headers provide metadata that helps the receiver understand the type and purpose of the message. They may contain information about the communication protocol or file format. Headers may include a tag to specify the type or purpose of the message or they can include checksums to ensure the authenticity of the encoded message.
 
# Tailored Encoding Scheme
The message header and payload sections are essential components and must be included in every message. The message structure follows the JSON format.

## Message Structure:
  - The message consists of a header section and a payload section.
  - The header section includes a variable number of headers.
  - Each header consists of a name and a value.

## Constraints:
  - Each header name and value is ASCII-encoded strings.
  - The header names and values are limited to 1023 bytes each.
  - The maximum number of headers per message is 63.
  - The payload size is limited to 256 KiB.

# Design Considerations
  - **Modular architecture**: The solution is designed using a modular architecture, where different components are organized into separate projects. This approach improves code maintainability, facilitates testing, and enables independent deployment of each module. Each module encapsulates its functionality which ensures loose coupling. This design enhances the solution's scalability, as new modules can be added or modified without impacting the overall system. 
  - **Dependency Inversion:** The decision to use interfaces and utilize dependency inversion in the project was motivated by the need to enhance modularity, flexibility, and testability. By abstracting the dependency through interfaces, the classes were decoupled from the concrete implementation, promoting loose coupling and enabling easier extensibility.
  - **Dictionary for headers:** The code utilizes a dictionary data structure to store the header names and values. This choice allows for efficient lookup and retrieval of header information based on their names with a time complexity of O(1). The dictionary also ensures that each header name is unique, which aligns with the requirement that header names are name-value pairs.
  - **Exception handling:** The code incorporates exception handling to handle scenarios where the provided input violates the specified constraints. It throws appropriate exceptions when the header count, header name/value lengths, or payload size exceed the defined limits. Additionally, it verifies that the header name/value is a valid ASCII-encoded string.

# Implementation Note
While developing the Simple Binary Message Encoder API, the primary focus was on implementing the functionality of the message encoder. As a result, the API does not currently implement any authentication or authorization mechanisms. All endpoints are publicly accessible without the need for a bearer token. It's important to note that this implementation choice was made to prioritize the development of the encoder functionality and to keep the scope of the project focused.

# JSON Streaming Example

## Overview
This repository contains an example of JSON streaming over HTTP. It demonstrates how to efficiently stream JSON data between a server and clients.

## Structure
- `JsonStreaming.sln`: The solution file that holds all references to the client and server projects.
- `clients`: Contains client-side implementations for consuming the streamed JSON data.
- `contracts/JsonStreamingContracts`: Holds the contracts used for JSON streaming, including requests and responses.
- `server/JsonStreamingServer`: The server component that streams JSON data to clients.

## Getting Started
To get started with this project, clone the repository and navigate to the respective client or server directory.

### Prerequisites
- Ensure you have the necessary environment set up for .NET development.

### Running the Server
Navigate to the `server/JsonStreamingServer` directory and run the following command:

```
dotnet run
```

### Running the Clients
Navigate to the `clients` directory and follow the instructions specific to each client implementation.

## Contributing
Contributions are welcome! Feel free to open a pull request or an issue if you have suggestions or improvements.

## License
This project is licensed under the MIT License - see the [LICENSE](https://github.com/abinkowski94/json-streaming-example) file for details.

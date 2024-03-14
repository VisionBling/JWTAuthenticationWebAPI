# ASP.NET Core JWT Authentication Web API

This project provides a comprehensive example of implementing JWT (JSON Web Tokens) authentication in an ASP.NET Core Web API, including the use of refresh tokens for maintaining user sessions securely. It's designed to demonstrate best practices for securing web APIs and includes Swagger integration for easy testing and documentation.

## Give It a Star! ⭐
If you found this project helpful, give it a star to show appreciation and help other developers discover it 

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [About JWT Authentication](#about-jwt-authentication)
- [Using Refresh Tokens](#using-refresh-tokens)
- [Testing with Swagger](#testing-with-swagger)
- [Contributing](#contributing)

## Getting Started

### Prerequisites

What things you need to install the software:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- An IDE (Visual Studio, Visual Studio Code, etc.)

### Installation

A step-by-step installation guide that tell you how to get a development environment running:

1. Clone the repo
   ```sh
   git clone https://github.com/ulomaramma/JWTAuthenticationWebAPI.git

2. Restore the .NET packages
   ```sh
   dotnet restore
   
3. Start the project
   ```sh
   dotnet run

### About JWT Authentication
JWT (JSON Web Token) is an open standard (RFC 7519) that defines a compact and self-contained way for securely transmitting information between parties as a JSON object. This information can be verified and trusted because it is digitally signed.

In the context of web APIs, JWTs are used to authenticate requests made by clients. When a user logs in with their credentials, the server generates a JWT that encapsulates the user's identity and other relevant attributes. This token is then sent back to the client, which will use it to authenticate subsequent requests to the server. The server will verify the token's validity before fulfilling the request.

### Using Refresh Tokens
A refresh token is a special kind of token used to obtain a renewed access token. This is necessary because JWT access tokens are typically set to expire after a short period for security reasons. However, constantly asking users to log in again after their access token expires would not provide a good user experience. This is where refresh tokens come into play.

In this project, alongside the JWT access token, a refresh token is also issued to the client upon login. The refresh token has a longer lifespan and can be used to request new access tokens without requiring the user to re-authenticate with their credentials

### Testing with Swagger
This project is configured with Swagger to test the API endpoints easily. To access the Swagger UI, navigate to http://localhost:port/swagger in your web browser after starting the project. 


### Contributing
Your contributions are welcome! If you'd like to improve the guide, add examples, or correct any mistakes, please feel free to fork the repository and submit a pull request.  Any contributions you make are greatly appreciated.

Fork the Project

1. Create your Feature Branch (git checkout -b feature/AmazingFeature)
2. Commit your Changes (git commit -m 'Add some AmazingFeature')
3. Push to the Branch (git push origin feature/AmazingFeature)
4. Open a Pull Request

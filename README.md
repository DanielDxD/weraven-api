# WeRaven - An open-source social media
WeRaven is a modern social network also based on other nostalgic social networks (Orkut, MSN, etc). This project is a WebAPI written in C# .NET 6

To run the program, follow the steps below :
- Go to project folder (The root of this repository is just a solution)
~~~bash
$ cd WeRaven
~~~
- Run docker containers 
~~~bash
$ docker-compose up -d
~~~
- Update database
~~~bash
$ dotnet ef database update
~~~
- Run application
~~~bash
$ dotnet run
~~~
## Requirements:
- .NET 7
- .NET EF Tools
- Docker
# ToDoTnet
Personal ToDo-list with web-api and persistence in backend database


## Prerequisites
Either docker or dotnet or both.

## Get it started

__1: with docker__
```
git clone https://github.com/zaker/ToDoTnet.git
cd ToDoTnet
docker build -t todotnet .
docker run -it -p 5000:5000 todotnet
```
__2: with dotnet__
```
git clone https://github.com/zaker/ToDoTnet.git
cd ToDoTnet
dotnet restore
dotnet ef database update
dotnet run
```


## api

### User
- Register
- Login
- Logout
- Sudo

### ToDo
- Create
- Read
- Get All
- Update
- Delete

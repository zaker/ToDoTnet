FROM microsoft/dotnet
WORKDIR /dotnetapp

# copy project.json and restore as distinct layers
COPY project.json .
RUN dotnet restore


# copy and build everything else
COPY . .

RUN dotnet publish -c Release -o out
RUN dotnet ef database update


RUN ls -la out
RUN ls -lars bin/Release/netcoreapp1.1
RUN cp bin/Debug/netcoreapp1.1/todo.db /dotnetapp/out/todo.db
RUN stat out/todo.db


EXPOSE 5000/tcp

ENTRYPOINT ["dotnet", "out/dotnetapp.dll"]

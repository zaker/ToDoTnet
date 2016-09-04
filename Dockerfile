FROM microsoft/dotnet:latest
COPY . /root/
EXPOSE 5000/tcp
WORKDIR /root/
ENTRYPOINT dotnet restore && dotnet ef database update && dotnet run 
#ENTRYPOINT pwd
#CMD ls

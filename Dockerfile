FROM microsoft/dotnet

ENV NUGET_XMLDOC_MODE skip
ARG CLRDBG_VERSION=VS2015U2
WORKDIR /clrdbg
RUN curl -SL https://raw.githubusercontent.com/Microsoft/MIEngine/getclrdbg-release/scripts/GetClrDbg.sh --output GetClrDbg.sh \
    && chmod 700 GetClrDbg.sh \
    && ./GetClrDbg.sh $CLRDBG_VERSION \
    && rm GetClrDbg.sh


WORKDIR /dotnetapp

# copy project.json and restore as distinct layers
COPY project.json .
RUN dotnet restore


# copy and build everything else
COPY . .

RUN dotnet publish -c Debug -o out
RUN dotnet ef database update

RUN cp bin/Debug/netcoreapp1.1/todo.db /dotnetapp/out/todo.db
RUN stat out/todo.db


EXPOSE 5000/tcp

ENTRYPOINT ["dotnet", "out/dotnetapp.dll"]

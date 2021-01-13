# ---- dotnet build stage ----
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

WORKDIR /build/

COPY ./MonitoringService/MonitoringService.csproj ./MonitoringService.csproj
RUN dotnet nuget add source https://ci.appveyor.com/nuget/docker-dotnet-hojfmn6hoed7 && \
    dotnet restore ./MonitoringService.csproj

COPY ./MonitoringService ./

RUN dotnet build -c ${BUILDCONFIG} -o out && dotnet publish ./MonitoringService.csproj -c ${BUILDCONFIG} -o out /p:Version=${VERSION}

# ---- final stage ----

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ENV DOTNET_PROGRAM_HOME=/opt/MonitoringService

COPY --from=build /build/out ${DOTNET_PROGRAM_HOME}

# Copy necessary scripts + configuration
COPY scripts /tmp/
RUN chmod +x /tmp/*.sh && \
    mv /tmp/* /usr/bin && \
    rm -rf /tmp/*

CMD [ "docker-entrypoint.sh" ]
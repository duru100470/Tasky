# Tasky

<div align=center>

![Static Badge](https://img.shields.io/badge/License-MIT-yellow)
[![Static Badge](https://img.shields.io/badge/release-v0.1.2-blue)](https://github.com/duru100470/Tasky/releases)

</div>

프로젝트 매니징 디스코드 봇

## Requirement

- <a href="https://discord.com/developers/applications">Your own discord bot</a>
- <a href="https://dotnet.microsoft.com/ko-kr/download/dotnet/8.0">.NET 8.0</a> for development
- <a href="https://www.docker.com/">Docker</a> for production

## Installation

1. 프로젝트 클론 후 ./src/bin/Debug/net8.0/에 appsettings.json 생성

appsettings.json:

```json
{
  "Environments": {
    "Token": "Your bot token",
    "TestServer": "Your test discord server id",
    "IsDebug": "true"
  }
}
```

2. 실행

도커 사용 시:

```bash
$ docker compose build
$ docker compose up
```

dotnet 사용 시:

```bash
$ dotnet watch run --project ./src/
```

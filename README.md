## Grafana

## Start

```bash
docker-compose up
```

## Services

- Grafana http://localhost:3000
- InfluxDB http://locahost:8086
- Chronograf http://localhost:8888

## Client

```bash
brew install telegraf
```

## Collect data

```
telegraf -test -config telegraf/telegraf.conf
telegraf -config telegraf/telegraf.conf
```
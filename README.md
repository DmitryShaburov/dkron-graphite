# dkron-graphite
Dkron jobs metrics sender for graphite  
Written @ https://www.sravni.ru

## Metrics
Writes into graphite four metrics for each job in Dkron:
* state: 0 for failed, 1 for successful last execution
* last_duration: duration of last successful execution
* success_count: number of successful executions
* error_count: number of failed executions

## Usage
`dkron-graphite.exe --url=http://dkron.domain.local:8080 --host=graphite.domain.local --prefix=apps.graphite`

## Example Dkron job
```
{
  "name": "dkron_graphite",
  "command": "c:/etc/dkron-graphite/dkron-graphite.exe --url=http://dkron.domain.local:8080 --host=graphite.domain.local --prefix=apps.dkron",
  "schedule": "@every 20s",
  "concurrency": "allow",
  "tags": {
    "role": "mon:1"
  }
}
```

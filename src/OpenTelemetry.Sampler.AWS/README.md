# AWS X-Ray Remote Sampler

[![NuGet](https://img.shields.io/nuget/v/OpenTelemetry.Sampler.AWS.svg)](https://www.nuget.org/packages/OpenTelemetry.Sampler.AWS)
[![NuGet](https://img.shields.io/nuget/dt/OpenTelemetry.Sampler.AWS.svg)](https://www.nuget.org/packages/OpenTelemetry.Sampler.AWS)

This package provides a sampler which can get sampling
configurations from AWS X-Ray to make sampling decisions.
See: [AWS X-Ray Sampling](https://docs.aws.amazon.com/xray/latest/devguide/xray-concepts.html#xray-concepts-sampling)

Start with installing the package

```shell
dotnet add package OpenTelemetry.Sampler.AWS
```

You can configure the `AWSXRayRemoteSampler` as per the following example.
Note that you will need to configure your [OpenTelemetry Collector for
X-Ray remote sampling](https://aws-otel.github.io/docs/getting-started/remote-sampling).
This example also sets up the Console Exporter,
which requires adding the package [`OpenTelemetry.Exporter.Console`](https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Console/README.md)
to the application.

```csharp
using OpenTelemetry;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Resources;
using OpenTelemetry.Contrib.Extensions.AWSXRay.Trace;
using OpenTelemetry.Sampler.AWS;
using OpenTelemetry.Trace;

var serviceName = "MyServiceName";

var resourceBuilder = ResourceBuilder
    .CreateDefault()
    .AddService(serviceName: serviceName)
    .AddDetector(new AWSEC2ResourceDetector());

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(serviceName)
    .SetResourceBuilder(resourceBuilder)
    .AddConsoleExporter()
    .SetSampler(AWSXRayRemoteSampler.Builder(resourceBuilder.Build()) // you must provide a resource
        .SetPollingInterval(TimeSpan.FromSeconds(5))
        .SetEndpoint("http://localhost:2000")
        .Build())
    .Build();
```

## References

- [OpenTelemetry Project](https://opentelemetry.io/)
- [AWS Distro for OpenTelemetry .NET](https://aws-otel.github.io/docs/getting-started/dotnet-sdk)

# Application Insights Queries for Feature Flag Cost Analysis

## Feature Flag Usage Summary
```kusto
customEvents
| where name == "FeatureFlagUsage"
| summarize 
    TotalExecutions = count(),
    AvgExecutionTimeMs = avg(todouble(customMeasurements.ExecutionTimeMs)),
    AvgMemoryUsedKB = avg(todouble(customMeasurements.MemoryUsedKB)),
    MaxExecutionTimeMs = max(todouble(customMeasurements.ExecutionTimeMs))
    by tostring(customDimensions.FeatureName), tostring(customDimensions.Implementation)
| order by TotalExecutions desc
```

## Performance Comparison: Optimized vs Legacy
```kusto
customEvents
| where name == "FeatureFlagUsage"
| extend 
    FeatureName = tostring(customDimensions.FeatureName),
    Implementation = tostring(customDimensions.Implementation),
    ExecutionTimeMs = todouble(customMeasurements.ExecutionTimeMs),
    MemoryUsedKB = todouble(customMeasurements.MemoryUsedKB)
| summarize 
    AvgExecutionTime = avg(ExecutionTimeMs),
    AvgMemory = avg(MemoryUsedKB),
    Count = count()
    by FeatureName, Implementation
| extend PerformanceGain = case(
    Implementation == "Optimized", 
    round(100 * (1 - AvgExecutionTime / (customEvents | where customDimensions.FeatureName == FeatureName and customDimensions.Implementation == "Legacy" | summarize avg(todouble(customMeasurements.ExecutionTimeMs)))), 2),
    0
  )
```

## Memory Usage Analysis
```kusto
customEvents
| where name == "FeatureFlagUsage"
| extend 
    FeatureName = tostring(customDimensions.FeatureName),
    Implementation = tostring(customDimensions.Implementation),
    MemoryUsedBytes = todouble(customMeasurements.MemoryUsedBytes)
| summarize 
    TotalMemoryUsedMB = sum(MemoryUsedBytes) / 1024 / 1024,
    AvgMemoryUsedKB = avg(MemoryUsedBytes) / 1024,
    Count = count()
    by FeatureName, Implementation
| order by TotalMemoryUsedMB desc
```

## Cost Impact Estimation
```kusto
customEvents
| where name == "CostImpact"
| extend 
    FeatureName = tostring(customDimensions.FeatureName),
    EstimatedCostUSD = todouble(customMeasurements.EstimatedCostUSD)
| summarize 
    TotalEstimatedCostUSD = sum(EstimatedCostUSD),
    AvgCostPerOperation = avg(EstimatedCostUSD),
    Count = count()
    by FeatureName
| order by TotalEstimatedCostUSD desc
```

## Operation Performance Tracking
```kusto
customEvents
| where name == "OperationPerformance"
| extend 
    OperationName = tostring(customDimensions.OperationName),
    Success = tostring(customDimensions.Success),
    ExecutionTimeMs = todouble(customMeasurements.ExecutionTimeMs)
| summarize 
    AvgExecutionTime = avg(ExecutionTimeMs),
    MaxExecutionTime = max(ExecutionTimeMs),
    MinExecutionTime = min(ExecutionTimeMs),
    SuccessCount = countif(Success == "True"),
    FailureCount = countif(Success == "False"),
    TotalCount = count()
    by OperationName
| extend SuccessRate = round(100.0 * SuccessCount / TotalCount, 2)
| order by AvgExecutionTime desc
```

## Feature Flag Impact on Cost Over Time
```kusto
customEvents
| where name == "FeatureFlagUsage"
| extend 
    FeatureName = tostring(customDimensions.FeatureName),
    Implementation = tostring(customDimensions.Implementation),
    ExecutionTimeMs = todouble(customMeasurements.ExecutionTimeMs)
| summarize 
    AvgExecutionTime = avg(ExecutionTimeMs),
    Count = count()
    by bin(timestamp, 1h), FeatureName, Implementation
| order by timestamp desc
```

## Daily Cost Summary
```kusto
customEvents
| where name == "CostImpact"
| extend 
    EstimatedCostUSD = todouble(customMeasurements.EstimatedCostUSD)
| summarize 
    DailyCostUSD = sum(EstimatedCostUSD),
    OperationCount = count()
    by bin(timestamp, 1d)
| order by timestamp desc
```

## Feature Flag Adoption Rate
```kusto
customEvents
| where name == "FeatureFlagUsage"
| extend Implementation = tostring(customDimensions.Implementation)
| summarize 
    OptimizedCount = countif(Implementation == "Optimized"),
    LegacyCount = countif(Implementation == "Legacy"),
    TotalCount = count()
| extend AdoptionRate = round(100.0 * OptimizedCount / TotalCount, 2)
```

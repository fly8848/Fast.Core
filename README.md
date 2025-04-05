# Fast.Core

一个核心工具包，提供通用功能和扩展方法，用于加速.NET应用程序的开发。

## 安装

```
dotnet add package Fast.Core
```

## 单元测试

[遵循最佳实践](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

遵循AAA模式(Arrange-Act-Assert)

## 自动发布

此项目配置了GitHub Actions自动化工作流，当以下情况发生时会自动构建、测试并发布NuGet包：

1. 推送到main分支时，会构建并发布到GitHub Packages
2. 创建以"v"开头的标签时（如v1.0.0），会构建并发布到NuGet.org

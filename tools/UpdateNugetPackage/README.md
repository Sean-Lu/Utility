## 使用示例

> 1.批处理命令（推荐）
>
> `注：相对路径需要根据实际情况做修改。`

- 示例1：
```
"..\..\..\..\tools\UpdateNugetPackage\bin\Debug\UpdateNugetPackage.exe" -dir "." -id "Sean.Utility" -version 2.0.0-beta2019052701
```
- 示例2：
```
"..\..\..\..\tools\UpdateNugetPackage\bin\Debug\UpdateNugetPackage.exe" -dir "." -id "Sean.Utility"
```

> 2.后期生成事件命令行（请勿使用这种方式，因为执行顺序有问题）

```
echo --Execute UpdateNugetPackage.exe file.
if not exist "$(SolutionDir)\tools\UpdateNugetPackage\UpdateNugetPackage.exe" exit
"$(SolutionDir)\tools\UpdateNugetPackage\UpdateNugetPackage.exe" -id $(ProjectName)
echo --Execute UpdateNugetPackage.exe Done.
```
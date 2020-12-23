## 导出到 `Excel` 示例

```
var list = new List<CheckInLogs>();
// do something
// ...
```

> 泛型数据 `IList<T>` 直接导出到 `Excel`

```
var excelSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\1.xlsx");
ExcelHelper.ToExcel(list, excelSavePath, "test", true, new CellStyle
{
    Border = true,
    TitleFontBold = true
}, 2);
```

> 先转换为 `DataTable` ，再导出到 `Excel`

```
var excelFileSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Files\1.xlsx");
var table = ModelConvert<CheckInLogs>.ToDataTable(list);
ExcelHelper.ToExcel(table, excelFileSavePath, true, null, 2);
```

## `.NET Core` > `HttpContextExt`使用说明

- 默认情况下如果在MVC项目中直接调用`UseHttpContextExt()`即可：
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
	app.UseHttpContextExt();
	...
}
```
- 在没有注入`HttpContextAccessor`的项目中，还需在`ConfigureServices`方法中手动注入：
```
services.AddHttpContextAccessor();
```
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
### Setup

### Visual Studio Plugins
* [Resharper](https://www.jetbrains.com/resharper/)
* [SpecFlow for Visual Studio 2015](https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowforVisualStudio2015) 
* [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.MarkdownEditor)

### Run Migrations against test database

```powershellz
Update-Database -ProjectName FTF.Storage.EntityFramework -ConnectionStringName FTF.Tests
```
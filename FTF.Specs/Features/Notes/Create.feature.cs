﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.1.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace FTF.Specs.Features.Notes
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.1.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("Create Note")]
    public partial class CreateNoteFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Create.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Create Note", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void FeatureBackground()
        {
#line 3
#line 4
 testRunner.Given("today is \'Feb 20 1984\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 5
 testRunner.And("I signup and signin as \'orodriguez\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Simple")]
        public virtual void Simple()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Simple", ((string[])(null)));
#line 7
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 8
 testRunner.Given("I created the note number 101 with text \'I was born\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I retrieve the note number 101", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Field",
                        "Value"});
            table1.AddRow(new string[] {
                        "Text",
                        "I was born"});
            table1.AddRow(new string[] {
                        "CreationDate",
                        "Feb 20 1984"});
            table1.AddRow(new string[] {
                        "UserName",
                        "orodriguez"});
#line 10
 testRunner.Then("the note should match:", ((string)(null)), table1, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("With Tags")]
        public virtual void WithTags()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("With Tags", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 17
 testRunner.Given("I created the note number 101 with text \'#Buy cheese at #SuperMarket\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 18
 testRunner.When("I retrieve the note number 101", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Name"});
            table2.AddRow(new string[] {
                        "Buy"});
            table2.AddRow(new string[] {
                        "SuperMarket"});
#line 19
 testRunner.Then("the note should contain the tags:", ((string)(null)), table2, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Empty Text")]
        public virtual void EmptyText()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Empty Text", ((string[])(null)));
#line 24
this.ScenarioSetup(scenarioInfo);
#line 3
this.FeatureBackground();
#line 25
 testRunner.When("I create a note with text \'\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 26
 testRunner.Then("it should show the error \'Text can not be empty\'", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion

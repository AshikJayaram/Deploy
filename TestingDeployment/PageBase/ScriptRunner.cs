namespace TestingDeployment.PageBase
{
    public static class ScriptRunner
    {
        public static void RunScript(this FluentAutomation.Interfaces.INativeActionSyntaxProvider I, string scriptCode)
        {
            // ReSharper disable UnusedVariable
            var element = I.Find("body").Invoke() as FluentAutomation.Element;
            // ReSharper restore UnusedVariable
        }

        public static void RunScript(this FluentAutomation.Interfaces.INativeActionSyntaxProvider I, string comboName, string itemText)
        {
            string scriptStr = "var i = {0}.FindItemByText('{1}');{0}.SetValue(i.value);";
            string script = string.Format(scriptStr, comboName, itemText);
            I.RunScript(script);
        }
    }
}

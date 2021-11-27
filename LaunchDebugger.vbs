' Get parameters from caller, normally msbuild after project publish
exelocation = Wscript.Arguments(0)
startfilelocation = Wscript.Arguments(1)

' Creates an instance of the Visual Studio IDE.
Set dte = CreateObject("VisualStudio.DTE")

' Make it visible and keep it open after we finish this script.
dte.MainWindow.Visible = True
dte.UserControl = True

' Open exe and start up file.
dte.ExecuteCommand "File.OpenProject", exelocation
dte.ItemOperations.OpenFile startfilelocation
dte.ActiveWindow.Activate()
dte.MainWindow.Activate()

' Sleep to ensure visual studio has time to load.
WScript.Sleep 100

dte.ActiveWindow.Activate()
dte.MainWindow.Activate()

' Find and select main function, ideally BreakPoints.Add("Main") would add
' a function breakpoint but I can't get it to work.
Set objFind = dte.ActiveDocument.Object("TextDocument").DTE.Find
objFind.FindWhat = "Main("
objFind.Execute()

' Insert breakpoint at selection and start debugging.
dte.ExecuteCommand "Debug.ToggleBreakPoint"
dte.ExecuteCommand "Debug.Start"

dte.ActiveWindow.Activate()
dte.MainWindow.Activate()
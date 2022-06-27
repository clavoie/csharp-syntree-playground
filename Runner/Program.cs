// See https://aka.ms/new-console-template for more information

using ExternalProject;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

Console.WriteLine("Hello, World!");

var method = typeof(UserMessageSender).GetMethod(nameof(UserMessageSender.Send));
var methodBody = method.GetMethodBody();

foreach (var i in methodBody.LocalVariables)
{
    Console.WriteLine(i.LocalType);
}

// Attempt to set the version of MSBuild.
// var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
// foreach (VisualStudioInstance instance in visualStudioInstances)
// {
//     Console.WriteLine(instance.MSBuildPath);
// }
// var instance = visualStudioInstances.Length == 1
//     // If there is only one instance of MSBuild on this machine, set that as the one to use.
//     ? visualStudioInstances[0]
//     // Handle selecting the version of MSBuild you want to use.
//     : SelectVisualStudioInstance(visualStudioInstances);
//
// Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");
//
// // NOTE: Be sure to register an instance with the MSBuildLocator 
// //       before calling MSBuildWorkspace.Create()
// //       otherwise, MSBuildWorkspace won't MEF compose.
// MSBuildLocator.RegisterInstance(instance);
MSBuildLocator.RegisterDefaults();


using var workspace = MSBuildWorkspace.Create();
workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);
var solution = await workspace.OpenSolutionAsync(@"C:\Users\Chris\RiderProjects\BendMessageDependencyTest\BendMessageDependencyTest.sln", new ConsoleProgressReporter());

foreach (var project in solution.Projects)
{
    if (project.Name != "ExternalProject")
    {
        continue;
    }
    

    foreach (Document document in project.Documents)
    {
        if (document.Name.Contains(nameof(UserMessageSender)) == false)
        {
            continue;
        }

        var model = await document.GetSemanticModelAsync();
        var root = await document.GetSyntaxRootAsync();
        var meths = root.DescendantNodes().OfType<ObjectCreationExpressionSyntax>();

        foreach (var objc in meths.Skip(1))
        {
            MethodDeclarationSyntax methodDeclarationSyntax = null;

            for (var parent = objc.Parent; parent != null; parent = parent.Parent)
            {
                if (parent is MethodDeclarationSyntax)
                {
                    methodDeclarationSyntax = (MethodDeclarationSyntax) parent;
                    break;
                }
            }

            if (methodDeclarationSyntax != null)
            {
                //methodDeclarationSyntax.Identifier
                var declaredSymbol = model.GetDeclaredSymbol(methodDeclarationSyntax);
                int j = 78;
            }

            var semanticType = model.GetTypeInfo(objc);
            // ISymbol? declaredType = model.GetDeclaredSymbol(objc);
            int i = 4;
        }

        if (root != null)
        {
            
        }
    }
}


 static VisualStudioInstance SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
{
    Console.WriteLine("Multiple installs of MSBuild detected please select one:");
    for (int i = 0; i < visualStudioInstances.Length; i++)
    {
        Console.WriteLine($"Instance {i + 1}");
        Console.WriteLine($"    Name: {visualStudioInstances[i].Name}");
        Console.WriteLine($"    Version: {visualStudioInstances[i].Version}");
        Console.WriteLine($"    MSBuild Path: {visualStudioInstances[i].MSBuildPath}");
    }

    while (true)
    {
        var userResponse = Console.ReadLine();
        if (int.TryParse(userResponse, out int instanceNumber) &&
            instanceNumber > 0 &&
            instanceNumber <= visualStudioInstances.Length)
        {
            return visualStudioInstances[instanceNumber - 1];
        }
        Console.WriteLine("Input not accepted, try again.");
    }
}

class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
{
    public void Report(ProjectLoadProgress loadProgress)
    {
        var projectDisplay = Path.GetFileName(loadProgress.FilePath);
        if (loadProgress.TargetFramework != null)
        {
            projectDisplay += $" ({loadProgress.TargetFramework})";
        }

        Console.WriteLine($"{loadProgress.Operation,-15} {loadProgress.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
    }
}

class NewInstanceWalker : CSharpSyntaxWalker
{
    // pu
}
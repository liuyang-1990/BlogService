///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Unit-Test");
var configuration = Argument("configuration", "Release");
var solution=Argument("solution","./blog.sln"); 
///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Task("Clean").Does(()=>{
    CleanDirectories("./src/*/bin");
    CleanDirectories("./test/*/bin");
});


Task("Restore-Packages")
   .IsDependentOn("Clean")
   .Does(()=>{
      DotNetCoreRestore(solution);
});


Task("Build")
   .IsDependentOn("Restore-Packages")
   .Does(()=>{
     DotNetCoreBuild(solution, new MSBuildSettings{
          NoRestore = true,
          Configuration = "Release"
     });
});

 Task("Unit-Test")
   .IsDependentOn("Build")
   .Does(()=>{
      DotNetCoreTest(solution);
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
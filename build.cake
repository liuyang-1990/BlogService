 #addin nuget:?package=Cake.Coverlet&version=2.3.4

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution=Argument("solution","./src/Blog.Api/Blog.Api.csproj"); 
var testProject=Argument("testProject","./test/Blog.Test/Blog.Test.csproj");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////
Task("Clean").Does(()=>{
    CleanDirectories("./src/*/bin");
    CleanDirectories("./src/*/obj");
    CleanDirectories("./test/*/bin");
    CleanDirectories("./test/*/obj");
});


Task("Restore-NuGet-Packages")
   .Does(()=>{
      DotNetCoreRestore(solution);
});

// Task("Build")
//   .DoesForEach(allProjects, (project) => 
//    { 
//         DotNetCoreBuild(project.GetDirectory().FullPath,new DotNetCoreBuildSettings(){
//             Framework = "netcoreapp2.2",
//             Configuration = configuration,
//             NoRestore = true,
//        });
//    });
Task("Build")
  .Does(() => 
   { 
        DotNetCoreBuild(solution,new DotNetCoreBuildSettings(){
            Framework = "netcoreapp2.2",
            Configuration = configuration,
            NoRestore = true,
       });
   });


 Task("Build-Unit-Test")
   .Does(()=>{
      DotNetCoreBuild(testProject,new DotNetCoreBuildSettings(){
          Framework = "netcoreapp2.2",
          Configuration = configuration
       });
});

   
 Task("Run-Unit-Test")
   .Does(()=>{
      DotNetCoreTest(testProject,
         new DotNetCoreTestSettings(){
             Framework = "netcoreapp2.2",
             Configuration = configuration,
             NoBuild = true,
             ArgumentCustomization= args => args.Append("/p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:Exclude=[xunit.*]*")
         },
         new CoverletSettings(){
               CollectCoverage = true,
               CoverletOutputFormat = CoverletOutputFormat.opencover,
               CoverletOutputDirectory = Directory(@".\coverage\"),
               CoverletOutputName = $"results"
         }
      );
});


Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore-NuGet-Packages")
  .IsDependentOn("Build")
  .IsDependentOn("Build-Unit-Test")
  .IsDependentOn("Run-Unit-Test");

RunTarget(target);
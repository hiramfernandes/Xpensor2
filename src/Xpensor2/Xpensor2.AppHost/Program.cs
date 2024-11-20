var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Xpensor2_Api>("xpensor2");

builder.Build().Run();

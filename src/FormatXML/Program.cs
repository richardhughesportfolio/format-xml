using FormatXML;

Application application = new(
    Environment.CommandLine,
    Console.In,
    Console.Out,
    Console.Error);

Environment.ExitCode = await application.Run();

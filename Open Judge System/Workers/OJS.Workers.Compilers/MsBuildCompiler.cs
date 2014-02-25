﻿namespace OJS.Workers.Compilers
{
    using System.IO;
    using System.Linq;
    using System.Text;

    using Ionic.Zip;

    using OJS.Common.Extensions;

    public class MsBuildCompiler : Compiler
    {
        private readonly string inputPath;
        private readonly string outputPath;

        public MsBuildCompiler()
        {
            this.inputPath = FileHelpers.CreateTempDirectory();
            this.outputPath = FileHelpers.CreateTempDirectory();
        }

        ~MsBuildCompiler()
        {
            if (Directory.Exists(this.inputPath))
            {
                Directory.Delete(this.inputPath, true);
            }

            if (Directory.Exists(this.outputPath))
            {
                Directory.Delete(this.outputPath, true);
            }
        }

        public override string RenameInputFile(string inputFile)
        {
            return inputFile + ".zip";
        }

        public override string ChangeOutputFileAfterCompilation(string outputFile)
        {
            var newOutputFile = Directory.GetFiles(this.outputPath).FirstOrDefault(x => x.EndsWith(".exe"));
            return newOutputFile;
        }

        public override string BuildCompilerArguments(string inputFile, string outputFile, string additionalArguments)
        {
            var arguments = new StringBuilder();

            UnzipFile(inputFile, this.inputPath);
            string solutionOrProjectFile = Directory.GetFiles(this.inputPath).FirstOrDefault(x => x.EndsWith(".sln"));
            if (string.IsNullOrWhiteSpace(solutionOrProjectFile))
            {
                solutionOrProjectFile = Directory.GetFiles(this.inputPath).FirstOrDefault(x => x.EndsWith(".csproj") || x.EndsWith(".vbproj"));
            }

            // Input file argument
            arguments.Append(string.Format("\"{0}\"", solutionOrProjectFile));
            arguments.Append(' ');

            // Settings
            arguments.Append("/t:rebuild ");
            arguments.Append("/p:Configuration=Release ");
            arguments.Append("/nologo ");

            // Output path argument
            arguments.Append(string.Format("/p:OutputPath=\"{0}\"", this.outputPath));
            arguments.Append(' ');

            // Additional compiler arguments
            arguments.Append(additionalArguments);

            return arguments.ToString().Trim();
        }

        private static void UnzipFile(string fileToUnzip, string outputDirectory)
        {
            using (ZipFile zip1 = ZipFile.Read(fileToUnzip))
            {
                foreach (ZipEntry entry in zip1)
                {
                    entry.Extract(outputDirectory, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}